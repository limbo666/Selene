Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading

' Assuming your project namespace has these classes available from your previous uploads
' If you named the project "SeleneV2", these might be "SeleneV2.ColorRichTextBox" etc.

Public Class frmMain



    ' =============================================================================
    ' THEME PERFORMANCE OPTIMIZATION
    ' =============================================================================
    ' Tracks the last theme applied to each tab. 
    ' If a tab's theme doesn't match the current global theme, we update it on click.
    Private _TabThemeState As New Dictionary(Of TabPage, String)
    Private _CurrentThemeName As String = "Default"
    ' =============================================================================
    ' THEME ENGINE VARIABLES
    ' =============================================================================
    ' Current Active Palette
    Private _CurrentTheme As ThemePalette

    ' Terminal Semantic Colors (Dynamic)
    Private _Color_RX As Color = Color.LimeGreen
    Private _Color_TX As Color = Color.DodgerBlue
    Private _Color_Prompt As Color = Color.Gold
    Private _Color_System As Color = Color.Orange
    Private _Color_Error As Color = Color.Gray

    Public _RespRect As New Rectangle(100, 100, 600, 400) ' Changed from Private to Public

    ' Raw Command History File
    Private _RawCmdFilePath As String = Path.Combine(Application.StartupPath, "suggestions.txt")

    Private _LastActiveTabIndex As Integer = 0
    ' Track the current color state between data packets
    Private _CurrentRXColor As Color = Color.LimeGreen

    ' =============================================================================
    ' 1. GLOBAL VARIABLES & INITIALIZATION
    ' =============================================================================
    Private _PlusTab As TabPage ' The special "+" tab
    Private _TabCounter As Integer = 1

    ' =============================================================================
    ' CONFIGURATION & STARTUP STATE
    ' =============================================================================
    Private _ConfigFilePath As String = Path.Combine(Application.StartupPath, "settings.ini")
    Private _RestoreSession As Boolean = True ' Default to TRUE
    ' Connectivity
    Private _AutoConnect As Boolean = False
    Private _LastPort As String = ""
    Private _LastBaud As String = "115200"

    ' Window Geometry
    Private _WinState As FormWindowState = FormWindowState.Normal
    Private _WinX As Integer = 100
    Private _WinY As Integer = 100
    Private _WinW As Integer = 1200
    Private _WinH As Integer = 800

    ' Splitter Positions (Using your specific names)
    Private _PosEvenOuter As Integer = 600
    Private _PosWorkArea As Integer = 600
    Private _PosMainOuter As Integer = 145

    ' =============================================================================
    ' MACRO SYSTEM VARIABLES
    ' =============================================================================
    Private Structure MacroItem
        Public Name As String
        Public Code As String
    End Structure

    ' Holds the 12 macros in memory (Indices 0-11 correspond to Buttons 1-12)
    Private _Macros(11) As MacroItem
    Private _MacroFilePath As String = Path.Combine(Application.StartupPath, "macros.txt")


    ' =============================================================================
    ' STATE VARIABLES FOR CAPTURE MODE
    ' =============================================================================
    Private _IsCapturingRead As Boolean = False
    Private _CaptureBuffer As New System.Text.StringBuilder()
    Private _CaptureFilename As String = ""

    ' SANDWICH MARKERS: We ignore everything outside these two tags
    Private Const START_MARKER As String = "---START_CAPTURE---"
    Private Const EOF_MARKER As String = "---EOF_CAPTURE---"

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupInterface()

        ' 1. Load & Apply Config
        LoadConfig()
        ApplyWindowSettings()
        If String.IsNullOrEmpty(_CurrentThemeName) Then _CurrentThemeName = "Default"
        ApplyTheme(_CurrentThemeName)
        RestoreSessionToolStripMenuItem.Checked = _RestoreSession
        AutoConnectOnStartupToolStripMenuItem.Checked = _AutoConnect
        If cmbBaud.Items.Contains(_LastBaud) Then cmbBaud.SelectedItem = _LastBaud

        ' 2. Populate Ports
        RefreshPorts()

        ' 3. Hook Events
        AddHandler ModGlobals.SerialEngine.DataReceivedUI, AddressOf OnDataReceived
        AddHandler ModGlobals.SerialEngine.ConnectionStatusChanged, AddressOf OnConnectionStatusChanged
        AddHandler ModGlobals.SerialEngine.DataSent, AddressOf OnDataSent

        InitMacroSystem()
        tcCodeEditors.TabPages.Clear()
        AddPlusTab()


        LoadRawCommands()


        '    AddNewTab("Untitled 1")
        RestoreSession()
        If _LastActiveTabIndex >= 0 AndAlso _LastActiveTabIndex < tcCodeEditors.TabCount Then
            tcCodeEditors.SelectedIndex = _LastActiveTabIndex
        End If

        ' 4. Auto Connect
        If _AutoConnect AndAlso cmbPorts.SelectedItem IsNot Nothing AndAlso cmbPorts.SelectedItem.ToString() = _LastPort Then
            ' Use a tiny delay to ensure UI is drawn before connecting
            Timer1.Interval = 500
            AddHandler Timer1.Tick, Sub()
                                        Timer1.Stop()
                                        AppendTerminal($"--- Auto-Connecting to {_LastPort} ---" & vbCrLf, Color.Orange)
                                        ModGlobals.SerialEngine.Connect(_LastPort, CInt(_LastBaud))
                                    End Sub
            Timer1.Start()
        End If
    End Sub

    ' =============================================================================
    ' TERMINAL COLORING & OUTPUT
    ' =============================================================================
    Private Sub AppendTerminal(text As String, color As Color)
        If rtbxTerminal.InvokeRequired Then
            rtbxTerminal.Invoke(Sub() AppendTerminal(text, color))
        Else
            ' 1. Set the color for the new text
            rtbxTerminal.SelectionStart = rtbxTerminal.TextLength
            rtbxTerminal.SelectionLength = 0
            rtbxTerminal.SelectionColor = color

            ' 2. Append the text
            rtbxTerminal.AppendText(text)

            ' 3. Scroll to bottom
            rtbxTerminal.ScrollToCaret()

            ' 4. Reset color back to default (just in case)
            rtbxTerminal.SelectionColor = rtbxTerminal.ForeColor
        End If
    End Sub


    ' =============================================================================
    ' INTELLIGENT TERMINAL FORMATTING
    ' =============================================================================
    Private Function IsNoiseData(text As String) As Boolean
        ' Safety: If it contains a prompt, it is definitely VALID data.
        If text.Contains(">") Then Return False

        Dim badCharCount As Integer = 0
        Dim totalChars As Integer = text.Length
        If totalChars = 0 Then Return False

        For Each c As Char In text
            Dim asciiVal As Integer = AscW(c)
            ' Check for unprintable chars
            If c = "?"c OrElse (asciiVal < 32 AndAlso asciiVal <> 13 AndAlso asciiVal <> 10 AndAlso asciiVal <> 9) Then
                badCharCount += 1
            End If
        Next

        Return (badCharCount / totalChars) > 0.2
    End Function
    ''' <summary>
    ''' PARSER V3: State Machine.
    ''' - Detects > to switch to COMMAND MODE (Blue).
    ''' - Detects Newline to switch to OUTPUT MODE (Green).
    ''' </summary>
    Private Sub AppendFormattedRX(text As String)
        ' 1. Check for Boot Garbage (Noise)
        If IsNoiseData(text) Then
            AppendTerminal(text, _Color_Error) '<-- CHANGED
            Return
        End If

        Dim buffer As New System.Text.StringBuilder()

        For Each c As Char In text
            If c = ">"c Then
                ' --- PROMPT DETECTED ---
                If buffer.Length > 0 Then
                    Dim cleanText As String = buffer.ToString().Replace(vbCr, "").Replace(vbLf, vbCrLf)
                    AppendTerminal(cleanText, _CurrentRXColor)
                    buffer.Clear()
                End If

                AppendTerminal(">", _Color_Prompt) '<-- CHANGED

                ' Switch to Command Color
                _CurrentRXColor = _Color_TX '<-- CHANGED

            ElseIf c = vbLf Then
                ' --- END OF LINE ---
                buffer.Append(c)
                Dim cleanText As String = buffer.ToString().Replace(vbCr, "").Replace(vbLf, vbCrLf)
                AppendTerminal(cleanText, _CurrentRXColor)
                buffer.Clear()

                ' Switch back to RX Color
                _CurrentRXColor = _Color_RX '<-- CHANGED

            Else
                buffer.Append(c)
            End If
        Next

        If buffer.Length > 0 Then
            Dim cleanText As String = buffer.ToString().Replace(vbCr, "").Replace(vbLf, vbCrLf)
            AppendTerminal(cleanText, _CurrentRXColor)
        End If
    End Sub

    Public Sub ApplyTheme(themeName As String)
        _CurrentThemeName = themeName
        Dim t As ThemePalette = ModThemes.GetTheme(themeName)
        _CurrentTheme = t

        ' 1. Apply Global Colors
        _Color_RX = t.Term_RX
        _Color_TX = t.Term_Command
        _Color_Prompt = t.Term_Prompt
        _Color_System = t.Term_System
        _Color_Error = t.Term_Error
        _CurrentRXColor = t.Term_RX

        Me.BackColor = t.FormBack
        Me.ForeColor = t.ForeColor

        If _FloatWindow IsNot Nothing Then
            _FloatWindow.BackColor = t.FormBack
            _FloatWindow.ForeColor = t.ForeColor
        End If

        UpdateControlsRecursively(Me.Controls, t)

        ' Terminal (Single instance, update immediately)
        rtbxTerminal.BackColor = t.EditorBack
        rtbxTerminal.ForeColor = t.Term_RX


        ' 2. UPDATE CODE EDITORS
        For Each tab As TabPage In tcCodeEditors.TabPages
            tab.BackColor = t.ControlBack

            Dim rtb As RichTextBox = GetEditorFromTab(tab)
            If rtb IsNot Nothing Then
                rtb.BackColor = t.EditorBack
                rtb.ForeColor = t.EditorFore

                ' Check if we have the highlighter stored
                If TypeOf rtb.Tag Is ColorRichTextBox.clsColorRichTextBox Then
                    Dim highlighter As ColorRichTextBox.clsColorRichTextBox = rtb.Tag

                    ' FIX: Call the new method directly!
                    ' This updates colors while keeping your Font and Keywords intact.
                    highlighter.SetThemeColors(t.EditorFore, t.Code_Keyword, t.Code_String, t.Code_Comment)

                    ' LAZY LOADING LOGIC
                    If tab Is tcCodeEditors.SelectedTab Then
                        ' If active, it's already recolored by SetThemeColors above.
                        ' Just mark as current.
                        If _TabThemeState.ContainsKey(tab) Then _TabThemeState(tab) = themeName
                    Else
                        ' Mark background tabs as outdated so we know to refresh them later
                        If _TabThemeState.ContainsKey(tab) Then _TabThemeState(tab) = "OUTDATED"
                    End If
                End If
            End If

            ' Update Line Numbers
            For Each pnl As Control In tab.Controls
                If TypeOf pnl Is Panel Then
                    For Each ctrl As Control In pnl.Controls
                        If ctrl.GetType().Name.Contains("LineNumber") Then
                            ctrl.BackColor = t.LineNumberBack
                            ctrl.ForeColor = t.LineNumberFore
                        End If
                    Next
                End If
            Next
        Next
        Me.Refresh()

        ' 9. UI FEEDBACK: Update Label
        lblCurrentTheme.Text = themeName

        ' 10. UI FEEDBACK: Update Menu Checkmarks
        UpdateThemeMenuCheckmarks(themeName)
    End Sub

    ' Helper to manage menu checkmarks
    Private Sub UpdateThemeMenuCheckmarks(currentTheme As String)
        ' Uncheck all first (assuming they are in a specific parent menu)
        ' You might need to adjust the parent name 'SelectThemeToolStripMenuItem' 
        ' based on your actual Designer name for the "Select Theme" menu item.
        For Each item As ToolStripItem In SelectThemeToolStripMenuItem.DropDownItems
            If TypeOf item Is ToolStripMenuItem Then
                DirectCast(item, ToolStripMenuItem).Checked = (item.Text = currentTheme)
            End If
        Next
    End Sub

    ' Helper to get RTB cleanly
    Private Function GetEditorFromTab(t As TabPage) As RichTextBox
        If t.Controls.Count > 0 AndAlso TypeOf t.Controls(0) Is Panel Then
            For Each c As Control In t.Controls(0).Controls
                If TypeOf c Is RichTextBox Then Return c
            Next
        End If
        Return Nothing
    End Function

    Private Sub UpdateControlsRecursively(ctrls As Control.ControlCollection, t As ThemePalette)
        For Each c As Control In ctrls
            If TypeOf c Is Button Then
                Dim btn As Button = c
                btn.FlatStyle = FlatStyle.Flat

                ' Check if this is a Macro Button
                If btn.Name.IndexOf("Macro", StringComparison.OrdinalIgnoreCase) >= 0 Then
                    ' --- MACRO BUTTONS (FILLED STYLE) ---
                    ' They keep the solid highlight color on the face
                    btn.BackColor = t.ButtonBack  ' The Highlight Color
                    btn.ForeColor = t.ButtonFore  ' Text Color (usually White/Black)
                    btn.FlatAppearance.BorderSize = 0 ' No border needed for filled buttons

                Else
                    ' --- STANDARD BUTTONS (OUTLINED STYLE) ---
                    ' 1. Make the background DARK (matches the panel/control background)
                    '    This ensures the colored border is actually visible!
                    btn.BackColor = t.ControlBack

                    ' 2. Apply the Highlight Color to the BORDER and TEXT
                    btn.FlatAppearance.BorderColor = t.ButtonBack ' Border is the Highlight Color
                    btn.FlatAppearance.BorderSize = 8            ' Nice thin border
                    btn.ForeColor = t.ButtonBack              ' Text matches the border (Professional look)

                    ' Optional: Add a slight mouse-over effect
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, t.ButtonBack)
                End If

            ElseIf TypeOf c Is Panel Or TypeOf c Is SplitContainer Then
                c.BackColor = t.ControlBack
                c.ForeColor = t.ForeColor
                If c.HasChildren Then UpdateControlsRecursively(c.Controls, t)

            ElseIf TypeOf c Is TextBox Or TypeOf c Is ComboBox Then
                c.BackColor = t.EditorBack
                c.ForeColor = t.EditorFore

            ElseIf TypeOf c Is Label Or TypeOf c Is CheckBox Or TypeOf c Is GroupBox Then
                c.ForeColor = t.ForeColor

            ElseIf TypeOf c Is TabControl Then
                c.BackColor = t.ControlBack
            End If
        Next
    End Sub

    Private Sub cmbRawCommands_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbRawCommands.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnSendRaw.PerformClick()
            e.SuppressKeyPress = True ' Stop the "Ding" sound
        End If
    End Sub

    Private Sub RefreshPorts()
        ' Remember current selection OR default to the Last Known Port
        Dim targetPort As String = If(cmbPorts.SelectedItem IsNot Nothing, cmbPorts.SelectedItem.ToString(), _LastPort)

        cmbPorts.Items.Clear()
        Dim ports = clsMicroSerial.GetAvailablePorts()
        For Each p In ports
            cmbPorts.Items.Add(p)
        Next

        ' Smart Selection: If our target port is plugged in, select it!
        If cmbPorts.Items.Contains(targetPort) Then
            cmbPorts.SelectedItem = targetPort
        ElseIf cmbPorts.Items.Count > 0 Then
            cmbPorts.SelectedIndex = 0 ' Fallback to the first available port
        End If
    End Sub
    ' =============================================================================
    ' MACRO PERSISTENCE (Load/Save)
    ' =============================================================================
    Private Sub LoadMacros()
        ' 1. Set Defaults if file doesn't exist
        If Not File.Exists(_MacroFilePath) Then
            For i As Integer = 0 To 11
                _Macros(i).Name = "Macro " & (i + 1)
                _Macros(i).Code = "print('Macro " & (i + 1) & "')"
            Next
            SaveMacros() ' Create the default file
        Else
            ' 2. Parse the file (Format: Name|Code)
            Dim lines = File.ReadAllLines(_MacroFilePath)
            For i As Integer = 0 To 11
                If i < lines.Length Then
                    Dim parts = lines(i).Split("|"c)
                    If parts.Length >= 2 Then
                        _Macros(i).Name = parts(0)
                        _Macros(i).Code = parts(1)
                    End If
                Else
                    _Macros(i).Name = "Macro " & (i + 1)
                    _Macros(i).Code = ""
                End If
            Next
        End If

        ' 3. Update UI Buttons
        UpdateMacroButtons()
    End Sub
    ' =============================================================================
    ' MACRO INTERACTION (Run & Edit)
    ' =============================================================================

    ' Call this from Form_Load to initialize the system
    Private Sub InitMacroSystem()
        LoadMacros()

        ' Bind Events for all 12 buttons dynamically
        Dim macroButtons() As Button = {btnMacro1, btnMacro2, btnMacro3, btnMacro4, btnMacro5, btnMacro6,
                                        btnMacro7, btnMacro8, btnMacro9, btnMacro10, btnMacro11, btnMacro12}

        For i As Integer = 0 To 11
            Dim index As Integer = i ' Capture for closure

            ' Clear old handlers to be safe
            RemoveHandler macroButtons(i).MouseDown, AddressOf MacroButton_MouseDown

            ' Add single handler for MouseDown (covers Left and Right click)
            macroButtons(i).Tag = index ' Store index in Tag
            AddHandler macroButtons(i).MouseDown, AddressOf MacroButton_MouseDown
        Next
    End Sub




    ' =============================================================================
    ' DOCKING SYSTEM (Terminal Detach/Reattach)
    ' =============================================================================
    Private _SavedTerminalWidth As Integer = 400 ' Default width
    Private _IsTerminalDetached As Boolean = False

    ' The form reference
    Private _FloatWindow As frmResponse

    Public Sub DetachTerminal()
        If _IsTerminalDetached Then Exit Sub

        ' 1. Save the current width of the terminal (SplitterDistance is from Left)
        ' Since Panel2 is on the right, we care about the SplitterDistance.
        _SavedTerminalWidth = scWorkArea.Width - scWorkArea.SplitterDistance

        ' 2. Create Floating Window
        _FloatWindow = New frmResponse
        _FloatWindow.Owner = Me ' Keep on top

        ' 3. MOVE THE CONTROLS (Only rtbxTerminal)
        rtbxTerminal.Parent = _FloatWindow
        rtbxTerminal.Dock = DockStyle.Fill
        rtbxTerminal.BringToFront()

        ' 4. COLLAPSE PANEL 2
        ' This hides the empty right side and lets Code Editor fill the screen
        scWorkArea.Panel2Collapsed = True

        ' 5. Show the window
        _FloatWindow.Show()
        _IsTerminalDetached = True
    End Sub
    Public Sub ReattachTerminal()
        ' 1. SAFETY CHECK: Stop recursion immediately
        If Not _IsTerminalDetached Then Exit Sub
        _IsTerminalDetached = False ' Lock the flag instantly

        ' 2. UN-COLLAPSE PANEL 2 (Show Right Side)
        scWorkArea.Panel2Collapsed = False

        ' 3. MOVE CONTROLS BACK
        ' We pull the control from the float window back to the panel
        rtbxTerminal.Parent = scWorkArea.Panel2
        rtbxTerminal.Dock = DockStyle.Fill
        rtbxTerminal.BringToFront()

        ' 4. RESTORE SPLITTER POSITION
        Try
            Dim newDist As Integer = scWorkArea.Width - _SavedTerminalWidth
            ' Sanity check for bounds
            If newDist < 50 Then newDist = scWorkArea.Width - 400
            If newDist > scWorkArea.Width - 50 Then newDist = scWorkArea.Width - 400

            scWorkArea.SplitterDistance = newDist
        Catch
        End Try

        ' 5. CLOSE THE WINDOW
        ' Check if it's valid and NOT already closing
        If _FloatWindow IsNot Nothing AndAlso Not _FloatWindow.IsDisposed Then
            ' We remove the handler to prevent any secondary trigger (optional extra safety)
            RemoveHandler _FloatWindow.FormClosing, AddressOf _FloatWindow_FormClosing
            _FloatWindow.Close()
        End If
        _FloatWindow = Nothing
    End Sub

    ' Helper to handle the float window closing if user clicked X
    ' (Make sure you bind this in DetachTerminal if not already handled by the class logic)
    Private Sub _FloatWindow_FormClosing(sender As Object, e As FormClosingEventArgs)
        ReattachTerminal()
    End Sub



    ' =============================================================================
    ' RAW COMMAND SYSTEM (The "Quick Fire" Box)
    ' =============================================================================

    Private Sub LoadRawCommands()
        cmbRawCommands.Items.Clear()
        cmbRawCommands.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbRawCommands.AutoCompleteSource = AutoCompleteSource.ListItems

        If File.Exists(_RawCmdFilePath) Then
            Dim lines = File.ReadAllLines(_RawCmdFilePath)
            For Each line In lines
                Dim cmd As String = line.Trim()
                If Not String.IsNullOrWhiteSpace(cmd) Then
                    cmbRawCommands.Items.Add(cmd)
                End If
            Next
        End If

        ' Default to empty or first item
        If cmbRawCommands.Items.Count > 0 Then cmbRawCommands.SelectedIndex = 0
    End Sub

    Private Sub SaveRawCommands()
        Dim lines As New List(Of String)
        ' Iterate through ComboBox items to save history
        For Each item As String In cmbRawCommands.Items
            If Not String.IsNullOrWhiteSpace(item) Then
                lines.Add(item)
            End If
        Next

        ' Overwrite the file with the current list
        Try
            File.WriteAllLines(_RawCmdFilePath, lines)
        Catch ex As Exception
            ' Ignore save errors on exit
        End Try
    End Sub

    ''' <summary>
    ''' SEND: Fires the command and moves it to the TOP of the history list.
    ''' </summary>
    Private Sub btnSendRaw_Click(sender As Object, e As EventArgs) Handles btnSendRaw.Click
        Dim cmd As String = cmbRawCommands.Text.Trim()

        If Not String.IsNullOrEmpty(cmd) AndAlso ModGlobals.SerialEngine.IsOpen Then
            ' 1. Send Logic
            ModGlobals.SerialEngine.Send(cmd)

            ' 2. Memory Logic: Move to Top (LRU Style)
            ' If the command exists anywhere in the list, remove it first.
            If cmbRawCommands.Items.Contains(cmd) Then
                cmbRawCommands.Items.Remove(cmd)
            End If

            ' Always insert at the top (Index 0) and select it
            cmbRawCommands.Items.Insert(0, cmd)
            cmbRawCommands.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' ADD: Appends the current text to suggestions.txt and reloads the list.
    ''' </summary>
    Private Sub lnkAddToList_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkAddToList.LinkClicked
        Dim newCmd As String = cmbRawCommands.Text.Trim()

        If String.IsNullOrEmpty(newCmd) Then Return

        ' 1. Check if already exists to avoid duplicates
        If cmbRawCommands.Items.Contains(newCmd) Then
            MessageBox.Show("Command already in list.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' 2. Append to file
        Try
            File.AppendAllText(_RawCmdFilePath, Environment.NewLine & newCmd)

            ' 3. Refresh UI
            LoadRawCommands()
            cmbRawCommands.Text = newCmd ' Restore what user typed

            MessageBox.Show("Added to suggestions.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error writing file: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' EDIT: Opens the raw text file in the default system editor (Notepad, VS Code, etc).
    ''' </summary>
    Private Sub lnkEditList_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkEditList.LinkClicked
        ' Ensure file exists before opening
        If Not File.Exists(_RawCmdFilePath) Then
            File.WriteAllText(_RawCmdFilePath, "node.restart()" & vbCrLf & "print(node.heap())")
        End If

        Try
            Process.Start(_RawCmdFilePath)
        Catch ex As Exception
            MessageBox.Show("Could not open editor: " & ex.Message)
        End Try
    End Sub






    Private Sub MacroButton_MouseDown(sender As Object, e As MouseEventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim index As Integer = CInt(btn.Tag)

        If e.Button = MouseButtons.Left Then
            ' --- RUN MACRO ---
            Dim codeToRun As String = _Macros(index).Code.Replace("\n", vbCrLf)
            If Not String.IsNullOrEmpty(codeToRun) Then
                If ModGlobals.SerialEngine.IsOpen Then
                    ModGlobals.SerialEngine.Send(codeToRun)
                Else
                    MessageBox.Show("Not connected.", "Macro", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If

        ElseIf e.Button = MouseButtons.Right Then
            ' --- EDIT MACRO ---
            ' We use input boxes for simplicity now. In V3, we can make a dedicated form.
            Dim newName As String = InputBox("Enter Name for Macro " & (index + 1), "Edit Macro Name", _Macros(index).Name)
            If String.IsNullOrWhiteSpace(newName) Then Return

            Dim newCode As String = InputBox("Enter Lua Code for " & newName, "Edit Macro Code", _Macros(index).Code.Replace("\n", vbCrLf))

            ' Save
            _Macros(index).Name = newName
            _Macros(index).Code = newCode.Replace(vbCrLf, "\n")
            SaveMacros()
            UpdateMacroButtons()
        End If
    End Sub
    Private Sub SaveMacros()
        Dim lines As New List(Of String)
        For i As Integer = 0 To 11
            ' Sanitize pipe characters to avoid corruption
            Dim safeName = _Macros(i).Name.Replace("|", "")
            Dim safeCode = _Macros(i).Code.Replace("|", "").Replace(vbCrLf, "\n") ' Flatten code
            lines.Add(safeName & "|" & safeCode)
        Next
        File.WriteAllLines(_MacroFilePath, lines)
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Disconnect cleanly
        If ModGlobals.SerialEngine.IsOpen Then
            ModGlobals.SerialEngine.Disconnect()
        End If

        ' Save Session Snapshot
        SaveSession()

        ' Save Configuration
        SaveConfig()

        SaveRawCommands()
    End Sub

    Private Sub UpdateMacroButtons()
        ' Map the array to the actual buttons controls
        ' We assume buttons are named btnMacro1, btnMacro2, etc.
        Dim macroButtons() As Button = {btnMacro1, btnMacro2, btnMacro3, btnMacro4, btnMacro5, btnMacro6,
                                        btnMacro7, btnMacro8, btnMacro9, btnMacro10, btnMacro11, btnMacro12}

        For i As Integer = 0 To 11
            If macroButtons(i) IsNot Nothing Then
                macroButtons(i).Text = _Macros(i).Name
                ' Add Tooltip so hovering shows the code
                Dim tt As New ToolTip()
                tt.SetToolTip(macroButtons(i), _Macros(i).Code.Replace("\n", vbCrLf))
            End If
        Next
    End Sub
    Private Sub SetupInterface()
        ' Dark Theme Base for the Shell
        tcCodeEditors.Dock = DockStyle.Fill
        lblStatusSquare.BackColor = Color.Maroon
        cmbBaud.SelectedItem = "115200"
        OnConnectionStatusChanged(False)
        ' Initial Splitter Positions
        scEvenOuter.SplitterDistance = Me.Height - 180

        ' --- NEW: SEARCH BOX MEMORY ---
        txtSeacrhTerm.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        txtSeacrhTerm.AutoCompleteSource = AutoCompleteSource.CustomSource
        txtSeacrhTerm.AutoCompleteCustomSource = New AutoCompleteStringCollection()
    End Sub

    ' =============================================================================
    ' 2. DYNAMIC TAB MANAGEMENT (The Core Request)
    ' =============================================================================

    ''' <summary>
    ''' Adds the special "+" tab that triggers new file creation
    ''' </summary>
    Private Sub AddPlusTab()
        If _PlusTab IsNot Nothing AndAlso tcCodeEditors.TabPages.Contains(_PlusTab) Then Return

        _PlusTab = New TabPage(" + ")
        _PlusTab.Name = "tabPlus"
        tcCodeEditors.TabPages.Add(_PlusTab)
    End Sub


    Private Sub LoadConfig()
        If File.Exists(_ConfigFilePath) Then
            Dim lines = File.ReadAllLines(_ConfigFilePath)
            For Each line In lines
                Dim parts = line.Split("="c)
                If parts.Length = 2 Then
                    Dim key = parts(0).Trim().ToUpper()
                    Dim val = parts(1).Trim()

                    Select Case key
                        ' Connectivity
                        Case "LASTPORT" : _LastPort = val
                        Case "LASTBAUD" : _LastBaud = val
                        Case "AUTOCONNECT" : Boolean.TryParse(val, _AutoConnect)

                        ' Window Geometry
                        Case "WINSTATE" : Integer.TryParse(val, _WinState)
                        Case "WINX" : Integer.TryParse(val, _WinX)
                        Case "WINY" : Integer.TryParse(val, _WinY)
                        Case "WINW" : Integer.TryParse(val, _WinW)
                        Case "WINH" : Integer.TryParse(val, _WinH)

                        ' Splitters (Mapping keys to variables)
                        Case "SPLIT_EVENOUTER" : Integer.TryParse(val, _PosEvenOuter)
                        Case "SPLIT_WORKAREA" : Integer.TryParse(val, _PosWorkArea)
                        Case "SPLIT_MAINOUTER" : Integer.TryParse(val, _PosMainOuter)
                        Case "RESTORESESSION" : Boolean.TryParse(val, _RestoreSession)
                        Case "LASTTAB" : Integer.TryParse(val, _LastActiveTabIndex) '
                        Case "THEME" : _CurrentThemeName = val
                    End Select
                End If
            Next
        End If
    End Sub

    Public Sub SaveConfig()
        Dim lines As New List(Of String)

        ' 1. Connectivity
        lines.Add("LASTPORT=" & If(cmbPorts.SelectedItem IsNot Nothing, cmbPorts.SelectedItem.ToString(), ""))
        lines.Add("LASTBAUD=" & cmbBaud.Text)
        lines.Add("AUTOCONNECT=" & _AutoConnect.ToString())

        ' 2. Window Geometry
        If Me.WindowState = FormWindowState.Normal Then
            lines.Add("WINSTATE=" & CInt(Me.WindowState))
            lines.Add("WINX=" & Me.Location.X)
            lines.Add("WINY=" & Me.Location.Y)
            lines.Add("WINW=" & Me.Size.Width)
            lines.Add("WINH=" & Me.Size.Height)
        Else
            lines.Add("WINSTATE=" & CInt(Me.WindowState))
            lines.Add("WINX=" & Me.RestoreBounds.X)
            lines.Add("WINY=" & Me.RestoreBounds.Y)
            lines.Add("WINW=" & Me.RestoreBounds.Width)
            lines.Add("WINH=" & Me.RestoreBounds.Height)
        End If

        ' 3. Splitters
        lines.Add("SPLIT_EVENOUTER=" & scEvenOuter.SplitterDistance)
        lines.Add("SPLIT_WORKAREA=" & scWorkArea.SplitterDistance)
        lines.Add("SPLIT_MAINOUTER=" & scMainOuter.SplitterDistance)

        ' 4. Session & Theme
        lines.Add("RESTORESESSION=" & _RestoreSession.ToString())
        lines.Add("LASTTAB=" & Math.Max(0, tcCodeEditors.SelectedIndex))
        lines.Add("THEME=" & _CurrentThemeName) ' <--- NEW LINE HERE

        File.WriteAllLines(_ConfigFilePath, lines)
    End Sub
    Private Sub ApplyWindowSettings()
        ' 1. Apply Size and Location
        Me.StartPosition = FormStartPosition.Manual
        Me.Location = New Point(_WinX, _WinY)
        Me.Size = New Size(_WinW, _WinH)

        ' 2. Ensure visible
        Dim isVisible As Boolean = False
        For Each scr As Screen In Screen.AllScreens
            If scr.Bounds.IntersectsWith(Me.Bounds) Then
                isVisible = True
                Exit For
            End If
        Next
        If Not isVisible Then
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Size = New Size(1024, 768)
        End If

        ' 3. Apply Window State
        Me.WindowState = _WinState

        ' 4. Apply Splitters (Try/Catch for each to prevent crashes on resize)
        Try
            scEvenOuter.SplitterDistance = _PosEvenOuter
        Catch
        End Try

        Try
            scWorkArea.SplitterDistance = _PosWorkArea
        Catch
        End Try

        Try
            scMainOuter.SplitterDistance = _PosMainOuter
        Catch
        End Try
    End Sub


    ' =============================================================================
    ' SESSION SNAPSHOT SYSTEM (The "Time Machine")
    ' =============================================================================
    Private Sub SaveSession()
        Dim cacheDir As String = Path.Combine(Application.StartupPath, "SessionCache")

        ' 1. Clean previous cache
        If Directory.Exists(cacheDir) Then Directory.Delete(cacheDir, True)
        Directory.CreateDirectory(cacheDir)

        If Not _RestoreSession Then Exit Sub

        ' 2. Save Metadata and Content
        Dim sessionLines As New List(Of String)

        For i As Integer = 0 To tcCodeEditors.TabPages.Count - 1
            Dim t As TabPage = tcCodeEditors.TabPages(i)
            If t Is _PlusTab Then Continue For

            ' Get Editor Content
            Dim rtb As RichTextBox = Nothing
            If t.Controls.Count > 0 AndAlso t.Controls(0).Controls.Count > 0 Then
                ' Accessing rtb inside the panel
                For Each c As Control In t.Controls(0).Controls
                    If TypeOf c Is RichTextBox Then rtb = c
                Next
            End If

            If rtb IsNot Nothing Then
                ' Save Content to a temp file
                Dim tempFile As String = Path.Combine(cacheDir, "tab_" & i & ".bak")
                File.WriteAllText(tempFile, rtb.Text)

                ' Save Metadata: ID | OriginalPath | TabTitle
                Dim originalPath As String = t.Tag.ToString()
                Dim title As String = t.Text
                sessionLines.Add(i & "|" & originalPath & "|" & title)
            End If
        Next

        File.WriteAllLines(Path.Combine(cacheDir, "session.config"), sessionLines)
    End Sub

    Private Sub RestoreSession()
        Dim cacheDir As String = Path.Combine(Application.StartupPath, "SessionCache")
        Dim configFile As String = Path.Combine(cacheDir, "session.config")

        ' If no session exists or feature disabled, just give a blank tab
        If Not _RestoreSession OrElse Not File.Exists(configFile) Then
            AddNewTab("Untitled 1")
            Return
        End If

        Try
            Dim lines = File.ReadAllLines(configFile)
            Dim tabsRestored As Integer = 0

            For Each line In lines
                Dim parts = line.Split("|"c)
                If parts.Length >= 3 Then
                    Dim id As String = parts(0)
                    Dim origPath As String = parts(1)
                    Dim title As String = parts(2)
                    Dim contentFile As String = Path.Combine(cacheDir, "tab_" & id & ".bak")

                    If File.Exists(contentFile) Then
                        Dim content As String = File.ReadAllText(contentFile)

                        ' Create the tab with the SNAPSHOT content
                        AddNewTab(title.Replace("*", ""), content, origPath)

                        ' CHECK DIRTY STATUS
                        ' If the snapshot content is different from what is actually on disk
                        ' (or if it was an untitled file), mark it as dirty (*).
                        Dim isDirty As Boolean = True

                        If Not String.IsNullOrEmpty(origPath) AndAlso File.Exists(origPath) Then
                            Dim diskContent As String = File.ReadAllText(origPath)
                            If diskContent = content Then isDirty = False
                        End If

                        ' Force the star if dirty
                        If isDirty Then
                            tcCodeEditors.SelectedTab.Text = title.Replace("*", "") & "*"
                        End If

                        tabsRestored += 1
                    End If
                End If
            Next

            ' Fallback if something corrupted
            If tabsRestored = 0 Then AddNewTab("Untitled 1")

        Catch ex As Exception
            ' If session load fails, start fresh
            AddNewTab("Untitled 1")
        End Try
    End Sub

    ''' <summary>
    ''' Creates a full-featured code tab. 
    ''' FIX: Sets Text AFTER attaching the Highlighter to ensure coloring triggers.
    ''' </summary>
    Private Sub AddNewTab(title As String, Optional content As String = "", Optional filePath As String = "")
        ' 1. Remove "+" tab temporarily
        If tcCodeEditors.TabPages.Contains(_PlusTab) Then tcCodeEditors.TabPages.Remove(_PlusTab)

        ' 2. Create Structure
        Dim newTab As New TabPage(title)
        newTab.Tag = filePath
        newTab.BackColor = Color.FromArgb(200, 200, 200)

        Dim pnlContainer As New Panel With {.Dock = DockStyle.Fill}

        ' 3. Create Editor (Do NOT set Text here yet!)
        Dim rtb As New RichTextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Consolas", 10),
            .AcceptsTab = True,
            .WordWrap = False,
            .BackColor = Color.FromArgb(230, 230, 230),
            .ForeColor = Color.Gainsboro,
            .BorderStyle = BorderStyle.None,
            .HideSelection = False
        }

        ' 4. Add Line Numbers
        Try
            Dim lineNums As New LineNumbers_For_RichTextBox With {
                .ParentRichTextBox = rtb,
                .Dock = DockStyle.Left,
                .Width = 35,
                .BackColor = Color.FromArgb(45, 45, 48),
                .ForeColor = Color.CadetBlue,
                .Show_BackgroundGradient = False,
                .Show_GridLines = False,
                .Show_BorderLines = True
            }
            pnlContainer.Controls.Add(lineNums)
        Catch ex As Exception
        End Try

        ' 5. Attach Syntax Highlighter (BEFORE setting text)
        Try
            Dim syntax As New ColorRichTextBox.clsColorRichTextBox(rtb)
            rtb.Tag = syntax ' <--- CRITICAL: Store reference for Theme Engine
        Catch ex As Exception
        End Try

        ' 6. Assembly
        pnlContainer.Controls.Add(rtb)
        rtb.BringToFront()
        newTab.Controls.Add(pnlContainer)

        ' 7. Context Menu
        Dim ctx As New ContextMenuStrip
        ctx.Items.Add("Run Selected Code", Nothing, AddressOf btnRunSelectedCode_Click)
        ctx.Items.Add(New ToolStripSeparator())
        ctx.Items.Add("Cut", Nothing, Sub() rtb.Cut())
        ctx.Items.Add("Copy", Nothing, Sub() rtb.Copy())
        ctx.Items.Add("Paste", Nothing, Sub() rtb.Paste())
        ctx.Items.Add(New ToolStripSeparator())
        ctx.Items.Add("Select All", Nothing, Sub() rtb.SelectAll())
        ctx.Items.Add("Clear All", Nothing, Sub() rtb.Clear())
        rtb.ContextMenuStrip = ctx

        ' 8. Events
        AddHandler rtb.TextChanged, AddressOf Editor_TextChanged

        ' 9. Add to Form
        tcCodeEditors.TabPages.Add(newTab)
        tcCodeEditors.SelectedTab = newTab

        ' 10. Re-add "+" tab
        AddPlusTab()

        UpdateUIForTab(newTab)

        ' =========================================================================
        ' THE FIX: Set Text LAST, so the Syntax Highlighter catches the change event
        ' =========================================================================
        If Not String.IsNullOrEmpty(content) Then
            rtb.Text = content
            ' Reset the "Dirty" flag because we just loaded a fresh file
            ' (Setting Text triggers the dirty flag logic, so we undo it here)
            newTab.Text = title
        End If
    End Sub


    ''' <summary>
    ''' Closes a tab. Returns TRUE if closed successfully, FALSE if user cancelled.
    ''' </summary>
    Private Function CloseTab(tab As TabPage) As Boolean
        ' Don't close the plus tab
        If tab Is _PlusTab Then Return True

        ' Check for unsaved changes
        If tab.Text.Contains("*") Then
            ' Select the tab so the user sees which one is asking
            tcCodeEditors.SelectedTab = tab

            Dim result = MessageBox.Show($"Save changes to {tab.Text.TrimEnd("*"c)}?", "Unsaved Work", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)

            If result = DialogResult.Cancel Then Return False ' User cancelled operation

            If result = DialogResult.Yes Then
                SaveCurrentFile()
                ' If after trying to save it's STILL dirty (e.g. user cancelled Save As), abort
                If tab.Text.Contains("*") Then Return False
            End If
        End If

        ' CLEANUP: Remove from theme tracker
        If _TabThemeState.ContainsKey(tab) Then _TabThemeState.Remove(tab)

        tcCodeEditors.TabPages.Remove(tab)

        ' If we closed the last tab, create a new untitled one
        If tcCodeEditors.TabPages.Count = 1 AndAlso tcCodeEditors.TabPages.Contains(_PlusTab) Then
            AddNewTab("Untitled 1")
        End If

        Return True
    End Function

    ' =============================================================================
    ' 3. UI STATE LOGIC (Switching Tabs, Updating Labels)
    Private Sub tcCodeEditors_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tcCodeEditors.SelectedIndexChanged
        Dim tab = tcCodeEditors.SelectedTab
        If tab Is Nothing Then Exit Sub

        If tab Is _PlusTab Then
            _TabCounter += 1
            AddNewTab("Untitled " & _TabCounter)
            Exit Sub
        End If

        ' LAZY RELOAD: If marked outdated, force re-color now
        If _TabThemeState.ContainsKey(tab) AndAlso _TabThemeState(tab) <> _CurrentThemeName Then
            Dim rtb As RichTextBox = GetEditorFromTab(tab)
            If rtb IsNot Nothing AndAlso TypeOf rtb.Tag Is ColorRichTextBox.clsColorRichTextBox Then
                Dim highlighter As ColorRichTextBox.clsColorRichTextBox = rtb.Tag
                highlighter.RecolorEntireText()
                _TabThemeState(tab) = _CurrentThemeName
            End If
        End If

        UpdateUIForTab(tab)
    End Sub

    Private Sub UpdateUIForTab(tab As TabPage)
        Dim fullPath As String = tab.Tag.ToString()

        ' 1. Update Working File Label
        If String.IsNullOrEmpty(fullPath) Then
            lblWorkingFile.Text = "Unsaved File"
        Else
            lblWorkingFile.Text = fullPath
        End If

        ' 2. Update Device Filename Textbox (Smart Load)
        If Not String.IsNullOrEmpty(fullPath) Then
            txtfile.Text = Path.GetFileName(fullPath)
        Else
            ' FIX: Prevent double extension (e.g., test.lua.lua)
            Dim cleanName As String = tab.Text.Replace(" ", "").Replace("*", "").ToLower()
            If Not cleanName.EndsWith(".lua") Then
                cleanName &= ".lua"
            End If
            txtfile.Text = cleanName
        End If
    End Sub
    Private Sub Editor_TextChanged(sender As Object, e As EventArgs)
        ' Handle the Dirty Flag (*)
        Dim rtb As RichTextBox = sender
        ' Hierarchy: RTB -> Panel -> TabPage
        Dim parentTab As TabPage = Nothing
        If TypeOf rtb.Parent Is Panel Then
            parentTab = rtb.Parent.Parent
        ElseIf TypeOf rtb.Parent Is TabPage Then
            parentTab = rtb.Parent
        End If

        If parentTab IsNot Nothing AndAlso Not parentTab.Text.EndsWith("*") Then
            parentTab.Text &= "*"
        End If
    End Sub

    Private Function GetActiveEditor() As RichTextBox
        Dim tab = tcCodeEditors.SelectedTab
        If tab Is Nothing OrElse tab Is _PlusTab Then Return Nothing

        ' Look for RTB inside the panel
        For Each c As Control In tab.Controls
            If TypeOf c Is Panel Then
                For Each subC As Control In c.Controls
                    If TypeOf subC Is RichTextBox Then Return subC
                Next
            End If
        Next
        Return Nothing
    End Function

    ' =============================================================================
    ' 4. FILE OPERATIONS (Save, Save As, Open)
    ' =============================================================================
    Private Sub SaveCurrentFile()
        Dim editor = GetActiveEditor()
        Dim tab = tcCodeEditors.SelectedTab
        If editor Is Nothing Then Exit Sub

        Dim currentPath As String = tab.Tag.ToString()

        ' If never saved, trigger SaveAs
        If String.IsNullOrEmpty(currentPath) Then
            SaveAsFile()
        Else
            Try
                File.WriteAllText(currentPath, editor.Text)
                tab.Text = Path.GetFileName(currentPath) ' Removes *
                lblWorkingFile.Text = currentPath
            Catch ex As Exception
                MessageBox.Show("Error saving file: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub SaveAsFile()
        Dim editor = GetActiveEditor()
        Dim tab = tcCodeEditors.SelectedTab
        If editor Is Nothing Then Exit Sub

        Dim sfd As New SaveFileDialog With {.Filter = "Lua Files|*.lua|All Files|*.*"}
        If sfd.ShowDialog() = DialogResult.OK Then
            Try
                File.WriteAllText(sfd.FileName, editor.Text)
                tab.Tag = sfd.FileName
                tab.Text = Path.GetFileName(sfd.FileName)
                lblWorkingFile.Text = sfd.FileName
                txtfile.Text = Path.GetFileName(sfd.FileName) ' Update device filename too
            Catch ex As Exception
                MessageBox.Show("Error saving file: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub OpenFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenFileToolStripMenuItem.Click
        Dim ofd As New OpenFileDialog With {
            .Filter = "Lua Files|*.lua|All Files|*.*",
            .Multiselect = True,  ' <--- ENABLE MULTI-SELECTION
            .Title = "Open Files"
        }

        If ofd.ShowDialog() = DialogResult.OK Then
            ' Loop through every selected file
            For Each f As String In ofd.FileNames
                Dim content As String = File.ReadAllText(f)
                Dim fName As String = Path.GetFileName(f)

                ' Check if we can reuse the current tab (if it's blank and unsaved)
                Dim editor = GetActiveEditor()
                Dim currentTab = tcCodeEditors.SelectedTab

                ' Condition: Editor exists, is empty, and has no saved file path
                Dim isBlankTab As Boolean = (editor IsNot Nothing AndAlso
                                             editor.TextLength = 0 AndAlso
                                             String.IsNullOrEmpty(currentTab.Tag.ToString()))

                If isBlankTab Then
                    ' Recycle the blank tab for the first file
                    editor.Text = content
                    currentTab.Tag = f         ' Store full path
                    currentTab.Text = fName    ' Set Tab Title
                    UpdateUIForTab(currentTab)
                Else
                    ' Create a new tab for subsequent files
                    AddNewTab(fName, content, f)
                End If
            Next
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        SaveCurrentFile()
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        SaveAsFile()
    End Sub

    ' =============================================================================
    ' 5. SERIAL & EXECUTION (The Engine)
    ' =============================================================================
    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        If btnConnect.Text = "Connect" Then
            If cmbPorts.SelectedItem Is Nothing Then Exit Sub

            ' Update memory and Save
            _LastPort = cmbPorts.SelectedItem.ToString()
            _LastBaud = cmbBaud.Text
            SaveConfig()
            ' Add this BEFORE ModGlobals.SerialEngine.Connect:
            AppendTerminal($"--- Connecting to {_LastPort} at {_LastBaud} ---" & vbCrLf, Color.Orange)
            ModGlobals.SerialEngine.Connect(_LastPort, CInt(_LastBaud))
        Else
            ModGlobals.SerialEngine.Disconnect()
        End If
    End Sub

    Private Sub AutoConnectOnStartupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutoConnectOnStartupToolStripMenuItem.Click
        ' Toggle the checkmark
        AutoConnectOnStartupToolStripMenuItem.Checked = Not AutoConnectOnStartupToolStripMenuItem.Checked

        ' Save the state
        _AutoConnect = AutoConnectOnStartupToolStripMenuItem.Checked
        SaveConfig()
    End Sub

    Private Sub btnRefreshPorts_Click(sender As Object, e As EventArgs) Handles btnRefreshPorts.Click
        RefreshPorts()
    End Sub



    Private Sub btnRunSelectedCode_Click(sender As Object, e As EventArgs) Handles btnRunSelectedCode.Click
        Dim editor = GetActiveEditor()
        If editor Is Nothing Then Exit Sub

        Dim code As String = editor.SelectedText

        If String.IsNullOrWhiteSpace(code) Then
            MessageBox.Show("Select the code you want to run first.", "Run Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' --- LOGIC FIX: Uncomment Lines if Requested ---
        If chkUncommentCode.Checked Then
            code = UncommentForExecution(code)
        End If
        ' -----------------------------------------------

        SendCodeToDevice(code)
    End Sub

    Private Sub SendCodeToDevice(code As String)
        If Not ModGlobals.SerialEngine.IsOpen Then Return

        lblTX.BackColor = Color.DodgerBlue
        tmrIndicatorReset.Start()

        Dim lines = code.Split(New String() {vbCrLf, vbLf}, StringSplitOptions.None)
        For Each line In lines
            ModGlobals.SerialEngine.Send(line)
            Thread.Sleep(20) ' Small throttle
        Next
    End Sub




    ''' <summary>
    ''' UI STATE MANAGER: Locks/Unlocks controls based on connection status.
    ''' </summary>
    Private Sub OnConnectionStatusChanged(isConnected As Boolean)
        If Me.InvokeRequired Then
            Me.Invoke(Sub() OnConnectionStatusChanged(isConnected))
        Else
            ' 1. Connection Button & Status Light
            If isConnected Then
                btnConnect.Text = "Disconnect"
                lblStatusSquare.BackColor = Color.LimeGreen
            Else
                btnConnect.Text = "Connect"
                lblStatusSquare.BackColor = Color.Maroon
            End If

            ' 2. Disable/Enable Main Control Groups
            grpManageFileSystem.Enabled = isConnected ' List, Format, etc.
            grpRun.Enabled = isConnected              ' Run Selected code
            grpFileActions.Enabled = isConnected      ' Read, Write, Delete

            ' 3. Disable/Enable Raw Command Stuff
            btnSendRaw.Enabled = isConnected
            ' Optional: cmbRawCommands.Enabled = isConnected ' Uncomment if you want to lock the text box too

            ' 4. Disable/Enable All 12 Macro Buttons
            Dim macroButtons() As Button = {btnMacro1, btnMacro2, btnMacro3, btnMacro4, btnMacro5, btnMacro6,
                                            btnMacro7, btnMacro8, btnMacro9, btnMacro10, btnMacro11, btnMacro12}

            For Each btn As Button In macroButtons
                If btn IsNot Nothing Then btn.Enabled = isConnected
            Next
        End If
    End Sub

    Private Sub tmrIndicatorReset_Tick(sender As Object, e As EventArgs) Handles tmrIndicatorReset.Tick
        lblRX.BackColor = Color.Gray
        lblTX.BackColor = Color.Gray
        tmrIndicatorReset.Stop()
    End Sub



    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    ' =============================================================================
    ' DEVICE FILE OPERATIONS (The "Cloud" Storage)
    ' =============================================================================

    Private Async Sub btnWrite_Click(sender As Object, e As EventArgs) Handles btnWrite.Click
        If Not ModGlobals.SerialEngine.IsOpen Then
            MessageBox.Show("Please connect to a device first.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim editor = GetActiveEditor()
        If editor Is Nothing Then Return

        Dim targetFilename As String = txtfile.Text.Trim()
        If String.IsNullOrEmpty(targetFilename) Then
            MessageBox.Show("Please specify a filename for the device (e.g., init.lua).", "Missing Filename", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Disable UI to prevent interference during upload
        grpFileActions.Enabled = False
        lblStatusSquare.BackColor = Color.Orange ' Busy status

        Try
            ' 1. Prepare the device
            AppendTerminal(vbCrLf & $"--- Starting Upload: {targetFilename} ---" & vbCrLf, Color.Orange)
            ModGlobals.SerialEngine.Send($"file.remove('{targetFilename}')")
            Await Task.Delay(100)
            ModGlobals.SerialEngine.Send($"file.open('{targetFilename}', 'w+')")
            Await Task.Delay(100)

            ' 2. Stream the lines
            Dim lines = editor.Lines
            Dim totalLines As Integer = lines.Length

            For i As Integer = 0 To totalLines - 1
                Dim line As String = lines(i)
                ' Escape Lua block comments if necessary, though [[ ]] is usually safe for code
                ' We use [[ string ]] syntax for safety against quotes " or '
                If line.Contains("]]") Then line = line.Replace("]]", "] ]") ' Basic sanitizer

                Dim command As String = $"file.writeline([[{line}]])"
                ModGlobals.SerialEngine.Send(command)

                ' Throttle the upload slightly to prevent Serial Buffer Overflow on the ESP
                ' A generic 30-50ms is usually safe for 115200 baud
                Await Task.Delay(40)

                ' Optional: Update a progress bar here if we added one
            Next

            ' 3. Close the file and finish
            ModGlobals.SerialEngine.Send("file.close()")
            AppendTerminal(vbCrLf & "--- Upload Complete ---" & vbCrLf, Color.Orange)

            ' 4. Auto-Run if checked
            If chkAutoExecute.Checked Then
                Await Task.Delay(200)
                ModGlobals.SerialEngine.Send($"dofile('{targetFilename}')")
            End If

        Catch ex As Exception
            MessageBox.Show("Upload failed: " & ex.Message)
        Finally
            grpFileActions.Enabled = True
            lblStatusSquare.BackColor = Color.LimeGreen
        End Try
    End Sub



    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If Not ModGlobals.SerialEngine.IsOpen Then Return

        Dim targetFilename As String = txtfile.Text.Trim()
        If MessageBox.Show($"Are you sure you want to delete '{targetFilename}' from the device?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            ModGlobals.SerialEngine.Send($"file.remove('{targetFilename}')")
            rtbxTerminal.AppendText(vbCrLf & $"--- Deleted {targetFilename} ---" & vbCrLf)
        End If
    End Sub

    Private Sub btnListFiles_Click(sender As Object, e As EventArgs) Handles btnListFiles.Click
        If Not ModGlobals.SerialEngine.IsOpen Then Return

        AppendTerminal(vbCrLf & "--- File List ---" & vbCrLf, Color.Orange)
        ' Classic NodeMCU file list loop
        Dim luaCmd As String = "l = file.list(); for k,v in pairs(l) do print('name:'..k..', size:'..v) end"
        ModGlobals.SerialEngine.Send(luaCmd)
    End Sub

    Private Sub btnFormat_Click(sender As Object, e As EventArgs) Handles btnFormat.Click
        If Not ModGlobals.SerialEngine.IsOpen Then Return

        If MessageBox.Show("FORMAT DEVICE? This will erase ALL files on the ESP chip!", "CRITICAL WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) = DialogResult.Yes Then
            AppendTerminal(vbCrLf & "--- Formatting... This may take up to 30 seconds ---" & vbCrLf, Color.Orange)
            ModGlobals.SerialEngine.Send("file.format()")
        End If
    End Sub


    Private Sub btnExecute_Click(sender As Object, e As EventArgs) Handles btnExecute.Click
        If Not ModGlobals.SerialEngine.IsOpen Then Return
        Dim targetFilename As String = txtfile.Text.Trim()
        If String.IsNullOrEmpty(targetFilename) Then Return

        ModGlobals.SerialEngine.Send($"dofile('{targetFilename}')")
    End Sub

    ' =============================================================================
    ' UI HELPERS
    ' =============================================================================
    Private Sub lnkDetachResponse_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkDetachResponse.LinkClicked
        DetachTerminal()
    End Sub








    ''' <summary>
    ''' WATCHDOG: Force-stops the capture if device takes too long or crashes.
    ''' </summary>
    Private Sub tmrReadTimeout_Tick(sender As Object, e As EventArgs) Handles tmrReadTimeout.Tick
        tmrReadTimeout.Stop()
        If _IsCapturingRead Then
            _IsCapturingRead = False
            Cursor = Cursors.Default
            lblStatusSquare.BackColor = Color.LimeGreen
            MessageBox.Show("Read Timed Out. The device did not respond correctly.", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ' Dump what we got so far for debugging
            rtbxTerminal.AppendText(vbCrLf & "--- PARTIAL DATA ---" & vbCrLf & _CaptureBuffer.ToString())
        End If
    End Sub

    ' =============================================================================
    ' FINAL CORRECTED LOGIC FOR READ & DATA RECEPTION
    ' (Replace any duplicates of these 3 subs with this block)
    ' =============================================================================
    Private Const READ_START_TOKEN As String = "###START###"
    ''' <summary>
    ''' TRAFFIC CONTROLLER: Routes incoming data.
    ''' </summary>
    Private Sub OnDataReceived(data As String)
        If Me.InvokeRequired Then
            Me.Invoke(Sub() OnDataReceived(data))
        Else
            ' PATH A: CAPTURE MODE (Downloading a file)
            If _IsCapturingRead Then
                _CaptureBuffer.Append(data)

                Dim currentData As String = _CaptureBuffer.ToString()
                If currentData.TrimEnd().EndsWith(">") Then
                    tmrReadTimeout.Stop()
                    _IsCapturingRead = False
                    Cursor = Cursors.Default
                    lblStatusSquare.BackColor = Color.LimeGreen

                    ProcessCapturedFile(currentData)
                End If
                Return
            End If

            ' PATH B: NORMAL TERMINAL MODE
            ' CRITICAL: Must call the Formatter, NOT AppendTerminal directly!
            AppendFormattedRX(data)

            ' Flash RX Light
            lblRX.BackColor = Color.LimeGreen
            tmrIndicatorReset.Start()
        End If
    End Sub

    ''' <summary>
    ''' VISUAL FEEDBACK: Flashes TX light.
    ''' We do NOT print text here anymore because NodeMCU echoes it back automatically.
    ''' </summary>
    Private Sub OnDataSent(data As String)
        If Me.InvokeRequired Then
            Me.Invoke(Sub() OnDataSent(data))
        Else
            ' 1. LOCAL ECHO DISABLED (Commented out)
            ' AppendTerminal(data & vbCrLf, Color.DodgerBlue) 

            ' 2. Flash TX Light
            lblTX.BackColor = Color.DodgerBlue
            tmrIndicatorReset.Stop()
            tmrIndicatorReset.Start()
        End If
    End Sub

    ''' <summary>
    ''' DOWNLOAD TRIGGER: Sends the read command with a start marker.
    ''' </summary>
    Private Sub btnRead_Click(sender As Object, e As EventArgs) Handles btnRead.Click
        If Not ModGlobals.SerialEngine.IsOpen Then Return

        Dim targetFilename As String = txtfile.Text.Trim()
        If String.IsNullOrEmpty(targetFilename) Then Return

        ' 1. Reset Capture State
        _IsCapturingRead = True
        _CaptureBuffer.Clear()
        _CaptureFilename = targetFilename

        lblStatusSquare.BackColor = Color.Orange
        Cursor = Cursors.WaitCursor

        ' Find "Requesting":
        AppendTerminal(vbCrLf & $"--- Requesting {targetFilename} ---" & vbCrLf, Color.Orange)

        ' 2. Start Safety Watchdog
        tmrReadTimeout.Stop()
        tmrReadTimeout.Interval = 8000
        tmrReadTimeout.Start()

        ' 3. Send Command (Using uart.write to avoid 'io' errors)
        Dim luaCmd As String = vbCrLf &
                               $"print('\n{READ_START_TOKEN}'); " &
                               $"if file.open('{targetFilename}', 'r') then " &
                               "repeat local l = file.readline() if l then uart.write(0,l) end until not l " &
                               "file.close() " &
                               "else print('ERROR_NOT_FOUND') end;"

        ModGlobals.SerialEngine.Send(luaCmd)
    End Sub


    ''' <summary>
    ''' STRICT PARSER: Iterates lines to distinguish between "Command Echo" and "Real Output".
    ''' </summary>
    Private Sub ProcessCapturedFile(rawContent As String)
        Try
            ' 1. Split into lines
            Dim lines As String() = rawContent.Replace(vbCr, "").Split(vbLf)

            Dim payloadBuilder As New System.Text.StringBuilder
            Dim isRecording As Boolean = False
            Dim foundStart As Boolean = False

            For Each line As String In lines
                Dim cleanLine As String = line.Trim()

                ' 2. STOP RECORDING if we hit the prompt
                ' (We check for the prompt > at the start or end of the line)
                If isRecording AndAlso cleanLine.EndsWith(">") Then
                    ' If the prompt is on the same line as code (rare but possible), strip it
                    Dim contentLine As String = line.TrimEnd().TrimEnd(">"c)
                    If Not String.IsNullOrEmpty(contentLine) Then payloadBuilder.AppendLine(contentLine)
                    Exit For
                End If

                ' 3. RECORDING PHASE
                If isRecording Then
                    payloadBuilder.AppendLine(line)
                End If

                ' 4. START TRIGGER
                ' We strictly match the token. 
                ' The echo line looks like: > print('###START###')... -> We ignore this.
                ' The output line looks like: ###START### -> We accept this.
                If cleanLine = READ_START_TOKEN Then
                    isRecording = True
                    foundStart = True
                End If
            Next

            ' 5. Process Result
            If foundStart Then
                Dim finalContent As String = payloadBuilder.ToString().Trim()

                ' Check for the specific Lua error string we programmed
                If finalContent.Contains("ERROR_NOT_FOUND") Then
                    rtbxTerminal.AppendText(vbCrLf & "--- Device Reported: File not found ---" & vbCrLf)
                Else
                    AddNewTab(_CaptureFilename, finalContent, "")
                    lblWorkingFile.Text = "Downloaded: " & _CaptureFilename
                    rtbxTerminal.AppendText(vbCrLf & "--- Read Successful ---" & vbCrLf)
                End If
            Else
                ' Fallback: If we missed the token, dump raw data to help debug
                rtbxTerminal.AppendText(vbCrLf & "--- Parser Error: Start Token missed. Raw Data below: ---" & vbCrLf)
                rtbxTerminal.AppendText(rawContent & vbCrLf)
            End If

        Catch ex As Exception
            MessageBox.Show("Error parsing file: " & ex.Message)
        Finally
            _IsCapturingRead = False
            Cursor = Cursors.Default
            lblStatusSquare.BackColor = Color.LimeGreen
        End Try
    End Sub

    ' =============================================================================
    ' 8. TAB CONTEXT MENU SYSTEM (Right-Click on Tabs)
    ' =============================================================================

    ''' <summary>
    ''' DETECTS RIGHT-CLICK on a specific tab header and shows the menu.
    ''' </summary>
    Private Sub tcCodeEditors_MouseUp(sender As Object, e As MouseEventArgs) Handles tcCodeEditors.MouseUp
        If e.Button = MouseButtons.Right Then
            ' 1. Hit Test: Find which tab was clicked
            For i As Integer = 0 To tcCodeEditors.TabCount - 1
                Dim r As Rectangle = tcCodeEditors.GetTabRect(i)
                If r.Contains(e.Location) Then
                    ' 2. Select the tab that was clicked (Standard UI behavior)
                    tcCodeEditors.SelectedIndex = i
                    Dim targetTab As TabPage = tcCodeEditors.TabPages(i)

                    ' Don't show menu for the "+" tab
                    If targetTab Is _PlusTab Then Return

                    ' 3. Build and Show the Menu
                    ShowTabContextMenu(targetTab, e.Location)
                    Return
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Dynamic Menu Builder for Tabs
    ''' </summary>
    Private Sub ShowTabContextMenu(targetTab As TabPage, location As Point)
        Dim ctx As New ContextMenuStrip()
        Dim hasFile As Boolean = Not String.IsNullOrEmpty(targetTab.Tag.ToString())

        ' --- FILE ACTIONS ---
        ctx.Items.Add("Save", Nothing, Sub() SaveCurrentFile())
        ctx.Items.Add("Save As...", Nothing, Sub() SaveAsFile())

        ctx.Items.Add(New ToolStripSeparator())

        ' --- TAB MANAGEMENT ---
        ctx.Items.Add("Close", Nothing, Sub() CloseTab(targetTab))

        Dim itemCloseOthers As New ToolStripMenuItem("Close All But This")
        AddHandler itemCloseOthers.Click, Sub() CloseAllTabsBut(targetTab)
        ctx.Items.Add(itemCloseOthers)

        ctx.Items.Add(New ToolStripSeparator())

        ' --- SYSTEM ACTIONS ---
        Dim itemCopyPath As New ToolStripMenuItem("Copy Full Path")
        itemCopyPath.Enabled = hasFile
        AddHandler itemCopyPath.Click, Sub() Clipboard.SetText(targetTab.Tag.ToString())
        ctx.Items.Add(itemCopyPath)

        Dim itemOpenFolder As New ToolStripMenuItem("Open Containing Folder")
        itemOpenFolder.Enabled = hasFile
        AddHandler itemOpenFolder.Click, Sub()
                                             If hasFile Then
                                                 Process.Start("explorer.exe", "/select," & targetTab.Tag.ToString())
                                             End If
                                         End Sub
        ctx.Items.Add(itemOpenFolder)

        ' Show the menu at the mouse position
        ctx.Show(tcCodeEditors, location)
    End Sub

    ''' <summary>
    ''' Closes every tab except the specific one (and keeps the + tab).
    ''' </summary>
    Private Sub CloseAllTabsBut(keepTab As TabPage)
        ' Loop backwards to avoid index shifting issues
        For i As Integer = tcCodeEditors.TabPages.Count - 1 To 0 Step -1
            Dim t As TabPage = tcCodeEditors.TabPages(i)

            ' Skip the tab we want to keep AND the "+" tab
            If t IsNot keepTab AndAlso t IsNot _PlusTab Then
                ' Check dirty state logic inside CloseTab
                CloseTab(t)
            End If
        Next
    End Sub
    ''' <summary>
    ''' Cleans Lua code by removing comments (--...) while preserving strings ("--").
    ''' </summary>
    Private Function SanitizeLuaCode(rawCode As String) As String
        Dim sb As New System.Text.StringBuilder
        Dim lines = rawCode.Split(New String() {vbCrLf, vbLf}, StringSplitOptions.None)

        For Each line As String In lines
            Dim cleanLine As String = line
            Dim inString As Boolean = False
            Dim quoteChar As Char = Nothing
            Dim commentIndex As Integer = -1

            ' Parse line char by char to find REAL comments (ignoring -- inside quotes)
            For i As Integer = 0 To line.Length - 1
                Dim c As Char = line(i)

                If inString Then
                    ' Check for closing quote (handle escaped quotes \" or \')
                    If c = quoteChar Then
                        If i = 0 OrElse line(i - 1) <> "\"c Then inString = False
                    End If
                Else
                    If c = """"c OrElse c = "'"c Then
                        inString = True
                        quoteChar = c
                    ElseIf c = "-"c AndAlso i < line.Length - 1 AndAlso line(i + 1) = "-"c Then
                        ' Found the start of a comment
                        commentIndex = i
                        Exit For
                    End If
                End If
            Next

            ' Cut off the comment if found
            If commentIndex >= 0 Then
                cleanLine = line.Substring(0, commentIndex)
            End If

            ' Only add non-empty lines
            If Not String.IsNullOrWhiteSpace(cleanLine) Then
                sb.AppendLine(cleanLine)
            End If
        Next

        Return sb.ToString().TrimEnd()
    End Function


    ''' <summary>
    ''' UNCOMMENTER: Removes leading "--" from lines so they execute.
    ''' Example: "-- print('Test')" becomes "print('Test')"
    ''' </summary>
    Private Function UncommentForExecution(rawCode As String) As String
        Dim sb As New System.Text.StringBuilder
        Dim lines = rawCode.Split(New String() {vbCrLf, vbLf}, StringSplitOptions.None)

        For Each line As String In lines
            ' Use Regex to find "--" at the start of the line (ignoring leading spaces)
            ' Pattern: ^ (Start) \s* (Any spaces) -- (The comment)
            Dim processedLine As String = System.Text.RegularExpressions.Regex.Replace(line, "^\s*--", "")

            If Not String.IsNullOrWhiteSpace(processedLine) Then
                sb.AppendLine(processedLine)
            End If
        Next

        Return sb.ToString().TrimEnd()
    End Function

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    ' =============================================================================
    ' SEARCH SYSTEM
    ' =============================================================================
    Private Sub btnSearchOnCode_Click(sender As Object, e As EventArgs) Handles btnSearchOnCode.Click
        Dim editor = GetActiveEditor()
        If editor Is Nothing Then Exit Sub

        Dim term As String = txtSeacrhTerm.Text
        If String.IsNullOrEmpty(term) Then Return

        ' 1. Add to Memory (Mnemonic)
        ' We add it to the autocomplete list so it appears next time you type
        If Not txtSeacrhTerm.AutoCompleteCustomSource.Contains(term) Then
            txtSeacrhTerm.AutoCompleteCustomSource.Add(term)
        End If

        ' 2. Determine Start Position
        ' If text is already selected (e.g. previous find), start searching AFTER it
        ' otherwise start from the cursor
        Dim startPos As Integer = editor.SelectionStart + editor.SelectionLength

        ' 3. Execute Search (Finds text from current position to End)
        Dim foundIndex As Integer = editor.Find(term, startPos, RichTextBoxFinds.None)

        ' 4. Handle Results
        If foundIndex >= 0 Then
            ' --- FOUND ---
            editor.Select(foundIndex, term.Length)
            editor.ScrollToCaret()
            editor.Focus() ' Give focus back to editor so you can see the blue highlight
        Else
            ' --- NOT FOUND (Possible Wrap Around) ---
            ' If we started partway through, try searching from the top
            If startPos > 0 Then
                foundIndex = editor.Find(term, 0, startPos, RichTextBoxFinds.None)
                If foundIndex >= 0 Then
                    editor.Select(foundIndex, term.Length)
                    editor.ScrollToCaret()
                    editor.Focus()
                    Return
                End If
            End If

            ' --- TRULY NOT FOUND ---
            MessageBox.Show($"Text '{term}' not found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' Optional: Trigger search when pressing Enter in the box
    Private Sub txtSeacrhTerm_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSeacrhTerm.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnSearchOnCode.PerformClick()
            ' Suppress the 'Ding' sound
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub RestoreSessionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestoreSessionToolStripMenuItem.Click
        RestoreSessionToolStripMenuItem.Checked = Not RestoreSessionToolStripMenuItem.Checked
        _RestoreSession = RestoreSessionToolStripMenuItem.Checked
        ' We don't save config immediately here, it saves on close.
    End Sub

    Private Sub lnkClearResponse_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkClearResponse.LinkClicked
        ' Works even if terminal is floating, because rtbxTerminal is the same object
        rtbxTerminal.Clear()
    End Sub

    Private Sub CloseAllFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseAllFilesToolStripMenuItem.Click
        ' Loop backwards to safely remove items while iterating
        For i As Integer = tcCodeEditors.TabPages.Count - 1 To 0 Step -1
            Dim t As TabPage = tcCodeEditors.TabPages(i)

            ' Skip the plus tab
            If t Is _PlusTab Then Continue For

            ' Attempt to close. If user hits Cancel, STOP closing the rest.
            If Not CloseTab(t) Then
                Exit For
            End If
        Next
    End Sub

    Private Sub lblWorkingFile_Click(sender As Object, e As EventArgs) Handles lblWorkingFile.Click
        Dim filePath As String = lblWorkingFile.Text

        ' Validations: ensure file exists and isn't the placeholder text
        If String.IsNullOrEmpty(filePath) OrElse filePath = "Unsaved File" OrElse Not File.Exists(filePath) Then
            Return
        End If

        ' 1. CTRL + CLICK -> Open Containing Folder
        If (Control.ModifierKeys And Keys.Control) = Keys.Control Then
            ' We wrap path in quotes here too just to be safe
            Process.Start("explorer.exe", "/select,""" & filePath & """")
            Return
        End If

        ' 2. NORMAL CLICK -> Open in Notepad++ (or Default)
        Dim nppPath As String = ""
        Dim possiblePaths As String() = {
            "C:\Program Files\Notepad++\notepad++.exe",
            "C:\Program Files (x86)\Notepad++\notepad++.exe"
        }

        For Each p In possiblePaths
            If File.Exists(p) Then
                nppPath = p
                Exit For
            End If
        Next

        Try
            If Not String.IsNullOrEmpty(nppPath) Then
                ' FIX: Wrap the path in double quotes so spaces don't break it
                Process.Start(nppPath, """" & filePath & """")
            Else
                ' Fallback to system default editor
                Process.Start(New ProcessStartInfo(filePath) With {.UseShellExecute = True})
            End If
        Catch ex As Exception
            MessageBox.Show("Could not open file: " & ex.Message)
        End Try
    End Sub

    Private Sub DefaultToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DefaultToolStripMenuItem.Click
        ApplyTheme("Default")
    End Sub

    Private Sub DarkMatterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DarkMatterToolStripMenuItem.Click
        ApplyTheme("DarkMatter")
    End Sub

    Private Sub BeehiveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BeehiveToolStripMenuItem.Click
        ApplyTheme("Beehive")
    End Sub

    Private Sub ThinkGreenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ThinkGreenToolStripMenuItem.Click
        ApplyTheme("Thin Green")
    End Sub

    Private Sub IntoTheLightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IntoTheLightToolStripMenuItem.Click
        ApplyTheme("Into The Light")
    End Sub

    Private Sub MeanOrangeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MeanOrangeToolStripMenuItem.Click
        ApplyTheme("Mean Orange")
    End Sub

    Private Sub HappyDayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HappyDayToolStripMenuItem.Click
        ApplyTheme("Happy Day")
    End Sub

    Private Sub SoLimboToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SoLimboToolStripMenuItem.Click
        ApplyTheme("So Limbo")
    End Sub

    Private Sub OldCoffeeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OldCoffeeToolStripMenuItem.Click
        ApplyTheme("Old Coffee")
    End Sub

    Private Sub PaintedStaiwayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PaintedStaiwayToolStripMenuItem.Click
        ApplyTheme("Painted Stairway")
    End Sub

    ' =============================================================================
    ' THEME CYCLING LOGIC
    ' =============================================================================

    Private Sub CycleTheme(direction As Integer)
        ' 1. Find current index
        Dim currentIndex As Integer = ModThemes.ThemeList.IndexOf(_CurrentThemeName)

        ' 2. Calculate new index (Direction: 1 for Next, -1 for Prev)
        Dim newIndex As Integer = currentIndex + direction

        ' 3. Handle Wrap-Around
        If newIndex >= ModThemes.ThemeList.Count Then
            newIndex = 0 ' Loop back to start
        ElseIf newIndex < 0 Then
            newIndex = ModThemes.ThemeList.Count - 1 ' Loop to end
        End If

        ' 4. Apply
        ApplyTheme(ModThemes.ThemeList(newIndex))
    End Sub

    Private Sub NextThemeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NextThemeToolStripMenuItem.Click
        CycleTheme(1)
    End Sub

    Private Sub PrevToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrevToolStripMenuItem.Click
        CycleTheme(-1)
    End Sub

    ' Click Label to Switch Theme
    Private Sub lblCurrentTheme_Click(sender As Object, e As EventArgs) Handles lblCurrentTheme.Click
        CycleTheme(1) ' Treat click as "Next"
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        frmAbout.ShowDialog()
    End Sub

    Private Sub VisitWebsiteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VisitWebsiteToolStripMenuItem.Click
        Try
            Process.Start("http://georgousis.info")
        Catch ex As Exception

        End Try

    End Sub

    Private Sub VisitGithubRepoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VisitGithubRepoToolStripMenuItem.Click
        Try
            Process.Start("https://github.com/limbo666/Selene")
        Catch ex As Exception

        End Try


    End Sub

    Private Sub FirmwareFlasherToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FirmwareFlasherToolStripMenuItem.Click
        frmFlash.Show()
    End Sub
End Class