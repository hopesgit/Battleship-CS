// See https://aka.ms/new-console-template for more information
using Battleship;

Console.WriteLine("____________");
Console.WriteLine("|          |");
Console.WriteLine("|BATTLESHIP|");
Console.WriteLine("|          |");
Console.WriteLine("------------");

Game battleship = new Game();
battleship.Setup();
battleship.Load();