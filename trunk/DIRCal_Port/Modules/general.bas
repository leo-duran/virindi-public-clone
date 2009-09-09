Attribute VB_Name = "general"
Public Const WM_CHAR = &H102
Public Const WM_KEYDOWN = &H100
Public Const WM_KEYUP = &H101
Public Const WM_LBUTTONDOWN = &H201
Public Const WM_RBUTTONDOWN = &H204

Public Const AC_GREEN = &H1
Public Const AC_WHITE = &H2
Public Const AC_YELLOW = &H3
Public Const AC_BROWN = &H4
Public Const AC_PURPLE = &H5
Public Const AC_RED = &H6
Public Const AC_BLUE = &H7
Public Const AC_PINK = &H8
Public Const AC_LTPINK = &H9
Public Const AC_GREY = &HC
Public Const AC_CYAN = &HD
Public Const AC_LTCYAN = &HE
Public Const AC_LTRED = &H16

Public Const AC_GREEN_RGB = &H7BFF84
Public Const AC_WHITE_RGB = &HFFFFFF
Public Const AC_YELLOW_RGB = &H39FFFF
Public Const AC_BROWN_RGB = &H63D7D6
Public Const AC_PURPLE_RGB = &HFF79FF
Public Const AC_RED_RGB = &H3938FF
Public Const AC_BLUE_RGB = &HFFBE39
Public Const AC_PINK_RGB = &H9496FF
Public Const AC_LTPINK_RGB = &HA5A6DE
Public Const AC_GREY_RGB = &HCED7D6
Public Const AC_CYAN_RGB = &HDEDF39
Public Const AC_LTCYAN_RGB = &HF7DFB5
Public Const AC_LTRED_RGB = &H7371F7

Public Const textHighlightColor = AC_YELLOW

Public Function notNULL(f As String) As String
If Len(f) > 0 Then notNULL = f Else notNULL = ""
End Function
