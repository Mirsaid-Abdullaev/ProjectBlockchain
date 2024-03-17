Imports System.IO
Imports System.Net
Imports System.Security.Authentication
Public Class WalletBaseView

    Private Sub WalletBaseView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DesignLoad(Me, WalletColours)
        Me.StatusLbl.Text = SetSharedLblText()
        GC.Collect()
    End Sub

    Private Sub CreateWallet_Click(sender As Object, e As EventArgs) Handles CreateWallet.Click
        Dim WalletName As String = ""
        While Not IsValidStr(WalletName)
            Dim TempResult As Object = CustomInputBox.ShowInputBox("Enter a valid wallet name as a string - alphanumerics only. Note: this CANNOT be changed once wallet is created!", "GENERATE NEW WALLET")
            WalletName = TempResult(0) 'get user to input a string wallet name until it is a valid one
            If Not TempResult(1) Then
                CustomMsgBox.ShowBox("Cancelled creation of a wallet. Thank you.", "STATUS", False)
                Exit Sub 'if clicked on cancel button, exits the creation process
            End If
        End While
        Dim TempWallet As New Wallet(WalletName) 'calls the New Wallet procedure to save a new wallet
        GC.Collect()
    End Sub

    Private Sub Disconnect_Click(sender As Object, e As EventArgs) Handles Disconnect.Click
        AppRunning = False
        DisconnectFromChain()
        Application.Exit()
    End Sub

    Private Sub LoginWallet_Click(sender As Object, e As EventArgs) Handles LoginWallet.Click
        Dim TempResult As Object = CustomListBoxInputBox.ShowInputBox("Please select a wallet from the list or click cancel to exit.", True, "WALLET LOGIN")
        Dim SelectedWallet As String
        If TempResult(1) = False Then
            Exit Sub '1st index of the showinputbox method returns the button clicked on the form, if button was CANCEL, exit the login procedure
        End If
        SelectedWallet = TempResult(0) 'otherwise, get the selected wallet

        Dim WalName As String = SelectedWallet.Replace(".wfwlt", "") 'cut off the file extension from the input
        If String.IsNullOrEmpty(SelectedWallet) Then
            CustomMsgBox.ShowBox("No wallet selected. Please generate a wallet, and then try again.", "ERROR", False)
            Exit Sub 'empty data selected
        End If
        Dim ActualHash As String = ""
        For I As Byte = 1 To 3 'allows 3 tries to enter the correct private key to the selected wallet
            TempResult = CustomInputBox.ShowInputBox($"Enter private key to wallet ""{SelectedWallet}"" or click Cancel to exit.", "WALLET LOGIN")
            If TempResult(1) = False Then
                Exit Sub
            ElseIf Not IsValidHexStr(TempResult(0), 64) Then
                Continue For 'checking whether data is in hex string, 64 character form
            End If

            Dim TempPrivKey As String = TempResult(0)
            Dim ComputedHash As String = GeneratePublicKey(TempPrivKey) 'get the expected hash/public address 
            Using SR As New StreamReader(DirectoryList(0) & SelectedWallet)
                ActualHash = SR.ReadLine() 'pull the actual public address from the file
            End Using
            If Not String.Equals(ActualHash, ComputedHash) AndAlso I < 3 Then
                If Not CustomMsgBox.ShowBox($"Incorrect private key. Please try again. {3 - I} more tries left.", "WARNING") Then
                    Exit Sub 'the actual hash is not equal to the computed hash - private key was wrong.
                End If
                Continue For
            ElseIf Not String.Equals(ActualHash, ComputedHash) AndAlso I = 3 Then 'final try
                CustomMsgBox.ShowBox($"Incorrect private key. Try again later, out of tries for now!", "WARNING", False)
                Exit Sub
            Else
                Exit For 'correct private key entered, continue processing
            End If
        Next
        CurrentWallet = New Wallet(WalName, ActualHash, DirectoryList(0) & SelectedWallet) 'update current wallet instance
        CustomMsgBox.ShowBox("Wallet login successful." & vbCrLf & $"Name: {CurrentWallet.GetWalletName}" & vbCrLf & $"Balance: {CurrentWallet.GetWalletBalance}" & vbCrLf & $"Address: {CurrentWallet.GetPublicAddress}", "STATUS", False)

        Me.StatusLbl.Text = SetSharedLblText() 'update status label to reflect these changes
        GC.Collect()
    End Sub

    Private Sub ViewWallets_Click(sender As Object, e As EventArgs) Handles ViewWallets.Click
        Dim Result As String = CustomListBoxInputBox.ShowInputBox("ALL LOCALLY STORED WALLETS", False, "VIEW WALLETS")(0)
        'this code snippet shows all the wallets stored in the \Wallets directory and allows the user to view the address of this wallet in both string form and an optional QR format too
        If Result = String.Empty Then
            Exit Sub 'none selected - result holds the selected data from the input box showing, see \Design.vb code file for implementation of ShowInputBox method
        End If
        Dim Address As String
        Using SR As New StreamReader(DirectoryList(0) & Result)
            Address = SR.ReadLine 'pull down the wallet address of the selected wallet
        End Using
        If Not CustomMsgBox.ShowBox($"ADDRESS: {Address}" & vbCrLf & "Would you like to see a QR of the wallet address before exiting?", "SHOW QR OF WALLET ADDRESS", True) Then
            Exit Sub 'shows address in string format, gets the user input of the custom msg box, if ok clicked, QR code will be generated, cancel will make the program exit
        End If
        'generate the QR code and the imagepopupform - implementation, please see \Design.vb, Public Class ImagePopupForm
        Try
            Dim QR As Image = GenerateQR(Address)
            If QR Is Nothing Then
                Exit Sub
            End If
            ImagePopupForm.ShowBox(QR, "WALLET ADDRESS IN QR FORMAT")
        Catch ex As Exception
            Exit Sub
        End Try
        GC.Collect()
    End Sub

    Private Sub DeleteWallet_Click(sender As Object, e As EventArgs) Handles DeleteWallet.Click
        Dim TempResult As Object = CustomListBoxInputBox.ShowInputBox("Please select a wallet from the list or click cancel to exit.", True, "DELETE WALLET")
        'tempresult holds 2 objects in the array - 0th index stores the data selected, 1st index stores the button clicked by the user
        If TempResult(1) = False Then 'if cancel clicked - exit the delete wallet process
            Exit Sub
        End If

        Dim SelectedWallet As String = TempResult(0) 'selected wallet name is stored here in the form {WalletName}.wfwlt

        If SelectedWallet = String.Empty Then
            CustomMsgBox.ShowBox("Delete failed. Try again later.", "ERROR", False)
            Exit Sub 'empty data selected
        End If

        Dim WalletPath As String = DirectoryList(0) & SelectedWallet 'the path
        Dim WalletAddress As String = "" 'the wallet address of the selected wallet

        Using SR As New StreamReader(DirectoryList(0) & SelectedWallet)
            WalletAddress = SR.ReadLine() 'Public Address/Hash pulled out from the file
        End Using

        Try
            File.Delete(WalletPath) 'delete the file
            CustomMsgBox.ShowBox("Wallet successfully deleted. Reminder: this is not reversible.", "STATUS", False)
        Catch ex As Exception
            CustomMsgBox.ShowBox($"Not able to delete wallet, or it does not exist. Error: {ex.Message}", "ERROR", False)
        End Try

        If CurrentWallet IsNot Nothing AndAlso CurrentWallet.GetPublicAddress = WalletAddress Then
            CurrentWallet = Nothing
            Me.StatusLbl.Text = SetSharedLblText() 'update the status label if the logged in wallet was the one the user deleted
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
        Me.Dispose()
        GC.Collect()
    End Sub

    Public Function GenerateQR(StrToConvert As String) As Image 'returns a QR code of a string, courtesy of https://www.c-sharpcorner.com/article/create-qr-code-using-google-charts-api-in-vb-net/ by Uday Dodiya
        Dim Data As String = StrToConvert
        Dim QrLink As String = "https://chart.googleapis.com/chart?cht=qr&chs=300x300&chl="
        QrLink &= Data
        Try
            Using WebClient As New WebClient
                Const _Tls12 As SslProtocols = &HC00
                Const Tls12 As SecurityProtocolType = _Tls12
                ServicePointManager.SecurityProtocol = Tls12
                Dim Bytes As Byte() = WebClient.DownloadData(QrLink)
                Using MS = New MemoryStream(Bytes)
                    Return Image.FromStream(MS)
                End Using
            End Using
        Catch ex As Exception
            CustomMsgBox.ShowBox("Error while generating QR, sorry for the inconvenience.", "ERROR", False)
            Return Nothing
        End Try
    End Function

    Private Sub GetHelpManual_Click(sender As Object, e As EventArgs) Handles GetHelpManual.Click
        Return 'not implemented
    End Sub
End Class