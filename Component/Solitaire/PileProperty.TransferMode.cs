namespace PlayingCards.Component.Solitaire
{
	public partial struct PileProperty
	{
		/// <summary>
		/// Companion enumeration with methods that uses <see cref="PileProperty"/> to transfer/craete piles.
		/// <para>
		/// Indicates the "mode" used when instantiate piles with <see cref="PileProperty"/>
		/// </para>
		/// </summary>
		public enum TransferMode
		{
			/// <summary>
			/// Indicates pile creation from PileProperty should take all needed properties, and the rest must be null/default.
			/// Raise an error if needed properties are missing and irrelevant ones are set.
			/// </summary>
			/// <remarks>This is the stricted mode.</remarks>
			STRICT = 10,

			/// <summary>
			/// Indicates pile creation from PileProperty should take all needed properties and ignore the rest.
			/// Raise an error if needed properties are missing.
			/// </summary>
			/// <remarks>Stricter than <see cref="INFERRED"/> but less  than <see cref="STRICT"/>.</remarks>
			ESSENTIAL = 20,

			/// <summary>
			/// Indicates pile creation from PileProperty should take all set properties and inferred missing ones.
			/// Needed but missing properties will be inferred with a default value.
			/// </summary>
			/// <remarks>The least strict <see cref="TransferMode"/></remarks>
			INFERRED = 30
		}
	}
}
