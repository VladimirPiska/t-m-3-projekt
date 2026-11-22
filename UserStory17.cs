using System.Numerics;

//USER STORY 17: Jako pořadatel chci vidět počet odehraných zápasů každého hráče, abych měl přehled o jeho účasti.

public void ShowPlayedMatches()
{
    Tournament tournament = tournamentService.CurrentTournament;
    if (tournament == null)
    {
        Console.WriteLine("Není otevřený žádný turnaj.");
        return;
    }

    if (tournament.PlayerCount == 0)
    {
        Console.WriteLine("V turnaji nejsou přihlášení žádní hráči.");
        return;
    }

    Console.WriteLine("Počet odehraných zápasů (uzavřená kola, bez volného losu):");
    for (int i = 0; i < tournament.PlayerCount; i++)
    {
        Player player = tournament.Players[i];
        Console.WriteLine(player.Nickname + ": " + player.PlayedMatches);
    }
}


Console.WriteLine("15 - Zobrazit odehrané zápasy hráčů");

case "15":
    {
        scoreboardService.ShowPlayedMatches();
        break;
    }
    
