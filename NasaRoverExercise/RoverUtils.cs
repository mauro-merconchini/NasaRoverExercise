using NasaRover;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// Represents the minimum set of shared functionality any rover could have.
    /// </summary>
    public interface IRover
    {
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public void ExecuteInstruction(Instruction instruction);
        public void ReportLocation();
    }

    /// <summary>
    /// Represents a tuple collection of a Rover and its associated instructions.
    /// </summary>
    public class RoverInstructionTuple
    {
        public Rover Rover { get; set; }
        public string Instructions { get; set; }

        /// <summary>
        /// Initializes a new tuple consisting of a Rover and its associated instructions.
        /// </summary>
        /// <param name="rover">The Rover to be managed.</param>
        /// <param name="instructions">The instructions that Rover must exeecute.</param>
        public RoverInstructionTuple(Rover rover, string instructions) 
        {
            Rover = rover;
            Instructions = instructions;
        }
    }
}
