Imports System.Net
Imports System.Text.RegularExpressions

Module AppGlobals
    Class GlobalData 'these will be used throughout all of the program to store and save data
        Public Shared CurrentWallet As Wallet

        Public Shared StatusLblText As String = SetSharedLblText("Offline", "No wallet logged in", "No wallet logged in") 'label text across all forms
        Public Shared TransactionPool As TransactionPool 'main transaction pool
        Public Shared MiningRewardBuffer As TransactionPool 'for saving and sending mining rewards to the miner of that block
        Public Shared Blockchain As BlockChain
        Public Shared Property IsMining As Boolean = False

        Public Shared ReadOnly DirectoryList() As String = {FileSystem.GlobalWFPath & "\Wallets\", FileSystem.GlobalWFPath & "\Blocks\", FileSystem.GlobalWFPath & "\Network\"}
        Public Shared ReadOnly ExtensionList() As String = {".wfwlt", ".wfbc", ".wfls"}

        Public Shared ReadOnly ServerPort As Integer = 24000
        Public Shared ReadOnly ClientPort As Integer = 36000
        Public Shared ReadOnly BroadcastAddress As IPAddress = IPAddress.Broadcast
        Public Shared ReadOnly BroadcastEP As New IPEndPoint(BroadcastAddress, ClientPort)
        Public Shared ReadOnly RemoteEP As New IPEndPoint(IPAddress.Any, ServerPort)
        Public Shared Online As Boolean = False 'overall program sychronised flag
        Public Shared IsLeaf As Boolean = False 'flag for connection request thread
        Public Shared AppRunning As Boolean = True 'flag for saying whether user is in application - gets flipped if user clicks "Disconnect" button on any form
        Public Shared ReceivedJSONBuffer As DataBufferQueue(Of String)
        Public Shared OutboundJSONBuffer As DataBufferQueue(Of String)

        Public Shared ReadOnly GenesisBlock As New Block("002FEDCEBD1721F3D3C71AACFB9F8BD4045DEDB599C4D76D5C9BBBE115F57249", StrDup(64, "0"), 0, 135, StrDup(64, "0"), Nothing) 'genesis block for all devices
    End Class

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
    Public Function IsValidInt(Data As String, Optional Length As Integer = Nothing) As Boolean
        Dim Pattern As String = "^[1-9]+[0-9]*$"
        If Length = Nothing Then
            Length = Data.Length
        End If
        Return Regex.IsMatch(Data, Pattern) AndAlso Data.Length = Length
    End Function
End Module