using Controller;

try
{
    string projectFolder = AppDomain.CurrentDomain.BaseDirectory;
    string filePath = Path.Combine(projectFolder, "input.txt");
    string input = File.ReadAllText(filePath);

    RoverController rc = new RoverController();
    rc.IngestInput(input);
    rc.ExecuteRoverInstructions();

    Console.ReadKey();
}
catch (Exception e)
{

    Console.WriteLine("ERROR: " + e.Message);
}

