using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingCards.Game.Solitaire.XmlSerialization
{
	public interface ISolitairePileTypeResolver
	{
		public Type ResolvePile(string str, string variant=null);

		public string ResolveBaseTypeName(Type type);
	}
}
