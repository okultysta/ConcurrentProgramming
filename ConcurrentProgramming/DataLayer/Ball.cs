﻿using System.Drawing;

namespace DataLayer
{
    public class Ball
    {
        public double x { get; set; } // środek
        public double y { get; set; } // środek
        public double radius { get; set; } 
        public double SpeedX { get; set; } // Prędkość w osi X
        public double SpeedY { get; set; } // Prędkość w osi Y

        public Ball() { }

        public Ball(double x, double y, int radius, double speedX, double speedY)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            this.SpeedX = speedX;
            this.SpeedY = speedY;
        }

        public void Move(double TimeInterval)
        {
            this.x += SpeedX*TimeInterval;
            this.y += SpeedY*TimeInterval;
        }
    }

}
