Public Class SendingScreen
    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        AppRunning = False
        'SendDisconnectedJSONToPrevPtr - add when configured
        Application.Exit()
    End Sub

    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
        GC.Collect()
    End Sub

    Private Sub SendingScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, SendingScreenColours)
        Me.StatusLbl.Text = StatusLblText
    End Sub

    Private Sub CheckConfirm_Click(sender As Object, e As EventArgs) Handles CheckConfirm.Click
        Dim Choice As Boolean = CustomMsgBox.ShowBox("This is your final chance to check any incorrect details before an irreversible transaction is sent to the blockchain. Click cancel to delete transaction." &
             vbCrLf & vbCrLf & $"Sending amount: {SendingAmountTxt.Text}" & vbCrLf & $"Recipient address: {RecipientAddressTxt.Text}", "TRANSACTION CONFIRMATION")
        If Not Choice Then
            CustomMsgBox.ShowBox("Cancelled user transaction. Thank you.", "TRANSACTION CANCEL", False)
            Exit Sub
        End If
        'SendTransaction(amount, address) to implement this
        RecipientAddressTxt.Clear()
        SendingAmountTxt.Clear()
    End Sub
End Class