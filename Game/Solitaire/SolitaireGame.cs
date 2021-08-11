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
			SenderComparer = new PrioritizedComparer<string>("foundation", "file", "cell", "waste", "reserve", "stock");
		}

		#region Fields
		//==========//

		private Metadata m_metadata;
		private Deck m_deck;

		private IdentifierTokenContext m_context;
		private Dictionary<IdentifierToken, SolitairePile> m_playingPiles;
		private readonly SolitairePile[] m_prioritySortedReceivers;
		private readonly SolitairePile[] m_prioritySortedSenders;

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
		public SolitaireGame(Metadata data, List<PileInfo> piles, IRandom rng, ITimer timer)
		{
			m_metadata = data;
			m_deck = data.GameDeck.CreateDeck();

			m_playingPiles = new Dictionary<IdentifierToken, SolitairePile>();
			m_context = piles[0].IdToken.Context;

			m_playingPiles = new Dictionary<IdentifierToken, SolitairePile>();
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

			tempSenders.Sort( (x, y) =>
				{
					return SenderComparer.Compare(x.Key.Type, y.Key.Type);
				});
			m_prioritySortedSenders = tempSenders.ConvertAll(x => x.Value).ToArray();
			
			tempReceivers.Sort((x, y) =>
			{
				return ReceiverComparer.Compare(x.Key.Type, y.Key.Type);
			});
			m_prioritySortedReceivers = tempReceivers.ConvertAll(x => x.Value).ToArray();

			m_rng = rng;
			m_timer = timer;
			m_history = new ScoredHistory();
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
		public bool Paused { get; protected set; }

		/// <summary>
		/// Readonly property. Whether undoing is still possible.
		/// </summary>
		public virtual bool CanUndo => m_history.CanUndo;

		/// <summary>
		/// Readonly property. Whether redoing is still possible.
		/// </summary>
		public virtual bool CanRedo => m_history.CanRedo;

		/// <inheritdoc cref="IGameContext.ResolveAssociation"/>
		public SolitairePile ResolveAssociation(SolitairePile sender, IdentifierToken token)
		{
			SolitairePile destination;
			bool res = m_playingPiles.TryGetValue(token, out destination);
			if (!res) return null;
			return destination;
		}
		//======================================================================//
		#endregion


		#region New methods
		//===============//

		//======================================================================//
		#endregion
	}
}
