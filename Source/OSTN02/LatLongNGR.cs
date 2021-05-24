using System;
using System.Text;
using System.Windows;

namespace Edonica.Projection
{
    /// <summary>
    /// Convenience class for converting Lat Long to OSGR coordinates
    /// There are three coordinate systems in use.  These are...
    /// 
    ///    WGS84 - Lat/Long coordinates you get from a GPS.  This can be called ETRS89
    ///    "ETRS89Grid" - Grid references directly derived from WGS84 (not what OS uses)
    ///    OSGB36 - Grid references that have been converted via OSTN02.  What OS uses.
    /// </summary>
    public class LatLongNGR
    {

        static GeoCorrectionReader OSTNData = new GeoCorrectionReader();


        /// <summary>
        /// 180 / PI - factor for converting from degrees to radians
        /// </summary>
        public const double RADIAN = 180 / Math.PI;
        
        //New definitions (from OSTN02 documentation)
        //GRS80 / WGS84
        /// <summary>major semi-axis of geoid</summary>
        public const double MAJORSEMIAXIS_WGS84 = 6378137.000;      // major semi-axis of geoid
        /// <summary>minor semi-axis of geoid</summary>
        public const double MINORSEMIAXIS_WGS84 = 6356752.3141;      // minor semi-axis of geoid

        /// <summary>major semi-axis of geoid</summary>
        public const double MAJORSEMIAXIS_AIRY1830 = 6377563.396;      // major semi-axis of geoid
        /// <summary>minor semi-axis of geoid</summary>
        public const double MINORSEMIAXIS_AIRY1830 = 6356256.910;      // minor semi-axis of geoid


        ////////////////////////////////////
        //Airy 1830
        //a in OS speak
        //public const double MAJORSEMIAXIS = 6377563.396;      // major semi-axis of geoid
        //b in OS speak
        //public const double MINORSEMIAXIS = 6356256.910;      // minor semi-axis of geoid

        //e^2 in OS speak = (a^2-b^2)/a^2
        /// <summary>Eccentricity of the ellipse</summary>
        public const double ECCENTRICITY = (MAJORSEMIAXIS_WGS84 * MAJORSEMIAXIS_WGS84 - MINORSEMIAXIS_WGS84 * MINORSEMIAXIS_WGS84) / (MAJORSEMIAXIS_WGS84 * MAJORSEMIAXIS_WGS84);

        //F0 in OS speak, not present in old DMFiler
        /// <summary>Scale factor</summary>
        public const double SCALE_FACTOR = 0.9996012717;       // scale factor

        ///<summary>Latitude of true origin Phi0 in OS speak</summary>
        public const double LAT_ORIGIN = (49.0 / RADIAN);    // latitude of true origin
        ///<summary>Longitude of true origin Lamda0 in OS speak</summary>
        public const double LON_ORIGIN    = (-2.0/RADIAN);    // longitude of true origin
        ///<summary>Noth of true origin N0 in OS speak</summary>
        public const double NORTH_ORIGIN = (-100000);        // northing of true origin
        ///<summary>Eastng of true origin E0 in OS speak</summary>
        public const double EAST_ORIGIN = (400000);         // easting of true origin
        ///<summary>Minor semi-axis of geoid - n in OS speak</summary>
        public const double KGEO = ((MAJORSEMIAXIS_WGS84-MINORSEMIAXIS_WGS84)/(MAJORSEMIAXIS_WGS84+MINORSEMIAXIS_WGS84));

        /// <summary>
        /// calculate radius of curvature of a meridian at latitude phi
        /// </summary>
        /// <param name="phi"></param>
        /// <returns></returns>
        internal static double curpv(double phi)
        {
            // calculate radius of curvature of a meridian at latitude phi

            //From OSTN02 docs
            return((MAJORSEMIAXIS_WGS84*SCALE_FACTOR)/(Math.Sqrt(1.0-ECCENTRICITY*Math.Sin(phi)*Math.Sin(phi))));
        }

        /// <summary>
        /// calculate radius of curvature in meridian
        /// v is radius of curvature in prime vertical
        /// </summary>
        /// <param name="v">radius of curvature in prime vertical</param>
        /// <param name="lat">latitude</param>
        /// <returns></returns> 
        public static double curmed(double v, double lat)
        {
            return (MAJORSEMIAXIS_WGS84 * SCALE_FACTOR * (1.0 - ECCENTRICITY) * Math.Pow((1.0 - ECCENTRICITY * Math.Sin(lat) * Math.Sin(lat)), -1.5));
        }

        /// <summary>
        /// calculate nu squared
        /// </summary>
        /// <param name="v">radius of curvature in prime vertical</param>
        /// <param name="r">radius of curvature in meridian</param>
        /// <returns></returns>
        public static double nusqur(double v, double r)
        {
            // calculate nu squared
            // v is radius of curvature in prime vertical
            // r is radius of curvature in meridian

            return ((v/r)-1.0);
        }

        /// <summary>
        /// calculate arc of meridian
        /// </summary>
        /// <param name="latsum">sum of latitude and latitude of true origin</param>
        /// <param name="latdif">difference between latitude and latitude of true origin</param>
        /// <returns></returns>
        public static double arcmer(double latsum, double latdif)
        {
            double var1,var2,var3,var4;
            // calculate arc of meridian
            // latdif is difference between latitude and latitude of true origin (l+l0)
            // latsum is sum of latitude and latitude of true origin (l-l0)

            var1=(1.0+KGEO+1.25*KGEO*KGEO+1.25*KGEO*KGEO*KGEO)*latdif;
            var2=(3.0*KGEO+3.0*KGEO*KGEO+2.625*KGEO*KGEO*KGEO)*Math.Sin(latdif)*Math.Cos(latsum);
            var3=(1.875*KGEO*KGEO+1.875*KGEO*KGEO*KGEO)*Math.Sin(latdif+latdif)*
                 Math.Cos(latsum+latsum);
            var4=((35.0/24.0)*KGEO*KGEO*KGEO)*Math.Sin(latdif+latdif+latdif)*
                 Math.Cos(latsum+latsum+latsum);
            return (MINORSEMIAXIS_WGS84*SCALE_FACTOR*(var1-var2+var3-var4));
        }

        /// <summary>
        /// Project a point from WGS84 coordinates to the national grid.  This is a direct mathematical projection and does not product OSGB36 coordinates
        /// </summary>
        /// <param name="LatLon"></param>
        /// <returns></returns>
        public static Point ConvertWGS84ToETRS89Grid(Point LatLon)
        {
            double x, y;
            ConvertWGS84ToETRS89Grid(LatLon.Y, LatLon.X, out x, out y);
            return new Point(x,y);
        }

        /// <summary>
        /// Project a point from WGS84 coordinates to the national grid.  This is a direct mathematical projection and does not product OSGB36 coordinates
        /// </summary>
        /// <param name="dLatitude"></param>
        /// <param name="dLongitude"></param>
        /// <param name="dNgrEast"></param>
        /// <param name="dNgrNorth"></param>
        public static void ConvertWGS84ToETRS89Grid(double dLatitude, double dLongitude, out double dNgrEast, out double dNgrNorth)
        {
            double slat,clat,tlat;
            double var1,var2,var3,var3a,var4,var5,var6;
            double p,v,r,h2,m;

            // convert to radians

            dLatitude/=RADIAN;
            dLongitude/=RADIAN;
            p=dLongitude-LON_ORIGIN;

            // initial calculations

            v=curpv(dLatitude);
            r=curmed(v,dLatitude);
            h2=nusqur(v,r);
            m=arcmer(dLatitude+LAT_ORIGIN,dLatitude-LAT_ORIGIN);

            // precalculate commonly used values

            slat=Math.Sin(dLatitude);
            clat=Math.Cos(dLatitude);
            tlat=slat/clat;

            // calculate northing co-efficients

            var1=NORTH_ORIGIN+m;
            var2=v*slat*clat/2.0;
            var3=v*slat*clat*clat*clat/24.0*(5.0-(tlat*tlat)+9.0*h2);
            var3a=v*slat*clat*clat*clat*clat*clat/720.0*
                  (61.0-58.0*(tlat*tlat)+(tlat*tlat*tlat*tlat));

            // calculate easting co-efficients

            var4=v*clat;
            var5=v*clat*clat*clat/6.0*((v/r)-tlat*tlat);
            var6=v*clat*clat*clat*clat*clat/120.0*
                 (5.0-18.0*(tlat*tlat)+(tlat*tlat*tlat*tlat)+
                 14.0*h2-58.0*(tlat*tlat)*h2);

            // calculate NGR for return

            dNgrNorth=var1+p*p*var2+p*p*p*p*var3+p*p*p*p*p*p*var3a;
            dNgrEast=EAST_ORIGIN+p*var4+p*p*p*var5+p*p*p*p*p*var6;
        }

        /// <summary>
        /// compute phi primed
        /// the latitude of the foot of the perpendicular drawn from a point
        ///  on the projection to the central meridian
        /// </summary>
        /// <param name="dNgrNorth"></param>
        /// <returns></returns>
        public static double PhiPrime(double dNgrNorth)
        {
            // compute phi primed
            // the latitude of the foot of the perpendicular drawn from a point
            //  on the projection to the central meridian

            double phi,mer;

            phi=((dNgrNorth-NORTH_ORIGIN)/MAJORSEMIAXIS_WGS84)+LAT_ORIGIN;
            while(true)
            {
                mer=arcmer(phi+LAT_ORIGIN,phi-LAT_ORIGIN);
                if(Math.Abs(dNgrNorth-NORTH_ORIGIN-mer)<0.00001)
                    return phi;
                else
                    phi+=(dNgrNorth-NORTH_ORIGIN-mer)/MAJORSEMIAXIS_WGS84;
            }
        }


        /// <summary>
        /// Convert a national grid reference to a Lat/Long
        /// </summary>
        /// <param name="LatLon">LatLon</param>
        /// <returns></returns>
        public static Point ConvertETRS89GridToWGS84(Point LatLon)
        {
            double x, y;
            ConvertETRS89GridToWGS84(LatLon.X, LatLon.Y, out x, out y);
            return new Point(x,y);
        }
        //
        //  NgrToLatLon(dNgrEast,dNgrNorth,*dLongitude,*dLatitude)
        //

        internal static void ConvertETRS89GridToWGS84(double E, double N, out double dLongitude, out double dLatitude)
        {
            //This is now iterative!

            int counter = 0;

            double PhiPrime = (N - NORTH_ORIGIN) / (MAJORSEMIAXIS_WGS84 * SCALE_FACTOR) + LAT_ORIGIN;

            double m = arcmer(PhiPrime + LAT_ORIGIN, PhiPrime - LAT_ORIGIN);

            //Iteratively try values of m PhiPrime until we get one within 0.01mm
            while (N - NORTH_ORIGIN - m > 0.00001 && counter < 50)
            {
                PhiPrime = (N - NORTH_ORIGIN - m ) / (MAJORSEMIAXIS_WGS84 * SCALE_FACTOR ) + PhiPrime;
                m = arcmer(PhiPrime + LAT_ORIGIN, PhiPrime - LAT_ORIGIN);
                counter++;
            }

            double v = curpv(PhiPrime);
            double r = curmed(v, PhiPrime);
            double h2 = nusqur(v, r);

            //dLatitude = PhiPrime;
            double tlat = Math.Tan(PhiPrime);
            double clat = Math.Cos(PhiPrime);

            double var7 = tlat / (2.0 * r * v);
            double var8 = tlat * (5.0 + 3.0 * (tlat * tlat) + h2 - 9.0 * (tlat * tlat) * h2) / (24.0 * r * v * v * v);
            double var9 = tlat * (61.0 + 90.0 * (tlat * tlat) + 45.0 * (tlat * tlat * tlat * tlat)) / (720.0 * r * v * v * v * v * v);

            double var10 = 1.0 / (clat * v);
            double var11 = ((v / r) + 2.0 * (tlat * tlat)) / (6.0 * clat * v * v * v);
            double var12 = (5.0 + 28.0 * (tlat * tlat) + 24.0 * (tlat * tlat * tlat * tlat)) /
                  (clat * 120.0 * v * v * v * v * v);

            double var12a = 1.0 / (5040 * clat * v * v * v * v * v * v * v);
            var12a *= (61.0 + 662.0 * tlat * tlat + 1320 * tlat * tlat * tlat * tlat +
                   720.0 * tlat * tlat * tlat * tlat * tlat * tlat);

            double y = E - EAST_ORIGIN;
            dLongitude = LON_ORIGIN + (y * var10) - (y * y * y * var11) + (y * y * y * y * y * var12) -
                                             (y * y * y * y * y * y * y * var12a);

            dLatitude = PhiPrime - y * y * var7 + y * y * y * y * var8 - y * y * y * y * y * y * var9;

            dLongitude *= RADIAN;
            dLatitude *= RADIAN;
        }
        /// <summary>
        /// Apply the OSTN02 transformation to some coordinates.
        /// This will convert an OS ETRS84/WGS84 grid reference into an OS grid reference
        /// This shifts data recieved by a GPS into a form we can handle directly.
        /// </summary>
        /// <param name="PETRS"></param>
        /// <returns></returns>
        public static Point ConvertETRS89GridToOSGB36(Point PETRS)
        {
            int east_index = (int)Math.Floor(PETRS.X/1000);
            int north_index = (int)Math.Floor(PETRS.Y/1000);

            OSTNData.EnsureInitialised();
            if (!OSTNData.HasOSTNData)
            {
                throw new Exception("ConvertETRS89ENToOSGB - Unable to locate OSTN data file : " + OSTNData.GetOSTNFileName());
            }

            GeoCorrectionReader.CorrectionRecord R0, R1, R2, R3;

            bool bOK=true;
            bOK |= OSTNData.GetRecord(east_index, north_index, out R0);
            bOK |= OSTNData.GetRecord(east_index + 1, north_index, out R1);
            bOK |= OSTNData.GetRecord(east_index, north_index + 1, out R2);
            bOK |= OSTNData.GetRecord(east_index+1, north_index+1, out R3);

            if (!bOK)
            {
                throw new Exception("Invalid coordinates for OSTN02 conversion");
            }

            double dX = PETRS.X - east_index * 1000;
            double dY = PETRS.Y - north_index * 1000;
            double t = dX / 1000;
            double u = dY / 1000;

            double SX = (1 - t) * (1 - u) * R0.ShiftX + (t) * (1 - u) * R1.ShiftX + (1 - t) * (u) * R2.ShiftX + t * u * R3.ShiftX;
            double SY = (1 - t) * (1 - u) * R0.ShiftY + (t) * (1 - u) * R1.ShiftY + (1 - t) * (u) * R2.ShiftY + t * u * R3.ShiftY;
            double SH = (1 - t) * (1 - u) * R0.ShiftH + (t) * (1 - u) * R1.ShiftH + (1 - t) * (u) * R2.ShiftH + t * u * R3.ShiftH;

            return new Point( PETRS.X + SX , PETRS.Y + SY );
        }

        /// <summary>
        /// Inverse of the ConvertETRS89ENToOSGB operation.  This shifts data from OS grid references (OSGB36) into grid references compatible with ETRS84 / WGS84 (GPS systems)
        /// </summary>
        /// <param name="OSGB">OSGB36 Grid reference</param>
        /// <returns></returns>
        public static Point ConvertOSGB36ToETRS89Grid(Point OSGB)
        {
            Point PETRS89 = OSGB;
            Point POSGBGuess = ConvertETRS89GridToOSGB36(PETRS89);
            int counter = 0;
            while ( ( Math.Abs(POSGBGuess.X - OSGB.X) > 0.00001 || Math.Abs(POSGBGuess.Y - OSGB.Y) > 0.00001 ) && counter < 1000)
            {
                PETRS89.X += (OSGB.X - POSGBGuess.X);
                PETRS89.Y += (OSGB.Y - POSGBGuess.Y);
                POSGBGuess = ConvertETRS89GridToOSGB36(PETRS89);
                counter++;
            }
            return PETRS89;
        }

        /// <summary>
        /// Convert a point from OSGB36 coordinates (as used by OS maps) to WGS84 (as used by GPS systems)
        /// </summary>
        /// <param name="OSGB36"></param>
        /// <returns></returns>
        public static Point ConvertOSGB36ToWGS84(Point OSGB36)
        {
            Point ETRS89Grid = ConvertOSGB36ToETRS89Grid(OSGB36);
            Point WGS84 = ConvertETRS89GridToWGS84(ETRS89Grid);
            return WGS84;
        }

        /// <summary>
        /// Convert a point from WGS84 coordinates (as used by GPS systems) to OSGB36 (as used by OS maps )
        /// </summary>
        /// <param name="WGS84"></param>
        /// <returns></returns>
        public static Point ConvertWGS84ToOSGB36(Point WGS84)
        {
            Point ETRS89Grid = ConvertWGS84ToETRS89Grid(WGS84);
            Point OSGB36 = ConvertETRS89GridToOSGB36(ETRS89Grid);
            return OSGB36;
        }


        /// <summary>
        /// Determine if OSTN02 data is available.
        /// This is used in the UK to accuratly convert OS grid references to WGS84 latitude and longitude for GPS systems
        /// </summary>
        public static bool OSTNDataPresent
        {
            get
            {
                OSTNData.EnsureInitialised();
                return OSTNData.HasOSTNData;
            }
        }
    }
}
