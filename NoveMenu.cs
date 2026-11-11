using System.Reflection;

Console.WriteLine("    ---MENU---" +
    "\n 1 - Nový turnaj" +
    "\n 2 - Načíst existující turnaj");
char znak = Console.ReadKey().KeyChar;

switch (znak)
{
    case '1':
        {
            Console.Clear();
            Console.WriteLine("Nový turnaj.");
            await Task.Delay(500);
            Console.Clear();
            Console.WriteLine("Nový turnaj..");
            await Task.Delay(500);
            Console.Clear();
            Console.WriteLine("Nový turnaj...");
            await Task.Delay(500);
            Console.Clear();
            Console.WriteLine("Nový turnaj.");
            await Task.Delay(500);
            Console.Clear();
            Console.WriteLine("Nový turnaj..");
            await Task.Delay(500);
            Console.Clear();
            Console.WriteLine("Nový turnaj...");
            await Task.Delay(500);
            turnaj();
            //vyvolat metodu pro zahájení nového turnaje - od Vládi
            break;
        }
        
    case '2':
        {
            //vyvolat načtení souboru starého turnaje
            break;
        }

}
void turnaj()
{
    Console.Clear();
    Console.WriteLine("    ---TURNAJ---" +
    "\n 1 - Začít nové kolo" +
    "\n 2 - Zobrazit tabulku" +
    "\n 3 - Uložit turnaj" +
    "\n 4 - Ukončit program");
    char znak = Console.ReadKey().KeyChar;
}



