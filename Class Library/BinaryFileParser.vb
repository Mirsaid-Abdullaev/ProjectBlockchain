Imports System.Diagnostics.Eventing
Imports System.IO
Imports System.Text.Encoding

Module BinaryFileParser
    Const BlockchainPath As String = "D:\CS coding projects\ProjectBlockchain\BIN_BLOCK_FILES\"
    Public Sub GetBlockFromBinFile(ByVal BlockPath As String) 'as block
        Dim BlockData As String = ""
        File.SetAttributes(BlockPath, FileAttributes.ReadOnly = 0)

        Try
            Using BReader As New BinaryReader(File.Open(BlockPath, FileMode.Open), ASCII)
                Dim pos As Integer = 0
                Dim length As Integer = BReader.BaseStream.Length
                While pos < length
                    Dim value As Integer = BReader.ReadByte()
                    BlockData += Chr(value)
                    pos += 1
                End While
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        File.SetAttributes(BlockPath, FileAttributes.ReadOnly)
        MsgBox("Block data: " + BlockData) 'need to get the broken down block properties from this
    End Sub

    Public Sub GetBinFileFromBlock(ByVal CurrentBlock As Block)
        Try
            CurrentBlock.GetBlockDataAsString()
            Dim Data As String = CurrentBlock.BlockData
            Dim BData As Byte() = ASCII.GetBytes(Data)
            Using BWriter As New BinaryWriter(File.Open(BlockchainPath + "Block" + CurrentBlock.Index.ToString + ".bc", FileMode.Create))
                BWriter.Write(BData)
            End Using
            File.SetAttributes(BlockchainPath + "Block" + CurrentBlock.Index.ToString, FileAttributes.ReadOnly)
            MsgBox("Successfully created bin file!")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Module
