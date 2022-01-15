Class App
    Inherits Application

    Const kFileQuitShortcut = ""
    Const kFileQuit = "Quit"
    Const kEditClear = "Clear"

    App.AboutCmd:
    Function AboutCmd() As Boolean
        AboutBox.Show
    End Function

End Class