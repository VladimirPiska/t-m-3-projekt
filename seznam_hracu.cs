
public void ShowPlayers()
{
    if (PlayerCount == 0)
    {
        Console.WriteLine("V systému nejsou žádní hráči.");
        return;
    }

    Console.WriteLine("Seznam hráčů:");
    for (int i = 0; i < PlayerCount; i++)
    {
        Console.WriteLine((i + 1) + ". " + Players[i].Nickname + " (" + Players[i].Name + ")");
    }
}

Console.WriteLine("2 - Zobrazit hráče");

case "2":
    playerService.ShowPlayers();
    break;