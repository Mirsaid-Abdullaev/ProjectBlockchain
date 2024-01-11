'Imports System.Text
'Imports System.Math
'Imports System.Numerics.BitOperations

'Module SHA256
'    Public ReadOnly ConstantsH As UInteger() = {&H6A09E667UI, &HBB67AE85UI, &H3C6EF372UI, &HA54FF53AUI, &H510E527FUI, &H9B05688CUI, &H1F83D9ABUI, &H5BE0CD19UI}

'    Public ReadOnly ConstantsK As UInteger() = {&H428A2F98UI, &H71374491UI, &HB5C0FBCFUI, &HE9B5DBA5UI, &H3956C25BUI, &H59F111F1UI, &H923F82A4UI, &HAB1C5ED5UI,
'&HD807AA98UI, &H12835B01UI, &H243185BEUI, &H550C7DC3UI, &H72BE5D74UI, &H80DEB1FEUI, &H9BDC06A7UI, &HC19BF174UI,
'&HE49B69C1UI, &HEFBE4786UI, &HFC19DC6UI, &H240CA1CCUI, &H2DE92C6FUI, &H4A7484AAUI, &H5CB0A9DCUI, &H76F988DAUI,
'&H983E5152UI, &HA831C66DUI, &HB00327C8UI, &HBF597FC7UI, &HC6E00BF3UI, &HD5A79147UI, &H6CA6351UI, &H14292967UI,
'&H27B70A85UI, &H2E1B2138UI, &H4D2C6DFCUI, &H53380D13UI, &H650A7354UI, &H766A0ABBUI, &H81C2C92EUI, &H92722C85UI,
'&HA2BFE8A1UI, &HA81A664BUI, &HC24B8B70UI, &HC76C51A3UI, &HD192E819UI, &HD6990624UI, &HF40E3585UI, &H106AA070UI,
'&H19A4C116UI, &H1E376C08UI, &H2748774CUI, &H34B0BCB5UI, &H391C0CB3UI, &H4ED8AA4AUI, &H5B9CCA4FUI, &H682E6FF3UI,
'&H748F82EEUI, &H78A5636FUI, &H84C87814UI, &H8CC70208UI, &H90BEFFFAUI, &HA4506CEBUI, &HBEF9A3F7UI, &HC67178F2UI}

'    Function GetSHA256HashFromString(ByVal BlockData As String) As String
'        Dim SHA256Hash As String = ""
'        Dim BinaryData As String = ""
'        Dim DataAsBytes As Byte() = Encoding.ASCII.GetBytes(BlockData)

'        For Each Character As Byte In DataAsBytes
'            BinaryData &= ByteToBinary(Character, 0)
'        Next
'        'at this point, function has a binary string for the data
'        Dim LengthAppend As UInteger = BinaryData.Length
'        BinaryData &= "1"
'        While (BinaryData.Length + 64) Mod 512 <> 0
'            BinaryData &= "0"
'        End While
'        BinaryData &= ByteToBinary(LengthAppend, 56)
'        'at this point, the binary is in a form where we can start hashing it
'        Dim NewBinaryData As Byte() = BinaryToByteArray(BinaryData)
'        Dim NoOfChunks As UInteger = Ceiling(NewBinaryData.Length * 8 / 512)
'        Dim MessageSchedule(NoOfChunks - 1)() As Byte
'        For I As Byte = 1 To NoOfChunks
'            Dim TempArray(63) As Byte
'            Array.Copy(NewBinaryData, TempArray, 63)
'            MessageSchedule(I - 1) = TempArray
'            If NewBinaryData.Length >= 64 Then
'                Dim TempArray2(NewBinaryData.Length - 65) As Byte
'                Array.ConstrainedCopy(NewBinaryData, 64, TempArray2, 0, NewBinaryData.Length - 64)
'                ReDim NewBinaryData(TempArray2.Length - 1)
'                NewBinaryData = TempArray2
'            End If


'        Next

'        Return SHA256Hash
'    End Function

'    Function ByteToBinary(ByVal Data As UInteger, ByVal TrailingCount As Byte) As String 'returns n-bit binary rep of any string
'        Dim Trailing As Byte = 7 - Floor(Math.Log2(Data)) + TrailingCount 'for n-bit representation, set trailing count = n - 8
'        Dim BinaryStr As String = ""
'        For i As Byte = 1 To Trailing
'            BinaryStr += "0"
'        Next
'        BinaryStr &= Convert.ToString(Data, 2)
'        Return BinaryStr
'    End Function
'    Function BinaryToByteArray(ByVal Data As String) As Byte() 'only works for data of length 8x
'        Dim ResultByteARR As Byte() = Array.Empty(Of Byte)()
'        Dim Count As UInteger = 0
'        For I As UInteger = 0 To Data.Length - 8 Step 8
'            ReDim Preserve ResultByteARR(ResultByteARR.Length)
'            ResultByteARR(Count) = Convert.ToUInt16(Data.Substring(I, 8), 2)
'            Count += 1
'        Next
'        'byte array with decimal values

'        Return ResultByteARR
'    End Function

'    Function CreateMessageSchedule(ByVal Data As Byte())
'        Return True
'    End Function
'End Module
Imports System.Text
Imports System.Security.Cryptography
Module Cryptography
    Function GetSHA256HashFromString(ByVal StringToHash As String) As String
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
End Module