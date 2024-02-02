using NasaRover;
using RoverUtils;
using System.Text.RegularExpressions;

namespace Controller
{
    internal class RoverController
    {
        private List<(Rover Rover, string Instructions)> RoverManagementList { get; set; }
        private int Xmax { get; set; }
        private int Ymax { get; set; }
        private HashSet<(int XRoverPos, int YRoverPos)> OccupiedCoordinates;

        public RoverController()
        {
            RoverManagementList = new List<(Rover Rover, string Instructions)>();
            OccupiedCoordinates = new HashSet<(int XRoverPos, int YRoverPos)>();
        }

        public void IngestInstructions(string instructionFilePath)
        {
            var inputLines = File.ReadLines(instructionFilePath).ToList();

            // Get the size of the plateau (maximum X and Y) from the first line
            string[] sizeTokens = inputLines.First().Split(' ');

            GetPlateauSize(inputLines.First());
            PrepareRoverManagementList(ref inputLines);
        }

        private void GetPlateauSize(string plateauSizeLine)
        {
            // Regex pattern of two integers with exactly one whitespace
            string pattern = @"^\d+\s\d+$";

            if (!Regex.IsMatch(plateauSizeLine, pattern))
            {
                throw new ArgumentException(String.Format("\"{0}\" does not match the expected plateau size format. It should be two integers separated by a space"), plateauSizeLine);
            }

            string[] sizeTokens = plateauSizeLine.Split(' ');
            Xmax = Convert.ToInt32(sizeTokens[0]);
            Ymax = Convert.ToInt32(sizeTokens[1]);
        }

        private void PrepareRoverManagementList(ref List<string> inputLines)
        {
            for (int i = 1; i < inputLines.Count(); i += 2)
            {
                string[] roverStartConditions = inputLines[i].Split(' ');
                int startingX = Convert.ToInt32(roverStartConditions[0]);
                int startingY = Convert.ToInt32(roverStartConditions[1]);
                Cardinal startingDirection = (Cardinal)Convert.ToChar(roverStartConditions[2]);

                RoverManagementList.Add((                
                    new Rover(startingX, startingY, startingDirection),
                    inputLines[i + 1]
                ));

                // Allocate the space needed for tracking rover coordinates
                OccupiedCoordinates.Add((startingX, startingY));
            }
        }

        public void ExecuteRoverInstructions()
        {
            foreach (var roverManagement in RoverManagementList)
            {
                foreach (Instruction instruction in roverManagement.Instructions)
                {
                    if (InstructionIsSafe(instruction, roverManagement.Rover))
                    {
                        roverManagement.Rover.ExecuteInstruction(instruction);
                    }
                }

                roverManagement.Rover.ReportLocation();
            }
        }

        private bool InstructionIsSafe(Instruction instruction, Rover rover)
        {
            if (instruction.Equals(Instruction.Move))
            {
                (int simX, int simY) simulatedCoordinates = rover.SimulatedMove();

                // Check that the rover is still within the bounds of the plateau
                if (simulatedCoordinates.simX > Xmax    || 
                    simulatedCoordinates.simX < 0       || 
                    simulatedCoordinates.simY > Ymax    || 
                    simulatedCoordinates.simY < 0) 
                {
                    throw new RoverOutOfBoundsException(simulatedCoordinates);
                }

                // Check for rover collisions
                if (OccupiedCoordinates.Contains(simulatedCoordinates))
                {
                    throw new RoverCollisionException(simulatedCoordinates);
                }

                OccupiedCoordinates.Remove((rover.Xpos, rover.Ypos));
                OccupiedCoordinates.Add(simulatedCoordinates);
            }

            return true;
        }
    }
}
