using System;
using System.Reflection;
using System.Collections.Generic;

namespace PlayingCards.Component.Solitaire
{
	/// <summary>
	/// Comprehensive struct containing complete characteristics any inherited <see cref="SolitairePile"/> can
	/// have. Provides no constructors. Needed fields should be set directly and the rest sets to default.
	/// </summary>
	/// <remarks>
	/// While varied in implementations. <see cref="PileProperty"/>'s behaviors should concur to <see cref="TransferMode"/>
	/// when used to set a <see cref="SolitairePile"/>'s properties.
	/// </remarks>
	public partial struct PileProperty
	{
		/// <summary>
		/// Pile's initial count, i.e how many cards it holds at the start.
		/// </summary>
		public int? InitialCount;

		/// <summary>
		/// Pile's build strategy, dictates how the pile is built by user's transfer.
		/// </summary>
		public BuildStrategy PileBuildStrategy;

		/// <summary>
		/// Pile's initial partition index, i.e how many cards are visible at the start.
		/// </summary>
		public int? InitialPartitionIndex;

		/// <summary>
		/// List of associated piles.
		/// </summary>
		public List<SolitairePile> AssociatedPiles;

		/// <summary>
		/// Quantity at which when <em><see cref="SolitairePile.Count"/> - <see cref="SolitairePile.AvailableIndex"/></em>
		/// reaches that a transfer is automatically made to <see cref="PileProperty.AssociatedPiles"/>
		/// </summary>
		/// <remarks>
		/// <see cref="PileProperty.AssociatedPiles"/> should have length of 1 if <see cref="AutoMoveThreshold"/> is not <see langword="null"/>
		/// </remarks>
		public int? AutoMoveThreshold;

		///// <summary>
		///// Indicates if wrapping is enabled
		///// </summary>
		//public bool? Wrapping;

		/// <summary>
		/// The pile's correspondent suit.
		/// </summary>
		public PlayingCards.Primitives.Suit? CorrespondentSuit;

		/// <summary>
		/// Dictates how many cards to send to <see cref="PileProperty.AssociatedPiles"/> at once.
		/// </summary>
		public int? DealAmount;

		/// <summary>
		/// Dictates how many restocks can the pile have.
		/// </summary>
		public int? RestockAllowance;


		/// <summary>
		/// Retrieves the default gameplay value of properties.
		/// </summary>
		/// <param name="fieldName">Name of the property (or field) to retrieve.</param>
		/// <returns>A nullable<see langword="object"/>, can be casted to appropriate type if needed.</returns>
		public static object GetDefaultNoneNullValue(string fieldName)
		{
			return fieldName switch
			{
				"InitialCount" => 0,
				"InitialPartitionIndex" => 0,
				"AutomoveThreshold" => Primitives.Rank.K_RANK,
				"DealAmmount" => 1,
				"RestockAllowance" => Stock.INFINITE,
				"PileBuildStrategy" => new NoBuildStrategy(),
				//case "Wrapping":
				//	return false;
				"AssociatedPile" => null,
				"CorrespondentSuit" => Primitives.Suit.HEARTS,
				_ => null,
			};
		}

		/// <summary>
		/// Retrieves the default gameplay value of properties.
		/// </summary>
		/// <param name="fieldInfo">Relevant <see cref="System.Reflection.FieldInfo"/>.</param>
		/// <returns>A nullable<see langword="object"/>, can be casted to appropriate type if needed.</returns>
		public static object GetDefaultNoneNullValue(System.Reflection.FieldInfo fieldInfo)
		{
			return GetDefaultNoneNullValue(fieldInfo.Name);
		}

		/// <summary>
		/// Determine the strictest <see cref="PileProperty.TransferMode"/> that <paramref name="properties"/> can be used to instantiate a 
		/// <see cref="SolitairePile"/> using <see cref="SolitairePile.CreatePile{T}"/>,
		/// of which properties in <paramref name="enforcedFields"/>is needed.
		/// </summary>
		/// <param name="properties">A <see cref="PileProperty"/> instance.</param>
		/// <param name="enforcedFields">An array of property's names.</param>
		/// <returns>A <see cref="PileProperty.TransferMode"/>, at least <see cref="PileProperty.TransferMode.INFERRED"/>.</returns>
		public static PileProperty.TransferMode GetMostApplicableTransferMode(PileProperty properties, string[] enforcedFields)
		{
			Type t = properties.GetType();
			bool non_essentialFlagged = false;
			foreach (var fieldInfo in t.GetFields(BindingFlags.Public | BindingFlags.Instance))
			{
				bool needed = Array.Exists(enforcedFields, x => x == fieldInfo.Name);
				var value = fieldInfo.GetValue(properties);
				if (!needed && value != null)
					non_essentialFlagged = true;
				else if (needed && value == null)
					return TransferMode.INFERRED;
			}
			return non_essentialFlagged ? TransferMode.ESSENTIAL : TransferMode.STRICT;
		}
	}
}
