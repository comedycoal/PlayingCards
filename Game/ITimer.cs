using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingCards.Game
{
	/// <summary>
	/// Interface defining a set of methods accounting for temporal changes
	/// during runtime for <see cref="IGame"/>.
	/// </summary>
	public interface ITimer
	{
		public void Start();
		public void Stop();
		public void Reset();

		public long TimeEllapsed { get; }
	}
}
