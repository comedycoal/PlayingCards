using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using PlayingCards.Extensions;
using PlayingCards.Random;

namespace PlayingCards.Primitives
{
    /// <summary>
    /// Class for an immutable data-centric container of cards, usually represents the set
	/// that can be played throughout a whole game.
    /// </summary>
	/// <remarks>
	/// The class provides numerous methods to create and manipulate decks of cards.
	/// New decks are retrieved using provided static methods, while masking its constructors.
	/// </remarks>
    public class Deck
    {
        #region Public constants
        //====================//
        
        /// <summary>
        /// Bitmask enumeration to streamline deck creation.
        /// </summary>
		/// <remarks>
		/// Used as arguments for static creation methods to modify how the deck should be sorted.
		/// <para>The default order is: Grouped by Suit, from Hearts to Spades and ranks from A to K</para>
		/// </remarks>
        [Flags]
        public enum SortingRule
        {
            /// <summary>
            /// Flag dictates grouping by rank (all cards of the same rank goes together)
            /// </summary>
            GROUP_BY_RANK = 0b_0001,

            /// <summary>
            /// Flag dictates suit is sorted in decreasing order (Spades to Hearts)
            /// </summary>
            SUIT_DECREASE = 0b_0010,

            /// <summary>
            /// Flag dictates rank is sorted in decreasing order
            /// </summary>
            RANK_DECREASE = 0b_0100
        }
        //======================================================================//
        #endregion


        #region Fields
        //==========//
        private List<Card> m_cards;
        //======================================================================//
        #endregion


        #region Constructors
        //================//

        private Deck()
        {
            m_cards = new List<Card>();
        }

        private Deck(List<Card> cards)
        {
            m_cards = cards.ConvertAll(card => new Card(card));
        }

        private Deck(List<Suit> suitList, List<Rank> rankList, SortingRule rule=0)
        {
            this.m_cards = new List<Card>();

			var suitValueList = suitList.ConvertAll<int>(suit => (int)suit);
			var rankValueList = rankList.ConvertAll<int>(rank => (int)rank);

			List<int> outerL, innerL;
            bool outerIncreased, innerIncreased;
            bool bysuit = (rule & SortingRule.GROUP_BY_RANK) != SortingRule.GROUP_BY_RANK;
            if (bysuit)
            {
                outerL = suitValueList;
                innerL = rankValueList;
                outerIncreased = (rule & SortingRule.SUIT_DECREASE) != SortingRule.SUIT_DECREASE;
                innerIncreased = (rule & SortingRule.RANK_DECREASE) != SortingRule.RANK_DECREASE;
            }
            else
            {
                outerL = rankValueList;
                innerL = suitValueList;
                outerIncreased = (rule & SortingRule.RANK_DECREASE) != SortingRule.RANK_DECREASE;
                innerIncreased = (rule & SortingRule.SUIT_DECREASE) != SortingRule.SUIT_DECREASE;
            }

            foreach (int i in outerIncreased ? outerL : outerL.GetReversedEnumerator())
            {
                foreach (int j in innerIncreased ? innerL : innerL.GetReversedEnumerator())
                {
                    this.m_cards.Add(bysuit ? new Card((Suit)i, (Rank)j) : new Card((Suit)j, (Rank)i));
                }
            }
        }
        //======================================================================//
        #endregion


        #region Static methods
        //==================//

        /// <summary>
        /// Retrieves a <see cref="Deck"/> consists of <paramref name="count"/> full sets of cards, each set follows another and concurs to a sortingRule.
        /// </summary>
        /// <param name="count">Set counts</param>
        /// <param name="sortRule">Sorting rule</param>
        /// <returns>A Deck object</returns>
        public static Deck GetFullDecks(int count=1, SortingRule sortRule=0)
        {
            Deck temp = new Deck();
            for (int i = 0; i < count; ++i)
            {
				Deck a = new Deck(Suit.FULL_SUITS_LIST.ToList<Suit>(), Rank.RANK_LIST.ToList<Rank>(), sortRule);
                temp.MergeWith(a);
            }
            return temp;
        }
        
        /// <summary>
        /// Retrieves a <see cref="Deck"/> consists of cards in selected suits.
        /// </summary>
        /// <param name="suits">List of Suits to pick from</param>
        /// <param name="sortRule">Sorting rule</param>
        /// <returns>A Deck object</returns>
        public static Deck GetSelectedSuitDeck(List<Suit> suits, SortingRule sortRule=0)
        {
			return new Deck(suits, Rank.RANK_LIST.ToList<Rank>(), sortRule);
        }

        /// <summary>
        /// Retrieves a <see cref="Deck"/> consists of selected cards.
        /// </summary>
        /// <param name="cards">A list of cards</param>
        /// <returns>A Deck object</returns>
        public static Deck GetCustomSuit(List<Card> cards)
        {
            return new Deck(cards);
        }

        //======================================================================//
        #endregion


        #region New properties
        //==================//
        
        /// <summary>
        /// Readonly property. Retrieves count of cards in deck
        /// </summary>
        public int Count => m_cards.Count;
        //======================================================================//
        #endregion


        #region New methods
        //===============//

        /// <summary>
        /// Merge deck with another <see cref="Deck"/> object, new cards are added at the end.
        /// </summary>
        /// <param name="other">A new <see cref="Deck"/> object</param>
        public void MergeWith(Deck other)
        {
            this.m_cards.AddRange(other.m_cards.ConvertAll(card => new Card(card)));
        }

        /// <summary>
        /// Retrives a <see langword="List&lt;Card&gt;"/> instance shuffled from the deck as a copy,
        /// using the Fisher-Yates shuffle algorithm and a MT19937 generator (with a system given seed).
        /// </summary>
        /// <returns>A <see langword="List&lt;Card&gt;"/> instance</returns>
        public List<Card> Shuffle()
        {
            MT19937 rng = new MT19937();
            return Shuffle(rng);
        }

        /// <summary>
        /// Retrives a <see langword="List&lt;Card&gt;"/> instance shuffled from the deck (with a Random generator) as a copy.
        /// </summary>
        /// <param name="rng">A random generator implementing <see cref="PlayingCards.Random.IRandom"/> interface</param>
        /// <returns>A <see langword="List&lt;Card&gt;"/> instance</returns>
        public List<Card> Shuffle(IRandom rng)
        {
            List<Card> toshuffle = Get();
            for (int i = m_cards.Count - 1; i >= 1; --i)
            {
                int index = rng.Next(i);
                Card tempref = toshuffle[i];
                toshuffle[i] = toshuffle[index];
                toshuffle[index] = tempref;
            }
            return toshuffle;
        }

        /// <summary>
        /// Retrives a <see langword="List&lt;Card&gt;"/> instance shuffled from the deck as a copy,
        /// using the Fisher-Yates shuffle algorithm and a MT19937 generator (with user provided seed).
        /// </summary>
        /// <param name="seed">seed to generate random shuffle</param>
        /// <returns>A <see langword="List&lt;Card&gt;"/> instance</returns>
        public List<Card> Shuffle(uint seed)
        {
            MT19937 rng = new MT19937(seed);
            return Shuffle(rng);
        }

        /// <summary>
        /// Retrives a copy of the deck as a <see langword="List&lt;Card&gt;"/> instance.
        /// </summary>
        /// <returns>A <see langword="List&lt;Card&gt;"/> instance contains all cards in the deck</returns>
        public List<Card> Get()
        {
            return m_cards.ConvertAll(card => new Card(card));
        }

        /// <summary>
        /// Retrives a copy of the deck in reversed order as a <see langword="List&lt;Card&gt;"/> instance.
        /// </summary>
        /// <returns>A <see langword="List&lt;Card&gt;"/> instance contains all cards in the deck in reversed order</returns>
        public List<Card> Reverse()
        {
            List<Card> reversed = Get();
            reversed.Reverse();
            return reversed;
        }
        //======================================================================//
        #endregion
    }
}