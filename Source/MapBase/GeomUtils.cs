using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Edonica.MapBase
{
    public static class GeomUtils
    {
        /// <summary>
        /// Calculate if a list of points is in clockwise or anticlockwise sense
        /// Polygon must be closed
        /// </summary>
        /// <param name="points"></param>
        /// <returns>True if points are clockwise</returns>
        public static bool IsClockwise(IEnumerable<Point> points)
        {
            IEnumerator<Point> enumerator= points.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                throw new GeometryException("Polygon has no points.  Unable to determine if it's clockwise or not!");
            }

            double area = 0;

            Point Last = enumerator.Current;
            while (enumerator.MoveNext())
            {
                Point Next = enumerator.Current;
                area += (Next.X + Last.X) * (Next.Y - Last.Y);
                Last = Next;
            }
            return area >= 0.0;
        }

        public static IEnumerable<Point> EnsureClockwise(IEnumerable<Point> points, bool WantClockwise)
        {
            if (IsClockwise(points) == WantClockwise)
                return points;
            else
                return points.Reverse(); 
        }

        public static IEnumerable<Point> EnsureClosed(IEnumerable<Point> points)
        {
            IEnumerator<Point> enumerator = points.GetEnumerator();

            //Get the first point out...
            if (!enumerator.MoveNext())
                throw new GeometryException("Unable to ensure point list is closed - it has no points");

            Point first = enumerator.Current;
            yield return first;


            //Go through and get subsequent points out.
            Point last = first;
            while (enumerator.MoveNext())
            {
                //Only output points that are different...
                Point next = enumerator.Current;
                if (next != last)
                {
                    yield return next;
                    last = next;
                }
            }

            if (last != first)
            {
                yield return first;
            }
        }

        public static IEnumerable<Point> TidyLinearRing(IEnumerable<Point> points, bool Clockwise)
        {
            return EnsureClockwise(EnsureClosed(points), Clockwise);
        }

    }

    public class GeometryException : Exception
    {
        public GeometryException( string s ) : base ( s )
        {

        }

    }

}
