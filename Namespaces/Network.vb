Imports System.Net
Imports System.Net.Sockets
Imports Newtonsoft.Json


Module Network
    Public PreviousPointer As String = "INIT" 'IPAddress of device connected; INIT if not connected to network; NULL if ROOT
    Public NextPointer As String = "INIT" 'same as above but NULL means LEAF node
    Public Connected As Boolean = False
    Public ROOT As Boolean = False
    Public ROOT_IP As String = ""
    Public Function GetOwnIP() As IPAddress 'gets private IPv4 address of the node
            Dim Hostname As String = Dns.GetHostName
            Dim DeviceIP As IPAddress = IPAddress.Loopback
            For Each HostAddress In System.Net.Dns.GetHostEntry(Hostname).AddressList()
                If HostAddress.AddressFamily = AddressFamily.InterNetwork Then
                    DeviceIP = HostAddress
                    Exit For
                End If
            Next
            Return DeviceIP
        End Function

        Public Function GetDeviceName() As String 'gets a hostname (max 64 chars)
            Try
                If Dns.GetHostName.Length > 64 Then
                    Return GetSHA256FromString(Dns.GetHostName)
                Else
                    Return Dns.GetHostName
                End If
            Catch ex As Exception
                Return GetSHA256FromString(TimeString)
            End Try
        End Function
    End Module

Module RequestsResponses

    Public MustInherit Class Request 'abstract base class for all requests
        Public ReadOnly Property MessageType As String
        Public ReadOnly Property DeviceName As String
        Public ReadOnly Property DeviceIP As String
        Public ReadOnly Property PrevPtr As String
        Public ReadOnly Property NextPtr As String
        Public ReadOnly Property AppIDMessage As String
        Public Sub New(RequestType As String)
            MessageType = RequestType
            DeviceName = GetDeviceName()
            DeviceIP = GetOwnIP().ToString
            PrevPtr = PreviousPointer
            NextPtr = NextPointer
            AppIDMessage = "WAYFARER_V1"
        End Sub

        Public MustOverride Function GetJSONMessage() As String 'abstract function - redefined in child classes
    End Class

    Public Class ConnectionRequest
        Inherits Request
        Public ReadOnly Property RootAddress As String 'ip address of root
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New()
            MyBase.New("ConnectionRequest")
            If ROOT Then
                RootAddress = DeviceIP 'this device is the root
            Else
                RootAddress = ROOT_IP 'sending the ip of the root, which is further up the linked list
            End If
        End Sub
    End Class
    Public Class SyncRequest
        Inherits Request
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New()
            MyBase.New("SyncRequest")
        End Sub
    End Class

    Public Class GetBlockchainDataRequest 'low priority
        Inherits Request
        Public ReadOnly Property StartBlock As UInteger
        Public ReadOnly Property EndBlock As UInteger 'for requesting specific ranges
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(StartBlock As UInteger, Optional EndBlock As UInteger = 0)
            MyBase.New("GetBlockchainDataRequest")
            Me.StartBlock = StartBlock
            Me.EndBlock = EndBlock
        End Sub
    End Class

    Public Class GetTransactionPoolRequest 'low priority
        Inherits Request
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New()
            MyBase.New("GetTransactionPoolRequest")
        End Sub
    End Class

    Public Class NewTransactionRequest
        Inherits Request
        Public ReadOnly Property Timestamp As String
        Public ReadOnly Property Sender As String
        Public ReadOnly Property Recipient As String
        Public ReadOnly Property Quantity As Double
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(Transact As Transaction)
            MyBase.New("NewTransactionRequest")
            Me.Timestamp = Transact.Timestamp
            Me.Recipient = Transact.Recipient
            Me.Sender = Transact.Sender
            Me.Quantity = Transact.Quantity
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
        Public Property DeviceName As String
        Public Property DeviceIP As String
        Public ReadOnly Property AppIDMessage As String = "WAYFARER_V1"
        Public Sub New(ResponseType As String)
            MessageType = ResponseType
            DeviceName = GetDeviceName()
            DeviceIP = GetOwnIP().ToString
        End Sub

        Public MustOverride Function GetJSONMessage() As String 'abstract function - redefined in child classes
    End Class

    Public Class ConnectionResponse
        Inherits Response
        Public ReadOnly Property Status As String
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(Status As String)
            MyBase.New("ConnectionResponse")
            Me.Status = Status
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
        Public ReadOnly Property Status As String
        Public ReadOnly Property FinalIndex As Integer
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(SendingBlock As Block, Status As String, Optional EndIndex As UInteger = 0)
            MyBase.New("BlockResponse")
            Me.Status = Status
            Me.Index = SendingBlock.GetIndex
            Me.Hash = SendingBlock.GetHash
            Me.PrevHash = SendingBlock.GetPrevHash
            Me.Timestamp = SendingBlock.GetTimestamp
            Me.Transactions = GetTransactionListAsString(SendingBlock.GetTransactionList)
            Me.Nonce = SendingBlock.GetNonce
            Me.FinalIndex = If(EndIndex = 0, WFBlockchain.GetLastBlock.GetIndex, EndIndex)
        End Sub
    End Class

    Public Class TransactionPoolResponse
        Inherits Response
        Public ReadOnly Property Status As String
        Public ReadOnly Property Transactions As String
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(Status As String, CurrentTransactionPool As TransactionPool)
            MyBase.New("TransactionPoolResponse")
            Me.Status = Status
            Me.Transactions = CurrentTransactionPool.GetTransactionListAsString
        End Sub
    End Class

    Public Class NewTransactionResponse
        Inherits Response
        Public ReadOnly Property Status As String
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(Status As String)
            MyBase.New("NewTransactionResponse")
            Me.Status = Status
        End Sub
    End Class
    Public Class ValidateNewMinedBlockResponse 'high priority
        Inherits Response
        Public ReadOnly Property Status As String 'accept or decline the block - if accept from both linked nodes, block is ok
        Public Overrides Function GetJSONMessage() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
        Public Sub New(Status As String)
            MyBase.New("ValidateNewMinedBlockResponse")
            Me.Status = Status
        End Sub
    End Class
End Module
