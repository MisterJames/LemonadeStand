Class HowManySheet
    Inherits Window

    HowManySheet.Present:
    Function Present(parentWindow As Window) As Integer
        // Present this sheet, and return how many people will be playing
        // (or 0 if the user cancels).
        InputFld.text = "1"
        InputFld.SelStart = 0
        InputFld.SelLength = 99999
        self.ShowModalWithin parentWindow
        return val( InputFld.text )
    End Function


    HowManySheet Control InputFld:
    Sub TextChange() Handles Event
        OKButn.Enabled = Val( me.text ) > 0 and Val( me.text ) <= 30
    End Sub


    HowManySheet Control CancelButn:
    Sub Action() Handles Event
        Hide
        InputFld.Text = ""
    End Sub


    HowManySheet Control OKButn:
    Sub Action() Handles Event
        Hide
    End Sub


End Class