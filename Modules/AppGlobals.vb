Imports System.Net
Imports System.Net.Sockets
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module AppGlobals
    Public ReadOnly DirectoryList() As String = {Synchronisation.GlobalWFPath & "\Wallets\", Synchronisation.GlobalWFPath & "\Blocks\", Synchronisation.GlobalWFPath & "\Network\"}
    Public ReadOnly ExtensionList() As String = {".wfwlt", ".wfbc", ".wfns"}
    'shortcuts for functionally abstracting the file path getting methods
    Public ReadOnly GenesisBlock As New Block("00422297B2426BB6B3D7B54D5AC18FA1166719B3C33F15C9AF439324EF623D5D", StrDup(64, "0"), 0, 355, StrDup(64, "0"), Nothing, StrDup(64, "0")) 'genesis block for all devices, hardcoded
    'constant block instance created at runtime as it is vital to keeping the blockchain integrity and error checking - due to daisy chain nature of blockchains, an error in the first block invalidates the entire chain
    Public Const ServerPort As Integer = 35000 'for receiving data inbound via non-root comms
    Public Const ClientPort As Integer = 36000 'for sending data outbound via non-root comms

    Public PrevTCP As TCPHandler = Nothing 'for prev device comms via tcp - described in the TCPHandler.vb class definition, model, and Design stage of project
    Public NextTCP As TCPHandler = Nothing 'for next device comms
    Public WFRootHandler As RootHandler = Nothing 'for root comms between root and device - this one used only if device is a root
    'Public WFRootReceiver As RootReceiver = Nothing 'this will need to be implemented as well to handle the client side of root comms


    Public IS_ROOT As Boolean = False 'boolean to store if device is a root
    Public ROOT_IP As String = "" 'stores the ip address of root node


    Public CurrentWallet As Wallet = Nothing 'wallet instance that will be used for the application, only one wallet available for login at a time obviously
    Public WFBlockchain As BlockChain ' blockchain instance for cross-form and background functionality

    Public StatusLblText As String = SetSharedLblText("Offline", "No wallet logged in", "No wallet logged in") 'holds label text across all forms, using the set function makes a custom formatted string

    Public CurrentBlockTransactionList As List(Of Transaction) 'current block's transaction pool - live transaction pool for the new block to be mined
    Public WFTransactionPool As TransactionPool 'overflow transaction pool for the main pool, to allow for new transactions during mining to be sent and held above the MAX_TRANSACT_SIZE limitation of the previous list
    Public Const MAX_TRANSACT_SIZE As Byte = 30 'max transaction pool size to start mining sequence - this could be higher or lower based on network activity, for testing, cant be too high or too low (waste of time)

    '
    'PROGRAM FLAGS
    '
    Public IsMining As Boolean = False 'flag for stopping threads and controlling program flow
    Public IsSynchronised As Boolean = False 'overall program sychronised flag
    Public IsLeaf As Boolean = False 'flag for connection request thread
    Public AppRunning As Boolean = True 'flag for saying whether user is in application - gets flipped if user clicks "Disconnect" button on any form
    Public IsMiner As Boolean = False 'flag for checking whether device is a miner or not, used for determining what proc to take during validation
    '
    '
    '

    Public ReachedMiningSeq As Boolean = False 'flag to direct program and control what is allowed to take place while mining

    Public InboundJSONBuffer As New DataBufferQueue(Of String) 'max size 200 by default, for storing data that has come in from tcphandlers
    Public OutboundJSONBuffer As New DataBufferQueue(Of String)

    Public ReceivedBlocks As List(Of Tuple(Of UInteger, Block)) 'received blocks from sync process, will move them to be a local variable in synchronisation.vb file later


    Public ReadOnly HighPriorityRRs As List(Of String) = {"ValidateNewMinedBlockResponse", "TransmitNewBlockRequest", "ValidateNewMinedBlockRequest"}.ToList 'high priority messagetypes for sorting the priority of additions to the inbound queue

    Public InboundMonitorThread As New Thread(AddressOf MonitorRecvQueue) With {.IsBackground = True}
    Public OutboundMonitorThread As New Thread(AddressOf MonitorSndQueue) With {.IsBackground = True}

    Public PreviousPointer As String = Nothing 'IPAddress of device connected previously (logically behind)
    Public NextPointer As String = Nothing 'same as above but for device connected after (logically ahead)


    Private Sub MonitorRecvQueue() 'this is the big boy processing sub - unfinished yet
        'this sub will be no joke, absolute behemoth
        While AppRunning
            If Not InboundJSONBuffer.IsEmpty And IsSynchronised Then
                Dim Data As String = InboundJSONBuffer.Dequeue
                Dim JsonObject As Object
                Try
                    JsonObject = JObject.Parse(Data)
                Catch ex As Exception
                    Continue While
                    'something cocked up during translation from JSON - discard and continue
                End Try

                'data received is in json format
                If JsonObject("AppIDMessage") <> "WAYFARER_V1" Then
                    Continue While 'error checking - delete request/response
                End If
                'json data has the correct appid - good

                Dim DeviceIP As IPAddress
                Try
                    DeviceIP = IPAddress.Parse(JsonObject("DeviceIP"))
                Catch ex As Exception
                    'the ip is messed up, so we get out of here
                    CustomMsgBox.ShowBox($"Error: received data from node is unreadable and no return data can be sent. Data: {Data}", "ERROR", False)
                    Continue While
                End Try
                'ipaddress of the data is valid too - got the basics validated


                'at this point we have the orign ip and the data in json format with the correct appidmsg - can start processing
                Select Case JsonObject("MessageType")

                    Case "NewTransactionRequest" 'this is done
                        Dim NewTransReq As NewTransactionRequest
                        Try
                            NewTransReq = JsonConvert.DeserializeObject(Of NewTransactionRequest)(Data)
                            If Not IsValidHexStr(NewTransReq.Sender, 64) Or Not IsValidHexStr(NewTransReq.Recipient, 64) Then
                                Throw New Exception 'either sender or recipient address is wrong - send a decline response to node
                            End If
                            'checking balance validity
                            Dim TempBalance As Double = ScanBlockBalanceUpdate(NewTransReq.Sender) + GetTemporaryBalanceUpdate(NewTransReq.Sender)
                            If TempBalance < NewTransReq.Quantity + NewTransReq.Fee Then
                                Throw New Exception 'sender has insufficient funds to process this transaction, send
                            End If

                        Catch ex As Exception 'you ended up here because the transaction is invalid for some reason
                            CustomMsgBox.ShowBox($"Error in RecvQueue thread: {ex.Message}. Data that threw error: {Data}", "ERROR", False)
                            Dim NewTransResp As New NewTransactionResponse(False) 'this will only go back to original sending device, as the newtransreq will not be redistributed
                            ReturnToSender(NewTransResp.GetJSONMessage, DeviceIP)
                            Continue While
                        End Try

                        'transaction is OK at this point, can continue processing
                        Dim Transact As New Transaction(NewTransReq.Sender, NewTransReq.Recipient, NewTransReq.Quantity, NewTransReq.Fee, NewTransReq.Timestamp)
                        If CurrentBlockTransactionList.Count >= MAX_TRANSACT_SIZE Then
                            WFTransactionPool.AddTransaction(Transact)
                        Else
                            CurrentBlockTransactionList.Add(Transact)
                        End If
                        Dim TransmitTrans As New TransmitTransactionRequest(Transact) 'resets request so that the device IP is now this device's ip
                        SendDataAlong(TransmitTrans.GetJSONMessage, DeviceIP)


                    Case "TransmitTransactionRequest"
                        Dim TransmitTransReq As TransmitTransactionRequest
                        Try
                            TransmitTransReq = JsonConvert.DeserializeObject(Of TransmitTransactionRequest)(Data)

                        Catch ex As Exception

                        End Try






                    Case "ValidateNewMinedBlockRequest"

                        Dim VNMBRq As ValidateNewMinedBlockRequest
                        Dim VNMBRs As ValidateNewMinedBlockResponse

                        Try
                            VNMBRq = JsonConvert.DeserializeObject(Of ValidateNewMinedBlockRequest)(Data)
                        Catch ex As Exception
                            CustomMsgBox.ShowBox($"Error in RecvQueue thread: {ex.Message}. Sent back false message", "ERROR", False)
                            VNMBRs = New ValidateNewMinedBlockResponse(False)
                            ReturnToSender(VNMBRs.GetJSONMessage, DeviceIP)
                            Continue While 'request not in correct json format, we can just exit
                        End Try
                        'at this point all block data is at least not erroneous

                        If VNMBRq.Index <> WFBlockchain.GetLastBlock.GetIndex + 1 Then
                            CustomMsgBox.ShowBox($"Error in RecvQueue thread: Index of block is wrong - rogue block. Sent back false message", "ERROR", False)
                            VNMBRs = New ValidateNewMinedBlockResponse(False)
                            ReturnToSender(VNMBRs.GetJSONMessage, DeviceIP)
                            Continue While 'request not in correct json format, we can just exit
                        End If
                        'index is ok

                        If VNMBRq.Hash.Substring(0, WFBlockchain.Difficulty) <> StrDup(WFBlockchain.Difficulty, "0") Then
                            CustomMsgBox.ShowBox($"Error in RecvQueue thread: hash received is invalid. Sent back false message", "ERROR", False)
                            VNMBRs = New ValidateNewMinedBlockResponse(False)
                            ReturnToSender(VNMBRs.GetJSONMessage, DeviceIP)
                            Continue While 'request not in correct json format, we can just exit
                        End If
                        'hash is ok

                        Try
                            Dim TransactList As List(Of Transaction) = GetTransactionListFromString(VNMBRq.Transactions)
                            Dim CurrTransList As List(Of Transaction) = CurrentBlockTransactionList
                            If Not CurrTransList.Count = TransactList.Count Then
                                Throw New Exception 'cant be referring to the same transaction pool
                            End If
                            If TransactList.Count = 0 Then
                                Exit Try 'this is ok because the other list also has 0
                            End If
                            For Each Transact As Transaction In TransactList
                                If Not CurrTransList.Contains(Transact) OrElse CUInt(Transact.Timestamp) < CUInt(WFBlockchain.GetLastBlock.GetTimestamp) Then 'OrElse Then
                                    Throw New Exception
                                End If
                            Next
                        Catch ex As Exception
                            CustomMsgBox.ShowBox($"Error in RecvQueue thread: Transaction list received doesn't match current pool. Sent back false message", "ERROR", False)
                            VNMBRs = New ValidateNewMinedBlockResponse(False)
                            ReturnToSender(VNMBRs.GetJSONMessage, DeviceIP)
                            Continue While 'request not in correct json format, we can just exit
                        End Try

                        Dim TempBlock As Block
                        Try
                            TempBlock = New Block(VNMBRq.Hash, VNMBRq.PrevHash, VNMBRq.Index, VNMBRq.Nonce, VNMBRq.Timestamp, GetTransactionListFromString(VNMBRq.Transactions), VNMBRq.Miner)
                        Catch ex As Exception
                            CustomMsgBox.ShowBox($"Error in RecvQueue: Validating new block failed - converting to block failed. Sent back false response", "ERROR", False)
                            VNMBRs = New ValidateNewMinedBlockResponse(False)
                            ReturnToSender(VNMBRs.GetJSONMessage, DeviceIP)
                            Continue While
                        End Try


                        If IsValidBlock(TempBlock, WFBlockchain.GetLastBlock) Then 'well done recruit, you passed the first stage
                            Try
                                Dim NodeIP As IPAddress = IPAddress.Parse(VNMBRq.DeviceIP)
                                If PrevTCP IsNot Nothing AndAlso IPAddress.Equals(PrevTCP.DeviceIP, NodeIP) Then
                                    Dim VNMBRp As New ValidateNewMinedBlockResponse("Accept")
                                    PrevTCP.SendData(VNMBRp.GetJSONMessage())
                                    Continue While 'thats it folks
                                End If

                                If NextTCP IsNot Nothing AndAlso IPAddress.Equals(NextTCP.DeviceIP, NodeIP) Then
                                    Dim VNMBRp As New ValidateNewMinedBlockResponse("Accept")
                                    NextTCP.SendData(VNMBRp.GetJSONMessage())
                                    Continue While 'thats it folks
                                End If
                            Catch ex As Exception
                                CustomMsgBox.ShowBox($"Error in RecvQueue thread: {ex.Message}", "ERROR", False)
                                Continue While
                            End Try
                        Else 'cheeky node tried to chuck an invalid block

                        End If




                    Case "TransmitNewBlockRequest"
                    Case "NewTransactionResponse"
                    Case "ValidateNewMinedBlockResponse"

                    Case "DisconnectRequest"
                    Case "NewTransactionResponse"
                    Case Else
                        Continue While 'received messagetype is invalid
                End Select

            End If
        End While
    End Sub


    Private Sub MonitorSndQueue() 'queue used only for tcp-handled data
        While AppRunning
            If IsSynchronised AndAlso Not OutboundJSONBuffer.IsEmpty AndAlso Not (PrevTCP Is Nothing AndAlso NextTCP Is Nothing) Then 'NAND of the two tcp handlers
                'send things out of it one by one to both tcp connections and handle null exceptions using trycatch
                Dim Data As String = OutboundJSONBuffer.Dequeue
                If PrevTCP IsNot Nothing Then
                    Try
                        PrevTCP.SendData(Data)
                    Catch ex As Exception
                        CustomMsgBox.ShowBox($"Error in SendQueue thread: {ex.Message}", "ERROR", False)
                    End Try
                End If
                If NextTCP IsNot Nothing Then
                    Try
                        NextTCP.SendData(Data)
                    Catch ex As Exception
                        CustomMsgBox.ShowBox($"Error in SendQueue thread: {ex.Message}", "ERROR", False)
                    End Try
                End If
            End If
        End While
    End Sub


    Public Sub ReturnToSender(Data As String, OriginIP As IPAddress) 'sends data back to where it came from
        If PrevTCP IsNot Nothing AndAlso Equals(OriginIP, PrevTCP.DeviceIP) Then
            PrevTCP?.SendData(Data)
        ElseIf NextTCP IsNot Nothing AndAlso Equals(OriginIP, NextTCP.DeviceIP) Then
            NextTCP?.SendData(Data)
        End If
        'uses null propagation check x2 to avoid any potential nullref exceptions
    End Sub

    Public Sub SendDataAlong(Data As String, OriginIP As IPAddress)
        'sub is used to simplify sending data on to the next node based on an origin IP
        If PrevTCP IsNot Nothing AndAlso Equals(PrevTCP.DeviceIP, OriginIP) Then
            'device that sent the data must be from the previous device, so send to nextptr node
            NextTCP?.SendData(Data)
            Exit Sub
        End If
        If NextTCP IsNot Nothing AndAlso Equals(NextTCP.DeviceIP, Data) Then
            'mustve come from nextptr device - send data to prevptr node
            PrevTCP?.SendData(Data)
        End If
    End Sub



    Public Sub DisconnectFromChain() 'incomplete
        'send prev ptr and next ptr nodes a Disconnect request
        'for each block on the current chain, check each block is saved in local
        'if root, delete root status file from the directory as the next node becomes root
    End Sub


    Public Function TimeToUnixMs(ByVal CurrentDateTime As Date) As String 'credit - https://stackoverflow.com/a/10534461/22325386 
        If CurrentDateTime.IsDaylightSavingTime = True Then
            CurrentDateTime = DateAdd(DateInterval.Hour, -1, CurrentDateTime)
        End If
        'I modified slightly to include a millisecond value as well
        Return DateDiff(DateInterval.Second, #1/1/1970#, CurrentDateTime) * 1000 + CurrentDateTime.Millisecond
        'gets long version of a unix timestamp
    End Function


    Function GetSHA256FromString(ByVal StringToHash As String) As String
        Dim Hash As Byte() = SHA256.Create.ComputeHash(Encoding.UTF8.GetBytes(StringToHash))
        Return ByteArrayToHexString(Hash)
        'as it says, returns the sha256 digest of an input string in uppercase hex
    End Function

    Function ByteArrayToHexString(ByteArray As Byte()) As String
        ' StringBuilder to store the hex string
        Dim HexBuilder As New StringBuilder()
        ' Iterate through each byte in the array
        For Each Item As Byte In ByteArray
            HexBuilder.Append(Item.ToString("X2"))
        Next
        Return HexBuilder.ToString()
        'creates and returns a hexa string from the byte array
    End Function


    Public Function SetSharedLblText(NetStatus As String, WalletName As String, Balance As String) As String
        Return "STATUS:" & NetStatus & vbCrLf & "WALLET: " & WalletName & vbCrLf & "BALANCE: " & Balance
        'used to set the lable on a form when entering and exiting a form instance
    End Function

    Public Function IsValidStr(Data As String) As Boolean
        ' Regular expression pattern to match only alphanumeric characters and not empty string
        Dim Pattern As String = "^[a-zA-Z0-9]+$"
        ' Check if the input string matches the pattern above
        Return Regex.IsMatch(Data, Pattern)
    End Function
    Public Function IsValidHexStr(Data As String, Optional Length As Integer = Nothing) As Boolean
        Dim Pattern As String = "^[A-F0-9]+$"
        If Length = Nothing Then
            Length = Data.Length
        End If
        'matches a hex string and optional length requirement
        Return Regex.IsMatch(Data, Pattern) AndAlso Data.Length = Length
    End Function


    Public Function GetOwnIP() As IPAddress 'gets private IPv4 address of the node
        Dim Hostname As String = Dns.GetHostName
        Dim DeviceIP As IPAddress = IPAddress.Loopback
        For Each HostAddress In System.Net.Dns.GetHostEntry(Hostname).AddressList()
            If HostAddress.AddressFamily = AddressFamily.InterNetwork Then
                DeviceIP = HostAddress
                Exit For
            End If
        Next
        Return DeviceIP 'gets ipv4 address of the node
    End Function

    Public Function GetDeviceName() As String 'gets a hostname (max 64 chars)
        Try
            If Dns.GetHostName.Length > 64 Then
                Return GetSHA256FromString(Dns.GetHostName)
            Else
                Return Dns.GetHostName 'return the registered device name if less than 64 length
            End If
        Catch ex As Exception
            Return GetSHA256FromString(TimeString) 'handling potential errors (shouldnt occur)
        End Try
    End Function
End Module