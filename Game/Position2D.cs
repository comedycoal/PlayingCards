using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingCards.Game
{
	public struct Position2D
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Position2D(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Position2D(Tuple<int, int> pair)
		{
			X = pair.Item1;
			Y = pair.Item2;
		}

		public Position2D(string str, char delimiter=',')
		{
			var a = str.Split(delimiter);
			X = Int32.Parse(a[0]);
			Y = Int32.Parse(a[1]);
		}

		public Position2D Flip()
		{
			var a = this;
			var temp = a.X;
			a.X = a.Y;
			a.Y = temp;
			return a;
		}

		public override bool Equals(object obj)
		{
			return obj is Position2D d &&
				   X == d.X &&
				   Y == d.Y;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y);
		}

		public static Position2D operator +(Position2D a, Position2D b)
		{
			return new Position2D(a.X + b.X, a.Y + b.Y);
		}

		public static Position2D operator -(Position2D a, Position2D b)
		{
			return new Position2D(a.X - b.X, a.Y - b.Y);
		}
		public static Position2D operator *(Position2D a, int b)
		{
			return new Position2D(a.X * b, a.Y * b);
		}

		public static bool operator ==(Position2D a, Position2D b)
		{
			return a.X == b.Y && a.Y == b.Y;
		}

		public static bool operator !=(Position2D a, Position2D b)
		{
			return !(a == b);
		}
	}
}
