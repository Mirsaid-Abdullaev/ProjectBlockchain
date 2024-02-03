Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Public Class TCPHandler
    Private Client As TcpClient
    Private Server As TcpListener
    Private DeviceIP As IPAddress
    Private ServerThread As Thread
    Public Sub New(DeviceIP As IPAddress)
        Client = New TcpClient()
        Server = New TcpListener(DeviceIP, 36000)
        Me.DeviceIP = DeviceIP

        ServerThread = New Thread(AddressOf ReceiveData) With {.IsBackground = True}
        ServerThread.Start()
    End Sub

    Public Sub SendData(Data As String)
        Try
            Client.Connect(DeviceIP, ServerPort)
            Dim Stream As NetworkStream = Client.GetStream()
            Dim SndBytes As Byte() = Encoding.UTF8.GetBytes(Data)
            Stream.Write(SndBytes, 0, SndBytes.Length)
        Catch ex As Exception
            SendData(Data)
            CustomMsgBox.ShowBox($"Error in TCPHandler.SendData(), error: {ex.Message}")
        Finally
            Client.Close()
        End Try
    End Sub

    Private Sub ReceiveData()
        Server.Start()
        While AppRunning
            Try
                If Server.Pending() Then
                    Dim Client As TcpClient = Server.AcceptTcpClient
                    CustomMsgBox.ShowBox($"Client {DirectCast(Client.Client.RemoteEndPoint, IPEndPoint).Address}")
                    Dim HandlerThread As New Thread(AddressOf HandleClientData) With {.IsBackground = True}
                    HandlerThread.Start(Client)
                End If
            Catch
                Continue While
            End Try
        End While

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
                InboundJSONBuffer.Enqueue(ReceivedData, If(HighPriorityRRs.Contains(JsonObject("MessageType")), "high", "low"))
            End Using
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
End Class
