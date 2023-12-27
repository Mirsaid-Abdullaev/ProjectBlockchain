Imports System.Security.Cryptography
Imports System.IO
Imports System.Text


Module Wallets
    Function GeneratePrivateKey() As String
        Dim RndBytes(31) As Byte
        Using SafeRNG As New RNGCryptoServiceProvider()
            SafeRNG.GetBytes(RndBytes)
        End Using
        Dim PrivateKey As String = BitConverter.ToString(RndBytes).Replace("-", "")
        Return PrivateKey
    End Function

    'absolute hashfest here - one way function from the private key.
    Function GeneratePublicKey(StrPrivKey As String) As String
        Dim TempStr As String = StrPrivKey
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
End Module
Public Class Wallet
    Private Name As String
    Private PubAddr As String
    Private FilePath As String
    Private Balance As Single

    Public ReadOnly Property PublicAddress As String
        Get
            Return PubAddr
        End Get
    End Property
    Public ReadOnly Property WalletName As String
        Get
            Return Name
        End Get
    End Property
    Public ReadOnly Property WalletBalance As Single
        Get
            Return Balance
        End Get
    End Property

    Public Sub New(WltName As String) 'for generating a new wallet
        Try
            Dim CurrCount As Byte = Directory.EnumerateFiles(FileSystem.DirectoryList(0)).Count
            If CurrCount >= 5 Then
                CustomMsgBox.ShowBox("User reached limit of wallets. Either log into an existing wallet, or delete an existing wallet.", "ERROR", False)
                Exit Sub
            End If
        Catch ex As Exception
            CustomMsgBox.ShowBox("Directory does not exist. Created new \Wallets folder.", "ERROR HANDLED", False)
            If Not Directory.Exists(FileSystem.DirectoryList(0)) Then
                Directory.CreateDirectory(FileSystem.DirectoryList(0))
            End If
        End Try
        Me.Name = WltName
        Me.Balance = 0
        Me.FilePath = FileSystem.DirectoryList(0) & WltName & FileSystem.ExtensionList(0) '0 is the wallet extension and folder path
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
            Me.PubAddr = GeneratePublicKey(PrivKey)
            CustomMsgBox.ShowBox(PrivKey, "PRIVATE KEY", False)

            Using fs As FileStream = File.Create(Me.FilePath)
                Using sw As New StreamWriter(fs)
                    sw.WriteLine(Me.WalletBalance)
                    sw.WriteLine(Me.PubAddr)
                End Using
            End Using
        Catch ex As Exception
            CustomMsgBox.ShowBox("Error creating wallet, please try again.", "ERROR", False)
            If File.Exists(FilePath) Then
                File.Delete(FilePath)
            End If
        End Try
    End Sub

    Public Sub New(WltName As String, PubAddress As String, FilePath As String, Balance As Single) 'for loading an existing wallet
        Name = WltName
        PubAddr = PubAddress
        Me.FilePath = FilePath
        Me.Balance = Balance
    End Sub
End Class
