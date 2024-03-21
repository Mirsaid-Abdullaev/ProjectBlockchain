Imports Newtonsoft.Json
Module RequestsResponses
    Public MustInherit Class Request 'abstract base class for all requests
        Public ReadOnly Property MessageType As String
        Public ReadOnly Property DeviceIP As String
        Public ReadOnly Property AppIDMessage As String
        Public ReadOnly Property RootAddress As String 'ip address of root
        Public Sub New(RequestType As String)
            MessageType = RequestType
            DeviceIP = GetOwnIP().ToString
            AppIDMessage = "WAYFARER_V1"
            If IsRoot Then
                RootAddress = DeviceIP 'this device is the root
            Else
                RootAddress = ROOT_IP 'sending the ip of the root, which is further up the linked list
            End If
        End Sub

        Public MustOverride Function GetJSONMessage() As String 'abstract function - redefined in child classes
    End Class

    Public Class ConnectionRequest
        Inherits Request
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New()
            MyBase.New("ConnectionRequest")

        End Sub
    End Class

    Public Class SyncRequest
        Inherits Request
        Public ReadOnly Property StartBlock As UInteger
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function

        Public Sub New(StartBlock As UInteger)
            MyBase.New("SyncRequest")
            Me.StartBlock = StartBlock
        End Sub
    End Class

    Public Class NewTransactionRequest
        Inherits Request
        Public ReadOnly Property Timestamp As String
        Public ReadOnly Property Sender As String
        Public ReadOnly Property Recipient As String
        Public ReadOnly Property Quantity As Double
        Public ReadOnly Property Fee As Double
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(Transact As Transaction)
            MyBase.New("NewTransactionRequest")
            Me.Timestamp = Transact.Timestamp
            Me.Recipient = Transact.Recipient
            Me.Sender = Transact.Sender
            Me.Quantity = Transact.Quantity
            Me.Fee = Transact.Fee
        End Sub
    End Class

    Public Class ValidateNewMinedBlockRequest 'high priority
        Inherits Request
        Public ReadOnly Property Index As UInteger
        Public ReadOnly Property PrevHash As String
        Public ReadOnly Property Nonce As UInteger
        Public ReadOnly Property Timestamp As String
        Public ReadOnly Property Transactions As String
        Public ReadOnly Property Hash As String
        Public ReadOnly Property Miner As String
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(ProposedBlock As Block)
            MyBase.New("ValidateNewMinedBlockRequest")
            Me.Index = ProposedBlock.GetIndex
            Me.Hash = ProposedBlock.GetHash
            Me.PrevHash = ProposedBlock.GetPrevHash
            Me.Timestamp = ProposedBlock.GetTimestamp
            Me.Transactions = GetTransactionListAsString(ProposedBlock.GetTransactionList)
            Me.Nonce = ProposedBlock.GetNonce
            Me.Miner = ProposedBlock.GetMiner
        End Sub
    End Class
    Public Class TransmitNewBlockRequest 'high priority
        Inherits Request
        Public ReadOnly Property Index As UInteger
        Public ReadOnly Property PrevHash As String
        Public ReadOnly Property Nonce As UInteger
        Public ReadOnly Property Timestamp As String
        Public ReadOnly Property Transactions As String
        Public ReadOnly Property Hash As String
        Public ReadOnly Property Miner As String
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(ValidatedBlock As Block)
            MyBase.New("TransmitNewBlockRequest")
            Me.Index = ValidatedBlock.GetIndex
            Me.Hash = ValidatedBlock.GetHash
            Me.PrevHash = ValidatedBlock.GetPrevHash
            Me.Timestamp = ValidatedBlock.GetTimestamp
            Me.Transactions = GetTransactionListAsString(ValidatedBlock.GetTransactionList)
            Me.Nonce = ValidatedBlock.GetNonce
            Me.Miner = ValidatedBlock.GetMiner
        End Sub
    End Class

    Public Class TransmitTransactionRequest
        Inherits Request
        Public ReadOnly Property Timestamp As String
        Public ReadOnly Property Sender As String
        Public ReadOnly Property Recipient As String
        Public ReadOnly Property Quantity As Double
        Public ReadOnly Property Fee As Double
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(Transact As Transaction)
            MyBase.New("TransmitTransactionRequest")
            Me.Timestamp = Transact.Timestamp
            Me.Recipient = Transact.Recipient
            Me.Sender = Transact.Sender
            Me.Quantity = Transact.Quantity
            Me.Fee = Transact.Fee
        End Sub
    End Class
    Public Class DisconnectRequest
        Inherits Request
        Public Sub New()
            MyBase.New("DisconnectRequest")
        End Sub
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
    End Class

    '
    'RESPONSES
    '

    Public MustInherit Class Response 'abstract base response
        Public Property MessageType As String
        Public Property DeviceIP As String
        Public ReadOnly Property AppIDMessage As String = "WAYFARER_V1"
        Public Property Status As String
        Public Sub New(ResponseType As String, Status As Boolean)
            MessageType = ResponseType
            DeviceIP = GetOwnIP().ToString
            Me.Status = If(Status, "Accept", "Decline")
        End Sub

        Public MustOverride Function GetJSONMessage() As String
    End Class

    Public Class ConnectionResponse
        Inherits Response
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(Status As Boolean)
            MyBase.New("ConnectionResponse", Status)
        End Sub
    End Class

    Public Class BlockResponse
        Inherits Response
        Public ReadOnly Property Index As UInteger
        Public ReadOnly Property PrevHash As String
        Public ReadOnly Property Nonce As UInteger
        Public ReadOnly Property Timestamp As String
        Public ReadOnly Property Transactions As String
        Public ReadOnly Property Hash As String

        Public ReadOnly Property Miner As String
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(SendingBlock As Block, Status As Boolean)
            MyBase.New("BlockResponse", Status)
            Me.Index = SendingBlock.GetIndex
            Me.Hash = SendingBlock.GetHash
            Me.PrevHash = SendingBlock.GetPrevHash
            Me.Timestamp = SendingBlock.GetTimestamp
            Me.Transactions = GetTransactionListAsString(SendingBlock.GetTransactionList)
            Me.Nonce = SendingBlock.GetNonce
            Me.Miner = SendingBlock.GetMiner
        End Sub
    End Class

    Public Class TransactionPoolResponse
        Inherits Response
        Public ReadOnly Property Transactions As String
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(Status As Boolean, CurrentTransactionPool As TransactionPool)
            MyBase.New("TransactionPoolResponse", Status)
            Me.Transactions = GetTransactionListAsString(CurrentTransactionPool.GetTransactionList)
        End Sub
    End Class

    Public Class NewTransactionResponse
        Inherits Response
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(Status As Boolean)
            MyBase.New("NewTransactionResponse", Status)
        End Sub
    End Class
    Public Class ValidateNewMinedBlockResponse 'high priority
        Inherits Response
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(Status As Boolean)
            MyBase.New("ValidateNewMinedBlockResponse", Status)
            'accept or decline the proposed block
        End Sub
    End Class

    Public Class SyncResponse 'used by root to send to clients
        Inherits Response
        Public ReadOnly Property ExpectedBlocks As UInteger
        Public Sub New(Status As String, StartBlock As UInteger)
            MyBase.New("SyncResponse", Status)
            ExpectedBlocks = WFBlockchain.LastBlock.GetIndex - StartBlock + 1
        End Sub
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
    End Class
End Module
