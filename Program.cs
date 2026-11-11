using System.Numerics;

 //USER STORY 8: Jako pořadatel chci vytvořit nové kolo turnaje, abych mohl pokračovat v soutěži.


public class Match
{
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }

    public Match(Player player1, Player player2)
    {
        Player1 = player1;
        Player2 = player2;
    }
}


public class Round
{
    public int Number { get; set; }
    public Match[] Matches { get; set; }
    public int MatchCount { get; set; }
    public bool IsClosed { get; set; }

    public Round(int number, int maxMatches)
    {
        Number = number;
        Matches = new Match[maxMatches];
        MatchCount = 0;
        IsClosed = false;
    }
}


public Round[] RoundList { get; set; }
    public int RoundCount { get; set; }


        RoundList = new Round[rounds];
RoundCount = 0;


public void AddRound(Round round)
{
    RoundList[RoundCount] = round;
    RoundCount++;
    CurrentRound = round.Number;
}

public Round GetRound(int roundNumber)
{
    for (int i = 0; i < RoundCount; i++)
    {
        if (RoundList[i].Number == roundNumber)
        {
            return RoundList[i];
        }
    }
    return null;
}


using System;

public class RoundService
{
    private TournamentService tournamentService;

    public RoundService(TournamentService tournamentService)
    {
        this.tournamentService = tournamentService;
    }

    public Round GetCurrentRound()
    {
        Tournament tournament = tournamentService.CurrentTournament;
        if (tournament == null || tournament.CurrentRound == 0)
        {
            return null;
        }
        return tournament.GetRound(tournament.CurrentRound);
    }

    public bool CreateRound()
    {
        Tournament tournament = tournamentService.CurrentTournament;
        if (tournament == null)
        {
            Console.WriteLine("Není otevřený žádný turnaj.");
            return false;
        }

        if (tournament.CurrentRound >= tournament.Rounds)
        {
            Console.WriteLine("Všechna kola turnaje už byla vytvořena.");
            return false;
        }

        if (tournament.PlayerCount < 2)
        {
            Console.WriteLine("Pro vytvoření kola jsou potřeba alespoň dva přihlášení hráči.");
            return false;
        }

        Round currentRound = GetCurrentRound();
        if (currentRound != null && !currentRound.IsClosed)
        {
            Console.WriteLine("Předchozí kolo ještě není uzavřené.");
            return false;
        }

        int maxMatches = (tournament.PlayerCount + 1) / 2;
        Round newRound = new Round(tournament.CurrentRound + 1, maxMatches);
        tournament.AddRound(newRound);

        Console.WriteLine("Bylo vytvořeno kolo číslo " + newRound.Number + ".");
        return true;
    }
}


RoundService roundService = new RoundService(tournamentService);


Console.WriteLine("7 - Vytvořit nové kolo");


case "7":
    roundService.CreateRound();
    break;

