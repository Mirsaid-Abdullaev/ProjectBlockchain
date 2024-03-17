Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Module WalletOperations
    Function GeneratePrivateKey() As String 'this sub uses the "safer" random generator than the generic rng to create a private key
        Dim RndBytes(31) As Byte 'initialising an array of 32 bytes (256 bits) to use to store randomly generated ints
        Using SafeRNG As New RNGCryptoServiceProvider()
            SafeRNG.GetBytes(RndBytes) 'using the higher entropy method of generating random integers to populate the array of bytes
        End Using
        Dim PrivateKey As String = BitConverter.ToString(RndBytes).Replace("-", "").ToUpper 'converts the 32 bytes into a hexadecimal string
        Return PrivateKey 'returns the final private key, a 64 character hexa string, representing a 256 bit integer
    End Function

    'absolute hashfest - one way function from the private key to get a public address that is mathematically linked to the device
    Function GeneratePublicKey(StrPrivKey As String) As String 'returns a public key for a given private key
        Dim TempStr As String = StrPrivKey
        TempStr &= "WAYFARER_V1" 'an appID salt is added to the privatekey as well for validating the file
        'first, two rounds of SHA256 are performed on the initial private key
        Using sha256 As SHA256 = SHA256.Create()
            For i As Byte = 1 To 2
                Dim PrivKeyBytes As Byte() = Encoding.UTF8.GetBytes(TempStr)
                Dim HashBytes As Byte() = sha256.ComputeHash(PrivKeyBytes)
                TempStr = BitConverter.ToString(HashBytes).Replace("-", "").ToUpper()
            Next
        End Using 'here, the initial key has been put through the sha256 function twice
        'then two rounds of sha512 using the same logic but using the output of the previous two rounds as the input
        Using sha512 As SHA512 = SHA512.Create()
            For i As Byte = 1 To 2
                Dim PrivKeyBytes As Byte() = Encoding.UTF8.GetBytes(TempStr)
                Dim HashBytes As Byte() = sha512.ComputeHash(PrivKeyBytes)
                TempStr = BitConverter.ToString(HashBytes).Replace("-", "").ToUpper()
            Next
        End Using
        'final two rounds of sha256 again on the output of the previous two rounds of sha512
        Using sha256 As SHA256 = SHA256.Create()
            For i As Byte = 1 To 2
                Dim PrivKeyBytes As Byte() = Encoding.UTF8.GetBytes(TempStr)
                Dim HashBytes As Byte() = sha256.ComputeHash(PrivKeyBytes)
                TempStr = BitConverter.ToString(HashBytes).Replace("-", "").ToUpper()
            Next
        End Using
        Return TempStr 'returns the final public address of a given private key in hexa
    End Function

    Public Function ScanBlockBalanceUpdate(PublicAddress As String) As Double 'function to get/update a wallet balance
        'go through every block loaded to find and update balance of a wallet - used in updating own balance, and validating others' transactions too
        Dim Balance As Double = 0 'initial balance is (obviously) 0
        Dim TempList As List(Of Block) = WFBlockchain.Blockchain
        For Each Item As Block In TempList 'as my program is very dynamic this is a measure to stop any runtime errors occurring from new blocks being added during for loop and blowing up the loop
            If Item.GetTransactionList Is Nothing Then
                Continue For
            End If 'skips any blocks that have no transactions, obviously
            'otherwise, we have a block with some transactions and can iterate through them sequentially - this is the only way to do so, as we have to check every transaction (worst case O(n^2)
            For Each Transact As Transaction In Item.GetTransactionList 'gets all the transactions from a block if there are any transactions
                'perform a search through all the transactions per block - as search will discard many transacts without processing, worst case O(n), best case O(1)
                If Item.GetMiner = PublicAddress Then 'this means that the address we are checking for is a miner of this block, and hence collects all the fees from each transaction
                    Balance += Transact.Fee
                End If
                If Transact.Sender <> PublicAddress And Transact.Recipient <> PublicAddress Then 'this checks whether 
                    Continue For
                End If
                Select Case Transact.Sender
                    Case = PublicAddress
                        'the transaction involved the address sending some amount, so negative change to balance
                        Balance -= Transact.Quantity
                    Case Else
                        Exit Select 'check the next condition
                End Select
                Select Case Transact.Recipient
                    Case = PublicAddress
                        'the address we are checking is receiving an amount, so add to the balance value
                        Balance += Transact.Quantity
                    Case Else
                        Exit Select 'otherwise quit the selection
                End Select
            Next
        Next

        'after searching each block and transaction list, we have a final value for the balance - this will not only be used for calculating own balances
        'but also when validating a new transaction request, i.e. checking if the sender has the required funds to prevent rogue/fraudulent requests
        Return Balance
    End Function

    Public Function GetTemporaryBalanceDelta(PublicAddress As String) As Double 'returns the future change in balance after the current block is mined
        Dim BalanceDelta As Double = 0
        If WFTransactionPool.GetPoolSize > 0 Then 'checking current transaction pool for any transactions sent by the device to avoid negative intermediate balances
            Dim TransactPool As List(Of Transaction) = WFTransactionPool.GetTransactionList()
            For Each Transact As Transaction In TransactPool
                If Not (Transact.Sender = PublicAddress Or Transact.Recipient = PublicAddress) Then
                    Continue For
                End If
                If Transact.Sender = PublicAddress Then
                    BalanceDelta -= Transact.Quantity
                    Continue For
                End If
                If Transact.Recipient = PublicAddress Then
                    BalanceDelta += Transact.Quantity
                End If
            Next
        End If
        Return BalanceDelta
    End Function
End Module



Public Class Wallet
    Private ReadOnly WalletName As String 'a basic identifier, used for accessing the file path of the wallet
    Private ReadOnly PublicAddress As String 'the address that can be used to send cryptocurrency to a given wallet
    Private ReadOnly FilePath As String 'the location of the wallet in storage
    Private Balance As Double 'the value of cryptocurrency held by a given wallet

    Public Function GetPublicAddress() As String 'getter method to obtain the public address
        Return PublicAddress
    End Function
    Public Function GetWalletName() As String 'getter for the wallet name
        Return WalletName
    End Function
    Public Function GetWalletBalance() As Double 'getter (and implicit setter) for the balance value of a wallet
        Balance = ScanBlockBalanceUpdate(PublicAddress)
        Return Balance
    End Function

    Public Sub New(WalletName As String) 'for generating a new wallet from a user input wallet name
        Dim Dir As List(Of String) = Directory.EnumerateFiles(DirectoryList(0)).ToList
        For Each FilePath As String In Dir
            If Path.GetExtension(FilePath) <> ".wfwlt" Then
                DeleteFileOrDir(FilePath) 'invalid file stored in the directory, delete it
            End If
        Next 'now the directory is clear of non-wallet files and we can check for number of wallets
        Try
            Dim CurrCount As Byte = Directory.EnumerateFiles(DirectoryList(0)).Count 'gets the number of wallet files now stored in the directory of \Wallets
            If CurrCount >= 25 Then 'there are too many wallets owned by the user, cannot make more from this device
                CustomMsgBox.ShowBox("User reached limit of wallets. Either log into an existing wallet, or delete an existing wallet.", "ERROR", False)
                'custommsgbox is a class for displaying a message, a message box title, and whether to display or not display a cancel button
                'please see the CustomMsgBox class definition and the Design.vb module code for further details
                Exit Sub
            End If
        Catch ex As Exception
            'this would happen if there was no directory in the first place - an unlikely error as the user woulld have had to delete the
            'directory after opening the app, but checking anyway
            CustomMsgBox.ShowBox("Required directory does not exist. Created new \Wallets folder.", "ERROR HANDLED", False)
            If Not Directory.Exists(DirectoryList(0)) Then
                Directory.CreateDirectory(DirectoryList(0))
            End If 'creating a \Wallets directory as directorylist(0) stores the path to the directory of \Wallets
        End Try
        Me.WalletName = WalletName 'saves the wallet name into the property of the instance
        Me.Balance = 0 'sets initial balance to 0
        Me.FilePath = DirectoryList(0) & WalletName & ExtensionList(0) '0 is the wallet extension and folder path, defined in AppGlobals.vb
        If File.Exists(FilePath) Then 'user tried to create a wallet with the same wallet name as an existing one
            CustomMsgBox.ShowBox("Wallet exists. Wallet creation failed.", "ERROR", False)
            Exit Sub
        End If
        Try
            Dim ButtonOKClicked As Boolean = CustomMsgBox.ShowBox("WARNING: YOU WILL BE SHOWN YOUR PRIVATE KEY NOW. MAKE SURE TO SAVE IT, IT WILL BE DELETED FOREVER. TO CANCEL WALLET CREATION, CLICK CANCEL.", "WARNING")
            'custommsgbox returns a boolean that represents whether OK or Cancel was clicked, checking for this here
            If Not ButtonOKClicked Then 'used clicked cancel
                CustomMsgBox.ShowBox("Cancelled creation of a wallet. Thank you.", "STATUS", False)
                Exit Sub 'exiting the subroutine
            End If
            Dim PrivKey As String = GeneratePrivateKey() 'otherwise, create a private key
            Me.PublicAddress = GeneratePublicKey(PrivKey) 'and a corresponding public key
            CustomMsgBox.ShowBox(PrivKey, "PRIVATE KEY", False) 'showing the user the private key once only
            PrivKey = "" 'overwriting the privkey to add more security to protecting the wallet
            Using fs As FileStream = File.Create(Me.FilePath) 'making a file for the wallet now to allow user to log in again in the future
                Using sw As New StreamWriter(fs)
                    'we write the public address only to the file, as this will be checked for when logging in with the private key
                    sw.WriteLine(Me.PublicAddress)
                End Using
            End Using
        Catch ex As Exception
            CustomMsgBox.ShowBox("Error creating wallet, please try again.", "ERROR", False)
            If File.Exists(FilePath) Then
                File.Delete(FilePath) 'something went wrong in the previous block of code, so just remove and revert any changes made
            End If
        End Try
    End Sub

    Public Sub New(WalletName As String, PublicAddress As String, FilePath As String) 'for loading an existing wallet from the directory, used in the WalletBaseView.vb Form
        Me.WalletName = WalletName
        Me.PublicAddress = PublicAddress
        Me.FilePath = FilePath
        Me.Balance = ScanBlockBalanceUpdate(Me.PublicAddress)
    End Sub
End Class

