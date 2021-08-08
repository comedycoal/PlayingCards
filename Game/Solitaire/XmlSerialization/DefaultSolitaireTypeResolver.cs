using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using PlayingCards.Component.Solitaire;

namespace PlayingCards.Game.Solitaire.XmlSerialization
{
	class DefaultSolitaireTypeResolver : ISolitairePileTypeResolver
	{
		public Type ResolvePile(string str, string variant=null)
		{
			var full = str + (variant != null ? (" " + variant) : "");
			return full switch
			{
				"cell" => typeof(Cell),
				"file base" => typeof(File),
				"file automove" => typeof(FileAutowaste),
				"file cell_inhibited" => throw new NotImplementedException(),
				"stock base" => typeof(Stock),
				"stock dealer" => typeof(StockDealer),
				"waste" => typeof(Waste),
				"foundation" => typeof(Foundation),
				"reserve" => throw new NotImplementedException(),
				_ => throw new NotImplementedException()
			};
		}

		public string ResolveBaseTypeName(Type type)
		{
			if (type.IsAssignableFrom(typeof(Cell)))
				return "cell";
			else if (type.IsAssignableFrom(typeof(File)))
				return "file";
			else if (type.IsAssignableFrom(typeof(Stock)))
				return "stock";
			else if (type.IsAssignableFrom(typeof(Waste)))
				return "waste";
			//else if (type.IsAssignableFrom(typeof(Reserve)))
			//	return "file";
			else if (type.IsAssignableFrom(typeof(Foundation)))
				return "foundation";
			else
				throw new NotImplementedException();
		}
	}
}
