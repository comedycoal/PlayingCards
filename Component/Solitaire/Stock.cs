using System;
using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.Component.Solitaire;
using PlayingCards.History;
using PlayingCards.History.Solitaire;

namespace PlayingCards.Component.Solitaire
{
	/// <summary>
	/// Class for a (single) <see cref="Stock"/> that deals cards to another pile, usually a <see cref="Waste"/>.
	/// </summary>
	/// <remarks>
	/// A (single) <see cref="Stock"/> has the following characteristics:
	/// <list type="number">
	/// <item>It cannot be built nor transfered from normally.</item>
	/// <item>It is associated with another <see cref="SolitairePile"/> called the <strong>destination pile</strong>, usually a <see cref="Waste"/>.</item>
	/// <item>It can "deal" a fixed ammount of cards (its <see cref="Stock.FlipAmount"/>) to its destination pile.</item>
	/// <item>Once empty, a <see cref="Stock"/> can perform a restock from its destination pile. It may perform limited or inifinite restocks,
	/// how many is corresponded to the <see cref="Stock.TotalRestock"/> property.</item>
	/// </list>
	/// A <see cref="Stock"/> generally should not be used polymorphously as a <see cref="SolitairePile"/>,
	/// as it provides functionality related to dealing and restocking while not concurring to traditional
	/// <see cref="TransferMove"/> executions.
	/// </remarks>
	public partial class Stock : SolitairePile, IDealer
    {
        static Stock()
        {
            AddRecord(typeof(Stock), "AssociatedPiles", "DealAmount", "RestockAllowance");
        }

        /// <summary>
        /// Public integer constant represent an arbitrary large integer.
        /// </summary>
        public const int INFINITE = int.MaxValue;


        #region Fields
        //==========//

        private int m_flipAmount;
        private int m_totalRestock;
		private int m_restockCount;
        private SolitairePile m_dealDestination;
		//======================================================================//
		#endregion


		#region Constructors
		//================//

		/// <summary>
		/// Constructs a standard <see cref="Stock"/>, dealing 1 card, infinite restocks.
		/// with a dangerous <see langword="null"/> destination pile.
		/// </summary>
		public Stock() : base()
		{
            m_dealDestination = null;
            m_flipAmount = 1;
            m_totalRestock = INFINITE;
            m_restockCount = 0;
        }

		/// <summary>
		/// Constructs a <see cref="Stock"/>.
		/// </summary>
		/// <param name="properties">Properties to instantiate with.</param>
		public Stock(PileProperty properties) : base()
		{
			SetProperties(properties);
            m_restockCount = 0;
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

        // 5 new
        #region New properties
        //==================//

        /// <summary>
        /// Readonly property. Retrieves reference to the deal destination <see cref="SolitairePile"/>.
        /// </summary>
        public virtual SolitairePile DealDestination => m_dealDestination;

        /// <summary>
        /// Readonly property. Retrieves the number of cards transfered to deal destination at once.
        /// </summary>
        public virtual int FlipAmount => m_flipAmount;

        /// <summary>
        /// Readonly property. Retrieves the amount of restocks made during the pile's lifetime.
        /// </summary>
        public virtual int RestockCount => m_restockCount == INFINITE ? INFINITE : m_restockCount;

        /// <summary>
        /// Readonly property. Retrieves the amount of restocks the pile is allowed to make.
        /// </summary>
        public virtual int TotalRestock => m_totalRestock;

        /// <summary>
        /// Readonly property. Retrieves whether the pile can restock.
        /// </summary>
        public virtual bool OutOfStock => (!(RestockCount == INFINITE)) || (RestockCount == TotalRestock);
        //======================================================================//
        #endregion


        #region Overloaded methods
        //======================//

        /// <inheritdoc cref="SolitairePile.SetProperties"/>
        protected override void SetProperties(PileProperty properties)
        {
            if (properties.AssociatedPiles == null || properties.AssociatedPiles.Count > 1)
            {
                // throw something
                throw new NotImplementedException();
            }
            m_dealDestination = properties.AssociatedPiles[0];
            m_flipAmount = properties.DealAmount ?? (int)PileProperty.GetDefaultNoneNullValue("DealAmount");
            m_totalRestock = properties.RestockAllowance ?? (int)PileProperty.GetDefaultNoneNullValue("RestockAllowance"); ;

			properties.InitialCount = 0;
            properties.PileBuildStrategy = new NoBuildStrategy();
            base.SetProperties(properties);
        }
        //======================================================================//
        #endregion

        // 4 new
        #region New methods
        //===============//

        /// <inheritdoc cref="IDealer.ReceiveDeck"/>
        public virtual void ReceiveDeck(List<Card> cards)
        {
            Cards.Add(cards);
        }


		/// <inheritdoc cref="IDealer.CreateInitialDeal"/>
		public virtual IMove CreateInitialDeal(List<SolitairePile> destinations)
		{
			return new InitialFillMove(Cards, destinations.ConvertAll(x => new Tuple<IPile<Card>, int>(x.Cards, x.InitialCount)));
		}

		/// <summary>
		/// Create an <see cref="IMove"/> that performs a deal to the destination piles.
		/// </summary>
		/// <returns>An <see cref="IMove"/>, or <see langword="null"/> if such move is not possible.</returns>
		public virtual IMove CreateDealMove()
        {
            if (Count > 0)
            {
                var transferData = new TransferData<Card>(Cards, null, Count >= FlipAmount ? FlipAmount : Count, default(IMove));
                return new ReversedOrderMove(DealDestination.Cards, transferData);
            }
            return null;
        }

		/// <summary>
		/// Create an <see cref="IMove"/> that performs a restock.
		/// </summary>
		/// <returns>An <see cref="IMove"/>, or <see langword="null"/> if such move is not possible.</returns>
		public virtual IMove CreateRestockMove()
        {
            if (Count == 0 && !OutOfStock)
            {
                System.Action exe = delegate () { ++m_restockCount; };
                System.Action und = delegate () { --m_restockCount; };
                var a = new GenericMove(exe, und);
                var transferData = new TransferData<Card>(DealDestination.Cards, null, DealDestination.Count, a);
                return new ReversedOrderMove(Cards, transferData);
            }
            return null;
        }

		/// <inheritdoc cref="SolitairePile.OnAddition" path="//*[not(self::remarks)]"/>
		/// <remarks><paramref name="transferData"/> is unchanged.</remarks>
		protected override void OnAddition(ref TransferData<Card> transferData)
		{
			return;
		}

		/// <inheritdoc cref="SolitairePile.OnExtraction" path="//*[not(self::remarks)]"/>
		/// <remarks><paramref name="transferData"/> is unchanged.</remarks>
		protected override void OnExtraction(ref TransferData<Card> transferData)
		{
			return;
		}
		//======================================================================//
		#endregion
	}
}