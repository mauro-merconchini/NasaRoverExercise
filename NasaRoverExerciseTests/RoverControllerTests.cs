using Microsoft.VisualStudio.TestTools.UnitTesting;
using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NasaRover;
using RoverUtils;

namespace Controller.Tests
{
    [TestClass()]
    public class RoverControllerTests
    {
        [TestMethod]
        public void IngestInput_ValidInput_SetsPlateauSizeAndRoverList()
        {
            // Arrange
            RoverController roverController = new RoverController();
            string input =
                "5 5\r\n" +
                "1 2 N\r\n" +
                "LMLMLMLMM\r\n" +
                "3 3 E\r\n" +
                "MMRMMRMRRM";

            // Act
            roverController.IngestInput(input);

            // Assert
            Assert.AreEqual(5, roverController.Xmax);
            Assert.AreEqual(5, roverController.Ymax);
            Assert.AreEqual(2, roverController.RoverManagementList.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IngestInput_InvalidPlateauSize_ThrowsException()
        {
            // Arrange
            RoverController roverController = new RoverController();
            string input =
                "5\r\n" +
                "1 2 N\r\n" +
                "LMLMLMLMM\r\n" +
                "3 3 E\r\n" +
                "MMRMMRMRRM";

            // Act
            roverController.IngestInput(input);

            // Assert: Expecting ArgumentException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IngestInput_InvalidRoverStartCondition_ThrowsException()
        {
            // Arrange
            RoverController roverController = new RoverController();
            string input =
                "5 5\r\n" +
                "1 2 N W\r\n" + // Invalid start condition
                "LMLMLMLMM\r\n" +
                "3 3 E\r\n" +
                "MMRMMRMRRM";

            // Act
            roverController.IngestInput(input);

            // Assert: Expecting ArgumentException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IngestInput_InvalidRoverInstructions_ThrowsException()
        {
            // Arrange
            RoverController roverController = new RoverController();
            string input =
                "5 5\r\n" +
                "1 2 N\r\n" +
                "LMLMLMLM1\r\n" + // Invalid instructions
                "3 3 E\r\n" +
                "MMRMMRMRRM";

            // Act
            roverController.IngestInput(input);

            // Assert: Expecting ArgumentException
        }

        [TestMethod]
        public void ExecuteRoverInstructions_ValidInput_OutputCorrectLocations()
        {
            // Arrange
            RoverController roverController = new RoverController();
            string input =
                "5 5\r\n" +
                "1 2 N\r\n" +
                "LMLMLMLMM\r\n" +
                "3 3 E\r\n" +
                "MMRMMRMRRM";

            // Act
            roverController.IngestInput(input);

            // Assert
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                roverController.ExecuteRoverInstructions();
                string expectedOutput =
                    "1 3 N\n" +
                    "5 1 E\n";
                Assert.AreEqual(expectedOutput, sw.ToString().Replace("\r\n", "\n"));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RoverCollisionException))]
        public void InstructionIsSafe_MoveCollidesWithOtherRover_ThrowsException()
        {
            // Arrange
            RoverController roverController = new RoverController();
            string input =
                "5 5\r\n" +
                "0 0 N\r\n" +
                "M\r\n" +
                "0 1 S\r\n" +
                "M";

            // Act
            roverController.IngestInput(input);
            roverController.ExecuteRoverInstructions();

            // Assert: Expecting RoverCollisionException
        }

        [TestMethod]
        [ExpectedException(typeof(RoverOutOfBoundsException))]
        public void InstructionIsSafe_MovePutsRoverOutOfBounds_ThrowsException()
        {
            // Arrange
            RoverController roverController = new RoverController();
            string input =
                "0 0\r\n" +
                "0 0 N\r\n" +
                "M";

            // Act
            roverController.IngestInput(input);
            roverController.ExecuteRoverInstructions();

            // Assert: Expecting RoverOutOfBoundsException
        }
    }
}