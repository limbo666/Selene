Public Class frmResponse



    ' --- 1. RESTORE POSITION ON LOAD ---
    Private Sub frmResponse_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Apply the saved rectangle from frmMain
        Me.StartPosition = FormStartPosition.Manual
        Me.Bounds = frmMain._RespRect

        ' Safety check: If it's off-screen, reset to default
        If Not Screen.GetWorkingArea(Me).IntersectsWith(Me.Bounds) Then
            Me.Location = New Point(100, 100)
            Me.Size = New Size(600, 400)
        End If

        ' Apply the current theme (Optional, ensures it matches immediately)
        frmMain.ApplyTheme(frmMain.lblCurrentTheme.Text)
    End Sub

    ' --- 2. SAVE POSITION ON CLOSE ---
    Private Sub frmResponse_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If e.CloseReason = CloseReason.UserClosing Then
            ' CRITICAL FIX: Do NOT set e.Cancel = True.
            ' Just trigger the reattach logic. The main form will grab the controls,
            ' and then this form will close naturally.

            If Me.Owner IsNot Nothing AndAlso TypeOf Me.Owner Is frmMain Then
                DirectCast(Me.Owner, frmMain).ReattachTerminal()
            End If
        End If
        ' Update the variable in frmMain with our current size/pos
        If Me.WindowState = FormWindowState.Normal Then
            frmMain._RespRect = Me.Bounds
        Else
            frmMain._RespRect = Me.RestoreBounds
        End If

        ' Trigger the Save immediately
        frmMain.SaveConfig()
    End Sub
End Class