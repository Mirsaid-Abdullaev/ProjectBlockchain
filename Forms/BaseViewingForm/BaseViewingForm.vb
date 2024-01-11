﻿Public Class BaseViewingForm
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

    Private Sub BlockchainExpF_Click(sender As Object, e As EventArgs) Handles BlockchainExpF.Click
        Me.Hide()
        BlockchainExplorerF.ShowDialog()
        Me.StatusLbl.Text = GlobalData.StatusLblText
        Try
            Me.Show()
        Catch ex As Exception
        End Try
        GC.Collect()
    End Sub

    Private Sub BaseViewingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, BlockchainExpColours)
        Me.StatusLbl.Text = GlobalData.StatusLblText
        GC.Collect()
    End Sub

    Private Sub BlockchainExpH_Click(sender As Object, e As EventArgs) Handles BlockchainExpH.Click
        Me.Hide()
        BlockchainExplorerH.ShowDialog()
        Me.StatusLbl.Text = GlobalData.StatusLblText
        Try
            Me.Show()
        Catch ex As Exception
        End Try
        GC.Collect()
    End Sub

    Private Sub NetworkStatusView_Click(sender As Object, e As EventArgs) Handles NetworkStatusView.Click
        Me.Hide()
        NetStatusView.ShowDialog()
        Me.StatusLbl.Text = GlobalData.StatusLblText
        Try
            Me.Show()
        Catch ex As Exception
        End Try
        GC.Collect()
    End Sub
End Class