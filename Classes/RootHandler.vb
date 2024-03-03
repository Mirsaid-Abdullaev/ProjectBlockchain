Imports Newtonsoft.Json.Linq
Imports System.Net.Sockets
Imports System.Net
Imports System.Text
Imports System.Threading

Public Class RootHandler 'doesnt need a destructor or destructor-handling variable as it needs to be alive for entire duration of application running.
    Private UDPSender As UdpClient
    Private ReadOnly UDPReceiver As UdpClient
    Private ReadOnly RootEP As New IPEndPoint(IPAddress.Any, ClientPort)
    Private ReadOnly ServerPort As Integer = 38000
    Private ReadOnly ClientPort As Integer = 37000


    Private ReadOnly RootListenThread As Thread
    Private ReadOnly ResponseManagementThread As Thread
    Private NodeQueue As DataBufferQueue(Of String) 'queue of devices to synchronise (shouldnt be required as the connection process is linear but just in case)
    Public Sub New()
        UDPReceiver = New UdpClient(RootEP) With {.EnableBroadcast = False}
        RootListenThread = New Thread(AddressOf ListenForSync) 'root taking requests
        ResponseManagementThread = New Thread(AddressOf CheckDeviceQueue)
    End Sub

    Public Sub Start()
        If Not IS_ROOT Then
            CustomMsgBox.ShowBox("Error initialising RootHandler: device is not a ROOT", "ERROR", False)
            Exit Sub
        End If
        RootListenThread.Start()
        ResponseManagementThread.Start()
    End Sub

    Private Sub CheckDeviceQueue() 'checks and sends data to any nodes currently in the nodequeue
        While AppRunning
            If Not NodeQueue.IsEmpty Then
                Dim Node As String() = NodeQueue.Dequeue().Split(",")
                Dim DeviceIP As String = Node(0)
                Dim StartIndex As UInteger = Node(1)
                SendChainData(DeviceIP, StartIndex)
            End If
        End While
    End Sub

    Private Sub SendChainData(DeviceIP As String, StartIndex As UInteger) 'this should be called from the thread when a new device registers
        'send blocks here one by one in the specified range

        Dim Node As IPAddress
        Try
            Node = IPAddress.Parse(DeviceIP)
        Catch ex As Exception
            CustomMsgBox.ShowBox("Error in RootHandler.SendChainData, IP address conversion issue", "ERROR", False)
            Exit Sub
        End Try
        Dim TempBlockchainList As List(Of Block) = WFBlockchain.GetChain.GetRange(StartIndex, WFBlockchain.GetChain.Count - StartIndex)
        Try
            UDPSender.Close()
        Catch ex As Exception
            UDPSender = Nothing
        End Try

        Dim EndPoint As New IPEndPoint(Node, ServerPort)
        UDPSender = New UdpClient(EndPoint)

        For Each Item As Block In TempBlockchainList
            Dim BlockResponse As New BlockResponse(Item, "Accepted")
            Dim SndData As String = BlockResponse.GetJSONMessage
            Dim SndBytes As Byte() = Encoding.UTF8.GetBytes(SndData)
            UDPSender.Send(SndBytes, SndBytes.Length)
        Next
        'send transaction pool here too
        UDPSender.Close()

    End Sub

    Private Sub ListenForSync() 'should work - registers when a new device sends a syncrequest
        While AppRunning
            Try
                Dim RecvBytes As Byte() = UDPReceiver.Receive(RootEP)
                Dim RecvData As String = Encoding.UTF8.GetString(RecvBytes)
                Dim JsonObject As Object = JObject.Parse(RecvData) 'dynamic json object variable that parses data into a json style format for lookups efficiently
                Dim MessageType As String = JsonObject("MessageType").ToString
                Select Case MessageType
                    Case "SyncRequest"
                        NodeQueue.Enqueue(String.Join(",", {JsonObject("DeviceIP"), JsonObject("StartBlock").ToString, JsonObject("EndBlock").ToString})) 'device IP, start and endblock indexes added, split by commas
                        'need to send this device all the blocks it needs for synchronisation
                    Case Else
                        Throw New Exception 'whatever arrived is not what i need, throw it out
                End Select
            Catch ex As Exception
                CustomMsgBox.ShowBox($"Error in UDPHandler.Listen thread, error: {ex.Message}")
            End Try
        End While
    End Sub
End Class
