Imports System.Threading
Public Class TransactPoolView
    Private UpdateTransactPool As Thread
    Private FormExited As Boolean
    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        FormExited = True
        Me.DialogResult = DialogResult.OK
        Me.Close()
        GC.Collect()
    End Sub

    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        AppRunning = False
        'SendDisconnectedJSONToPrevPtr - add when configured
        Application.Exit()
    End Sub

    Private Sub TransactPoolView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.StatusLbl.Text = StatusLblText
        Me.FormExited = False
        DesignLoad(Me, TransactPoolColours)
        UpdateTransactPool = New Thread(AddressOf UpdateTxt) With {.IsBackground = True}
        UpdateTransactPool.Start()
        CheckForIllegalCrossThreadCalls = False
    End Sub

    Private Sub UpdateTxt()
        While Not Me.FormExited
            Try
                Me.TransactTxt.Text = WFTransactionPool.GetTransactionListAsString()
            Catch ex As Exception
            End Try
            Thread.Sleep(2000)
        End While
        CheckForIllegalCrossThreadCalls = True
        Exit Sub
    End Sub

    Private Sub TransactPoolView_Activated(sender As Object, e As EventArgs) Handles Me.Click
        While Not CustomMsgBox.ShowBox("Starting to modify transaction pool...", "THREAD STARTED", False)
            Continue While
        End While

        WFTransactionPool.FlushPool()
        For i As Byte = 1 To 10
            Dim randsender As String = StrDup(i, "a")
            Dim randrecipient As String = StrDup(i, "b")
            Dim transact As New Transaction(randsender, randrecipient, i * 3.866)
            WFTransactionPool.AddTransaction(transact)
        Next
    End Sub

    Private Sub TransactPoolView_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub
End Class