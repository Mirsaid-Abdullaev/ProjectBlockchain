Imports System.IO
Imports System.Threading
Public Class SyncForm
    Private Sub SyncForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, MainMenuColours)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Synchronise() 'performs the full sync procedure
        Me.Close()
    End Sub
End Class
