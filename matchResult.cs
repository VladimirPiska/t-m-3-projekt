using System.Text.RegularExpressions;


/// USER STORY 12: Jako pořadatel chci zadávat výsledek zápasu (výhra prvního, výhra druhého nebo remíza), abych zaznamenal průběh turnaje.


public enum MatchResult
{
    NotPlayed,
    FirstWin,
    SecondWin,
    Draw
}


public MatchResult Result { get; set; }


public void SetResult(MatchResult result)
{
    Result = result;
}


public Match GetMatch(int matchNumber)
{
    if (matchNumber < 1 || matchNumber > MatchCount)
    {
        return null;
    }
    return Matches[matchNumber - 1];
}


public bool ParseResult(int choice, out MatchResult result)
{
    if (choice == 1)
    {
        result = MatchResult.FirstWin;
        return true;
    }
    if (choice == 2)
    {
        result = MatchResult.SecondWin;
        return true;
    }
    if (choice == 3)
    {
        result = MatchResult.Draw;
        return true;
    }
    result = MatchResult.NotPlayed;
    return false;
}

public bool SetMatchResult(int matchNumber, MatchResult result)
{
    Round round = GetCurrentRound();
    if (round == null)
    {
        Console.WriteLine("Není vytvořené žádné kolo.");
        return false;
    }

    if (round.IsClosed)
    {
        Console.WriteLine("Kolo je už uzavřené, výsledek nelze zadat.");
        return false;
    }

    Match match = round.GetMatch(matchNumber);
    if (match == null)
    {
        Console.WriteLine("Zápas s tímto číslem neexistuje.");
        return false;
    }

    if (match.IsBye)
    {
        Console.WriteLine("U volného losu se výsledek nezadává.");
        return false;
    }

    if (result == MatchResult.NotPlayed)
    {
        Console.WriteLine("Neplatný výsledek.");
        return false;
    }

    match.SetResult(result);
    Console.WriteLine("Výsledek zápasu " + matchNumber + " byl uložen.");
    return true;
}

public void PrintCurrentRound()
{
    Round round = GetCurrentRound();
    if (round == null)
    {
        Console.WriteLine("Není vytvořené žádné kolo.");
        return;
    }

    Console.WriteLine("Kolo " + round.Number + " z " + tournamentService.CurrentTournament.Rounds + (round.IsClosed ? " (uzavřeno)" : ""));
    if (round.MatchCount == 0)
    {
        Console.WriteLine("V kole zatím nejsou žádné zápasy.");
        return;
    }

    for (int i = 0; i < round.MatchCount; i++)
    {
        Match match = round.Matches[i];
        string resultText;
        if (match.IsBye)
        {
            resultText = "volný los";
        }
        else if (match.Result == MatchResult.FirstWin)
        {
            resultText = "výhra prvního";
        }
        else if (match.Result == MatchResult.SecondWin)
        {
            resultText = "výhra druhého";
        }
        else if (match.Result == MatchResult.Draw)
        {
            resultText = "remíza";
        }
        else
        {
            resultText = "výsledek nezadán";
        }

        string secondName = match.IsBye ? "-" : match.Player2.Nickname;                 // claude - pastenutej kod a jak se vyvarovat  eroru z volnyho losu cuz no hrac 2, ale je to proste if a else
        Console.WriteLine((i + 1) + ". " + match.Player1.Nickname + " vs " + secondName + " [" + resultText + "]");
    }
}


Console.WriteLine("10 - Zobrazit aktuální kolo");
Console.WriteLine("11 - Zadat výsledek zápasu");


case "10":
    roundService.PrintCurrentRound();
    break;
case "11":
    {
        roundService.PrintCurrentRound();
        Console.Write("Číslo zápasu: ");
        int matchNumber;
        if (!int.TryParse(Console.ReadLine(), out matchNumber))
        {
            Console.WriteLine("Číslo zápasu musí být celé číslo.");
            break;
        }

        Console.Write("Výsledek (1 = výhra prvního, 2 = výhra druhého, 3 = remíza): ");
        int resultChoice;
        if (!int.TryParse(Console.ReadLine(), out resultChoice))
        {
            Console.WriteLine("Výsledek musí být číslo 1 až 3.");
            break;
        }

        MatchResult result;   // claude - nejlepsi zpusob jak udelat vyjimku na cisla mimo possible resulty (do 3 ), byl jsem linej vymyslet
        if (!roundService.ParseResult(resultChoice, out result))
        {
            Console.WriteLine("Neplatná volba výsledku.");
            break;
        }

        roundService.SetMatchResult(matchNumber, result);
        break;
    }
