Imports System.IO
Public Class SyncForm
    Private Sub SyncForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, MainMenuColours)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        For i As Integer = 1 To 100
            ProgressBar1.Value += 1
        Next 'in reality, sync procedure will happen here, this is a placeholder
        FileSystem.Initialise()
        Me.Close()
    End Sub
End Class
