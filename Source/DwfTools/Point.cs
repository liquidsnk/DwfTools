using System;

namespace DwfTools
{
    public struct Point : IEquatable<Point>
    {
        public Point(long x, long y)
            : this()
        {
            X = x;
            Y = y;
        }

        public long X { get; private set; }

        public long Y { get; private set; }

        public static bool operator ==(Point a, Point b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Point a, Point b)
        {
            return !Equals(a, b);
        }

        public static Point Zero { get { return new Point(0, 0); } }

        public bool Equals(Point other)
        {
            if (X != other.X) return false;
            if (Y != other.Y) return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point)) return false;

            return Equals((Point)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
    }
}
