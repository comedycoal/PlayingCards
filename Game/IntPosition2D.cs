using System;

namespace PlayingCards.Game
{
	/// <summary>
	/// Struct representing an integral position in the 2D plane.
	/// </summary>
	public struct IntPosition2D
	{
		/// <summary>
		/// Exposes the X part of the coordinate.
		/// </summary>
		public int X { get; set; }

		/// <summary>
		/// Exposes the Y part of the coordinate.
		/// </summary>
		public int Y { get; set; }

		/// <summary>
		/// Constructs a <see cref="IntPosition2D"/>.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordiante.</param>
		public IntPosition2D(int x, int y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Constructs a <see cref="IntPosition2D"/>.
		/// </summary>
		/// <param name="pair">A tuple with (x,y) as coordinate.</param>
		public IntPosition2D(Tuple<int, int> pair)
		{
			X = pair.Item1;
			Y = pair.Item2;
		}

		/// <summary>
		/// Constructs a <see cref="IntPosition2D"/>.
		/// </summary>
		/// <param name="str">A string comprises of x and y coordinate.</param>
		/// <param name="delimiter">The seperator between x and y.</param>
		public IntPosition2D(string str, char delimiter=',')
		{
			var a = str.Split(delimiter);
			X = Int32.Parse(a[0]);
			Y = Int32.Parse(a[1]);
		}

		/// <summary>
		/// Flips X and Y of the point.
		/// </summary>
		/// <returns>A new <see cref="IntPosition2D"/> that is the result of flipping.</returns>
		public IntPosition2D Flip()
		{
			var a = this;
			var temp = a.X;
			a.X = a.Y;
			a.Y = temp;
			return a;
		}

		/// <inheritdoc cref="object.Equals(object)"/>
		public override bool Equals(object obj)
		{
			return obj is IntPosition2D d &&
				   X == d.X &&
				   Y == d.Y;
		}

		/// <inheritdoc cref="object.GetHashCode"/>
		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y);
		}

		/// <summary>
		/// Adds two position. The result is the same as a vector addition.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>A new <see cref="IntPosition2D"/> as the addition result.</returns>
		public static IntPosition2D operator +(IntPosition2D a, IntPosition2D b)
		{
			return new IntPosition2D(a.X + b.X, a.Y + b.Y);
		}

		/// <summary>
		/// Subtracts two position. The result is the same as a vector subtraction.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>A new <see cref="IntPosition2D"/> as the subtraction result.</returns>
		public static IntPosition2D operator -(IntPosition2D a, IntPosition2D b)
		{
			return new IntPosition2D(a.X - b.X, a.Y - b.Y);
		}

		/// <summary>
		/// Performs a multiplication with an interger.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>A new <see cref="IntPosition2D"/> as the multiplication result.</returns>
		public static IntPosition2D operator *(IntPosition2D a, int b)
		{
			return new IntPosition2D(a.X * b, a.Y * b);
		}

		/// <summary>
		/// Performs a multiplication with an <see cref="IntPosition2D"/>.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>A new <see cref="IntPosition2D"/> as the multiplication result.</returns>
		public static IntPosition2D operator *(int a, IntPosition2D b)
		{
			return new IntPosition2D(a * b.X, a * b.Y);
		}

		/// <summary>
		/// Determines if two coordinate represent the same point in the 2D plane.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(IntPosition2D a, IntPosition2D b)
		{
			return a.X == b.Y && a.Y == b.Y;
		}

		/// <summary>
		/// Determines if two coordinate does not represent the same point in the 2D plane.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(IntPosition2D a, IntPosition2D b)
		{
			return !(a == b);
		}
	}
}
