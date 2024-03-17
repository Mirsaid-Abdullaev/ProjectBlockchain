Public Class NetStatusView
    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
        Me.Dispose()
        GC.Collect()
    End Sub

    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        AppRunning = False
        DisconnectFromChain()
        Application.Exit()
    End Sub

    Private Sub NetStatusView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, BlockchainExpColours)
        Me.StatusLbl.Text = SetSharedLblText()
        Dim Role As String = "OFFLINE"
        If IsSynchronised Then
            If IsRoot Then
                Role = "ROOT"
            ElseIf IsLeaf Then
                Role = "LEAF"
            Else
                Role = "BRANCH"
            End If
        End If

        Dim Text As String = $"PREVPTR: ""{If(PrevTCP IsNot Nothing, PrevTCP.ToString(), "NULL")}""{vbCrLf}NEXTPTR: ""{If(NextTCP IsNot Nothing, NextTCP.ToString(), "NULL")}""{vbCrLf}NODETYPE: ""{Role}""{vbCrLf}IPADDRESS: ""{GetOwnIP()}""{vbCrLf}ONLINE: ""{If(IsSynchronised, "TRUE", "FALSE")}"""
        Me.NetStatusTxt.Text = Text
    End Sub

End Class