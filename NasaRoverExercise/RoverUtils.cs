using NasaRover;

namespace RoverUtils
{
    /// <summary>
    /// A cardinal direction mapped from a single character to a more readable name.
    /// </summary>
    public enum Cardinal
    {
        North = 'N',
        South = 'S',
        East = 'E',
        West = 'W',
    }

    /// <summary>
    /// A rover instruction mapped from a single character to a more readable name.
    /// </summary>
    public enum Instruction
    {
        Move = 'M',
        RotateRight = 'R',
        RotateLeft = 'L',
    }

    /// <summary>
    /// Represents the minimum set of shared functionality any rover should have.
    /// </summary>
    public interface IRover
    {
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public Dictionary<Instruction, Action> InstructionSet { get; }
        public void ExecuteInstruction(Instruction instruction);
        public void ReportLocation();
    }

    /// <summary>
    /// Represents an error caused by passing an invalid instruction to a Rover.
    /// </summary>
    public class InvalidInstructionException : Exception
    {
        public InvalidInstructionException(Instruction invalidInstruction) : 
            base($"\"{(char)invalidInstruction}\" is not part of this Rover's Instruction Set!") {}
    }

    /// <summary>
    /// Represents an error caused by two rovers trying to occupy the same coordinate at the same time.
    /// </summary>
    public class RoverCollisionException : Exception
    {
        public RoverCollisionException((int Xpos, int Ypos) collisionCoordinates) : 
            base($"Rover Collision avoided at location ({collisionCoordinates.Xpos},{collisionCoordinates.Ypos})") {}
    }

    /// <summary>
    /// Represents an error caused by a rover trying to move to out-of-bounds coordinates.
    /// </summary>
    public class RoverOutOfBoundsException : Exception
    {
        public RoverOutOfBoundsException((int Xpos, int Ypos) outOfBoundsCoordinates) : 
            base($"Rover attempted to reach out-of-bounds coordinate ({outOfBoundsCoordinates.Xpos},{outOfBoundsCoordinates.Ypos})") {}
    }
}
