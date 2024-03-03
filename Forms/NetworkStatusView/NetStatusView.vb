Public Class NetStatusView
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

    Private Sub NetStatusView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, BlockchainExpColours)
        Me.StatusLbl.Text = StatusLblText
        Dim Role As String = "INIT"
        If IS_ROOT Then
            Role = "ROOT"
        ElseIf Not IsSynchronised Then
            Role = "NEW"
        ElseIf IsLeaf Then
            Role = "LEAF"
        ElseIf IsSynchronised And PreviousPointer IsNot Nothing And NextPointer IsNot Nothing Then
            Role = "BRANCH"
        End If

        Dim Text As String = $"PREVPTR: ""{PreviousPointer}""{vbCrLf}NEXTPTR: ""{NextPointer}""{vbCrLf}NODETYPE: ""{Role}""{vbCrLf}IPADDRESS: ""{GetOwnIP()}""{vbCrLf}ONLINE: ""{If(IsSynchronised, "TRUE", "FALSE")}"""
        Me.NetStatusTxt.Text = Text
    End Sub

End Class