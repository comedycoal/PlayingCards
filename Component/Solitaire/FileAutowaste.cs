using System;
using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.History;
using PlayingCards.History.Solitaire;

namespace PlayingCards.Component.Solitaire
{
    /// <summary>
    /// Class for a <see cref="FileAutowaste"/>, variant of <see cref="File"/>. Typically used in "packing" games
    /// (games that built suits in the tableau rather than the <see cref="Foundation"/>s.
    /// </summary>
	/// <remarks>
	/// <para>
    /// A <see cref="FileAutowaste"/>, apart from <see cref="File"/>'s, has the following characteristics:
	/// <list type="number">
	/// <item>It has a <strong>waste pile</strong>, usually a <see cref="Waste"/>.</item>
    /// <item>Once built with <see cref="AutoMoveThreshold"/> consecutive cards, those cards are automatically moved to the waste pile.</item>
	/// </list></para>
    /// <see cref="FileAutowaste"/> should acts identical to any <see cref="SolitairePile"/>.</remarks>
	class FileAutowaste : File
	{
        static FileAutowaste()
        {
            AddRecord(typeof(FileAutowaste), "InitialCount", "PileBuildStrategy", "InitialShown", "AssociatedPiles", "AutoMoveThreshold");
        }

        #region Fields
        //==========//

        private IdentifierToken m_associationToken;
		private int m_automoveThreshold;
		//======================================================================//
		#endregion


		#region Constructors
		//================//

		/// <summary>
		/// Constructs a standard Spider-like <see cref="FileAutowaste"/> (<see cref="SameSuitStrategy"/> build) with 0 initial cards
		/// with a dangerous <see langword="null"/> destination pile.
		/// </summary>
		/// <param name="context">Game context associated</param>
		public FileAutowaste(IGameContext context) : base(context, new PileProperty {
			InitialCount = 0,
			InitialShown = 0,
			AssociationTokens = new List<IdentifierToken> { IdentifierToken.None },
			PileBuildStrategy = new SameSuitStrategy(BuildStrategy.Setting.StandardFileSetting, Suit.ANY_SUIT)
			})
		{

			m_automoveThreshold = (int)Rank.K_RANK;
        }

		/// <summary>
		/// Constructs a <see cref="FileAutowaste"/>.
		/// </summary>
		/// <param name="properties">Properties to instantiate with.</param>
		/// <param name="context">Game context associated</param>
		public FileAutowaste(IGameContext context, PileProperty properties) : this(context)
		{
			SetProperties(properties);
		}
        //======================================================================//
        #endregion


        #region Overloaded properties
        //=========================//

        /// <inheritdoc cref="SolitairePile.Fillable"/>
        public override bool Fillable => false;

        /// <inheritdoc cref="SolitairePile.Clearable"/>
        public override bool Clearable => false;
        //======================================================================//
        #endregion

        // 1 new
        #region New properties
        //==================//

        public int AutoMoveThreshold => m_automoveThreshold;

		/// <summary>
		/// Readonly property. Retrieves association token to the deal destination.
		/// </summary>
		public virtual IdentifierToken AssociationToken => m_associationToken;

		/// <summary>
		/// Readonly property. Retrieves reference to the deal destination <see cref="SolitairePile"/>, as resolved by the pile's <see cref="IGameContext"/>.
		/// </summary>
		private SolitairePile DealDestination => GameContext.ResolveAssociation(this, AssociationToken);
		//======================================================================//
		#endregion


		#region Overloaded methods
		//======================//

		/// <inheritdoc cref="SolitairePile.SetProperties"/>
		protected override void SetProperties(PileProperty properties)
        {
            if (properties.AssociationTokens == null || properties.AssociationTokens.Count > 1)
            {
                // throw something
                throw new NotImplementedException();
            }

            m_associationToken = properties.AssociationTokens[0];
            m_automoveThreshold = properties.AutoMoveThreshold ?? (int)PileProperty.GetDefaultNoneNullValue("AutoMoveThreshold");
            base.SetProperties(properties);
        }

		/// <inheritdoc cref="File.OnAddition" path="//*[not(self::remarks)]"/>
		/// <remarks>On termination, an <see cref="IMove"/> to move a completed suit, if any,
		/// is added to <paramref name="transferData"/>.</remarks>
		protected override void OnAddition(ref TransferData<Card> transferData)
		{
			if (Count - AvailableIndex >= AutoMoveThreshold)
			{
				transferData.AddAction(this.CreateTransfer(DealDestination, AutoMoveThreshold));
			}
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
