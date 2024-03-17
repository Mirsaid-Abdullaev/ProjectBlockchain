Public Class SendingScreen
    Private SendingAmount As Double
    Private Recipient As String
    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        AppRunning = False
        DisconnectFromChain()
        Application.Exit()
    End Sub

    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
        Me.Dispose()
        GC.Collect()
    End Sub

    Private Sub SendingScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, SendingScreenColours)
        Me.StatusLbl.Text = SetSharedLblText()
        Me.TransactionStatus.Text = If(UnvalidatedTransaction Is Nothing, "No pending transactions, clear to send.", $"Currently 1 pending transaction: {UnvalidatedTransaction}")
        Dim UpdatedStatus As String = If(ReachedMiningSeq, $"Unable to currently send transaction, in mining sequence.", Me.TransactionStatus.Text)
        Me.TransactionStatus.Text = UpdatedStatus
    End Sub

    Private Sub CheckConfirm_Click(sender As Object, e As EventArgs) Handles CheckConfirm.Click
        If CurrentWallet Is Nothing Then
            CustomMsgBox.ShowBox("Error: no wallet logged in, please log into a wallet and try again.", "ERROR", False)
            Me.Close()
            Me.Dispose()
            GC.Collect()
        End If
        'clears all input and exits the form if no wallet logged in - should not happen as this is validated before we enter the form in MainMenu

        If SendingAmount = Nothing OrElse Recipient Is Nothing Then 'checking for null values in the input fields
            CustomMsgBox.ShowBox("Error: not all the fields were filled in. Try again.", "ERROR", False)
            SendingAmount = Nothing
            Recipient = Nothing
            AddressInputTxt.Clear()
            SendingAmountTxt.Clear()
            Exit Sub
        End If

        'now address and amount are validated
        Dim Fee As Double = 0.05 * SendingAmount

        Dim Choice As Boolean = CustomMsgBox.ShowBox("This is your final chance to check any incorrect details before an irreversible transaction is sent to the blockchain. Click CANCEL to delete transaction." &
             vbCrLf & vbCrLf & $"Sending amount: {SendingAmount * 0.95}" & vbCrLf & $"Miner's fee: 5% of sending amount: {Fee}" & vbCrLf & $"Recipient address: {Recipient}", "TRANSACTION CONFIRMATION", )
        If Not Choice Then
            CustomMsgBox.ShowBox("Cancelled user transaction. Thank you.", "TRANSACTION CANCEL", False)
            SendingAmount = Nothing
            Recipient = Nothing
            AddressInputTxt.Clear()
            SendingAmountTxt.Clear()
            Exit Sub
        End If
        Dim Transact As New Transaction(CurrentWallet.GetPublicAddress, Recipient, SendingAmount * 0.95, Fee)
        SendTransaction(Transact)

        AddressInputTxt.Clear()
        SendingAmountTxt.Clear()
        SendingAmount = Nothing
        Recipient = Nothing

        Me.TransactionStatus.Text = If(UnvalidatedTransaction Is Nothing, "No pending transactions, clear to send.", $"Currently 1 pending transaction: {UnvalidatedTransaction}")
        Dim UpdatedStatus As String = If(ReachedMiningSeq, $"Unable to currently send transaction, in mining sequence.", Me.TransactionStatus.Text)
        Me.TransactionStatus.Text = UpdatedStatus
    End Sub

    Private Sub SendTransaction(Transact As Transaction)
        Dim NewTransReq As New NewTransactionRequest(Transact)
        If Not (IsSynchronised And Not ReachedMiningSeq) Then 'device needs to be synced and not in the mining sequence in order to send a transaction
            CustomMsgBox.ShowBox("Error: device is not connected to the network or mining is taking place, unable to send this transaction to the network. Try again - transaction processing aborted, no charge taken.", "ERROR", False)
            Exit Sub
        End If
        If UnvalidatedTransaction IsNot Nothing Then
            'theres already a transaction being sent across, wait
            CustomMsgBox.ShowBox("Error: another transaction is waiting for confirmations. Try again later - transaction processing aborted, no charge taken.", "ERROR", False)
            Exit Sub
        End If
        SendToBothNodes(NewTransReq.GetJSONMessage()) 'sending to both nodes whichever are connected
        UnvalidatedTransaction = Transact
        ReceivedTransactionConfirmations = 0
        If IsRoot OrElse IsLeaf Then
            'has no prevptr/nextptr so can add one confirmation already
            ReceivedTransactionConfirmations = 1
        End If
        CustomMsgBox.ShowBox($"Transaction has been successfully sent to the outbound buffer. Details: {Transact}", "SUCCESSFUL TRANSACT", False)
        Me.TransactionStatus.Text = If(UnvalidatedTransaction Is Nothing, "No pending transactions, clear to send.", $"Currently 1 pending transaction: {UnvalidatedTransaction}")
        Dim UpdatedStatus As String = If(ReachedMiningSeq, $"Unable to currently send transaction, in mining sequence.", Me.TransactionStatus.Text)
        Me.TransactionStatus.Text = UpdatedStatus
    End Sub

    Private Sub AddressInput_Click(sender As Object, e As EventArgs) Handles AddressInput.Click 'validates user input of the address value
        Recipient = Nothing
        Recipient = CustomInputBox.ShowInputBox("PLEASE ENTER THE PUBLIC ADDRESS, WITH ALL LETTERS CAPITALISED, OTHERWISE YOU WILL LOSE FUNDS", "SENDING ADDRESS", False)(0)
        If Not IsValidHexStr(Recipient, 64) Then
            CustomMsgBox.ShowBox("Error: recipient address you inputted was invalid, enter a valid address (64 digit hex string, uppercase alpha characters)", "ERROR", False)
            Recipient = Nothing
            Exit Sub
        End If
        AddressInputTxt.Text = Recipient
        Me.TransactionStatus.Text = If(UnvalidatedTransaction Is Nothing, "No pending transactions, clear to send.", $"Currently 1 pending transaction: {UnvalidatedTransaction}")
        Dim UpdatedStatus As String = If(ReachedMiningSeq, $"Unable to currently send transaction, in mining sequence.", Me.TransactionStatus.Text)
        Me.TransactionStatus.Text = UpdatedStatus
    End Sub


    Private Sub AmountInput_Click(sender As Object, e As EventArgs) Handles AmountInput.Click 'validates user input of the amount value
        SendingAmount = Nothing

        Try
            SendingAmount = CDbl(CustomInputBox.ShowInputBox("ENTER THE AMOUNT YOU WISH TO SEND, AS A REAL NUMBER LARGER THAN 0 AND LESS THAN/EQUAL TO CURRENT BALANCE", "SENDING AMOUNT", False)(0))
            If SendingAmount <= 0 Then
                Throw New Exception
            End If
        Catch ex As Exception
            CustomMsgBox.ShowBox("Error: input of the amount was invalid, enter a valid quantity (a valid real number > 0 and < current wallet balance)", "ERROR", False)
            SendingAmount = Nothing
            Exit Sub
        End Try

        If CurrentWallet Is Nothing Then
            CustomMsgBox.ShowBox("Error: no wallet logged in, try again after logging in", "ERROR", False)
            Exit Sub
        End If

        If CurrentWallet.GetWalletBalance < SendingAmount Then
            CustomMsgBox.ShowBox("Error: amount you inputted was more than the balance of your current wallet, either log into a different wallet, or enter a valid quantity (a valid real number less than or equal to your balance.)", "ERROR", False)
            SendingAmount = Nothing
            Exit Sub
        End If

        SendingAmountTxt.Text = SendingAmount.ToString
        Me.TransactionStatus.Text = If(UnvalidatedTransaction Is Nothing, "No pending transactions, clear to send.", $"Currently 1 pending transaction: {UnvalidatedTransaction}")
        Dim UpdatedStatus As String = If(ReachedMiningSeq, $"Unable to currently send transaction, in mining sequence.", Me.TransactionStatus.Text)
        Me.TransactionStatus.Text = UpdatedStatus

    End Sub

    Private Sub GetHelpManual_Click(sender As Object, e As EventArgs) Handles GetHelpManual.Click
        Return 'not implemented
    End Sub
End Class