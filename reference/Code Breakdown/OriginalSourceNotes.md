# Original Source Notes

These were the notes that were made by codenautics.com/lemonade in the port of the 1979 code, listed at `OriginalSource.bas` in this directory.

## Subroutines & Branch Points
```
300 REM START OF GAME
400 REM WEATHER REPORT
500 REM START OF NEW DAY
600 REM CURRENT EVENTS (and get player inputs)
1100: clear screen and go to next player
1105: compute results
2000 REM RANDOM EVENTS
3000: ends the app (not used)
4000: convert STI (number) to STI$ (dollars and cents string)
5000: display results for lemonade stand I
10000 REM INITIALIZE
11000 REM INTRODUCTION (title animation)
11700: play music (defined at the current DATA position)
12000 REM TITLE PAGE (text intro)
13000 REM NEW BUSINESS (print instructions)
14000 REM CONTINUE OLD GAME
15000 REM WEATHER DISPLAY
16000: set up machine language music player
17000: if sc = 5 then ... display thunder & lightning
18000 VTAB 24: PRINT " PRESS SPACE TO CONTINUE, ESC TO END...";
```

## Variables
```
A(i): Assets (cash on hand, in dollars)
C: cost of lemonade per glass, in cents
G(i): normally 1; 0 if everything is ruined by thunderstorm
H(i): apparently intended to relate to storms, but never assigned a value
I: current player number, 1 to N
L(i): number of glasses of lemonade made by player i
N: number of players
P(i): Price charged for lemonade, per glass, in cents
R1: weather factor; 1 for good weather, 0>R<1 for poor weather;
also adjusts traffic for things like street crews working
R2: set to 2 half the time when street department is working;
indicates that street crew bought all lemonade at lunch
R3: always equal to 0; not used
S(i): Number of signs made by player i
S3: cost per advertising sign, in dollars
SC: sky color (2=sunny, 5=thunderstorms, 7=hot & dry, 10=cloudy).
X1: set to 1 when it's cloudy; not sure what the intent was, but has no actual effect since line 2100 is unreachable.
X2: set to 1 when the street crew has worked and was not thirsty;  apparently intended to prevent street crews from coming again, but doesn't actually work since line 2200 is unreachable.
```
