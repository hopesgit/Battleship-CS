using Battleship;

Console.WriteLine("____________");
Console.WriteLine("|          |");
Console.WriteLine("|BATTLESHIP|");
Console.WriteLine("|          |");
Console.WriteLine("------------");

Game battleship = new();
battleship.Setup();
battleship.Load();