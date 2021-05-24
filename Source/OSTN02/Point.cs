using System;
using System.Collections.Generic;
using System.Text;

namespace Edonica.GIS
{
    /// <summary>
    /// Simple type representing a point item in an arbitrary coordinate space
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// Construct a new point
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        public Point(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
        /// <summary>
        /// X coordinate
        /// </summary>
        public double X;
        /// <summary>
        /// Y coordinate
        /// </summary>
        public double Y;
    }
}
