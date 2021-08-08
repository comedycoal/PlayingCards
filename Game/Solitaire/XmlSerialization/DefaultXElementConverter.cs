using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;

using PlayingCards.Primitives;
using PlayingCards.Component.Solitaire;


namespace PlayingCards.Game.Solitaire.XmlSerialization
{
	public class DefaultXElementConverter : IXElementConverter
	{
		private static readonly List<string> RegisteredPileSetNames;
		private static readonly List<string> ExhaustiveAssociationNames;

		public static ISolitairePileTypeResolver TypeResolver;

		static DefaultXElementConverter()
		{
			RegisteredPileSetNames = new List<string>
			{
				"foundation_set", "cell_set", "file_set"
			};

			ExhaustiveAssociationNames = new List<string>
			{
				"file", "cell", "reserve", "wastes", "foundation"
			};

			TypeResolver = new DefaultSolitaireTypeResolver();
		}

		private static string GetPileNameFromSetName(string set)
		{
			return set.Split('_')[0];
		}
		
		public static List<IdentifierToken> ConvertTokenString(string tokenString, List<PileInfo> pileinfoList)
		{
			if (tokenString is null) return null;
			var res = new List<IdentifierToken>();
			var strTokens = tokenString.Split(',');
			foreach (var strToken in strTokens)
			{
				int value;
				if (int.TryParse(strToken, out value))
				{
					try
					{
						res.Add(pileinfoList[value].IdToken);
					}
					catch
					{
						throw new NotImplementedException();
					}
				}
				else if (ExhaustiveAssociationNames.Contains(strToken))
				{
					Predicate<PileInfo> pred = pile =>
					{
						var type = TypeResolver.ResolvePile(strToken);
						return type.IsAssignableFrom(pile.PileType);
					};

					res.AddRange(pileinfoList.FindAll(pred).ConvertAll(x => x.IdToken));
				}
				else if (strToken == "tableau")
				{
					Predicate<PileInfo> pred = pile =>
					{
						return pile.IsInTableau;
					};

					res.AddRange(pileinfoList.FindAll(pred).ConvertAll(x => x.IdToken));
				}
			}

			return res;
		}

		private XName m_ns;

		public DefaultXElementConverter(XName ns)
		{
			m_ns = ns;
		}

		public GameInfo Convert(XElement element)
		{
			var metadata = ConvertMetadata(element.Element(m_ns + "metadata"));
			var layoutData = ConvertLayout(element.Element(m_ns + "layout"));

			return new GameInfo(metadata, layoutData);
		}

		public XElement Convert(GameInfo gameInfo)
		{
			throw new NotImplementedException();
		}


		private Metadata ConvertMetadata(XElement metadataElement)
		{
			var metadata = new Metadata
			{
				Name = (string)metadataElement.Attribute("name"),
				Description = (string)metadataElement.Element(m_ns + "description"),
				GameWinCondition = ConvertWinCondition((string)metadataElement.Attribute("win_condition")),
				GameDeck = ConvertDeck(metadataElement.Element(m_ns + "deck"))
			};
			return metadata;
		}

		private XElement ConvertMetadata(Metadata metadata)
		{
			var element = new XElement(m_ns + "metadata",
				new XElement(m_ns + "description", metadata.Description),
				new XElement(m_ns + "deck", ConvertDeck(metadata.GameDeck)));

			element.SetAttributeValue("name", metadata.Name);
			element.SetAttributeValue("win_condition", ConvertWinCondition(metadata.GameWinCondition));

			return element;
		}


		private WinCondition ConvertWinCondition(string winCond)
		{
			return winCond switch
			{
				"fill_foundation" => WinCondition.FILL_FOUNDATION,
				"clear_tableau" => WinCondition.CLEAR_TABLEAU,
				"custom" => WinCondition.CUSTOM,
				_ => throw new NotImplementedException()
			};
		}

		private string ConvertWinCondition(WinCondition winCond)
		{
			return winCond switch
			{
				WinCondition.FILL_FOUNDATION => "fill_foundation",
				WinCondition.CLEAR_TABLEAU => "clear_tableau",
				WinCondition.CUSTOM => "custom",
				_ => throw new NotImplementedException()
			};
		}


		private DeckDescription ConvertDeck(XElement deckElement)
		{
			return new DeckDescription(
				(string)deckElement.Attribute("type"),
				(int)deckElement.Attribute("count"));
		}

		private XElement ConvertDeck(DeckDescription deckDescription)
		{
			var a = new XElement(m_ns + "deck");
			a.SetAttributeValue("type", deckDescription.Type);
			a.SetAttributeValue("count", deckDescription.Count);
			return a;
		}

		private List<PileInfo> ConvertLayout(XElement layoutElement)
		{
			var pileMap = new SortedDictionary<int, PileInfo>();

			var pileLikeElements = from element in layoutElement.Descendants()
								   where element.Name.LocalName != "tableau"
										&& element.Name.LocalName != "cascade"
								   select new Tuple<bool, XElement>(element.Parent.Name.LocalName == "tableau", element);

			// Pass one: Convert every XElement into single-pile XElement
			List<Tuple<bool, XElement>> pileListPass1 = null;
			foreach (var elementPair in pileLikeElements)
			{
				string name;
				int count;
				
				if (RegisteredPileSetNames.Contains(elementPair.Item2.Name.LocalName))
				{
					if (elementPair.Item2.Name.LocalName == "foundation_set")
						count = 4;
					else
						count = (int?)elementPair.Item2.Attribute("count") ?? 1;

					name = GetPileNameFromSetName(elementPair.Item2.Name.LocalName);
					pileListPass1.AddRange(
						ConvertSetElement(elementPair.Item2).ConvertAll(
							x => new Tuple<bool, XElement>(elementPair.Item1, x)));
				}
				else
				{
					pileListPass1.Add(elementPair);
				}
			}

			// Pass two: Convert every single-pile XElement into Idmapped PileInfo.
			int counter = 0;
			var context = new IdentifierTokenContext();
			List<Tuple<PileInfo, string>> pileListPass2 = pileListPass1.ConvertAll(x =>
			{
				string associationStr;
				var a = ConvertPileWithoutAssociation(x.Item2,
														counter++,
														new IdentifierToken(x.Item2.Name.LocalName, context),
														x.Item1,
														out associationStr);
				return new Tuple<PileInfo, string>(a, associationStr);
			});

			// Pass three: Set up association tokens
			var tempPiles = pileListPass2.ConvertAll(x => x.Item1);
			var pileListPass3 = pileListPass2.ConvertAll(x =>
			{
				var a = x.Item1;
				a.Property.AssociationTokens = ConvertTokenString(x.Item2, tempPiles);
				return a;
			});

			return pileListPass3;
		}

		private XElement ConvertLayout(List<PileInfo> layout)
		{
			throw new NotImplementedException();
		}


		private PileInfo ConvertPileWithoutAssociation(XElement pileElement, int supplementId, IdentifierToken token, bool isInTableau, out string associationString)
		{
			int gamewiseId;
			string strId = pileElement.Attribute("id").Value;
			if (strId is null)
				gamewiseId = -1;
			else if (strId == "auto")
				gamewiseId = supplementId;
			else if (int.TryParse(strId, out gamewiseId))
			{
				if (gamewiseId != supplementId)
					throw new NotImplementedException();
			}
			else
				throw new NotImplementedException();

			var strat = (string?)pileElement.Attribute("build_strat");
			var initRank = (Rank?)(string)pileElement.Attribute("build_init");
			var gap = (int?)pileElement.Attribute("build_init");
			var wrapping = (bool?)pileElement.Attribute("build_init");
			var suit = (Suit?)(string)pileElement.Attribute("suit");

			BuildStrategy.Setting settingUsed = new BuildStrategy.Setting(
				initialRank: initRank ?? Rank.K_RANK,
				rankGap: gap ?? -1,
				allowWrapping: wrapping ?? false);

			BuildStrategy buildStratUsed = BuildStrategy.Create(strat ?? "any", settingUsed, suit ?? Suit.ANY_SUIT);

			associationString = (string)pileElement.Attribute("association");

			var a = new PileInfo
			{
				Position = new Position2D((string)pileElement.Attribute("pos") ?? "0,0"),
				IsInTableau = isInTableau,
				GamewiseId = gamewiseId,
				IdToken = token,
				PileType = TypeResolver.ResolvePile(pileElement.Name.LocalName),
				Property = new PileProperty
				{
					AutoMoveThreshold = (int?)pileElement.Attribute("automove_threshold"),
					CorrespondentSuit = (Suit?)(string)pileElement.Attribute("suit"),
					DealAmount = (int?)pileElement.Attribute("deal_amount"),
					InitialCount = (int?)pileElement.Attribute("initial_count"),
					InitialShown = (int?)pileElement.Attribute("initial_shown"),
					AssociationTokens = null,
					PileBuildStrategy = buildStratUsed,
					RestockAllowance = (int?)pileElement.Attribute("restock_allowance")
				}
			};
			return a;
		}

		private XElement ConvertPileWithoutAssociation(PileInfo pileInfo)
		{
			throw new NotImplementedException();
		}


		private List<XElement> ConvertSetElement(XElement setElement)
		{
			throw new NotImplementedException();
		}

		private XElement ConvertSet(List<PileInfo> pileInfos)
		{
			throw new NotImplementedException();
		}

	}
}
