Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Text.Encoding
Imports System.Threading
Imports Newtonsoft.Json

Namespace Network

    Module NetworkVariables
        Public ReadOnly NodeTypesDict As String() = {"ROOT", "BRANCH", "LEAF", "INIT"} '0=ROOT, 1=BRANCH, 2=LEAF, 3=INIT
        Public NodeType As String = NodeTypesDict(3)
        Public PreviousPointer As String = "INIT" 'IPAddress of device connected; INIT if not connected to network; NULL if ROOT
        Public NextPointer As String = "INIT" 'same as above but NULL means LEAF node
        Public Connected As Boolean = False
    End Module


    Public Class TCPHandler
        Private TCPClient As TcpClient
        Private TCPServer As TcpListener
        'way to 
    End Class




    Public Class ConnectionHandler
        Private udpClient As UdpClient

        Public Sub New()

            udpClient = New UdpClient() With {.EnableBroadcast = True}

            Dim listenerThread As New Thread(AddressOf ListenForBroadcasts)
            listenerThread.Start()
            Dim broadcastThread As New Thread(AddressOf BroadcastConnectionRequest)
            broadcastThread.Start()
        End Sub

        Private Sub ListenForBroadcasts()

        End Sub

        Private Sub BroadcastConnectionRequest()

        End Sub



        Private Function IsRequestRelevant(request As ConnectionRequest) As Boolean
            'to test whether the json received is actually valid, e.g. checking appidmessage and the messagetype field
            Return True
        End Function

    End Class



    Module MiscLibrary
        Public Class DeviceUser
            Public DeviceName As String
            Public DeviceIP As IPAddress
            Public NodeType As String
        End Class
        Public Function GetOwnIP() As IPAddress
            Dim Hostname As String = Dns.GetHostName
            Dim DeviceIP As IPAddress = IPAddress.Loopback
            For Each HostAddress In System.Net.Dns.GetHostEntry(Hostname).AddressList()
                If HostAddress.AddressFamily.ToString = "InterNetwork" Then
                    DeviceIP = HostAddress
                    Exit For
                End If
            Next
            Return DeviceIP
        End Function

        Public Function GetDeviceName() As String
            Try
                If Dns.GetHostName.Length > 15 Then
                    Return Dns.GetHostName.Substring(0, 15)
                Else
                    Return Dns.GetHostName
                End If
            Catch ex As Exception
                Return "E" + TimeString.Substring(0, 2) + TimeString.Substring(3, 2) + TimeString.Substring(6, 2)
            End Try
        End Function
    End Module

    Module RequestsResponses
        Public MustInherit Class Request 'abstract base class for all requests
            Public Property MessageType As String
            Public Property DeviceName As String
            Public Property DeviceIP As String
            Public ReadOnly Property AppIDMessage As String = "WAYFARER_V1"
            Public Sub New(RequestType As String)
                MessageType = RequestType
                DeviceName = GetDeviceName()
                DeviceIP = GetOwnIP().ToString
            End Sub

            Public MustOverride Function GetJSONMessage() As String 'abstract function - redefined in child classes
        End Class

        Public Class ConnectionRequest
            Inherits Request
            Public ReadOnly Property NodeType As String
            Public Overrides Function GetJSONMessage() As String
                Return JsonConvert.SerializeObject(Me)
            End Function
            Public Sub New()
                MyBase.New("ConnectionRequest")
                NodeType = "INIT"
            End Sub
        End Class

        Public Class GetBlockchainDataRequest
            Inherits Request
            Public ReadOnly Property StartBlock As UInteger
            Public Overrides Function GetJSONMessage() As String
                Return JsonConvert.SerializeObject(Me)
            End Function
            Public Sub New(StartBlock As UInteger)
                MyBase.New("GetBlockchainDataRequest")
                Me.StartBlock = StartBlock
            End Sub
        End Class

        Public Class GetTransactionPoolRequest
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
            Public ReadOnly Property Quantity As Single
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

        Public Class ValidateNewMinedBlockRequest
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
                Me.Transactions = GetTransactionListAsString(ProposedBlock.TransactionList)
                Me.Nonce = ProposedBlock.GetNonce
            End Sub
        End Class
        Public Class TransmitNewBlockRequest
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
                Me.Transactions = GetTransactionListAsString(ValidatedBlock.TransactionList)
                Me.Nonce = ValidatedBlock.GetNonce
            End Sub
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
            Public Overrides Function GetJSONMessage() As String
                Return JsonConvert.SerializeObject(Me)
            End Function
            Public Sub New(SendingBlock As Block, Status As String)
                MyBase.New("BlockResponse")
                Me.Status = Status
                Me.Index = SendingBlock.GetIndex
                Me.Hash = SendingBlock.GetHash
                Me.PrevHash = SendingBlock.GetPrevHash
                Me.Timestamp = SendingBlock.GetTimestamp
                Me.Transactions = GetTransactionListAsString(SendingBlock.TransactionList)
                Me.Nonce = SendingBlock.GetNonce
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
                Me.Transactions = CurrentTransactionPool.GetTransactions
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

        Public Class ValidateNewMinedBlockResponse
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
End Namespace