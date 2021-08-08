using System;
using System.Collections.Generic;
using System.Text;

using PlayingCards.Component.Solitaire;

namespace PlayingCards.Game.Solitaire
{
	public struct PileInfo
	{
		public Position2D Position;
		public bool IsInTableau;
		public int GamewiseId;
		public Type PileType;
		public IdentifierToken IdToken;
		public PileProperty Property;
	}
}
