Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class TestForm
    Public Sub UpdateStatusTxt(ByRef text As String)
        txtstatus.Text &= text & vbCrLf & vbCrLf
    End Sub


    Private Sub BeginTest_Click(sender As Object, e As EventArgs) Handles BeginTest.Click


    End Sub


End Class