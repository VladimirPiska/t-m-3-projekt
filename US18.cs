//USER STORY 18: Jako pořadatel chci uložit rozehraný turnaj, abych mohl program vypnout a později pokračovat.

public class StorageService
{
    public const string FilePath = "tournament.txt";

    public bool SaveTournament(PlayerService playerService, TournamentService tournamentService)
    {
        Tournament tournament = tournamentService.CurrentTournament;
        if (tournament == null)
        {
            Console.WriteLine("Není co ukládat, žádný turnaj není otevřený.");
            return false;
        }

        List<string> lines = new List<string>();

        for (int i = 0; i < playerService.PlayerCount; i++)
        {
            Player player = playerService.Players[i];
            lines.Add("PLAYER;" + player.Nickname + ";" + player.Name + ";" + player.PlayedMatches);
        }

        lines.Add("TOURNAMENT;" + tournament.Name + ";" + tournament.Discipline + ";" + tournament.Rounds + ";" + tournament.CurrentRound);

        for (int i = 0; i < tournament.PlayerCount; i++)
        {
            lines.Add("TPLAYER;" + tournament.Players[i].Nickname);
        }

        for (int r = 0; r < tournament.RoundCount; r++)
        {
            Round round = tournament.RoundList[r];
            lines.Add("ROUND;" + round.Number + ";" + (round.IsClosed ? "1" : "0"));

            for (int m = 0; m < round.MatchCount; m++)
            {
                Match match = round.Matches[m];
                string secondNickname = match.IsBye ? "-" : match.Player2.Nickname;
                lines.Add("MATCH;" + match.Player1.Nickname + ";" + secondNickname + ";" + (int)match.Result);
            }
        }

        try
        {
            File.WriteAllLines(FilePath, lines);
        }
        catch (IOException)
        {
            Console.WriteLine("Turnaj se nepodařilo uložit do souboru " + FilePath + ".");
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Do souboru " + FilePath + " nemáš oprávnění zapisovat.");
            return false;
        }

        Console.WriteLine("Turnaj byl uložen do souboru " + FilePath + ".");
        return true;
    }
}

StorageService storageService = new StorageService();

Console.WriteLine("16 - Uložit turnaj");

case "16":
    storageService.SaveTournament(playerService, tournamentService);
    break;