using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingCards.Game.Solitaire.XmlSerialization
{

	/// <summary>
	/// Inteface defining conversion methods to resolves queries regarding solitaire piles'
	/// underlying types.
	/// </summary>
	public interface ISolitairePileTypeResolver
	{
		/// <summary>
		/// Retrieves the underying <see cref="Type"/> corresponding to the <paramref name="str"/> and <paramref name="variant"/>
		/// </summary>
		/// <param name="str">The base name of the pile.</param>
		/// <param name="variant">Variant of the pile.</param>
		/// <returns>A <see cref="Type"/> object.</returns>
		public Type ResolvePile(string str, string variant=null);

		/// <summary>
		/// Retrieves the corresponding base string type of a <see cref="PlayingCards.Component.Solitaire.SolitairePile"/>
		/// inherited <see cref="Type"/>.
		/// </summary>
		/// <param name="type"></param>
		/// <returns>A string object.</returns>
		public string ResolveBaseTypeName(Type type);
	}
}
