using System;
using System.Collections.Generic;
using System.Xml.Linq;

using PlayingCards.Primitives;
using PlayingCards.Component.Solitaire;
using PlayingCards.History;
using PlayingCards.Random;
using PlayingCards.History.Solitaire;

namespace PlayingCards.Game.Solitaire
{
	/// <summary>
	/// Please add details
	/// </summary>
	public partial class SolitaireGame : IGame
	{
		#region Fields
		//==========//

		private Metadata m_metadata;
		
		private Dictionary<string, SolitairePile> m_playingPiles;
		private readonly List<SolitairePile> m_prioritySortedReceivers;
		private readonly List<SolitairePile> m_prioritySortedSenders;

		private List<SolitairePile> m_tableau;
		private List<IFoundation> foundations;
		private IDealer m_dealer;

		private IRandom m_rng;
		private ITimer m_timer;
		private ScoredHistory m_history;

		//======================================================================//
		#endregion


		#region Constructors
		//================//

		public SolitaireGame(XElement gameElement)
		{
			// Read game meta data
			//var gameNode = gameElement.Element("game");
			//m_metadata = new Metadata
			//{
			//	Name = gameNode.Attribute("name").Value,
			//	Description = gameNode.Attribute("description").Value,
			//	GameWinCondition = gameNode.Attribute("description").Value,
			//	GameDeck = gameNode.Attribute("description").Value
			//}


			// Read layout meta data
		}
		//======================================================================//
		#endregion


		#region Static methods
		//==================//
		//======================================================================//
		#endregion


		#region New properties
		//==================//

		public uint Seed => m_rng.Seed;

		public string Description { get; protected set; }

		public bool Started { get; protected set; }

		public bool Paused { get; protected set; }

		public virtual bool CanUndo => m_history.CanUndo;

		public virtual bool CanRedo => m_history.CanRedo;
		//======================================================================//
		#endregion


		#region New methods
		//===============//

		//======================================================================//
		#endregion
	}
}
