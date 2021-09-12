using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml.Linq;

using PlayingCards.Primitives;
using PlayingCards.Utilities;
using PlayingCards.Component.Solitaire;
using PlayingCards.History;
using PlayingCards.Random;
using PlayingCards.History.Solitaire;

namespace PlayingCards.Game.Solitaire
{
	/// <summary>
	/// Please add details
	/// </summary>
	public partial class SolitaireGame : IGame, IGameContext
	{
		private static readonly PrioritizedComparer<string> SenderComparer;
		private static readonly PrioritizedComparer<string> ReceiverComparer;

		static SolitaireGame()
		{
			SenderComparer = new PrioritizedComparer<string>("file", "reserve", "waste", "cell", "stock", "foundation");
			ReceiverComparer = new PrioritizedComparer<string>("foundation", "file", "cell", "waste", "reserve", "stock");
		}

		public class HintInstance
        {
			public IdentifierToken Source { get; protected set; }
			public IdentifierToken Destination { get; protected set; }
			public int Count { get; protected set; }

			public HintInstance(IdentifierToken src, IdentifierToken dest, int count)
            {
				Source = src;
				Destination = dest;
				Count = count;
            }
        }


		#region Fields
		//==========//

		private Metadata m_metadata;
		private Deck m_deck;

		private IdentifierTokenContext m_context;
		private Dictionary<IdentifierToken, SolitairePile> m_playingPiles;
		private readonly IdentifierToken[] m_prioritySortedReceivers;
		private readonly IdentifierToken[] m_prioritySortedSenders;

		private SolitairePile[] m_tableau;
		private IFoundation[] m_foundations;
		private IDealer m_dealer;

		private IRandom m_rng;
		private ITimer m_timer;
		private ScoredHistory m_history;

		//======================================================================//
		#endregion


		#region Constructors
		//================//

		/// <summary>
		/// Constructs a <see cref="SolitaireGame"/>
		/// </summary>
		/// <param name="data">The metadata description.</param>
		/// <param name="piles">The layout description.</param>
		/// <param name="rng">The random engine used. Its <see cref="IRandom.Seed"/> does not need to be set beforehand.</param>
		/// <param name="timer">The timer used.</param>
		/// <param name="customSeed"></param>
		public SolitaireGame(Metadata data, List<PileInfo> piles, IRandom rng, ITimer timer, uint? customSeed = null)
		{
			m_metadata = data;
			m_deck = data.GameDeck.CreateDeck();

			m_playingPiles = new Dictionary<IdentifierToken, SolitairePile>();
			m_context = piles[0].IdToken.Context;

			var allPileList = new List<SolitairePile>();
			var tempTableau = new List<SolitairePile>();
			var tempFoundations = new List<IFoundation>();
			m_dealer = null;

			foreach (var info in piles)
			{
				InterfaceReturn m_returns;
				var playingPile = SolitairePile.CreatePile(this, info.PileType, out m_returns, info.Property, PileProperty.TransferMode.ESSENTIAL);

				allPileList.Add(playingPile);

				m_playingPiles.Add(info.IdToken, playingPile);

				if (m_returns.FoundationReference != null)
					tempFoundations.Add(m_returns.FoundationReference);

				if (m_returns.DealerReference != null && m_dealer == null)
					m_dealer = m_returns.DealerReference;
				else if (m_dealer != null)
					throw new NotImplementedException();

				if (info.IsInTableau)
					tempTableau.Add(playingPile);
			}

			m_tableau = tempTableau.ToArray();
			m_foundations = tempFoundations.ToArray();

			var tempReceivers = new List<KeyValuePair<IdentifierToken, SolitairePile>>(m_playingPiles);
			var tempSenders = new List<KeyValuePair<IdentifierToken, SolitairePile>>(m_playingPiles);

			tempSenders.Sort((x, y) =>
				{
					return SenderComparer.Compare(x.Key.Type, y.Key.Type);
				});
			m_prioritySortedSenders = tempSenders.ConvertAll(x => x.Key).ToArray();
			
			tempReceivers.Sort((x, y) =>
			{
				return ReceiverComparer.Compare(x.Key.Type, y.Key.Type);
			});
			m_prioritySortedReceivers = tempReceivers.ConvertAll(x => x.Key).ToArray();

			var seed = customSeed ?? (uint)Environment.TickCount;
			m_rng = rng;
			m_rng.Seed = seed;
			m_timer = timer;
			m_history = new ScoredHistory();

			Started = false;
			Paused = Ended = null;
		}
		//======================================================================//
		#endregion


		#region Static methods
		//==================//
		//======================================================================//
		#endregion


		#region New properties
		//==================//

		/// <summary>
		/// Readonly property. The seed being used in the game.
		/// </summary>
		public uint Seed => m_rng.Seed;

		/// <summary>
		/// Readonly property. The descriptiong of the game.
		/// </summary>
		public string Description { get; protected set; }

		/// <summary>
		/// Readonly property. Whether the game is started.
		/// </summary>
		public bool Started { get; protected set; }

		/// <summary>
		/// Readonly property. Whether the game is paused.
		/// </summary>
		public bool? Paused { get; protected set; }

		
		public bool? Ended { get; protected set; }

		public bool AllowInteractions => !(!Started || (bool)Paused || (bool)Ended);

		/// <summary>
		/// Readonly property. Whether undoing is still possible.
		/// </summary>
		public virtual bool CanUndo => m_history.CanUndo;

		/// <summary>
		/// Readonly property. Whether redoing is still possible.
		/// </summary>
		public virtual bool CanRedo => m_history.CanRedo;
		
		
		public int TableauCardCount
		{
			get
			{
				int count = 0;
				foreach (var pile in m_tableau)
				{
					count += pile.Count;
				}
				return count;
			}
		}


		public int FoundationFilledCount
        {
			get
            {
				int count = 0;
				foreach(var f in m_foundations)
                {
					count += f.Filled ? 1 : 0;
                }
				return count;
            }
        }
		//======================================================================//
		#endregion


		#region New methods
		//===============//

		// Utilities
		protected ScoredMove CreateMeaningfulMove(IMove move)
		{
			return new ScoredMove(move, 1);
		}

		protected ScoredMove CreateAuxiliaryMove(IMove move)
		{
			return new ScoredMove(move, 0);
		}

		protected bool CheckWin()
		{
			if (!Started) return false;
			if (m_metadata.GameWinCondition == WinCondition.CLEAR_TABLEAU)
			{
				return TableauCardCount == 0;
			}
			else if (m_metadata.GameWinCondition == WinCondition.FILL_FOUNDATION)
			{
				return FoundationFilledCount == m_foundations.Length;
			}
			else if (m_metadata.GameWinCondition == WinCondition.CUSTOM)
			{
				throw new NotImplementedException();
			}

			throw new NotImplementedException();
		}

		protected virtual void MakeStatistics()
        {

        }

		protected virtual void ClearStatistics()
        {

        }

		protected virtual void LayoutCleanup()
        {
			var tempTableau = new List<SolitairePile>();
			var tempFoundations = new List<IFoundation>();
			m_dealer = null;

			foreach(var pair in m_playingPiles)
            {
				InterfaceReturn ire;
				var newPile = SolitairePile.CreateCopyOf(pair.Value, out ire);
				m_playingPiles[pair.Key] = newPile;
				tempTableau.Add(newPile);
				if (ire.FoundationReference != null) tempFoundations.Add(ire.FoundationReference);
				if (m_dealer != null && ire.DealerReference != null)
                {
					throw new NotImplementedException();
                }
				else if (ire.DealerReference != null)
                {
					m_dealer = ire.DealerReference;
				}
            }

			m_foundations = tempFoundations.ToArray();
			m_tableau = tempTableau.ToArray();
        }

		protected virtual void Cleanup()
        {
			LayoutCleanup();
			m_rng.Reset();
			m_history.FullClear();
			m_timer.Reset();
        }


		// Game context methods

		/// <inheritdoc cref="IGameContext.ResolveAssociation"/>
		public SolitairePile ResolveAssociation(SolitairePile sender, IdentifierToken token)
		{
			SolitairePile destination;
			bool res = m_playingPiles.TryGetValue(token, out destination);
			if (!res) return null;
			return destination;
		}

		protected SolitairePile Get(IdentifierToken token)
		{
			return m_playingPiles[token];
		}


		// General "game" methods

		public ISaveObject Save()
        {
			throw new NotImplementedException();
        }

		public void Load(ISaveObject saveObject)
        {
			throw new NotImplementedException();
        }

		public void Start()
		{
            if (!Started)
            {
				if (!(Ended is null)) Cleanup();
				else if (Ended ?? false) ClearStatistics();

				Started = true;
				Paused = false;
				Ended = false;

				// First-time card deal
				m_dealer.ReceiveDeck(m_deck.Shuffle(m_rng));
				m_history.Execute(
					CreateAuxiliaryMove(
						m_dealer.CreateInitialDeal(m_playingPiles.Values)
					)
				);

				m_timer.Start();
			}
        }

		public void Pause()
        {
			if (Started && !(Ended ?? false))
            {
				Paused = true;

				m_timer.Stop();
			}
        }

		public void Resume()
        {
			if (Paused ?? false)
            {
				Paused = false;

				m_timer.Start();
			}
		}

		public void Reset()
        {
			End();
			ClearStatistics();
			Start();
        }

		public void End()
        {
			if (Started)
            {
				Ended = true;
				Paused = null;

				m_timer.Stop();

				MakeStatistics();
			}
        }


		// Game object interaction methods

		protected IMove GetTransfer(IdentifierToken from, IdentifierToken to, int count)
        {
			if (!AllowInteractions) return null;
			try
            {
				var transfer = m_playingPiles[from].CreateTransfer(m_playingPiles[to], count);
				return transfer;
			}
			catch(KeyNotFoundException e)
            {
				throw new NotImplementedException();
				return null;
            }

        }
		public bool TestTransfer(IdentifierToken from, IdentifierToken to, int count)
		{
			return GetTransfer(from, to, count) != null;
		}

		public bool Transfer(IdentifierToken from, IdentifierToken to, int count)
        {
			try
			{
				m_history.Execute(new ScoredMove(GetTransfer(from, to, count), 1));
				return true;
			}
			catch
            {
				return false;
            }
        }

		public HintInstance Hint()
        {
			if (!AllowInteractions) return null;
			foreach(var srcTok in m_prioritySortedSenders)
            {
				var candidatePile = Get(srcTok);
				foreach (var destTok in m_prioritySortedReceivers)
                {
					for(int i = candidatePile.Count - candidatePile.AvailableIndex; i > 0; --i)
                    {
						if (TestTransfer(srcTok, destTok, i))
							return new HintInstance(srcTok, destTok, i);
                    }
                }
            }

			return null;
        }

		public bool InferMove(IdentifierToken sourceToken, int count)
        {
			if (!AllowInteractions) return false;
			foreach(var destTok in m_prioritySortedReceivers)
            {
				var move = GetTransfer(sourceToken, destTok, count);
				if (move != null)
                {
					m_history.Execute(CreateMeaningfulMove(move));
					return true;
                }
			}

			return false;
        }

		public void Deal()
        {
			m_history.Execute(CreateMeaningfulMove(m_dealer.CreateDealMove()));
        }

		//======================================================================//
		#endregion
	}
}
