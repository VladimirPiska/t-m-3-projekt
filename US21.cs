//USER STORY 21: Jako pořadatel chci ovládat program pomocí přehledného menu, abych mohl jednoduše využívat jeho funkce.

using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        PlayerService playerService = new PlayerService();
        TournamentService tournamentService = new TournamentService();
        RoundService roundService = new RoundService(tournamentService);
        ScoreboardService scoreboardService = new ScoreboardService(tournamentService);
        StorageService storageService = new StorageService();

        bool running = true;
        while (running)
        {
            ShowMenu(tournamentService);
            Console.Write("Volba: ");
            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    {
                        string name = ReadText("Jméno hráče: ");
                        string nickname = ReadText("Přezdívka: ");
                        playerService.AddPlayer(name, nickname);
                        break;
                    }
                case "2":
                    playerService.ShowPlayers();
                    break;
                case "3":
                    {
                        string nickname = ReadText("Přezdívka hráče: ");
                        playerService.RemovePlayer(nickname, tournamentService.CurrentTournament);
                        break;
                    }
                case "4":
                    {
                        string name = ReadText("Název turnaje: ");
                        string discipline = ReadText("Disciplína: ");
                        int rounds = ReadNumber("Počet kol: ", 1, 100);
                        tournamentService.CreateTournament(name, discipline, rounds);
                        break;
                    }
                case "5":
                    {
                        string nickname = ReadText("Přezdívka hráče: ");
                        Player player = playerService.FindPlayer(nickname);
                        tournamentService.RegisterPlayer(player);
                        break;
                    }
                case "6":
                    {
                        string nickname = ReadText("Přezdívka hráče: ");
                        tournamentService.UnregisterPlayer(nickname);
                        break;
                    }
                case "7":
                    roundService.CreateRound();
                    break;
                case "8":
                    {
                        string nickname1 = ReadText("Přezdívka prvního hráče: ");
                        string nickname2 = ReadText("Přezdívka druhého hráče: ");
                        roundService.AddMatch(nickname1, nickname2);
                        break;
                    }
                case "9":
                    roundService.AssignBye();
                    break;
                case "10":
                    roundService.PrintCurrentRound();
                    break;
                case "11":
                    {
                        roundService.PrintCurrentRound();
                        int matchNumber = ReadNumber("Číslo zápasu: ", 1, 1000);
                        int resultChoice = ReadNumber("Výsledek (1 = výhra prvního, 2 = výhra druhého, 3 = remíza): ", 1, 3);
                        MatchResult result;
                        if (roundService.ParseResult(resultChoice, out result))
                        {
                            roundService.SetMatchResult(matchNumber, result);
                        }
                        else
                        {
                            Console.WriteLine("Neplatná volba výsledku.");
                        }
                        break;
                    }
                case "12":
                    {
                        roundService.PrintCurrentRound();
                        int matchNumber = ReadNumber("Číslo zápasu, který chceš opravit: ", 1, 1000);
                        int newResultChoice = ReadNumber("Nový výsledek (1 = výhra prvního, 2 = výhra druhého, 3 = remíza): ", 1, 3);
                        MatchResult result;
                        if (roundService.ParseResult(newResultChoice, out result))
                        {
                            roundService.ChangeMatchResult(matchNumber, result);
                        }
                        else
                        {
                            Console.WriteLine("Neplatná volba výsledku.");
                        }
                        break;
                    }
                case "13":
                    roundService.CloseRound();
                    break;
                case "14":
                    scoreboardService.ShowScoreboard();
                    break;
                case "15":
                    scoreboardService.ShowPlayedMatches();
                    break;
                case "16":
                    storageService.SaveTournament(playerService, tournamentService);
                    break;
                case "17":
                    storageService.LoadTournament(playerService, tournamentService);
                    break;
                case "0":
                    running = false;
                    Console.WriteLine("Program se ukončuje.");
                    break;
                default:
                    Console.WriteLine("Neplatná volba, zadej číslo z menu.");
                    break;
            }

            if (running)
            {
                Pause();
            }
        }
    }

    static void ShowMenu(TournamentService tournamentService)
    {
        Console.WriteLine();
        Console.WriteLine("==========================================");
        Console.WriteLine("             TURNAJOVÝ SYSTÉM");
        Console.WriteLine("==========================================");

        Tournament tournament = tournamentService.CurrentTournament;
        if (tournament == null)
        {
            Console.WriteLine("Aktuální turnaj: žádný");
        }
        else
        {
            Console.WriteLine("Aktuální turnaj: " + tournament.Name + " (" + tournament.Discipline + "), kolo "
                + tournament.CurrentRound + "/" + tournament.Rounds + ", hráčů: " + tournament.PlayerCount);
        }

        Console.WriteLine("------------------------------------------");
        Console.WriteLine(" HRÁČI");
        Console.WriteLine("  1 - Přidat hráče");
        Console.WriteLine("  2 - Zobrazit hráče");
        Console.WriteLine("  3 - Odebrat hráče");
        Console.WriteLine(" TURNAJ");
        Console.WriteLine("  4 - Založit turnaj");
        Console.WriteLine("  5 - Přihlásit hráče do turnaje");
        Console.WriteLine("  6 - Odhlásit hráče z turnaje");
        Console.WriteLine(" KOLA A ZÁPASY");
        Console.WriteLine("  7 - Vytvořit nové kolo");
        Console.WriteLine("  8 - Přiřadit hráče do dvojice");
        Console.WriteLine("  9 - Přidělit volný los");
        Console.WriteLine(" 10 - Zobrazit aktuální kolo");
        Console.WriteLine(" 11 - Zadat výsledek zápasu");
        Console.WriteLine(" 12 - Změnit výsledek zápasu");
        Console.WriteLine(" 13 - Uzavřít kolo");
        Console.WriteLine(" TABULKA A ULOŽENÍ");
        Console.WriteLine(" 14 - Zobrazit tabulku");
        Console.WriteLine(" 15 - Zobrazit odehrané zápasy hráčů");
        Console.WriteLine(" 16 - Uložit turnaj");
        Console.WriteLine(" 17 - Načíst turnaj");
        Console.WriteLine("  0 - Konec");
        Console.WriteLine("------------------------------------------");
    }

    static void Pause()
    {
        Console.WriteLine();
        Console.Write("Pokračuj stiskem Enter...");
        Console.ReadLine();
    }

    static string ReadText(string prompt)
    {
        Console.Write(prompt);
        string text = Console.ReadLine();
        if (text == null)
        {
            text = "";
        }
        return text.Trim();
    }

    static int ReadNumber(string prompt, int min, int max)
    {
        Console.Write(prompt);
        int number;
        if (!int.TryParse(Console.ReadLine(), out number) || number < min || number > max)
        {
            Console.WriteLine("Neplatné číslo.");
            return -1;
        }
        return number;
    }
}