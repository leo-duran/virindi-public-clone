Attribute VB_Name = "Hub"
Option Explicit

Public Declare Function GetTickCount Lib "kernel32" () As Long

Public pSite2 As Decal.PluginSite2
Public pSite1 As PluginSite
Public ACHooks As Decal.ACHooks

Public Controls As New cControls
Public BootList As New Collection
Public BootTime As Long

Public Running As Boolean
Public LastTime As Long
Public LastUpdateTime As Long

Public Function FileToString(sFile As String) As String
  Dim lngFileNr As Long, sLine As String
  FileToString = ""
  lngFileNr = FreeFile(0)
  Open App.Path & "\" & sFile For Input As #lngFileNr
    Do Until EOF(lngFileNr)
      Line Input #lngFileNr, sLine
      FileToString = FileToString & sLine
    Loop
  Close #lngFileNr
  

End Function

Public Function SubStr(Str As String, L As Long, R As Long) As String
    SubStr = Left(Str, R)
    SubStr = Right(SubStr, Len(SubStr) - L + 1)
End Function

Public Sub LogWrite(S As String)
    Dim f As Long
    f = FreeFile
    Open App.Path + "\bootlog.txt" For Append As f
        Print #f, "[" + CStr(DateTime.Now) + "]" + Left(S, Len(S) - 1)
    Close f
End Sub
