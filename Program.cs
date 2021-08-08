using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;

using PlayingCards.Primitives;
using PlayingCards.Game.Solitaire;
using PlayingCards.Component.Solitaire;
using PlayingCards.History;
using PlayingCards.History.Solitaire;

namespace PlayingCards
{
	class ConsoleTester
    {
		static void Main(string[] args)
		{
			//List<File> playingPiles = new List<File>();
			//Waste waste;
			//Stock stock;
			//List<Foundation> foundations = new List<Foundation>();

			//// Files:
			//BuildStrategy fileBS = new AlternatingColorStrategy(new BuildStrategy.Setting(Rank.K_RANK));
			//PileProperty fileP = new PileProperty { InitialCount = 0, PileBuildStrategy = fileBS };
			//for (int i = 0; i < 7; i++)
			//{
			//	fileP.InitialCount = i + 1;
			//	fileP.InitialShown = i;
			//	File file = SolitairePile.CreatePile<File>(TODO, fileP);
			//	playingPiles.Add(file);
			//}

			//// Foundations:
			//BuildStrategy foundBS = new SameSuitStrategy(new BuildStrategy.Setting(Rank.A_RANK, 1), Suit.ANY_SUIT);
			//PileProperty foundP = new PileProperty { PileBuildStrategy = foundBS, CorrespondentSuit = Suit.ANY_SUIT };
			//foreach (var suit in Suit.FULL_SUITS_LIST)
			//{
			//	foundP.CorrespondentSuit = suit;
			//	Foundation found = SolitairePile.CreatePile<Foundation>(TODO, foundP);
			//	foundations.Add(found);
			//}

			//// Waste
			//BuildStrategy wasteBS = new NoBuildStrategy();
			//PileProperty wasteP = new PileProperty { InitialShown = 3 };
			//waste = SolitairePile.CreatePile<Waste>(TODO, wasteP);

			//// Stock:
			//BuildStrategy stockBS = new NoBuildStrategy();
			//PileProperty stockP = new PileProperty { DealAmount = 3, RestockAllowance = Stock.INFINITE, AssociationTokens = new List<SolitairePile> { waste } };
			//stock = SolitairePile.CreatePile<Stock>(TODO, stockP);

			//ScoredHistory history = new ScoredHistory();

			//Deck deck = Deck.GetFullDecks(1);
			//var list = deck.Shuffle(99);
			//stock.ReceiveDeck(list);

			//history.Execute(new ScoredMove(stock.CreateInitialDeal(playingPiles.ConvertAll(x => (SolitairePile)x))));

			//IMove move2;
			//move2 = playingPiles[6].CreateTransfer(foundations[0], 1);
			//history.Execute(new ScoredMove(move2));
			//move2 = playingPiles[1].CreateTransfer(foundations[0], 1);
			//history.Execute(new ScoredMove(move2));
			//move2 = stock.CreateDealMove();
			//history.Execute(new ScoredMove(move2));

			//foreach (var pile in playingPiles)
			//	Console.WriteLine(pile.ToString());

			//history.Undo();
			//history.Undo();
			//history.Undo();
			//history.Undo();
		}
    }
}
