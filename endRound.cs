using System.Numerics;
using System.Text.RegularExpressions;

//USER STORY 14: Jako pořadatel chci uzavřít kolo až po zadání všech výsledků.



public bool AllResultsEntered()
{
    bool allResultsEntered = true;
    for (int i = 0; i < MatchCount; i++)
    {
        Match match = Matches[i];
        if (!match.IsBye && match.Result == MatchResult.NotPlayed)
        {
            allResultsEntered = false;
        }
    }
    return allResultsEntered;
}

public void Close()
{
    IsClosed = true;
}


public bool CloseRound()
{
    Tournament tournament = tournamentService.CurrentTournament;
    Round round = GetCurrentRound();

    if (round == null)
    {
        Console.WriteLine("Není vytvořené žádné kolo.");
        return false;
    }

    if (round.IsClosed)
    {
        Console.WriteLine("Kolo je už uzavřené.");
        return false;
    }

    if (round.CountPlayers() != tournament.PlayerCount)
    {
        Console.WriteLine("V kole nejsou zařazení všichni hráči (případně chybí volný los).");
        return false;
    }

    bool isComplete = round.AllResultsEntered();
    if (!isComplete)
    {
        Console.WriteLine("Nejsou zadané všechny výsledky, kolo nelze uzavřít.");
        return false;
    }

    round.Close();

    for (int i = 0; i < round.MatchCount; i++)
    {
        Match match = round.Matches[i];
        if (match.IsBye)
        {
            continue;
        }

        Player player = match.Player1;
        player.PlayedMatches++;
        player = match.Player2;
        player.PlayedMatches++;
    }

    Console.WriteLine("Kolo " + round.Number + " bylo uzavřeno.");
    if (round.Number == tournament.Rounds)
    {
        Console.WriteLine("Bylo odehráno poslední kolo, turnaj je dokončen.");
    }
    return true;
}

Console.WriteLine("13 - Uzavřít kolo");


case "13":
    roundService.CloseRound();
    break;
