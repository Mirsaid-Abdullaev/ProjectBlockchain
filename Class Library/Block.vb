Imports System.Threading

Public Class Block
    Private ReadOnly Property pIndex As UInteger 'index of current block
    Private Property Nonce As UInteger 'value linked to difficulty - higher difficulty = higher nonce = higher compute per block
    Private Property Transactions As List(Of Transaction) 'store of transactions in current block

    Public Property Hash As String 'hash of current block
    Public Property PrevHash As String 'hash of previous block
    Public Property Time As String 'timestamp of block's mining
    Public ReadOnly Property Index As UInteger 'index of current block - non-private member used to read the value only
        Get
            Return pIndex
        End Get
    End Property
    Public BlockData As String

    Public Sub GetBlockDataAsString()
        BlockData = ""
        Dim TransactData As String = ""
        For Each Transact As Transaction In Transactions
            TransactData += Transact.ReturnStringOfTransaction() + "_"
        Next
        BlockData += Hash + "*" + PrevHash + "*" + Index.ToString + "*" + Nonce.ToString + "*" + Time + "*" + TransactData

    End Sub

    Public Sub MineBlock(ByVal Difficulty As Byte)
        Dim DifficultyString As String = ""
        For I As Byte = 1 To Difficulty
            DifficultyString &= "0"
        Next
        GetBlockDataAsString()
        Hash = GetSHA256HashFromString(BlockData)

        'here, the hashing is delegated to a separate thread to keep UI from freezing/crashing

        Dim TempMiningThread As New Thread(Sub() HashingUntilDifficulty(Difficulty, DifficultyString)) With {.IsBackground = True}
        TempMiningThread.Start()
    End Sub



    Private Sub HashingUntilDifficulty(ByVal Difficulty As Byte, ByVal DifficultyString As String)
        While Mid(Hash, 1, Difficulty) <> DifficultyString
            Nonce += 1
            GetBlockDataAsString()
            Hash = GetSHA256HashFromString(BlockData)
            'FormTxtBoxTest.Text += Hash & " - " & Mid(Hash, 1, Difficulty) & vbCrLf
        End While
        Time = TimeToUnix(Date.Now)
        MsgBox("Block mined. Hash: " + Hash.ToString + ", " + "Nonce: " + Nonce.ToString)
    End Sub

    Public Sub AddBlockTransaction(ByVal NewTransaction As Transaction)
        Transactions.Add(NewTransaction)
    End Sub

    Public Function GetHeaderData() As String
        Return Index.ToString & ") " & Hash & "*" & PrevHash & "*" & Nonce.ToString & "*" & Time
    End Function

    Public Function GetBlockData() As String
        Dim TransactData As String = ""
        For Each Transact As Transaction In Transactions
            TransactData += Transact.ReturnStringOfTransaction() + "_"
        Next
        Return Index.ToString & ") " & Hash & "*" & PrevHash & "*" & Nonce.ToString & "*" & Time & ":" & TransactData
    End Function
    Public Sub New(ByVal CurrentChainIndex As UInteger, ByVal CurrPrevHash As String)
        pIndex = CurrentChainIndex + 1
        Nonce = 0
        Transactions = New List(Of Transaction)
        PrevHash = CurrPrevHash
    End Sub

    Public Sub New(ByVal BlockType As String) 'only to be used for the genesis block
        If BlockType = "Genesis" Then
            pIndex = 0
            Nonce = 0
            Transactions = New List(Of Transaction)
            PrevHash = "GenesisBlock"
            Hash = "GenesisBlock"
            Time = "GenesisBlock"
        Else
            MsgBox("Used wrong constructor - constructor reserved for genesis block.")
        End If
    End Sub

End Class
