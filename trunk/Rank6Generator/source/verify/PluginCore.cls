VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "PluginCore"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

' ==============================================================================================================================
' Declarations
' ==============================================================================================================================
Implements Decal.IPlugin2

Public WithEvents eACHooks As ACHooks
Attribute eACHooks.VB_VarHelpID = -1
Public cstats As DecalFilters.CharacterStats
Public WithEvents eEcho2 As DecalFilters.EchoFilter2
Attribute eEcho2.VB_VarHelpID = -1

Private started As Boolean

Private Sub eEcho2_EchoServer(ByVal pMsg As IMessage2)
    If pMsg.Type = &HF746& Then
        started = True
        eACHooks.AddChatText "Started.", 0, 0
    End If
End Sub

Private Sub eACHooks_MessageProcessed(ByVal pbData As Long, ByVal dwSize As Long)
On Error GoTo eek

If started Then

'eACHooks.AddChatText cstats.Patron.Name, 0, 0
If cstats.Patron.Name = "" Then
    Controls.btnAdd.FaceColor = &HFF0000
Else
    Controls.btnAdd.FaceColor = &HFF&
End If

End If

GoTo done
eek:
eACHooks.AddChatText "eACHooks_MessageProcessed " + Err.Description, 8, 2
Controls.btnAdd.FaceColor = &HFFFFFF
done:
End Sub

Private Sub IPlugin2_Initialize(ByVal pSite As Decal.IPluginSite2)
On Error GoTo eek
    
    started = False
    Set pSite2 = pSite
    Set ACHooks = pSite.Hooks
    
    Set pSite1 = pSite2.PluginSite
    Set cstats = pSite1.NetworkFilter("DecalFilters.CharacterStats")
    Set eEcho2 = pSite1.NetworkFilter("DecalFilters.EchoFilter2")
    
    Controls.InitControls
    
    Controls.btnAdd.FaceColor = &HFFFFFF
    
    Set eACHooks = pSite.Hooks

GoTo done
eek:
MsgBox "init " + Err.Description
done:
End Sub

Private Sub IPlugin2_Terminate()

    Set cstats = Nothing
    Set eEcho2 = Nothing

    Set Controls = Nothing

    Set pSite1 = Nothing
    Set eACHooks = Nothing
    Set ACHooks = Nothing
    Set pSite2 = Nothing

End Sub


