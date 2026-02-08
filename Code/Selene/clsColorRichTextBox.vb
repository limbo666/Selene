'Name             : clsColorRichTextBox
'Description      : Class for coloring and format syntax in System.Windows.Forms.RichTextBox
'Publish Date     : 24.07.2003
'Legal Copyright  : © Rostislav V. Konnov, 2003
'Software Licence : Freeware
'Author           : Rostislav V. Konnov
'E-mail           : bigroko@mail.ru
'ICQ              : 38880828

Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

Namespace ColorRichTextBox

    Namespace WIN32API


        Public Module WIN32API

            Public Declare Function LockWindowUpdate Lib "user32" _
                (ByVal hWndLock As Integer) As Integer
            Public Declare Function GetDesktopWindow Lib "user32" _
                Alias "GetDesktopWindow" () As Integer
            Public Declare Function SendMessage Lib "user32" _
                Alias "SendMessageA" _
                (ByVal hwnd As IntPtr, ByVal wMsg As Integer,
                ByVal wParam As Integer, ByVal lParam As Integer) As Integer
            Public Declare Function SendMessage Lib "user32" _
                Alias "SendMessageA" _
                (ByVal hwnd As IntPtr, ByVal wMsg As Integer,
                ByVal wParam As Integer, ByRef lParam As CHARFORMAT) As Integer
            Public Declare Function SendMessage Lib "user32" _
                Alias "SendMessageA" _
                (ByVal hwnd As IntPtr, ByVal wMsg As Integer,
                ByVal wParam As Integer, ByVal lParam As String) As Integer

            Public Const EM_LINEFROMCHAR As Int32 = &HC9
            Public Const EM_LINEINDEX As Int32 = &HBB
            Public Const EM_LINELENGTH As Int32 = &HC1
            Public Const EM_REPLACESEL As Int32 = &HC2
            Public Const EM_SETSEL As Int32 = &HB1

            Public Const WM_USER As Int32 = &H400
            Public Const EM_GETCHARFORMAT As Int32 = (WM_USER + 58)
            Public Const EM_SETCHARFORMAT As Int32 = (WM_USER + 68)

            Public Const SCF_SELECTION As Int32 = &H1

            Public Const CFM_BOLD As Int32 = &H1
            Public Const CFM_CHARSET As Int32 = &H8000000
            Public Const CFM_COLOR As Int32 = &H40000000
            Public Const CFM_FACE As Int32 = &H20000000
            Public Const CFM_ITALIC As Int32 = &H2
            Public Const CFM_OFFSET As Int32 = &H10000000
            Public Const CFM_PROTECTED As Int32 = &H10
            Public Const CFM_SIZE As Int32 = &H80000000
            Public Const CFM_STRIKEOUT As Int32 = &H8
            Public Const CFM_UNDERLINE As Int32 = &H4

            Public Const CFE_AUTOCOLOR As Int32 = &H40000000
            Public Const CFE_BOLD As Int32 = &H1
            Public Const CFE_DISABLED As Int32 = &H2000
            Public Const CFE_ITALIC As Int32 = &H2
            Public Const CFE_PROTECTED As Int32 = &H10
            Public Const CFE_STRIKEOUT As Int32 = &H8
            Public Const CFE_UNDERLINE As Int32 = &H4

            Public Const LF_FACESIZE As Integer = 32

            <StructLayoutAttribute(LayoutKind.Sequential)>
            Public Structure CHARFORMAT
                Dim cbSize As Int32
                Dim dwMask As Int32
                Dim dwEffects As Int32
                Dim yHeight As Int32
                Dim yOffset As Int32
                Dim crTextColor As Int32
                Dim bCharSet As Byte
                Dim bPitchAndFamily As Byte
                <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=LF_FACESIZE)>
                Dim szFaceName As String
            End Structure

        End Module

    End Namespace

    Public Class clsColorRichTextBox

#Region " Constructors "

        Public Sub New(ByVal rtbControl As System.Windows.Forms.RichTextBox)
            Me.New(rtbControl,
            New clsColorRichTextBoxSchema(
                clsColorRichTextBoxSchema.enColorSchemaType.VBNET),
            True)
        End Sub

        Public Sub New(ByVal rtbControl As System.Windows.Forms.RichTextBox,
            ByVal rtbColorSchema As ColorRichTextBox.clsColorRichTextBoxSchema)
            Me.New(rtbControl, rtbColorSchema, True)
        End Sub

        Public Sub New(ByVal rtbControl As System.Windows.Forms.RichTextBox,
            ByVal rtbColorSchema As ColorRichTextBox.clsColorRichTextBoxSchema,
            ByVal bColorCode As Boolean)
            RichTextBoxControl = rtbControl
            ColorSchema = rtbColorSchema
            m_bColorCode = bColorCode
            m_rtbControl.ForeColor = m_ColorSchema.NormalColor
            m_rtbControl.Font = m_ColorSchema.NormalFont
        End Sub

#End Region

#Region " Private properties "

        Private bIsPaste As Boolean
        Private iPreviousLineIndex As Integer = 0
        Private iPreviousTextLength As Integer = 0
        Private bIsProcessingText As Boolean = False
        Private Matches As MatchCollection

#End Region

#Region " Public properties "

        Private WithEvents m_rtbControl As _
            System.Windows.Forms.RichTextBox
        Private m_bColorCode As Boolean = True
        Private m_ColorSchema As clsColorRichTextBoxSchema

        Public Property RichTextBoxControl() As _
            System.Windows.Forms.RichTextBox
            Get
                Return m_rtbControl
            End Get
            Set(ByVal Value As System.Windows.Forms.RichTextBox)
                m_rtbControl = Value
            End Set
        End Property

        Public Property ColorCode() As Boolean
            Get
                Return m_bColorCode
            End Get
            Set(ByVal Value As Boolean)
                m_bColorCode = Value
                If m_bColorCode = False Then UnColorEntireText()
            End Set
        End Property

        Public Property ColorSchema() As clsColorRichTextBoxSchema
            Get
                Return m_ColorSchema
            End Get
            Set(ByVal Value As clsColorRichTextBoxSchema)
                m_ColorSchema = Value
            End Set
        End Property

#End Region

#Region " Private methods "

        Private Sub ProcessString(ByVal sString As String)
            bIsProcessingText = True
            Dim iCurrentLineStartPosition As Integer =
                GetCurrentLineStartPosition()

            WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETSEL,
                iCurrentLineStartPosition,
                iCurrentLineStartPosition + sString.Length)
            WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETCHARFORMAT,
                WIN32API.SCF_SELECTION, m_ColorSchema.CharFormatNormalText)

            Dim slExcludeSpaces As New SortedList()
            Dim iCount As Integer

            'Coloring strings
            If Not sString.IndexOf("""") = -1 _
                AndAlso Not m_ColorSchema.RegexPatternString = String.Empty Then
                Matches = Regex.Matches(sString, m_ColorSchema.RegexPatternString)
                For iCount = 0 To Matches.Count - 1
                    WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETSEL,
                        Matches(iCount).Index + iCurrentLineStartPosition,
                        Matches(iCount).Index + iCurrentLineStartPosition + Matches(iCount).Length)
                    WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETCHARFORMAT,
                        WIN32API.SCF_SELECTION, m_ColorSchema.CharFormatString)
                    slExcludeSpaces.Add(Matches(iCount).Index + iCurrentLineStartPosition,
                        Matches(iCount).Index + iCurrentLineStartPosition + Matches(iCount).Length)
                Next iCount
            End If

            'Coloring line comments
            If Not sString.IndexOf(m_ColorSchema.LineComment) = -1 _
                AndAlso Not m_ColorSchema.RegexPatternLineComment = String.Empty Then
                Matches = Regex.Matches(sString, m_ColorSchema.RegexPatternLineComment)
                For iCount = 0 To Matches.Count - 1
                    If Not IsInExcludeSpace(Matches(iCount).Index + iCurrentLineStartPosition,
                        slExcludeSpaces) Then
                        WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETSEL,
                            Matches(iCount).Index + iCurrentLineStartPosition,
                            Matches(iCount).Index + iCurrentLineStartPosition + Matches(iCount).Length)
                        WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETCHARFORMAT,
                        WIN32API.SCF_SELECTION, m_ColorSchema.CharFormatLineComment)
                        slExcludeSpaces.Add(Matches(iCount).Index + iCurrentLineStartPosition,
                            Matches(iCount).Index + iCurrentLineStartPosition + Matches(iCount).Length)
                    End If
                Next iCount
            End If

            'Coloring keywords
            If Not m_ColorSchema.RegexPatternKeyword = String.Empty Then
                If m_ColorSchema.CaseSensitive Then
                    Matches = Regex.Matches(sString, m_ColorSchema.RegexPatternKeyword)
                Else
                    Matches = Regex.Matches(sString, m_ColorSchema.RegexPatternKeyword,
                        RegexOptions.IgnoreCase)
                End If
                For iCount = 0 To Matches.Count - 1
                    If Not IsInExcludeSpace(Matches(iCount).Index + iCurrentLineStartPosition,
                        slExcludeSpaces) Then
                        WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETSEL,
                            Matches(iCount).Index + iCurrentLineStartPosition,
                            Matches(iCount).Index + iCurrentLineStartPosition + Matches(iCount).Length)
                        WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETCHARFORMAT,
                            WIN32API.SCF_SELECTION, m_ColorSchema.CharFormatKeyword)
                        'Coloring keywords
                        If Not m_ColorSchema.CaseSensitive AndAlso m_ColorSchema.FormatKeyword Then
                            If m_ColorSchema.slKeywords.Contains(Matches(iCount).Value.ToLower) Then
                                WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_REPLACESEL, 0,
                                    m_ColorSchema.slKeywords(Matches(iCount).Value.ToLower).ToString)
                            End If
                        End If
                    End If
                Next iCount
            End If

            bIsProcessingText = False
        End Sub

        Private Function IsInExcludeSpace(ByVal iIndex As Integer,
            ByVal slExcludeSpaces As SortedList) As Boolean
            If slExcludeSpaces.Keys.Count = 0 Then Return False
            Dim aList As New ArrayList(slExcludeSpaces.Keys)
            Dim iFoundIndex As Integer = GetExcludeClosestStartPosition(iIndex, aList)
            If iFoundIndex = -1 Then Return False
            If iIndex < CInt(slExcludeSpaces(slExcludeSpaces.GetKey(iFoundIndex))) Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function GetExcludeClosestStartPosition(ByVal iIndex As Integer,
            ByVal aList As ArrayList) As Integer
            Dim iFoundIndex As Integer = aList.BinarySearch(iIndex)
            If iFoundIndex < 0 Then
                iFoundIndex = Not iFoundIndex
                If Not iFoundIndex = 0 Then
                    iFoundIndex -= 1
                    Return iFoundIndex
                Else
                    Return -1
                End If
            Else
                Return iFoundIndex
            End If
        End Function

        Private Sub LockWindow(ByVal bLock As Boolean)
            If bLock Then
                WIN32API.LockWindowUpdate(WIN32API.GetDesktopWindow())
            Else
                WIN32API.LockWindowUpdate(Nothing)
            End If
        End Sub

        Private Function GetCurrentLineStartPosition() As Integer
            Return WIN32API.SendMessage(m_rtbControl.Handle,
                WIN32API.EM_LINEINDEX, GetCurrentLineIndex(), 0)
        End Function

        Private Function GetCurrentLineString() As String
            Try
                Return m_rtbControl.Lines(GetCurrentLineIndex())
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

        Private Function GetCurrentLineIndex() As Integer
            Return WIN32API.SendMessage(m_rtbControl.Handle,
                WIN32API.EM_LINEFROMCHAR, -1, 0)
        End Function

        Private Function GetCurrentLineLength() As Integer
            Return WIN32API.SendMessage(m_rtbControl.Handle,
                WIN32API.EM_LINELENGTH, m_rtbControl.SelectionStart, 0)
        End Function

        Private Function GetLineLength(ByVal iLineIndex As Integer) As Integer
            Return WIN32API.SendMessage(m_rtbControl.Handle,
                WIN32API.EM_LINELENGTH, iLineIndex, 0)
        End Function

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' NEW: Updates syntax colors dynamically without breaking fonts or keywords.
        ''' </summary>
        Public Sub SetThemeColors(ByVal Normal As System.Drawing.Color,
                                  ByVal Keyword As System.Drawing.Color,
                                  ByVal Str As System.Drawing.Color,
                                  ByVal Comment As System.Drawing.Color)

            ' 1. Update the Schema Colors
            ' The setters in clsColorRichTextBoxSchema automatically call "PrepareCharFormats"
            m_ColorSchema.NormalColor = Normal
            m_ColorSchema.KeywordColor = Keyword
            m_ColorSchema.StringColor = Str
            m_ColorSchema.LineCommentColor = Comment

            ' 2. Update the RTB Base Colors
            m_rtbControl.ForeColor = Normal

            ' 3. Force Re-Coloring of the text
            RecolorEntireText()
            m_rtbControl.Refresh()
        End Sub

        Public Sub RecolorEntireText()


            If m_bColorCode Then
                If Not m_rtbControl.Text Is Nothing _
                    OrElse Not m_rtbControl.Text.Length = 0 Then
                    bIsProcessingText = True
                    LockWindow(True)
                    Dim iCurrentPosition As Integer = m_rtbControl.SelectionStart
                    m_rtbControl.SelectionStart =
                        WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_LINEINDEX, 0, 0)
                    ProcessString(m_rtbControl.Text)
                    WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETSEL,
                        iCurrentPosition, iCurrentPosition)
                    m_rtbControl.SelectionColor = m_ColorSchema.NormalColor
                    m_rtbControl.SelectionFont = m_ColorSchema.NormalFont
                    iPreviousTextLength = m_rtbControl.TextLength
                    LockWindow(False)
                    bIsProcessingText = False
                End If
            End If
        End Sub

        Public Sub UnColorEntireText()
            If Not m_rtbControl.Text Is Nothing _
                OrElse Not m_rtbControl.Text.Length = 0 Then
                bIsProcessingText = True
                LockWindow(True)
                Dim iCurrentPosition As Integer = m_rtbControl.SelectionStart
                WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETSEL,
                    0, m_rtbControl.TextLength)
                m_rtbControl.SelectionColor = m_ColorSchema.NormalColor
                m_rtbControl.SelectionFont = m_ColorSchema.NormalFont
                WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETSEL,
                    iCurrentPosition, iCurrentPosition)
                iPreviousTextLength = m_rtbControl.TextLength
                LockWindow(False)
                bIsProcessingText = False
            End If
        End Sub

#End Region

#Region " Handlers "

        Private Sub m_rtbControl_KeyDown(ByVal sender As Object,
            ByVal e As System.Windows.Forms.KeyEventArgs) Handles m_rtbControl.KeyDown
            bIsPaste = False
            If e.Control = True Then
                Select Case e.KeyCode
                    Case Keys.V
                        bIsPaste = True
                        iPreviousLineIndex = GetCurrentLineIndex()
                        If m_rtbControl.SelectionLength = m_rtbControl.TextLength Then
                            iPreviousTextLength = 0
                        Else
                            iPreviousTextLength = m_rtbControl.TextLength
                        End If
                End Select
            ElseIf e.Shift = True Then
                Select Case e.KeyCode
                    Case Keys.Insert
                        bIsPaste = True
                        iPreviousLineIndex = GetCurrentLineIndex()
                        If m_rtbControl.SelectionLength = m_rtbControl.TextLength Then
                            iPreviousTextLength = 0
                        Else
                            iPreviousTextLength = m_rtbControl.TextLength
                        End If
                End Select
            End If
        End Sub
        'HERE IS THE NEW TEXT CHANGED EVENT HANDLER
        Private Sub m_rtbControl_TextChanged(ByVal sender As Object,
            ByVal e As System.EventArgs) Handles m_rtbControl.TextChanged
            If bIsProcessingText Then Return
            If m_bColorCode Then
                LockWindow(True)
                If m_rtbControl.TextLength = 0 Then
                    iPreviousTextLength = 0
                    WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETSEL, 0, -1)
                    WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETSEL, 0, 0)
                    WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETCHARFORMAT,
                        WIN32API.SCF_SELECTION, m_ColorSchema.CharFormatNormalText)
                    Return
                End If
                Dim iCurrentPosition As Integer = m_rtbControl.SelectionStart
                If bIsPaste Then
                    If iPreviousTextLength = 0 Then
                        RecolorEntireText()
                    Else
                        Dim iCurrentLineIndex As Integer = GetCurrentLineIndex()
                        If iPreviousLineIndex = iCurrentLineIndex Then
                            ProcessString(GetCurrentLineString())
                        Else
                            Try
                                m_rtbControl.SelectionStart = WIN32API.SendMessage(
                                                              m_rtbControl.Handle, WIN32API.EM_LINEINDEX,
                                                              iPreviousLineIndex, 0)
                                ProcessString(
                                    String.Join(vbLf,
                                                m_rtbControl.Lines,
                                                iPreviousLineIndex,
                                                iCurrentLineIndex - iPreviousLineIndex + 1))
                            Catch ex As Exception

                            End Try

                        End If
                    End If
                Else
                    If iPreviousTextLength = 0 Then
                        RecolorEntireText()
                        '      m_rtbControl.Refresh() ' new addition
                    Else
                        ProcessString(GetCurrentLineString())
                    End If
                End If
                WIN32API.SendMessage(m_rtbControl.Handle, WIN32API.EM_SETSEL,
                    iCurrentPosition, iCurrentPosition)
                m_rtbControl.SelectionColor = m_ColorSchema.NormalColor
                m_rtbControl.SelectionFont = m_ColorSchema.NormalFont
                iPreviousTextLength = m_rtbControl.TextLength
                LockWindow(False)
            End If




        End Sub



#End Region

    End Class

    <Serializable()> Public Class clsColorRichTextBoxSchema

#Region " Constructors "

        Public Sub New(ByVal TypeOfColorSchema As enColorSchemaType)
            ColorSchemaType = TypeOfColorSchema
        End Sub

        Public Sub New(ByVal ColorSchema As clsColorRichTextBoxSchema)
            m_aKeywords = ColorSchema.Keywords
            m_aKeywords2 = ColorSchema.Keywords2
            m_sLineComment = ColorSchema.LineComment
            m_cNormalColor = ColorSchema.NormalColor
            m_cKeywordColor = ColorSchema.KeywordColor
            m_cLineCommentColor = ColorSchema.LineCommentColor
            m_cStringColor = ColorSchema.StringColor
            m_fNormalFont = ColorSchema.NormalFont
            m_fKeywordFont = ColorSchema.KeywordFont
            m_fLineCommentFont = ColorSchema.LineCommentFont
            m_fStringFont = ColorSchema.StringFont
            m_bCaseSensitive = ColorSchema.CaseSensitive
            m_bFormatKeyword = ColorSchema.FormatKeyword
            m_ColorSchemaType = ColorSchema.ColorSchemaType
            PrepareSchema()
        End Sub

#End Region

#Region " Private properties "

        Private Const SCHEMA_KEYWORDS_SEPARATOR As String = "|"

#End Region

#Region " Public properties "

        Public Enum enColorSchemaType
            VBNET
            Other
        End Enum

        Private m_aKeywords() As String
        Private m_aKeywords2() As String

        Private m_sLineComment As String
        Private m_cNormalColor As System.Drawing.Color
        Private m_cKeywordColor As System.Drawing.Color
        Private m_cLineCommentColor As System.Drawing.Color
        Private m_cStringColor As System.Drawing.Color
        Private m_fNormalFont As System.Drawing.Font
        Private m_fKeywordFont As System.Drawing.Font
        Private m_fLineCommentFont As System.Drawing.Font
        Private m_fStringFont As System.Drawing.Font
        Private m_bCaseSensitive As Boolean
        Private m_bFormatKeyword As Boolean
        Private m_ColorSchemaType As enColorSchemaType
        <NonSerialized()> Private m_sRegexPatternKeyword As String
        <NonSerialized()> Private m_sRegexPatternLineComment As String
        <NonSerialized()> Private m_sRegexPatternString As String
        <NonSerialized()> Private m_slKeywords As New SortedList()
        <NonSerialized()> Private m_slKeywords2 As New SortedList()
        <NonSerialized()> Private m_cfNormalText As WIN32API.CHARFORMAT
        <NonSerialized()> Private m_cfKeyword As WIN32API.CHARFORMAT
        <NonSerialized()> Private m_cfLineComment As WIN32API.CHARFORMAT
        <NonSerialized()> Private m_cfString As WIN32API.CHARFORMAT

        Public Property Keywords() As String()
            Get
                Return m_aKeywords
            End Get
            Set(ByVal Value As String())
                m_aKeywords = Value
                PrepareSchema()
            End Set
        End Property
        Public Property Keywords2() As String()
            Get
                Return m_aKeywords2
            End Get
            Set(ByVal Value As String())
                m_aKeywords2 = Value
                PrepareSchema()
            End Set
        End Property

        Public Property LineComment() As String
            Get
                Return m_sLineComment
            End Get
            Set(ByVal Value As String)
                m_sLineComment = Value
                PrepareSchema()
            End Set
        End Property

        Public Property NormalColor() As System.Drawing.Color
            Get
                Return m_cNormalColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_cNormalColor = Value
                PrepareCharFormats(m_cfNormalText, m_fNormalFont, m_cNormalColor)
            End Set
        End Property

        Public Property KeywordColor() As System.Drawing.Color
            Get
                Return m_cKeywordColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_cKeywordColor = Value
                PrepareCharFormats(m_cfKeyword, m_fKeywordFont, m_cKeywordColor)
            End Set
        End Property

        Public Property LineCommentColor() As System.Drawing.Color
            Get
                Return m_cLineCommentColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_cLineCommentColor = Value
                PrepareCharFormats(m_cfLineComment,
                    m_fLineCommentFont, m_cLineCommentColor)
            End Set
        End Property

        Public Property StringColor() As System.Drawing.Color
            Get
                Return m_cStringColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_cStringColor = Value
                PrepareCharFormats(m_cfString, m_fStringFont, m_cStringColor)
            End Set
        End Property

        Public Property NormalFont() As System.Drawing.Font
            Get
                Return m_fNormalFont
            End Get
            Set(ByVal Value As System.Drawing.Font)
                m_fNormalFont = Value
                PrepareCharFormats(m_cfNormalText, m_fNormalFont, m_cNormalColor)
            End Set
        End Property

        Public Property KeywordFont() As System.Drawing.Font
            Get
                Return m_fKeywordFont
            End Get
            Set(ByVal Value As System.Drawing.Font)
                m_fKeywordFont = Value
                PrepareCharFormats(m_cfKeyword, m_fKeywordFont, m_cKeywordColor)
            End Set
        End Property

        Public Property LineCommentFont() As System.Drawing.Font
            Get
                Return m_fLineCommentFont
            End Get
            Set(ByVal Value As System.Drawing.Font)
                m_fLineCommentFont = Value
                PrepareCharFormats(m_cfLineComment,
                    m_fLineCommentFont, m_cLineCommentColor)
            End Set
        End Property

        Public Property StringFont() As System.Drawing.Font
            Get
                Return m_fStringFont
            End Get
            Set(ByVal Value As System.Drawing.Font)
                m_fStringFont = Value
                PrepareCharFormats(m_cfString, m_fStringFont, m_cStringColor)
            End Set
        End Property

        Public Property CaseSensitive() As Boolean
            Get
                Return m_bCaseSensitive
            End Get
            Set(ByVal Value As Boolean)
                m_bCaseSensitive = Value
            End Set
        End Property

        Public Property FormatKeyword() As Boolean
            Get
                Return m_bFormatKeyword
            End Get
            Set(ByVal Value As Boolean)
                m_bFormatKeyword = Value
            End Set
        End Property

        Public Property ColorSchemaType() As enColorSchemaType
            Get
                Return m_ColorSchemaType
            End Get
            Set(ByVal Value As enColorSchemaType)
                m_ColorSchemaType = Value
                ChangeSchema(Value)
            End Set
        End Property

        Public ReadOnly Property RegexPatternKeyword() As String
            Get
                Return m_sRegexPatternKeyword
            End Get
        End Property

        Public ReadOnly Property RegexPatternLineComment() As String
            Get
                Return m_sRegexPatternLineComment
            End Get
        End Property

        Public ReadOnly Property RegexPatternString() As String
            Get
                Return m_sRegexPatternString
            End Get
        End Property

        Public ReadOnly Property slKeywords() As SortedList
            Get
                Return m_slKeywords
            End Get
        End Property

        Public ReadOnly Property CharFormatNormalText() As WIN32API.CHARFORMAT
            Get
                Return m_cfNormalText
            End Get
        End Property

        Public ReadOnly Property CharFormatKeyword() As WIN32API.CHARFORMAT
            Get
                Return m_cfKeyword
            End Get
        End Property

        Public ReadOnly Property CharFormatLineComment() As WIN32API.CHARFORMAT
            Get
                Return m_cfLineComment
            End Get
        End Property

        Public ReadOnly Property CharFormatString() As WIN32API.CHARFORMAT
            Get
                Return m_cfString
            End Get
        End Property

#End Region

#Region " Private methods "

        Private Sub ChangeSchema(ByVal SchemaType As enColorSchemaType)
            Select Case SchemaType
                Case ColorSchemaType.VBNET
                    '      m_aKeywords = "Addhandler,AddressOf,Alias,And,AndAlso,Ansi,As,Assembly,Auto,Boolean,ByRef,Byte,ByVal,Call,Case,Catch,CBool,CByte,CChar,CDate,CDbl,CDec,Char,CInt,Class,CLng,CObj,Const,CShort,CSng,CStr,CType,Date,Decimal,Declare,Default,Delegate,Dim,DirectCast,Do,Double,Each,Else,ElseIf,End,EndIf,Enum,Erase,Error,Event,Exit,Explicit,ExternalSource,False,Finally,For,Friend,Function,Get,GetType,GoTo,Handles,If,Implements,Imports,In,Inherits,Integer,Interface,Is,Lib,Like,Long,Loop,Me,Mod,Module,MustInherit,MustOverride,MyBase,MyClass,Namespace,New,Next,Not,Nothing,NotInheritable,NotOverridable,Object,Off,On,Option,Option Compare Binary,Option Compare Text,Optional,Or,OrElse,Overloads,Overridable,Overrides,ParamArray,Preserve,Private,Property,Protected,Public,RaiseEvent,ReadOnly,ReDim,Region,RemoveHandler,Resume,Return,Select,Set,Shadows,Shared,Short,Single,Static,Step,Stop,Strict,String,Structure,Sub,SyncLock,Then,Throw,To,True,Try,TypeOf,Unicode,Until,When,While,With,WithEvents,WriteOnly,Xor".Split(","c)
                    '     m_aKeywords = "collectgarbage,gpio.INPUT,gpio.OUTPUT,adc.read,adc.readvdd33,dht.read,dht.read11,file.close,file.exists,file.flush,file.format,file.fscfg,file.fsinfo,file.list,file.open,file.read,file.readline,file.remove,file.rename,file.seek,file.write,file.writeline,http.delete,http.get,http.post,http.put,http.request,i2c.address,i2c.read,i2c.setup,i2c.start,i2c.stop,i2c.write,mdns.register,mdns.close,mqtt.Client,ow.check_crc16,ow.crc16,ow.crc8,ow.depower,ow.read,ow.read_bytes,ow.reset,ow.reset_search,ow.search,ow.select,ow.setup,ow.skip,ow.target_search,ow.write,ow.write_bytes,pwm.close,pwm.getclock,pwm.getduty,pwm.setclock,pwm.setduty,pwm.setup,pwm.start,pwm.stop,tmr.alarm,tmr.delay,tmr.interval,tmr.now,tmr.register,tmr.softwd,tmr.start,tmr.state,tmr.stop,tmr.time,tmr.unregister,tmr.wdclr,uart.alt,uart.on,uart.setup,uart.write,if,then,else,elseif,end,nil,print,false,true,function,local,for,do,dofile,while,break,not,or,until,repeat,return,gpio.mode,gpio.write,gpio.read,gpio.serout,gpio.trig,node.bootreason,node.chipid,node.compile,node.dsleep,node.flashid,node.heap,node.info,node.input,node.key,node.led,node.output,node.readvdd33,node.restart,node.restore,node.setcpufreq,node.stripdebug,node.egc.setmode,wifi.getchannel,wifi.getmode,wifi.getphymode,wifi.setmode,wifi.setphymode,wifi.sleeptype,wifi.startsmart,wifi.stopsmart,wifi.sta.autoconnect,wifi.sta.config,wifi.sta.connect,wifi.sta.disconnect,wifi.sta.eventMonReg,wifi.sta.eventMonStart,wifi.sta.eventMonStop,wifi.sta.getap,wifi.sta.getbroadcast,wifi.sta.getconfig,wifi.sta.gethostname,wifi.sta.getip,wifi.sta.getmac,wifi.sta.sethostname,wifi.sta.setip,wifi.sta.setmac,wifi.sta.status,wifi.ap.config,wifi.ap.getbroadcast,wifi.ap.getclient,wifi.ap.getip,wifi.ap.getmac,wifi.ap.setip,wifi.ap.setmac,wifi.ap.dhcp.config,wifi.ap.dhcp.start,wifi.ap.dhcp.stop,net.createConnection,net.createServer,net.server:close,net.TCP,net.UDP,net.server:listen,net.socket:close,net.socket:connect,net.socket:dns,net.socket:on,net.socket:send,net.dns.getdnsserver,net.dns.resolve,net.dns.setdnsserver".Split(","c)
                    m_aKeywords = "string.gmatch,string.find,gpio.HIGH,gpio.LOW,collectgarbage,gpio.INPUT,gpio.OUTPUT,adc.read,adc.readvdd33,dht.read,dht.read11,file.close,file.exists,file.flush,file.format,file.fscfg,file.fsinfo,file.list,file.open,file.read,file.readline,file.remove,file.rename,file.seek,file.write,file.writeline,http.delete,http.get,http.post,http.put,http.request,i2c.address,i2c.read,i2c.setup,i2c.start,i2c.stop,i2c.write,mdns.register,mdns.close,mqtt.Client,ow.check_crc16,ow.crc16,ow.crc8,ow.depower,ow.read,ow.read_bytes,ow.reset,ow.reset_search,ow.search,ow.select,ow.setup,ow.skip,ow.target_search,ow.write,ow.write_bytes,pwm.close,pwm.getclock,pwm.getduty,pwm.setclock,pwm.setduty,pwm.setup,pwm.start,pwm.stop,tmr.alarm,tmr.delay,tmr.interval,tmr.now,tmr.register,tmr.softwd,tmr.start,tmr.state,tmr.stop,tmr.time,tmr.unregister,tmr.wdclr,uart.alt,uart.on,uart.setup,uart.write,if,then,else,elseif,end,nil,print,false,true,function,local,for,do,dofile,while,break,not,or,until,repeat,return,gpio.mode,gpio.write,gpio.read,gpio.serout,gpio.trig,node.bootreason,node.chipid,node.compile,node.dsleep,node.flashid,node.heap,node.info,node.input,node.key,node.led,node.output,node.readvdd33,node.restart,node.restore,node.setcpufreq,node.stripdebug,node.egc.setmode,wifi.getchannel,wifi.getmode,wifi.getphymode,wifi.setmode,wifi.setphymode,wifi.sleeptype,wifi.startsmart,wifi.stopsmart,wifi.sta.autoconnect,wifi.sta.config,wifi.sta.connect,wifi.sta.disconnect,wifi.sta.eventMonReg,wifi.sta.eventMonStart,wifi.sta.eventMonStop,wifi.sta.getap,wifi.sta.getbroadcast,wifi.sta.getconfig,wifi.sta.gethostname,wifi.sta.getip,wifi.sta.getmac,wifi.sta.sethostname,wifi.sta.setip,wifi.sta.setmac,wifi.sta.status,wifi.ap.config,wifi.ap.getbroadcast,wifi.ap.getclient,wifi.ap.getip,wifi.ap.getmac,wifi.ap.setip,wifi.ap.setmac,wifi.ap.dhcp.config,wifi.ap.dhcp.start,wifi.ap.dhcp.stop,net.createConnection,net.createServer,net.server:close,net.TCP,net.UDP,net.server:listen,net.socket:close,net.socket:connect,net.socket:dns,net.socket:on,net.socket:send,net.dns.getdnsserver,net.dns.resolve,net.dns.setdnsserver".Split(","c)

                    m_aKeywords2 = "gpio.HIGH,gpio.LOW".Split(","c)
                    m_sLineComment = "--"
                    m_bCaseSensitive = False
                    m_bFormatKeyword = True

                Case ColorSchemaType.Other
                    m_aKeywords = CType(
                        Array.CreateInstance(GetType(
                                System.String), 0), String())
                    m_sLineComment = String.Empty
                    m_bCaseSensitive = False
                    m_bFormatKeyword = False
            End Select
            m_cNormalColor = System.Drawing.Color.Black
            m_cKeywordColor = System.Drawing.Color.RoyalBlue
            m_cLineCommentColor = System.Drawing.Color.DarkOliveGreen
            m_cStringColor = System.Drawing.Color.Firebrick
            m_fNormalFont = New System.Drawing.Font(
                New Drawing.FontFamily("Courier New"), 10,
                FontStyle.Regular)
            m_fKeywordFont = New System.Drawing.Font(
                New Drawing.FontFamily("Courier New"), 10,
                FontStyle.Regular)
            m_fLineCommentFont = New System.Drawing.Font(
                New Drawing.FontFamily("Courier New"), 10,
                FontStyle.Regular Or FontStyle.Italic)
            m_fStringFont = New System.Drawing.Font(
                New Drawing.FontFamily("Courier New"), 10,
                FontStyle.Regular)
            PrepareSchema()
        End Sub

        Private Sub PrepareSchema()
            m_sRegexPatternKeyword = String.Empty
            m_sRegexPatternLineComment = String.Empty
            m_sRegexPatternString = String.Empty
            If Not m_aKeywords Is Nothing _
                AndAlso Not m_aKeywords.Length = 0 Then
                m_slKeywords = New SortedList()
                Dim iCount As Integer
                For iCount = 0 To m_aKeywords.Length - 1
                    m_sRegexPatternKeyword &=
                    ChangeSystemSymbols(m_aKeywords(iCount)) _
                    & SCHEMA_KEYWORDS_SEPARATOR
                    m_slKeywords.Add(m_aKeywords(iCount).ToLower, m_aKeywords(iCount))
                Next iCount
                m_sRegexPatternKeyword = m_sRegexPatternKeyword.Substring(0,
                    m_sRegexPatternKeyword.Length - SCHEMA_KEYWORDS_SEPARATOR.Length)
                If Not m_sRegexPatternKeyword = String.Empty Then
                    m_sRegexPatternKeyword = "\b(" & m_sRegexPatternKeyword & ")\b"
                End If
            End If
            If Not m_sLineComment = String.Empty Then
                m_sRegexPatternLineComment = ChangeSystemSymbols(m_sLineComment) & "(.*?)(\n|$)"
            End If
            ''''''' my add

            'If Not m_aKeywords2 Is Nothing _
            '   AndAlso Not m_aKeywords2.Length = 0 Then
            '    m_slKeywords2 = New SortedList()
            '    Dim iCount As Integer
            '    For iCount = 0 To m_aKeywords2.Length - 1
            '        m_sRegexPatternKeyword &= _
            '        ChangeSystemSymbols(m_aKeywords2(iCount)) _
            '        & SCHEMA_KEYWORDS_SEPARATOR
            '        m_slKeywords2.Add(m_aKeywords2(iCount).ToLower, m_aKeywords2(iCount))
            '    Next iCount
            '    m_sRegexPatternKeyword = m_sRegexPatternKeyword.Substring(0, _
            '        m_sRegexPatternKeyword.Length - SCHEMA_KEYWORDS_SEPARATOR.Length)
            '    If Not m_sRegexPatternKeyword = String.Empty Then
            '        m_sRegexPatternKeyword = "\b(" & m_sRegexPatternKeyword & ")\b"
            '    End If
            'End If

            '''''''
            m_sRegexPatternString = """(.*?)(""|\n|$)"
            PrepareCharFormats(m_cfNormalText, m_fNormalFont, m_cNormalColor)
            PrepareCharFormats(m_cfLineComment, m_fLineCommentFont, m_cLineCommentColor)
            PrepareCharFormats(m_cfKeyword, m_fKeywordFont, m_cKeywordColor)
            PrepareCharFormats(m_cfString, m_fStringFont, m_cStringColor)
        End Sub

        Private Sub PrepareCharFormats(ByRef CharFormat As WIN32API.CHARFORMAT,
            ByVal CharFormatFont As System.Drawing.Font,
            ByVal CharFormatColor As System.Drawing.Color)
            CharFormat = New WIN32API.CHARFORMAT()
            CharFormat.cbSize = Marshal.SizeOf(GetType(WIN32API.CHARFORMAT))
            CharFormat.dwMask = WIN32API.CFM_BOLD Or WIN32API.CFM_COLOR Or
                WIN32API.CFM_FACE Or WIN32API.CFM_ITALIC Or
                WIN32API.CFM_SIZE Or WIN32API.CFM_STRIKEOUT Or
                WIN32API.CFM_UNDERLINE
            If CharFormatFont.Bold Then CharFormat.dwEffects =
                WIN32API.CFE_BOLD
            If CharFormatFont.Italic Then CharFormat.dwEffects =
                CharFormat.dwEffects Or WIN32API.CFE_ITALIC
            If CharFormatFont.Strikeout Then CharFormat.dwEffects =
                CharFormat.dwEffects Or WIN32API.CFE_STRIKEOUT
            If CharFormatFont.Underline Then CharFormat.dwEffects =
                CharFormat.dwEffects Or WIN32API.CFE_UNDERLINE
            CharFormat.yHeight = CInt((CharFormatFont.Size * 20))
            CharFormat.szFaceName = CharFormatFont.Name
            CharFormat.crTextColor =
                System.Drawing.Color.FromArgb(
                    0, CharFormatColor.B, CharFormatColor.G, CharFormatColor.R).ToArgb
        End Sub

        Private Function ChangeSystemSymbols(ByVal sString As String) As String
            sString = sString.Replace("\", "\\")
            sString = sString.Replace("/", "\/")
            sString = sString.Replace("*", "\*")
            sString = sString.Replace("^", "\^")
            sString = sString.Replace("+", "\+")
            sString = sString.Replace("?", "\?")
            sString = sString.Replace("[", "\[")
            sString = sString.Replace("]", "\]")
            sString = sString.Replace("{", "\{")
            sString = sString.Replace("}", "\}")
            sString = sString.Replace(".", "\.")
            sString = sString.Replace("|", "\|")
            Return sString
        End Function

#End Region

#Region " Public methods "

        Public Overloads Sub SaveSchema(ByVal FileName As String)
            SaveSchema(FileName, Me)
        End Sub

        Public Overloads Sub SaveSchema(ByVal FileName As String,
            ByVal Schema As clsColorRichTextBoxSchema)
            Try
                Dim stream As IO.Stream =
                    IO.File.Open(FileName, IO.FileMode.Create)
                Dim formatter As New BinaryFormatter()
                formatter.Serialize(stream, Schema)
                stream.Close()
            Catch
            End Try
        End Sub

        Public Function LoadSchema(ByVal FileName As String) As clsColorRichTextBoxSchema
            Try
                Dim stream As IO.Stream = IO.File.Open(FileName, IO.FileMode.Open)
                Dim formatter As New BinaryFormatter()
                Dim RetVal As clsColorRichTextBoxSchema =
                    CType(formatter.Deserialize(stream), clsColorRichTextBoxSchema)
                stream.Close()
                Return RetVal
            Catch
                Return Nothing
            End Try
        End Function

#End Region

    End Class

End Namespace
