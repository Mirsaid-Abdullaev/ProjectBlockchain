Imports System.Threading
Imports System.IO
Imports System.Text.RegularExpressions
Public Class Block
    Private ReadOnly Property Index As UInteger 'index of current block
    Public Function GetIndex() As UInteger
        Return Index
    End Function



    Private Property Nonce As UInteger 'value linked to difficulty - higher difficulty = higher nonce = higher compute per block
    Public Function GetNonce() As UInteger
        Return Nonce
    End Function




    Private Property Transactions As List(Of Transaction) 'store of transactions in current block
    Public ReadOnly Property TransactionList As List(Of Transaction)
        Get
            Return Transactions
        End Get
    End Property



    Private Property Hash As String 'hash of current block
    Public Function GetHash() As String
        Return Hash
    End Function




    Private Property PrevHash As String 'hash of previous block
    Public Function GetPrevHash() As String
        Return PrevHash
    End Function




    Private Property Timestamp As String 'timestamp of block's mining
    Public Function GetTimestamp() As String
        Return Timestamp
    End Function


    Public Function GetBlockDataForMining() As String
        Dim TempResult As String = String.Join(vbLf, {$"Block: {Index}", $"Previous hash: {PrevHash}", $"Nonce: {Nonce}", $"Transactions: {GetTransactionListAsString(Transactions)}", $"Timestamp: {Timestamp}"})
        Return TempResult
    End Function

    Public Sub MineBlock(ByVal Difficulty As Byte)
        Dim DataToHash As String = GetBlockDataForMining()
        If Not PrevHash = StrDup(64, "0") Then
            Timestamp = TimeToUnixMs(Date.Now)
        End If
        Hash = GetSHA256HashFromString(DataToHash)

        'here, the hashing is delegated to a separate thread to keep UI from freezing/crashing

        Dim TempMiningThread As New Thread(Sub() HashingUntilDifficulty(Difficulty)) With {.IsBackground = True}
        GlobalData.IsMining = True
        TempMiningThread.Start()
    End Sub



    Private Sub HashingUntilDifficulty(ByVal Difficulty As Byte)
        Dim DifficultyString As String = StrDup(Difficulty, "0")
        While Mid(Hash, 1, Difficulty) <> DifficultyString
            Nonce += 1
            Dim DataToHash As String = GetBlockDataForMining()
            Hash = GetSHA256HashFromString(DataToHash)
        End While
        ProjectBlockchain.AppGlobals.GlobalData.IsMining = False
        CustomMsgBox.ShowBox("Block mined. Hash: " + Hash.ToString + ", " + "Nonce: " + Nonce.ToString, "BLOCK MINED", False)
    End Sub

    Public Sub AddBlockTransaction(ByVal NewTransaction As Transaction)
        Transactions.Add(NewTransaction)
    End Sub


    'FOR BLOCKCHAIN EXPLORER
    Public Function GetHeaderData() As String
        Return String.Join(vbCrLf, {$"Block: {Index}", $"Hash: {Hash}", $"Previous hash: {PrevHash}", $"Nonce: {Nonce}", $"Timestamp: {Timestamp}"}) & vbLf
    End Function

    Public Function GetFullData() As String
        Dim TransactData As String = GetTransactionListAsString(Transactions) 'transaction list in the format [], [], [], etc
        Return GetHeaderData() & $"Transactions: {TransactData}" & vbCrLf
    End Function
    'END BLOCKCHAIN EXPLORER



    Public Sub New(ByVal CurrentChainIndex As UInteger, ByVal CurrHash As String) 'for programmatic declaration of new blocks AFTER sync process
        Index = CurrentChainIndex + 1
        Nonce = 0
        Transactions = New List(Of Transaction)
        PrevHash = CurrHash
    End Sub


    Public Sub New(ByVal BlockType As String) 'only to be used for the genesis block
        If BlockType = "Genesis" Then
            Index = 0
            Nonce = 0
            Transactions = Nothing
            PrevHash = StrDup(64, "0")
            Timestamp = StrDup(64, "0")
        Else
            CustomMsgBox.ShowBox("Used wrong constructor - constructor reserved for genesis block.", "ERROR", False)
        End If
    End Sub

    Public Sub New(Hash As String, PrevHash As String, Index As UInteger, Nonce As UInteger, Timestamp As String, Transactions As List(Of Transaction))
        Me.Hash = Hash
        Me.PrevHash = PrevHash
        Me.Nonce = Nonce
        Me.Index = Index
        Me.Timestamp = Timestamp
        Me.Transactions = Transactions
    End Sub
End Class

Module BlockOperations
    Public Function GetBlockFromFile(FilePath As String) As Block
        Dim RawData As String = ""
        Dim ParsedBlock As Block
        If File.Exists(FilePath) Then
            Using sr As New StreamReader(FilePath)
                RawData = sr.ReadToEnd
            End Using
        Else
            CustomMsgBox.ShowBox($"Error - no block file specified at path: {FilePath}", "ERROR", False)
            Return Nothing
        End If
        Dim BlockComps() As String = RawData.Split(vbLf)
        For i As Byte = 0 To 5
            Try
                BlockComps(i) = BlockComps(i).Replace(vbCr, "")
            Catch ex As Exception
                Continue For
            End Try
        Next
        For i As Byte = 0 To 5
            Select Case i
                Case 0 'block index
                    If BlockComps(i).Substring(0, 7) <> "Block: " Then
                        GoTo ErrorCondition
                    End If
                    BlockComps(i) = BlockComps(i).Substring(7)
                Case 1 'block prev hash
                    If BlockComps(i).Substring(0, 15) <> "Previous hash: " Then
                        GoTo ErrorCondition
                    End If
                    BlockComps(i) = BlockComps(i).Substring(15)
                    If Not IsValidHexStr(BlockComps(i), 64) Then
                        GoTo ErrorCondition
                    End If
                Case 2 'block nonce value
                    If BlockComps(i).Substring(0, 7) <> "Nonce: " Then
                        GoTo ErrorCondition
                    End If
                    BlockComps(i) = BlockComps(i).Substring(7)
                Case 3 'block transactionlist string
                    If BlockComps(i).Substring(0, 14) <> "Transactions: " Then
                        GoTo ErrorCondition
                    End If
                    BlockComps(i) = BlockComps(i).Substring(14)
                    If BlockComps(i).Length <= 1 Then
                        BlockComps(i) = Nothing
                    End If
                Case 4 'block hash
                    If BlockComps(i).Substring(0, 6) <> "Hash: " Then
                        GoTo ErrorCondition
                    End If
                    BlockComps(i) = BlockComps(i).Substring(6)
                    If Not IsValidHexStr(BlockComps(i), 64) Then
                        GoTo ErrorCondition
                    End If
                Case 5 'block timestamp
                    If BlockComps(i).Substring(0, 11) <> "Timestamp: " Then
                        GoTo ErrorCondition
                    End If
                    BlockComps(i) = BlockComps(i).Substring(11)
                Case Else
ErrorCondition:     CustomMsgBox.ShowBox($"Block file ""{FilePath}"" seems to be corrupted. It will be deleted and any blocks after it will be too.", "ERROR", False)
            End Select
        Next
        ParsedBlock = New Block(BlockComps(4), BlockComps(1), BlockComps(0), BlockComps(2), BlockComps(5), GetTransactionListFromString(BlockComps(3)))
        Return ParsedBlock
    End Function
    Public Sub CreateFileFromBlock(ByRef Block As Block)
        Dim FilePath As String = GlobalData.DirectoryList(1) & "Block" & Block.GetIndex.ToString & GlobalData.ExtensionList(1)
        If File.Exists(FilePath) Then
            CustomMsgBox.ShowBox("Block file already exists, or there is an error with an existing file. Not created the block file.", "ERROR", False)
            Exit Sub
        End If
        Using sw As New StreamWriter(FilePath)
            sw.WriteLine($"Block: {Block.GetIndex()}")
            sw.WriteLine($"Previous hash: {Block.GetPrevHash()}")
            sw.WriteLine($"Nonce: {Block.GetNonce}")
            sw.WriteLine("Transactions: " & GetTransactionListAsString(Block.TransactionList))
            sw.WriteLine($"Hash: {Block.GetHash()}")
            sw.WriteLine($"Timestamp: {Block.GetTimestamp()}")
        End Using
    End Sub

    Public Function IsValidBlockFilePath(FilePath As String) As Boolean 'for localsync proc
        Dim Pattern As String = "^Block[1-9]\d*$"
        Return Regex.IsMatch(FilePath, Pattern)
    End Function

    Public Function IsValidBlock(Block As Block) As Boolean
        Return GetSHA256HashFromString(Block.GetBlockDataForMining) = Block.GetHash
    End Function
End Module