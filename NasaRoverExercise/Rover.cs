using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoverUtils;

namespace NasaRover
{
    internal class Rover
    {
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public Cardinal Direction { get; set; }
        private int CompassIndex {  get; set; }
        private Cardinal[] Compass = { Cardinal.North, Cardinal.East, Cardinal.South, Cardinal.West };

        public Rover(int xStart, int yStart, Cardinal directionStart) 
        {
            Xpos = xStart;
            Ypos = yStart;
            Direction = directionStart;
            CompassIndex = Array.IndexOf(Compass, directionStart);
        }

        public void Move()
        {
            switch (Direction) 
            {
                case Cardinal.North:
                    Ypos++;
                    break;

                case Cardinal.South:
                    Ypos--;
                    break;

                case Cardinal.East:
                    Xpos++;
                    break;
            
                case Cardinal.West:
                    Xpos--;
                    break;
            }
        }

        public void RotateLeft()
        {
            // Ensure the array is circular and index values wrap back around instead of going out-of-bounds
            if (--CompassIndex < 0)
            {
                CompassIndex = Compass.Length + CompassIndex;
            }

            Direction = Compass[CompassIndex];
        }

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
