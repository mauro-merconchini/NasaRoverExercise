using NasaRover;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverUtils
{
    enum Cardinal
    {
        North = 'N',
        South = 'S',
        East = 'E',
        West = 'W',
    }

    enum Instruction
    {
        Move = 'M',
        RotateRight = 'R',
        RotateLeft = 'L',
    }

    class RoverInstructionSet : Tuple<Rover, string>
    {
        public RoverInstructionSet(Rover rover, string instructionString) : base(rover, instructionString)
        {

        }

        public Rover Rover { get { return this.Item1; } }
        public string Instructions { get { return this.Item2; } }

    }
}
