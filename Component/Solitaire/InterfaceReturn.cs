using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingCards.Component.Solitaire
{
	/// <summary>
	/// Struct used as a return value for <see cref="SolitairePile.CreatePile"/> to
	/// accomodate different interface implementation of piles.
	/// </summary>
	public struct InterfaceReturn
	{
		/// <summary>
		/// Public field holds the reference to a <see cref="IDealer"/>,
		/// if any, else <see langword="null"/>
		/// </summary>
		public IDealer DealerReference;

		/// <summary>
		/// Public field holds the reference to a <see cref="FoundationReference"/>,
		/// if any, else <see langword="null"/>.
		/// </summary>
		public IFoundation FoundationReference;
	}
}
