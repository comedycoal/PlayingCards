namespace PlayingCards.Random
{
	/// <summary>
	/// Interface for random number generators suitable for uses with <see cref="PlayingCards.Primitives.Deck"/>.
	/// </summary>
	public interface IRandom
    {
		/// <summary>
		/// Engine seed.
		/// </summary>
		public uint Seed { get; set; }

        /// <summary>
        /// Generate the next random number from the generator.
        /// Generated value should be equavalent to any other MT19937 generators
        /// </summary>
        /// <returns>An integer</returns>
        public int Next();

        /// <summary>
        /// Generate and interpolate the next random number from the generator.
        /// Generated value is positive and never equals to or larger than <paramref name="max"/>.
        /// </summary>
        /// <param name="max">An integer</param>
        /// <returns>A positive integer in range of <see langword="[0, max)"/>.</returns>
        public int Next(int max);

        /// <summary>
        /// Reset the generator to its initial state
        /// </summary>
        public void Reset();
    }
}