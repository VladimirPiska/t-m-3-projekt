
using System.Numerics;
public class Tournament
{
    public const int MaxPlayers = 100;
    public string Name { get; set; }
    public string Discipline { get; set; }
    public int Rounds { get; set; }
    public int CurrentRound { get; set; }
    public Player[] Players { get; set; }
    public int PlayerCount { get; set; }
    public Tournament(string name, string discipline, int rounds)
    {
        Name = name;
        Discipline = discipline;
        Rounds = rounds;
        CurrentRound = 0;
        Players = new Player[MaxPlayers];
        PlayerCount = 0;
    }
}
using System;
public class TournamentService
{
    public Tournament CurrentTournament { get; set; }

    public TournamentService()
    {
        CurrentTournament = null;
    }

    public bool CreateTournament(string name, string discipline, int rounds)
    {
        if (rounds < 1)
        {
            Console.WriteLine("Pocet kol musí být alespoň 1.");
            return false;
        }

        CurrentTournament = new Tournament(name, discipline, rounds);
        Console.WriteLine("Turnaj " + name + " (" + discipline + ", " + rounds + " kol) byl založen.");
        return true;
    }
}
TournamentService tournamentService = new TournamentService();

Console.WriteLine("4 - Založit turnaj");

case "4":
    {
        Console.Write("Název turnaje: ");
        string name = Console.ReadLine();
        Console.Write("Disciplína: ");
        string discipline = Console.ReadLine();


        Console.Write("Počet kol: ");
        int rounds;
        if (!int.TryParse(Console.ReadLine(), out rounds))
        {
            Console.WriteLine("Počet kol musí být celé číslo.");
            break;
        }
        tournamentService.CreateTournament(name, discipline, rounds);
        break;
    }