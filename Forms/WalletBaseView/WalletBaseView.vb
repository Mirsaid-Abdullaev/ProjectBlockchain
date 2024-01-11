Imports System.IO
Public Class WalletBaseView

    Private Sub WalletBaseView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, WalletColours)
        Me.StatusLbl.Text = GlobalData.StatusLblText
        GC.Collect()
    End Sub

    Private Sub CreateWallet_Click(sender As Object, e As EventArgs) Handles CreateWallet.Click
        Dim WalletName As String = ""
        While Not IsValidStr(WalletName)
            Dim TempResult As Object = CustomInputBox.ShowInputBox("Enter a valid wallet name as a string - alphanumerics only. Note: this CANNOT be changed once wallet is created!", "GENERATE NEW WALLET")
            WalletName = TempResult(0)
            If Not TempResult(1) Then
                CustomMsgBox.ShowBox("Cancelled creation of a wallet. Thank you.", "STATUS", False)
                Exit Sub
            End If
        End While
        Dim TempWallet As New Wallet(WalletName)
        GC.Collect()
    End Sub

    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        GlobalData.AppRunning = False
        'SendDisconnectedJSONToPrevPtr - add when configured
        Application.Exit() 'implement the planned disconnect procedure here and transfer to all these instances
    End Sub

    Private Sub LoginWallet_Click(sender As Object, e As EventArgs) Handles LoginWallet.Click
        Dim TempResult As Object = CustomListBoxInputBox.ShowInputBox("Please select a wallet from the list or click cancel to exit.", True, "WALLET LOGIN")
        Dim SelectedWallet As String
        If TempResult(1) = False Then
            Exit Sub
        End If
        SelectedWallet = TempResult(0)
        Dim WalName As String = SelectedWallet.Replace(".wfwlt", "")
        If String.IsNullOrEmpty(SelectedWallet) Then
            CustomMsgBox.ShowBox("No wallet selected. Please generate a wallet, and then try again.", "ERROR", False)
            Exit Sub
        End If
        Dim Balance As Single = 0
        Dim ActualHash As String = ""
        For I As Byte = 1 To 3
            TempResult = CustomInputBox.ShowInputBox($"Enter private key to wallet ""{SelectedWallet}"" or click Cancel to exit.", "WALLET LOGIN")
            If TempResult(1) = False Then
                Exit Sub
            ElseIf Not IsValidStr(TempResult(0)) Then
                Continue For
            End If
            Dim TempPrivKey As String = TempResult(0)
            Dim ComputedHash As String = GeneratePublicKey(TempPrivKey)
            Using SR As New StreamReader(GlobalData.DirectoryList(0) & SelectedWallet)
                Balance = CSng(SR.ReadLine())
                ActualHash = SR.ReadLine()
            End Using
            If Not String.Equals(ActualHash, ComputedHash) AndAlso I < 3 Then
                If Not CustomMsgBox.ShowBox($"Incorrect private key. Please try again. {3 - I} more tries left.", "WARNING") Then
                    Exit Sub
                End If
                Continue For
            ElseIf Not String.Equals(ActualHash, ComputedHash) AndAlso I = 3 Then
                CustomMsgBox.ShowBox($"Incorrect private key. Try again later, out of tries for now!", "WARNING", False)
                Exit Sub
            Else
                Exit For
            End If
        Next
        GlobalData.CurrentWallet = New Wallet(WalName, ActualHash, GlobalData.DirectoryList(0) & SelectedWallet, Balance)
        CustomMsgBox.ShowBox("Wallet login successful." & vbCrLf & $"Name: {GlobalData.CurrentWallet.WalletName}" & vbCrLf & $"Balance: {GlobalData.CurrentWallet.WalletBalance}" & vbCrLf & $"Address: {GlobalData.CurrentWallet.PublicAddress}", "STATUS", False)
        Me.StatusLbl.Text = SetSharedLblText(If(AppGlobals.GlobalData.Online, "Online", "Offline"), GlobalData.CurrentWallet.WalletName, GlobalData.CurrentWallet.WalletBalance.ToString) '"CURRENT WALLET: " & GlobalData.CurrentWallet.WalletName & vbCrLf & "BALANCE: " & GlobalData.CurrentWallet.WalletBalance.ToString
        GlobalData.StatusLblText = Me.StatusLbl.Text
        GC.Collect()
    End Sub

    Private Sub ViewWallets_Click(sender As Object, e As EventArgs) Handles ViewWallets.Click
        CustomListBoxInputBox.ShowInputBox("ALL LOCALLY STORED WALLETS", False, "VIEW WALLETS")
        GC.Collect()
    End Sub

    Private Sub DeleteWallet_Click(sender As Object, e As EventArgs) Handles DeleteWallet.Click
        Dim TempResult As Object = CustomListBoxInputBox.ShowInputBox("Please select a wallet from the list or click cancel to exit.", True, "DELETE WALLET")
        Dim SelectedWallet As String
        If TempResult(1) = False Then
            Exit Sub
        End If
        SelectedWallet = TempResult(0)
        If SelectedWallet = String.Empty Then
            CustomMsgBox.ShowBox("Delete failed. Try again later.", "ERROR", False)
            Exit Sub
        End If
        Dim WalletPath As String = GlobalData.DirectoryList(0) & SelectedWallet

        Dim ActualHash As String = ""
        TempResult = CustomInputBox.ShowInputBox($"Enter private key to wallet {SelectedWallet} or click Cancel to exit.", "WALLET LOGIN")
        If TempResult(1) = False Then
            Exit Sub
        ElseIf Not IsValidStr(TempResult(0)) Then
            TempResult(0) = ""
        End If
        Dim TempPrivKey As String = TempResult(0)
        Dim ComputedHash As String = GeneratePublicKey(TempPrivKey)
        Using SR As New StreamReader(GlobalData.DirectoryList(0) & SelectedWallet)
            SR.ReadLine()
            Dim PubAddr As String = SR.ReadLine() 'Public Address/Hash
            ActualHash = PubAddr
        End Using
        If Not String.Equals(ActualHash, ComputedHash) Then
            CustomMsgBox.ShowBox("Incorrect private key.", "WARNING", False)
            Exit Sub
        End If
        Try
            File.Delete(WalletPath)
            CustomMsgBox.ShowBox("Wallet successfully deleted. Reminder: this is not reversible.", "STATUS", False)
        Catch ex As Exception
            CustomMsgBox.ShowBox($"Not able to delete wallet, or it does not exist. Error: {ex.Message}", "ERROR", False)
        End Try

        If GlobalData.CurrentWallet IsNot Nothing AndAlso GlobalData.CurrentWallet.PublicAddress = ActualHash Then
            GlobalData.CurrentWallet = Nothing
            Me.StatusLbl.Text = SetSharedLblText(If(AppGlobals.GlobalData.Online, "Online", "Offline"), "Not logged into a wallet.", "Not logged into a wallet.")
            GlobalData.StatusLblText = Me.StatusLbl.Text
        End If
        GC.Collect()
    End Sub

    Private Sub WalletBaseView_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Me.DialogResult = DialogResult.OK
        GC.Collect()
    End Sub

    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
        GC.Collect()
    End Sub
End Class