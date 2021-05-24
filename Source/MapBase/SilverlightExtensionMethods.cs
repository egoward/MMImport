using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Edonica.MapBase
{
    public static class SilverlightExtensionMethods
    {
        
        public static List<TOutput> ConvertAll<T,TOutput>(this IEnumerable<T> inputList, Converter<T, TOutput> converter)
        {
            List<TOutput> ret = new List<TOutput>();
            foreach( T t in inputList )
            {
                ret.Add(converter(t));
            }
            return ret;
        }

        public static bool Contains(this Rect This, Rect rect)
        {
            if (This.IsEmpty || rect.IsEmpty)
            {
                return false;
            }
            return ((((This.X <= rect.X) && (This.Y <= rect.Y)) && ((This.X + This.Width) >= (rect.X + rect.Width))) && ((This.Y + This.Height) >= (rect.Y + rect.Height)));
        }

        public static bool IntersectsWith(this Rect This, Rect rect)
        {
            if (This.IsEmpty || rect.IsEmpty)
            {
                return false;
            }
            return ((((rect.Left <= This.Right) && (rect.Right >= This.Left)) && (rect.Top <= This.Bottom)) && (rect.Bottom >= This.Top));
        }

 


 



    }
}
