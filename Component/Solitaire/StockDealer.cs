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
    public class StockDealer : Stock
    {
        #region Public constants
        //====================//

        static StockDealer()
        {
			AddRecord(typeof(StockDealer), "AssociatedPiles", "DealCount");
		}

        //======================================================================//
        #endregion


        #region Fields
        //==========//

        private List<SolitairePile> m_dealDestinations;
        private int m_dealCount;
		//======================================================================//
		#endregion


		#region Constructors
		//================//

		/// <summary>
		/// Constructs a standard <see cref="Stock"/>, distributing one card per round in distribute mode
		/// with a dangerous <see langword="null"/> destination pile.
		/// </summary>
		public StockDealer() : base()
		{
			m_dealDestinations = null;
			m_dealCount = 1;
		}

		/// <summary>
		/// Constructs a <see cref="StockDealer"/>.
		/// </summary>
		/// <param name="properties">Properties to instantiate with.</param>
		public StockDealer(PileProperty properties) : base()
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

		/// <inheritdoc cref="Stock.DealDestination"/>
		public override SolitairePile DealDestination => throw new NotImplementedException();
		//======================================================================//
		#endregion

		// 1 new
		#region New properties
		//==================//

		/// <summary>
		/// 
		/// </summary>
		public virtual int DealCount => m_dealCount;

		/// <summary>
		/// 
		/// </summary>
		public virtual List<SolitairePile> DealDestinations => m_dealDestinations;
        //======================================================================//
        #endregion


        #region Overloaded methods
        //======================//

        /// <inheritdoc cref="SolitairePile.SetProperties"/>
        protected override void SetProperties(PileProperty properties)
        {
            if (properties.AssociatedPiles == null)
            {
                // throw something
                throw new System.NotImplementedException();
            }
            m_dealDestinations = properties.AssociatedPiles;
            m_dealCount = properties.DealAmount ?? (int)PileProperty.GetDefaultNoneNullValue("DealAmount");

			properties.InitialCount = 0;
            properties.PileBuildStrategy = new NoBuildStrategy();
            base.SetProperties(properties);
        }
        //======================================================================//
        #endregion

        // 4 new
        #region New methods
        //===============//

        /// <inheritdoc cref="Stock.CreateDealMove"/>
        public override IMove CreateDealMove()
        {
			return new DistributeMove(Cards, DealDestinations.ConvertAll(x => (IPile<Card>)x.Cards), DealCount);
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