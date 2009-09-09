VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "dIRCalIRC"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'   dIRCal main class file
'   Originally written by Lane (lane_meyer@acdev.org)
'   Updated for ToD by Virindi-Inquisitor (rubyduchess@hotmail.com)
'
'   This version is available at http://questers.polarism.com/viplugins
'   The original version is available at http://dircal.sourceforge.net/
'
'
'   Original credits:
'
'   Major Thanks to Jaime Cabral (cabral@n-link.com) - http://darkimages.cjb.net/
'       and Temmu (temmu_acgm@hotmail.com) - http://icdweb.cc.purdue.edu/~crosbyz/acirc/
'       and Hazridi (hazridi@acdev.org) - http://hazridi.acdev.org/
'       and Your_God (yourgod@acdev.org) - http://yourgod.acdev.org/
'       and the authors of Decal - http://decaldev.sourceforge.net/
'       as well as all the others in #acdev on SorceryNet who assisted me
'
'
Option Explicit

Implements Decal.IPlugin2
'Implements IPluginSink

Private PluginSite As PluginSite
Private PluginSite2 As PluginSite2

Private VCS As VirindiChatSystem.PluginCore

Private WithEvents eACHooks As ACHooks
Attribute eACHooks.VB_VarHelpID = -1

'Main
Private ViewSchema As String

Public WithEvents tcp As Winsock
Attribute tcp.VB_VarHelpID = -1

Private WithEvents MainView As View
Attribute MainView.VB_VarHelpID = -1

Private WithEvents edServer As DecalControls.Edit
Attribute edServer.VB_VarHelpID = -1
Private WithEvents edServerPort As DecalControls.Edit
Attribute edServerPort.VB_VarHelpID = -1
Private WithEvents edNick As DecalControls.Edit
Attribute edNick.VB_VarHelpID = -1
Private WithEvents edChannel As DecalControls.Edit
Attribute edChannel.VB_VarHelpID = -1
Private WithEvents edPassword As DecalControls.Edit
Attribute edPassword.VB_VarHelpID = -1
Private WithEvents btnConnect As DecalControls.PushButton
Attribute btnConnect.VB_VarHelpID = -1
Private WithEvents stStatus As DecalControls.StaticText
Attribute stStatus.VB_VarHelpID = -1

Private WithEvents btnOptions As DecalControls.PushButton
Attribute btnOptions.VB_VarHelpID = -1

Private stCurrentChannel As DecalControls.StaticText
Private WithEvents lChannel As DecalControls.List
Attribute lChannel.VB_VarHelpID = -1
Private stUsersInChannel As DecalControls.StaticText
Private WithEvents lUser As DecalControls.List
Attribute lUser.VB_VarHelpID = -1

Private WithEvents chkGreenText As DecalControls.CheckBox
Attribute chkGreenText.VB_VarHelpID = -1

'OptionView
Private WithEvents OptionView As View
Attribute OptionView.VB_VarHelpID = -1

Private stDefault As DecalControls.StaticText

Private stServerOption As DecalControls.StaticText
Private WithEvents edServerOption As DecalControls.Edit
Attribute edServerOption.VB_VarHelpID = -1
Private stServerPortOption As DecalControls.StaticText
Private WithEvents edServerPortOption As DecalControls.Edit
Attribute edServerPortOption.VB_VarHelpID = -1
Private stNickOption As DecalControls.StaticText
Private WithEvents edNickOption As DecalControls.Edit
Attribute edNickOption.VB_VarHelpID = -1
Private stChannelOption As DecalControls.StaticText
Private WithEvents edChannelOption As DecalControls.Edit
Attribute edChannelOption.VB_VarHelpID = -1
Private stPasswordOption As DecalControls.StaticText
Private WithEvents edPasswordOption As DecalControls.Edit
Attribute edPasswordOption.VB_VarHelpID = -1
Private WithEvents cbPerformOnConnectOption As DecalControls.CheckBox
Attribute cbPerformOnConnectOption.VB_VarHelpID = -1
Private WithEvents cbAutoConnectOption As DecalControls.CheckBox
Attribute cbAutoConnectOption.VB_VarHelpID = -1

Private WithEvents cbAction As DecalControls.CheckBox
Attribute cbAction.VB_VarHelpID = -1
Private WithEvents cbCTCP As DecalControls.CheckBox
Attribute cbCTCP.VB_VarHelpID = -1
Private WithEvents cbHighlight As DecalControls.CheckBox
Attribute cbHighlight.VB_VarHelpID = -1
Private WithEvents cbInfo As DecalControls.CheckBox
Attribute cbInfo.VB_VarHelpID = -1
Private WithEvents cbNoticeAuth As DecalControls.CheckBox
Attribute cbNoticeAuth.VB_VarHelpID = -1
Private WithEvents cbInvite As DecalControls.CheckBox
Attribute cbInvite.VB_VarHelpID = -1
Private WithEvents cbJoin As DecalControls.CheckBox
Attribute cbJoin.VB_VarHelpID = -1
Private WithEvents cbKick As DecalControls.CheckBox
Attribute cbKick.VB_VarHelpID = -1
Private WithEvents cbMode As DecalControls.CheckBox
Attribute cbMode.VB_VarHelpID = -1
Private WithEvents cbNick As DecalControls.CheckBox
Attribute cbNick.VB_VarHelpID = -1
Private WithEvents cbActiveChannel As DecalControls.CheckBox
Attribute cbActiveChannel.VB_VarHelpID = -1
Private WithEvents cbInactiveChannel As DecalControls.CheckBox
Attribute cbInactiveChannel.VB_VarHelpID = -1
Private WithEvents cbNotice As DecalControls.CheckBox
Attribute cbNotice.VB_VarHelpID = -1
Private WithEvents cbNotify As DecalControls.CheckBox
Attribute cbNotify.VB_VarHelpID = -1
Private WithEvents cbOther As DecalControls.CheckBox
Attribute cbOther.VB_VarHelpID = -1
Private WithEvents cbOwn As DecalControls.CheckBox
Attribute cbOwn.VB_VarHelpID = -1
Private WithEvents cbPart As DecalControls.CheckBox
Attribute cbPart.VB_VarHelpID = -1
Private WithEvents cbQuit As DecalControls.CheckBox
Attribute cbQuit.VB_VarHelpID = -1
Private WithEvents cbTopic As DecalControls.CheckBox
Attribute cbTopic.VB_VarHelpID = -1
Private WithEvents cbWallops As DecalControls.CheckBox
Attribute cbWallops.VB_VarHelpID = -1
Private WithEvents cbWhois As DecalControls.CheckBox
Attribute cbWhois.VB_VarHelpID = -1

Private bGreenText As Boolean

Private bAction As Boolean
Private bCTCP As Boolean
Private bHighlight As Boolean
Private bInfo As Boolean
Private bNoticeAuth As Boolean
Private bInvite As Boolean
Private bJoin As Boolean
Private bKick As Boolean
Private bMode As Boolean
Private bNick As Boolean
Private bActiveChannel As Boolean
Private bInactiveChannel As Boolean
Private bNotice As Boolean
Private bNotify As Boolean
Private bOther As Boolean
Private bOwn As Boolean
Private bPart As Boolean
Private bQuit As Boolean
Private bTopic As Boolean
Private bWallops As Boolean
Private bWhois As Boolean

Private WithEvents chTextType As DecalControls.Choice
Attribute chTextType.VB_VarHelpID = -1
Private edColorTest As DecalControls.Edit
Private WithEvents lColors As DecalControls.List
Attribute lColors.VB_VarHelpID = -1

Private WithEvents lPerform As DecalControls.List
Attribute lPerform.VB_VarHelpID = -1
Private WithEvents edPerform As DecalControls.Edit
Attribute edPerform.VB_VarHelpID = -1
Private linePerform As Long
Private WithEvents btnPerformOK As DecalControls.PushButton
Attribute btnPerformOK.VB_VarHelpID = -1
Private WithEvents btnPerformCancel As DecalControls.PushButton
Attribute btnPerformCancel.VB_VarHelpID = -1

Private OptionSite As String
'Private Connection As String
'Private Filters As String
'Private Colors As String
'Private Perform As String
'Private Notify As String
'Private Logging As String
'Private Ignore As String

Private WithEvents lOptions As DecalControls.List
Attribute lOptions.VB_VarHelpID = -1
'Private WithEvents btnOptionsApply As DecalControls.PushButton
Private WithEvents btnOptionsOK As DecalControls.PushButton
Attribute btnOptionsOK.VB_VarHelpID = -1
'Private WithEvents btnOptionsCancel As DecalControls.PushButton

Private bAutoConnect As Boolean

'General
Private bCommandDebug As Boolean
Private bMessageDebug As Boolean
Private bIRC As Boolean
Private b366 As Boolean
Private bSendComplete As Boolean

Private strNickName As String
Private strServerPort As String
Private strServer As String
Private strChannel As String

Private strReplyTo As String
Private strRetellTo As String

Private NotifyList() As String
Private ActiveNotifyList() As String
Private Reg As RegOp

Private PingStart As Long
Private WhoAmI As String
Private MyGUID As Long
Private MyCoords As String

Enum eState
    STATE_CONNECTED = 0
    STATE_CONNECTING = 1
    STATE_DISCONNECTED = 2
    STATE_IRCCHAT = 3
End Enum

Private WithEvents NetEcho As EchoFilter2
Attribute NetEcho.VB_VarHelpID = -1

Private Sub btnConnect_Accepted(ByVal nID As Long)
On Error GoTo Error_Handler:
    Select Case btnConnect.Text
        Case "Connect"
            If (notNULL(edServer.Text) <> "") Then
                strServer = notNULL(edServer.Text)
            Else
                SetStatus ("Invalid Server!")
                Exit Sub
            End If
    
            If (notNULL(edServerPort.Text) <> "") Then
                strServerPort = notNULL(edServerPort.Text)
            Else
                SetStatus ("Invalid Server Port!")
                Exit Sub
            End If
    
            If (notNULL(edNick.Text) <> "") And _
            (InStr(notNULL(edNick.Text), " ") = 0) And _
            (InStr(notNULL(edNick.Text), ".") = 0) And _
            (InStr(notNULL(edNick.Text), ",") = 0) And _
            (InStr(notNULL(edNick.Text), "!") = 0) And _
            (InStr(notNULL(edNick.Text), "~") = 0) And _
            (InStr(notNULL(edNick.Text), "@") = 0) And _
            (InStr(notNULL(edNick.Text), "#") = 0) And _
            (InStr(notNULL(edNick.Text), "$") = 0) And _
            (InStr(notNULL(edNick.Text), "%") = 0) And _
            (InStr(notNULL(edNick.Text), "&") = 0) And _
            (InStr(notNULL(edNick.Text), "*") = 0) And _
            (InStr(notNULL(edNick.Text), "(") = 0) And _
            (InStr(notNULL(edNick.Text), ")") = 0) And _
            (InStr(notNULL(edNick.Text), "+") = 0) And _
            (InStr(notNULL(edNick.Text), "=") = 0) And _
            (InStr(notNULL(edNick.Text), ":") = 0) And _
            (InStr(notNULL(edNick.Text), ";") = 0) And _
            (InStr(notNULL(edNick.Text), "'") = 0) And _
            (InStr(notNULL(edNick.Text), """") = 0) And _
            (InStr(notNULL(edNick.Text), "/") = 0) And _
            (InStr(notNULL(edNick.Text), "?") = 0) _
            Then
                strNickName = notNULL(edNick.Text)
            Else
                SetStatus ("Invalid Nickname!")
                Exit Sub
            End If
            
            SetStatus ("Looking up host " & strServer)
            If tcp.state = sckClosed Then
                tcp.Connect strServer, Val(strServerPort)
            Else
                tcp.Close
                tcp.Connect strServer, Val(strServerPort)
            End If
            btnConnect.Text = "Cancel"
            'MainView.SetIcon &H6000513
            SetMainIcon STATE_CONNECTING

        Case "Disconnect"
            QuitIRC ("")
    
        Case "Cancel"
            strChannel = ""
            stCurrentChannel.Text = "Current Channel:"
            stUsersInChannel.Text = "Users in Channel:"
            lChannel.Clear
            lUser.Clear
            strNickName = ""
            strServer = ""
            strServerPort = ""
            If Not tcp.state = sckClosed Then tcp.Close
            SetStatus "Disconnected!"
            btnConnect.Text = "Connect"
            'MainView.SetIcon &H600137B
            SetMainIcon STATE_DISCONNECTED
    End Select
Exit Sub

Error_Handler:

    Call ERRORROUTINE("Connect Error!! - ", Err, Error$(Err))
    Resume Next
End Sub

Private Sub btnOptions_Accepted(ByVal nID As Long)
    Set OptionView = PluginSite.LoadView(OptionSite)
    OptionInit
    MainView.Deactivate
    OptionView.Activate
End Sub

Private Sub btnOptionsOK_Accepted(ByVal nID As Long)
    WriteRegSettings
    LoadRegSettings
    
    MainView.Activate
    Set OptionView = Nothing
End Sub

Private Sub btnPerformCancel_Accepted(ByVal nID As Long)
    edPerform.Text = ""
End Sub

Private Sub LoadPerform()
    Dim intFileNumber As Integer
    Dim tmpPerformINI As String
    Dim nRow As Long
    
    lPerform.Clear
    intFileNumber = FreeFile
    
    Open Reg.Value & "\perform.ini" For Input As #intFileNumber
    While Not EOF(intFileNumber)
        Line Input #intFileNumber, tmpPerformINI
        nRow = lPerform.AddRow
        lPerform.Data(0, nRow) = tmpPerformINI
        lPerform.Data(1, nRow, 0) = App.hInstance
        lPerform.Data(1, nRow, 1) = 128
        lPerform.Data(2, nRow, 0) = App.hInstance
        lPerform.Data(2, nRow, 1) = 129
        lPerform.Data(3, nRow, 0) = App.hInstance
        lPerform.Data(3, nRow, 1) = 130
    Wend
    Close #intFileNumber
End Sub

Private Sub SavePerform()
    Dim intFileNumber As Integer
    Dim i As Long
    
    intFileNumber = FreeFile
    
    Open Reg.Value & "\perform.ini" For Output As #intFileNumber
    For i = 0 To lPerform.Count - 1
        If lPerform.Data(0, i) <> "" Then Print #intFileNumber, lPerform.Data(0, i)
    Next
    
    Close #intFileNumber
End Sub

Private Sub btnPerformOK_Accepted(ByVal nID As Long)
    'Dim tmpPerformINI As String
    
    Dim tBox As String
    Dim nRow As Long
    tBox = notNULL(edPerform.Text)
    If tBox <> "" Then
        nRow = lPerform.AddRow
        lPerform.Data(0, nRow) = tBox
        lPerform.Data(1, nRow, 0) = App.hInstance
        lPerform.Data(1, nRow, 1) = 128
        lPerform.Data(2, nRow, 0) = App.hInstance
        lPerform.Data(2, nRow, 1) = 129
        lPerform.Data(3, nRow, 0) = App.hInstance
        lPerform.Data(3, nRow, 1) = 130
    
        SavePerform
        
        edPerform.Text = ""
    End If
End Sub

Private Sub cbAction_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H1&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H1&
    End If
    bAction = bChecked
End Sub

Private Sub chkGreenText_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    bGreenText = bChecked
    Reg.Value("GreenText") = CStr(bChecked)
End Sub

Private Sub cbActiveChannel_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H400&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H400&
    End If
    bActiveChannel = bChecked
End Sub

Private Sub cbAutoConnectOption_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    Reg.Value("AutoConnect") = bChecked
End Sub

Private Sub cbCTCP_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H2&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H2&
    End If
    bCTCP = bChecked
End Sub

Private Sub cbHighlight_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H4&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H4&
    End If
    bHighlight = bChecked
End Sub

Private Sub cbPerformOnConnectOption_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    Reg.Value("PerformOnConnect") = bChecked
End Sub

Private Sub cbInactiveChannel_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H800&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H800&
    End If
    bInactiveChannel = bChecked
End Sub

Private Sub cbInfo_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H8&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H8&
    End If
    bInfo = bChecked
End Sub

Private Sub cbNoticeAuth_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H10&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H10&
    End If
    bNoticeAuth = bChecked
End Sub

Private Sub cbInvite_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H20&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H20&
    End If
    bInvite = bChecked
End Sub

Private Sub cbJoin_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H40&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H40&
    End If
    bJoin = bChecked
End Sub

Private Sub cbKick_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H80&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H80&
    End If
    bKick = bChecked
End Sub

Private Sub cbMode_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H100&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H100&
    End If
    bMode = bChecked
End Sub

Private Sub cbNick_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H200&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H200&
    End If
    bNick = bChecked
End Sub

Private Sub cbNotice_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H1000&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H1000&
    End If
    bNotice = bChecked
End Sub

Private Sub cbNotify_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H2000&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H2000&
    End If
    bNotify = bChecked
End Sub

Private Sub cbOther_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H4000&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H4000&
    End If
    bOther = bChecked
End Sub

Private Sub cbOwn_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H8000&
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H8000&
    End If
    bOwn = bChecked
End Sub

Private Sub cbPart_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H10000
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H10000
    End If
    bPart = bChecked
End Sub

Private Sub cbQuit_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H20000
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H20000
    End If
    bQuit = bChecked
End Sub

Private Sub cbTopic_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H40000
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H40000
    End If
    bTopic = bChecked
End Sub

Private Sub cbWallops_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H80000
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H80000
    End If
    bWallops = bChecked
End Sub

Private Sub cbWhois_Change(ByVal nID As Long, ByVal bChecked As Boolean)
    If bChecked Then
        Reg.Value("Filters") = Reg.Value("Filters") + &H100000
    Else
        Reg.Value("Filters") = Reg.Value("Filters") - &H100000
    End If
    bWhois = bChecked
End Sub

Private Sub chTextType_Change(ByVal nID As Long, ByVal nIndex As Long)
    '
End Sub

Private Sub chTextType_DropDown(ByVal nID As Long)
    '
End Sub

Private Sub Class_Initialize()
On Error GoTo Error_Handler:
                                        ' ' M A I N   S C H E M A ' '
    Set Reg = New RegOp
    Reg.Root = HKEY_LOCAL_MACHINE
    Reg.Key = "SOFTWARE\dIRCal"
    

    ViewSchema = "<?xml version=""1.0""?>" + _
        "<view icon=""4987"" title=""dIRCal"" height=""275"" width=""160"">" + _
            "<control progid=""DecalControls.Notebook"" name=""nbMain"">"
    ViewSchema = ViewSchema + _
                "<page label=""Config"">" + _
                    "<control progid=""DecalControls.FixedLayout"">" + _
                        "<control progid=""DecalControls.StaticText"" left=""5""  top=""0""   width=""150"" height=""14"" text=""IRC Server:"" />" + _
                        "<control progid=""DecalControls.Edit""       left=""5""  top=""15""  width=""150"" height=""16"" text=""irc.sorcery.net"" name=""edServer""     imageportalsrc=""4410"" textcolor=""16777215""/>" + _
                        "<control progid=""DecalControls.StaticText"" left=""5""  top=""35""  width=""150"" height=""14"" text=""IRC Server Port:"" />" + _
                        "<control progid=""DecalControls.Edit""       left=""5""  top=""50""  width=""150"" height=""16"" text=""6667"" name=""edServerPort"" imageportalsrc=""4410"" textcolor=""16777215""/>" + _
                        "<control progid=""DecalControls.StaticText"" left=""5""  top=""70""  width=""150"" height=""14"" text=""Nickname:"" />" + _
                        "<control progid=""DecalControls.Edit""       left=""5""  top=""85""  width=""150"" height=""16"" text=""" & Reg.Value("Nickname") & """ name=""edNick""       imageportalsrc=""4410"" textcolor=""16777215""/>" + _
                        "<control progid=""DecalControls.StaticText"" left=""5""  top=""105"" width=""150"" height=""14"" text=""Channel Auto-Join:"" />" + _
                        "<control progid=""DecalControls.Edit""       left=""5""  top=""120"" width=""150"" height=""16"" text=""" & Reg.Value("Channel") & """ name=""edChannel""    imageportalsrc=""4410"" textcolor=""16777215"" />" + _
                        "<control progid=""DecalControls.StaticText"" left=""5""  top=""140"" width=""150"" height=""14"" text=""Password:"" />" + _
                        "<control progid=""DecalControls.Edit""       left=""5""  top=""155"" width=""150"" height=""16"" text=""" & Crypt(Reg.Value("Password")) & """ password=""true""  name=""edPassword""   imageportalsrc=""4410"" textcolor=""16777215""/>" + _
                        "<control progid=""DecalControls.PushButton"" left=""10"" top=""192"" width=""55""  height=""16"" text=""Options""             name=""btnOptions"" />" + _
                        "<control progid=""DecalControls.PushButton"" left=""75"" top=""192"" width=""75""  height=""16"" text=""Connect""             name=""btnConnect"" />" + _
                        "<control progid=""DecalControls.StaticText"" left=""5""  top=""210"" width=""150"" height=""14"" text=""Status: ""            name=""stStatus""/>" + _
                    "</control>" + _
                "</page>"
    ViewSchema = ViewSchema + _
                "<page label=""Channels"">" + _
                    "<control progid=""DecalControls.BorderLayout"">" + _
                        "<border edge=""top"" size=""15"">" + _
                            "<control progid=""DecalControls.FixedLayout"">" + _
                                "<control name=""stCurrentChannel"" progid=""DecalControls.StaticText"" left=""0"" top=""0"" width=""160"" height=""15"" text=""Current Channel:"" />" + _
                            "</control>" + _
                        "</border>" + _
                        "<center>" + _
                            "<control progid=""DecalControls.List"" name=""lChannel"">" + _
                                "<column progid=""DecalControls.TextColumn"" />" + _
                                "<column progid=""DecalControls.IconColumn"" />" + _
                            "</control>" + _
                        "</center>" + _
                    "</control>" + _
                "</page>"
    ViewSchema = ViewSchema + _
                "<page label=""Users"">" + _
                    "<control progid=""DecalControls.BorderLayout"">" + _
                        "<border edge=""top"" size=""15"">" + _
                            "<control progid=""DecalControls.FixedLayout"">" + _
                                "<control name=""stUsersInChannel"" progid=""DecalControls.StaticText"" left=""0"" top=""0"" width=""160"" height=""15"" text=""Users in Channel:"" />" + _
                            "</control>" + _
                        "</border>" + _
                        "<center>" + _
                            "<control progid=""DecalControls.List"" name=""lUser"">" + _
                                "<column progid=""DecalControls.TextColumn"" />" + _
                                "<column progid=""DecalControls.IconColumn"" />" + _
                            "</control>" + _
                        "</center>" + _
                    "</control>" + _
                "</page>"
    ViewSchema = ViewSchema + _
            "</control>" + _
        "</view>"

                                        ' ' O P T I O N   S C H E M A ' '

    OptionSite = "<?xml version=""1.0""?>" + _
        "<view icon=""4987"" title=""dIRCal - Options"" height=""275"" width=""350"" alpha=""255"">" + _
            "<control progid=""DecalControls.BorderLayout"">" + _
                "<border edge=""left"" size=""95"">" + _
                    "<control progid=""DecalControls.List"" name=""lOptions"">" + _
                        "<column progid=""DecalControls.TextColumn"" />" + _
                    "</control>" + _
                "</border>" + _
                "<center>" + _
                    "<control progid=""DecalControls.FixedLayout"">" + _
                        "<control progid=""DecalControls.StaticText"" left=""45""   top=""61""  width=""90"" height=""123"" text=""Please select a category from the left."" name=""stDefault"" />"

    OptionSite = OptionSite + _
                        "<control progid=""DecalControls.StaticText"" left=""300""  top=""300"" width=""170"" height=""14"" text=""IRC Server:"" name=""stServerOption"" />" + _
                        "<control progid=""DecalControls.Edit""       left=""0""  top=""0"" width=""0"" height=""0"" text=""" & Reg.Value("Server") & """ name=""edServerOption"" imageportalsrc=""4410"" textcolor=""16777215"" />" + _
                        "<control progid=""DecalControls.StaticText"" left=""300""  top=""300"" width=""170"" height=""14"" text=""IRC Server Port:"" name=""stServerPortOption"" />" + _
                        "<control progid=""DecalControls.Edit""       left=""0""  top=""0"" width=""0"" height=""0"" text=""" & Reg.Value("ServerPort") & """ name=""edServerPortOption"" imageportalsrc=""4410"" textcolor=""16777215"" />" + _
                        "<control progid=""DecalControls.StaticText"" left=""300""  top=""300"" width=""170"" height=""14"" text=""Nickname:"" name=""stNickOption"" />" + _
                        "<control progid=""DecalControls.Edit""       left=""0""  top=""0"" width=""0"" height=""0"" text=""" & Reg.Value("Nickname") & """ name=""edNickOption"" imageportalsrc=""4410"" textcolor=""16777215"" />" + _
                        "<control progid=""DecalControls.StaticText"" left=""300""  top=""300"" width=""170"" height=""14"" text=""Auto-Join:"" name=""stChannelOption"" />" + _
                        "<control progid=""DecalControls.Edit""       left=""0""  top=""0"" width=""0"" height=""0"" text=""" & Reg.Value("Channel") & """ name=""edChannelOption"" imageportalsrc=""4410"" textcolor=""16777215"" />" + _
                        "<control progid=""DecalControls.StaticText"" left=""300""  top=""300"" width=""170"" height=""14"" text=""Password:"" name=""stPasswordOption"" />" + _
                        "<control progid=""DecalControls.Edit""       left=""0""  top=""0"" width=""0"" height=""0"" text=""" & Crypt(Reg.Value("Password")) & """ name=""edPasswordOption"" imageportalsrc=""4410"" textcolor=""16777215"" />" + _
                        "<control progid=""DecalControls.Checkbox""   left=""0""  top=""0"" width=""0"" height=""0"" text=""Perform On Connect"" name=""cbPerformOnConnectOption"" checked=""" & Reg.Value("PerformOnConnect") & """ />" + _
                        "<control progid=""DecalControls.Checkbox""   left=""0""  top=""0"" width=""0"" height=""0"" text=""Auto-Connect"" name=""cbAutoConnectOption"" checked=""" & Reg.Value("AutoConnect") & """ />" + _
                        "<control progid=""DecalControls.Checkbox"" name=""chkGreenText"" left=""0""  top=""0"" width=""0"" height=""0"" text=""Add green links to names"" checked=""" + Reg.Value("GreenText") + """ />"

    OptionSite = OptionSite + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Action text""    name=""cbAction"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""CTCP text""      name=""cbCTCP"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Highlight text"" name=""cbHighlight"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Info text""      name=""cbInfo"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Connect text""   name=""cbNoticeAuth"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Invite text""    name=""cbInvite"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Join text""      name=""cbJoin"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Kick text""      name=""cbKick"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Mode text""      name=""cbMode"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Nick text""      name=""cbNick"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Active channel text""   name=""cbActiveChannel"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Inactive channel text"" name=""cbInactiveChannel"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Notice text""    name=""cbNotice"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Notify text""    name=""cbNotify"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0""  top=""0"" width=""0"" height=""0"" checked=""false"" text=""Other text""     name=""cbOther"" />"
    
    OptionSite = OptionSite + _
                        "<control progid=""DecalControls.Checkbox"" left=""0"" top=""0"" width=""0"" height=""0"" checked=""false"" righttoleft=""true"" text=""Own text""     name=""cbOwn"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0"" top=""0"" width=""0"" height=""0"" checked=""false"" righttoleft=""true"" text=""Part text""    name=""cbPart"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0"" top=""0"" width=""0"" height=""0"" checked=""false"" righttoleft=""true"" text=""Quit text""    name=""cbQuit"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0"" top=""0"" width=""0"" height=""0"" checked=""false"" righttoleft=""true"" text=""Topic text""   name=""cbTopic"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0"" top=""0"" width=""0"" height=""0"" checked=""false"" righttoleft=""true"" text=""Wallops text"" name=""cbWallops"" />" + _
                        "<control progid=""DecalControls.Checkbox"" left=""0"" top=""0"" width=""0"" height=""0"" checked=""false"" righttoleft=""true"" text=""Whois text""   name=""cbWhois"" />"
    
    OptionSite = OptionSite + _
                        "<control progid=""DecalControls.Choice"" left=""0"" top=""0"" width=""0"" height=""0"" name=""chTextType"" droplines=""7"" />" + _
                        "<control progid=""DecalControls.List""   left=""0"" top=""0"" width=""0"" height=""0"" name=""lColors"" >" + _
                            "<column progid=""DecalControls.IconColumn"" />" + _
                            "<column progid=""DecalControls.TextColumn"" width=""0"" />" + _
                        "</control>" + _
                        "<control progid=""DecalControls.Edit""   left=""0"" top=""0"" width=""0"" height=""0"" name=""edColorTest"" text=""This is a test."" imageportalsrc=""4373"" textcolor=""" & "bgrcolor" & """/>"
    
    OptionSite = OptionSite + _
                        "<control progid=""DecalControls.List""   left=""0"" top=""0"" width=""0"" height=""0"" name=""lPerform"" >" + _
                            "<column progid=""DecalControls.TextColumn"" />" + _
                            "<column progid=""DecalControls.IconColumn"" />" + _
                            "<column progid=""DecalControls.IconColumn"" />" + _
                            "<column progid=""DecalControls.IconColumn"" />" + _
                        "</control>" + _
                        "<control progid=""DecalControls.Edit""   left=""0"" top=""0"" width=""0"" height=""0"" name=""edPerform"" text="""" imageportalsrc=""4373"" textcolor=""16777215"" />" + _
                        "<control progid=""DecalControls.PushButton"" left=""0"" top=""0"" width=""0"" height=""0"" text=""Add"" name=""btnPerformOK"" />" + _
                        "<control progid=""DecalControls.PushButton"" left=""0"" top=""0"" width=""0"" height=""0"" text=""Discard"" name=""btnPerformCancel"" />"

    OptionSite = OptionSite + _
                        "<control progid=""DecalControls.PushButton"" left=""0"" top=""0"" width=""0"" height=""0"" text=""Apply"" name=""btnOptionsApply"" />" + _
                        "<control progid=""DecalControls.PushButton"" left=""60"" top=""225"" width=""60"" height=""16"" text=""OK"" name=""btnOptionsOK"" />" + _
                        "<control progid=""DecalControls.PushButton"" left=""0"" top=""0"" width=""0"" height=""0"" text=""Cancel"" name=""btnOptionsCancel"" />" + _
                    "</control>" + _
                "</center>" + _
            "</control>" + _
        "</view>"
    
Exit Sub

Error_Handler:
    Call ERRORROUTINE("Class_Initialize", Err, Error(Err))
    Resume Next
End Sub

Private Sub Class_Terminate()
On Error GoTo Error_Handler:
    Set Reg = Nothing
Exit Sub

Error_Handler:
    Call ERRORROUTINE("Class_Initialize", Err, Error(Err))
    Resume Next
End Sub

Private Sub edChannel_Change(ByVal nID As Long, ByVal strText As String)
    Reg.Value("Channel") = strText
End Sub

Private Sub edChannelOption_Change(ByVal nID As Long, ByVal strText As String)
    edChannel.Text = notNULL(edChannelOption.Text)
    Reg.Value("Channel") = strText
End Sub

Private Sub edNick_Change(ByVal nID As Long, ByVal strText As String)
    Reg.Value("NickName") = strText
End Sub

Private Sub edNickOption_Change(ByVal nID As Long, ByVal strText As String)
    edNick.Text = notNULL(edNickOption.Text)
    Reg.Value("NickName") = strText
End Sub

Private Sub edPassword_Change(ByVal nID As Long, ByVal strText As String)
    Reg.Value("Password") = Crypt(strText)
End Sub
 
Private Sub edPasswordOption_Change(ByVal nID As Long, ByVal strText As String)
    edPassword.Text = notNULL(edPasswordOption.Text)
    Reg.Value("Password") = Crypt(strText)
End Sub

Private Sub edPerform_End(ByVal nID As Long, ByVal bSuccess As Boolean)
    If bSuccess Then
        If lPerform.Data(0, linePerform) = "" Then lPerform.AddRow
        lPerform.Data(0, linePerform) = notNULL(edPerform.Text)
    End If
End Sub

Private Sub edServer_Change(ByVal nID As Long, ByVal strText As String)
    Reg.Value("Server") = strText
End Sub

Private Sub edServerOption_Change(ByVal nID As Long, ByVal strText As String)
    edServer.Text = notNULL(edServerOption.Text)
    Reg.Value("Server") = strText
End Sub

Private Sub edServerPort_Change(ByVal nID As Long, ByVal strText As String)
    Reg.Value("ServerPort") = strText
End Sub

Private Sub edServerPortOption_Change(ByVal nID As Long, ByVal strText As String)
    edServerPort.Text = notNULL(edServerPortOption.Text)
    Reg.Value("ServerPort") = strText
End Sub

Private Sub IPlugin2_Initialize(ByVal pSite2 As Decal.IPluginSite2)
On Error GoTo Error_Handler:
    Dim pSite As PluginSite
    Set pSite = pSite2.PluginSite
    Set PluginSite2 = pSite2

    Set eACHooks = pSite2.Hooks

    Set PluginSite = pSite
    
    Set NetEcho = pSite.NetworkFilter("DecalFilters.EchoFilter2")
    
    Set MainView = PluginSite.LoadView(ViewSchema)
    
    Set btnConnect = MainView.Control("btnConnect")

    Set edServer = MainView.Control("edServer")
    Set edServerPort = MainView.Control("edServerPort")
    Set edNick = MainView.Control("edNick")
    Set edChannel = MainView.Control("edChannel")
    Set edPassword = MainView.Control("edPassword")
    Set stStatus = MainView.Control("stStatus")
    Set btnOptions = MainView.Control("btnOptions")
    
    Set stCurrentChannel = MainView.Control("stCurrentChannel")
    Set lChannel = MainView.Control("lChannel")
    Set stUsersInChannel = MainView.Control("stUsersInChannel")
    Set lUser = MainView.Control("lUser")
    'Set chkGreenText = MainView.Control("chkGreenText")
    
    Set tcp = New Winsock
'    Set dIRCalNotify.dIRCalIRC = Me
    
    If Reg.Value("Filters") = "" Then Reg.Value("Filters") = &H0&
    bCommandDebug = Reg.Value("CommandDebug")
    bMessageDebug = Reg.Value("MessageDebug")
    bIRC = False
    b366 = False
    
    NotifyList = Split(Reg.Value("NotifyList"), ",")
    
    LoadOptions
    
    SetMainIcon STATE_DISCONNECTED
    
    If bAutoConnect Then btnConnect_Accepted (0)
Exit Sub

Error_Handler:
    Call ERRORROUTINE("IPlugin_Initialize", Err, Error(Err))
    Resume Next
End Sub

Private Sub IPlugin2_Terminate()
On Error GoTo Error_Handler:
    
    QuitIRC ("")
    
    'Set chkGreenText = Nothing
    Set lUser = Nothing
    Set stUsersInChannel = Nothing
    Set lChannel = Nothing
    Set stCurrentChannel = Nothing
    
    Set btnOptions = Nothing
    Set stStatus = Nothing
    Set edPassword = Nothing
    Set edNick = Nothing
    Set edServerPort = Nothing
    Set edServer = Nothing
    
    Set btnConnect = Nothing
    
    Set MainView = Nothing
    
    Set VCS = Nothing
    Set NetEcho = Nothing

    Set tcp = Nothing
    
    Set eACHooks = Nothing

    Set PluginSite2 = Nothing
    Set PluginSite = Nothing
Exit Sub

Error_Handler:
    Call ERRORROUTINE("IPlugin_Terminate", Err, Error(Err))
    Resume Next
End Sub

'Private Function IPluginSink_ChatMessage(ByVal bstrMsg As String, pColor As Long) As Boolean
'    IPluginSink_ChatMessage = False
'End Function

                                        ' ' O U T G O I N G   C O M M A N D S ' '

                                        ' ' O U T G O I N G   C O M M A N D S ' '

                                        ' ' O U T G O I N G   C O M M A N D S ' '

'Private Function IPluginSink_ChatText(ByVal bstrMsg As String) As Boolean
Private Sub eACHooks_ChatParserIntercept(ByVal bstrMsg As String, eat As Boolean)
On Error GoTo Error_Handler:
Dim temps As String
'<Checksum> set that bool to true and it eats the message
    bstrMsg = Trim(bstrMsg)
    
    If (bIRC = True) And Not (left(bstrMsg, 1) = "/" Or left(bstrMsg, 1) = "@") Then
        If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
            tcp.SendData "PRIVMSG " & strChannel & " :" & bstrMsg & vbCrLf
            AddChatText "<" & strNickName & "> " & bstrMsg, AC_GREY
        End If
        eat = True
    ElseIf left(bstrMsg, 1) = "/" Or left(bstrMsg, 1) = "@" Then
        Dim command As String
        Dim i As Long
        
        bstrMsg = Mid(bstrMsg, 2)
        
        For i = 1 To Len(bstrMsg)
            If Mid(bstrMsg, i, 1) = Chr(32) Then
                command = LCase(Trim(Mid(bstrMsg, 1, i - 1)))
                bstrMsg = Trim(Mid(bstrMsg, i + 1))
                Exit For
            End If
        Next i
        
        If command = "" Then
            command = LCase(bstrMsg)
            bstrMsg = ""
        End If
        
        'figure out aliases
        Select Case command
            Case "i"
                command = "irc_chat"
            
            Case "ir"
                command = "priv_reply"
            Case "irp"
                command = "priv_reply"
            Case "ireply"
                command = "priv_reply"
            
            Case "irt"
                command = "priv_retell"
            Case "iretell"
                command = "priv_retell"
            
            Case "msg"
                command = "priv_tell"
            Case "it"
                command = "priv_tell"
            Case "itell"
                command = "priv_tell"
            
            Case "ime"
                command = "irc_emote"
            Case "iem"
                command = "irc_emote"
            Case "iemote"
                command = "irc_emote"
            Case "ie"
                command = "irc_emote"
                
            Case "tell"
                If Mid$(FirstWordNC(bstrMsg), 1, 1) = "[" And Mid$(FirstWordNC(bstrMsg), Len(FirstWordNC(bstrMsg)) - 1, 2) = "]," Then
                    temps = FirstWord(bstrMsg)
                    bstrMsg = Mid$(temps, 2, Len(temps) - 3) + " " + bstrMsg
                    command = "priv_tell"
                End If
            Case "t"
                If Mid$(FirstWordNC(bstrMsg), 1, 1) = "[" And Mid$(FirstWordNC(bstrMsg), Len(FirstWordNC(bstrMsg)) - 1, 2) = "]," Then
                    temps = FirstWord(bstrMsg)
                    bstrMsg = Mid$(temps, 2, Len(temps) - 3) + " " + bstrMsg
                    command = "priv_tell"
                End If

        End Select

        Dim dest As String
        Dim Channel As String
        'actual commands
        Select Case command
'   3.  Message Details ............................................   9
'      3.1  Connection Registration ................................  10
'         3.1.1  Password message ..................................  10
                'Not implemented
'         3.1.2  Nick message ......................................  10
            Case "nick"
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "NICK " & bstrMsg & vbCrLf
                End If
                eat = True

'         3.1.3  User message ......................................  11
                'Not implemented
'         3.1.4  Oper message ......................................  12
                'Not implemented
'         3.1.5  User mode message .................................  12
            Case "mode"
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "MODE " & bstrMsg & vbCrLf
                End If
                eat = True

'         3.1.6  Service message ...................................  13
                'Not implemented
'         3.1.7  Quit ..............................................  14
            Case "quit"
                QuitIRC (bstrMsg)
                eat = True

'         3.1.8  Squit .............................................  15
                'Not implemented
'      3.2  Channel operations .....................................  15
'         3.2.1  Join message ......................................  16
            Case "join"
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "JOIN " & bstrMsg & vbCrLf
                End If
                eat = True

'         3.2.2  Part message ......................................  17
            Case "part"
                If (btnConnect.Text = "Disconnect") Then
                    If Len(bstrMsg) > 0 Then
                        For i = 1 To Len(bstrMsg)
                            If Mid(bstrMsg, i, 1) = Chr(32) Then
                                Channel = Trim(Mid(bstrMsg, 1, i - 1))
                                bstrMsg = Mid(bstrMsg, i + 1)
                                Exit For
                            End If
                        Next i
                        tcp.SendData "PART " & Channel & " :" & bstrMsg & vbCrLf
                    Else
                        tcp.SendData "PART " & strChannel & vbCrLf
                    End If
                End If
                eat = True

'         3.2.3  Channel mode message ..............................  18
                'Covered in 3.1.5  User mode message ...............  12
'         3.2.4  Topic message .....................................  19
            Case "topic"
                If left(bstrMsg, 1) = "#" Then
                    Channel = FirstWord(bstrMsg)
                    If (btnConnect.Text = "Disconnect") And (Len(Channel) > 1) Then
                        If Len(bstrMsg) > 0 Then
                            'set topic
                            tcp.SendData "TOPIC " & Channel & " :" & bstrMsg & vbCrLf
                        Else
                            'view topic
                            tcp.SendData "TOPIC " & Channel & vbCrLf
                        End If
                    End If
                Else
                    If (btnConnect.Text = "Disconnect") And (Len(strChannel) > 0) Then
                        If Len(bstrMsg) > 0 Then
                            'set topic
                            tcp.SendData "TOPIC " & strChannel & " :" & bstrMsg & vbCrLf
                        Else
                            'view topic
                            tcp.SendData "TOPIC " & strChannel & vbCrLf
                        End If
                    End If
                End If
                eat = True
                
            Case "cleartopic"
                If left(bstrMsg, 1) = "#" Then
                    Channel = FirstWord(bstrMsg)
                    If (btnConnect.Text = "Disconnect") And (Len(Channel) > 1) Then
                        'clear topic
                        tcp.SendData "TOPIC " & Channel & " :" & vbCrLf
                    End If
                Else
                    If (btnConnect.Text = "Disconnect") And (Len(strChannel) > 0) Then
                        'clear topic
                        tcp.SendData "TOPIC " & strChannel & " :" & vbCrLf
                    End If
                End If
                eat = True

'         3.2.5  Names message .....................................  20
            Case "names"
                Channel = FirstWord(bstrMsg)
                If (btnConnect.Text = "Disconnect") And (Len(Channel) > 0) Then
                    tcp.SendData "NAMES " & Channel & vbCrLf
                End If
                eat = True
                
'         3.2.6  List message ......................................  21
                'Not implemented
'         3.2.7  Invite message ....................................  21
            Case "invite"
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "INVITE " & bstrMsg & vbCrLf
                End If
                eat = True

'         3.2.8  Kick command ......................................  22
            Case "kick"
                Dim USER As String
                Channel = FirstWord(bstrMsg)
                USER = FirstWord(bstrMsg)
                If bstrMsg = "" Then bstrMsg = strNickName
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "KICK " & Channel & " " & USER & " :" & bstrMsg & vbCrLf
                End If
                eat = True

'      3.3  Sending messages .......................................  23
'         3.3.1  Private messages ..................................  23
            Case "irc_chat"
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "PRIVMSG " & strChannel & " :" & bstrMsg & vbCrLf
                    AddChatText "<" & strNickName & "> " & bstrMsg, AC_GREY
                End If
                eat = True
            Case "priv_reply"
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "PRIVMSG " & strReplyTo & " :" & bstrMsg & vbCrLf
                    If Not bGreenText Then
                        AddChatText "You tell <" & strReplyTo & ">, """ & bstrMsg & """", AC_RED
                    Else
                        AddChatText "You tell <Tell:IIDString:0:" & "[" & strReplyTo & "]>" & "<" & strReplyTo & ">" & "<\Tell>, """ & bstrMsg & """", AC_RED
                    End If
                End If
                eat = True
            Case "priv_retell"
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "PRIVMSG " & strRetellTo & " :" & bstrMsg & vbCrLf
                    If Not bGreenText Then
                        AddChatText "You tell <" & strRetellTo & ">, """ & bstrMsg & """", AC_RED
                    Else
                        AddChatText "You tell <Tell:IIDString:0:" & "[" & strRetellTo & "]>" & "<" & strRetellTo & ">" & "<\Tell>, """ & bstrMsg & """", AC_RED
                    End If
                End If
                eat = True
            Case "priv_tell"
                dest = FirstWord(bstrMsg)
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "PRIVMSG " & dest & " :" & bstrMsg & vbCrLf
                    If Not bGreenText Then
                        AddChatText "You tell <" & dest & ">, """ & bstrMsg & """", AC_RED
                    Else
                        AddChatText "You tell <Tell:IIDString:0:" & "[" & dest & "]>" & "<" & dest & ">" & "<\Tell>, """ & bstrMsg & """", AC_RED
                    End If
                    strRetellTo = dest
                End If
                eat = True

'         3.3.2  Notice ............................................  24
            Case "notice"
                dest = FirstWord(bstrMsg)
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "NOTICE " & dest & " :" & bstrMsg & vbCrLf
                    AddChatText "-" & dest & "- " & bstrMsg & """", AC_RED
                End If
                eat = True
                
'      3.4  Server queries and commands ............................  25
'         3.4.1  Motd message ......................................  25
                'Not implemented
'         3.4.2  Lusers message ....................................  25
                'Not implemented
'         3.4.3  Version message ...................................  26
                'Not implemented
'         3.4.4  Stats message .....................................  26
                'Not implemented
'         3.4.5  Links message .....................................  27
                'Not implemented
'         3.4.6  Time message ......................................  28
                'Not implemented
'         3.4.7  Connect message ...................................  28
                'Not implemented
'         3.4.8  Trace message .....................................  29
                'Not implemented
'         3.4.9  Admin command .....................................  30
                'Not implemented
'         3.4.10 Info command ......................................  31
                'Not implemented
'      3.5  Service Query and Commands .............................  31
'         3.5.1  Servlist message ..................................  31
                'Not implemented
'         3.5.2  Squery ............................................  32
                'Not implemented
'      3.6  User based queries .....................................  32
'         3.6.1  Who query .........................................  32
            Case "who"
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "WHO " & bstrMsg & vbCrLf
                End If
                eat = True

'         3.6.2  Whois query .......................................  33
            Case "whois"
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "WHOIS " & bstrMsg & vbCrLf
                End If
                eat = True

'         3.6.3  Whowas ............................................  34
            Case "whowas"
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "WHOWAS " & bstrMsg & vbCrLf
                End If
                eat = True

'      3.7  Miscellaneous messages .................................  34
'         3.7.1  Kill message ......................................  35
                'Not implemented
'         3.7.2  Ping message ......................................  36
                'Not implemented
'         3.7.3  Pong message ......................................  37
                'Not implemented
'         3.7.4  Error .............................................  37
                'Not implemented

            Case "raw"
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData bstrMsg & vbCrLf
                    AddChatText "-> Server: " & bstrMsg, AC_RED
                End If
                eat = True
                
            Case "ctcp"
                Dim Target As String
                Dim CTCP As String
                Target = FirstWord(bstrMsg)
                CTCP = FirstWord(bstrMsg)
                If (btnConnect.Text = "Disconnect") And (Len(CTCP) > 0) Then
                    Select Case UCase(CTCP)
                        Case "PING"
                            tcp.SendData "PRIVMSG " & Target & " :" & UCase(CTCP) & " " & Timer & "" & vbCrLf
                            PingStart = Timer
                            AddChatText "[" & Target & "] PING", AC_RED
                        Case "TIME"
                            tcp.SendData "PRIVMSG " & Target & " :" & UCase(CTCP) & "" & vbCrLf
                            AddChatText "[" & Target & "] TIME", AC_RED
                        Case "VERSION"
                            tcp.SendData "PRIVMSG " & Target & " :" & UCase(CTCP) & "" & vbCrLf
                            AddChatText "[" & Target & "] VERSION", AC_RED
'                        Case "WHO"
'                            tcp.SendData "PRIVMSG " & Target & " :" & UCase(CTCP) & "" & vbCrLf
'                            AddChatText "[" & Target & "] WHO", AC_RED
'                        Case "COORDS"
'                            tcp.SendData "PRIVMSG " & Target & " :" & UCase(CTCP) & "" & vbCrLf
'                            AddChatText "[" & Target & "] COORDS", AC_RED
                        Case Else
                            If Len(bstrMsg) > 0 Then
                                tcp.SendData "PRIVMSG " & Target & " :" & UCase(CTCP) & " " & bstrMsg & "" & vbCrLf
                                AddChatText "[" & Target & "] " & UCase(CTCP) & " " & bstrMsg, AC_RED
                            Else
                                tcp.SendData "PRIVMSG " & Target & " :" & UCase(CTCP) & "" & vbCrLf
                                AddChatText "[" & Target & "] " & UCase(CTCP), AC_RED
                            End If
                    End Select
                End If
                eat = True
                
            Case "irc_emote"
                If (btnConnect.Text = "Disconnect") And (Len(bstrMsg) > 0) Then
                    tcp.SendData "PRIVMSG " & strChannel & " :ACTION " & bstrMsg & "" & vbCrLf
                    AddChatText "* " & strNickName & " " & bstrMsg, AC_PURPLE
                End If
                eat = True
                
            Case "irc"
                If bIRC = True Then
                    AddChatText "Already Chatting in IRC.  Use '/ac' to switch to AC.", AC_GREEN
                Else
                    bIRC = True
                    AddChatText "Now chatting in IRC", AC_RED
                    'MainView.SetIcon &H6001F63
                    SetMainIcon STATE_IRCCHAT
                End If
                eat = True

            Case "ac"
                If bIRC = False Then
                    AddChatText "Already Chatting in AC.  Use '/irc' to switch to IRC.", AC_GREEN
                Else
                    bIRC = False
                    AddChatText "Now chatting in AC", AC_RED
                    'MainView.SetIcon &H6000516
                    SetMainIcon STATE_CONNECTED
                End If
                eat = True

            Case "debugcmd"
                If bCommandDebug = True Then
                    bCommandDebug = False
                    AddChatText "Command debugging OFF", AC_RED
                Else
                    bCommandDebug = True
                    AddChatText "Command debugging ON", AC_GREEN
                End If
                eat = True
                WriteRegSettings
                
            Case "debugmsg"
                If bMessageDebug = True Then
                    bMessageDebug = False
                    AddChatText "Message debugging OFF", AC_RED
                Else
                    bMessageDebug = True
                    AddChatText "Message debugging ON", AC_GREEN
                End If
                eat = True
                WriteRegSettings
            
            Case "connect"
                Select Case btnConnect.Text
                    Case "Connect"
                        btnConnect_Accepted (0)
                    Case "Cancel"
                        SetStatus "Already connecting"
                    Case "Disconnect"
                        SetStatus "Already connected"
                End Select
                eat = True
            Case "disconnect"
                Select Case btnConnect.Text
                    Case "Connect"
                        SetStatus "Not Connected"
                    Case "Cancel"
                        btnConnect_Accepted (0)
                    Case "Disconnect"
                        btnConnect_Accepted (0)
                End Select
                eat = True
            Case "status"
                Select Case tcp.state
                    Case sckClosed
                        SetStatus "Socket: Closed"
                    Case sckClosing
                        SetStatus "Socket: Closing"
                    Case sckConnected
                        SetStatus "Socket: Connected"
                    Case sckConnecting
                        SetStatus "Socket: Connecting"
                    Case sckConnectionPending
                        SetStatus "Socket: Connection Pending"
                    Case sckError
                        SetStatus "Socket: Error"
                    Case sckHostResolved
                        SetStatus "Socket: Host Resolved"
                    Case sckListening
                        SetStatus "Socket: Listening"
                    Case sckOpen
                        SetStatus "Socket: Open"
                    Case sckResolvingHost
                        SetStatus "Socket: Resolving Host"
                    Case Else
                        SetStatus "Socket: Unknown"
                End Select
                eat = True
            
            Case "help"
                Select Case LCase(bstrMsg)
                    Case "dircal"
                        dIRCalHelp
                        eat = True
                        
                    Case "irc"
                        AddChatText "IRC help coming soon!", AC_GREEN
                        'IRCHelp
                        eat = True
                        
                    Case Else
                        eat = False
                End Select
                
            Case Else
                eat = False
        End Select
    End If

Exit Sub

Error_Handler:
    Call ERRORROUTINE("IPluginSink_ChatText", Err, Error(Err))
    Resume Next
End Sub

                                        ' ' S E R V E R   R E S P O N S E S ' '

                                        ' ' S E R V E R   R E S P O N S E S ' '

                                        ' ' S E R V E R   R E S P O N S E S ' '

Private Sub IRC_Command(prefix As String, command As String, params As String)
On Error GoTo Error_Handler:
    Dim i As Integer
    Dim x As Integer
    Dim ix As Integer
    Dim n As Integer
    Dim wordNum As Integer
    
    Dim found As Boolean
    Dim Target As String
    Dim SourceEmail As String
    Dim Source As String
    Dim Channel As String
            
'            'chop off the leading ":"
'            If Left(params, 1) = ":" Then params = Mid(params, 2)

    'break down source
    For i = 1 To Len(prefix)
        If Mid(prefix, i, 1) = "!" Then
            SourceEmail = Mid(prefix, i + 1)
            Source = Mid(prefix, 1, i - 1)
        End If
    Next i
    If Source = "" Then Source = prefix

'    If bCommandDebug = True Then AddChatText " prefix = " & prefix & " | command = " & command & " | params = " & params, AC_GREEN
    
    Select Case UCase(command)
'   3.  Message Details ............................................   9
'      3.1  Connection Registration ................................  10
'
'         3.1.1  Password message ..................................  10
        Case "PASS"
            
'         3.1.2  Nick message ......................................  10
        Case "NICK"
            'chop off the leading ":"
            If left(params, 1) = ":" Then params = Mid(params, 2)
            For i = 0 To (lUser.Count - 1)
                If LCase(lUser.Data(0, i)) = LCase(Source) Then lUser.Data(0, i) = Trim(params)
                If LCase(lUser.Data(0, i)) = LCase("@" & Source) Then lUser.Data(0, i) = Trim("@" & params)
                If LCase(lUser.Data(0, i)) = LCase("+" & Source) Then lUser.Data(0, i) = Trim("+" & params)
            Next i
            If Not bNick Then AddChatText "*** " & Source & " is now known as " & params, AC_GREEN
            'Update strNickName if it was you
            If Trim(strNickName) = Trim(Source) Then strNickName = Trim(params)

'         3.1.3  User message ......................................  11
        Case "USER"
            
'         3.1.4  Oper message ......................................  12
        Case "OPER"
            
'         3.1.5  User mode message .................................  12
        Case "MODE"
            If left(params, 1) = "&" _
            Or left(params, 1) = "#" _
            Or left(params, 1) = "+" _
            Or left(params, 1) = "!" _
            Then
                'channel MODE
                For i = 1 To Len(params)
                    If Mid(params, i, 1) = Chr(32) Then
                        Channel = Trim(Mid(params, 1, i - 1))   '<channel>
                        params = Mid(params, i + 1)             '<topic>
                        Exit For
                    End If
                Next i
                If LCase(strChannel) = LCase(Channel) Then
                    If Not bMode Then AddChatText "*** " & Source & " sets mode: " & params, AC_CYAN
                End If
            Else
                'user MODE
                For i = 1 To Len(params)
                    If Mid(params, i, 1) = Chr(32) Then
                        Target = Trim(Mid(params, 1, i - 1))   '<channel>
                        params = Mid(params, i + 1)             '<topic>
                        Exit For
                    End If
                Next i
                
                'chop off the leading ":"
                If left(params, 1) = ":" Then params = Mid(params, 2)
                If LCase(strNickName) = LCase(Target) Then
                    If Not bMode Then AddChatText "*** " & Source & " sets user mode: " & params, AC_CYAN
                End If
            End If
            
'         3.1.6  Service message ...................................  13
        Case "SERVICE"
            
'         3.1.7  Quit ..............................................  14
        Case "QUIT"
            'chop off the leading ":"
            If left(params, 1) = ":" Then params = Mid(params, 2)
'            If LCase(strChannel) = LCase(channel) Then
            If LCase(Source) = LCase(strNickName) Then
                AddChatText "*** You have quit IRC (" & params & ")", AC_GREEN
                strChannel = ""
                stCurrentChannel.Text = "Current Channel:"
                stUsersInChannel.Text = "Users in Channel:"
                lChannel.Clear
                lUser.Clear
            Else
                For x = 0 To (lUser.Count - 1)
                    If (LCase(lUser.Data(0, x)) = LCase(Source) _
                     Or LCase(lUser.Data(0, x)) = LCase("@" & Source) _
                     Or LCase(lUser.Data(0, x)) = LCase("+" & Source)) _
                    Then
                        lUser.DeleteRow x
                        Exit For
                    End If
                Next x
                If Not bQuit Then AddChatText "*** " & Source & " has quit IRC (" & params & ")", AC_GREEN
            End If
'        End If

'         3.1.8  Squit .............................................  15
        Case "SQUIT"
            
'      3.2  Channel operations .....................................  15
'
'         3.2.1  Join message ......................................  16
        Case "JOIN"
            Dim nRow As Long
            'chop off the leading ":"
            If left(params, 1) = ":" Then params = Mid(params, 2)
            If LCase(Source) = LCase(strNickName) Then
                strChannel = params
                AddChatText "*** Now talking in " & params, AC_GREEN
                stCurrentChannel.Text = "Current Channel: " & strChannel
                stUsersInChannel.Text = "Users in Channel: " & strChannel
                'add a new channel
                nRow = lChannel.AddRow
                lChannel.Data(0, nRow) = strChannel
                For x = 0 To (lChannel.Count - 1)
                    'clear the channel icons
                    For i = 0 To lChannel.Count - 1
                        lChannel.Data(1, i, 1) = &H0&
                    Next i
                    'put an icon with the right channel
                    If LCase(lChannel.Data(0, x)) = LCase(strChannel) Then
                        lChannel.Data(1, x, 1) = &H60011F9
                        Exit For
                    End If
                Next x
                Exit Sub
            End If
            If (LCase(strChannel) = LCase(params)) Then
                i = lUser.AddRow
                lUser.Data(0, i) = Trim(Source)
                If Not bJoin Then AddChatText "*** " & Source & " has joined " & params, AC_GREEN
            End If

'         3.2.2  Part message ......................................  17
        Case "PART"
            Channel = FirstWord(params)
            If left(params, 1) = ":" Then params = " (" & Mid(params, 2) & ")"
            If LCase(Source) = LCase(strNickName) Then
                AddChatText "*** You have left " & Channel & params, AC_GREEN
                For x = 0 To (lChannel.Count - 1)
                    If LCase(lChannel.Data(0, x)) = LCase(Channel) Then
                        lChannel.DeleteRow (x)
                        Exit For
                    End If
                Next x
                If LCase(strChannel) = LCase(Channel) Then
                    strChannel = ""
                    stCurrentChannel.Text = "Current Channel:"
                    stUsersInChannel.Text = "Users in Channel:"
                    lUser.Clear
                    If lChannel.Count > 0 Then Call lChannel_Change(0, 0, lChannel.Count - 1)
                End If
            Else
                If Not bPart Then AddChatText "*** " & Source & " has left " & Channel & params, AC_GREEN
            End If
            
'         3.2.3  Channel mode message ..............................  18
'        Case "MODE"
'            Covered in User Mode message
'
'         3.2.4  Topic message .....................................  19
        Case "TOPIC" ' from chanserv or people
        If Not bTopic Then
        'seperate <channel> [ <topic> ]
            For i = 1 To Len(params)
                If Mid(params, i, 1) = Chr(32) Then
                    Channel = Trim(Mid(params, 1, i - 1))   '<channel>
                    params = Mid(params, i + 1)             '<topic>
                    Exit For
                End If
            Next i
            'chop off the leading ":"
            If left(params, 1) = ":" Then params = Mid(params, 2)
            
            If left(Channel, 1) = "&" _
            Or left(Channel, 1) = "#" _
            Or left(Channel, 1) = "+" _
            Or left(Channel, 1) = "!" _
            Then
                If LCase(Source) = LCase("ChanServ") Then
                    AddChatText "*** Topic is '" & params & "'", AC_CYAN
                Else
                    AddChatText "*** " & Source & " changes topic to '" & params & "'", AC_CYAN
                End If
            End If
        End If

'         3.2.5  Names message .....................................  20
        Case "NAMES"
            
'         3.2.6  List message ......................................  21
        Case "LIST"
            
'         3.2.7  Invite message ....................................  21
        Case "INVITE"
        If Not bInvite Then
        'seperate <user> :<channel>
            For i = 1 To Len(params)
                If Mid(params, i, 1) = Chr(32) Then
                    params = Mid(params, i + 1)             '<topic>
                    Exit For
                End If
            Next i
            'chop off the leading ":"
            If left(params, 1) = ":" Then params = Mid(params, 2)
            
            If left(params, 1) = "&" _
            Or left(params, 1) = "#" _
            Or left(params, 1) = "+" _
            Or left(params, 1) = "!" _
            Then AddChatText "*** " & Source & " has invited you to join " & params, AC_CYAN
        End If
            
'         3.2.8  Kick command ......................................  22
        Case "KICK"
            'seperate <channel> and <user> and <comment>
            For i = 1 To Len(params)
                If Mid(params, i, 1) = Chr(32) Then
                    wordNum = wordNum + 1
                    If wordNum = 1 Then
                        Channel = Trim(Mid(params, 1, i - 1))   '<channel> (x=1)
                    ElseIf wordNum = 2 Then
                        Target = Trim(Mid(params, 1, i - 1))    '<user> (x=2)
                    ElseIf wordNum > 2 Then
                        Exit For
                    End If
                    params = Mid(params, i + 1)                 'recycle (x=1) and <comment> (x=2)
                    i = 1
                End If
            Next i
            
            'chop off the leading ":"
            If left(params, 1) = ":" Then params = Mid(params, 2)
            
            If LCase(strChannel) = LCase(Channel) Then
                If LCase(Target) = LCase(strNickName) Then
                    AddChatText "*** You were kicked by " & Source & " (" & params & ")", AC_GREEN
                    For x = 0 To (lChannel.Count - 1)
                        If LCase(lChannel.Data(0, x)) = LCase(Channel) Then
                            lChannel.DeleteRow (x)
                            Exit For
                        End If
                    Next x
                    strChannel = ""
                    stCurrentChannel.Text = "Current Channel:"
                    stUsersInChannel.Text = "Users in Channel:"
                    lUser.Clear
                Else
                    For x = 0 To (lUser.Count - 1)
                        If (LCase(lUser.Data(0, x)) = LCase(Target) _
                         Or LCase(lUser.Data(0, x)) = LCase("@" & Target) _
                         Or LCase(lUser.Data(0, x)) = LCase("+" & Target)) _
                        Then
                            If Not bKick Then AddChatText "*** " & Target & " was kicked by " & Source & " (" & params & ")", AC_GREEN
                            lUser.DeleteRow (x)
                            Exit For
                        End If
                    Next x
                End If
            Else
                If LCase(Target) = LCase(strNickName) Then
                    AddChatText "*** You were kicked from " & Channel & " by " & Source & " (" & params & ")", AC_GREEN
                    For x = 0 To (lChannel.Count - 1)
                        If LCase(lChannel.Data(0, x)) = LCase(Channel) Then
                            lChannel.DeleteRow (x)
                            Exit For
                        End If
                    Next x
                End If
            End If
            
'      3.3  Sending messages .......................................  23
'
'         3.3.1  Private messages ..................................  23
        Case "PRIVMSG"
            Dim CTCP As String
            'seperate <msgtarget> and <text to be sent>
            For i = 1 To Len(params)
                If Mid(params, i, 1) = Chr(32) Then
                    Target = Trim(Mid(params, 1, i - 1))    '<msgtarget>
                    params = Mid(params, i + 1)             '<text to be sent>
                    Exit For
                End If
            Next i
            'chop off the leading ":"
            If left(params, 1) = ":" Then params = Mid(params, 2)
            
            If left(Target, 1) = "&" _
            Or left(Target, 1) = "#" _
            Or left(Target, 1) = "+" _
            Or left(Target, 1) = "!" _
            Then
                If LCase(strChannel) = LCase(Target) Then
                    'if it's an active channel action
'                    If LCase(Left(params, 7)) = LCase("ACTION") Then
'                        If (Right(params, 1) = "") Then params = Left(params, Len(params) - 1)
'                        AddChatText "* " & Source & " " & LTrim(Mid(params, 8)), AC_PURPLE
'                    Else
                    If LCase(left(params, 1)) = LCase("") Then
                        params = Mid(params, 2)
                        If (Right(params, 1) = "") Then params = left(params, Len(params) - 1)
                        CTCP = UCase(FirstWord(params))
                        Select Case CTCP
                            Case "ACTION"
                                If Not bAction And Not bActiveChannel Then
                                    If InStr(LCase(params), LCase(strNickName)) And bHighlight Then
                                        AddChatText "* " & Source & " " & params, textHighlightColor
                                    Else
                                        AddChatText "* " & Source & " " & params, AC_PURPLE
                                    End If
                                End If
                            Case "VERSION"
                                tcp.SendData "NOTICE " & Source & " :VERSION dIRCal v" & App.Major & "." & App.Minor & "." & App.Revision & "" & vbCrLf
                                If Not bCTCP Then AddChatText "[" & Source & " VERSION]", AC_RED
                            Case "TIME"
                                tcp.SendData "NOTICE " & Source & " :TIME " & Now & "" & vbCrLf
                                If Not bCTCP Then AddChatText "[" & Source & " TIME]", AC_RED
                            Case "PING"
                                tcp.SendData "NOTICE " & Source & " :PING " & params & "" & vbCrLf
                                If Not bCTCP Then AddChatText "[" & Source & " PING]", AC_RED
'                            Case "WHO"
'                                'If bReplyWho Then
'                                    tcp.SendData "NOTICE " & Source & " :WHO " & WhoAmI & "" & vbCrLf
'                                'End If
'                                If Not bCTCP Then AddChatText "[" & Source & " WHO]", AC_RED
'                            Case "COORDS"
'                                'If bReplyCoords Then
'                                    tcp.SendData "NOTICE " & Source & " :COORDS " & MyCoords & "" & vbCrLf
'                                'End If
'                                If Not bCTCP Then AddChatText "[" & Source & " COORDS]", AC_RED
                        End Select
                    Else
                        If Not bActiveChannel Then
                            If InStr(LCase(params), LCase(strNickName)) And bHighlight Then
                                'AddChatText "<Tell:IIDString:0:" + "-" & Source & "->" + "<" & Source & ">" + "<\Tell> " & params, textHighlightColor
                                AddChatText "<" & Source & "> " & params, textHighlightColor
                                'AddChatText "Hi mom", textHighlightColor
                            Else
                                If Not bGreenText Then
                                    AddChatText "<" & Source & "> " & params, AC_LTCYAN
                                Else
                                    AddChatText "<Tell:IIDString:0:" & "[" & Source & "]>" & "<" & Source & ">" & "<\Tell> " & params, AC_LTCYAN
                                End If
                            End If
                        End If
                    End If
                Else
                    ' TODO: Add an option here
                    'if it's an inactive channel action
'                    If LCase(Left(params, 7)) = LCase("ACTION") Then
'                        If (Right(params, 1) = "") Then params = Left(params, Len(params) - 1)
'                        AddChatText "* " & LCase(target) & ":" & Source & " " & LTrim(Mid(params, 8)), AC_PURPLE
'                    Else
                    If LCase(left(params, 1)) = LCase("") Then
                        params = Mid(params, 2)
                        If (Right(params, 1) = "") Then params = left(params, Len(params) - 1)
                        CTCP = UCase(FirstWord(params))
                        Select Case CTCP
                            Case "ACTION"
                                If Not bAction And Not bInactiveChannel Then
                                    If InStr(LCase(params), LCase(strNickName)) And bHighlight Then
                                        AddChatText "* " & LCase(Target) & ":" & Source & " " & params, textHighlightColor
                                    Else
                                        AddChatText "* " & LCase(Target) & ":" & Source & " " & params, AC_PURPLE
                                    End If
                                End If
                            Case "VERSION"
                                tcp.SendData "NOTICE " & Source & " :VERSION dIRCal v" & App.Major & "." & App.Minor & "." & App.Revision & "" & vbCrLf
                                If Not bCTCP Then AddChatText "[" & Source & " VERSION]", AC_RED
                            Case "TIME"
                                tcp.SendData "NOTICE " & Source & " :TIME " & Now & "" & vbCrLf
                                If Not bCTCP Then AddChatText "[" & Source & " TIME]", AC_RED
                            Case "PING"
                                tcp.SendData "NOTICE " & Source & " :PING " & params & "" & vbCrLf
                                If Not bCTCP Then AddChatText "[" & Source & " PING]", AC_RED
'                            Case "WHO"
'                                'If bReplyWho Then
'                                    tcp.SendData "NOTICE " & Source & " :WHO " & WhoAmI & "" & vbCrLf
'                                'End If
'                                If Not bCTCP Then AddChatText "[" & Source & " WHO]", AC_RED
'                            Case "COORDS"
'                                'If bReplyCoords Then
'                                    tcp.SendData "NOTICE " & Source & " :COORDS " & MyCoords & "" & vbCrLf
'                                'End If
'                                If Not bCTCP Then AddChatText "[" & Source & " COORDS]", AC_RED
                        End Select
                    Else
                        If Not bInactiveChannel Then
                            If InStr(LCase(params), LCase(strNickName)) And bHighlight Then
                                AddChatText "<" & LCase(Target) & ":" & Source & "> " & params, textHighlightColor
                            Else
                                AddChatText "<" & LCase(Target) & ":" & Source & "> " & params, AC_LTCYAN
                            End If
                        End If
                    End If
                End If
            Else
            'Direct PRIVMSG
                If LCase(left(params, 1)) = LCase("") Then
                    params = Mid(params, 2)
                    If (Right(params, 1) = "") Then params = left(params, Len(params) - 1)
                    CTCP = UCase(FirstWord(params))
                    Select Case CTCP
                        Case "ACTION"
                            'If Not bAction And Not bPrivate Then
                            If Not bAction Then AddChatText "* " & Source & " " & params, AC_PURPLE
                        Case "VERSION"
                            tcp.SendData "NOTICE " & Source & " :VERSION dIRCal v" & App.Major & "." & App.Minor & "." & App.Revision & "" & vbCrLf
                            If Not bCTCP Then AddChatText "[" & Source & " VERSION]", AC_RED
                        Case "TIME"
                            tcp.SendData "NOTICE " & Source & " :TIME " & Now & "" & vbCrLf
                            If Not bCTCP Then AddChatText "[" & Source & " TIME]", AC_RED
                        Case "PING"
                            tcp.SendData "NOTICE " & Source & " :PING " & params & "" & vbCrLf
                            If Not bCTCP Then AddChatText "[" & Source & " PING]", AC_RED
'                        Case "WHO"
'                            'If bReplyWho Then
'                                tcp.SendData "NOTICE " & Source & " :WHO " & WhoAmI & "" & vbCrLf
'                            'End If
'                            If Not bCTCP Then AddChatText "[" & Source & " WHO]", AC_RED
'                        Case "COORDS"
'                            'If bReplyCoords Then
'                                tcp.SendData "NOTICE " & Source & " :COORDS " & MyCoords & "" & vbCrLf
'                            'End If
'                            If Not bCTCP Then AddChatText "[" & Source & " COORDS]", AC_RED
                    End Select
                Else
                    'If Not bPrivate Then
                        If Not bGreenText Then
                            AddChatText "<" & Source & "> tells you, """ & params & """", AC_PINK
                        Else
                            AddChatText "<Tell:IIDString:0:" & "[" & Source & "]>" & "<" & Source & ">" & "<\Tell> tells you, """ & params & """", AC_PINK
                        End If
                        strReplyTo = Source
                    'End If
                End If
            End If

'         3.3.2  Notice ............................................  24
        Case "NOTICE"
            For i = 1 To Len(params)
                If Mid(params, i, 1) = Chr(32) Then
                    Target = Trim(Mid(params, 1, i - 1))    '<target>
                    params = Mid(params, i + 1)             '<text to be sent>
                    Exit For
                End If
            Next i
            'chop off the leading ":"
            If left(params, 1) = ":" Then params = Mid(params, 2)
            
            If UCase(Target) = "AUTH" Then
                If Not bNoticeAuth Then
                    AddChatText params, AC_BLUE
                End If
            ElseIf (UCase(Target) = UCase(strNickName)) And (LCase(left(params, 1)) = LCase("")) Then
                params = Mid(params, 2)
                If (Right(params, 1) = "") Then params = left(params, Len(params) - 1)
                CTCP = UCase(FirstWord(params))
                Select Case CTCP
                    Case "VERSION"
                        AddChatText "[" & Source & " " & CTCP & " reply]: " & params, AC_PINK
                    Case "TIME"
                        AddChatText "[" & Source & " " & CTCP & " reply]: " & params, AC_PINK
                    Case "PING"
                        Dim PingTime As Long
                        PingTime = Timer - PingStart
                        AddChatText "[" & Source & " " & CTCP & " reply]: " & PingTime & " seconds", AC_PINK
'                    Case "WHO"
'                        AddChatText "[" & Source & " " & CTCP & " reply]: " & params, AC_PINK
'                    Case "COORDS"
'                        AddChatText "[" & Source & " " & CTCP & " reply]: " & params, AC_PINK
                End Select
            Else
                If Not bNotice Then AddChatText Source & "> " & params, AC_CYAN
            End If

'      3.4  Server queries and commands ............................  25
'
'         3.4.1  Motd message ......................................  25
        Case "MOTD"
            
'         3.4.2  Lusers message ....................................  25
        Case "LUSERS"

'         3.4.3  Version message ...................................  26
        Case "VERSION"

'         3.4.4  Stats message .....................................  26
        Case "STATS"

'         3.4.5  Links message .....................................  27
        Case "LINKS"

'         3.4.6  Time message ......................................  28
        Case "TIME"

'         3.4.7  Connect message ...................................  28
        Case "CONNECT"

'         3.4.8  Trace message .....................................  29
        Case "TRACE"

'         3.4.9  Admin command .....................................  30
        Case "ADMIN"

'         3.4.10 Info command ......................................  31
        Case "INFO"

'      3.5  Service Query and Commands .............................  31
'
'         3.5.1  Servlist message ..................................  31
        Case "SERVLIST"

'         3.5.2  Squery ............................................  32
        Case "SQUERY"

'      3.6  User based queries .....................................  32
'
'         3.6.1  Who query .........................................  32
        Case "WHO"

'         3.6.2  Whois query .......................................  33
        Case "WHOIS"

'         3.6.3  Whowas ............................................  34
        Case "WHOWAS"

'      3.7  Miscellaneous messages .................................  34
'
'         3.7.1  Kill message ......................................  35
        Case "KILL"

'         3.7.2  Ping message ......................................  36
        Case "PING"
            If bCommandDebug = True Then SetStatus "Ping!"
            If left(params, 1) = ":" Then params = Mid(params, 2)
            tcp.SendData "PONG " & params & vbCrLf
            If bCommandDebug = True Then SetStatus "Pong!"
            
'         3.7.3  Pong message ......................................  37
        Case "PONG"

'         3.7.4  Error .............................................  37
        Case "ERROR"
            If bCommandDebug = True Then AddChatText "prefix = " & prefix & " | command = " & command & " | params = " & params, AC_GREEN
            SetStatus Mid(params, 2)
            strChannel = ""
            strNickName = ""
            strServer = ""
            strServerPort = ""
            stCurrentChannel.Text = "Current Channel:"
            stUsersInChannel.Text = "Users in Channel:"
            lChannel.Clear
            lUser.Clear
            If Not tcp.state = sckClosed Then tcp.Close
            btnConnect.Text = "Connect"
            SetStatus "Disconnected!"
            'MainView.SetIcon &H600137B
            SetMainIcon STATE_DISCONNECTED
                                        
                                        ' ' N U M E R I C   R E S P O N S E S ' '

                                        ' ' N U M E R I C   R E S P O N S E S ' '

                                        ' ' N U M E R I C   R E S P O N S E S ' '

        Case "001"
'       001    RPL_WELCOME
'              "Welcome to the Internet Relay Network <nick>!<user>@<host>"
            'MainView.SetIcon &H6000516
            SetMainIcon STATE_CONNECTED
            btnConnect.Text = "Disconnect"
            Target = FirstWord(params)
            If Not (strNickName = Target) Then
                strNickName = Target
                Call edNick_Change(0, strNickName)
                edNick.Text = strNickName
            End If
'            dIRCalNotify.ISON.Enabled = True
        Case "002"
'       002    RPL_YOURHOST
'              "Your host is <servername>, running version <ver>"
            'MainView.SetIcon &H6000516
            SetMainIcon STATE_CONNECTED
            btnConnect.Text = "Disconnect"
            Target = FirstWord(params)
            If Not (strNickName = Target) Then
                strNickName = Target
                Call edNick_Change(0, strNickName)
                edNick.Text = strNickName
            End If
'            dIRCalNotify.ISON.Enabled = True
        Case "003"
'       003    RPL_CREATED
'              "This server was created <date>"
            'MainView.SetIcon &H6000516
            SetMainIcon STATE_CONNECTED
            btnConnect.Text = "Disconnect"
            Target = FirstWord(params)
            If Not (strNickName = Target) Then
                strNickName = Target
                Call edNick_Change(0, strNickName)
                edNick.Text = strNickName
            End If
'            dIRCalNotify.ISON.Enabled = True
        Case "004"
'       004    RPL_MYINFO
'              "<servername> <version> <available user modes> <available channel modes>"
            'MainView.SetIcon &H6000516
            SetMainIcon STATE_CONNECTED
            btnConnect.Text = "Disconnect"
            Target = FirstWord(params)
            If Not (strNickName = Target) Then
                strNickName = Target
                Call edNick_Change(0, strNickName)
                edNick.Text = strNickName
            End If
'            dIRCalNotify.ISON.Enabled = True

        Case "005"
'       005    RPL_BOUNCE
'              "Try server <server name>, port <port number>"
'            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "200"
'       200    RPL_TRACELINK
'              "Link <version & debug level> <destination> <next server> V<protocol version> <link uptime in seconds> <backstream sendq> <upstream sendq>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "201"
'       201    RPL_TRACECONNECTING
'              "Try. <class> <server>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "202"
'       202    RPL_TRACEHANDSHAKE
'              "H.S. <class> <server>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "203"
'       203    RPL_TRACEUNKNOWN
'              "???? <class> [<client IP address in dot form>]"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "204"
'       204    RPL_TRACEOPERATOR
'              "Oper <class> <nick>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "205"
'       205    RPL_TRACEUSER
'              "User <class> <nick>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "206"
'       206    RPL_TRACESERVER
'              "Serv <class> <int>S <int>C <server> <nick!user|*!*>@<host|server> V<protocol version>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "207"
'       207    RPL_TRACESERVICE
'              "Service <class> <name> <type> <active type>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "208"
'       208    RPL_TRACENEWTYPE
'              "<newtype> 0 <client name>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "209"
'       209    RPL_TRACECLASS
'              "Class <class> <count>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
'        Case "210"
'       210    RPL_TRACERECONNECT
'              Unused.
        
        Case "211"
'       211    RPL_STATSLINKINFO
'              "<linkname> <sendq> <sent messages> <sent Kbytes> <received messages> <received Kbytes> <time open>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
        Case "212"
'       212    RPL_STATSCOMMANDS
'              "<command> <count> <byte count> <remote count>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
        Case "219"
'       219    RPL_ENDOFSTATS
'              "<stats letter> :End of STATS report"
            Call SimpleParseOne(params, bInfo, AC_GREEN)
        
        Case "221"
'       221    RPL_UMODEIS
'              "<user mode string>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "234"
'       234    RPL_SERVLIST
'              "<name> <server> <mask> <type> <hopcount> <info>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "235"
'       235    RPL_SERVLISTEND
'              "<mask> <type> :End of service listing"
            Call SimpleParseTwo(params, bInfo, AC_GREEN)

        Case "242"
'       242    RPL_STATSUPTIME
'              ":Server Up %d days %d:%02d:%02d"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
        Case "243"
'       243    RPL_STATSOLINE
'              "O <hostmask> * <name>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
        Case "251"
'       251    RPL_LUSERCLIENT
'              ":There are <integer> users and <integer> services on <integer> servers"
            Call SimpleParseZero(params, bNoticeAuth, AC_GREEN)
        Case "252"
'       252    RPL_LUSEROP
'              "<integer> :operator(s) online"
            Call SimpleParseOne(params, bNoticeAuth, AC_GREEN)
        Case "253"
'       253    RPL_LUSERUNKNOWN
'              "<integer> :unknown connection(s)"
            Call SimpleParseOne(params, bNoticeAuth, AC_GREEN)
        Case "254"
'       254    RPL_LUSERCHANNELS
'              "<integer> :channels formed"
            Call SimpleParseOne(params, bNoticeAuth, AC_GREEN)
        Case "255"
'       255    RPL_LUSERME
'              ":I have <integer> clients and <integer> servers"
            Call SimpleParseZero(params, bNoticeAuth, AC_GREEN)
        
        Case "256"
'       256    RPL_ADMINME
'              "<server> :Administrative info"
            Call SimpleParseOne(params, bInfo, AC_GREEN)
        Case "257"
'       257    RPL_ADMINLOC1
'              ":<admin info>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "258"
'       258    RPL_ADMINLOC2
'              ":<admin info>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "259"
'       259    RPL_ADMINEMAIL
'              ":<admin info>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "261"
'       261    RPL_TRACELOG
'              "File <logfile> <debug level>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "262"
'       262    RPL_TRACEEND
'              "<server name> <version & debug level> :End of TRACE"
            Call SimpleParseTwo(params, bInfo, AC_GREEN)
        
        Case "263"
'       263    RPL_TRYAGAIN
'              "<command> :Please wait a while and try again."
            Call SimpleParseOne(params, bInfo, AC_GREEN)
        
        Case "302"
'       302    RPL_USERHOST
'              ":*1<reply> *( " " <reply> )"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
        Case "303"
'       303    RPL_ISON
'              ":*1<nick> *( " " <nick> )"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "301"
'       301    RPL_AWAY
'              "<nick> :<away message>"
            Call SimpleParseOne(params, bInfo, AC_GREEN)
        Case "305"
'       305    RPL_UNAWAY
'              ":You are no longer marked as being away"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "306"
'       306    RPL_NOWAWAY
'              ":You have been marked as being away"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "311"
'       311    RPL_WHOISUSER
'              "<nick> <user> <host> * :<real name>"
            Target = FirstWord(params)
            If Not bWhois Then AddChatText FirstWord(params) & " is " & FirstWord(params) & "@" & FirstWord(params) & " " & FirstWord(params) & " " & Mid(params, 2), AC_GREEN

        Case "312"
'       312    RPL_WHOISSERVER
'              "<nick> <server> :<server info>"
            Target = FirstWord(params)
            If Not bWhois Then AddChatText FirstWord(params) & " using " & FirstWord(params) & " " & Mid(params, 2), AC_GREEN

        Case "313"
'       313    RPL_WHOISOPERATOR
'              "<nick> :is an IRC operator"
            Call FancyParseOne(params, bWhois, AC_GREEN)

        Case "314"
'       314    RPL_WHOWASUSER
'              "<nick> <user> <host> * :<real name>"
            Target = FirstWord(params)
            If Not bWhois Then AddChatText FirstWord(params) & " was " & FirstWord(params) & "@" & FirstWord(params) & " " & FirstWord(params) & " " & Mid(params, 2), AC_GREEN
        
        Case "315"
'       315    RPL_ENDOFWHO
'              "<name> :End of WHO list"
            Call FancyParseOne(params, bWhois, AC_GREEN)

        Case "317"
'       317    RPL_WHOISIDLE
'              "<nick> <integer> :seconds idle"
            Target = FirstWord(params)
            If Not bWhois Then AddChatText FirstWord(params) & " idle " & FirstWord(params) & " seconds", AC_GREEN

        Case "318"
'       318    RPL_ENDOFWHOIS
'              "<nick> :End of WHOIS list"
            Call FancyParseOne(params, bWhois, AC_GREEN)

        Case "319"
'       319    RPL_WHOISCHANNELS
'              "<nick> :*( ( "@" / "+" ) <channel> " " )"
            Target = FirstWord(params)
            If Not bWhois Then AddChatText FirstWord(params) & " on " & Mid(params, 2), AC_GREEN

        
'        Case "321"
'       321    RPL_LISTSTART
'              Obsolete. Not used.
        
        Case "322"
'       322    RPL_LIST
'              "<channel> <# visible> :<topic>"
            Call SimpleParseTwo(params, bInfo, AC_GREEN)
        Case "323"
'       323    RPL_LISTEND
'              ":End of LIST"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
        Case "325"
'       325    RPL_UNIQOPIS
'              "<channel> <nickname>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "324"
'       324    RPL_CHANNELMODEIS
'              "<channel> <mode> <mode params>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "331"
'       331    RPL_NOTOPIC
'              "<channel> :No topic is set"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "332" ' not from chanserv or people
'       332    RPL_TOPIC
'              "<channel> :<topic>"
            If Not bTopic Then
                For i = 1 To Len(params)
                    If Mid(params, i, 1) = Chr(32) Then
                        wordNum = wordNum + 1
                        If wordNum = 1 Then
                            'user = Trim(Mid(params, 1, i - 1))      '<user> (x=1)
                        ElseIf wordNum = 2 Then
                            Channel = Trim(Mid(params, 1, i - 1))   '<channel> (x=2)
                        ElseIf wordNum > 2 Then
                            Exit For
                        End If
                        params = Mid(params, i + 1)                 'recycle (x=1) and <comment> (x=2)
                    i = 1
                    End If
                Next i
            
                'chop off the leading ":"
                If left(params, 1) = ":" Then params = Mid(params, 2)
                If (left(Channel, 1) = "&" _
                Or left(Channel, 1) = "#" _
                Or left(Channel, 1) = "+" _
                Or left(Channel, 1) = "!") _
                And Len(params) > 0 Then AddChatText "*** Topic is '" & params & "'", AC_LTCYAN
            End If

        Case "333"
'        Case "333" 'command = 333 | params = Oscar_Meyer #ag Lane_Meyer 999879948
            '
        
        Case "341"
'       341    RPL_INVITING
'              "<channel> <nick>"
            Target = FirstWord(params)
            Target = FirstWord(params)
            Channel = FirstWord(params)
            If Not bInvite Then AddChatText "You have invited " & Target & " to " & Channel, AC_GREEN
        
        Case "342"
'       342    RPL_SUMMONING
'              "<user> :Summoning user to IRC"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "346"
'       346    RPL_INVITELIST
'              "<channel> <invitemask>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "347"
'       347    RPL_ENDOFINVITELIST
'              "<channel> :End of channel invite list"
            Call FancyParseOne(params, bInfo, AC_GREEN)

        Case "348"
'       348    RPL_EXCEPTLIST
'              "<channel> <exceptionmask>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "349"
'       349    RPL_ENDOFEXCEPTLIST
'              "<channel> :End of channel exception list"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "351"
'       351    RPL_VERSION
'              "<version>.<debuglevel> <server> :<comments>"
            Call SimpleParseTwo(params, bInfo, AC_GREEN)
        
        Case "352"
'       352    RPL_WHOREPLY
'              "<channel> <user> <host> <server> <nick> ( "H" / "G" > ["*"] [ ( "@" / "+" ) ] :<hopcount> <real name>"
            Dim WhoChannel As String
            Target = FirstWord(params)
            Channel = FirstWord(params)
            WhoChannel = FirstWord(params) & "@" & FirstWord(params) & " " & FirstWord(params)
            If Not bWhois Then AddChatText Channel & " " & FirstWord(params) & " " & FirstWord(params) & " " & WhoChannel & " " & params, AC_GREEN
        
        Case "353"
'       353    RPL_NAMREPLY
'              "( "=" / "*" / "@" ) <channel> :[ "@" / "+" ] <nick> *( " " [ "@" / "+" ] <nick> )
'         - "@" is used for secret channels, "*" for private
'           channels, and "=" for others (public channels).

':valinor.sorcery.net 353 Oscar_Meyer = #beginner :Oscar_Meyer D`atuk Kalroth AyhaM30 +[wItcHi3] big_bird eva elizabeth @Slaughter dewa^^sakti eric^86 [[HawK]]a i-L[]v3-xiA0-LiNg @Blowfish_1363 jbbb +E|aiNe^16^f GEMINI^^^^^^ g_vtec {[GeNgStEar]} jack_lingam @kane willy SWEETI^PIE AJ^^ noms23 BONITO kelateeee Surayaama cicakkubin chaosssss Green_Jack^^^^ `[U]nClea Vahswar friendllyguy +wax @prabu
':bluestream.sorcery.net 353 Oscar_Meyer = #beginner :Lisa_Gal @s[H]sIkuA sode9`a Kenny_Matsunami @IceMan vtech Fancy^ dingin-salju shey28 WLSF1437 syah playful^toyboy ss-boy +I-L[]v3-aH-m|n @GT-R amrit jacky muratcan^^ +I-L[]v3-aH-Loon dewa^^sakti [[HawK]]a jbbb g_vtec {[GeNgStEar]} SWEETI^PIE AJ^^ noms23 BONITO kelateeee Surayaama cicakkubin Green_Jack^^^^ `[U]nClea Vahswar friendllyguy @prabu
':nebula.sorcery.net 353 Oscar_Meyer @ #acdev :Oscar_Meyer @Grey parad0x @Arkayas +Angel_Deepdale Smileyx Cephas +Solandra Arcanor_MT anarax Akilla|Sleep Viscid Vain Blackhawk|Sleepin Frenchie Ethon Leg Sort|Snoozin codex|sleep ThorAzaroth sidewalk` Lane @Wembly @Checksum @elph @Your_God Lasivian GA|ZzZzZzZ TemplarArqD @Cibo_Matto loap Nzom_zzzZzzzZ Draken ent|semiafk +Cibo|work HoNam_afk doodle Kal|work acdevstats Raeven +elph__ Henrik Annika|Aargau +NerfGolem PugMajere
':nebula.sorcery.net 353 Oscar_Meyer @ #asherons-call :Oscar_Meyer Asherawn @Zammie penelope Aman_Ra Arkayas @Cephas Shazram @ramandu elph @anarax @Nedry Gireal|FFX Akye Akilla|Sleep @Bahamut Frenchie Mass_Work @Leg Juffo-Wup Ethon codex|sleep Reeves @dm|notathome @Tier|Work loap @Uriel Cibo|work doodle Burke Kal|work elph__ thelonius dm|notatwork vision Mass_WE NerfGolem PugMajere
':cookie.sorcery.net 353 Oscar_Meyer @ #acdev :+Oscar_Meyer pwh Darthakia Cephas +sentou Kalroth|wrk anarax Arcanor_MT GA Synnah XuqAtWork Portalstorm @elph parad0x loap gouru|atwork ThorAzaroth AsAFK Fizzle_Storm Nzom_Moving Bats @Kalroth Ydria +Solandra doodle @Keba Vain [Entr0phy] furikuri Blackhawk|Sleepin @Hazridi HoNam|zzz @Greyed ana-afk Leg\faraway likwid Draken Darien PugMajere Codex @Checksum Cibo|work acdevstats +elph__ +NerfGolem Daven TemplarArqD Cibo_Matto @Lane +Gonjin|sleep Apoztle Wembly
':cookie.sorcery.net 353 Oscar_Meyer @ #acdev :Lock +Oscar_Meyer pwh Darthakia Cephas +sentou Kalroth|wrk anarax Arcanor_MT GA Synnah XuqAtWork Portalstorm @elph parad0x loap gouru|atwork ThorAzaroth AsAFK Fizzle_Storm Nzom_Moving Bats @Kalroth Ydria +Solandra doodle @Keba Vain [Entr0phy] furikuri Blackhawk|Sleepin @Hazridi HoNam|zzz @Greyed ana-afk Leg\faraway likwid Draken Darien PugMajere Codex @Checksum Cibo|work acdevstats +elph__ +NerfGolem Daven TemplarArqD Cibo_Matto @Lane +Gonjin|sleep Apoztle Wem
':cookie.sorcery.net 353 Oscar_Meyer @ #acdev :@Lane_Meyer +Oscar_Meyer Darthakia Cephas +sentou Kalroth|wrk anarax Arcanor_MT GA Synnah XuqAtWork Portalstorm @elph parad0x loap gouru|atwork ThorAzaroth AsAFK Fizzle_Storm Nzom_Moving Bats @Kalroth Ydria +Solandra doodle @Keba Vain [Entr0phy] furikuri Blackhawk|Sleepin @Hazridi HoNam|zzz @Greyed ana-afk Leg\faraway likwid Draken Darien PugMajere Codex @Checksum Cibo|work acdevstats +elph__ +NerfGolem Daven TemplarArqD Cibo_Matto @Lane +Gonjin|sleep Apoztle
':cookie.sorcery.net 353 Oscar_Meyer @ #acdev :@Lane_Meyer +Oscar_Meyer Darthakia Cephas +sentou Kalroth|wrk anarax Arcanor_MT GA Synnah XuqAtWork Portalstorm @elph parad0x loap gouru|atwork ThorAzaroth AsAFK Fizzle_Storm Nzom_Moving @Kalroth Ydria +Solandra doodle @Keba Vain [Entr0phy] furikuri Blackhawk|Sleepin @Hazridi HoNam|zzz @Greyed ana-afk Leg\faraway likwid Draken Darien PugMajere Codex @Checksum Cibo|work acdevstats +elph__ +NerfGolem Daven TemplarArqD Cibo_Matto @Lane +Gonjin|sleep Apoztle Wembl
':cookie.sorcery.net 353 Oscar_Meyer @ #acdev :@Lane_Meyer +Oscar_Meyer Darthakia Cephas +sentou Kalroth|wrk anarax Arcanor_MT GA Synnah XuqAtWork Portalstorm @elph parad0x loap gouru|atwork ThorAzaroth Asriel Fizzle_Storm Nzom_Moving @Kalroth Ydria +Solandra doodle @Keba Vain [Entr0phy] furikuri Blackhawk|Sleepin @Hazridi HoNam|zzz @Greyed ana-afk Leg\faraway likwid Draken Darien PugMajere Codex @Checksum Cibo|work acdevstats +elph__ +NerfGolem Daven TemplarArqD Cibo_Matto @Lane +Gonjin|sleep Apoztle Wemb
':cookie.sorcery.net 353 Oscar_Meyer @ #acdev :@Lane_Meyer_______ +Oscar_Meyer Darthakia Cephas +sentou Kalroth|wrk anarax Arcanor_MT GA Synnah Portalstorm @elph loap gouru|atwork ThorAzaroth Asriel Fizzle_Storm Nzom_Moving @Kalroth Ydria +Solandra doodle @Keba Vain [Entr0phy] furikuri Blackhawk|Sleepin @Hazridi HoNam|zzz @Greyed ana-afk Leg\faraway likwid Draken Darien PugMajere Codex @Checksum Cibo|work acdevstats +elph__ +NerfGolem Daven TemplarArqD Cibo_Matto @_____Lane________ +Gonjin|sleep Apoztle We
':cookie.sorcery.net 353 _____Lane________ @ #acdev :@Lane_Meyer_______ +Oscar_Meyer Darthakia Cephas +sentou Kalroth|wrk anarax Arcanor_MT GA Synnah Portalstorm @elph loap gouru|atwork ThorAzaroth Asriel Fizzle_Storm Nzom_Moving @Kalroth Ydria +Solandra doodle @Keba Vain [Entr0phy] furikuri Blackhawk|Sleepin @Hazridi HoNam|zzz @Greyed ana-afk Leg\faraway likwid Draken Darien PugMajere Codex @Checksum Cibo|work acdevstats +elph__ +NerfGolem Daven TemplarArqD Cibo_Matto @_____Lane________ +Gonjin|sleep Apoz

            For i = 1 To Len(params)
                If Mid(params, i, 1) = Chr(32) Then
                    wordNum = wordNum + 1
                    If wordNum = 3 Then
                        Channel = Trim(Mid(params, 1, i - 1))   '<channel> (x=1)
                    ElseIf wordNum > 3 Then
                        Exit For
                    End If
                    params = Mid(params, i + 1)                 'recycle (x=1) and <comment> (x=2)
                    i = 1
                End If
            Next i
            
            'chop off the leading ":"
            If left(params, 1) = ":" Then params = Mid(params, 2)
            
            If (LCase(strChannel) = LCase(Channel)) And (b366 = True) Then lUser.Clear
            b366 = False
            
            Dim ChanName(1 To 1000) As String
            'seperate names
            For i = 1 To Len(params)
                If Mid(params, i, 1) = Chr(32) Then
                    x = x + 1
                    ChanName(x) = Trim(Mid(params, 1, i - 1))
                    params = Trim(Mid(params, i + 1))
                    
                    'add name to user listbox
                    n = lUser.AddRow
                    lUser.Data(0, n) = Trim(ChanName(x))

                    'For ix = 1 To 10
                    '    If LCase(ChannelName(ix)) = LCase(ChanName(2)) And x > 2 Then
                    '        Channel(ix).lstNames.AddItem ChanName(x)
                    '    End If
                    'Next ix
                    i = 0
                End If
            Next i
            'get last name in 353
            x = x + 1
            ChanName(x) = params
            n = lUser.AddRow
            lUser.Data(0, n) = Trim(ChanName(x))

        Case "364"
'       364    RPL_LINKS
'              "<mask> <server> :<hopcount> <server info>"
            Call FancyParseTwo(params, bInfo, AC_GREEN)
        Case "365"
'       365    RPL_ENDOFLINKS
'              "<mask> :End of LINKS list"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "366"
'       366    RPL_ENDOFNAMES
'              "<channel> :End of NAMES list"
            b366 = True

        Case "367"
'       367    RPL_BANLIST
'              "<channel> <banmask>"
            Target = FirstWord(params)
            If Not bMode Then AddChatText FirstWord(params) & " +b " & FirstWord(params) & " set by " & FirstWord(params), AC_GREEN
        
        Case "368"
'       368    RPL_ENDOFBANLIST
'              "<channel> :End of channel ban list"
            Call FancyParseOne(params, bMode, AC_GREEN)

        Case "369"
'       369    RPL_ENDOFWHOWAS
'              "<nick> :End of WHOWAS"
            Call FancyParseOne(params, bWhois, AC_GREEN)

        Case "371"
'       371    RPL_INFO
'              ":<string>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
        Case "372"
'       372    RPL_MOTD
'              ":- <text>"
            'cut off the first word if it doesn't start with a ":"
            If bCommandDebug = True Then
                If Not left(params, 1) = ":" Then
                    For i = 1 To Len(params)
                        If Mid(params, i, 1) = Chr(32) Then
                            If left(params, 1) = ":" Then Exit For
                            params = Mid(params, i + 1)
                            i = 1
                        End If
                    Next i
                End If
                'chop off the ":" in front of all parameters
                If left(params, 1) = ":" Then params = Mid(params, 2)
                AddChatText params, AC_RED
            End If

        Case "374"
'       374    RPL_ENDOFINFO
'              ":End of INFO list"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
        Case "375"
'       375    RPL_MOTDSTART
'              ":- <server> Message of the day - "
            'find the first word that starts with a ":"
            If bCommandDebug = True Then
                If Not left(params, 1) = ":" Then
                    For i = 1 To Len(params)
                        If Mid(params, i, 1) = Chr(32) Then
                            If left(params, 1) = ":" Then Exit For
                            params = Mid(params, i + 1)
                            i = 1
                        End If
                    Next i
                End If
                'chop off the ":" in front of all parameters
                If left(params, 1) = ":" Then params = Mid(params, 2)
                AddChatText params, AC_RED
            End If

        Case "376"
'       376    RPL_ENDOFMOTD
'              ":End of MOTD command"
            'cut off the first word if it doesn't start with a ":"
            If bCommandDebug = True Then
                If Not left(params, 1) = ":" Then
                    For i = 1 To Len(params)
                        If Mid(params, i, 1) = Chr(32) Then
                            If left(params, 1) = ":" Then Exit For
                            params = Mid(params, i + 1)
                            i = 1
                        End If
                    Next i
                End If
                'chop off the ":" in front of all parameters
                If left(params, 1) = ":" Then params = Mid(params, 2)
                AddChatText params, AC_RED
            End If
            
            SetStatus "Connected!"
            tcp.SendData ("MODE " & strNickName & " +i" & vbCrLf)
            If Reg.Value("PerformOnConnect") = True Then PerformOnConnect
            If Trim(notNULL(edChannel.Text)) <> "" Then tcp.SendData ("JOIN " & notNULL(edChannel.Text) & vbCrLf)

        Case "381"
'       381    RPL_YOUREOPER
'              ":You are now an IRC operator"
'
'         - RPL_YOUREOPER is sent back to a client which has
'           just successfully issued an OPER message and gained
'           operator status.
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "382"
'       382    RPL_REHASHING
'              "<config file> :Rehashing"
'
'         - If the REHASH option is used and an operator sends
'           a REHASH message, an RPL_REHASHING is sent back to
'           the operator.
            Call SimpleParseOne(params, bInfo, AC_GREEN)

        Case "383"
'       383    RPL_YOURESERVICE
'              "You are service <servicename>"
'
'         - Sent by the server to a service upon successful
'           registration.
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "391"
'       391    RPL_TIME
'              "<server> :<string showing server's local time>"
'
'         - When replying to the TIME message, a server MUST send
'           the reply using the RPL_TIME format above.  The string
'           showing the time need only contain the correct day and
'           time there.  There is no further requirement for the
'           time string.
            Call SimpleParseOne(params, bInfo, AC_GREEN)

        Case "392"
'       392    RPL_USERSSTART
'              ":UserID   Terminal  Host"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "393"
'       393    RPL_USERS
'              ":<username> <ttyline> <hostname>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "394"
'       394    RPL_ENDOFUSERS
'              ":End of users"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "395"
'       395    RPL_NOUSERS
'              ":Nobody logged in"
'
'         - If the USERS message is handled by a server, the
'           replies RPL_USERSTART, RPL_USERS, RPL_ENDOFUSERS and
'           RPL_NOUSERS are used.  RPL_USERSSTART MUST be sent
'           first, following by either a sequence of RPL_USERS
'           or a single RPL_NOUSER.  Following this is
'           RPL_ENDOFUSERS.
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
'5.2 Error Replies
'       Error replies are found in the range from 400 to 599.
        
        Case "401"
'       401    ERR_NOSUCHNICK
'              "<nickname> :No such nick/channel"
            Call FancyParseOne(params, bWhois, AC_GREEN)

        Case "402"
'       402    ERR_NOSUCHSERVER
'              "<server name> :No such server"
            Call FancyParseOne(params, bInfo, AC_GREEN)

        Case "403"
'       403    ERR_NOSUCHCHANNEL
'              "<channel name> :No such channel"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "404"
'       404    ERR_CANNOTSENDTOCHAN
'              "<channel name> :Cannot send to channel"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "405"
'       405    ERR_TOOMANYCHANNELS
'              "<channel name> :You have joined too many channels"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "406"
'       406    ERR_WASNOSUCHNICK
'              "<nickname> :There was no such nickname"
            Call FancyParseOne(params, bWhois, AC_GREEN)

        Case "407"
'       407    ERR_TOOMANYTARGETS
'              "<target> :<error code> recipients. <abort message>"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "408"
'       408    ERR_NOSUCHSERVICE
'              "<service name> :No such service"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "409"
'       409    ERR_NOORIGIN
'              ":No origin specified"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
        Case "411"
'       411    ERR_NORECIPIENT
'              ":No recipient given (<command>)"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "412"
'       412    ERR_NOTEXTTOSEND
'              ":No text to send"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        Case "413"
'       413    ERR_NOTOPLEVEL
'              "<mask> :No toplevel domain specified"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        Case "414"
'       414    ERR_WILDTOPLEVEL
'              "<mask> :Wildcard in toplevel domain"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        Case "415"
'       415    ERR_BADMASK
'              "<mask> :Bad Server/host mask"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "421"
'       421    ERR_UNKNOWNCOMMAND
'              "<command> :Unknown command"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "422"
'       422    ERR_NOMOTD
'              ":MOTD File is missing"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "423"
'       423    ERR_NOADMININFO
'              "<server> :No administrative info available"
            Call FancyParseOne(params, bInfo, AC_GREEN)

        Case "424"
'       424    ERR_FILEERROR
'              ":File error doing <file op> on <file>"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
        Case "431"
'       431    ERR_NONICKNAMEGIVEN
'              ":No nickname given"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "432"
'       432    ERR_ERRONEUSNICKNAME
'              "<nick> :Erroneous nickname"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "433"
'       433    ERR_NICKNAMEINUSE
'              "<nick> :Nickname is already in use"
            Call FancyParseOne(params, bInfo, AC_CYAN)
            If btnConnect.Text <> "Disconnect" Then
                Dim newNick As String
                Randomize
                newNick = Mid(Trim(notNULL(edNick.Text)), 1, Len(Trim(notNULL(edNick.Text))) - 1) & Int((Rnd * 1000) + 0.5)
                tcp.SendData ("NICK " & newNick & vbCrLf)
                strNickName = newNick
            End If

        Case "436"
'       436    ERR_NICKCOLLISION
'              "<nick> :Nickname collision KILL from <user>@<host>"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "437"
'       437    ERR_UNAVAILRESOURCE
'              "<nick/channel> :Nick/channel is temporarily unavailable"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "441"
'       441    ERR_USERNOTINCHANNEL
'              "<nick> <channel> :They aren't on that channel"
            Call FancyParseTwo(params, bInfo, AC_GREEN)
        
        Case "442"
'       442    ERR_NOTONCHANNEL
'              "<channel> :You're not on that channel"
            Call FancyParseOne(params, bInvite, AC_GREEN)

        Case "443"
'       443    ERR_USERONCHANNEL
'              "<user> <channel> :is already on channel"
            Target = FirstWord(params)
            Target = FirstWord(params)
            Channel = FirstWord(params)
            If Not bInfo Then AddChatText Target & " " & Mid(params, 2) & " " & Channel, AC_GREEN
        
        Case "444"
'       444    ERR_NOLOGIN
'              "<user> :User not logged in"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        
        Case "445"
'       445    ERR_SUMMONDISABLED
'              ":SUMMON has been disabled"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "446"
'       446    ERR_USERSDISABLED
'              ":USERS has been disabled"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "451"
'       451    ERR_NOTREGISTERED
'              ":You have not registered"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "461"
'       461    ERR_NEEDMOREPARAMS
'              "<command> :Not enough parameters"
            Call FancyParseOne(params, bInfo, AC_GREEN)

        Case "462"
'       462    ERR_ALREADYREGISTRED
'              ":Unauthorized command (already registered)"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "463"
'       463    ERR_NOPERMFORHOST
'              ":Your host isn't among the privileged"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "464"
'       464    ERR_PASSWDMISMATCH
'              ":Password incorrect"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
        Case "465"
'       465    ERR_YOUREBANNEDCREEP
'              ":You are banned from this server"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "466"
'       466    ERR_YOUWILLBEBANNED

        Case "467"
'       467    ERR_KEYSET
'              "<channel> :Channel key already set"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        Case "471"
'       471    ERR_CHANNELISFULL
'              "<channel> :Cannot join channel (+l)"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        Case "472"
'       472    ERR_UNKNOWNMODE
'              "<char> :is unknown mode char to me for <channel>"
            Call SimpleParseOne(params, bInfo, AC_GREEN)
        Case "473"
'       473    ERR_INVITEONLYCHAN
'              "<channel> :Cannot join channel (+i)"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        Case "474"
'       474    ERR_BANNEDFROMCHAN
'              "<channel> :Cannot join channel (+b)"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        Case "475"
'       475    ERR_BADCHANNELKEY
'              "<channel> :Cannot join channel (+k)"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        Case "476"
'       476    ERR_BADCHANMASK
'              "<channel> :Bad Channel Mask"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        Case "477"
'       477    ERR_NOCHANMODES
'              "<channel> :Channel doesn't support modes"
            Call FancyParseOne(params, bInfo, AC_GREEN)
        Case "478"
'       478    ERR_BANLISTFULL
'              "<channel> <char> :Channel list is full"
            Call FancyParseTwo(params, bInfo, AC_GREEN)

        Case "481"
'       481    ERR_NOPRIVILEGES
'              ":Permission Denied- You're not an IRC operator"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "482"
'       482    ERR_CHANOPRIVSNEEDED
'              "<channel> :You're not channel operator"
            Call FancyParseOne(params, bInfo, AC_GREEN)

        Case "483"
'       483    ERR_CANTKILLSERVER
'              ":You can't kill a server!"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "484"
'       484    ERR_RESTRICTED
'              ":Your connection is restricted!"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "485"
'       485    ERR_UNIQOPPRIVSNEEDED
'              ":You're not the original channel operator"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "491"
'       491    ERR_NOOPERHOST
'              ":No O-lines for your host"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "501"
'       501    ERR_UMODEUNKNOWNFLAG
'              ":Unknown MODE flag"
            Call SimpleParseZero(params, bInfo, AC_GREEN)

        Case "502"
'       502    ERR_USERSDONTMATCH
'              ":Cannot change mode for other users"
            Call SimpleParseZero(params, bInfo, AC_GREEN)
        
        Case "550"
        'command = 550 | params = Oscar_Meyer Oscar_Meyer :is masked as c089ecd7.ne.e8e8b457.net.hmsk
            Call SimpleParseOne(params, bWhois, AC_GREEN)

        Case Else
            If bCommandDebug Then AddChatText "command = " & command & " | " & "params = " & params, AC_GREEN
    End Select
Exit Sub

Error_Handler:
    Call ERRORROUTINE("IRC_Command", Err, Error(Err))
    Resume Next
End Sub

Private Sub lChannel_Change(ByVal nID As Long, ByVal nX As Long, ByVal nY As Long)
    Dim i As Long
    Dim Target As String
    
    'clear the icons
    For i = 0 To lChannel.Count - 1
        lChannel.Data(1, i, 1) = &H0&
    Next i
    'put an icon where you clicked
    lChannel.Data(1, nY, 1) = &H60011F9
    
    lUser.Clear
    strChannel = lChannel.Data(0, nY)
    
    stCurrentChannel.Text = "Current Channel: " & strChannel
    stUsersInChannel.Text = "Users in Channel: " & strChannel
    
    If strChannel <> "" Then tcp.SendData "NAMES " & strChannel & vbCrLf
    
End Sub

Private Sub lColors_Change(ByVal nID As Long, ByVal nX As Long, ByVal nY As Long)
    Select Case nY
        Case 0
            edColorTest.textColor = AC_GREEN_RGB
        Case 1
            edColorTest.textColor = AC_WHITE_RGB
        Case 2
            edColorTest.textColor = AC_YELLOW_RGB
        Case 3
            edColorTest.textColor = AC_BROWN_RGB
        Case 4
            edColorTest.textColor = AC_PURPLE_RGB
        Case 5
            edColorTest.textColor = AC_RED_RGB
        Case 6
            edColorTest.textColor = AC_BLUE_RGB
        Case 7
            edColorTest.textColor = AC_PINK_RGB
        Case 8
            edColorTest.textColor = AC_LTPINK_RGB
        Case 9
            edColorTest.textColor = AC_GREY_RGB
        Case 10
            edColorTest.textColor = AC_CYAN_RGB
        Case 11
            edColorTest.textColor = AC_LTCYAN_RGB
        Case 12
            edColorTest.textColor = AC_LTRED_RGB
        Case Else
            edColorTest.textColor = &H0
    End Select
    '
    'WriteRegSettings
End Sub

Private Sub lOptions_Change(ByVal nID As Long, ByVal nX As Long, ByVal nY As Long)
On Error GoTo Error_Handler:
    Dim TheOption As String
    
    TheOption = lOptions.Data(0, nY)
    
    OptionView.Title = "dIRCal - " & TheOption
    
    LoadOptions
    
    Call MoveResize(stDefault, 0, 0, 0, 0)
    
    Call MoveResize(stServerOption, 0, 0, 0, 0)
    Call MoveResize(edServerOption, 0, 0, 0, 0)
    Call MoveResize(stServerPortOption, 0, 0, 0, 0)
    Call MoveResize(edServerPortOption, 0, 0, 0, 0)
    Call MoveResize(stNickOption, 0, 0, 0, 0)
    Call MoveResize(edNickOption, 0, 0, 0, 0)
    Call MoveResize(stChannelOption, 0, 0, 0, 0)
    Call MoveResize(edChannelOption, 0, 0, 0, 0)
    Call MoveResize(stPasswordOption, 0, 0, 0, 0)
    Call MoveResize(edPasswordOption, 0, 0, 0, 0)
    Call MoveResize(cbPerformOnConnectOption, 0, 0, 0, 0)
    Call MoveResize(cbAutoConnectOption, 0, 0, 0, 0)
    Call MoveResize(chkGreenText, 0, 0, 0, 0)
    
    Call MoveResize(cbAction, 0, 0, 0, 0)
    Call MoveResize(cbCTCP, 0, 0, 0, 0)
    Call MoveResize(cbHighlight, 0, 0, 0, 0)
    Call MoveResize(cbInfo, 0, 0, 0, 0)
    Call MoveResize(cbNoticeAuth, 0, 0, 0, 0)
    Call MoveResize(cbInvite, 0, 0, 0, 0)
    Call MoveResize(cbJoin, 0, 0, 0, 0)
    Call MoveResize(cbKick, 0, 0, 0, 0)
    Call MoveResize(cbMode, 0, 0, 0, 0)
    Call MoveResize(cbNick, 0, 0, 0, 0)
    Call MoveResize(cbActiveChannel, 0, 0, 0, 0)
    Call MoveResize(cbInactiveChannel, 0, 0, 0, 0)
    Call MoveResize(cbNotice, 0, 0, 0, 0)
    Call MoveResize(cbNotify, 0, 0, 0, 0)
    Call MoveResize(cbOther, 0, 0, 0, 0)
    Call MoveResize(cbOwn, 0, 0, 0, 0)
    Call MoveResize(cbPart, 0, 0, 0, 0)
    Call MoveResize(cbQuit, 0, 0, 0, 0)
    Call MoveResize(cbTopic, 0, 0, 0, 0)
    Call MoveResize(cbWallops, 0, 0, 0, 0)
    Call MoveResize(cbWhois, 0, 0, 0, 0)
    
'    Call MoveResize(chTextType, 0, 0, 0, 0)
'    Call MoveResize(lColors, 0, 0, 0, 0)
'    Call MoveResize(edColorTest, 0, 0, 0, 0)
    
    Call MoveResize(lPerform, 0, 0, 0, 0)
    Call MoveResize(edPerform, 0, 0, 0, 0)
    Call MoveResize(btnPerformOK, 0, 0, 0, 0)
    Call MoveResize(btnPerformCancel, 0, 0, 0, 0)
    
    'Call MoveResize(btnOptionsApply, 0, 0, 0, 0)
    Call MoveResize(btnOptionsOK, 60, 225, 60, 16)
    'Call MoveResize(btnOptionsCancel, 0, 0, 0, 0)

    Select Case TheOption
        Case "Connection"
            Call MoveResize(stServerOption, 5, 5, 170, 14)
            Call MoveResize(edServerOption, 5, 20, 170, 16)
            Call MoveResize(stServerPortOption, 5, 40, 170, 14)
            Call MoveResize(edServerPortOption, 5, 55, 170, 16)
            Call MoveResize(stNickOption, 5, 75, 170, 14)
            Call MoveResize(edNickOption, 5, 90, 170, 16)
            Call MoveResize(stChannelOption, 5, 110, 170, 14)
            Call MoveResize(edChannelOption, 5, 125, 170, 16)
            Call MoveResize(stPasswordOption, 5, 145, 170, 14)
            Call MoveResize(edPasswordOption, 5, 160, 170, 16)
            Call MoveResize(cbPerformOnConnectOption, 5, 180, 170, 13)
            Call MoveResize(cbAutoConnectOption, 5, 195, 170, 13)
            Call MoveResize(chkGreenText, 5, 210, 200, 19)
        Case "Filters"
            Call MoveResize(cbAction, 5, 0, 80, 13)
            Call MoveResize(cbCTCP, 5, 15, 80, 13)
            Call MoveResize(cbHighlight, 5, 30, 80, 13)
            Call MoveResize(cbInfo, 5, 45, 80, 13)
            Call MoveResize(cbNoticeAuth, 5, 60, 80, 13)
            Call MoveResize(cbInvite, 5, 75, 80, 13)
            Call MoveResize(cbJoin, 5, 90, 80, 13)
            Call MoveResize(cbKick, 5, 105, 80, 13)
            Call MoveResize(cbMode, 5, 120, 80, 13)
            Call MoveResize(cbNick, 5, 135, 80, 13)
            Call MoveResize(cbActiveChannel, 5, 150, 170, 13)
            Call MoveResize(cbInactiveChannel, 5, 165, 170, 13)
            Call MoveResize(cbNotice, 5, 180, 80, 13)
            Call MoveResize(cbNotify, 5, 195, 80, 13)
            Call MoveResize(cbOther, 5, 210, 80, 13)
            
            Call MoveResize(cbOwn, 95, 45, 80, 13)
            Call MoveResize(cbPart, 95, 60, 80, 13)
            Call MoveResize(cbQuit, 95, 75, 80, 13)
            Call MoveResize(cbTopic, 95, 90, 80, 13)
            Call MoveResize(cbWallops, 95, 105, 80, 13)
            Call MoveResize(cbWhois, 95, 120, 80, 13)
'        Case "Colors"
'            Call MoveResize(chTextType, 5, 5, 130, 18)
'            Call MoveResize(lColors, 139, 5, 36, 80)
'            Call MoveResize(edColorTest, 5, 160, 170, 16)
        Case "Perform"
            Call MoveResize(btnOptionsOK, 0, 0, 0, 0)
            
            Call MoveResize(lPerform, 0, 0, 255, 205)
            Call MoveResize(edPerform, 2, 207, 251, 16)
            Call MoveResize(btnPerformOK, 20, 225, 60, 16)
            Call MoveResize(btnPerformCancel, 100, 225, 60, 16)
'        Case "Notify"
'            Call MoveResize(stDefault, 45, 61, 90, 123)
'        Case "Logging"
'            Call MoveResize(stDefault, 45, 61, 90, 123)
'        Case "Ignore"
'            Call MoveResize(stDefault, 45, 61, 90, 123)
        Case Else
            Call MoveResize(stDefault, 45, 61, 90, 123)
    End Select
    
    OptionView.Deactivate
    OptionView.Activate
Exit Sub

Error_Handler:
    Call ERRORROUTINE("lOptions_Change", Err, Error(Err))
    Resume Next
End Sub

Private Sub lPerform_Change(ByVal nID As Long, ByVal nX As Long, ByVal nY As Long)
    Dim Sz As Long
    Dim i As Long
    Dim SwapStr As String
    Sz = lPerform.Count - 1
    
    Select Case (nX)
        Case 0  'Clicked the text area
            edPerform.Text = lPerform.Data(0, nY)
        Case 1  'Clicked up arrow
            If nY > 0 Then
                'Swap with above value
                SwapStr = lPerform.Data(0, nY - 1)
                lPerform.Data(0, nY - 1) = lPerform.Data(0, nY)
                lPerform.Data(0, nY) = SwapStr
                
                SavePerform
            End If
        Case 2  'Clicked down arrow
            If nY < Sz Then
                'Swap with below value
                SwapStr = lPerform.Data(0, nY + 1)
                lPerform.Data(0, nY + 1) = lPerform.Data(0, nY)
                lPerform.Data(0, nY) = SwapStr
                
                SavePerform
            End If
        Case 3  'Clicked red X
            lPerform.DeleteRow nY
            SavePerform
    End Select
    'If lPerform.Data(nX, nY) = "" Then edPerform.Text = "<new>"
    'linePerform = nY
    'set focus to edPerform if possible
End Sub

Private Sub lUser_Change(ByVal nID As Long, ByVal nX As Long, ByVal nY As Long)
    Dim i As Long
    Dim Target As String
    
    'clear the icons
    For i = 0 To lUser.Count - 1
        lUser.Data(1, i, 1) = &H0&
    Next i
    
    'put an icon where you clicked
    lUser.Data(1, nY, 1) = &H60011F9
    
    'if ya click on the icon, initiate text to that person
    If nX = 1 Then
        If left(lUser.Data(0, nY), 1) = "@" _
        Or left(lUser.Data(0, nY), 1) = "+" _
        Then
            Target = Mid(lUser.Data(0, nY), 2)
        Else
            Target = lUser.Data(0, nY)
        End If
        SendKeys ("{ENTER}/i " & Target & ": ")
        'clear it so it doesn't look so silly
        lUser.Data(1, nY, 1) = &H0&
    End If
    '512, 513, 514
End Sub

Private Sub NetEcho_EchoServer(ByVal pMsg2 As IMessage2)
On Error GoTo merror

If pMsg2.Type = &HF746& Then
    On Error Resume Next
    Set VCS = PluginSite.Plugin("VirindiChatSystem.PluginCore")
    On Error GoTo merror
    SetMainIcon STATE_DISCONNECTED
End If
'GoTo done
'Dim pMsg As DecalNet.IMessage2
'Set pMsg = pMsg2.Begin.Message
'    Select Case pMsg.Type
'        Case &HF748&
'            Select Case pMsg.Member("object")
'                Case MyGUID
'                    MyCoords = "hi"
'            End Select
'        Case &HF74C&
'            If pMsg.Member("character") = MyGUID And pMsg.Member("animationID2") = &H11B Then QuitIRC ("")
'        Case &HF7B0&
'            MyGUID = pMsg.Member("character")
'            Select Case pMsg.Member("event")
'                Case &H13&
'                    Dim i As Long 'Counter
'                    Dim pStrings As IMessageMember
'
'                    Set pStrings = pMsg.Member("strings")
'
'                    For i = 0 To pStrings.Count - 1
'                        If pStrings.Member(i).Member("key") = 1 Then
'                            WhoAmI = pStrings.Member(i).Member("string")
'                        End If
'                    Next i
'            End Select
'    End Select
GoTo done
merror:
MsgBox "eep: " + Err.Description
done:
End Sub

Private Sub OptionView_Activate()
    '
End Sub

Private Sub tcp_Connect()
On Error GoTo Error_Handler:
    SetStatus "Connecting..."
    btnConnect.Text = "Cancel"
    'USER <user> <mode> <unused> <realname>
    tcp.SendData "USER dircal@pinko.acdev.org " & tcp.LocalHostName & " " & tcp.RemoteHost & " :dIRCal v" & App.Major & "." & App.Minor & "." & App.Revision & vbCrLf
    tcp.SendData "NICK " & strNickName & vbCrLf
Exit Sub

Error_Handler:
    Call ERRORROUTINE("tcp_Connect", Err, Error(Err))
    Resume Next
End Sub

Private Sub tcp_DataArrival(ByVal bytesTotal As Long)
On Error GoTo Error_Handler:
    Dim strData As String
    
    tcp.GetData strData
    WinsockDataToLines strData
    
Exit Sub

Error_Handler:
    Call ERRORROUTINE("tcp_DataArrival", Err, Error(Err))
    Resume Next
End Sub


                                        ' ' G E N E R A L ' '

                                        ' ' G E N E R A L ' '

                                        ' ' G E N E R A L ' '

Private Sub OptionInit()
On Error GoTo Error_Handler:
    Dim nRow As Integer
    
    Set lOptions = OptionView.Control("lOptions")
    nRow = lOptions.AddRow
    lOptions.Data(0, nRow) = "Connection"
    nRow = lOptions.AddRow
    lOptions.Data(0, nRow) = "Filters"
'    nRow = lOptions.AddRow
'    lOptions.Data(0, nRow) = "Colors"
    nRow = lOptions.AddRow
    lOptions.Data(0, nRow) = "Perform"
'    nRow = lOptions.AddRow
'    lOptions.Data(0, nRow) = "Notify"
'    nRow = lOptions.AddRow
'    lOptions.Data(0, nRow) = "Logging"
'    nRow = lOptions.AddRow
'    lOptions.Data(0, nRow) = "Ignore"
    
'        Case "Connection"
            Set stDefault = OptionView.Control("stDefault")
            
            Set stServerOption = OptionView.Control("stServerOption")
            Set edServerOption = OptionView.Control("edServerOption")
                edServerOption.Text = Reg.Value("Server")
            Set stServerPortOption = OptionView.Control("stServerPortOption")
            Set edServerPortOption = OptionView.Control("edServerPortOption")
                edServerPortOption.Text = Reg.Value("ServerPort")
            Set stNickOption = OptionView.Control("stNickOption")
            Set edNickOption = OptionView.Control("edNickOption")
                edNickOption.Text = Reg.Value("Nickname")
            Set stChannelOption = OptionView.Control("stChannelOption")
            Set edChannelOption = OptionView.Control("edChannelOption")
                edChannelOption.Text = Reg.Value("Channel")
            Set stPasswordOption = OptionView.Control("stPasswordOption")
            Set edPasswordOption = OptionView.Control("edPasswordOption")
                edPasswordOption.Text = Crypt(Reg.Value("Password"))
            Set cbPerformOnConnectOption = OptionView.Control("cbPerformOnConnectOption")
                cbPerformOnConnectOption.Checked = Reg.Value("PerformOnConnect")
            Set cbAutoConnectOption = OptionView.Control("cbAutoConnectOption")
                cbAutoConnectOption.Checked = Reg.Value("AutoConnect")
            Set chkGreenText = OptionView.Control("chkGreenText")
                chkGreenText.Checked = bGreenText
'        Case "Filters"
            Set cbAction = OptionView.Control("cbAction")
            Set cbCTCP = OptionView.Control("cbCTCP")
            Set cbHighlight = OptionView.Control("cbHighlight")
            Set cbInfo = OptionView.Control("cbInfo")
            Set cbNoticeAuth = OptionView.Control("cbNoticeAuth")
            Set cbInvite = OptionView.Control("cbInvite")
            Set cbJoin = OptionView.Control("cbJoin")
            Set cbKick = OptionView.Control("cbKick")
            Set cbMode = OptionView.Control("cbMode")
            Set cbNick = OptionView.Control("cbNick")
            Set cbActiveChannel = OptionView.Control("cbActiveChannel")
            Set cbInactiveChannel = OptionView.Control("cbInactiveChannel")
            Set cbNotice = OptionView.Control("cbNotice")
            Set cbNotify = OptionView.Control("cbNotify")
            Set cbOther = OptionView.Control("cbOther")
            Set cbOwn = OptionView.Control("cbOwn")
            Set cbPart = OptionView.Control("cbPart")
            Set cbQuit = OptionView.Control("cbQuit")
            Set cbTopic = OptionView.Control("cbTopic")
            Set cbWallops = OptionView.Control("cbWallops")
            Set cbWhois = OptionView.Control("cbWhois")
            
            cbAction.Checked = bAction
            cbCTCP.Checked = bCTCP
            cbHighlight.Checked = bHighlight
            cbInfo.Checked = bInfo
            cbNoticeAuth.Checked = bNoticeAuth
            cbInvite.Checked = bInvite
            cbJoin.Checked = bJoin
            cbKick.Checked = bKick
            cbMode.Checked = bMode
            cbNick.Checked = bNick
            cbActiveChannel.Checked = bActiveChannel
            cbInactiveChannel.Checked = bInactiveChannel
            cbNotice.Checked = bNotice
            cbNotify.Checked = bNotify
            cbOther.Checked = bOther
            cbOwn.Checked = bOwn
            cbPart.Checked = bPart
            cbQuit.Checked = bQuit
            cbTopic.Checked = bTopic
            cbWallops.Checked = bWallops
            cbWhois.Checked = bWhois

'        Case "Colors"
'            Set chTextType = OptionView.Control("chTextType")
'            Set lColors = OptionView.Control("lColors")
'            Set edColorTest = OptionView.Control("edColorTest")
'
'            chTextType.AddChoice "Action text"
'            chTextType.AddChoice "CTCP text"
'            chTextType.AddChoice "Highlight text"
'            chTextType.AddChoice "Info text"
'            chTextType.AddChoice "Connect text"
'            chTextType.AddChoice "Invite text"
'            chTextType.AddChoice "Join text"
'            chTextType.AddChoice "Kick text"
'            chTextType.AddChoice "Mode text"
'            chTextType.AddChoice "Nick text"
'            chTextType.AddChoice "Active channel text"
'            chTextType.AddChoice "Inactive channel text"
'            chTextType.AddChoice "Notice text"
'            chTextType.AddChoice "Notify text"
'            chTextType.AddChoice "Other text"
'            chTextType.AddChoice "Own text"
'            chTextType.AddChoice "Part text"
'            chTextType.AddChoice "Quit text"
'            chTextType.AddChoice "Topic text"
'            chTextType.AddChoice "Wallops text"
'            chTextType.AddChoice "Whois text"
'
'            Call lColors.AddRow
'            lColors.Data(0, 0, 0) = App.hInstance
'            lColors.Data(0, 0, 1) = 101
'            lColors.Data(1, 0) = AC_GREEN
'            Call lColors.AddRow
'            lColors.Data(0, 1, 0) = App.hInstance
'            lColors.Data(0, 1, 1) = 102
'            lColors.Data(1, 1) = AC_WHITE
'            Call lColors.AddRow
'            lColors.Data(0, 2, 0) = App.hInstance
'            lColors.Data(0, 2, 1) = 103
'            lColors.Data(1, 2) = AC_YELLOW
'            Call lColors.AddRow
'            lColors.Data(0, 3, 0) = App.hInstance
'            lColors.Data(0, 3, 1) = 104
'            lColors.Data(1, 3) = AC_BROWN
'            Call lColors.AddRow
'            lColors.Data(0, 4, 0) = App.hInstance
'            lColors.Data(0, 4, 1) = 105
'            lColors.Data(1, 4) = AC_PURPLE
'            Call lColors.AddRow
'            lColors.Data(0, 5, 0) = App.hInstance
'            lColors.Data(0, 5, 1) = 106
'            lColors.Data(1, 5) = AC_RED
'            Call lColors.AddRow
'            lColors.Data(0, 6, 0) = App.hInstance
'            lColors.Data(0, 6, 1) = 107
'            lColors.Data(1, 6) = AC_BLUE
'            Call lColors.AddRow
'            lColors.Data(0, 7, 0) = App.hInstance
'            lColors.Data(0, 7, 1) = 108
'            lColors.Data(1, 7) = AC_PINK
'            Call lColors.AddRow
'            lColors.Data(0, 8, 0) = App.hInstance
'            lColors.Data(0, 8, 1) = 109
'            lColors.Data(1, 8) = AC_LTPINK
'            Call lColors.AddRow
'            lColors.Data(0, 9, 0) = App.hInstance
'            lColors.Data(0, 9, 1) = 110
'            lColors.Data(1, 9) = AC_GREY
'            Call lColors.AddRow
'            lColors.Data(0, 10, 0) = App.hInstance
'            lColors.Data(0, 10, 1) = 111
'            lColors.Data(1, 10) = AC_CYAN
'            Call lColors.AddRow
'            lColors.Data(0, 11, 0) = App.hInstance
'            lColors.Data(0, 11, 1) = 112
'            lColors.Data(1, 11) = AC_LTCYAN
'            Call lColors.AddRow
'            lColors.Data(0, 12, 0) = App.hInstance
'            lColors.Data(0, 12, 1) = 113
'            lColors.Data(1, 12) = AC_LTRED
        
'        Case "Perform"
            Set lPerform = OptionView.Control("lPerform")
            Set edPerform = OptionView.Control("edPerform")
            Set btnPerformOK = OptionView.Control("btnPerformOK")
            Set btnPerformCancel = OptionView.Control("btnPerformCancel")
            Call LoadPerform
            Call btnPerformCancel_Accepted(0)
    
'        Case "Notify"
            '
'        Case "Logging"
            '
'        Case "Ignore"
            '
    
'    Set btnOptionsApply = OptionView.Control("btnOptionsApply")
    Set btnOptionsOK = OptionView.Control("btnOptionsOK")
'    Set btnOptionsCancel = OptionView.Control("btnOptionsCancel")
    
    LoadOptions
Exit Sub

Error_Handler:
    Call ERRORROUTINE("OptionInit", Err, Error(Err))
    Resume Next
End Sub

Private Sub SetStatus(strMsg As String)
On Error GoTo Error_Handler:
    stStatus.Text = "Status: " & strMsg
    AddChatText "Status: " & strMsg, AC_BLUE
Exit Sub

Error_Handler:
    Call ERRORROUTINE("SetStatus", Err, Error(Err))
    Resume Next
End Sub

Private Function StripMIRCCodes(strIn As String) As String
On Error GoTo Error_Handler:
    Dim i As Integer
    Dim x As Integer
    Dim strTemp
    
    Const Ctrl_B As Byte = 2
    Const Ctrl_K As Byte = 3
    Const Ctrl_O As Byte = 15
    Const Ctrl_R As Byte = 22
    Const Ctrl_U As Byte = 31

    strIn = Replace(strIn, Chr(Ctrl_B), "")
    strIn = Replace(strIn, Chr(Ctrl_U), "")
    strIn = Replace(strIn, Chr(Ctrl_R), "")
    strIn = Replace(strIn, Chr(Ctrl_O), "")

    i = InStr(strIn, Chr(Ctrl_K))
    While (i > 0)
        strTemp = ""
        x = i + 1
        While (IsNumeric(Mid(strIn, x, 1)))
            x = x + 1
        Wend
        If (i > 1) Then
            strTemp = left(strIn, i - 1)
        End If
        strTemp = strTemp & Mid(strIn, x)
        strIn = strTemp
        i = InStr(strIn, Chr(Ctrl_K))
    Wend
    
    StripMIRCCodes = strIn
Exit Function

Error_Handler:
    Call ERRORROUTINE("StripMIRCCodes", Err, Error(Err))
    Resume Next
End Function

Private Function GetWord(ByRef strSource As String) As String
On Error GoTo Error_Handler:
    Dim i As Integer
    Dim strTemp As String

    If (Len(strSource) > 1) Then
        i = InStr(strSource, Chr(32))
        If (i > 0) Then
            strTemp = Trim(Mid(strSource, 1, i - 1))
            strSource = Trim(Mid(strSource, i + 1))
        End If
    Else
        strTemp = ""
    End If
    GetWord = strTemp
Exit Function

Error_Handler:
    Call ERRORROUTINE("GetWord", Err, Error(Err))
    Resume Next
End Function

Private Function FirstWord(ByRef strSource As String) As String
On Error GoTo Error_Handler:
    Dim i As Integer
    Dim strTemp As String

    For i = 1 To Len(strSource)
        If Mid(strSource, i, 1) = Chr(32) Then
            FirstWord = Trim(Mid(strSource, 1, i - 1))
            strSource = Trim(Mid(strSource, i + 1))
            Exit For
        End If
    Next i
    
    If FirstWord = "" Then
        FirstWord = strSource
        strSource = ""
    End If
Exit Function

Error_Handler:
    Call ERRORROUTINE("FirstWord", Err, Error(Err))
    Resume Next
End Function

Private Function FirstWordNC(ByVal strSource As String) As String
On Error GoTo Error_Handler:
    Dim i As Integer
    Dim strTemp As String

    For i = 1 To Len(strSource)
        If Mid(strSource, i, 1) = Chr(32) Then
            FirstWordNC = Trim(Mid(strSource, 1, i - 1))
            'strSource = Trim(Mid(strSource, i + 1))
            Exit For
        End If
    Next i
    
    If FirstWordNC = "" Then
        FirstWordNC = strSource
        'strSource = ""
    End If
Exit Function

Error_Handler:
    Call ERRORROUTINE("FirstWordNC", Err, Error(Err))
    Resume Next
End Function

Private Sub WinsockDataToLines(strLine As String)
On Error GoTo Error_Handler:
    Dim i As Integer
    Dim oneline As String
    Static RestLine As String
    
    strLine = RestLine & strLine
    
    'for each byte in the bigass string
    For i = 1 To Len(strLine)
        'if the two byte sequence is a carriage return & line feed then
        If Mid(strLine, i, 2) = Chr(13) & Chr(10) Then
            'set oneline to byte 1 to previous byte (the one before LFCR)
            oneline = Mid(strLine, 1, i - 1)
            'call processOneLine on the line
            If Len(oneline) > 0 Then Call processOneLine(oneline)
            'set strline to begin at the next byte
            strLine = Mid(strLine, i + 2)
            'set the byte marker back to 1
            i = 1
        End If
    ' do it again
    Next i
    RestLine = strLine
    
Exit Sub

Error_Handler:
    Call ERRORROUTINE("WinsockDataToLines", Err, Error(Err))
    Resume Next
End Sub

Private Sub processOneLine(strOneLine As String)
On Error GoTo Error_Handler:

    If bMessageDebug = True Then ShowDebug "OneLine>" & strOneLine
    
    Dim i As Integer
    Dim wordNum As Integer
    Dim IRCprefix As String
    Dim IRCcommand As String
    Dim IRCparams As String
    Dim thisWord As String
    
    IRCprefix = ""
    IRCcommand = ""
    IRCparams = ""
    thisWord = ""
    
    'check for carriage return or linefeed in front of line and delete
    If Mid(strOneLine, 1, 1) = Chr(13) _
    Or Mid(strOneLine, 1, 1) = Chr(10) _
    Then strOneLine = Mid(strOneLine, 2)


    For i = 1 To Len(strOneLine)
        If Mid(strOneLine, i, 1) = Chr(32) Then
            thisWord = Trim(Mid(strOneLine, 1, i - 1))
            
            wordNum = wordNum + 1

            If (wordNum = 1) And (Not (left(thisWord, 1) = ":")) Then wordNum = 2
            
            If wordNum = 1 Then
                IRCprefix = Trim(thisWord)
                'chop off the leading ":"
                If left(IRCprefix, 1) = ":" Then IRCprefix = Mid(IRCprefix, 2)
            ElseIf wordNum = 2 Then
                IRCcommand = Trim(thisWord)
            ElseIf wordNum > 2 Then
                Exit For
            End If
            
            strOneLine = Mid(strOneLine, i + 1)
            
            i = 1
        End If
    Next i
    
    IRCparams = Trim(strOneLine)
    
    'call the command sub, passing prefix, command, and the rest
    Call IRC_Command(IRCprefix, IRCcommand, IRCparams)
    
Exit Sub

Error_Handler:
    Call ERRORROUTINE("processOneLine", Err, Error(Err))
    Resume Next
End Sub

Private Function AddChatText(strText As Variant, Optional textColor As Long = 1, Optional lineFeed As Boolean = True)
On Error GoTo Error_Handler:
    Dim strTemp As String
    strTemp = strText
    strTemp = StripMIRCCodes(strTemp)
    strTemp = "[dIRCal] " & strTemp
    'If lineFeed Then strTemp = strTemp + vbLf
    'PluginSite.RawWriteToChatWindow strTemp, textColor
    
    If VCS Is Nothing Then
        PluginSite2.Hooks.AddChatText strTemp, textColor, 1
    Else
        VCS.DisplayMessage strTemp & " ", textColor, 1
    End If


Exit Function

Error_Handler:
    Call ERRORROUTINE("AddChatChat", Err, Error(Err))
    Resume Next
End Function

Private Function Crypt(ByVal strCrypt As String) As String
On Error GoTo Error_Handler:
    Dim iChar As Integer, TempStr As String
    
    If strCrypt <> "" Then
        strCrypt = StrReverse(strCrypt)
        For iChar = 1 To Len(strCrypt)
            TempStr = String(2 - Len(Hex(Asc(Mid(strCrypt, iChar, 1)))), "0") & Hex(Asc(Mid(strCrypt, iChar, 1)))
            TempStr = Right(TempStr, 1) & left(TempStr, 1)
            TempStr = Chr(Val(CLng("&H" & TempStr)))
            Crypt = Crypt & TempStr
        Next iChar
    Else
        Crypt = ""
    End If
Exit Function

Error_Handler:
    Call ERRORROUTINE("Crypt", Err, Error(Err))
    Resume Next
End Function

Private Sub MoveResize(ByRef Control As Object, left As Long, top As Long, width As Long, height As Long)
    Dim ControlPositionRect As tagRECT
    Dim tmpLayer As ILayer
    
    Set tmpLayer = Control
    ControlPositionRect.left = left
    ControlPositionRect.top = top
    ControlPositionRect.Right = left + width
    ControlPositionRect.bottom = top + height
    
    tmpLayer.Position = ControlPositionRect
End Sub

Private Sub LoadOptions()
On Error GoTo Error_Handler:
    With Reg
        If .Value("Server") <> "" Then edServer.Text = .Value("Server")
        If .Value("ServerPort") <> "" Then edServerPort.Text = .Value("ServerPort")
        If .Value("Nickname") <> "" Then edNick.Text = .Value("Nickname")
        If .Value("Channel") <> "" Then edChannel.Text = .Value("Channel")
        If .Value("Password") <> "" Then edPassword.Text = Crypt(Reg.Value("Password"))
        If .Value("AutoConnect") = "True" Then bAutoConnect = True
        If .Value("CommandDebug") = "True" Then bCommandDebug = True
        If .Value("MessageDebug") = "True" Then bMessageDebug = True
        
        bGreenText = True
        If .Value("GreenText") = "False" Then bGreenText = False
        
        bAction = CBool(.Value("Filters") And &H1&)
        bCTCP = CBool(.Value("Filters") And &H2&)
        bHighlight = CBool(.Value("Filters") And &H4&)
        bInfo = CBool(.Value("Filters") And &H8&)
        bNoticeAuth = CBool(.Value("Filters") And &H10&)
        bInvite = CBool(.Value("Filters") And &H20&)
        bJoin = CBool(.Value("Filters") And &H40&)
        bKick = CBool(.Value("Filters") And &H80&)
        bMode = CBool(.Value("Filters") And &H100&)
        bNick = CBool(.Value("Filters") And &H200&)
        bActiveChannel = CBool(.Value("Filters") And &H400&)
        bInactiveChannel = CBool(.Value("Filters") And &H800&)
        bNotice = CBool(.Value("Filters") And &H1000&)
        bNotify = CBool(.Value("Filters") And &H2000&)
        bOther = CBool(.Value("Filters") And &H4000&)
        bOwn = CBool(.Value("Filters") And &H8000&)
        bPart = CBool(.Value("Filters") And &H10000)
        bQuit = CBool(.Value("Filters") And &H20000)
        bTopic = CBool(.Value("Filters") And &H40000)
        bWallops = CBool(.Value("Filters") And &H80000)
        bWhois = CBool(.Value("Filters") And &H100000)
        
        'chkGreenText.Checked = bGreenText
    End With
Exit Sub

Error_Handler:
    Call ERRORROUTINE("LoadOptions", Err, Error(Err))
    Resume Next
End Sub

Private Sub LoadRegSettings()
On Error GoTo Error_Handler:
    With Reg
        If .Value("Server") <> "" Then edServer.Text = .Value("Server")
        If .Value("ServerPort") <> "" Then edServerPort.Text = .Value("ServerPort")
        If .Value("Nickname") <> "" Then edNick.Text = .Value("Nickname")
        If .Value("Channel") <> "" Then edChannel.Text = .Value("Channel")
        If .Value("Password") <> "" Then edPassword.Text = Crypt(Reg.Value("Password"))
        If .Value("AutoConnect") = "True" Then bAutoConnect = True
        If .Value("CommandDebug") = "True" Then bCommandDebug = True
        If .Value("MessageDebug") = "True" Then bMessageDebug = True
        If .Value("GreenText") = "True" Then bGreenText = True
    End With
Exit Sub

Error_Handler:
    Call ERRORROUTINE("LoadRegSettings", Err, Error(Err))
    Resume Next
End Sub

Private Sub PerformOnConnect()
On Error GoTo Error_Handler:
    Dim intFileNumber As Integer
    Dim tmpPerformINI As String
    Dim HasPassword As Long
    
    intFileNumber = FreeFile
    
    Open Reg.Value & "\perform.ini" For Input As #intFileNumber
    While Not EOF(intFileNumber)
        Line Input #intFileNumber, tmpPerformINI
        
        HasPassword = InStr(tmpPerformINI, "%PASSWORD%")
        If HasPassword > 0 Then
            Dim LeftSide As String
            Dim RightSide As String
            
            LeftSide = left$(tmpPerformINI, HasPassword - 1)
            RightSide = Right$(tmpPerformINI, (Len(tmpPerformINI) - HasPassword - 9))
            
            tmpPerformINI = LeftSide & Crypt(Reg.Value("Password")) & RightSide
        End If
        
        'Private Sub eACHooks_ChatParserIntercept(ByVal bstrMsg As String, eat As Boolean)
        
        'If tmpPerformINI <> "" Then Call IPluginSink_ChatText(tmpPerformINI)
        If tmpPerformINI <> "" Then
            Dim tB As Boolean
            Call eACHooks_ChatParserIntercept(tmpPerformINI, tB)
        End If
    Wend
    Close #intFileNumber
Exit Sub

Error_Handler:
    Call ERRORROUTINE("PerformOnConnect", Err, Error(Err))
    Resume Next
End Sub

Private Sub WriteRegSettings()
On Error GoTo Error_Handler:
    With Reg
        .Value("Server") = notNULL(edServer.Text)
        .Value("ServerPort") = notNULL(edServerPort.Text)
        .Value("Nickname") = notNULL(edNick.Text)
        .Value("Channel") = notNULL(edChannel.Text)
        .Value("Password") = Crypt(notNULL(edPassword.Text))
        .Value("CommandDebug") = bCommandDebug
        .Value("MessageDebug") = bMessageDebug
        .Value("GreenText") = bGreenText
    End With
Exit Sub

Error_Handler:
    Call ERRORROUTINE("WriteRegSettings", Err, Error(Err))
    Resume Next
End Sub

Private Sub ShowDebug(strDebug As String)
    AddChatText strDebug, AC_PINK
End Sub

Private Function ERRORROUTINE(strRoutine As String, intErr As Long, strMessageType As String)
    Dim strErrorMessage  As String
    Dim intFileNumber    As Integer
    Dim intReturn        As Integer

    On Error Resume Next
    '***Get the error message from the error return code***
    strErrorMessage = Error$(intErr)
    '***Save the error to the logfile***
    intFileNumber = FreeFile
    'If DEBUG_VERSION Then
    Open (App.Path & "\dIRCal.log") For Append As #intFileNumber
            Print #intFileNumber, Now & ": v" & App.Major & "." & App.Minor & "." & App.Revision & "  " & "Error in Routine: " & strRoutine & "    Error Number: " & Str$(intErr) & "   Error Message: " & strMessageType
    Close #intFileNumber
    'End If
End Function

Private Sub dIRCalHelp()
    AddChatText "dIRCal Version " & App.Major & "." & App.Minor & "." & App.Revision, AC_YELLOW
    AddChatText "  http://dircal.sourceforge.net", AC_YELLOW
    AddChatText "Available commands:", AC_GREEN
    AddChatText "  Note: You can subsitute a @ for the /", AC_GREEN
    AddChatText " Connection Commands:", AC_CYAN
    AddChatText "  /connect -- Connects to the IRC server.", AC_GREEN
    AddChatText "  /disconnect -- Disconnects from the IRC server.", AC_GREEN
    AddChatText " Chat Toggles:", AC_CYAN
    AddChatText "  /ac -- Toggles chat to AC.", AC_GREEN
    AddChatText "  /irc -- Toggles chat to IRC.", AC_GREEN
    AddChatText " Communication:", AC_CYAN
    AddChatText "  /i -- /i <message> -- Sends a message to the current IRC channel.", AC_GREEN
    AddChatText "  /ir, /irp, ireply -- /ir <message> -- Sends a private message to the last user to send you a private message.", AC_GREEN
    AddChatText "  /irt, /iretell -- /irt <message> -- Sends a private message to the last user you sent a private message to.", AC_GREEN
    AddChatText "  /ie, /ime, /iem, /iemote -- /ie <action> -- Sends an action/emote to the channel.", AC_GREEN
    AddChatText "  /msg, /it, /itell -- /msg <NickName> <message> -- Sends a private message to the specified user.", AC_GREEN
    AddChatText " IRC Commands:", AC_CYAN
    AddChatText "  /cleartopic -- Clears the current channel topic, if permitted.", AC_GREEN
    AddChatText "  /ctcp -- /ctcp <user | #channel> <command> -- Sends a CTCP command to a user or channel.  Valid commands are PING, TIME, VERSION", AC_GREEN
    AddChatText "  /invite -- /invite <user> <#channel> -- Invites <user> to join <#channel>.", AC_GREEN
    AddChatText "  /join -- /join <#chan[,#chan2]> [chankey,chankey2] -- Joins the specified IRC channel(s) with optional channel key/password.", AC_GREEN
    AddChatText "  /kick -- /kick <#channel> <user> [reason] -- Kicks <user> from <#channel>.", AC_GREEN
    AddChatText "  /mode -- /mode <#channel | NickName> <+/- mode(s)> [User(s)] -- Sets user or channel modes.  For more info see the irc_intro.txt file.", AC_GREEN
    AddChatText "  /nick -- /nick <NickName> -- Changes your current NicNake to <NickName>.", AC_GREEN
    AddChatText "  /notice -- /notice <user | #channel> <message> -- Similar to a /msg, this sends a notice to a user or channel.", AC_GREEN
    AddChatText "  /part -- /part [#channel[,#chan2] [reason]] -- Leaves the current IRC channel, or [#channel[,#chan2] [reason]] if specified.", AC_GREEN
    AddChatText "  /quit -- /quit [reason] -- Quits IRC and leaves a [reason] if specified.", AC_GREEN
    AddChatText "  /topic -- /topic [newtopic] -- Shows the current channel's topic, or sets the topic to [newtopic] if specified.", AC_GREEN
    AddChatText "  /who -- /who <#channel> -- Returns info on users in a channel.", AC_GREEN
    AddChatText "  /whois -- /whois <NickName> -- Returns info on the specified nickname.", AC_GREEN
    AddChatText "  /whowas -- /whowas <NickName> -- Returns info on the last few users of the specified nickname.", AC_GREEN
End Sub

Private Sub SimpleParseZero(ByRef params As String, ByVal bFilterType As Boolean, lColor As Long)
    Dim param0 As String
    param0 = FirstWord(params)
    
    If Not bFilterType Then
        If left(params, 1) = ":" Then
            AddChatText Mid(params, 2), lColor
        Else
            AddChatText params, lColor
        End If
    End If
End Sub

Private Sub SimpleParseOne(ByRef params As String, ByVal bFilterType As Boolean, lColor As Long)
    Dim param0 As String
    param0 = FirstWord(params)
    
    If Not bFilterType Then AddChatText FirstWord(params) & " " & Mid(params, 2), lColor
End Sub

Private Sub SimpleParseTwo(ByRef params As String, ByVal bFilterType As Boolean, lColor As Long)
    Dim param0 As String
    param0 = FirstWord(params)
    
    If Not bFilterType Then AddChatText FirstWord(params) & " " & FirstWord(params) & " " & Mid(params, 2), lColor
End Sub

Private Sub FancyParseOne(ByRef params As String, ByVal bFilterType As Boolean, lColor As Long)
    Dim param0 As String
    param0 = FirstWord(params)
    
    If Not bFilterType Then AddChatText FirstWord(params) & " :: " & Mid(params, 2), lColor
End Sub

Private Sub FancyParseTwo(ByRef params As String, ByVal bFilterType As Boolean, lColor As Long)
    Dim param0 As String
    param0 = FirstWord(params)
    
    If Not bFilterType Then AddChatText FirstWord(params) & " " & FirstWord(params) & " :: " & Mid(params, 2), lColor
End Sub

Private Sub QuitIRC(strReason As String)
    If strReason = "" Then strReason = "dIRCal ¤ v" & App.Major & "." & App.Minor & "." & App.Revision
    
    If (btnConnect.Text = "Disconnect") Then
        If (tcp.state = sckConnected) Then
            tcp.SendData "QUIT :" & strReason & vbCrLf
        Else
            AddChatText "Status: Unable to disconnect properly.  Killing connection.", AC_BLUE, True
            tcp.Close
            If tcp.state = sckClosed Then
                strChannel = ""
                strNickName = ""
                strServer = ""
                strServerPort = ""
                stCurrentChannel.Text = "Current Channel:"
                stUsersInChannel.Text = "Users in Channel:"
                lChannel.Clear
                lUser.Clear
                btnConnect.Text = "Connect"
                SetStatus "Disconnected!"
            End If
        End If
    End If
    'MainView.SetIcon &H600137B
    SetMainIcon STATE_DISCONNECTED
End Sub

Private Sub SetMainIcon(state As eState)
    Dim ico As Long
    ico = 115 + (state * 2)
    If Not VCS Is Nothing Then ico = ico + 1
    MainView.SetIcon ico, App.hInstance
End Sub
