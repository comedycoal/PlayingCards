using System;

namespace PlayingCards.Random
{

	/// <summary>
	/// An Int32 Psuedo Random Number Generator using the MT19937 algorithm.
	/// Based on code from https://create.stephan-brumme.com/mersenne-twister/original.c
	/// by Takuji Nishimura and Makoto Matsumoto (Copyright (C) 1997 - 2002).
	/// </summary>
	public class MT19937 : IRandom
    {
        private const int N = 624;
        private const int M = 397;
        private const uint A = 0x9908B0DFu;
        private const uint UPPER_MASK = 0x80000000u;
        private const uint LOWER_MASK = 0x7fffffffu;

        private uint[] m_states;
        private int m_i;
        private uint m_seed;


        /// <summary>
        /// Create and initialize a generator, with seed from Environment.TickCount
        /// </summary>
        public MT19937() : this((uint)Environment.TickCount) { }

        /// <summary>
        /// Create and initialize a generator with a provided seed
        /// </summary>
        /// <param name="seed">An unsigned integer</param>
        public MT19937(uint seed)
        {
			Seed = seed;
        }

		/// <inheritdoc cref="IRandom.Seed"/>
		public uint Seed
		{
			get { return m_seed; }
			protected set { m_seed = value; Reset(); }
		}

        
        // The "Twist" funtion
        private void Twist()
        {
            for (int i = 0; i < N; ++i)
            {
                uint x = (m_states[i] & UPPER_MASK) + (m_states[(i + 1) % N] & LOWER_MASK);
                uint xA = x >> 1;
                if ((x % 2) != 0)
                    xA ^= A;
                m_states[i] = m_states[(i + M) % N] ^ xA;
            }

            m_i = 0;
        }


        /// <inheritdoc cref="IRandom.Next(int)"/>
        public int Next(int max)
        {
            return (int)((uint)this.Next() % max);
        }

        /// <inheritdoc cref="IRandom.Next()"/>
        public int Next()
        {
            if (m_i >= N)
                Twist();

            long val = m_states[m_i++];
            val ^= ((val >> 11) & 0xFFFFFFFFu);
            val ^= ((val << 7) & 0x9D2C5680u);
            val ^= ((val << 15) & 0xEFC60000u);
            val ^= (val >> 18);

            return (int)(val & 0xFFFFFFFFu);
        }

		/// <inheritdoc cref="IRandom.Reset()"/>
        public void Reset()
        {
            m_states = new uint[N];
            m_i = N + 1;

            m_states[0] = m_seed;
            for (m_i = 1; m_i < N; ++m_i)
                m_states[m_i] = (uint)(0xffffffffu &
                    (1812433253u * (m_states[m_i - 1] ^ (m_states[m_i - 1] >> 30)) + m_i));

            Twist();
        }
	}
}