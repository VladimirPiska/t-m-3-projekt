
//USER STORY 19: Jako pořadatel chci načíst uložený turnaj, abych mohl pokračovat tam, kde jsem skončil.

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

Console.WriteLine("17 - Načíst turnaj");

case "17":
    storageService.LoadTournament(playerService, tournamentService);
    break;