using System;
using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.History.Solitaire;

namespace PlayingCards.Component.Solitaire
{
    /// <summary>
    /// Class for <see cref="Waste"/> pile that usually is the destination of <see cref="Stock"/>, or out-of-play cards.
    /// </summary>
    /// <remarks>
    /// A <see cref="Waste"/> has the following characteristics:
	/// <list type="number">
    /// <item>It can only be built by force transfer, not <see cref="TransferMove"/></item>
	/// <item>Only the first card is available to be transferred.</item>
	/// </list>
    /// </remarks>
    public class Waste : SolitairePile
    {
        static Waste()
        {
            AddRecord(typeof(Waste), "InitialPartitionIndex");
        }

        #region Fields
        //==========//

        private int m_shownCount;
		//======================================================================//
		#endregion


		#region Constructors
		//================//

		/// <summary>
		/// Constructs a standard <see cref="Waste"/> with one card showing.
		/// </summary>
		/// <param name="context">Game context associated</param>
		public Waste(IGameContext context) : base(context)
		{
            m_shownCount = 1;
		}

		/// <summary>
		/// Constructs a <see cref="Waste"/>.
		/// </summary>
		/// <param name="properties">Properties to instantiate with.</param>
		/// <param name="context">Game context associated</param>
		public Waste(IGameContext context, PileProperty properties) : base(context)
        {
			SetProperties(properties);
        }
		//======================================================================//
		#endregion

		// Empty
		#region Static methods
		//==================//
		//======================================================================//
		#endregion


		#region Overloaded properties
		//=========================//

		/// <inheritdoc cref="SolitairePile.PartitionIndex"/>
		public override int PartitionIndex { get { return Count == 0 ? -1 : Count - m_shownCount; } protected set {} }

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
        /// Readonly property. Retrieves the number of cards that is shown. Can be infered from <see cref="PartitionIndex"/>
        /// </summary>
        public virtual int ShownCardCounts => m_shownCount;
        //======================================================================//
        #endregion


        #region Overloaded methods
        //======================//

        /// <inheritdoc cref="SolitairePile.SetProperties"/>
		protected override void SetProperties(PileProperty properties)
		{
			m_shownCount = properties.InitialShown ?? 1;

			properties.InitialCount = 0;
			properties.PileBuildStrategy = new NoBuildStrategy();
			base.SetProperties(properties);
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

		// Empty
		#region New methods
		//===============//
		//======================================================================//
		#endregion
	}
}
