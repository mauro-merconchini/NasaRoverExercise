using Controller;

RoverController rc = new RoverController();

try
{
    rc.IngestInstructions("D:\\Programming\\.NET\\NasaRoverExercise\\NasaRoverExercise\\input.txt");
    rc.ExecuteRoverInstructions();
}
catch (Exception e)
{

    Console.WriteLine("ERROR: " + e.Message);
}

