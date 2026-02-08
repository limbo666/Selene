Imports System.IO

Module ModGlobals
    ' The Heart of the Application: Single Global Serial Instance
    Public SerialEngine As New clsMicroSerial

    ' Application State Flags
    Public GlobalAppTheme As Integer = 1
    Public WorkingFilePath As String = ""
    Public IsConnected As Boolean = False

    ' System Paths (Dynamic detection)
    Public AppPath As String = Application.StartupPath
    Public UserSnippetsPath As String = Path.Combine(AppPath, "snippets")

    ' Configuration Constants
    Public Const DEFAULT_BAUD As Integer = 115200

    ' Helper to log debug messages to the Output window (Professional Debugging)
    Public Sub LogDebug(msg As String)
        System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {msg}")
    End Sub
End Module