Imports System.IO
Imports System.Diagnostics
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks

Public Class frmFlash

    ' Path to the external tool
    Private _toolPath As String = Path.Combine(Application.StartupPath, "esptool.exe")
    Private _downloadUrl As String = "https://github.com/espressif/esptool/releases"

    ' Property to accept COM port from Main Form
    Public Property TargetPort As String = ""

    Private Sub frmFlash_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 1. Check for Tool
        If File.Exists(_toolPath) Then
            lblToolStatus.Text = "esptool.exe found."
            lblToolStatus.ForeColor = Color.DarkGreen
            lnkDownload.Visible = False
            grpSettings.Enabled = True
            grpFirmware.Enabled = True
            btnFlash.Enabled = True
        Else
            lblToolStatus.Text = "esptool.exe NOT found!"
            lblToolStatus.ForeColor = Color.Red
            lnkDownload.Visible = True
            grpSettings.Enabled = False
            grpFirmware.Enabled = False
            btnFlash.Enabled = False
            rtbLog.AppendText("CRITICAL ERROR: esptool.exe is missing." & vbCrLf)
        End If

        ' 2. Populate Ports
        cmbPort.Items.Clear()
        cmbPort.Items.AddRange(IO.Ports.SerialPort.GetPortNames())
        If Not String.IsNullOrEmpty(TargetPort) AndAlso cmbPort.Items.Contains(TargetPort) Then
            cmbPort.SelectedItem = TargetPort
        ElseIf cmbPort.Items.Count > 0 Then
            cmbPort.SelectedIndex = 0
        End If

        ' 3. Populate Chips (Focused on ESP8266 family)
        cmbChip.Items.Clear()
        cmbChip.Items.AddRange({"auto", "esp8266", "esp8285"})
        ' Note: ESP-12 and ESP-01 are technically 'esp8266' chips. 
        ' We distinguish them by Flash Size.
        cmbChip.SelectedItem = "auto"

        ' 4. Populate Flash Modes (Standard SPI Modes)
        cmbFlashMode.Items.Clear()
        cmbFlashMode.Items.AddRange({"qio", "qout", "dio", "dout"})
        cmbFlashMode.SelectedItem = "dio" ' Safe default for most ESP8266

        ' 5. Populate Flash Sizes (Critical for ESP-01 vs ESP-12)
        cmbFlashSize.Items.Clear()
        cmbFlashSize.Items.AddRange({"detect", "512KB", "1MB", "2MB", "4MB", "8MB", "16MB"})
        cmbFlashSize.SelectedItem = "detect" ' Let esptool decide usually
    End Sub

    ' --- HELPER: HANDLE TOOL DOWNLOAD ---
    Private Sub lnkDownload_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkDownload.LinkClicked
        Process.Start(New ProcessStartInfo(_downloadUrl) With {.UseShellExecute = True})
    End Sub

    ' --- ACTION: DETECT CHIP & CONFIG ---
    Private Async Sub btnDetect_Click(sender As Object, e As EventArgs) Handles btnDetect.Click
        If String.IsNullOrEmpty(cmbPort.Text) Then Return

        btnDetect.Enabled = False
        rtbLog.Clear()
        Log(">>> Detecting Chip & Memory Details...")

        ' We run 'flash_id' because it gives us Chip Type AND Flash Size in one go
        Dim args As String = $"--port {cmbPort.Text} flash_id"
        Dim output As String = Await RunEsptoolAsync(args)

        ' 1. Identify Chip Family
        If output.Contains("ESP8285") Then
            SelectChip("esp8285")
            Log(">>> Chip: ESP8285 (Embedded Flash detected)")

            ' ESP8285 MUST use DOUT mode
            SelectFlashMode("dout")
            Log(">>> Auto-Setting Flash Mode to DOUT (Required for 8285)")

        ElseIf output.Contains("ESP8266") Then
            SelectChip("esp8266")
            Log(">>> Chip: ESP8266EX (Generic/ESP-12/ESP-01)")

            ' Default ESP8266 to DIO (Safe) or QIO (Faster, if supported)
            ' If it was already set manually, we might leave it, but DIO is safest default.
            If cmbFlashMode.SelectedItem.ToString() = "dout" Then
                ' If user had dout selected (maybe from previous 8285), switch back to dio
                SelectFlashMode("dio")
            End If
        Else
            Log(">>> Chip flavor not explicitly recognized (Standard Generic selected)")
            SelectChip("auto")
        End If

        ' 2. Identify Flash Size (To distinguish ESP-01 vs ESP-12)
        ' Output example: "Detected flash size: 4MB"
        Dim sizeMatch As Match = Regex.Match(output, "Detected flash size: (\d+MB|\d+KB)")
        If sizeMatch.Success Then
            Dim detectedSize As String = sizeMatch.Groups(1).Value
            Log($">>> Detected Flash Size: {detectedSize}")

            ' Auto-select in dropdown if it matches
            If cmbFlashSize.Items.Contains(detectedSize) Then
                cmbFlashSize.SelectedItem = detectedSize
            End If

            ' heuristic for module type
            If detectedSize = "1MB" Or detectedSize = "512KB" Then
                Log(">>> Hint: This looks like an ESP-01 or ESP-01S module.")
            ElseIf detectedSize = "4MB" Then
                Log(">>> Hint: This looks like an ESP-12 / NodeMCU module.")
            End If
        End If

        btnDetect.Enabled = True
    End Sub

    Private Sub SelectChip(chip As String)
        If cmbChip.Items.Contains(chip) Then cmbChip.SelectedItem = chip
    End Sub

    Private Sub SelectFlashMode(mode As String)
        If cmbFlashMode.Items.Contains(mode) Then cmbFlashMode.SelectedItem = mode
    End Sub

    ' --- ACTION: BROWSE ---
    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Bin Files|*.bin|All Files|*.*"
            If ofd.ShowDialog() = DialogResult.OK Then
                txtBinPath.Text = ofd.FileName
            End If
        End Using
    End Sub
    ' --- ACTION: FLASH ---
    Private Async Sub btnFlash_Click(sender As Object, e As EventArgs) Handles btnFlash.Click
        If String.IsNullOrEmpty(txtBinPath.Text) OrElse Not File.Exists(txtBinPath.Text) Then
            MessageBox.Show("Please select a valid .bin file.")
            Return
        End If

        ' Safety: ESP8285 requires DOUT
        If cmbChip.Text = "esp8285" AndAlso cmbFlashMode.Text <> "dout" Then
            If MessageBox.Show("ESP8285 requires 'dout' mode. Continue with '" & cmbFlashMode.Text & "'?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
        End If

        btnFlash.Enabled = False
        btnDetect.Enabled = False
        rtbLog.Clear()

        Dim port = cmbPort.Text
        Dim binPath = txtBinPath.Text
        Dim chip = cmbChip.Text
        Dim mode = cmbFlashMode.Text
        Dim size = cmbFlashSize.Text

        Try
            Log(">>> Starting Flash Process...")
            Log($">>> Chip: {chip} | Mode: {mode} | Size: {size}")

            ' 1. BAUD RATE: 115200 (Matches PyFlasher stability)
            ' 2. RESET: --after no_reset (Prevents bootloader hang)
            ' 3. ERASE: --erase-all (Forces clean slate, triggers the FS format on boot)
            Dim eraseArg As String = If(chkErase.Checked, "--erase-all", "")

            Dim args As String = $"--port {port} --baud 115200 --after no_reset write_flash -fm {mode} -fs {size} {eraseArg} 0x0 ""{binPath}"""

            If chip <> "auto" Then args = $"--chip {chip} " & args

            Await RunEsptoolAsync(args)

            Log("------------------------------------------------")
            Log(">>> FLASH SUCCESSFUL!")
            Log(">>> ACTION REQUIRED: Reset the board manually.")
            Log(">>> NOTE: First boot takes some time to format filesystem.")
            Log("------------------------------------------------")

            MessageBox.Show("Flash Complete!" & vbCrLf & vbCrLf &
                            "1. Unplug and Replug the device." & vbCrLf &
                            "2. WAIT 60 SECONDS for the filesystem to format." & vbCrLf &
                            "3. Do not interrupt it until you see the prompt.",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            Log("ERROR: " & ex.Message)
            MessageBox.Show("Flash failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnFlash.Enabled = True
            btnDetect.Enabled = True
        End Try
    End Sub


    ' --- ENGINE: RUN PROCESS (Unchanged mostly) ---
    Private Function RunEsptoolAsync(arguments As String) As Task(Of String)
        Dim tcs As New TaskCompletionSource(Of String)
        Dim fullOutput As New System.Text.StringBuilder()

        Dim p As New Process()
        p.StartInfo.FileName = _toolPath
        p.StartInfo.Arguments = arguments
        p.StartInfo.UseShellExecute = False
        p.StartInfo.RedirectStandardOutput = True
        p.StartInfo.RedirectStandardError = True
        p.StartInfo.CreateNoWindow = True

        AddHandler p.OutputDataReceived, Sub(s, args)
                                             If args.Data IsNot Nothing Then
                                                 fullOutput.AppendLine(args.Data)
                                                 Me.BeginInvoke(Sub() Log(args.Data))
                                             End If
                                         End Sub
        AddHandler p.ErrorDataReceived, Sub(s, args)
                                            If args.Data IsNot Nothing Then
                                                fullOutput.AppendLine(args.Data)
                                                Me.BeginInvoke(Sub() Log("STDERR: " & args.Data))
                                            End If
                                        End Sub

        p.EnableRaisingEvents = True
        AddHandler p.Exited, Sub(s, args)
                                 tcs.SetResult(fullOutput.ToString())
                             End Sub

        p.Start()
        p.BeginOutputReadLine()
        p.BeginErrorReadLine()

        Return tcs.Task
    End Function

    Private Sub Log(text As String)
        If rtbLog.IsDisposed Then Return
        rtbLog.AppendText(text & vbCrLf)
        rtbLog.ScrollToCaret()
    End Sub

End Class