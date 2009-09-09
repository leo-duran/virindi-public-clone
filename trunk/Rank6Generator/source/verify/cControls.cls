VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "cControls"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Public WithEvents btnAdd As DecalControls.PushButton
Attribute btnAdd.VB_VarHelpID = -1

Public mainview As IView

Public Sub InitControls()
On Error GoTo eep

'Set up view
Set mainview = pSite1.LoadView(FileToString("view.xml"))
mainview.SetIcon 131, App.hInstance

    'Set lstCharnames = mainview.Control("lstCharnames")
    Set btnAdd = mainview.Control("btnAdd")
    'Set txtNewname = mainview.Control("txtNewname")
    'Set lblStatus = mainview.Control("lblStatus")
    'Set btnBoottime = mainview.Control("btnBoottime")
    'Set txtBoottime = mainview.Control("txtBoottime")
    'Set btnStartStop = mainview.Control("btnStartStop")


GoTo done
eep:
MsgBox "Initcontrols: " + Err.Description
done:
End Sub

Private Sub Class_Terminate()

    'Set lstCharnames = Nothing
    Set btnAdd = Nothing
    'Set txtNewname = Nothing
    'Set lblStatus = Nothing
    'Set btnBoottime = Nothing
    'Set txtBoottime = Nothing
    'Set btnStartStop = Nothing
    
    Set mainview = Nothing
End Sub


Public Sub UpdateList()
On Error GoTo eek



GoTo done
eek:
MsgBox "UpdateList " + Err.Description
done:
End Sub

'Private Sub btnBoottime_Accepted(ByVal nID As Long)
'If Int(txtBoottime.Text) >= 10 Then
'    BootTime = txtBoottime.Text
'Else
'    BootTime = 10
'End If
'txtBoottime.Text = CStr(BootTime)
'SaveBootList
'End Sub
