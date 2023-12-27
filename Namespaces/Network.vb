Imports System.Net
Imports System.Net.Sockets
Imports System.Text.Encoding
Imports System.Threading

Namespace Network
    Module NetworkVariables
        Public Const ServerPort As Integer = 24000 '24000 will be the port for server comms between peers (both TCP and UDP)
        Public Const ClientPort As Integer = 36000 '36000 will be the port for client comms between peers (both TCP and UDP)
        Public ReadOnly MulticastAddress As IPAddress = IPAddress.Parse("224.0.0.26")
        Public DeviceIP As IPAddress = GetOwnIP()
        Public DeviceName As String = GetDeviceName()
        Public Const AppIDMessage As String = "WAYFARER_V1"
        Public ReadOnly NodeTypesDict As Object() = {"ROOT", "BRANCH", "LEAF", "INIT"}
        Public NodeType As String = NodeTypesDict(3)
        Public PreviousPointer As String = "INIT"
        Public NextPointer As String = "INIT"
        Public AppRunning As Boolean = False
        Public Connected As Boolean = False

        Public ParentForm As TestForm
    End Module

    Module AckDiscovery
        Private UDPAckClient As UdpClient
        Private UDPAckEndpoint As IPEndPoint
        Private UDPAckListenerClient As UdpClient
        Private UDPAckListenerEndpoint As IPEndPoint

        Private AckBroadcastThread As Thread
        Private AckListenerThread As Thread
        Private StatusUpdateThread As Thread
        Private AckConnThread As Thread
        Private ConnThread As Thread


        Private WF_DevicesList As List(Of Device)
        Private WF_DevicesDict As Dictionary(Of IPAddress, Device)
        '{AppIDMessage, DeviceIP.ToString, DeviceName, NodeType, PreviousPointer, NextPointer}
        'worst case msg: 11bytes, 15 bytes, 15 bytes, 6 bytes, 15 bytes, 15 bytes - total 82 bytes/broadcast
        'targeting a 50Mb/s bandwidth, 6.25MB/s, so take theoretical 1% max bandwidth usage = 62,500 bytes/second
        'assume a theoretical max of 10 devices on this LAN, so allowance per device is 62,500/10 bytes/second = 6250 bytes/s
        'this means a 6250/82 broadcasts/second allowance per device = around 76 broadcasts per device per second.
        'will use 60 broadcasts/second to allow for overheads during communications
        Private ReadOnly BroadcastMessage As String = GetMessage()



        Public Sub Initialise(ParFrm As TestForm)
            ParentForm = ParFrm
            Thread.Sleep(2000)
            AppRunning = True

            WF_DevicesList = New List(Of Device)
            WF_DevicesDict = New Dictionary(Of IPAddress, Device)
            'note: pull these out to networkvariables to avoid multisocket exceptions
            UDPAckClient = New UdpClient(ClientPort) With {.EnableBroadcast = True} 'broadcasting client - all devices broadcast from 36000 to 24000
            UDPAckClient.JoinMulticastGroup(MulticastAddress)
            UDPAckEndpoint = New IPEndPoint(MulticastAddress, ServerPort) 'endpoint for broadcasters - to all LAN IP's on port 24000

            UDPAckListenerClient = New UdpClient(ServerPort)
            UDPAckListenerClient.JoinMulticastGroup(MulticastAddress)
            UDPAckListenerEndpoint = New IPEndPoint(IPAddress.Any, ServerPort)

            AckBroadcastThread = New Thread(AddressOf AckBroadcaster) With {.IsBackground = True}
            AckListenerThread = New Thread(AddressOf AckListener) With {.IsBackground = True}

            StatusUpdateThread = New Thread(AddressOf StatusChecker) With {.IsBackground = True}
            ConnThread = New Thread(AddressOf Connection) With {.IsBackground = True}
            AckConnThread = New Thread(AddressOf ProcessConnRequest) With {.IsBackground = True}


            Try
                AckBroadcastThread.Start()
                AckListenerThread.Start()
                'StatusUpdateThread.Start()
            Catch ex As Exception
                ParentForm.UpdateStatusTxt("Error initialising threads: " + ex.Message + ": " + TimeString)
            End Try
        End Sub





        Private Sub AckListener() 'udp listener server for receiving broadcasts
            ParentForm.UpdateStatusTxt("Listening to port 24000.")
            Try
                While AppRunning
                    UDPAckListenerEndpoint = New IPEndPoint(IPAddress.Any, ClientPort)
                    Dim ReceivedBytes As Byte()
                    Try
                        ReceivedBytes = UDPAckListenerClient.Receive(UDPAckListenerEndpoint)
                    Catch ex As Exception
                        ReceivedBytes = {99}
                    End Try
                    Dim Msg As String = ASCII.GetString(ReceivedBytes)

                    If Msg.StartsWith(AppIDMessage) AndAlso Not WF_DevicesDict.ContainsKey(UDPAckListenerEndpoint.Address) Then 'AndAlso Not IPAddress.Equals(UDPAckListenerEndpoint.Address, DeviceIP)
                        ParentForm.UpdateStatusTxt("New broadcaster found." + UDPAckListenerEndpoint.Address.ToString + ":" + UDPAckListenerEndpoint.Port.ToString + ", " + TimeString)
                        Dim Temp As String = ""
                        Dim ItemArr(4) As String 'to store the values for saving to a Device class instance later
                        Dim Counter As Byte = 0
                        Msg = Msg.Substring(12) + ":"
                        For Each Item As Char In Msg
                            If Item <> ":" Then
                                Temp += Item
                            Else
                                ItemArr(Counter) = Temp
                                Temp = ""
                                Counter += 1
                            End If
                        Next
                        Dim TempDevice As New Device(IPAddress.Parse(ItemArr(0)), ItemArr(1), ItemArr(2), ItemArr(3), ItemArr(4)) 'ItemArr holds the split Msg string into its corresp. fields
                        WF_DevicesList.Add(TempDevice)
                        WF_DevicesDict.Add(IPAddress.Parse(ItemArr(0)), TempDevice) 'dictionary for referencing specific Device instances
                    End If
                End While
            Catch ex As Exception
                ParentForm.UpdateStatusTxt("Error while listening to UDP: " + ex.Message + " at " + TimeString)
            End Try
        End Sub
        Private Sub AckBroadcaster() 'broadcaster thread sends (1000/n) datagrams per second, where n is the thread sleep value

            ParentForm.UpdateStatusTxt("Broadcasting to port 24000.")
            While AppRunning
                Try
                    Dim BCMsg() As Byte = ASCII.GetBytes(BroadcastMessage)
                    UDPAckClient.Send(BCMsg, BCMsg.Length, UDPAckEndpoint)
                    Thread.Sleep(17)
                Catch ex As Exception
                    ParentForm.UpdateStatusTxt("Error while sending datagram: " + ex.Message + ": " + TimeString)
                End Try
            End While
        End Sub

        Private Sub Connection(LeafIPEndpoint As IPEndPoint)
            'use a udp transmission "WFCONNECT" to send to leaf node - result should be a referential connection rather than a full logical conn.
        End Sub
        Private Sub ProcessConnRequest()
            'use a received udp transmission "WFCONNECT" to determine if connection is possible (if node is leaf node)
        End Sub

        Private Sub StatusChecker() 'thread to check for existing broadcasters' messages and status of network

        End Sub
        Public Sub DisconnectFromNetwork() 'ending the broadcast - will only happen at exit of application or disconnection from network etc.
            AppRunning = False
            Try
                'add a DISCONECTION broadcast here too e.g. "WAYFARER_V1:DISCONNECT:deviceip etc..." so that others update their records
                ParentForm.UpdateStatusTxt("Broadcast stopped to port 24000.")
                WF_DevicesList = New List(Of Device)
                WF_DevicesDict = New Dictionary(Of IPAddress, Device)
            Catch ex As Exception
                ParentForm.UpdateStatusTxt("Error in exiting broadcasting thread: " + ex.Message + ": " + TimeString)
            End Try
        End Sub
    End Module

    Module MiscLibrary
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
                If Dns.GetHostName.Length < 15 Then
                    Return Dns.GetHostName.Substring(0, 15)
                Else
                    Return Dns.GetHostName
                End If
            Catch ex As Exception
                Return "E" + TimeString.Substring(0, 2) + TimeString.Substring(3, 2) + TimeString.Substring(6, 2)
            End Try
        End Function

        Public Function GetMessage() As String
            Dim Result As String = AppIDMessage & ":" & DeviceIP.ToString & ":" & DeviceName & ":" & NodeType & ":" & PreviousPointer.ToString & ":" & NextPointer.ToString
            Return Result
        End Function
    End Module

End Namespace