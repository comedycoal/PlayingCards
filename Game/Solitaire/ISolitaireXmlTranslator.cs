using System;
using System.Collections.Generic;
using System.Text;

using PlayingCards.Component.Solitaire;

namespace PlayingCards.Game.Solitaire
{
	public interface ISolitaireXmlTranslator
	{
		public void SetGameProperty(SolitaireGame game, string name, string value);
		public void SetPileProperty(PileProperty property, string name, string value);
	}
}
