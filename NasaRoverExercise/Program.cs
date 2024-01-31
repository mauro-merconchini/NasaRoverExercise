
// See https://aka.ms/new-console-template for more information
using Controller;

Console.WriteLine("Hello, World!");

RoverController rc = new RoverController();
rc.IngestInstructions("D:\\Programming\\.NET\\NasaRoverExercise\\NasaRoverExercise\\input.txt");
rc.ExecuteRoverInstructions();
