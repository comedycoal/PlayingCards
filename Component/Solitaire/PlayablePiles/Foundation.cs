using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.Component.Solitaire;
using PlayingCards.History.Solitaire;

namespace PlayingCards.Component.Solitaire
{
    /// <summary>
	/// Class encapsulating a Foundation.
	/// </summary>
    public class Foundation : SolitairePile
    {
		static Foundation()
		{
			AddRecord(typeof(Foundation), "PileBuildStrategy", "CorrespondentSuit");
		}

		#region Fields
		//==========//

		private Suit m_associatedSuit;
		//======================================================================//
		#endregion


		#region Constructors
		//================//

		/// <summary>
		/// Constructs a standard <see cref="Foundation"/> with <see cref="Suit.HEARTS"/> suit.
		/// </summary>
		/// <param name="context">Game context associated</param>
		public Foundation(IGameContext context) : base(context)
		{
			m_associatedSuit = Suit.HEARTS;
			SetProperties(new PileProperty
			{
				InitialCount = 0,
				PileBuildStrategy = new SameSuitStrategy(BuildStrategy.Setting.StandardFoundationSetting, m_associatedSuit)
			});
		}

		/// <summary>
		/// Constructs a <see cref="Foundation"/>.
		/// </summary>
		/// <param name="properties">Properties to instantiate with.</param>
		/// <param name="context">Game context associated</param>
		public Foundation(IGameContext context, PileProperty properties) : base(context)
        {
			SetProperties(properties);
		}
		//======================================================================//
		#endregion

		// 1 new
		#region Static methods
		//==================//

		/// <summary>
		/// Static helper method aids the creation of foundation sets containing all suit foundations for a deck.
		/// All foundations are uniform, only differs in suit.
		/// </summary>
		/// <param name="properties">Properties to instantiate with.</param>
		/// <param name="context">Game context associated.</param>
		/// <returns>A list of 4 <see cref="Foundation"/> of each <see cref="Suit"/>.</returns>
		public static List<Foundation> CreateFoundationSet(IGameContext context, PileProperty properties)
        {
			List<Foundation> temp = new List<Foundation>();
			foreach (var item in Suit.FULL_SUITS_LIST)
			{
				properties.CorrespondentSuit = item;
				temp.Add(new Foundation(context, properties));
			}
			return temp;
        }
		//======================================================================//
		#endregion


		#region Overloaded properties
		//=========================//

        /// <inheritdoc cref="SolitairePile.PartitionIndex"/>
		public override int PartitionIndex { get { return 0; } protected set { } }

		/// <inheritdoc cref="SolitairePile.AvailableIndex"/>
		public override int AvailableIndex { get { return Count == 0 ? - 1 : Count - 1; } protected set { } }

		/// <inheritdoc cref="SolitairePile.Fillable"/>
		public override bool Fillable => true;

		/// <inheritdoc cref="SolitairePile.Clearable"/>
		public override bool Clearable => false;
		//======================================================================//
		#endregion

		// 2 new
		#region New properties
		//==================//

		/// <summary>
		/// Readonly property. Retrieves the suit asscociated with the foundation.
		/// </summary>
		public virtual Suit AssociatedSuit => m_associatedSuit;
		//======================================================================//
		#endregion


		#region Overloaded methods
		//======================//

		/// <inheritdoc cref="SolitairePile.GetProperties"/>
		public override PileProperty GetProperties()
		{
			PileProperty property = base.GetProperties();
			property.CorrespondentSuit = m_associatedSuit;
			return property;
		}

		/// <inheritdoc cref="SolitairePile.SetProperties"/>
		protected override void SetProperties(PileProperty properties)
		{
			m_associatedSuit = properties.CorrespondentSuit ?? (Suit)PileProperty.GetDefaultNoneNullValue("CorrespondentSuit");
			properties.PileBuildStrategy = new SameSuitStrategy(properties.PileBuildStrategy, m_associatedSuit);

			properties.InitialCount = 0;
			base.SetProperties(properties);
		}

		///<inheritdoc cref="SolitairePile.AllowTransfer"/>
		protected override bool AllowTransfer(TransferData<Card> transferData)
		{
			return transferData.Count == 1 && base.AllowTransfer(transferData);
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

		/// <inheritdoc cref="SolitairePile.Duplicate"/>
		public override SolitairePile Duplicate()
		{
			var a = GetProperties();
			return new Foundation(GameContext, a);
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