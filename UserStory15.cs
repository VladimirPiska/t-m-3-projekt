using System.Numerics;
public int Points { get; set; }
using System;

//USER STORY 15: Jako pořadatel chci zobrazit průběžnou tabulku turnaje, abych viděl aktuální pořadí hráčů.

public class ScoreboardService
{
    private TournamentService tournamentService;

    public ScoreboardService(TournamentService tournamentService)
    {
        this.tournamentService = tournamentService;
    }

    public void ShowScoreboard()
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

        Player[] sorted = new Player[tournament.PlayerCount];
        for (int i = 0; i < tournament.PlayerCount; i++)
        {
            sorted[i] = tournament.Players[i];
        }

        for (int i = 0; i < sorted.Length - 1; i++)
        {
            for (int j = 0; j < sorted.Length - 1 - i; j++)
            {
                bool swap = sorted[j].Points < sorted[j + 1].Points
                    || (sorted[j].Points == sorted[j + 1].Points
                        && string.Compare(sorted[j].Nickname, sorted[j + 1].Nickname, StringComparison.Ordinal) > 0);
                if (swap)
                {
                    Player temp = sorted[j];
                    sorted[j] = sorted[j + 1];
                    sorted[j + 1] = temp;
                }
            }
        }

        Console.WriteLine("Průběžná tabulka - " + tournament.Name + " (" + tournament.Discipline + ")");
        for (int i = 0; i < sorted.Length; i++)
        {
            Console.WriteLine((i + 1) + ". " + sorted[i].Nickname + " - " + sorted[i].Points + " b.");
        }
    }
}

ScoreboardService scoreboardService = new ScoreboardService(tournamentService);

Console.WriteLine("14 - Zobrazit tabulku");

case "14":
    {
        scoreboardService.ShowScoreboard();
        break;
    } 

}
    