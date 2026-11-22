
public Player FindPlayer(string nickname)
{
    if (CurrentTournament == null)
    {
        return null;
    }

    Player foundPlayer = null;
    for (int i = 0; i < CurrentTournament.PlayerCount; i++)
    {
        if (CurrentTournament.Players[i].Nickname == nickname)
        {
            foundPlayer = CurrentTournament.Players[i];
        }
    }
    return foundPlayer;
}

public bool UnregisterPlayer(string nickname)
{
    if (CurrentTournament == null)
    {
        Console.WriteLine("Není otevřený žádný turnaj.");
        return false;
    }

    if (CurrentTournament.CurrentRound > 0)
    {
        Console.WriteLine("Turnaj už začal, hráče nelze odhlásit.");
        return false;
    }

    Player player = FindPlayer(nickname);
    if (player == null)
    {
        Console.WriteLine("Hráč " + nickname + " není v turnaji přihlášený.");
        return false;
    }

    int position = 0;
    for (int i = 0; i < CurrentTournament.PlayerCount; i++)
    {
        if (CurrentTournament.Players[i] == player)
        {
            position = i;
        }
    }

    for (int j = position; j < CurrentTournament.PlayerCount - 1; j++)
    {
        CurrentTournament.Players[j] = CurrentTournament.Players[j + 1];
    }

    CurrentTournament.PlayerCount--;
    CurrentTournament.Players[CurrentTournament.PlayerCount] = null;

    Console.WriteLine("Hráč " + nickname + " byl odhlášen z turnaje.");
    return true;
}


Console.WriteLine("6 - Odhlásit hráče z turnaje");

case "6":
    {
        Console.Write("Přezdívka hráče: ");
        string nickname = Console.ReadLine();
        tournamentService.UnregisterPlayer(nickname);
        break;
    }


