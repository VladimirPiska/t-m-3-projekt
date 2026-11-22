//USER STORY 22: Jako pořadatel chci, aby program kontroloval všechny vstupy, abych nemohl zadat neplatné údaje.
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