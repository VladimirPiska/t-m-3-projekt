public class Player
{
    public string Name { get; set; }
    public string Nickname { get; set; }
    public int PlayedMatches { get; set; }

    public Player(string name, string nickname)
    {
        Name = name;
        Nickname = nickname;
        PlayedMatches = 0;
    }
}

using System;

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
}


using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        PlayerService playerService = new PlayerService();

        bool running = true;
        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("1 - Přidat hráče");
            Console.WriteLine("0 - Konec");
            Console.Write("Volba: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    {
                        Console.Write("Jméno hráče: ");
                        string name = Console.ReadLine();
                        Console.Write("Přezdívka: ");
                        string nickname = Console.ReadLine();
                        playerService.AddPlayer(name, nickname);
                        break;
                    }
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Neplatná volba.");
                    break;
            }
        }
    }
}