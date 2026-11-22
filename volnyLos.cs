using System.Numerics;
using System.Text.RegularExpressions;

//USER STORY 11: Jako pořadatel chci automaticky přidělit volný los hráči bez soupeře, abych mohl správně dokončit kolo s lichým počtem hráčů.

//bacha - Volný los se bere jako výhra (v tabulce dostane hráč body za výhru), ale nezapočítává se mu odehraný zápas a nezadává se k němu výsledek.


public bool IsBye { get; set; }


public Match(Player player1)
{
    Player1 = player1;
    Player2 = null;
    IsBye = true;
}


public int CountPlayers()
{
    int count = 0;
    for (int i = 0; i < MatchCount; i++)
    {
        count++;
        if (Matches[i].Player2 != null)
        {
            count++;
        }
    }
    return count;
}


public bool AssignBye()
{
    Tournament tournament = tournamentService.CurrentTournament;
    Round currentRound = GetCurrentRound();

    if (currentRound == null)
    {
        Console.WriteLine("Není vytvořené žádné kolo.");
        return false;
    }

    if (currentRound.IsClosed)
    {
        Console.WriteLine("Kolo je už uzavřené.");
        return false;
    }

    if (tournament.PlayerCount % 2 == 0)
    {
        Console.WriteLine("Počet hráčů je sudý, volný los se nepřiděluje.");
        return false;
    }

    bool found = false;
    for (int j = 0; j < currentRound.MatchCount; j++)
    {
        if (currentRound.Matches[j].IsBye)
        {
            found = true;
        }
    }
    if (found)
    {
        Console.WriteLine("Volný los už byl v tomto kole přidělen.");
        return false;
    }

    int playersInRound = currentRound.CountPlayers();
    if (tournament.PlayerCount - playersInRound != 1)
    {
        Console.WriteLine("Volný los lze přidělit, až když zbývá jediný hráč bez soupeře.");
        return false;
    }

    Player freePlayer = null;
    for (int i = 0; i < tournament.PlayerCount; i++)
    {
        Player player = tournament.Players[i];
        bool isFree = !currentRound.ContainsPlayer(player);
        if (isFree)
        {
            freePlayer = player;
        }
    }

    Match byeMatch = new Match(freePlayer);
    currentRound.AddMatch(byeMatch);

    Console.WriteLine("Hráč " + freePlayer.Nickname + " má v tomto kole volný los.");
    return true;
}


Console.WriteLine("9 - Přidělit volný los");


case "9":
    roundService.AssignBye();
    break;

