Imports System.Drawing

Public Module ModThemes

    Public Structure ThemePalette
        Public Name As String
        ' Main UI
        Public FormBack As Color
        Public ControlBack As Color
        Public ForeColor As Color
        Public ButtonBack As Color
        Public ButtonFore As Color

        ' Editors
        Public EditorBack As Color
        Public EditorFore As Color
        Public LineNumberBack As Color
        Public LineNumberFore As Color

        ' Syntax Highlighting (High Contrast)
        Public Code_Keyword As Color
        Public Code_String As Color
        Public Code_Comment As Color

        ' Terminal Semantic Colors
        Public Term_RX As Color
        Public Term_Prompt As Color
        Public Term_Command As Color
        Public Term_System As Color
        Public Term_Error As Color
    End Structure

    Public Function GetTheme(name As String) As ThemePalette
        Dim t As New ThemePalette
        t.Name = name

        Select Case name
            Case "Default" ' Dark Grey / Green
                t.FormBack = Color.FromArgb(45, 45, 48)
                t.ControlBack = Color.FromArgb(30, 30, 30)
                t.ForeColor = Color.Gainsboro
                t.ButtonBack = Color.SeaGreen ' Makes the border/macro buttons distinct green
                t.ButtonFore = Color.White

                t.EditorBack = Color.FromArgb(30, 30, 30)
                t.EditorFore = Color.Gainsboro
                t.LineNumberBack = Color.FromArgb(45, 45, 48)
                t.LineNumberFore = Color.CadetBlue

                ' Classic VS Style
                t.Code_Keyword = Color.DeepSkyBlue
                t.Code_String = Color.LightSalmon
                t.Code_Comment = Color.SeaGreen

                t.Term_RX = Color.LimeGreen
                t.Term_Prompt = Color.Gold
                t.Term_Command = Color.DodgerBlue
                t.Term_System = Color.Orange
                t.Term_Error = Color.Gray

            Case "DarkMatter" ' Deep Blue / Neon
                t.FormBack = Color.FromArgb(20, 20, 35)
                t.ControlBack = Color.FromArgb(10, 10, 25)
                t.ForeColor = Color.AliceBlue
                t.ButtonBack = Color.FromArgb(40, 40, 60)
                t.ButtonFore = Color.Cyan

                t.EditorBack = Color.FromArgb(15, 15, 30)
                t.EditorFore = Color.Cyan
                t.LineNumberBack = Color.FromArgb(25, 25, 40)
                t.LineNumberFore = Color.SlateBlue

                ' Neon Cyberpunk
                t.Code_Keyword = Color.Magenta
                t.Code_String = Color.Yellow
                t.Code_Comment = Color.FromArgb(100, 100, 150)

                t.Term_RX = Color.Cyan
                t.Term_Prompt = Color.Magenta
                t.Term_Command = Color.White
                t.Term_System = Color.Yellow
                t.Term_Error = Color.DimGray

            Case "Beehive" ' Black / Gold
                t.FormBack = Color.Black
                t.ControlBack = Color.FromArgb(25, 25, 25)
                t.ForeColor = Color.Gold
                t.ButtonBack = Color.FromArgb(40, 40, 0)
                t.ButtonFore = Color.Gold

                t.EditorBack = Color.Black
                t.EditorFore = Color.Khaki
                t.LineNumberBack = Color.FromArgb(20, 20, 0)
                t.LineNumberFore = Color.Goldenrod

                ' High Contrast Yellows
                t.Code_Keyword = Color.Orange
                t.Code_String = Color.White
                t.Code_Comment = Color.Gray

                t.Term_RX = Color.Yellow
                t.Term_Prompt = Color.OrangeRed
                t.Term_Command = Color.White
                t.Term_System = Color.Orange
                t.Term_Error = Color.FromArgb(64, 64, 64)

            Case "Thin Green" ' White / Green
                t.FormBack = Color.White
                t.ControlBack = Color.WhiteSmoke
                t.ForeColor = Color.Black
                t.ButtonBack = Color.Honeydew
                t.ButtonFore = Color.DarkGreen

                t.EditorBack = Color.White
                t.EditorFore = Color.Black
                t.LineNumberBack = Color.WhiteSmoke
                t.LineNumberFore = Color.SeaGreen

                ' Classic IDE (Bold colors on white)
                t.Code_Keyword = Color.Blue
                t.Code_String = Color.Red
                t.Code_Comment = Color.Green

                t.Term_RX = Color.DarkGreen
                t.Term_Prompt = Color.Red
                t.Term_Command = Color.Blue
                t.Term_System = Color.DarkOrange
                t.Term_Error = Color.LightGray

            Case "Into The Light" ' Light Gray / Blue
                t.FormBack = Color.FromArgb(240, 240, 245)
                t.ControlBack = Color.White
                t.ForeColor = Color.DarkSlateBlue
                t.ButtonBack = Color.LightSteelBlue
                t.ButtonFore = Color.DarkBlue

                t.EditorBack = Color.White
                t.EditorFore = Color.Black
                t.LineNumberBack = Color.FromArgb(230, 230, 240)
                t.LineNumberFore = Color.SteelBlue

                ' Corporate Blue Style
                t.Code_Keyword = Color.Navy
                t.Code_String = Color.Maroon
                t.Code_Comment = Color.SlateGray

                t.Term_RX = Color.Navy
                t.Term_Prompt = Color.DarkMagenta
                t.Term_Command = Color.Black
                t.Term_System = Color.Crimson
                t.Term_Error = Color.Silver

            Case "Mean Orange" ' Black / Orange
                t.FormBack = Color.FromArgb(20, 20, 20)
                t.ControlBack = Color.Black
                t.ForeColor = Color.OrangeRed
                t.ButtonBack = Color.FromArgb(60, 30, 0)
                t.ButtonFore = Color.Orange

                t.EditorBack = Color.Black
                t.EditorFore = Color.Orange
                t.LineNumberBack = Color.FromArgb(40, 20, 0)
                t.LineNumberFore = Color.Chocolate

                ' Fiery Colors
                t.Code_Keyword = Color.Gold
                t.Code_String = Color.White
                t.Code_Comment = Color.Gray

                t.Term_RX = Color.OrangeRed
                t.Term_Prompt = Color.Yellow
                t.Term_Command = Color.White
                t.Term_System = Color.Red
                t.Term_Error = Color.DimGray

            Case "Happy Day" ' Beige / Brown
                t.FormBack = Color.Beige
                t.ControlBack = Color.Cornsilk
                t.ForeColor = Color.SaddleBrown
                t.ButtonBack = Color.Wheat
                t.ButtonFore = Color.SaddleBrown

                t.EditorBack = Color.Ivory
                t.EditorFore = Color.Black
                t.LineNumberBack = Color.Cornsilk
                t.LineNumberFore = Color.Peru

                ' Earthy Tones
                t.Code_Keyword = Color.SaddleBrown
                t.Code_String = Color.IndianRed
                t.Code_Comment = Color.OliveDrab

                t.Term_RX = Color.SaddleBrown
                t.Term_Prompt = Color.Red
                t.Term_Command = Color.DarkBlue
                t.Term_System = Color.DarkMagenta
                t.Term_Error = Color.LightGray

            Case "So Limbo" ' Dark Gray / Lime
                t.FormBack = Color.DimGray
                t.ControlBack = Color.Gray
                t.ForeColor = Color.White
                t.ButtonBack = Color.OliveDrab
                t.ButtonFore = Color.White

                t.EditorBack = Color.FromArgb(50, 50, 50)
                t.EditorFore = Color.GreenYellow
                t.LineNumberBack = Color.DimGray
                t.LineNumberFore = Color.LightGray

                ' Radioactive Style
                t.Code_Keyword = Color.Cyan
                t.Code_String = Color.Yellow
                t.Code_Comment = Color.LightGreen

                t.Term_RX = Color.GreenYellow
                t.Term_Prompt = Color.White
                t.Term_Command = Color.Cyan
                t.Term_System = Color.Orange
                t.Term_Error = Color.DarkGray

            Case "Old Coffee" ' Brown / Tan
                t.FormBack = Color.FromArgb(60, 40, 30)
                t.ControlBack = Color.FromArgb(50, 30, 20)
                t.ForeColor = Color.BurlyWood
                t.ButtonBack = Color.FromArgb(80, 50, 40)
                t.ButtonFore = Color.Wheat

                t.EditorBack = Color.FromArgb(40, 25, 15)
                t.EditorFore = Color.Wheat
                t.LineNumberBack = Color.FromArgb(50, 35, 25)
                t.LineNumberFore = Color.Tan

                ' Vintage Paper
                t.Code_Keyword = Color.Gold
                t.Code_String = Color.LightSalmon
                t.Code_Comment = Color.DarkSeaGreen

                t.Term_RX = Color.BurlyWood
                t.Term_Prompt = Color.White
                t.Term_Command = Color.Cyan
                t.Term_System = Color.Orange
                t.Term_Error = Color.FromArgb(80, 60, 50)

            Case "Painted Stairway" ' Slate / Teal
                t.FormBack = Color.SlateGray
                t.ControlBack = Color.LightSlateGray
                t.ForeColor = Color.White
                t.ButtonBack = Color.DarkSlateGray
                t.ButtonFore = Color.White

                t.EditorBack = Color.FromArgb(230, 240, 250)
                t.EditorFore = Color.Black
                t.LineNumberBack = Color.LightSlateGray
                t.LineNumberFore = Color.White

                ' Cool Tones
                t.Code_Keyword = Color.DarkBlue
                t.Code_String = Color.Crimson
                t.Code_Comment = Color.Teal

                t.Term_RX = Color.DarkSlateGray
                t.Term_Prompt = Color.Red
                t.Term_Command = Color.Blue
                t.Term_System = Color.Purple
                t.Term_Error = Color.Silver
        End Select
        Return t
    End Function


    '
    Public ThemeList As New List(Of String) From {
        "Default", "DarkMatter", "Beehive", "Thin Green",
        "Into The Light", "Mean Orange", "Happy Day",
        "So Limbo", "Old Coffee", "Painted Stairway"
    }

End Module