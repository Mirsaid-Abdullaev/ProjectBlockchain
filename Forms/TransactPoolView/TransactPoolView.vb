Imports System.Threading
Public Class TransactPoolView
    Private UpdateTransactPool As Thread
    Private FormExited As Boolean = False
    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        Me.DialogResult = DialogResult.OK
        FormExited = True
        Me.Close()
        GC.Collect()
    End Sub

    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        GlobalData.AppRunning = False
        'SendDisconnectedJSONToPrevPtr - add when configured
        Application.Exit()
    End Sub

    Private Sub TransactPoolView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.StatusLbl.Text = GlobalData.StatusLblText
        DesignLoad(Me, TransactPoolColours)
        UpdateTransactPool = New Thread(AddressOf UpdateTxt)
        UpdateTransactPool.Start()
    End Sub

    Private Sub UpdateTxt()
        While Not Me.FormExited
            'Me.TransactTxt.Text = GlobalData.TransactionPool.GetContents()
        End While
        Exit Sub
    End Sub
End Class