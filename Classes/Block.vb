Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading
Public Class Block
    Private ReadOnly Index As UInteger 'index of current block
    Public ReadOnly Property GetIndex As UInteger
        Get
            Return Index
        End Get
    End Property
    Private Nonce As UInteger 'value linked to difficulty - higher difficulty = higher nonce = higher compute per block
    Public ReadOnly Property GetNonce As UInteger
        Get
            Return Nonce
        End Get
    End Property

    Private Transactions As List(Of Transaction) 'store of transactions in current block
    Public ReadOnly Property GetTransactionList As List(Of Transaction)
        Get
            Return Transactions
        End Get
    End Property

    Private Hash As String 'hash of current block
    Public ReadOnly Property GetHash As String
        Get
            Return Hash
        End Get
    End Property

    Private ReadOnly PrevHash As String 'hash of previous block
    Public ReadOnly Property GetPrevHash As String
        Get
            Return PrevHash
        End Get
    End Property


    Private Timestamp As String = Nothing 'timestamp of block's mining
    Public ReadOnly Property GetTimestamp As String
        Get
            Return Timestamp
        End Get
    End Property


    Private Miner As String 'public address of miner wallet
    Public ReadOnly Property GetMiner As String
        Get
            Return Miner
        End Get
    End Property

    Public ReadOnly Property BlockDataForMining() As String 'returns the string version of the block data used in the mining thread to mine the block
        Get
            Return String.Join(vbLf, {$"Block: {Index}", $"Previous hash: {PrevHash}", $"Nonce: {Nonce}", $"Transactions: {GetTransactionListAsString(Transactions)}", $"Timestamp: {Timestamp}", $"Miner: {Miner}"})
        End Get
    End Property

    Public Sub MineBlock(ByVal Difficulty As Byte) 'difficulty is a constant in the blockchain instance
        If Not IsMiner Or CurrentWallet Is Nothing Or Index = 0 Or IsMining Then
            Exit Sub 'cannot mine as user not logged into a wallet or the node opted out of mining or still mining something else
        End If
        'this is checking the conditions for mining to begin, i.e. a logged in wallet, the node being a mining node, the block not being the genesis block and mining is not taking place already
        Miner = CurrentWallet.GetPublicAddress 'sets the miner field to wallet address of logged in wallet - to collect fees if this block is accepted

        Timestamp = TimeToUnixMs(Date.Now) 'sets the timestamp of mining beginning
        Dim DataToHash As String = BlockDataForMining() 'gets the initial data to hash
        Hash = GetSHA256FromString(DataToHash) 'sets the initial candidate hash
        'here, the hashing is delegated to a separate thread to keep UI from freezing/crashing and program flow to continue

        Dim TempMiningThread As New Thread(Sub() HashUntilDifficulty(Difficulty)) With {.IsBackground = True}
        IsMining = True
        TempMiningThread.Start()
        'CustomMsgBox.ShowBox("Block mined. Hash: " + Hash.ToString + ", " + "Nonce: " + Nonce.ToString, "BLOCK MINED", False)
    End Sub

    Private Sub HashUntilDifficulty(Difficulty As Byte)
        Dim DifficultyString As String = StrDup(Difficulty, "0") 'gets the success condition data
        While Mid(Hash, 1, Difficulty) <> DifficultyString And Not StopMining 'keeps retrying another hash while the prepending difficulty characters are not all 0
            Nonce += 1 'increment nonce and rehash
            Dim DataToHash As String = BlockDataForMining()
            Hash = GetSHA256FromString(DataToHash)
        End While
        IsMining = False 'mining here is finished, switch mining flag off
    End Sub

    Public Sub AddTransaction(ByVal NewTransaction As Transaction)
        Transactions.Add(NewTransaction) 'literally does what it says, adds a transaction instance to the block instance's list of transactions
    End Sub


    'FOR BLOCKCHAIN EXPLORER
    Public ReadOnly Property HeaderData As String 'gets a formatted string to display a block in the blockchain explorer for the header only view
        Get
            Return String.Join(vbCrLf, {$"Block: {Index}", $"Hash: {Hash}", $"Previous hash: {PrevHash}", $"Nonce: {Nonce}", $"Timestamp: {Timestamp}", $"Miner: {Miner}"}) & vbLf
        End Get
    End Property

    Public ReadOnly Property FullData As String 'same as above but adds transaction data to it too
        Get
            Dim TransactData As String = GetTransactionListAsString(Transactions) 'transaction list in the format [], [], [], etc
            Return HeaderData() & $"Transactions: {TransactData}" & vbCrLf
        End Get
    End Property
    '
    'END BLOCKCHAIN EXPLORER
    '


    Public Sub New() 'for programmatic declaration of new blocks AFTER sync process
        Index = WFBlockchain.GetLastBlock.GetIndex + 1
        Nonce = 0
        Transactions = New List(Of Transaction)
        PrevHash = WFBlockchain.GetLastBlock.GetHash
    End Sub

    Public Sub New(Hash As String, PrevHash As String, Index As UInteger, Nonce As UInteger, Timestamp As String, Transactions As List(Of Transaction), Miner As String) 'only for loading already-created blocks in
        Me.Hash = Hash
        Me.PrevHash = PrevHash
        Me.Nonce = Nonce
        Me.Index = Index
        Me.Timestamp = Timestamp
        Me.Transactions = Transactions
        Me.Miner = Miner 'new addition - miner identification for fee claiming
    End Sub

    Public Shadows Function Equals(BlockA As Block)
        Return BlockA.FullData = Me.FullData
    End Function
End Class

Module BlockOperations
    'if getblockfromfile returns nothing, it means file is erroneous
    Public Function GetBlockFromFile(FilePath As String) As Block 'converts a .wfbc file into a block instance

        Dim RawData As String = "" 'data to read from
        Dim ParsedBlock As Block 'final result
        If File.Exists(FilePath) Then
            Using SR As New StreamReader(FilePath)
                RawData = SR.ReadToEnd 'gets all the data written to the file
            End Using
        Else 'filepath given has no block file - unlikely to happen as this is only called in local synchronisation 
            Return Nothing 'returning nothing means error occurred in this sub
        End If
        Dim BlockComps() As String = RawData.Split(vbLf) 'splitting raw data into an array of fields from the lines to parse according to default block file structure
        For I As Byte = 0 To 6 '7 expected fields, matching to the block class fields above
            Try
                BlockComps(I) = BlockComps(I).Replace(vbCr, "") 'all fields apart from the last one have a carriage-return character at the end, so we remove it if it is present
            Catch ex As Exception
                Continue For 'line didnt have a carriage return character, just skip
            End Try
        Next

        For I As Byte = 0 To 6 '7 expected fields as above
            Select Case I
                Case 0 'block index field
                    If BlockComps(I).Substring(0, 7) <> "Block: " Then
                        Return Nothing 'this is how all line 1's start, if not throw the error condition
                    End If
                    BlockComps(I) = BlockComps(I).Substring(7) 'otherwise rewrite the block component to hold all the data from the "Block: {index}" line after the 8th char (the space)
                Case 1 'block prev hash field
                    If BlockComps(I).Substring(0, 15) <> "Previous hash: " Then 'same as above - line 2 always starts with this
                        Return Nothing
                    End If
                    BlockComps(I) = BlockComps(I).Substring(15) 'get the actual hash value
                    If Not IsValidHexStr(BlockComps(I), 64) Then 'check if it is actually a hex string of length 64, otherwise dump the block
                        Return Nothing
                    End If
                Case 2 'block nonce value field
                    If BlockComps(I).Substring(0, 7) <> "Nonce: " Then
                        Return Nothing 'same thing
                    End If
                    BlockComps(I) = BlockComps(I).Substring(7)
                Case 3 'block transactionlist string
                    If BlockComps(I).Substring(0, 14) <> "Transactions: " Then
                        Return Nothing
                    End If
                    BlockComps(I) = BlockComps(I).Substring(14)
                    If BlockComps(I).Length <= 1 Then 'no transaction instance is that short, so trans list is instantly nothing
                        BlockComps(I) = Nothing
                    End If
                Case 4 'block hash field
                    If BlockComps(I).Substring(0, 6) <> "Hash: " Then
                        Return Nothing
                    End If
                    BlockComps(I) = BlockComps(I).Substring(6)
                    If Not IsValidHexStr(BlockComps(I), 64) Then
                        Return Nothing 'same as prevhash essentially
                    End If
                Case 5 'block timestamp field
                    If BlockComps(I).Substring(0, 11) <> "Timestamp: " Then
                        Return Nothing
                    End If
                    BlockComps(I) = BlockComps(I).Substring(11)
                    If BlockComps(I).Length < 10 Then
                        Return Nothing 'timestamps are 10+ length
                    End If
                Case 6 'block miner field
                    If BlockComps(I).Substring(0, 7) <> "Miner: " Then
                        Return Nothing
                    End If
                    BlockComps(I) = BlockComps(I).Substring(7)
                    If Not IsValidHexStr(BlockComps(I), 64) Then
                        Return Nothing 'miner field is invalid as not a 64-digit hexa
                    End If
                Case Else
                    Exit Select 'just in case
            End Select
        Next
        ParsedBlock = New Block(BlockComps(4), BlockComps(1), BlockComps(0), BlockComps(2), BlockComps(5), GetTransactionListFromString(BlockComps(3)), BlockComps(6)) 'makes a block instance
        If Not IsValidBlock(ParsedBlock) Then
            Return Nothing
        End If
        Return ParsedBlock
    End Function

    Public Sub CreateFileFromBlock(Block As Block)
        Dim FilePath As String = DirectoryList(1) & "Block" & Block.GetIndex.ToString & ExtensionList(1) 'creates the string file path
        If File.Exists(FilePath) Then 'checks if the block to be saved exists, and if it exactly the same as the block to be saved - overwritten if not the same, otherwise exited
            Try
                Dim Data As String = GetBlockFromFile(FilePath).FullData
                If Data <> Nothing AndAlso Data = Block.FullData Then
                    Exit Sub
                End If
            Catch ex As Exception
                'the file is wrong, overwrite it, so let program keep going
            End Try
        End If
        Using SW As New StreamWriter(FilePath, False) 'sets up the writer so that it overwrites anything in the existing file if there is one
            SW.WriteLine($"Block: {Block.GetIndex}")
            SW.WriteLine($"Previous hash: {Block.GetPrevHash}")
            SW.WriteLine($"Nonce: {Block.GetNonce}")
            SW.WriteLine("Transactions: " & GetTransactionListAsString(Block.GetTransactionList))
            SW.WriteLine($"Hash: {Block.GetHash}")
            SW.WriteLine($"Timestamp: {Block.GetTimestamp}")
            SW.Write($"Miner: {Block.GetMiner}")
            'this is the block format, as described in the above sub as well
        End Using
    End Sub

    Public Function IsValidBlockFilePath(FilePath As String) As Boolean 'for localsync process
        Dim Pattern As String = "^Block[1-9]\d*$"
        Return Regex.IsMatch(FilePath, Pattern)
    End Function

    Public Function IsValidNextBlock(Block As Block, PrevBlock As Block) As Boolean
        Return GetSHA256FromString(Block.BlockDataForMining()) = Block.GetHash _
               AndAlso Block.GetPrevHash = PrevBlock.GetHash _
               AndAlso CULng(Block.GetTimestamp) >= CULng(PrevBlock.GetTimestamp) _
               AndAlso IsValidHexStr(Block.GetHash, 64) _
               AndAlso IsValidHexStr(Block.GetPrevHash, 64) _
               AndAlso IsValidHexStr(Block.GetMiner, 64) _
               AndAlso Block.GetIndex = PrevBlock.GetIndex + 1
    End Function

    Public Function IsValidBlock(Block As Block) As Boolean
        Return Block.GetHash = GetSHA256FromString(Block.BlockDataForMining)
    End Function

    Public Class BlockIndexComparer 'this is for sorting a list of blocks by their indexes
        Implements IComparer(Of Block)

        Public Function Compare(BlockA As Block, BlockB As Block) As Integer Implements IComparer(Of Block).Compare
            Return BlockA.GetIndex.CompareTo(BlockA.GetIndex)
        End Function
    End Class
End Module