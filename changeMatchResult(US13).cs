using System.Text.RegularExpressions;

//USER STORY 13: Jako pořadatel chci měnit výsledek zápasu před uzavřením kola, abych mohl opravit chybný výsledek.


public bool ChangeMatchResult(int matchNumber, MatchResult newResult)
{
    Round round = GetCurrentRound();
    if (round == null)
    {
        Console.WriteLine("Není vytvořené žádné kolo.");
        return false;
    }

    bool isClosed = round.IsClosed;
    if (isClosed)
    {
        Console.WriteLine("Kolo je už uzavřené, výsledek nelze změnit.");
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
        Console.WriteLine("Výsledek volného losu nelze měnit.");
        return false;
    }

    if (match.Result == MatchResult.NotPlayed)
    {
        Console.WriteLine("U tohoto zápasu ještě není zadaný výsledek, použij zadání výsledku.");
        return false;
    }

    if (newResult == MatchResult.NotPlayed)
    {
        Console.WriteLine("Neplatný výsledek.");
        return false;
    }

    match.SetResult(newResult);
    Console.WriteLine("Výsledek zápasu " + matchNumber + " byl změněn.");
    return true;
}


Console.WriteLine("12 - Změnit výsledek zápasu");

case "12":
    {
        roundService.PrintCurrentRound();
        Console.Write("Číslo zápasu, který chceš opravit: ");
        int matchNumber;
        if (!int.TryParse(Console.ReadLine(), out matchNumber))
        {
            Console.WriteLine("Číslo zápasu musí být celé číslo.");
            break;
        }

        Console.Write("Nový výsledek (1 = výhra prvního, 2 = výhra druhého, 3 = remíza): ");
        int newResultChoice;
        if (!int.TryParse(Console.ReadLine(), out newResultChoice))
        {
            Console.WriteLine("Výsledek musí být číslo 1 až 3.");
            break;
        }

        MatchResult result;
        if (!roundService.ParseResult(newResultChoice, out result))
        {
            Console.WriteLine("Neplatná volba výsledku.");
            break;
        }

        roundService.ChangeMatchResult(matchNumber, result);
        break;
    }
