using System;
using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.Component.Solitaire;
using PlayingCards.History;
using PlayingCards.History.Solitaire;

namespace PlayingCards.Component.Solitaire
{
    /// <summary>
    /// Class for a <see cref="Stock"/> that deals cards to multiple piles, either once at game start or sequentially during gameplay.
    /// </summary>
    /// <remarks>
    /// A <see cref="StockDealer"/> has the following characteristics:
	/// <list type="number">
    /// <item>It cannot be built. It can never receive cards from any other pile.</item>
	/// <item>It is associated with several <see cref="SolitairePile"/>s called the <strong>destination piles</strong>, usually a <see cref="File"/>.</item>
    /// <item>It can either fill (like in Klondike, at the start) , or distribute a fixed number of cards (like in Spider) to all destination piles</item>
	/// </list>
    /// A <see cref="StockDealer"/> generally should not be used polymorphously as a <see cref="SolitairePile"/>,
    /// as it provides functionality related to dealing and restocking while not concurring to traditional
    /// <see cref="TransferMove"/> executions.
    /// </remarks>
    public class StockDealer : Stock, IDealer
    {
        #region Public constants
        //====================//

        static StockDealer()
        {
			AddRecord(typeof(StockDealer), "AssociationTokens", "DealCount");
		}

        //======================================================================//
        #endregion


        #region Fields
        //==========//

        private List<IdentifierToken> m_associationTokens;
        private int m_dealCount;
		//======================================================================//
		#endregion


		#region Constructors
		//================//

		/// <summary>
		/// Constructs a standard <see cref="Stock"/>, distributing one card per round in distribute mode
		/// with a dangerous <see langword="null"/> destination pile.
		/// </summary>
		/// <param name="context">Game context associated</param>
		public StockDealer(IGameContext context) : base(context)
		{
			m_associationTokens = null;
			m_dealCount = 1;
		}

		/// <summary>
		/// Constructs a <see cref="StockDealer"/>.
		/// </summary>
		/// <param name="properties">Properties to instantiate with.</param>
		/// <param name="context">Game context associated</param>
		public StockDealer(IGameContext context, PileProperty properties) : base(context)
		{
			SetProperties(properties);
        }
		//======================================================================//
		#endregion


		#region Static methods
		//==================//
		//======================================================================//
		#endregion


		#region Overloaded properties
		//=========================//

		/// <inheritdoc cref="SolitairePile.PartitionIndex"/>
		public override int PartitionIndex { get { return Count; } protected set { } }

		/// <inheritdoc cref="SolitairePile.AvailableIndex"/>
		public override int AvailableIndex { get { return Count - 1; } protected set { } }

		/// <inheritdoc cref="SolitairePile.Fillable"/>
		public override bool Fillable => false;

        /// <inheritdoc cref="SolitairePile.Clearable"/>
        public override bool Clearable => true;
		//======================================================================//
		#endregion

		// 1 new
		#region New properties
		//==================//

		/// <summary>
		/// Public property. Retrieves the count of cards the pile deals to a destination each deal.
		/// </summary>
		public virtual int DealCount => m_dealCount;

		/// <summary>
		/// Public property. Retrieves a list of associations, which is of the pile's destinations.
		/// </summary>
		public virtual List<IdentifierToken> AssociationTokens => m_associationTokens;

		private List<SolitairePile> DealDestinations => AssociationTokens.ConvertAll(x => GameContext.ResolveAssociation(this, x));
		//======================================================================//
		#endregion


		#region Overloaded methods
		//======================//

		/// <inheritdoc cref="Stock.GetProperties"/>
		public override PileProperty GetProperties()
		{
			PileProperty property = base.GetProperties();
			property.AssociationTokens = new List<IdentifierToken>(m_associationTokens);
			property.DealAmount = m_dealCount;
			return property;
		}

		/// <inheritdoc cref="Stock.SetProperties"/>
		protected override void SetProperties(PileProperty properties)
        {
            if (properties.AssociationTokens == null)
            {
                // throw something
                throw new System.NotImplementedException();
            }
			m_associationTokens = new List<IdentifierToken>(properties.AssociationTokens);
            m_dealCount = properties.DealAmount ?? (int)PileProperty.GetDefaultNoneNullValue("DealAmount");

			properties.InitialCount = 0;
            properties.PileBuildStrategy = new NoBuildStrategy();
            base.SetProperties(properties);
        }

		/// <inheritdoc cref="Stock.Duplicate"/>
		public override SolitairePile Duplicate()
		{
			var a = GetProperties();
			return new StockDealer(GameContext, a);
		}
		//======================================================================//
		#endregion

		// 2 new
		#region New methods
		//===============//

		/// <inheritdoc cref="Stock.CreateDealMove"/>
		public override IMove CreateDealMove()
        {
			var piles = DealDestinations.ConvertAll(x => (IPile<Card>)x.Cards);
			return new DistributeMove(Cards, piles, DealCount);
        }

        /// <inheritdoc cref="Stock.CreateRestockMove"/>
        public override IMove CreateRestockMove()
        {
            return null;
        }
		//==================================================================//
		#endregion
	}
}