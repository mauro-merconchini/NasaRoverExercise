using Microsoft.VisualStudio.TestTools.UnitTesting;
using NasaRover;
using RoverUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasaRover.Tests
{
    [TestClass()]
    public class RoverTests
    {
        [TestMethod]
        public void Constructor_InitializesPositionAndDirectionCorrectly()
        {
            // Arrange
            int xStart = 1;
            int yStart = 2;
            Cardinal directionStart = Cardinal.East;

            // Act
            Rover rover = new Rover(xStart, yStart, directionStart);

            // Assert
            Assert.AreEqual(xStart, rover.Xpos);
            Assert.AreEqual(yStart, rover.Ypos);
            Assert.AreEqual(directionStart, rover.Direction);
        }

        [TestMethod]
        public void Move_UpdatesPositionCorrectly()
        {
            // Arrange
            Rover rover = new Rover(1, 1, Cardinal.North);

            // Act
            rover.Move();

            // Assert
            Assert.AreEqual(1, rover.Xpos);
            Assert.AreEqual(2, rover.Ypos);
        }

        [TestMethod]
        public void RotateLeft_UpdatesDirectionCorrectly()
        {
            // Arrange
            Rover rover = new Rover(1, 1, Cardinal.North);

            // Act
            rover.RotateLeft();

            // Assert
            Assert.AreEqual(Cardinal.West, rover.Direction);
        }

        [TestMethod]
        public void RotateRight_UpdatesDirectionCorrectly()
        {
            // Arrange
            Rover rover1 = new Rover(1, 1, Cardinal.North);
            Rover rover2 = new Rover(1, 1, Cardinal.North);

            // Act
            rover1.RotateRight();
            rover2.RotateLeft();

            // Assert
            Assert.AreEqual(Cardinal.East, rover1.Direction);
            Assert.AreEqual(Cardinal.West, rover2.Direction);
        }

        [TestMethod]
        public void ExecuteInstruction_Move_UpdatesPositionCorrectly()
        {
            // Arrange
            Rover rover = new Rover(1, 1, Cardinal.North);

            // Act
            rover.ExecuteInstruction(Instruction.Move);

            // Assert
            Assert.AreEqual(1, rover.Xpos);
            Assert.AreEqual(2, rover.Ypos);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidInstructionException))]
        public void ExecuteInstruction_InvalidInstruction_ThrowsException()
        {
            // Arrange
            Rover rover = new Rover(1, 1, Cardinal.North);

            // Act
            rover.ExecuteInstruction((Instruction)99); // Using an invalid instruction

            // Assert: Expecting an exception
        }

        [TestMethod]
        public void SimulatedMove_ReturnsCorrectValues()
        {
            // Arrange
            Rover rover = new Rover(1, 1, Cardinal.North);

            // Act
            var result = rover.SimulatedMove();

            // Assert
            Assert.AreEqual(1, result.newX);
            Assert.AreEqual(2, result.newY);
        }

        [TestMethod]
        public void RotateFourTimes_ReturnsToOriginalDirection()
        {
            // Arrange
            Rover roverLeft = new Rover(1, 1, Cardinal.North);
            Rover roverRight = new Rover(1, 1, Cardinal.North);

            // Act
            roverLeft.RotateLeft();
            roverLeft.RotateLeft();
            roverLeft.RotateLeft();
            roverLeft.RotateLeft();

            roverRight.RotateRight();
            roverRight.RotateRight();
            roverRight.RotateRight();
            roverRight.RotateRight();

            // Assert
            Assert.AreEqual(Cardinal.North, roverLeft.Direction);
            Assert.AreEqual(Cardinal.North, roverRight.Direction);
        }
    }
}