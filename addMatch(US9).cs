//USER STORY 9: Jako pořadatel chci ručně přiřazovat hráče do dvojic, abych mohl určit, kdo proti komu hraje.
public void AddMatch(Match match)
{
    Matches[MatchCount] = match;
    MatchCount++;
}

public bool AddMatch(string nickname1, string nickname2)
{
    Round currentRound = GetCurrentRound();
    if (currentRound == null)
    {
        Console.WriteLine("Není vytvořené žádné kolo.");
        return false;
    }

    if (currentRound.IsClosed)
    {
        Console.WriteLine("Kolo je už uzavřené, nelze do něj přidávat zápasy.");
        return false;
    }

    if (currentRound.MatchCount >= currentRound.Matches.Length)
    {
        Console.WriteLine("V kole už je maximální počet zápasů.");
        return false;
    }

    Player player1 = tournamentService.FindPlayer(nickname1);
    Player player2 = tournamentService.FindPlayer(nickname2);
    if (player1 == null || player2 == null)
    {
        Console.WriteLine("Některý z hráčů není přihlášený do turnaje.");
        return false;
    }

    Match match = new Match(player1, player2);
    currentRound.AddMatch(match);

    Console.WriteLine("Zápas " + player1.Nickname + " vs " + player2.Nickname + " byl přidán.");
    return true;
}

Console.WriteLine("8 - Přiřadit hráče do dvojice");

case "8":
{
    Console.Write("Přezdívka prvního hráče: ");
    string nickname1 = Console.ReadLine();
    Console.Write("Přezdívka druhého hráče: ");
    string nickname2 = Console.ReadLine();
    roundService.AddMatch(nickname1, nickname2);
    break;
}
