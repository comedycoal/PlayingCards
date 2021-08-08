using System;
using System.Collections.Generic;
using System.Text;

using System.Xml.Linq;
using System.Linq;

namespace PlayingCards.Game.Solitaire.XmlSerialization
{
	public interface IXElementConverter
	{
		public GameInfo Convert(XElement element);
		public XElement Convert(GameInfo gameInfo);
	}
}
