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
		/// <summary>
		/// Performs a conversion between a <see cref="XElement"/> of type "gameInfo"
		/// as specified by the accompanying schema to a <see cref="GameInfo"/> object.
		/// </summary>
		/// <param name="element"></param>
		/// <returns>A <see cref="GameInfo"/> instance.</returns>
		public GameInfo Convert(XElement element);

		/// <summary>
		/// Performs a conversion between a <see cref="GameInfo"/> object to a
		/// <see cref="XElement"/> of type "gameInfo" as specified by the accompanying schema.
		/// </summary>
		/// <param name="gameInfo"></param>
		/// <returns>A <see cref="XElement"/> object.</returns>
		public XElement Convert(GameInfo gameInfo);
	}
}
