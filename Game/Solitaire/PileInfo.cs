using System;
using PlayingCards.Component.Solitaire;

namespace PlayingCards.Game.Solitaire
{
	/// <summary>
	/// Struct comprising the information of a solitaire pile, in context of a specific game.
	/// </summary>
	public struct PileInfo
	{
		/// <summary>
		/// The position of the pile in the game grid.
		/// </summary>
		public IntPosition2D Position;

		/// <summary>
		/// Whether the pile is in the game tableau or not.
		/// </summary>
		public bool IsInTableau;

		/// <summary>
		/// The global Id of the pile in the game.
		/// </summary>
		public int GamewiseId;

		private Type m_type;

		/// <summary>
		/// The type of the pile.
		/// </summary>
		public Type PileType
		{
			get { return m_type; }
			set
			{ 
				if (!typeof(SolitairePile).IsAssignableFrom(value))
					throw new NotImplementedException();
				m_type = value;
			}
		}

		/// <summary>
		/// The identifier token of the pile.
		/// </summary>
		public IdentifierToken IdToken;

		/// <summary>
		/// The properties of the pile, used to instantiate a <see cref="SolitairePile"/> reference.
		/// </summary>
		public PileProperty Property;
	}
}
