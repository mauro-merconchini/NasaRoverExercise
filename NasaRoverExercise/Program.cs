
using Controller;

RoverController rc = new RoverController();
rc.IngestInstructions("D:\\Programming\\.NET\\NasaRoverExercise\\NasaRoverExercise\\input.txt");
rc.ExecuteRoverInstructions();
