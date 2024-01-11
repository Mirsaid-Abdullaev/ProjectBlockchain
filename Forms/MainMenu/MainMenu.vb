Public Class MainMenu

    Private Sub GetWalletView_Click(sender As Object, e As EventArgs) Handles GetWalletView.Click
        Me.Hide()
        WalletBaseView.ShowDialog()
        Me.StatusLbl.Text = GlobalData.StatusLblText
        Try
            Me.Show()
        Catch ex As Exception
        End Try
        GC.Collect()
    End Sub

    Private Sub MainMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, MainMenuColours)
        Me.StatusLbl.Text = GlobalData.StatusLblText
        GC.Collect()
    End Sub

    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        GlobalData.AppRunning = False
        'SendDisconnectedJSONToPrevPtr - add when configured
        Application.Exit()
    End Sub

    Private Sub GetSendingView_Click(sender As Object, e As EventArgs) Handles GetSendingView.Click
        Me.Hide()
        SendingScreen.ShowDialog()
        Me.StatusLbl.Text = GlobalData.StatusLblText
        Try
            Me.Show()
        Catch ex As Exception
        End Try
        GC.Collect()
    End Sub

    Private Sub GetTransactionPool_Click(sender As Object, e As EventArgs) Handles GetTransactionPool.Click
        Me.Hide()
        TransactPoolView.ShowDialog()
        Me.StatusLbl.Text = GlobalData.StatusLblText
        Try
            Me.Show()
        Catch ex As Exception
        End Try
        GC.Collect()
    End Sub

    Private Sub GetViewingForm_Click(sender As Object, e As EventArgs) Handles GetViewingForm.Click
        Me.Hide()
        BaseViewingForm.ShowDialog()
        Me.StatusLbl.Text = GlobalData.StatusLblText
        Try
            Me.Show()
        Catch ex As Exception
        End Try
        GC.Collect()
    End Sub
End Class