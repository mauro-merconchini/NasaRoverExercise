using NasaRover;
using RoverUtils;
using System.Text.RegularExpressions;

namespace Controller
{
    /// <summary>
    /// Represents a controller which manages one or more rover objects.
    /// </summary>
    public class RoverController
    {
        public List<(Rover Rover, string Instructions)> RoverManagementList { get; private set; }
        public int Xmax { get; private set; }
        public int Ymax { get; private set; }
        private HashSet<(int XRoverPos, int YRoverPos)> OccupiedCoordinates;

        /// <summary>
        /// Initializes a new RoverController object.
        /// </summary>
        public RoverController()
        {
            RoverManagementList = new List<(Rover Rover, string Instructions)>();
            OccupiedCoordinates = new HashSet<(int XRoverPos, int YRoverPos)>();
        }

        /// <summary>
        /// Intake the input text file to be used by the controller.
        /// </summary>
        /// <param name="input">The contents of the input.txt file.</param>
        public void IngestInput(string input)
        {
            //var inputLines = File.ReadLines(input).ToList();
            var inputLines = input.Split(Environment.NewLine);

            // Get the size of the plateau (maximum X and Y) from the first line
            string[] sizeTokens = inputLines[0].Split(' ');

            GetPlateauSize(inputLines[0]);
            PrepareRoverManagementList(inputLines);
        }

        /// <summary>
        /// Sequentially execute each instruction for each managed rover.
        /// </summary>
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

        /// <summary>
        /// Derive the size of the plateau from the input.
        /// </summary>
        /// <param name="plateauSizeLine">The input line containing the plateau dimensions.</param>
        /// <exception cref="ArgumentException">Thrown when the input is not valid.</exception>
        private void GetPlateauSize(string plateauSizeLine)
        {
            // Regex pattern of two integers with exactly one whitespace
            string plateauSizePattern = @"^\d+\s\d+$";

            if (!Regex.IsMatch(plateauSizeLine, plateauSizePattern))
            {
                throw new ArgumentException($"\"{plateauSizeLine}\" does not match the expected plateau size format. It should be two integers separated by a space.");
            }

            string[] sizeTokens = plateauSizeLine.Split(' ');
            Xmax = Convert.ToInt32(sizeTokens[0]);
            Ymax = Convert.ToInt32(sizeTokens[1]);
        }

        /// <summary>
        /// Generate a list of tuples containing a rover and its associated instructions.
        /// </summary>
        /// <param name="inputLines">The lines of the file containing a rover's starting conditions and its instructions.</param>
        /// <exception cref="ArgumentException">Thrown when invalid data is passed.</exception>
        private void PrepareRoverManagementList(string[] inputLines)
        {
            // Regex pattern for two numbers and a direction separated by a whitespace
            string roverStartConditionsPattern = @"^\d+\s\d+\s[NSEW]$";

            // Regex pattern for a sequence of capital letters
            string roverInstructionsPattern = "[A-Z]+";

            for (int i = 1; i < inputLines.Count(); i += 2)
            {
                if (!Regex.IsMatch(inputLines[i], roverStartConditionsPattern))
                {
                    throw new ArgumentException($"\"{inputLines[i]}\" is not a valid rover start condition. It must be two numbers and a cardinal direction.");
                }

                if (!Regex.IsMatch(inputLines[i + 1], roverInstructionsPattern))
                {
                    throw new ArgumentException($"\"{inputLines[i + 1]}\" is not a valid sequence of rover instructions. It must be a string of capital letters.");
                }

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

        /// <summary>
        /// Checks if it is safe for a rover to perform an instruction.
        /// </summary>
        /// <param name="instruction">The instruction to be executed.</param>
        /// <param name="rover">The rover which will be executing the instruction.</param>
        /// <returns>Whether or not the rover is safe to proceed with the instruction execution.</returns>
        /// <exception cref="RoverOutOfBoundsException">Thrown when a rover's instruction would cause it to go outside the plateau grid.</exception>
        /// <exception cref="RoverCollisionException">Thrown when a rover's instruction would cause it to collide with another rover.</exception>
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
