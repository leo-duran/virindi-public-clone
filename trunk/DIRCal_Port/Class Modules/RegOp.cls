VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "RegOp"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'© Copyright 2000-2001 Romke Soldaat. All Rights Reserved.
'-------------------------------------------------------------------
'The source code described in the article, and listed in this
'module, is copyrighted material which may not be published
'in any form without explicit prior permission from the author.
'However, you are free to use the source code in your private,
'non-commercial, projects without permission. You are allowed to
'use these functions and routines in commercial products, provided
'the documentation of these products makes a reference to the
'original source. The following reference is recommended:
'-------------------------------------------------------------------
'PART OF THIS SOFTWARE IS BASED ON SOURCE CODE, ORIGINALLY
'CREATED BY ROMKE SOLDAAT (ROMKE@SOLDAAT.COM), AND PUBLISHED IN
'MICROSOFT OFFICE & VISUAL BASIC FOR APPLICATIONS DEVELOPER,
'BY INFORMANT COMMUNICATIONS GROUP (WWW.OFFICEVBA.COM)
'-------------------------------------------------------------------

Option Explicit
DefStr S
DefLng H-I, L, N
DefVar V
DefBool B

Private Type OSVERSIONINFO
  dwOSVersionInfoSize As Long
  dwMajorVersion As Long
  dwMinorVersion As Long
  dwBuildNumber As Long
  dwPlatformId As Long
  szCSDVersion As String * 128
End Type

Private Type SECURITY_ATTRIBUTES
  nLength As Long
  lpSecurityDescriptor As Long
  bInheritHandle As Long
End Type

' RegCreateKeyEx creates the specified key. If the key already exists,
' the function opens it. The phkResult parameter receives the key handle.
Private Declare Function RegCreateKeyEx Lib "advapi32.dll" Alias "RegCreateKeyExA" ( _
  ByVal hKey As Long, ByVal lpSubKey As String, ByVal Reserved As Long, _
  ByVal lpClass As String, ByVal dwOptions As Long, ByVal samDesired As Long, _
  lpSecurityAttributes As SECURITY_ATTRIBUTES, phkResult As Long, _
  lpdwDisposition As Long) As Long

' RegCloseKey releases a handle to the specified key.
' (Key handles should not be left open any longer than necessary.)
Private Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hCurKey As Long) As Long

' RegQueryInfoKey retrieves information about the specified key,
' such as the number of subkeys and values, the length of the
' longest value and key name, and the size of the longest data
' component among the key's values.
Private Declare Function RegQueryInfoKey Lib "advapi32.dll" Alias "RegQueryInfoKeyA" ( _
  ByVal hCurKey As Long, ByVal lpClass As String, lpcbClass As Long, _
  ByVal lpReserved As Long, lpcSubKeys As Long, lpcbMaxSubKeyLen As Long, _
  lpcbMaxClassLen As Long, lpcValues As Long, lpcbMaxValueNameLen As Long, _
  lpcbMaxValueLen As Long, lpcbSecurityDescriptor As Long, _
  lpftLastWriteTime As Long) As Long

' RegEnumKeyEx enumerates subkeys of the specified open key.
' Retrieves the name (and its length) of each subkey.
Private Declare Function RegEnumKeyEx Lib "advapi32.dll" Alias "RegEnumKeyExA" ( _
  ByVal hCurKey As Long, ByVal dwIndex As Long, ByVal lpName As String, _
  lpcbName As Long, ByVal lpReserved As Long, ByVal lpClass As String, _
  lpcbClass As Long, lpftLastWriteTime As Long) As Long
  
' RegEnumValue enumerates the values for the specified open key.
' Retrieves the name (and its length) of each value, and the type,
' content and size of the data.
Private Declare Function RegEnumValue Lib "advapi32.dll" Alias "RegEnumValueA" ( _
  ByVal hCurKey As Long, ByVal dwIndex As Long, ByVal lpValueName As String, _
  lpcbValueName As Long, ByVal lpReserved As Long, lpType As Long, _
  lpData As Any, lpcbData As Long) As Long
  
' RegQueryValueEx retrieves the type, content and data for a specified value name.
' Note that if you declare the lpData parameter as String, you must pass it By Value.
Private Declare Function RegQueryValueEx Lib "advapi32.dll" Alias "RegQueryValueExA" ( _
  ByVal hCurKey As Long, ByVal lpValueName As String, ByVal lpReserved As Long, _
  lpType As Long, lpData As Any, lpcbData As Long) As Long

' RegSetValueEx sets the data and type of a specified value under a key.
Private Declare Function RegSetValueEx Lib "advapi32.dll" Alias "RegSetValueExA" ( _
  ByVal hCurKey As Long, ByVal lpValueName As String, ByVal Reserved As Long, _
  ByVal dwType As Long, lpData As Any, ByVal cbData As Long) As Long

' RegDeleteValue removes a named value from the specified key.
Private Declare Function RegDeleteValue Lib "advapi32.dll" Alias "RegDeleteValueA" ( _
  ByVal hCurKey As Long, ByVal lpValueName As String) As Long

' RegDeleteKey deletes a subkey. Under Win 95/98, also deletes all
' subkeys and values. Under Windows NT/2000, the subkey to be deleted
' must not have subkeys. The class attempts to use SHDeleteKey (see below)
' before using RegDeleteKey.
Private Declare Function RegDeleteKey Lib "advapi32.dll" Alias "RegDeleteKeyA" ( _
    ByVal hKey As Long, ByVal lpSubKey As String) As Long

' SHDeleteKey deletes a subkey and all its descendants.
' Under Windows NT 4.0, Internet Explorer 4.0 or later is required.
'Private Declare Function SHDeleteKey Lib "Shlwapi" Alias "SHDeleteKeyA" ( _
'    ByVal hKey As Long, ByVal lpSubKey As String) As Long

Private Declare Function LoadLibrary Lib "Kernel32" Alias "LoadLibraryA" ( _
  ByVal lpLibFileName As String) As Long
  
Private Declare Function FreeLibrary Lib "Kernel32" (ByVal hLibModule As Long) As Long

Private Declare Function ExpandEnvStrings Lib "Kernel32" Alias _
  "ExpandEnvironmentStringsA" (ByVal lpSrc As String, ByVal lpDst As String, _
  ByVal nSize As Long) As Long

Private Declare Function GetVersionEx Lib "Kernel32" Alias _
    "GetVersionExA" (lpVersionInformation As OSVERSIONINFO) As Long

Private Const REG_SZ = 1
Private Const REG_EXPAND_SZ = 2
Private Const REG_DWORD = 4
Private Const REG_DWORD_LITTLE_ENDIAN = REG_DWORD
Private Const REG_MULTI_SZ = 7

' The following values are only relevant under WinNT/2K,
' and are ignored by Win9x:
Private Const STANDARD_RIGHTS_READ = &H20000
Private Const STANDARD_RIGHTS_WRITE = &H20000
Private Const STANDARD_RIGHTS_ALL = &H1F0000
Private Const KEY_CREATE_LINK = &H20
Private Const KEY_QUERY_VALUE = &H1
Private Const KEY_ENUMERATE_SUB_KEYS = &H8
Private Const KEY_NOTIFY = &H10
Private Const KEY_SET_VALUE = &H2
Private Const KEY_CREATE_SUB_KEY = &H4
Private Const SYNCHRONIZE = &H100000

'Access right to query and enumerate values:
Private Const KEY_READ = ((STANDARD_RIGHTS_READ Or _
  KEY_QUERY_VALUE Or KEY_ENUMERATE_SUB_KEYS Or _
  KEY_NOTIFY) And (Not SYNCHRONIZE))
'Access right to create values and keys:
Private Const KEY_WRITE = ((STANDARD_RIGHTS_WRITE Or _
  KEY_SET_VALUE Or KEY_CREATE_SUB_KEY) And (Not SYNCHRONIZE))
'Access right to create/delete values and keys:
Private Const KEY_ALL_ACCESS = ((STANDARD_RIGHTS_ALL Or _
  KEY_QUERY_VALUE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY Or _
  KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY Or KEY_CREATE_LINK) _
  And (Not SYNCHRONIZE))

Private lRequiredAccess
Private lPreviousAccess

' Return values for all Registry functions:
Private Const ERROR_SUCCESS = 0

'Property variables:
Private lRoot 'default is HKEY_LOCAL_MACHINE
Private lOptions
Private strKeyName
Private strValueName
Private vData

' Variables set in GetKeyHandle:
Private hCurKey
Private nSubKeys
Private nValues
Private lMaxSubKeyLen
Private lMaxValueNameLen
Private lMaxValueLen

Private bIsWinNT

Public Enum RegOptions 'variable: lOptions
  StoreNumbersAsStrings = 1
  ReturnMultiStringsAsArrays = 2
  ExpandEnvironmentStrings = 4
  ShowErrorMessages = 8
End Enum

Public Enum RegRoot 'variable: lRoot
  HKEY_CLASSES_ROOT = &H80000000
  HKEY_CURRENT_USER = &H80000001  'default
  HKEY_LOCAL_MACHINE = &H80000002
End Enum

' Message constants:
Private Const ERROR_NO_KEY As String = _
  "No Key name specified!"
Private Const ERROR_NO_HANDLE = _
  "Could not open Registry Key!"
Private Const ERR_MSG_NO_OVERWRITE As String = _
  "Existing value has unsupported data type " & _
    "and will not be overwritten"
Private Const RETURN_UNSUPPORTED As String = _
  "(unsupported data format)"

Private ValueList As Object

Property Let Root(lProp As RegRoot)
'Don't accept an invalid Root value:
Select Case lProp
Case HKEY_CLASSES_ROOT, HKEY_CURRENT_USER, _
  HKEY_LOCAL_MACHINE
  'all is well
Case Else
  lRoot = HKEY_CURRENT_USER
End Select
If lProp <> lRoot Then
  lRoot = lProp
  If Len(strKeyName) Then
    GetKeyHandle lRoot, strKeyName
  End If
End If
lRoot = lProp
End Property

Property Let Key(strProp)
'Don't accept an empty key name:
If Len(strProp) = 0 Then Exit Property
If Len(strKeyName) = 0 Then 'first time
  strKeyName = strProp
ElseIf StrComp(strProp, strKeyName, vbTextCompare) <> 0 Then
  strKeyName = strProp
  GetKeyHandle lRoot, strKeyName
Else
End If
End Property

Property Let Options(lProp As RegOptions)
'Don't accept an invalid Options value:
Select Case lProp
Case 0 To 15: lOptions = lProp
Case Else: End Select
End Property

Property Let Value(Optional ValueName As String, vValue)
If IsEmpty(vValue) Then Exit Property Else vData = vValue

If bIsWinNT Then lRequiredAccess = KEY_WRITE Or KEY_READ

If PropertiesOK Then
  ' First see if this is an existing value, and,
  ' if so, what data type we have here:
  Dim strBuffer, lBuffer, lType
  If RegQueryValueEx(hCurKey, ValueName, 0, lType, _
    ByVal strBuffer, lBuffer) = ERROR_SUCCESS Then
    ' Make sure our new value is the same data type:
    Select Case lType
    Case REG_SZ, REG_EXPAND_SZ 'existing string
      vData = CStr(vData)
    Case REG_DWORD, REG_DWORD_LITTLE_ENDIAN 'existing long
      vData = CLng(vData)
    Case REG_MULTI_SZ 'existing array
      vData = CVar(vData)
    Case Else
      ShowErrMsg ERR_MSG_NO_OVERWRITE
      Exit Property
    End Select
  End If
  
  If (lOptions And StoreNumbersAsStrings) Then
    If IsNumeric(vData) Then vData = CStr(vData)
  End If
  
  'If nameless "(Default)" value:
  If Len(ValueName) = 0 Then vData = CStr(vData)
  
  ' Look at the data type of vData, and store it
  ' in the appropriate Registry format
  If VarType(vData) And vbArray Then  '8192
    Dim sTemp As String
    'REG_MULTI_SZ values must end with 2 null characters:
    sTemp = Join(vData, vbNullChar) & String$(2, 0)
    Call RegSetValueEx(hCurKey, ValueName, 0, REG_MULTI_SZ, _
      ByVal sTemp, Len(sTemp))
  Else
  
    Select Case VarType(vData)
    Case vbInteger, vbLong
      Call RegSetValueEx(hCurKey, ValueName, 0, REG_DWORD, _
        CLng(vData), 4)
    Case vbString
      If ContainsEnvString(CStr(vData)) Then
        Call RegSetValueEx(hCurKey, ValueName, 0, REG_EXPAND_SZ, _
          ByVal CStr(vData), Len(vData) + 1)
      Else
        Call RegSetValueEx(hCurKey, ValueName, 0, REG_SZ, _
          ByVal CStr(vData), Len(vData) + 1)
      End If
    Case Else 'store any other data type as string
      Call RegSetValueEx(hCurKey, ValueName, 0, REG_SZ, _
        ByVal CStr(vData), Len(vData) + 1)
    End Select
    
  End If
  
  'Update Value Count:
  Call RegQueryInfoKey(hCurKey, vbNullString, 0, 0, 0, _
    0, 0, nValues, 0, 0, 0, 0)
  'Clear the values database:
  ValueList.RemoveAll
End If
End Property

Property Get Value(Optional ValueName As String) As Variant
With ValueList
  If .Count = 0 Then FillDataList
  If .Exists(ValueName) Then Value = .Item(ValueName)
End With
End Property

Property Get AllValues() As Variant
If bIsWinNT Then lRequiredAccess = KEY_READ

If PropertiesOK Then
  If nValues = 0 Then Exit Property
  
  With ValueList
    If .Count = 0 Then FillDataList
    If .Count Then
      Dim i, vKeys, vItems
      vKeys = .Keys
      vItems = .items
      ReDim vTemp(.Count - 1, 1)
      For i = 0 To .Count - 1
        vTemp(i, 0) = vKeys(i)
        vTemp(i, 1) = vItems(i)
      Next
      AllValues = vTemp
    End If
  End With

End If
End Property

Property Get AllKeys() As Variant
If bIsWinNT Then lRequiredAccess = KEY_READ

If PropertiesOK Then
  If nSubKeys = 0 Then Exit Property
  
  Dim i: ReDim vTemp(nSubKeys - 1)
  For i = 0 To nSubKeys - 1
    strKeyName = String$(lMaxSubKeyLen + 1, 0)
    If RegEnumKeyEx(hCurKey, i, strKeyName, lMaxSubKeyLen + 1, 0, _
      vbNullString, 0, 0) = ERROR_SUCCESS Then
        vTemp(i) = TrimNull(strKeyName)
    End If
  Next
  AllKeys = vTemp

End If
End Property

Function DeleteValue(Optional ValueName As String) As Boolean

If bIsWinNT Then lRequiredAccess = KEY_ALL_ACCESS

If PropertiesOK Then
  DeleteValue = (RegDeleteValue(hCurKey, ValueName) = ERROR_SUCCESS)
  ' Update Value Count:
  If DeleteValue Then
    Call RegQueryInfoKey(hCurKey, vbNullString, 0, 0, 0, _
      0, 0, nValues, 0, 0, 0, 0)
    ' Empty the database:
    ValueList.RemoveAll
  End If
End If
End Function

'Function DeleteKey() As Boolean
'If Len(strKeyName) = 0 Then ShowErrMsg ERROR_NO_KEY: Exit Function

'Dim n, strLastKey
'n = InStrRev(strKeyName, "\")
'If n > 0 And n < Len(strKeyName) Then
'  strLastKey = Mid$(strKeyName, n + 1)
'  strKeyName = Left$(strKeyName, n - 1)
'
'  If bIsWinNT Then lRequiredAccess = KEY_ALL_ACCESS
'
'  Call GetKeyHandle(lRoot, strKeyName)
'  If hCurKey = 0 Then Exit Function
'  If ShlwapiInstalled Then
'    ' this should always work:
'    DeleteKey = (SHDeleteKey(hCurKey, strLastKey) = ERROR_SUCCESS)
'  Else
'    ' this will only work under Win95/98:
'    DeleteKey = (RegDeleteKey(hCurKey, strLastKey) = ERROR_SUCCESS)
'  End If
'  'Update Key and Value Count:
'  If DeleteKey Then
'    Call RegQueryInfoKey(hCurKey, vbNullString, 0, 0, nSubKeys, _
'      0, 0, 0, 0, 0, 0, 0)
'    ValueList.RemoveAll
'  End If
'End If

'End Function

Property Get ValueCount() As Long
If PropertiesOK Then ValueCount = nValues
End Property

Property Get KeyCount() As Long
If PropertiesOK Then KeyCount = nSubKeys
End Property

Private Function PropertiesOK() As Boolean
If Len(strKeyName) = 0 Then ShowErrMsg ERROR_NO_KEY: Exit Function
If lPreviousAccess Then
  If lRequiredAccess <> lPreviousAccess Then CloseCurrentKey
End If
If hCurKey = 0 Then Call GetKeyHandle(lRoot, strKeyName)
If hCurKey = 0 Then ShowErrMsg ERROR_NO_HANDLE: Exit Function
PropertiesOK = True
End Function

Private Sub Class_Initialize()
lRoot = HKEY_CURRENT_USER
bIsWinNT = IsWinNT
If bIsWinNT Then lRequiredAccess = KEY_READ

On Error Resume Next
Set ValueList = CreateObject("Scripting.Dictionary")
If IsObject(ValueList) Then
  ValueList.CompareMode = vbTextCompare
Else
End If

End Sub

Private Sub Class_Terminate()
CloseCurrentKey
Set ValueList = Nothing
End Sub

Private Sub CloseCurrentKey()
If hCurKey Then
  Call RegCloseKey(hCurKey)
  hCurKey = 0
End If
End Sub

Private Sub GetKeyHandle(lKey, strKey)
CloseCurrentKey
If lKey = 0 Then lKey = HKEY_CURRENT_USER
Dim SA As SECURITY_ATTRIBUTES
Call RegCreateKeyEx(lKey, strKey, 0, vbNull, 0, _
  lRequiredAccess, SA, hCurKey, 0)
  If hCurKey Then
    Call RegQueryInfoKey(hCurKey, vbNullString, 0, 0, nSubKeys, _
      lMaxSubKeyLen, 0, nValues, lMaxValueNameLen, lMaxValueLen, _
      0, 0)
    ValueList.RemoveAll
    lPreviousAccess = lRequiredAccess
  End If
End Sub

Private Function TrimNull(ByVal strIn) As String
TrimNull = left$(strIn, InStr(strIn, vbNullChar) - 1)
End Function

Private Function TrimDoubleNull(ByVal strIn) As String
If Len(strIn) Then TrimDoubleNull = _
  left$(strIn, InStr(strIn, String$(2, 0)) - 1)
End Function

Private Function ExpandString(strIn) As String
Dim nChars, strBuff, nBuffSize
nBuffSize = 1024
strBuff = String$(nBuffSize, 0)
nChars = ExpandEnvStrings(strIn, strBuff, nBuffSize)
If nChars Then ExpandString = left$(strBuff, nChars - 1)
End Function

Private Function ShlwapiInstalled() As Boolean
Dim hLib As Long
hLib = LoadLibrary("Shlwapi")
If hLib Then
  ShlwapiInstalled = True
  FreeLibrary hLib
End If
End Function

Private Function ContainsEnvString(ByVal strTest) As Boolean
Const PCT As String = "%"
' see if there is a percent sign
Dim n As Long: n = InStr(strTest, PCT)
If n = 0 Then Exit Function
' see if there is a second percent sign
If n = InStrRev(strTest, PCT) Then Exit Function
'now we have a potential environment string
Dim Env As String, EnvSplit() As String
Dim i As Long
For i = 1 To 100
  Env = Environ(i)
  If Len(Env) Then
    EnvSplit = Split(Env, "=")
    If InStr(1, strTest, PCT & EnvSplit(0) & PCT, vbTextCompare) Then
      ContainsEnvString = True
      Exit For
    End If
  Else
    Exit For
  End If
Next
End Function

Private Sub ShowErrMsg(strMsg)
If (lOptions And ShowErrorMessages) Then
  MsgBox strMsg, vbExclamation, "Registry Error"
Else
  Debug.Print strMsg
End If
End Sub

Private Function IsWinNT()
'Returns True if the OS is Windows NT/2000
Const VER_PLATFORM_WIN32_NT As Long = 2
Dim osvi As OSVERSIONINFO
osvi.dwOSVersionInfoSize = Len(osvi)
GetVersionEx osvi
IsWinNT = (osvi.dwPlatformId = VER_PLATFORM_WIN32_NT)
End Function

Private Sub FillDataList(Optional Key As String)
If Len(Key) Then strKeyName = Key
If Len(strKeyName) = 0 Then ShowErrMsg ERROR_NO_KEY: Exit Sub

If bIsWinNT Then lRequiredAccess = KEY_READ

If PropertiesOK Then
  If nValues = 0 Then Exit Sub
    
  ValueList.RemoveAll
  
  Dim i, lValuename, lType, lBuffer, strValue, strBuffer
  For i = 0 To nValues - 1
    lValuename = lMaxValueNameLen + 1
    strValue = String$(lValuename, 0)
    lBuffer = lMaxValueLen + 1
    strBuffer = String$(lBuffer, 0)
    If RegEnumValue(hCurKey, i, strValue, lValuename, 0, lType, _
      ByVal strBuffer, lBuffer) = ERROR_SUCCESS Then
        strValue = TrimNull(strValue)
        Select Case lType
        Case REG_SZ
          ValueList(strValue) = TrimNull(strBuffer)
        Case REG_EXPAND_SZ
          If (lOptions And ExpandEnvironmentStrings) Then
            ValueList(strValue) = ExpandString(TrimNull(strBuffer))
          Else
            ValueList(strValue) = TrimNull(strBuffer)
          End If
        Case REG_MULTI_SZ
          If (lOptions And ReturnMultiStringsAsArrays) Then
            ValueList(strValue) = Split(TrimDoubleNull(strBuffer), vbNullChar)
          Else
            ValueList(strValue) = TrimDoubleNull(strBuffer)
          End If
        Case REG_DWORD, REG_DWORD_LITTLE_ENDIAN
          Dim nBuffer
          If RegEnumValue(hCurKey, i, strValue, Len(strValue) + 1, _
            0, REG_DWORD, nBuffer, 4) = ERROR_SUCCESS Then
              ValueList(strValue) = nBuffer
          End If
        Case Else
          ValueList(strValue) = RETURN_UNSUPPORTED
        End Select
    End If
  Next
  
End If
End Sub


