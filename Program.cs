using System.Numerics;

//USER STORY 10: Jako pořadatel chci, aby každý hráč mohl být v jednom kole pouze jednou, abych zabránil duplicitním zápasům.


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
