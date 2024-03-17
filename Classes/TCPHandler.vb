Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Public Class TCPHandler
    Private ReadOnly Client As TcpClient
    Private ReadOnly Server As TcpListener
    Public ReadOnly DeviceIP As IPAddress
    Private ReadOnly ServerThread As Thread
    Public Property KillHandler As Boolean = False
    Public Sub New(DeviceIP As IPAddress)
        Client = New TcpClient()
        Server = New TcpListener(DeviceIP, GLOBAL_CLIENT_PORT) 'sets the server to listen to the specified device ip, on the client port
        Me.DeviceIP = DeviceIP

        ServerThread = New Thread(AddressOf ReceiveData) With {.IsBackground = True} 'server listens on the background thread
        ServerThread.Start()
    End Sub

    Public Sub SendData(Data As String) 'sends data provided to the connected device
        Try
            Client.Connect(DeviceIP, GLOBAL_SERVER_PORT) 'sends to the server port
            Dim Stream As NetworkStream = Client.GetStream()
            Dim SndBytes As Byte() = Encoding.UTF8.GetBytes(Data)
            Stream.Write(SndBytes, 0, SndBytes.Length)
        Catch ex As Exception
            CustomMsgBox.ShowBox($"Error in TCPHandler.SendData(), error: {ex.Message}")
        Finally
            Client.Close() 'closes the client instance to release socket binding
        End Try
    End Sub

    Private Sub ReceiveData() 'thread of the server to listen for data
        Server.Start()
        While AppRunning And Not KillHandler
            Try
                If Server.Pending() Then 'checks whether clients waiting to transmit data
                    Dim Client As TcpClient = Server.AcceptTcpClient 'creates a temporary client to communicate with
                    Dim HandlerThread As New Thread(AddressOf HandleClientData) With {.IsBackground = True} 'makes the client response management happen on a different thread to allow receiving multiple client communications simultaneously
                    HandlerThread.Start(Client)
                End If
            Catch
                Continue While
            End Try
        End While
        Server.Stop() 'stops the server
    End Sub

    Private Sub HandleClientData(ClientObj As Object)
        Try
            Dim Client As TcpClient = DirectCast(ClientObj, TcpClient)
            Using Stream As NetworkStream = Client.GetStream
                Dim Buffer(1024) As Byte
                Dim BytesRead As Integer = Stream.Read(Buffer, 0, Buffer.Length)
                Dim ReceivedData As String = Encoding.UTF8.GetString(Buffer, 0, BytesRead)

                'need to check whether valid data
                Dim JsonObject As Object = JObject.Parse(ReceivedData)
                If JsonObject("AppIDMessage") <> "WAYFARER_V1" Then
                    Throw New Exception
                End If
                InboundJSONBuffer.Enqueue(ReceivedData, If(HighPriorityRRs.Contains(JsonObject("MessageType")), "high", "low")) 'adds the received data to the inbound queue based on messagetype priority
            End Using
        Catch ex As Exception
            Exit Sub
        Finally
            Client.Close() 'disposes of the client instance
            Client.Dispose()
        End Try
    End Sub

    Public Overrides Function ToString() As String
        Return DeviceIP.ToString()
    End Function
End Class
