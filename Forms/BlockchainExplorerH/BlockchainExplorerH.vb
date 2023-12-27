Imports System.Threading
Public Class BlockchainExplorerH
    Private UpdateBlockchain As Thread
    Private FormExiting As Boolean
    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
        GC.Collect()
    End Sub

    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        Application.Exit()
    End Sub

    Private Sub BlockchainExplorer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, BlockchainExpColours)
        CheckForIllegalCrossThreadCalls = False
        Me.StatusLbl.Text = GlobalData.StatusLblText
        UpdateBlockchain = New Thread(AddressOf UpdateBlockchainTxt)
        UpdateBlockchain.Start()
        GC.Collect()
    End Sub

    Private Sub UpdateBlockchainTxt()
        While Not FormExiting
            'BlockchainTxt.Clear()
            'For Each B As Block In GlobalData.Blockchain.Blockchain
            '    BlockchainTxt.Text &= B.GetHeaderData & vbCrLf
            'Next
            'Thread.Sleep(5000)
        End While
        GC.Collect()
        Exit Sub
    End Sub

    Private Sub BlockchainExplorerH_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        FormExiting = True
        GC.Collect()
    End Sub
End Class