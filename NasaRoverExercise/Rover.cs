using RoverUtils;

namespace NasaRover
{
    /// <summary>
    /// Represents a NASA Rover
    /// </summary>
    public class Rover : IRover
    {
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public Cardinal Direction { get; set; }
        public Dictionary<Instruction, Action> InstructionSet { get; }
        private int CompassIndex;
        private readonly Cardinal[] Compass;
        private readonly Dictionary<Cardinal, (int deltaX, int deltaY)> MovementAxes;

        /// <summary>
        /// Creates a new NASA Rover with pre-defined starting conditions.
        /// </summary>
        /// <param name="xStart">The Rover's starting X position.</param>
        /// <param name="yStart">The Rover's starting Y position.</param>
        /// <param name="directionStart">The Rover's starting cardinal direction.</param>
        public Rover(int xStart, int yStart, Cardinal directionStart)
        {
            Xpos = xStart;
            Ypos = yStart;
            Direction = directionStart;
            Compass = [Cardinal.North, Cardinal.East, Cardinal.South, Cardinal.West];
            CompassIndex = Array.IndexOf(Compass, directionStart);

            // Bind a specific instruction to a specific method for the Rover object
            InstructionSet = new Dictionary<Instruction, Action>
            {
                { Instruction.Move, () => Move() },
                { Instruction.RotateLeft, () => RotateLeft() },
                { Instruction.RotateRight, () => RotateRight() },
            };

            // Map all directions to their corresponding change to the X or Y position value
            MovementAxes = new Dictionary<Cardinal, (int deltaX, int deltaY)>
            {
                { Cardinal.North, (0, 1) },
                { Cardinal.South, (0, -1) },
                { Cardinal.East, (1, 0) },
                { Cardinal.West, (-1, 0) },
            };
        }

        /// <summary>
        /// Execute a single Rover Instruction.
        /// </summary>
        /// <param name="instruction">The Instruction to be executed.</param>
        /// <exception cref="InvalidInstructionException">Thrown when the Instruction is not part of the Rover's Instruction Set.</exception>
        public void ExecuteInstruction(Instruction instruction)
        {
            if (InstructionSet.ContainsKey(instruction))
            {
                InstructionSet[instruction].Invoke();
                return;
            }

            throw new InvalidInstructionException(instruction);
        }

        /// <summary>
        /// Print out a message stating the current X,Y coordinate and cardinal direction.
        /// </summary>
        public void ReportLocation()
        {
            Console.WriteLine($"{Xpos} {Ypos} {(char)Direction}");
        }

        /// <summary>
        /// Update the Rover's X or Y position based on its cardinal direction.
        /// </summary>
        public void Move()
        {
            Xpos += MovementAxes[Direction].deltaX;
            Ypos += MovementAxes[Direction].deltaY;
        }

        /// <summary>
        /// Calculate the X and Y values that would result from a Move instruction.
        /// </summary>
        /// <returns>A tuple containing the simulated results.</returns>
        public (int newX, int newY) SimulatedMove()
        {
            return (Xpos + MovementAxes[Direction].deltaX, Ypos + MovementAxes[Direction].deltaY); 
        }

        /// <summary>
        /// Rotate the rover to the left and update its cardinal direction.
        /// </summary>
        public void RotateLeft()
        {
            // Ensure the array is circular and index values wrap back around instead of going out-of-bounds
            if (--CompassIndex < 0)
            {
                CompassIndex = Compass.Length + CompassIndex;
            }

            Direction = Compass[CompassIndex];
        }

        /// <summary>
        /// Rotate the rover to the right and update its cardinal direction.
        /// </summary>
        public void RotateRight()
        {
            // Ensure the array is circular and index values wrap back around instead of going out-of-bounds
            if (++CompassIndex >= Compass.Length)
            {
                CompassIndex = CompassIndex % Compass.Length;
            }

            Direction = Compass[CompassIndex];
        }
    }
}
