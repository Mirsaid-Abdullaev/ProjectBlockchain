Public Class BaseViewingForm
    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
        GC.Collect()
    End Sub

    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        AppRunning = False
        DisconnectFromChain()
        Application.Exit()
    End Sub

    Private Sub BlockchainExpF_Click(sender As Object, e As EventArgs) Handles BlockchainExpF.Click
        Me.Hide()
        BlockchainExplorerF.ShowDialog()
        Me.StatusLbl.Text = StatusLblText
        Try
            Me.Show()
        Catch ex As Exception
            Me.Close()
        End Try
        GC.Collect()
    End Sub

    Private Sub BaseViewingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, BlockchainExpColours)
        Me.StatusLbl.Text = StatusLblText
        GC.Collect()
    End Sub

    Private Sub BlockchainExpH_Click(sender As Object, e As EventArgs) Handles BlockchainExpH.Click
        Me.Hide()
        BlockchainExplorerH.ShowDialog()
        Me.StatusLbl.Text = StatusLblText
        Try
            Me.Show()
        Catch ex As Exception
            Me.Close()
        End Try
        GC.Collect()
    End Sub

    Private Sub NetworkStatusView_Click(sender As Object, e As EventArgs) Handles NetworkStatusView.Click
        Me.Hide()
        NetStatusView.ShowDialog()
        Me.StatusLbl.Text = StatusLblText
        Try
            Me.Show()
        Catch ex As Exception
            Me.Close()
        End Try
        GC.Collect()
    End Sub
End Class