Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports Newtonsoft.Json.Linq

Module AppGlobals
    Public ReadOnly DirectoryList() As String = {Synchronisation.GlobalWFPath & "\Wallets\", Synchronisation.GlobalWFPath & "\Blocks\", Synchronisation.GlobalWFPath & "\Network\"}
    Public ReadOnly ExtensionList() As String = {".wfwlt", ".wfbc", ".wfns"}

    Public ReadOnly GenesisBlock As New Block("002FEDCEBD1721F3D3C71AACFB9F8BD4045DEDB599C4D76D5C9BBBE115F57249", StrDup(64, "0"), 0, 135, StrDup(64, "0"), Nothing) 'genesis block for all devices

    Public Const ServerPort As Integer = 35000 'for receiving data inbound
    Public Const ClientPort As Integer = 36000 'for sending data outbound

    Public ReadOnly BroadcastEP As New IPEndPoint(IPAddress.Broadcast, ClientPort)
    Public ReadOnly RemoteEP As New IPEndPoint(IPAddress.Any, ServerPort)

    Public CurrentWallet As Wallet = Nothing

    Public StatusLblText As String = SetSharedLblText("Offline", "No wallet logged in", "No wallet logged in") 'label text across all forms
    Public WFTransactionPool As TransactionPool 'main transaction pool
    Public MiningRewardBuffer As TransactionPool 'for saving and sending mining rewards to the miner of that block
    Public WFBlockchain As BlockChain ' blockchain instance for cross-form and background functionality
    Public IsMining As Boolean = False 'flag for stopping threads and controlling program flow



    Public IsSynchronised As Boolean = False 'overall program sychronised flag
    Public IsLeaf As Boolean = False 'flag for connection request thread
    Public AppRunning As Boolean = True 'flag for saying whether user is in application - gets flipped if user clicks "Disconnect" button on any form


    Public InboundJSONBuffer As DataBufferQueue(Of String)
    Public OutboundJSONBuffer As DataBufferQueue(Of String)
    Public ReceivedBlocks As List(Of Tuple(Of UInteger, Block))

    Public PrevTCP As TCPHandler = Nothing 'for prev device comms
    Public NextTCP As TCPHandler = Nothing 'for next device comms

    Public ReadOnly HighPriorityRRs As List(Of String) = {"ValidateNewMinedBlockResponse", "TransmitNewBlockRequest", "ValidateNewMinedBlockRequest"}.ToList

    Public InboundMonitorThread As New Thread(AddressOf MonitorRecvQueue)
    Public OutboundMonitorThread As New Thread(AddressOf MonitorSndQueue)




    Private Sub MonitorRecvQueue()
        While AppRunning
            If Not InboundJSONBuffer.IsEmpty Then
                Dim Data As String = InboundJSONBuffer.Dequeue
                Dim JsonObject As Object = JObject.Parse(Data)
                If JsonObject("AppIDMessage") <> "WAYFARER_V1" Then
                    Continue While 'error checking - delete request/response
                End If
                Select Case JsonObject("MessageType")
                    Case "GetBlockchainDataRequest"
                        If Not IsSynchronised Then 'invalid request as device is not yet synchronised
                            Dim BlockResp As New BlockResponse(Nothing, "INVALID") 'sending a response with an INVALID status message - can check for that
                            Try
                                Dim Recipient As IPAddress = IPAddress.Parse(JsonObject("DeviceIP"))
                                Dim TempTCPHandler As New TCPHandler(Recipient)
                                TempTCPHandler.SendData(BlockResp.GetJSONMessage)
                            Catch ex As Exception

                            End Try
                        End If
                    Case "GetTransactionPoolRequest"
                    Case "NewTransactionRequest"
                    Case "ValidateNewMinedBlockRequest"
                    Case "TransmitNewBlockRequest"
                    Case "BlockResponse"
                        Try
                            Select Case JsonObject("Status")
                                Case "VALID"
                                    If JsonObject("Hash").ToString.Length <> 64 OrElse Not IsValidHexStr(JsonObject("Hash")) OrElse JsonObject("PrevHash").ToString.Length <> 64 OrElse Not IsValidHexStr(JsonObject("PrevHash")) OrElse Not Regex.IsMatch(JsonObject("Nonce"), "^[0-9]*$") OrElse Not Regex.IsMatch(JsonObject("Timestamp"), "^[0-9]*$") Then 'OrElse Then

                                    End If
                                    Dim RecvBlock As New Block(JsonObject("Hash"), JsonObject("PrevHash"), CUInt(JsonObject("Index")), CUInt(JsonObject("Nonce")), JsonObject("Timestamp"), GetTransactionListFromString(JsonObject("Transactions")))

                                Case Else
                                    Exit Select
                            End Select
                        Catch ex As Exception 'problem with block - dont save to received blocks and continue
                        End Try
                    Case "TransactionPoolResponse"
                    Case "NewTransactionResponse"
                    Case "ValidateNewMinedBlockResponse"
                    Case Else
                        Continue While 'received message is not of a correct format
                End Select

            End If
        End While
    End Sub

    Private Sub MonitorSndQueue()
        While AppRunning
            If Not OutboundJSONBuffer.IsEmpty AndAlso Not (PrevTCP Is Nothing AndAlso NextTCP Is Nothing) Then 'NAND of the two prev tcps
                'send things out of it one by one
                Dim Data As String = OutboundJSONBuffer.Dequeue
                Try
                    PrevTCP.SendData(Data)
                Catch ex As Exception
                    CustomMsgBox.ShowBox($"Error in SendQueue thread: {ex.Message}", )
                End Try
                Try
                    NextTCP.SendData(Data)
                Catch ex As Exception
                End Try
            End If
        End While
    End Sub










    Public Function TimeToUnixMs(ByVal CurrentDateTime As Date) As String 'credit - https://stackoverflow.com/a/10534461/22325386 'I modified slightly to include a millisecond value as well
        If CurrentDateTime.IsDaylightSavingTime = True Then
            CurrentDateTime = DateAdd(DateInterval.Hour, -1, CurrentDateTime)
        End If
        Return DateDiff(DateInterval.Second, #1/1/1970#, CurrentDateTime) * 1000 + CurrentDateTime.Millisecond
    End Function


    Function GetSHA256FromString(ByVal StringToHash As String) As String
        Dim Hash As Byte() = SHA256.Create.ComputeHash(Encoding.UTF8.GetBytes(StringToHash))
        Return ByteArrayToHexString(Hash)
    End Function

    Function ByteArrayToHexString(ByteArray As Byte()) As String
        ' StringBuilder to store the hex string
        Dim HexBuilder As New StringBuilder()
        ' Iterate through each byte in the array
        For Each Item As Byte In ByteArray
            HexBuilder.Append(Item.ToString("X2"))
        Next
        ' Return the final hex string
        Return HexBuilder.ToString()
    End Function


    Public Function SetSharedLblText(NetStatus As String, WalletName As String, Balance As String) As String
        Return "STATUS:" & NetStatus & vbCrLf & "WALLET: " & WalletName & vbCrLf & "BALANCE: " & Balance
    End Function

    Public Function IsValidStr(Data As String) As Boolean
        ' Regular expression pattern to match only alphanumeric characters and not empty string
        Dim Pattern As String = "^[a-zA-Z0-9]+$"
        ' Check if the input string matches the pattern
        Return Regex.IsMatch(Data, Pattern)
    End Function
    Public Function IsValidHexStr(Data As String, Optional Length As Integer = Nothing) As Boolean
        Dim Pattern As String = "^[A-F0-9]+$"
        If Length = Nothing Then
            Length = Data.Length
        End If
        Return Regex.IsMatch(Data, Pattern) AndAlso Data.Length = Length
    End Function
End Module