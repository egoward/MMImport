using System;

namespace Edonica.MapBase
{
	public struct Rect
	{
        /// <summary>
        /// Constructor which sets the initial values to bound the two points provided.
        /// </summary>
        public Rect(Point point1,
                    Point point2)
        {
            _x = Math.Min(point1._x, point2._x);
            _y = Math.Min(point1._y, point2._y);

            //  Max with 0 to prevent double weirdness from causing us to be (-epsilon..0)
            _width = Math.Max(Math.Max(point1._x, point2._x) - _x, 0);
            _height = Math.Max(Math.Max(point1._y, point2._y) - _y, 0);
        }

        public void Union(Rect rect)
        {
            if (IsEmpty)
            {
                this = rect;
            }
            else if (!rect.IsEmpty)
            {
                double left = Math.Min(Left, rect.Left);
                double top = Math.Min(Top, rect.Top);


                // We need this check so that the math does not result in NaN
                if ((rect.Width == Double.PositiveInfinity) || (Width == Double.PositiveInfinity))
                {
                    _width = Double.PositiveInfinity;
                }
                else
                {
                    //  Max with 0 to prevent double weirdness from causing us to be (-epsilon..0)                    
                    double maxRight = Math.Max(Right, rect.Right);
                    _width = Math.Max(maxRight - left, 0);
                }

                // We need this check so that the math does not result in NaN
                if ((rect.Height == Double.PositiveInfinity) || (Height == Double.PositiveInfinity))
                {
                    _height = Double.PositiveInfinity;
                }
                else
                {
                    //  Max with 0 to prevent double weirdness from causing us to be (-epsilon..0)
                    double maxBottom = Math.Max(Bottom, rect.Bottom);
                    _height = Math.Max(maxBottom - top, 0);
                }

                _x = left;
                _y = top;
            }
        }
        public bool IsEmpty
        {
            get
            {
                return _width < 0;
            }
        }
        public Point Location
        {
            get
            {
                return new Point(_x, _y);
            }
            set
            {
                _x = value._x;
                _y = value._y;
            }
        }

        /// <summary>
        /// X - The X coordinate of the Location.
        /// If this is the empty rectangle, the value will be positive infinity.
        /// If this rect is Empty, setting this property is illegal.
        /// </summary>
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
        /// Y - The Y coordinate of the Location
        /// If this is the empty rectangle, the value will be positive infinity.
        /// If this rect is Empty, setting this property is illegal.
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

        /// <summary>
        /// Width - The Width component of the Size.  This cannot be set to negative, and will only
        /// be negative if this is the empty rectangle, in which case it will be negative infinity.
        /// If this rect is Empty, setting this property is illegal.
        /// </summary>
        public double Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        /// <summary>
        /// Height - The Height component of the Size.  This cannot be set to negative, and will only
        /// be negative if this is the empty rectangle, in which case it will be negative infinity.
        /// If this rect is Empty, setting this property is illegal.
        /// </summary>
        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        /// <summary>
        /// Left Property - This is a read-only alias for X
        /// If this is the empty rectangle, the value will be positive infinity.
        /// </summary>
        public double Left
        {
            get
            {
                return _x;
            }
        }

        /// <summary>
        /// Top Property - This is a read-only alias for Y
        /// If this is the empty rectangle, the value will be positive infinity.
        /// </summary>
        public double Top
        {
            get
            {
                return _y;
            }
        }

        /// <summary>
        /// Right Property - This is a read-only alias for X + Width
        /// If this is the empty rectangle, the value will be negative infinity.
        /// </summary>
        public double Right
        {
            get
            {
                if (IsEmpty)
                {
                    return Double.NegativeInfinity;
                }

                return _x + _width;
            }
        }

        /// <summary>
        /// Bottom Property - This is a read-only alias for Y + Height
        /// If this is the empty rectangle, the value will be negative infinity.
        /// </summary>
        public double Bottom
        {
            get
            {
                if (IsEmpty)
                {
                    return Double.NegativeInfinity;
                }

                return _y + _height;
            }
        }

        /// <summary>
        /// TopLeft Property - This is a read-only alias for the Point which is at X, Y
        /// If this is the empty rectangle, the value will be positive infinity, positive infinity.
        /// </summary>
        public Point TopLeft
        {
            get
            {
                return new Point(Left, Top);
            }
        }

        /// <summary>
        /// TopRight Property - This is a read-only alias for the Point which is at X + Width, Y
        /// If this is the empty rectangle, the value will be negative infinity, positive infinity.
        /// </summary>
        public Point TopRight
        {
            get
            {
                return new Point(Right, Top);
            }
        }

        /// <summary>
        /// BottomLeft Property - This is a read-only alias for the Point which is at X, Y + Height
        /// If this is the empty rectangle, the value will be positive infinity, negative infinity.
        /// </summary>
        public Point BottomLeft
        {
            get
            {
                return new Point(Left, Bottom);
            }
        }

        /// <summary>
        /// BottomRight Property - This is a read-only alias for the Point which is at X + Width, Y + Height
        /// If this is the empty rectangle, the value will be negative infinity, negative infinity.
        /// </summary>
        public Point BottomRight
        {
            get
            {
                return new Point(Right, Bottom);
            }
        }

        public void Union(Point point)
        {
            Union(new Rect(point, point));
        }

        internal double _x;
        internal double _y;
        internal double _width;
        internal double _height;
    }


}