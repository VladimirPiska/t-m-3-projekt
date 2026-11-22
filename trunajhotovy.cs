using System;
using System.Collections.Generic;
using System.IO;

public class Player
{
    public string Name { get; set; }
    public string Nickname { get; set; }
    public int PlayedMatches { get; set; }
    public int Points { get; set; }

    public Player(string name, string nickname)
    {
        Name = name;
        Nickname = nickname;
        PlayedMatches = 0;
    }
}

public class PlayerService
{
    public const int MaxPlayers = 100;

    public Player[] Players { get; set; }
    public int PlayerCount { get; set; }

    public PlayerService()
    {
        Players = new Player[MaxPlayers];
        PlayerCount = 0;
    }

    public Player FindPlayer(string nickname)
    {
        Player player = null;
        for (int i = 0; i < PlayerCount; i++)
        {
            if (Players[i].Nickname == nickname)
            {
                player = Players[i];
            }
        }
        return player;
    }

    public bool AddPlayer(string name, string nickname)
    {
        if (PlayerCount >= MaxPlayers)
        {
            Console.WriteLine("Je dosažen maximální počet hráčů.");
            return false;
        }

        if (FindPlayer(nickname) != null)
        {
            Console.WriteLine("Hráč s přezdívkou " + nickname + " už existuje.");
            return false;
        }

        Players[PlayerCount] = new Player(name, nickname);
        PlayerCount++;

        Console.WriteLine("Hráč " + nickname + " byl přidán.");
        return true;
    }

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

    public bool RemovePlayer(string nickname, Tournament tournament)
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

        if (tournament != null && tournament.ContainsPlayer(player))
        {
            Console.WriteLine("Hráč je přihlášený do turnaje, nejdřív ho odhlas.");
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
            Players[j] = Players[j + 1];
        }

        PlayerCount--;
        Players[PlayerCount] = null;

        Console.WriteLine("Hráč " + nickname + " byl odebrán.");
        return true;
    }
}

public enum MatchResult
{
    NotPlayed,
    FirstWin,
    SecondWin,
    Draw
}

public class Match
{
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public bool IsBye { get; set; }
    public MatchResult Result { get; set; }

    public Match(Player player1, Player player2)
    {
        Player1 = player1;
        Player2 = player2;
    }

    public Match(Player player1)
    {
        Player1 = player1;
        Player2 = null;
        IsBye = true;
    }

    public bool ContainsPlayer(Player player)
    {
        if (Player1 != null && Player1.Nickname == player.Nickname)
        {
            return true;
        }
        if (Player2 != null && Player2.Nickname == player.Nickname)
        {
            return true;
        }
        return false;
    }

    public void SetResult(MatchResult result)
    {
        Result = result;
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

    public void AddMatch(Match match)
    {
        Matches[MatchCount] = match;
        MatchCount++;
    }

    public bool ContainsPlayer(Player player)
    {
        for (int i = 0; i < MatchCount; i++)
        {
            if (Matches[i].ContainsPlayer(player))
            {
                return true;
            }
        }
        return false;
    }

    public int CountPlayers()
    {
        int count = 0;
        for (int i = 0; i < MatchCount; i++)
        {
            count++;
            if (Matches[i].Player2 != null)
            {
                count++;
            }
        }
        return count;
    }

    public Match GetMatch(int matchNumber)
    {
        if (matchNumber < 1 || matchNumber > MatchCount)
        {
            return null;
        }
        return Matches[matchNumber - 1];
    }

    public bool AllResultsEntered()
    {
        bool allResultsEntered = true;
        for (int i = 0; i < MatchCount; i++)
        {
            Match match = Matches[i];
            if (!match.IsBye && match.Result == MatchResult.NotPlayed)
            {
                allResultsEntered = false;
            }
        }
        return allResultsEntered;
    }

    public void Close()
    {
        IsClosed = true;
    }
}

public class Tournament
{
    public const int MaxPlayers = 100;

    public string Name { get; set; }
    public string Discipline { get; set; }
    public int Rounds { get; set; }
    public int CurrentRound { get; set; }
    public Player[] Players { get; set; }
    public int PlayerCount { get; set; }
    public Round[] RoundList { get; set; }
    public int RoundCount { get; set; }

    public Tournament(string name, string discipline, int rounds)
    {
        Name = name;
        Discipline = discipline;
        Rounds = rounds;
        CurrentRound = 0;
        Players = new Player[MaxPlayers];
        PlayerCount = 0;
        RoundList = new Round[rounds];
        RoundCount = 0;
    }

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
}

public class TournamentService
{
    public Tournament CurrentTournament { get; set; }

    public TournamentService()
    {
        CurrentTournament = null;
    }

    public bool CreateTournament(string name, string discipline, int rounds)
    {
        if (CurrentTournament != null)
        {
            Console.WriteLine("Už je otevřený turnaj " + CurrentTournament.Name + ". Může být otevřený jen jeden.");
            return false;
        }

        if (rounds < 1)
        {
            Console.WriteLine("Pocet kol musí být alespoň 1.");
            return false;
        }

        CurrentTournament = new Tournament(name, discipline, rounds);

        Console.WriteLine("Turnaj " + name + " (" + discipline + ", " + rounds + " kol) byl založen.");
        return true;
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
}

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

    public bool AddMatch(string nickname1, string nickname2)
    {
        Round currentRound = GetCurrentRound();
        if (currentRound == null)
        {
            Console.WriteLine("Není vytvořené žádné kolo.");
            return false;
        }

        if (currentRound.IsClosed)
        {
            Console.WriteLine("Kolo je už uzavřené, nelze do něj přidávat zápasy.");
            return false;
        }

        if (currentRound.MatchCount >= currentRound.Matches.Length)
        {
            Console.WriteLine("V kole už je maximální počet zápasů.");
            return false;
        }

        Player player1 = tournamentService.FindPlayer(nickname1);
        Player player2 = tournamentService.FindPlayer(nickname2);
        if (player1 == null || player2 == null)
        {
            Console.WriteLine("Některý z hráčů není přihlášený do turnaje.");
            return false;
        }

        bool isSamePlayer = player1.Nickname == player2.Nickname;
        if (isSamePlayer)
        {
            Console.WriteLine("Hráč nemůže hrát sám proti sobě.");
            return false;
        }

        bool isPlayer1Used = currentRound.ContainsPlayer(player1);
        if (isPlayer1Used)
        {
            Console.WriteLine("Hráč " + player1.Nickname + " už v tomto kole hraje.");
            return false;
        }

        bool isPlayer2Used = currentRound.ContainsPlayer(player2);
        if (isPlayer2Used)
        {
            Console.WriteLine("Hráč " + player2.Nickname + " už v tomto kole hraje.");
            return false;
        }

        Match match = new Match(player1, player2);
        currentRound.AddMatch(match);

        Console.WriteLine("Zápas " + player1.Nickname + " vs " + player2.Nickname + " byl přidán.");
        return true;
    }

    public bool AssignBye()
    {
        Tournament tournament = tournamentService.CurrentTournament;
        Round currentRound = GetCurrentRound();

        if (currentRound == null)
        {
            Console.WriteLine("Není vytvořené žádné kolo.");
            return false;
        }

        if (currentRound.IsClosed)
        {
            Console.WriteLine("Kolo je už uzavřené.");
            return false;
        }

        if (tournament.PlayerCount % 2 == 0)
        {
            Console.WriteLine("Počet hráčů je sudý, volný los se nepřiděluje.");
            return false;
        }

        bool found = false;
        for (int j = 0; j < currentRound.MatchCount; j++)
        {
            if (currentRound.Matches[j].IsBye)
            {
                found = true;
            }
        }
        if (found)
        {
            Console.WriteLine("Volný los už byl v tomto kole přidělen.");
            return false;
        }

        int playersInRound = currentRound.CountPlayers();
        if (tournament.PlayerCount - playersInRound != 1)
        {
            Console.WriteLine("Volný los lze přidělit, až když zbývá jediný hráč bez soupeře.");
            return false;
        }

        Player freePlayer = null;
        for (int i = 0; i < tournament.PlayerCount; i++)
        {
            Player player = tournament.Players[i];
            bool isFree = !currentRound.ContainsPlayer(player);
            if (isFree)
            {
                freePlayer = player;
            }
        }

        Match byeMatch = new Match(freePlayer);
        currentRound.AddMatch(byeMatch);

        Console.WriteLine("Hráč " + freePlayer.Nickname + " má v tomto kole volný los.");
        return true;
    }

    public bool ParseResult(int choice, out MatchResult result)
    {
        if (choice == 1)
        {
            result = MatchResult.FirstWin;
            return true;
        }
        if (choice == 2)
        {
            result = MatchResult.SecondWin;
            return true;
        }
        if (choice == 3)
        {
            result = MatchResult.Draw;
            return true;
        }
        result = MatchResult.NotPlayed;
        return false;
    }

    public bool SetMatchResult(int matchNumber, MatchResult result)
    {
        Round round = GetCurrentRound();
        if (round == null)
        {
            Console.WriteLine("Není vytvořené žádné kolo.");
            return false;
        }

        if (round.IsClosed)
        {
            Console.WriteLine("Kolo je už uzavřené, výsledek nelze zadat.");
            return false;
        }

        Match match = round.GetMatch(matchNumber);
        if (match == null)
        {
            Console.WriteLine("Zápas s tímto číslem neexistuje.");
            return false;
        }

        if (match.IsBye)
        {
            Console.WriteLine("U volného losu se výsledek nezadává.");
            return false;
        }

        if (result == MatchResult.NotPlayed)
        {
            Console.WriteLine("Neplatný výsledek.");
            return false;
        }

        match.SetResult(result);
        Console.WriteLine("Výsledek zápasu " + matchNumber + " byl uložen.");
        return true;
    }

    public void PrintCurrentRound()
    {
        Round round = GetCurrentRound();
        if (round == null)
        {
            Console.WriteLine("Není vytvořené žádné kolo.");
            return;
        }

        Console.WriteLine("Kolo " + round.Number + " z " + tournamentService.CurrentTournament.Rounds + (round.IsClosed ? " (uzavřeno)" : ""));
        if (round.MatchCount == 0)
        {
            Console.WriteLine("V kole zatím nejsou žádné zápasy.");
            return;
        }

        for (int i = 0; i < round.MatchCount; i++)
        {
            Match match = round.Matches[i];
            string resultText;
            if (match.IsBye)
            {
                resultText = "volný los";
            }
            else if (match.Result == MatchResult.FirstWin)
            {
                resultText = "výhra prvního";
            }
            else if (match.Result == MatchResult.SecondWin)
            {
                resultText = "výhra druhého";
            }
            else if (match.Result == MatchResult.Draw)
            {
                resultText = "remíza";
            }
            else
            {
                resultText = "výsledek nezadán";
            }

            string secondName = match.IsBye ? "-" : match.Player2.Nickname;
            Console.WriteLine((i + 1) + ". " + match.Player1.Nickname + " vs " + secondName + " [" + resultText + "]");
        }
    }

    public bool ChangeMatchResult(int matchNumber, MatchResult newResult)
    {
        Round round = GetCurrentRound();
        if (round == null)
        {
            Console.WriteLine("Není vytvořené žádné kolo.");
            return false;
        }

        bool isClosed = round.IsClosed;
        if (isClosed)
        {
            Console.WriteLine("Kolo je už uzavřené, výsledek nelze změnit.");
            return false;
        }

        Match match = round.GetMatch(matchNumber);
        if (match == null)
        {
            Console.WriteLine("Zápas s tímto číslem neexistuje.");
            return false;
        }

        if (match.IsBye)
        {
            Console.WriteLine("Výsledek volného losu nelze měnit.");
            return false;
        }

        if (match.Result == MatchResult.NotPlayed)
        {
            Console.WriteLine("U tohoto zápasu ještě není zadaný výsledek, použij zadání výsledku.");
            return false;
        }

        if (newResult == MatchResult.NotPlayed)
        {
            Console.WriteLine("Neplatný výsledek.");
            return false;
        }

        match.SetResult(newResult);
        Console.WriteLine("Výsledek zápasu " + matchNumber + " byl změněn.");
        return true;
    }

    public bool CloseRound()
    {
        Tournament tournament = tournamentService.CurrentTournament;
        Round round = GetCurrentRound();

        if (round == null)
        {
            Console.WriteLine("Není vytvořené žádné kolo.");
            return false;
        }

        if (round.IsClosed)
        {
            Console.WriteLine("Kolo je už uzavřené.");
            return false;
        }

        if (round.CountPlayers() != tournament.PlayerCount)
        {
            Console.WriteLine("V kole nejsou zařazení všichni hráči (případně chybí volný los).");
            return false;
        }

        bool isComplete = round.AllResultsEntered();
        if (!isComplete)
        {
            Console.WriteLine("Nejsou zadané všechny výsledky, kolo nelze uzavřít.");
            return false;
        }

        round.Close();

        for (int i = 0; i < round.MatchCount; i++)
        {
            Match match = round.Matches[i];
            if (match.IsBye)
            {
                continue;
            }

            Player player = match.Player1;
            player.PlayedMatches++;
            player = match.Player2;
            player.PlayedMatches++;
        }

        Console.WriteLine("Kolo " + round.Number + " bylo uzavřeno.");
        if (round.Number == tournament.Rounds)
        {
            Console.WriteLine("Bylo odehráno poslední kolo, turnaj je dokončen.");
        }
        return true;
    }
}

public class ScoreboardService
{
    public const int WinPoints = 3;
    public const int DrawPoints = 1;

    private TournamentService tournamentService;

    public ScoreboardService(TournamentService tournamentService)
    {
        this.tournamentService = tournamentService;
    }

    public void RecalculatePoints()
    {
        Tournament tournament = tournamentService.CurrentTournament;
        if (tournament == null)
        {
            return;
        }

        for (int i = 0; i < tournament.PlayerCount; i++)
        {
            tournament.Players[i].Points = 0;
        }

        for (int r = 0; r < tournament.RoundCount; r++)
        {
            Round round = tournament.RoundList[r];
            if (!round.IsClosed)
            {
                continue;
            }

            for (int m = 0; m < round.MatchCount; m++)
            {
                Match match = round.Matches[m];
                if (match.IsBye)
                {
                    match.Player1.Points += WinPoints;
                }
                else if (match.Result == MatchResult.FirstWin)
                {
                    match.Player1.Points += WinPoints;
                }
                else if (match.Result == MatchResult.SecondWin)
                {
                    match.Player2.Points += WinPoints;
                }
                else if (match.Result == MatchResult.Draw)
                {
                    match.Player1.Points += DrawPoints;
                    match.Player2.Points += DrawPoints;
                }
            }
        }
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

        RecalculatePoints();
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

        if (!VerifyScoreboard())
        {
            Console.WriteLine("POZOR: tabulka neodpovídá výsledkům, nespoléhej se na ni.");
        }
    }

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

    private int ComputePoints(Tournament tournament, Player player)
    {
        int points = 0;
        for (int r = 0; r < tournament.RoundCount; r++)
        {
            Round round = tournament.RoundList[r];
            if (!round.IsClosed)
            {
                continue;
            }

            for (int m = 0; m < round.MatchCount; m++)
            {
                Match match = round.Matches[m];
                if (!match.ContainsPlayer(player))
                {
                    continue;
                }

                if (match.IsBye)
                {
                    points += WinPoints;
                }
                else if (match.Result == MatchResult.Draw)
                {
                    points += DrawPoints;
                }
                else if (match.Result == MatchResult.FirstWin && match.Player1 == player)
                {
                    points += WinPoints;
                }
                else if (match.Result == MatchResult.SecondWin && match.Player2 == player)
                {
                    points += WinPoints;
                }
            }
        }
        return points;
    }

    private int ComputePlayedMatches(Tournament tournament, Player player)
    {
        int played = 0;
        for (int r = 0; r < tournament.RoundCount; r++)
        {
            Round round = tournament.RoundList[r];
            if (!round.IsClosed)
            {
                continue;
            }

            for (int m = 0; m < round.MatchCount; m++)
            {
                Match match = round.Matches[m];
                if (!match.IsBye && match.ContainsPlayer(player))
                {
                    played++;
                }
            }
        }
        return played;
    }

    public bool VerifyScoreboard()
    {
        Tournament tournament = tournamentService.CurrentTournament;
        if (tournament == null)
        {
            Console.WriteLine("Není otevřený žádný turnaj.");
            return false;
        }

        RecalculatePoints();

        bool isCorrect = true;
        for (int i = 0; i < tournament.PlayerCount; i++)
        {
            Player player = tournament.Players[i];

            int expectedPoints = ComputePoints(tournament, player);
            if (player.Points != expectedPoints)
            {
                Console.WriteLine("CHYBA: hráč " + player.Nickname + " má v tabulce " + player.Points + " b., ale podle výsledků má mít " + expectedPoints + " b.");
                isCorrect = false;
            }

            int expectedPlayed = ComputePlayedMatches(tournament, player);
            if (player.PlayedMatches != expectedPlayed)
            {
                Console.WriteLine("CHYBA: hráč " + player.Nickname + " má evidováno " + player.PlayedMatches + " zápasů, ale podle výsledků jich odehrál " + expectedPlayed + ".");
                isCorrect = false;
            }
        }
        return isCorrect;
    }
}

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

    public bool LoadTournament(PlayerService playerService, TournamentService tournamentService)
    {
        string[] lines = File.ReadAllLines(FilePath);

        Player[] players;
        int playerCount;
        Tournament tournament;
        ParseLines(lines, out players, out playerCount, out tournament);

        playerService.Players = players;
        playerService.PlayerCount = playerCount;
        tournamentService.CurrentTournament = tournament;

        Console.WriteLine("Turnaj " + tournament.Name + " byl načten.");
        return true;
    }

    private static Player FindInArray(Player[] array, int count, string nickname)
    {
        for (int i = 0; i < count; i++)
        {
            if (array[i].Nickname == nickname)
            {
                return array[i];
            }
        }
        return null;
    }

    private void ParseLines(string[] lines, out Player[] players, out int playerCount, out Tournament tournament)
    {
        players = new Player[PlayerService.MaxPlayers];
        playerCount = 0;
        tournament = null;
        Round round = null;
        int savedCurrentRound = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line == "")
            {
                continue;
            }

            string[] parts = line.Split(';');
            string type = parts[0];
            string place = "řádek " + (i + 1) + ": ";

            if (type == "PLAYER")
            {
                if (parts.Length != 4)
                {
                    throw new FormatException(place + "neplatný záznam hráče");
                }
                if (playerCount >= players.Length)
                {
                    throw new FormatException(place + "příliš mnoho hráčů");
                }
                if (FindInArray(players, playerCount, parts[1]) != null)
                {
                    throw new FormatException(place + "duplicitní přezdívka " + parts[1]);
                }

                int playedMatches = int.Parse(parts[3]);
                if (playedMatches < 0)
                {
                    throw new FormatException(place + "záporný počet zápasů");
                }

                Player player = new Player(parts[2], parts[1]);
                player.PlayedMatches = playedMatches;
                players[playerCount] = player;
                playerCount++;
            }
            else if (type == "TOURNAMENT")
            {
                if (parts.Length != 5 || tournament != null)
                {
                    throw new FormatException(place + "neplatný záznam turnaje");
                }

                int rounds = int.Parse(parts[3]);
                if (rounds < 1)
                {
                    throw new FormatException(place + "neplatný počet kol");
                }

                tournament = new Tournament(parts[1], parts[2], rounds);
                savedCurrentRound = int.Parse(parts[4]);
            }
            else if (type == "TPLAYER")
            {
                if (parts.Length != 2 || tournament == null)
                {
                    throw new FormatException(place + "neplatný záznam přihlášeného hráče");
                }

                Player player = FindInArray(players, playerCount, parts[1]);
                if (player == null)
                {
                    throw new FormatException(place + "neznámý hráč " + parts[1]);
                }
                if (tournament.ContainsPlayer(player))
                {
                    throw new FormatException(place + "hráč " + parts[1] + " je přihlášený dvakrát");
                }

                tournament.Players[tournament.PlayerCount] = player;
                tournament.PlayerCount++;
            }
            else if (type == "ROUND")
            {
                if (parts.Length != 3 || tournament == null)
                {
                    throw new FormatException(place + "neplatný záznam kola");
                }
                if (tournament.RoundCount >= tournament.RoundList.Length)
                {
                    throw new FormatException(place + "kol je víc, než má turnaj");
                }

                int number = int.Parse(parts[1]);
                if (number != tournament.RoundCount + 1)
                {
                    throw new FormatException(place + "kola nejdou za sebou");
                }

                round = new Round(number, (tournament.PlayerCount + 1) / 2);
                round.IsClosed = parts[2] == "1";
                tournament.AddRound(round);
            }
            else if (type == "MATCH")
            {
                if (parts.Length != 4 || round == null)
                {
                    throw new FormatException(place + "neplatný záznam zápasu");
                }
                if (round.MatchCount >= round.Matches.Length)
                {
                    throw new FormatException(place + "v kole je víc zápasů, než je možné");
                }

                Player player1 = FindInArray(tournament.Players, tournament.PlayerCount, parts[1]);
                if (player1 == null)
                {
                    throw new FormatException(place + "neznámý hráč " + parts[1]);
                }

                int resultNumber = int.Parse(parts[3]);
                if (resultNumber < 0 || resultNumber > 3)
                {
                    throw new FormatException(place + "neplatný výsledek zápasu");
                }

                Match match;
                if (parts[2] == "-")
                {
                    match = new Match(player1);
                }
                else
                {
                    Player player2 = FindInArray(tournament.Players, tournament.PlayerCount, parts[2]);
                    if (player2 == null)
                    {
                        throw new FormatException(place + "neznámý hráč " + parts[2]);
                    }
                    match = new Match(player1, player2);
                }

                match.Result = (MatchResult)resultNumber;
                round.AddMatch(match);
            }
            else
            {
                throw new FormatException(place + "neznámý typ záznamu " + type);
            }
        }

        if (tournament == null)
        {
            throw new FormatException("v souboru chybí turnaj");
        }
        if (savedCurrentRound != tournament.RoundCount)
        {
            throw new FormatException("číslo aktuálního kola nesedí s počtem kol");
        }
        tournament.CurrentRound = savedCurrentRound;
    }
}

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
                case "18":
                    {
                        bool isCorrect = scoreboardService.VerifyScoreboard();
                        if (isCorrect)
                        {
                            Console.WriteLine("Tabulka odpovídá výsledkům všech uzavřených kol.");
                        }
                        break;
                    }
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
        Console.WriteLine(" 18 - Zkontrolovat správnost tabulky");
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
        while (true)
        {
            Console.Write(prompt);
            string text = Console.ReadLine();
            if (text == null)
            {
                Environment.Exit(0);
            }

            text = text.Trim();
            if (text == "")
            {
                Console.WriteLine("Vstup nesmí být prázdný.");
                continue;
            }
            if (text.Contains(";"))
            {
                Console.WriteLine("Vstup nesmí obsahovat znak ;");
                continue;
            }
            if (text.Length > 30)
            {
                Console.WriteLine("Vstup může mít nejvýše 30 znaků.");
                continue;
            }
            return text;
        }
    }

    static int ReadNumber(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (input == null)
            {
                Environment.Exit(0);
            }

            int number;
            if (int.TryParse(input, out number) && number >= min && number <= max)
            {
                return number;
            }
            Console.WriteLine("Zadej celé číslo od " + min + " do " + max + ".");
        }
    }
}