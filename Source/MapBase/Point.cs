using System;

namespace Edonica.MapBase
{
    //Subset of logic in Microsoft.DotNet.Wpf, removing dependencies
    //https://github.com/dotnet/wpf/blob/main/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Point.cs

    public struct Point
	{
        public Point(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public double X
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
            }
        }

        /// <summary>
        ///     Y - double.  Default value is 0.
        /// </summary>
        public double Y
        {
            get
            {
                return _y;
            }

            set
            {
                _y = value;
            }
        }

        public static bool operator ==(Point point1, Point point2)
        {
            return point1.X == point2.X &&
                   point1.Y == point2.Y;
        }
        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1 == point2);
        }
        public static bool Equals(Point point1, Point point2)
        {
            return point1.X.Equals(point2.X) &&
                   point1.Y.Equals(point2.Y);
        }
        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Point))
            {
                return false;
            }

            Point value = (Point)o;
            return Point.Equals(this, value);
        }
        public override int GetHashCode()
        {
            // Perform field-by-field XOR of HashCodes
            return X.GetHashCode() ^
                   Y.GetHashCode();
        }





        internal double _x;
        internal double _y;



    }
}