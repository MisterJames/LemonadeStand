Class MusicPlayer
    Inherits Timer

    MusicPlayer.Action:
    Sub Action() Handles Event
        ContinueSong
    End Sub

    MusicPlayer.Start:
    Sub Start()
        me.Mode = Timer.ModeMultiple
    End Sub

    MusicPlayer.Stop:
    Sub Stop()
        StopLastNote
        me.Mode = Timer.ModeOff
    End Sub

    MusicPlayer.Instrument:
    Protected Sub Instrument(inst as integer)
        Instrument = inst
        Player.Instrument = Instrument
    End Sub


    MusicPlayer.ContinueSong:
    Protected Sub ContinueSong()
        // check whether the last note (or rest) isn't done yet
        if Wait > 0 then
            Wait = Wait - 1
            Return
        end if

        // check whether the song is over
        if Position >= EndPosition then
            Stop
            Return
        end if

        // stop the last note playing, if any
        StopLastNote
        // start the next note (or rest)
        if MusicArray(Position,0) = "r" then // Rest
            Wait = val(MusicArray(Position,1)) - 1 // should be -1 here, too! Good catch!
            else
            if val(MusicArray(Position,1)) > 1 then // Set note hold for specified duration
                Wait = val(MusicArray(Position,1)) - 1 // why -1 for notes, but not for rests?!? - See above
            end if

            NotePlaying = val(MusicArray(Position,0))
            Player.PlayNote NotePlaying, Velocity // Play the note
        end if

        Position = Position + 1

    End Sub


    MusicPlayer.Music:
    Protected Sub Music(music as string)
        // takes input like 67,5 r,2 where comma separates NOTE VALUE from DURATION
        // r is rest for given duration (silence)
        // 67,5 40,3 r,2 20,5 would play note 67 for 5 beats, 40 for 3, silence for 2, then 20 for 5
        Dim a(-1) as string, s(-1) as string, i as integer
        a = music.Split (" ")

        Redim MusicArray (UBound(a),1)
        for i = 0 to UBound(a)
            s = a(i).Split (",")
            MusicArray(i,0) = Trim(s(0))
            MusicArray(i,1) = Trim(s(1))
            s.Remove(0)
        next i
        EndPosition = UBound(MusicArray) + 1
    End Sub
    
    MusicPlayer.Constructor:
    Sub Constructor()
        Player = new NotePlayer
        Velocity = 60
        Period = 250
        Position = 0
        EndPosition = 0
        Instrument = 1
        Wait = 0
        Mode = Timer.ModeOff
        Enabled = true
    End Sub


    MusicPlayer.PlaySong:
    Sub PlaySong(instrument As Integer, tempo As Integer, musicDef As String)
        self.Instrument = instrument
        Period = tempo
        Music musicDef
        Position = 0
        Stop
        Start
    End Sub


    MusicPlayer.StopLastNote:
    Protected Sub StopLastNote()
        // Stop the last note playing, if any
        if NotePlaying <> 0 then
            Player.PlayNote NotePlaying, 0
            NotePlaying = 0
        end if
    End Sub


    MusicPlayer.Instrument:

End Class