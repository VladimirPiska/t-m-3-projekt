using System.Text;
using System.Text.RegularExpressions;

//USER STORY 16: Jako pořadatel chci, aby se body automaticky přepočítávaly podle výsledků, abych nemusel počítat skóre ručně.

public const int WinPoints = 3;
public const int DrawPoints = 1;

public void RecalculatePoints()
{
    Tournament tournament = tournamentService.CurrentTournament;
    if (tournament == null)
    {
        return;
    }

    for (int i = 0; i < tournament.PlayerCount; i++)
    {
        tournament.Players[i].Points = 0;
    }

    for (int r = 0; r < tournament.RoundCount; r++)
    {
        Round round = tournament.RoundList[r];
        if (!round.IsClosed)
        {
            continue;
        }

        for (int m = 0; m < round.MatchCount; m++)
        {
            Match match = round.Matches[m];
            if (match.IsBye)
            {
                match.Player1.Points += WinPoints;
            }
            else if (match.Result == MatchResult.FirstWin)
            {
                match.Player1.Points += WinPoints;
            }
            else if (match.Result == MatchResult.SecondWin)
            {
                match.Player2.Points += WinPoints;
            }
            else if (match.Result == MatchResult.Draw)
            {
                match.Player1.Points += DrawPoints;
                match.Player2.Points += DrawPoints;
            }
        }
    }
}

RecalculatePoints();
