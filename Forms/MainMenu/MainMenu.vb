Public Class MainMenu

    Private Sub GetWalletView_Click(sender As Object, e As EventArgs) Handles GetWalletView.Click
        Me.Hide()
        Dim WalView As New WalletBaseView
        WalView.ShowDialog()
        Me.StatusLbl.Text = SetSharedLblText()
        Try
            Me.Show()
        Catch ex As Exception
        End Try
        GC.Collect()
    End Sub

    Private Sub MainMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, MainMenuColours)
        Me.StatusLbl.Text = SetSharedLblText()
        GC.Collect()
    End Sub

    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        AppRunning = False
        DisconnectFromChain()
        Application.Exit()
    End Sub

    Private Sub GetSendingView_Click(sender As Object, e As EventArgs) Handles GetSendingView.Click
        If CurrentWallet Is Nothing Then
            CustomMsgBox.ShowBox("Error: no wallet logged in. Cannot send cryptocurrency if there is no source address. Log into a wallet and try again.", "ERROR", False)
            Exit Sub
        End If
        Me.Hide()
        Dim SendScr As New SendingScreen
        SendScr.ShowDialog()
        Me.StatusLbl.Text = SetSharedLblText()
        Try
            Me.Show()
        Catch ex As Exception
        End Try
        GC.Collect()
    End Sub

    Private Sub GetTransactionPool_Click(sender As Object, e As EventArgs) Handles GetTransactionPool.Click
        Me.Hide()
        Dim TransPoolView As New TransactPoolView
        TransPoolView.ShowDialog()
        Me.StatusLbl.Text = SetSharedLblText()
        Try
            Me.Show()
        Catch ex As Exception
        End Try
        GC.Collect()
    End Sub

    Private Sub GetViewingForm_Click(sender As Object, e As EventArgs) Handles GetViewingForm.Click
        Me.Hide()
        Dim BaseFrm As New BaseViewingForm
        BaseFrm.ShowDialog()
        Me.StatusLbl.Text = SetSharedLblText()
        Try
            Me.Show()
        Catch ex As Exception
        End Try
        GC.Collect()
    End Sub
End Class