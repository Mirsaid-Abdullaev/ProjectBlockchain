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
    Public Function GetTransactionList() As List(Of Transaction)
        Return Transactions
    End Function



    Private Property Hash As String 'hash of current block
    Public Function GetHash() As String
        Return Hash
    End Function




    Private Property PrevHash As String 'hash of previous block
    Public Function GetPrevHash() As String
        Return PrevHash
    End Function




    Private Property Timestamp As String = Nothing 'timestamp of block's mining
    Public Function GetTimestamp() As String
        Return Timestamp
    End Function


    Public Function GetBlockDataForMining() As String
        Dim TempResult As String = String.Join(vbLf, {$"Block: {Index}", $"Previous hash: {PrevHash}", $"Nonce: {Nonce}", $"Transactions: {GetTransactionListAsString(Transactions)}", $"Timestamp: {Timestamp}"})
        Return TempResult
    End Function

    Public Sub MineBlock(ByVal Difficulty As Byte)
        Dim DataToHash As String = GetBlockDataForMining()
        If Timestamp = Nothing Then
            Timestamp = TimeToUnixMs(Date.Now)
        End If
        Hash = GetSHA256FromString(DataToHash)

        'here, the hashing is delegated to a separate thread to keep UI from freezing/crashing

        Dim TempMiningThread As New Thread(Sub() HashUntilDifficulty(Difficulty)) With {.IsBackground = True}
        IsMining = True
        TempMiningThread.Start()
        'CustomMsgBox.ShowBox("Block mined. Hash: " + Hash.ToString + ", " + "Nonce: " + Nonce.ToString, "BLOCK MINED", False)
    End Sub



    Private Sub HashUntilDifficulty(ByVal Difficulty As Byte)
        Dim DifficultyString As String = StrDup(Difficulty, "0")
        While Mid(Hash, 1, Difficulty) <> DifficultyString
            Nonce += 1
            Dim DataToHash As String = GetBlockDataForMining()
            Hash = GetSHA256FromString(DataToHash)
        End While
        IsMining = False
    End Sub

    Public Sub AddTransaction(ByVal NewTransaction As Transaction)
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



    Public Sub New() 'for programmatic declaration of new blocks AFTER sync process
        Index = WFBlockchain.GetLastBlock.GetIndex + 1
        Nonce = 0
        Transactions = New List(Of Transaction)
        PrevHash = WFBlockchain.GetLastBlock.GetHash
    End Sub

    Public Sub New(Hash As String, PrevHash As String, Index As UInteger, Nonce As UInteger, Timestamp As String, Transactions As List(Of Transaction)) 'only for loading ready blocks in
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
            CustomMsgBox.ShowBox($"Error - no block file specified at path: ""{FilePath}""", "ERROR", False)
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
                    If BlockComps(i).Length < 10 Then
                        GoTo ErrorCondition
                    End If
                Case Else
                    GoTo ErrorCondition
            End Select
        Next
        ParsedBlock = New Block(BlockComps(4), BlockComps(1), BlockComps(0), BlockComps(2), BlockComps(5), GetTransactionListFromString(BlockComps(3)))
        If IsValidBlock(ParsedBlock) Then
            Return ParsedBlock
        Else
            GoTo ErrorCondition
        End If
ErrorCondition: CustomMsgBox.ShowBox($"Block file ""{FilePath}"" seems to be corrupted. It will be deleted and any blocks after it will be too.", "ERROR", False)
        'delete current file and after it - use a "Nothing" checker to determine during LocalSync to delete all  block files by using a loop counter
        Return Nothing
    End Function
    Public Sub CreateFileFromBlock(Block As Block)
        Dim FilePath As String = DirectoryList(1) & "Block" & Block.GetIndex.ToString & ExtensionList(1)
        If File.Exists(FilePath) AndAlso GetBlockFromFile(FilePath).GetFullData = Block.GetFullData Then
            Exit Sub 'checks if the block to be saved exists, and if it exactly the same as the block to be saved - overwritten if not the same, otherwise exited
        End If
        Using sw As New StreamWriter(FilePath, False)
            sw.WriteLine($"Block: {Block.GetIndex()}")
            sw.WriteLine($"Previous hash: {Block.GetPrevHash()}")
            sw.WriteLine($"Nonce: {Block.GetNonce}")
            sw.WriteLine("Transactions: " & GetTransactionListAsString(Block.GetTransactionList))
            sw.WriteLine($"Hash: {Block.GetHash()}")
            sw.WriteLine($"Timestamp: {Block.GetTimestamp()}")
        End Using
        'File.SetAttributes(FilePath, File.GetAttributes(FilePath) Or FileAttributes.ReadOnly)
    End Sub

    Public Function IsValidBlockFilePath(FilePath As String) As Boolean 'for localsync proc
        Dim Pattern As String = "^Block[1-9]\d*$"
        Return Regex.IsMatch(FilePath, Pattern)
    End Function

    Public Function IsValidBlock(Block As Block) As Boolean
        Return GetSHA256FromString(Block.GetBlockDataForMining) = Block.GetHash
    End Function
End Module