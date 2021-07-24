using PlayingCards.Primitives;

namespace PlayingCards.Component.Solitaire
{
	/// <summary>
	/// Public interface consists of methods for a Foundation pile
	/// </summary>
	public interface IFoundation
	{
		/// <summary>
		/// Public readonly property. Returns the associated <see cref="Suit"/>, if any that the pile is built on
		/// </summary>
		public Suit AssociatedSuit { get; }

		/// <summary>
		/// Public readonly property. Returns whether the pile is completely built and considered done.
		/// </summary>
		public bool Filled { get; }
	}
}
