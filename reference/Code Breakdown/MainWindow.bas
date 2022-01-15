Class MainWindow
    Inherits Window

    Protected Const kStartingAssets = 2.00
    Protected Const kSignCost = 0.15
    Protected Const kNewDayPage = 0
    Protected Const kResultsPage = 1
    Protected Const kP9 = 10
    Protected Const kS2 = 30
    Protected Const kC9 = 0.5
    Protected Const kC2 = 1
    Protected Const kWeatherSunny = 2
    Protected Const kWeatherHot = 7
    Protected Const kWeatherCloudy = 10
    Protected Const kWeatherStorm = 5

    MainWindow.Open:
    Sub Open() Handles Event
        InitGame 1
        StartNewDay
    End Sub

    MainWindow.StartNewDay:
    Protected Sub StartNewDay()
        Day = Day + 1
        MakeWeather
        WeatherTitleTxt.text = "Weather Report for Day " + str(Day)
        CostLabelTxt.text = "On day " + str(Day) + ", the cost of lemonade is:"

        Dim specialEvent As String = DoRandomEvents

        if specialEvent <> "" then
            WeatherTxt.text = WeatherTxt.text + EndOfLine + specialEvent
        end if

        ShowDecisionPage
    End Sub

    MainWindow.InitGame:
    Protected Sub InitGame(qty As Integer)
        QtyPlayers = qty
        Redim Assets( QtyPlayers - 1 )
        Redim GlassesMade( QtyPlayers - 1 )
        Redim GlassesSold( QtyPlayers - 1 )
        Redim PricePerGlass( QtyPlayers - 1 )
        Redim SignsMade( QtyPlayers - 1 )
        Dim i As Integer
        for i = 0 to QtyPlayers-1
            Assets(i) = kStartingAssets
        next
        WeatherFactor = 1.0
    End Sub


    MainWindow.CalculateResults:
    Protected Sub CalculateResults()
        // calculate how many glasses are sold
        Dim specialResult As String
        Dim N1 As Double

        if pricePerGlass( CurPlayer ) >= kP9 then
            N1 = ((kP9 ^ 2) * kS2 / pricePerGlass( CurPlayer ) ^ 2 )
        else
            N1 = (kP9 - pricePerGlass( CurPlayer )) / kP9 * 0.8 * kS2 + kS2
        end if

        Dim W As Double
        W = -signsMade( CurPlayer ) * kC9

        Dim adBenefit As Double // % increase in sales due to ads
        adBenefit = 1 - ( Exp(W) * kC2 )

        Dim N2 As Double
        N2 = Floor( WeatherFactor * N1 * (1 + adBenefit) )

        if StormBrewing then
            Weather = kWeatherStorm

            UpdateWeatherReport
            PlayThunderClap
            Player.PlaySong 100, 180, "0,8 55,2 67,3 64,1 62,2 60,1 57,6 55,2 60,4 60,1 62,2 64,1 67,4"
            N2 = 0

            if glassesMade( CurPlayer ) > 0 then
                specialResult = "All lemonade was ruined."
            end if

        elseif StreetCrewThirsty then
            N2 = glassesMade( CurPlayer )
            specialResult = "The street crews bought all your lemonade at lunchtime!"
        end if

        GlassesSold(CurPlayer) = Min( N2, glassesMade( CurPlayer ) )

        // calculate income and expenses
        Dim expenses As Double = glassesMade( CurPlayer ) * CostPerGlass/100 + signsMade( CurPlayer ) * kSignCost
        Dim income As Double = GlassesSold(CurPlayer) * pricePerGlass( CurPlayer ) / 100
       
        
        // adjust assets
        Assets(CurPlayer) = Assets(CurPlayer) + income - expenses

        // stuff all this into the results display
        SpecialResultTxt.text = specialResult
        GlassesSoldTxt.text = str( GlassesSold(CurPlayer) )

        if GlassesSold(CurPlayer) = 1 then
            GlassesSoldLbl.text = "Glass Sold"
        else
            GlassesSoldLbl.text = "Glasses Sold"
        end if


        PricePerGlassTxt.text = DFormat( pricePerGlass( CurPlayer ) / 100 )
        SalesIncTxt.text = DFormat( GlassesSold(CurPlayer) * pricePerGlass( CurPlayer ) / 100 )
        GlassesMadeTxt.text = str(glassesMade( CurPlayer ))

        if glassesMade( CurPlayer ) = 1 then
            GlassesMadeLbl.text = "Glass Made"
        else
            GlassesMadeLbl.text = "Glasses Made"
        end if

        CostPerGlassTxt.text = DFormat( CostPerGlass / 100 )
        LemonadeExpTxt.text = DFormat( glassesMade( CurPlayer ) * CostPerGlass / 100 )
        SignsMadeTxt.text = str(signsMade( CurPlayer ))

        if signsMade( CurPlayer ) = 1 then
            SignsMadeLbl.text = "Sign Made"
        else
            SignsMadeLbl.text = "Signs Made"
        end if

        CostPerSignTxt.text = DFormat( kSignCost )
        AdExpTxt.text = DFormat( signsMade( CurPlayer ) * kSignCost )
        ProfitTxt.text = DFormat( income - expenses )

        if income < expenses then
            ProfitTxt.TextColor = &c800000
        else
            ProfitTxt.TextColor = &c000000
        end if

        Profitable = income > expenses
        NewAssetsTxt.text = DFormat( Assets(CurPlayer) )
        ShowResults

    End Sub

    MainWindow.ShowResults:
    Protected Sub ShowResults()
        DayTxt.text = "Day " + str(Day)
        StandTxt.text = "Stand " + str(CurPlayer + 1)
        Panel.Value = kResultsPage

        if Profitable then
            // Note: I'm taking a small liberty here with the original design, which
            // played "we're in the money" whether you made money or not. That
            // bugged me even as a kid, so I'm changing it so that it only plays if
            // you were profitable. Also, this allows the thunderstorm music to be
            // heard, which otherwise would get overwritten with our side-by-side
            // approach to the UI.

            Player.PlaySong 4, 125, "64,2 67,3 64,1 65,2 67,4"
        end if
    End Sub


    MainWindow.DFormat:
    Protected Function DFormat(value As Double) As String
        // Format the given value as dollars and cents.
        if value < 0.00 then
            return "$-" + Format( value, "0.00" )
        else
            return "$" + Format( value, "0.00" )
        end if
    End Function


    MainWindow.MakeWeather:
    Protected Sub MakeWeather()
        Dim r As Double = Rnd
        if r < 0.6 then
            Weather = kWeatherSunny
        elseif r < 0.8 then
            Weather = kWeatherCloudy
        else
            if Day < 3 then Weather = kWeatherSunny else Weather = kWeatherHot
        end if

        ChanceOfRain = 0

        if Weather = kWeatherCloudy then
            ChanceOfRain = 30 + Floor( Rnd * 5 ) * 10
            WeatherFactor = 1.0 - ChanceOfRain / 100
            Player.PlaySong 97, 175, "64,3 64,2 64,1 65,2 64,1 62,2 60,1 64,5"
        elseif Weather = kWeatherHot then
            WeatherFactor = 2.0
            Player.PlaySong 20, 175, "69,2 67,1 69,5 67,2 65,1 67,2 69,2 65,3 62,3 57,5"
        else
            WeatherFactor = 1.0
            Player.PlaySong 76, 250, "72,3 74,1 67,1 72,1 76,1 67,1 72,5"
        end if
        UpdateWeatherReport
    End Sub


    MainWindow.UpdateWeatherReport:
    Protected Sub UpdateWeatherReport()
        Dim report As String

        select case Weather
            case kWeatherSunny
                report = "Sunny"
                WeatherCanv.Backdrop = SunnyPic
            case kWeatherCloudy
                report = "Cloudy" + EndOfLine + "There is a " + str(ChanceOfRain) + "% chance of light rain, " + "and the weather is cooler today."
                WeatherCanv.Backdrop = CloudyPic
            case kWeatherHot
                report = "Hot and Dry" + EndOfLine + "A heat wave is predicted for today!"
                WeatherCanv.Backdrop = HotAndDryPic
            case kWeatherStorm
                report = "Thunderstorms!" + EndOfLine _
                + "A severe thunderstorm hit Lemonsville earlier today, " _
                + "just as the lemonade stands were being set up. Unfortunately, " _
                + "everything was ruined!"
                WeatherCanv.Backdrop = StormPic
        end select
        
        WeatherTxt.text = report
    End Sub


    MainWindow.DoRandomEvents:
    Protected Function DoRandomEvents() As String
        Dim specialDesc As String
        StreetCrewThirsty = false
        StormBrewing = false
        if Weather = kWeatherCloudy then
            if Rnd < 0.25 then
                // thunderstorm!
                StormBrewing = true
            end if
            elseif Weather = kWeatherHot then
                // heat wave (see original source line 2410)...
                // already handled in MakeWeather
            else
            if Rnd >= 0.25 then return "" // no special event today
                // street department is working (original source line 2210)
                specialDesc = "The street department is working today. " _
                + "There will be no traffic on your street."
                if Rnd < 0.5 then
                    StreetCrewThirsty = true
                else
                    WeatherFactor = 0.1
            end if
        end if
        return specialDesc

    End Function


    MainWindow.Validate:
    Protected Sub Validate()
        // Make sure the user's inputs are reasonable.
        Dim valid As Boolean = true
        Dim glasses, signs, price As Integer
        glasses = CDbl( InpGlassFld.text )

        if glasses < 0 or glasses > 1000 then valid = false
            signs = CDbl( InpSignFld.text )

        if signs < 0 or signs > 50 then valid = false
            price = CDbl( InpPriceFld.text )

        if price < 0 or price > 100 then valid = false

        if glasses * CostPerGlass / 100 + signs * kSignCost > Assets(CurPlayer) then valid = false

        DecisionOKButn.Enabled = valid
    End Sub


    MainWindow.PlayThunderClap:
    Protected Sub PlayThunderClap()
        Dim NotePlayer1 as New NotePlayer
        NotePlayer1.Instrument = 118
        NotePlayer1.PlayNote(20,60)
        NotePlayer1.Instrument = 123
        NotePlayer1.PlayNote(21,120)
        NotePlayer1.Instrument = 122
        NotePlayer1.PlayNote(22,100)
    End Sub


    MainWindow.SetQtyPlayers:
    Sub SetQtyPlayers(newQtyPlayers As Integer)
        InitGame newQtyPlayers
    End Sub


    MainWindow.ShowDecisionPage:
    Protected Sub ShowDecisionPage()
        DecisionTitleTxt.text = "Decisions for Lemonade Stand " + str(CurPlayer+1)
        Dim explanation As String
        
        if Day < 3 then CostPerGlass = 2
        elseif Day < 7 then CostPerGlass = 4

        if Day = 3 then explanation = "(Your mother quit giving you free sugar.)"
            else
        if Day = 7 then explanation = "(The price of lemonade mix just went up.)"
            CostPerGlass = 5
        end if

        CostTxt.text = "$.0" + str(CostPerGlass)
        CostExpTxt.text = explanation
        AssetsTxt.text = "$" + Format( Assets(CurPlayer), "-0.00")
        InpGlassLabel.text = "How many glasses of lemonade (" _
        + Format( CostPerGlass, "0" ) + " cents each) do you wish to make?"
        InpSignLabel.text = "How many advertising signs (" _
        + Format( kSignCost * 100, "0" ) + " cents each) do you want to make?"
        InpGlassFld.text = CStr( GlassesMade( CurPlayer ) )
        InpSignFld.text = CStr( SignsMade( CurPlayer ) )
        InpPriceFld.text = CStr( PricePerGlass( CurPlayer ) )
        
        Validate
        Panel.Value = kNewDayPage

        InpGlassFld.SetFocus
        InpGlassFld.SelStart = 0
        InpGlassFld.SelLength = 99999

    End Sub


    MainWindow.CostPerGlass:
    CostPerGlass As Integer
    Cost of lemonade per glass, in cents.

    MainWindow.Day:
    Day As Integer
    Which day of the game we're on.


    MainWindow Control WeatherCanv:
    Sub Paint(g As Graphics) Handles Event
        g.DrawRect 0, 0, me.width, me.height
    End Sub

    MainWindow Control DecisionOKButn:
    Sub Action() Handles Event
        GlassesMade( CurPlayer ) = CDbl( InpGlassFld.text )
        SignsMade( CurPlayer ) = CDbl( InpSignFld.text )
        PricePerGlass( CurPlayer ) = CDbl( InpPriceFld.text )

        CurPlayer = CurPlayer + 1

        if CurPlayer >= QtyPlayers then
            CurPlayer = 0
            CalculateResults
            ShowResults
        else
            ShowDecisionPage
        end if

    End Sub


    MainWindow Control InpGlassFld:
    Sub TextChange() Handles Event
        Validate
    End Sub


    MainWindow Control InpSignFld:
    Sub TextChange() Handles Event
        Validate
    End Sub

    MainWindow Control InpPriceFld:
    Sub TextChange() Handles Event
        Validate
    End Sub

    MainWindow Control ResultsOKButn:
    Sub Action() Handles Event
        CurPlayer = CurPlayer + 1
        if CurPlayer >= QtyPlayers then
            CurPlayer = 0
            StartNewDay
        else
            CalculateResults
        end if
    End Sub

End Class