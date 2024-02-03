Imports Newtonsoft.Json.Linq
Imports System.Net.Sockets
Imports System.Net
Imports System.Text
Imports System.Threading

Public Class RootHandler
    Private UDPSender As UdpClient
    Private UDPReceiver As UdpClient

    Private RootListenThread As Thread
    Private ResponseManagementThread As Thread
    Private NodeResponseThread As Thread
    Private NodeQueue As DataBufferQueue(Of String) 'queue of devices to synchronise (shouldnt be required as the connection process is linear but just in case)

    Public Sub New()
        'need a listener for sync messages, a response sub for sending all block files to a device
    End Sub

    Public Sub Start()
        If Not ROOT Then
            CustomMsgBox.ShowBox("Error initialising RootHandler: device is not a ROOT", "ERROR", False)
            Exit Sub
        End If
        UDPReceiver = New UdpClient(RemoteEP) With {.EnableBroadcast = False}
        RootListenThread = New Thread(AddressOf ListenForSync) 'root taking requests
        ResponseManagementThread = New Thread(AddressOf CheckForDevices)

    End Sub

    Private Sub CheckForDevices()
        While AppRunning
            If Not NodeQueue.IsEmpty Then

            End If
        End While
    End Sub

    Private Sub SendChainData(DeviceIP As String, StartIndex As UInteger, EndIndex As UInteger) 'this should be called from the thread when a new device registers
        'send blocks here one by one in the specified range
        Dim Node As IPAddress
        Try
            Node = IPAddress.Parse(DeviceIP)
        Catch ex As Exception
            CustomMsgBox.ShowBox("Error in RootHandler.SendChainData, IP address conversion issue", "ERROR", False)
            Exit Sub
        End Try
        If EndIndex = 0 Then

        End If
        For Each Item As Block In WFBlockchain.GetChain.ToList.ToList.ToList
            Dim BlockResponse As New BlockResponse(Item, "Accepted")
        Next
    End Sub

    Private Sub ListenForSync()
        While AppRunning
            Try
                Dim RecvBytes As Byte() = UDPReceiver.Receive(RemoteEP)
                Dim RecvData As String = Encoding.UTF8.GetString(RecvBytes)
                Dim JsonObject As Object = JObject.Parse(RecvData) 'dynamic json object variable that parses data into a json style format for lookups efficiently
                Dim MessageType As String = JsonObject("MessageType").ToString
                Select Case MessageType
                    Case "SyncRequest"
                        NodeQueue.Enqueue(JsonObject("DeviceIP")) 'device IP added

                        'need to send this device all the blocks until 
                    Case Else
                        Throw New Exception 'whatever arrived is not what i need, throw it out
                End Select
            Catch ex As Exception
                CustomMsgBox.ShowBox($"Error in UDPHandler.Listen thread, error: {ex.Message}")
                Continue While
            End Try
        End While
    End Sub
End Class
