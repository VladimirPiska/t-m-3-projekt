using System.Numerics;

public bool RemovePlayer(string nickname)
{
    Player player = FindPlayer(nickname);
    if (player == null)
    {
        Console.WriteLine("Hráč s přezdívkou " + nickname + " neexistuje.");
        return false;
    }

    if (player.PlayedMatches > 0)
    {
        Console.WriteLine("Hráč už odehrál zápasy a nelze ho odebrat.");
        return false;
    }

    int position = 0;
    for (int i = 0; i < PlayerCount; i++)
    {
        if (Players[i] == player)
        {
            position = i;
        }
    }

    for (int j = position; j < PlayerCount - 1; j++)
    {
        Players[j] = Players[j + 1];                        // edit kontroly - proc nedeletnout hrace remove from list ale vlastne mergovat dva lidi do jednoho?
    }

    PlayerCount--;
    Players[PlayerCount] = null;

    Console.WriteLine("Hráč " + nickname + " byl odebrán.");
    return true;
}

Console.WriteLine("3 - Odebrat hráče");


case "3":
    {
        Console.Write("Přezdívka hráče: ");
        string nickname = Console.ReadLine();
        playerService.RemovePlayer(nickname);
        break;
    }
