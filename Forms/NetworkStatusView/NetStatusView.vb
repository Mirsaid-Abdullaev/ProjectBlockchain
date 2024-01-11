Public Class NetStatusView
    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
        GC.Collect()
    End Sub

    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        GlobalData.AppRunning = False
        'SendDisconnectedJSONToPrevPtr - add when configured
        Application.Exit()
    End Sub

    Private Sub NetStatusView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, BlockchainExpColours)
        Me.StatusLbl.Text = GlobalData.StatusLblText
    End Sub
End Class