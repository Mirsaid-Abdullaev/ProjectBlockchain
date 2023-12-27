Imports System.Numerics

Public Class TestForm
    Public Sub UpdateStatusTxt(ByRef text As String)
        txtstatus.Text &= text & vbCrLf
    End Sub


    Private Sub BeginTest_Click(sender As Object, e As EventArgs) Handles BeginTest.Click
        'FileSystem.Initialise()
        'UpdateStatusTxt("File at Wayfarer Directory successfully created.")
        'Dim wallet As New Wallet("testwallet")
        For Each ctl As Control In Me.Controls.OfType(Of Button)
            SetControlGradient(ctl, ButtonColours)
        Next
    End Sub

    Private Sub TestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'For Each ctl As Control In Me.Controls.OfType(Of Button)
        '    SetControlGradient(ctl, colors1)
        'Next
    End Sub
End Class