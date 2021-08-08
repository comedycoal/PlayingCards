using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.History.Solitaire;
using PlayingCards.History;

namespace PlayingCards.Component.Solitaire
{
    /// <summary>
    /// Class for a <see cref="File"/> pile. Typically used in a Tableau
    /// </summary>
	/// <remarks>
	/// <para>
    /// A File has the following characteristics:
	/// <list type="number">
	/// <item>The last card is always faced up. Its <see cref="PartitionIndex"/>
	/// therefore initialized at one less than <see cref="SolitairePile.InitialCount"/>.</item>
	/// <item>Its <see cref="AvailableIndex"/> is at least equal to its Its <see cref="PartitionIndex"/>.</item>
	/// <item>Anytime a card is moved to or from a <see cref="File"/>, its <see cref="PartitionIndex"/>
	/// and <see cref="AvailableIndex"/> is re-evaluated.</item>
	/// </list></para></remarks>
    public class File : SolitairePile
    {
        static File()
        {
            AddRecord(typeof(File), "InitialCount", "PileBuildStrategy", "InitialShown");
        }

		// Empty
		#region Public constants
		//====================//

		//======================================================================//
		#endregion


		#region Fields
		//==========//

		/// <summary>
		/// Protected field. Holds the partition index (index at which cards are not faced down)
		/// </summary>
		protected int m_partitionIndex;

        /// <summary>
        /// Protected field. Holds the moveable partition index (index at which cards can be moved)
        /// </summary>
        protected int m_availableIndex;

		//======================================================================//
		#endregion


		#region Constructors
		//================//

		/// <summary>
		/// Constructs a <see cref="File"/> with standard Klondike setting and 0 initial cards
		/// </summary>
		/// <param name="context">Game context associated</param>
		public File(IGameContext context) : base(context)
		{
            m_partitionIndex = 0;
			m_availableIndex = 0;
			var temp = new PileProperty
			{
				InitialCount = 0,
				PileBuildStrategy = new AlternatingColorStrategy(BuildStrategy.Setting.StandardFileSetting),
			};
			SetProperties(temp);
        }

		/// <summary>
		/// Constructs a <see cref="File"/>.
		/// </summary>
		/// <param name="properties">Properties to instantiate with.</param>
		/// <param name="context">Game context associated</param>
		public File(IGameContext context, PileProperty properties) : this(context)
        {
			SetProperties(properties);
			m_availableIndex = 0;
		}
        //======================================================================//
        #endregion


        #region Overloaded properties
        //=========================//
        
        ///<inheritdoc cref="SolitairePile.PartitionIndex"/>
        public override int PartitionIndex { get { return m_partitionIndex; } protected set { m_partitionIndex = value; } }

		///<inheritdoc cref="SolitairePile.AvailableIndex"/>
		public override int AvailableIndex { get { return m_availableIndex; } protected set { m_availableIndex = value; } }

		/// <inheritdoc cref="SolitairePile.Fillable"/>
		public override bool Fillable => false;

        /// <inheritdoc cref="SolitairePile.Clearable"/>
        public override bool Clearable => false;

		//======================================================================//
		#endregion

		// Empty
		#region New properties
		//==================//

		//======================================================================//
		#endregion


		#region Overloaded methods
		//======================//

        /// <inheritdoc cref="SolitairePile.SetProperties"/>
        protected override void SetProperties(PileProperty properties)
        {
			m_partitionIndex = InitialCount - properties.InitialShown ?? (int)PileProperty.GetDefaultNoneNullValue("InitialShown");
			base.SetProperties(properties);    
        }

		/// <inheritdoc cref="SolitairePile.OnAddition" path="//*[not(self::remarks)]"/>
		/// <remarks><paramref name="transferData"/> is unchanged.</remarks>
		protected override void OnAddition(ref TransferData<Card> transferData)
		{
			return;
		}

		/// <inheritdoc cref="SolitairePile.OnExtraction" path="//*[not(self::remarks)]"/>
		/// <remarks>On termiation, a <see cref="IMove"/> to adjust <see cref="PartitionIndex"/>
		/// and <see cref="AvailableIndex"/> is added to <paramref name="transferData"/></remarks>
		protected override void OnExtraction(ref TransferData<Card> transferData)
		{
			if (AvailableIndex == Count - transferData.Count)
			{
				int newIndex = EvaluateAvailableIndex(AvailableIndex - 1);
				int oldIndex = AvailableIndex;
				System.Action exe = delegate () { AvailableIndex = newIndex; };
				System.Action und = delegate () { AvailableIndex = oldIndex; };
				transferData.AddAction(new GenericMove(exe, und));
			}

			if (PartitionIndex == Count - transferData.Count)
			{
				int newIndex = PartitionIndex - 1;
				int oldIndex = PartitionIndex;
				System.Action exe = delegate () { PartitionIndex = newIndex; };
				System.Action und = delegate () { PartitionIndex = oldIndex; };
				transferData.AddAction(new GenericMove(exe, und));
			}
		}
		//======================================================================//
		#endregion

		// 1 new
		#region New methods
		//===============//

		/// <summary>
		/// Evaluate the <see cref="PartitionIndex"/> and <see cref="AvailableIndex"/> 
		/// </summary>
		protected virtual int EvaluateAvailableIndex(int startingIndex)
        {
			int newAvailableIndex = startingIndex;
			while (newAvailableIndex >= PartitionIndex && newAvailableIndex > 0)
            {
                if (!PileBuildStrategy.CanPile(ForceGetCard(newAvailableIndex - 1), ForceGetCard(newAvailableIndex)))
                    break;
				--newAvailableIndex;
            }
			return newAvailableIndex;
        }
		//======================================================================//
		#endregion
	}
}
