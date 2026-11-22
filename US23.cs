//USER STORY 23: Jako pořadatel chci mít jistotu, že výpis tabulky odpovídá skutečným výsledkům, abych mohl důvěřovat výpočtům programu.

private int ComputePoints(Tournament tournament, Player player)
{
    int points = 0;
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
            if (!match.ContainsPlayer(player))
            {
                continue;
            }

            if (match.IsBye)
            {
                points += WinPoints;
            }
            else if (match.Result == MatchResult.Draw)
            {
                points += DrawPoints;
            }
            else if (match.Result == MatchResult.FirstWin && match.Player1 == player)
            {
                points += WinPoints;
            }
            else if (match.Result == MatchResult.SecondWin && match.Player2 == player)
            {
                points += WinPoints;
            }
        }
    }
    return points;
}

private int ComputePlayedMatches(Tournament tournament, Player player)
{
    int played = 0;
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
            if (!match.IsBye && match.ContainsPlayer(player))
            {
                played++;
            }
        }
    }
    return played;
}

public bool VerifyScoreboard()
{
    Tournament tournament = tournamentService.CurrentTournament;
    if (tournament == null)
    {
        Console.WriteLine("Není otevřený žádný turnaj.");
        return false;
    }

    RecalculatePoints();

    bool isCorrect = true;
    for (int i = 0; i < tournament.PlayerCount; i++)
    {
        Player player = tournament.Players[i];

        int expectedPoints = ComputePoints(tournament, player);
        if (player.Points != expectedPoints)
        {
            Console.WriteLine("CHYBA: hráč " + player.Nickname + " má v tabulce " + player.Points + " b., ale podle výsledků má mít " + expectedPoints + " b.");
            isCorrect = false;
        }

        int expectedPlayed = ComputePlayedMatches(tournament, player);
        if (player.PlayedMatches != expectedPlayed)
        {
            Console.WriteLine("CHYBA: hráč " + player.Nickname + " má evidováno " + player.PlayedMatches + " zápasů, ale podle výsledků jich odehrál " + expectedPlayed + ".");
            isCorrect = false;
        }
    }
    return isCorrect;
}

if (!VerifyScoreboard())
{
    Console.WriteLine("POZOR: tabulka neodpovídá výsledkům, nespoléhej se na ni.");
}

Console.WriteLine(" 18 - Zkontrolovat správnost tabulky");

 case "18":
    {
        bool isCorrect = scoreboardService.VerifyScoreboard();
        if (isCorrect)
        {
            Console.WriteLine("Tabulka odpovídá výsledkům všech uzavřených kol.");
        }
        break;
    }