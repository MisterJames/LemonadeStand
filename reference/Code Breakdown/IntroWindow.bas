Class IntroWindow
    Inherits Window

    IntroWindow.Open:
    Sub Open() Handles Event
        Player.PlaySong 10, 200, "72,3 67,1 69,1 67,2 65,1 64,1 67,6 67,1 69,1 74,2 72,1 71,1
        69,2 71,1 72,5"
    End Sub


    IntroWindow.FillUpCup:
    Protected Sub FillUpCup()
        Select Case FillUpCounter
            Case 20
                FillRect.Visible = True
                LemonadeRect.FillColor = FillRect.FillColor
            Case 21 to 95
                LemonadeRect.Top = LemonadeRect.Top - 1
                LemonadeRect.Height = LemonadeRect.Height + 1
            Case 96
                FillRect.Visible = False
                FillUpTimer.Mode = 0
        End Select

        FillUpCounter = FillUpCounter + 1
    End Sub


    IntroWindow Control InstructionsButn:
    Sub Action() Handles Event
        PagePanel1.value = (PagePanel1.Value + 1) mod 3
        select case PagePanel1.Value
            case 0
                me.Caption = "Instructions"
            case 1
                me.Caption = "More"
            case 2
                me.Caption = "Back"
            end select
    End Sub


    IntroWindow Control PlayButn:
    Sub Action() Handles Event
        Dim qtyPlayers As Integer = HowManySheet.Present( self )
        if qtyPlayers > 0 then
            MainWindow.SetQtyPlayers qtyPlayers
            MainWindow.Show
            Close
        end if
    End Sub


    IntroWindow Control FillUpTimer:
    Sub Action() Handles Event
        FillUpCup()
    End Sub


End Class