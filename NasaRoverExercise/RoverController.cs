using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoverUtils;
using NasaRover;
using System.Text.RegularExpressions;

namespace Controller
{
    internal class RoverController
    {
        private List<RoverInstructionTuple> RoverManagementList { get; set; }
        private int Xmax { get; set; }
        private int Ymax { get; set; }

        public RoverController()
        {
            RoverManagementList = new List<RoverInstructionTuple>();
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

                RoverManagementList.Add(new RoverInstructionTuple
                (
                    new Rover(startingX, startingY, startingDirection),
                    inputLines[i + 1]
                ));
            }
        }

        public void ExecuteRoverInstructions()
        {
            foreach (var roverManagement in RoverManagementList)
            {
                foreach (Instruction instruction in roverManagement.Instructions)
                {
                    roverManagement.Rover.ExecuteInstruction(instruction);
                }

                roverManagement.Rover.ReportLocation();
            }
        }
    }
}
