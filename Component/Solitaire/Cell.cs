using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.History.Solitaire;

namespace PlayingCards.Component.Solitaire
{
	/// <summary>
	/// Class represents the "cell", for example in FreeCells.
	/// </summary>
	/// <remarks>
	/// Cell has the capacity of only one, but any card. It is mostly known for the FreeCell-type games.
	/// Any transfer requires the cell to be empty and the transfer size of 1.
	/// </remarks>
	public class Cell : SolitairePile
	{
		static Cell()
		{
			AddRecord(typeof(Cell));
		}

		// Empty
		#region Public constants
		//====================//
		//======================================================================//
		#endregion


		#region Constructors
		//================//

		/// <summary>
		/// Constructs a <see cref="Cell"/>.
		/// </summary>
		/// <param name="context">Game context associated</param>
		public Cell(IGameContext context) : base(context) { }

		/// <summary>
		/// Constructs a <see cref="Cell"/>.
		/// </summary>
		/// <param name="properties">Properties to instantiate with.</param>
		/// <param name="context">Game context associated</param>
		public Cell(IGameContext context, PileProperty properties): base(context)
		{
			SetProperties(properties);
		}
		//======================================================================//
		#endregion


		#region Overloaded properties
		//=========================//

		/// <inheritdoc cref="SolitairePile.PartitionIndex"/>
		public override int PartitionIndex { get { return 0; } protected set { } }

		/// <inheritdoc cref="SolitairePile.AvailableIndex"/>
		public override int AvailableIndex { get { return Count == 0 ? -1 : Count - 1; } protected set { } }

		/// <inheritdoc cref="SolitairePile.Fillable"/>
		public override bool Fillable => true;

		/// <inheritdoc cref="SolitairePile.Clearable"/>
		public override bool Clearable => false;
		//======================================================================//
		#endregion

		// 1 new
		#region New properties
		//==================//

		/// <summary>
		/// Readonly property. Retrieves a <see langword="bool"/> indicating whether the cell is currently holding a card.
		/// </summary>
		public virtual bool Occupied => Count == 1;
		//======================================================================//
		#endregion


		#region Overloaded methods
		//======================//

		/// <inheritdoc cref="SolitairePile.SetProperties"/>
		protected override void SetProperties(PileProperty properties)
		{
			properties.InitialCount = 0;
			properties.PileBuildStrategy = new AnySuitStrategy();
			base.SetProperties(properties);
		}

		/// <inheritdoc cref="SolitairePile.AllowTransfer" path="//*[not(self::remarks)]"/>
		/// <remarks>Only single card transfers on an empty cell can be perform.</remarks>
		protected override bool AllowTransfer(TransferData<Card> transferData)
		{
			return transferData.Count == 1 && Count == 0 && base.AllowTransfer(transferData);
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