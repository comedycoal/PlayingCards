using System;
using System.Collections.Generic;
using System.Text;

using System.Xml.Linq;
using System.Linq;

namespace PlayingCards.Game.Solitaire.XmlSerialization
{
	/// <summary>
	/// Interface providing methods to serialize and deserialize between <see cref="GameInfo"/>
	/// and <see cref="XElement"/>.
	/// </summary>
	public interface IXElementConverter
	{
		public GameInfo Convert(XElement element);
		public XElement Convert(GameInfo gameInfo);
	}
}
