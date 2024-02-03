Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Module WalletOperations
    Function GeneratePrivateKey() As String
        Dim RndBytes(31) As Byte
        Using SafeRNG As New RNGCryptoServiceProvider()
            SafeRNG.GetBytes(RndBytes)
        End Using
        Dim PrivateKey As String = BitConverter.ToString(RndBytes).Replace("-", "")
        Return PrivateKey
    End Function

    'absolute hashfest - one way function from the private key.
    Function GeneratePublicKey(StrPrivKey As String) As String
        Dim TempStr As String = StrPrivKey
        TempStr = TempStr & "WAYFARER_V1" & GetDeviceName() 'an appID + devicename salt is added to the privatekey as well for validating the file
        'first, two rounds of SHA256 are performed
        Using sha256 As SHA256 = SHA256.Create()
            For i As Byte = 1 To 2
                Dim PrivKeyBytes As Byte() = Encoding.UTF8.GetBytes(TempStr)
                Dim HashBytes As Byte() = sha256.ComputeHash(PrivKeyBytes)
                TempStr = BitConverter.ToString(HashBytes).Replace("-", "").ToUpper()
            Next
        End Using
        'then two rounds of sha512
        Using sha512 As SHA512 = SHA512.Create()
            For i As Byte = 1 To 2
                Dim PrivKeyBytes As Byte() = Encoding.UTF8.GetBytes(TempStr)
                Dim HashBytes As Byte() = sha512.ComputeHash(PrivKeyBytes)
                TempStr = BitConverter.ToString(HashBytes).Replace("-", "").ToUpper()
            Next
        End Using
        'final two rounds of sha256
        Using sha256 As SHA256 = SHA256.Create()
            For i As Byte = 1 To 2
                Dim PrivKeyBytes As Byte() = Encoding.UTF8.GetBytes(TempStr)
                Dim HashBytes As Byte() = sha256.ComputeHash(PrivKeyBytes)
                TempStr = BitConverter.ToString(HashBytes).Replace("-", "").ToUpper()
            Next
        End Using
        Return TempStr
    End Function

    Public Function ScanBlockBalanceUpdate(PublicAddress As String) As Double
        'go through every block loaded to find balance
        Dim Balance As Double = 0
        For Each Item As Block In WFBlockchain.GetChain
            If Item.GetTransactionList Is Nothing Then
                Continue For
            End If 'skips any blocks that have no transactions
            For Each Transact As Transaction In Item.GetTransactionList
                'perform a search through all the transactions per block - should be OK time-complexity wise, as search will discard many transacts without processing, worst case O(n)
                If Transact.Sender <> PublicAddress AndAlso Transact.Recipient <> PublicAddress Then
                    Continue For
                End If

                If Transact.Sender = PublicAddress Then
                    Balance -= Transact.Quantity
                    Continue For
                ElseIf Transact.Recipient = PublicAddress Then
                    Balance += Transact.Quantity
                End If
            Next
        Next
        Return Balance
    End Function
End Module
Public Class Wallet
    Private ReadOnly WalletName As String
    Private ReadOnly PublicAddress As String
    Private ReadOnly FilePath As String
    Private Balance As Double

    Public Function GetPublicAddress() As String
        Return PublicAddress
    End Function
    Public Function GetWalletName() As String
        Return WalletName
    End Function
    Public Function GetWalletBalance() As Double
        Balance = ScanBlockBalanceUpdate(PublicAddress)
        Return Balance
    End Function

    Public Sub New(WalletName As String) 'for generating a new wallet
        Try
            Dim CurrCount As Byte = Directory.EnumerateFiles(DirectoryList(0)).Count
            If CurrCount >= 5 Then
                CustomMsgBox.ShowBox("User reached limit of wallets. Either log into an existing wallet, or delete an existing wallet.", "ERROR", False)
                Exit Sub
            End If
        Catch ex As Exception
            CustomMsgBox.ShowBox("Directory does not exist. Created new \Wallets folder.", "ERROR HANDLED", False)
            If Not Directory.Exists(DirectoryList(0)) Then
                Directory.CreateDirectory(DirectoryList(0))
            End If
        End Try
        Me.WalletName = WalletName
        Me.Balance = 0
        Me.FilePath = DirectoryList(0) & WalletName & ExtensionList(0) '0 is the wallet extension and folder path
        If File.Exists(FilePath) Then
            CustomMsgBox.ShowBox("Wallet exists. Wallet creation failed.", "ERROR", False)
            Exit Sub
        End If
        Try
            Dim ButtonOKClicked As Boolean = CustomMsgBox.ShowBox("WARNING: YOU WILL BE SHOWN YOUR PRIVATE KEY NOW. MAKE SURE TO SAVE IT, IT WILL BE DELETED FOREVER. TO CANCEL WALLET CREATION, CLICK CANCEL.", "WARNING")
            If Not ButtonOKClicked Then
                CustomMsgBox.ShowBox("Cancelled creation of a wallet. Thank you.", "STATUS", False)
                Exit Try
            End If
            Dim PrivKey As String = GeneratePrivateKey()
            Me.PublicAddress = GeneratePublicKey(PrivKey)
            CustomMsgBox.ShowBox(PrivKey, "PRIVATE KEY", False)

            Using fs As FileStream = File.Create(Me.FilePath)
                Using sw As New StreamWriter(fs)
                    'balance is not needed in the file anymore - scanblock procedure will get the balance at startup
                    sw.WriteLine(Me.PublicAddress)
                End Using
            End Using
        Catch ex As Exception
            CustomMsgBox.ShowBox("Error creating wallet, please try again.", "ERROR", False)
            If File.Exists(FilePath) Then
                File.Delete(FilePath)
            End If
        End Try
    End Sub

    Public Sub New(WalletName As String, PublicAddress As String, FilePath As String) 'for loading an existing wallet
        Me.WalletName = WalletName
        Me.PublicAddress = PublicAddress
        Me.FilePath = FilePath
        Me.Balance = ScanBlockBalanceUpdate(Me.PublicAddress)
    End Sub

    Public Sub ChangeBalance(Value As Double)
        Me.Balance += Value
    End Sub
End Class

