using System.Numerics;
public bool ContainsPlayer(Player player)
{
    for (int i = 0; i < PlayerCount; i++)
    {
        if (Players[i].Nickname == player.Nickname)
        {
            return true;
        }
    }
    return false;
}

public bool RegisterPlayer(Player player)
{
    if (CurrentTournament == null)
    {
        Console.WriteLine("Není otevřený žádný turnaj.");
        return false;
    }

    if (player == null)
    {
        Console.WriteLine("Hráč s touto přezdívkou neexistuje.");
        return false;
    }

    if (CurrentTournament.CurrentRound > 0)
    {
        Console.WriteLine("Turnaj už začal, nové hráče nelze přihlašovat.");
        return false;
    }

    if (CurrentTournament.ContainsPlayer(player))
    {
        Console.WriteLine("Hráč " + player.Nickname + " už je do turnaje přihlášený.");
        return false;
    }

    if (CurrentTournament.PlayerCount >= Tournament.MaxPlayers)
    {
        Console.WriteLine("Turnaj je plný.");
        return false;
    }

    CurrentTournament.Players[CurrentTournament.PlayerCount] = player;
    CurrentTournament.PlayerCount++;

    Console.WriteLine("Hráč " + player.Nickname + " byl přihlášen do turnaje.");
    return true;
}


public bool RemovePlayer(string nickname, Tournament tournament)

    if (tournament != null && tournament.ContainsPlayer(player))
{
    Console.WriteLine("Hráč je přihlášený do turnaje, nejdřív ho odhlas.");
    return false;
}

playerService.RemovePlayer(nickname, tournamentService.CurrentTournament);

Console.WriteLine("5 - Přihlásit hráče do turnaje");

case "5":
    {
        Console.Write("Přezdívka hráče: ");
        string nickname = Console.ReadLine();
        Player player = playerService.FindPlayer(nickname);
        tournamentService.RegisterPlayer(player);
        break;
    }