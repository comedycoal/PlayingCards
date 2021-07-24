using System;
using PlayingCards.Primitives;

namespace PlayingCards.Component.Solitaire
{
    /// <summary>
    /// Abstract class for pile build strategy.
    /// </summary>
    /// <remarks>
    /// A <see cref="BuildStrategy"/> mainly concerns with the following properties:
    /// <list type="number">
    /// <item>
    /// <para> Its <see cref="Setting.InitialRank"/>,
    /// dictates which <see cref="Rank"/> of a <see cref="Card"/> can be put on an empty pile.</para>
    /// </item>
    /// <item>
    /// <para>Its <see cref="Setting.InitialRank"/>,
    /// dictates the gap between two consecutive <see cref="Card"/> when building.</para>
    /// <para>A <see cref="Setting.RankGap"/> of 2 would allow a 3 to be put on an Ace and -3 an Ace on a 4.
    /// </para>
    /// <para>It is not unusual for a Solitaire game to employs such a build strategy, most often
    /// with <see cref="Setting.AllowWrapping"/> on.</para>
    /// </item>
    /// <item>Its <see cref="Setting.AllowWrapping"/>,
    /// which indicates whether wrapping is allowed, often used alongside plural <see cref="Setting.RankGap"/>.
    /// <para>Wrapping, for example, allows a King to be placed on an Ace when building down.</para>
    /// </item>
    /// </list>
    /// Concrete subclasses of <see cref="Setting"/> express the build strategy's integrity with suit correspondence.
    /// </remarks>
    public abstract class BuildStrategy
    {
        /// <summary>
        /// Exception for invalid card argument passing
        /// </summary>
        public class InvalidCardException: Exception
        {
            /// <summary>
            /// Constructor for InvalidCardException. No arguments are passed
            /// </summary>
            public InvalidCardException(): base("Invalid card object passed.") { }
        }

        /// <summary>
        /// Public structs encapsulate setting for a <see cref="BuildStrategy"/>
        /// </summary>
        public struct Setting
		{
            /// <summary>
            /// Intial rank allowed in the build, or <see cref="Rank.ANY_RANK"/> if not imposed.
            /// </summary>
            public Rank InitialRank;

            /// <summary>
            /// Rank gap of the build, or <see cref="BuildStrategy.ANY_GOES_GAP"/> if arbitrary order is imposed.
            /// </summary>
            public int RankGap;

            /// <summary>
            /// Indicates whether build allows wrapping (eg: from A to K) or not.
            /// </summary>
            public bool AllowWrapping;

            /// <summary>
            /// Public readonly constant. Default setting of a <see cref="BuildStrategy"/>: Any ranks no wrapping.
            /// </summary>
            public static readonly Setting Default;

            static Setting()
			{
                Default = new Setting(Rank.ANY_RANK, ANY_GOES_GAP, false);
			}

            /// <summary>
            /// Constructs a <see cref="Setting"/>
            /// </summary>
            /// <param name="initialRank">Initial Rank</param>
            /// <param name="rankGap">Rank Gap</param>
            /// <param name="allowWrapping">Allow Wrapping or not</param>
            public Setting(Rank initialRank, int rankGap=-1, bool allowWrapping=false)
			{
                InitialRank = initialRank;
                RankGap = rankGap;
                AllowWrapping = allowWrapping;
            }

			/// <summary>
			/// Constant that represents a standard build down non-wrapping strategy (usually used for <see cref="File"/>
			/// </summary>
			public static readonly Setting StandardFileSetting = new Setting(Rank.ANY_RANK, -1, false);

			/// <summary>
			/// Constant that represents a standard build up non-wrapping strategy (usually used for <see cref="Foundation"/>
			/// </summary>
			public static readonly Setting StandardFoundationSetting = new Setting(Rank.A_RANK, 1, false);
        }


        //-------// Public constants
        /// <summary>
        /// Constant, passed to constructor by default, signalling that ranking does not matter.
        /// </summary>
        public const int ANY_GOES_GAP = 0;


        //-------// Internal data
        private Setting m_buildSetting;


        //-------// Public properties, methods

        /// <summary>
        /// Readonly property. Retrieves the <see cref="Setting"/> of the build.
        /// </summary>
        public Setting BuildSetting => m_buildSetting;


        /// <summary>
        /// Contructs a <see cref="BuildStrategy"/>
        /// </summary>
        /// <param name="setting">A <see cref="Setting"/> instance.</param>
        protected BuildStrategy(Setting setting)
        {
            m_buildSetting = setting;
        }

        /// <summary>
        /// Copy constructors for <see cref="BuildStrategy"/>.
        /// </summary>
        /// <param name="other">A <see cref="BuildStrategy"/> instance.</param>
        protected BuildStrategy(BuildStrategy other) : this(other.BuildSetting) { }

        /// <summary>
        /// Examine whether <paramref name="front"/> can be put on top of <paramref name="rear"/> using the corresponding build strategy.
        /// </summary>
        /// <remarks>
        /// Only takes into acccount the inital rank, the rank gap and wrapping, suit correspondence is not checked.
        /// Override this method to accomodate suit correspondence of individual strategies.
        /// </remarks>
        /// <param name="rear">A card object</param>
        /// <param name="front">A card object</param>
        /// <returns><see langword="true"/> if the build strategy allows this formation; otherwise <see langword="false"/>.</returns>
        public virtual bool CanPile(Card rear, Card front)
        {
            if ((!Card.IsValid(rear) && rear != Card.EMPTY ) || !Card.IsValid(front))
				return false;

            if (rear == Card.EMPTY)
                return BuildSetting.InitialRank == Rank.ANY_RANK || front.CardRank == BuildSetting.InitialRank;

            int gap = (front.CardRank - rear.CardRank + Rank.RANK_COUNT) % Rank.RANK_COUNT;
            return BuildSetting.RankGap == ANY_GOES_GAP
                || (BuildSetting.AllowWrapping ? gap == BuildSetting.RankGap : gap == BuildSetting.RankGap && gap * BuildSetting.RankGap > 0);
        }
    }
    

    /// <summary>
    /// Concrete class for build strategy: build regardless of suit.
    /// </summary>
    /// <remarks>Giving the parameters of <see cref="Rank.ANY_RANK"/> and <see cref="BuildStrategy.ANY_GOES_GAP"/>
    /// essentially makes an arbitrary build strategy.</remarks>
    public class AnySuitStrategy : BuildStrategy
    {
        /// <summary>
        /// Constructs an arbitrary build strategy.
        /// </summary>
        public AnySuitStrategy() : base(Setting.Default) { }

        /// <summary>
        /// Contructs an <see cref="AnySuitStrategy"/>
        /// </summary>
        /// <param name="setting">A <see cref="BuildStrategy.Setting"/> instance.</param>
        public AnySuitStrategy(Setting setting) : base(setting) { }

        /// <summary>
        /// Contructs an <see cref="AnySuitStrategy"/> from an instance of <see cref="BuildStrategy"/>.
        /// </summary>
        /// <param name="other">A <see cref="BuildStrategy"/> instance.</param>
        public AnySuitStrategy(BuildStrategy other) : base(other) { }


        /// <inheritdoc cref="BuildStrategy.CanPile" path="//*[not(self::remarks)]"/>
        /// <remarks>Only concerns the cards ranking, disregards the suits altogether.</remarks>
        public override bool CanPile(Card rear, Card front)
        {
            return base.CanPile(rear, front);
        }        
    }


    /// <summary>
    /// Concrete class for build strategy: No building allowed
    /// </summary>
    public class NoBuildStrategy : BuildStrategy
    {
        /// <summary>
        /// Constructs a strategy that forbids bulding.
        /// </summary>
        public NoBuildStrategy() : base(Setting.Default) { }

        /// <inheritdoc cref="BuildStrategy.CanPile" path="//*[not(self::remarks)]"/>
        /// <remarks>No building is allowe with this strategy.</remarks>
        public override bool CanPile(Card rear, Card front)
        {
            return false;
        }        
    }


    /// <summary>
    /// Concrete class for build strategy: build in accordance of a suit.
    /// </summary>
    /// <remarks>
    /// The strategy ensures the 2 properties:
    /// <list type="number">
    /// <item>If <see cref="SameSuitStrategy.CorrespondentSuit"/> is not <see cref="Suit.ANY_SUIT"/>,
    /// the first card built needs to follow that suit.</item>
    /// <item>All subsequent cards must have the same suit as its previous.</item>
    /// </list>
    /// </remarks>
    public class SameSuitStrategy : BuildStrategy
    {
        //--------// Internal data
        private Suit m_correspondentSuit;


        //--------// Public properties and methods
        /// <summary>
        /// Readonly property. Retrieves the suit correspondence.
        /// </summary>
        public Suit CorrespondentSuit => m_correspondentSuit;


        /// <summary>
        /// Constructor for <see cref="SameSuitStrategy"/>
        /// </summary>
        /// <param name="setting">A <see cref="BuildStrategy.Setting"/> instance.</param>
        /// <param name="correspondenceSuit">A forced Suit correspondence</param>
        public SameSuitStrategy(Setting setting, Suit correspondenceSuit) : base(setting)
        {
            m_correspondentSuit = correspondenceSuit;
        }

        /// <summary>
        /// Contructs a <see cref="SameSuitStrategy"/> from a <see cref="BuildStrategy"/> instance.
        /// </summary>
        /// <param name="other">A <see cref="BuildStrategy"/> instance.</param>
        /// <param name="correspondenceSuit">Correspondent suit of the strategy.</param>
        public SameSuitStrategy(BuildStrategy other, Suit correspondenceSuit) : base(other)
        {
            m_correspondentSuit = correspondenceSuit;
        }


        /// <inheritdoc cref="BuildStrategy.CanPile" path="//*[not(self::remarks)]"/>
        /// <remarks><paramref name="front"/> and <paramref name="rear"/> needs to be in the same suit to be able to return true.</remarks>
        public override bool CanPile(Card rear, Card front)
        {
            return (rear == Card.EMPTY ? (CorrespondentSuit == Suit.ANY_SUIT || front.CardSuit == CorrespondentSuit) : rear.CardSuit == front.CardSuit)
                && base.CanPile(rear, front);
        }        
    }


    /// <summary>
    /// Concrete class for build strategy: build in accordance to suit color
    /// </summary>
    public class SameColorStrategy : BuildStrategy
    {
        /// <summary>
        /// Constructor for <see cref="SameColorStrategy"/>
        /// </summary>
        /// <param name="setting">A <see cref="BuildStrategy.Setting"/> instance.</param>
        public SameColorStrategy(Setting setting) : base(setting) { }

        /// <summary>
        /// Contructs a <see cref="SameColorStrategy"/> from a <see cref="BuildStrategy"/> instance.
        /// </summary>
        /// <param name="other">A <see cref="BuildStrategy"/> instance.</param>
        public SameColorStrategy(BuildStrategy other) : base(other) { }


        /// <inheritdoc cref="BuildStrategy.CanPile" path="//*[not(self::remarks)]"/>
        /// <remarks><paramref name="front"/> and <paramref name="rear"/> needs to be in the same color to be able to return true.</remarks>
        public override bool CanPile(Card rear, Card front)
        {
            return rear.CardSuit.IsRed == front.CardSuit.IsRed && base.CanPile(rear, front);
        }        
    }


    /// <summary>
    /// Concrete class for build strategy: build alternating the color of suit
    /// </summary>
    public class AlternatingColorStrategy : BuildStrategy
    {
        /// <summary>
        /// Constructor for <see cref="AlternatingColorStrategy"/>
        /// </summary>
        /// <param name="setting">A <see cref="BuildStrategy.Setting"/> instance.</param>
        public AlternatingColorStrategy(Setting setting) : base(setting) { }

        /// <summary>
        /// Contructs an <see cref="AlternatingColorStrategy"/> from a <see cref="BuildStrategy"/> instance.
        /// </summary>
        /// <param name="other">An <see cref="BuildStrategy"/> instance.</param>
        public AlternatingColorStrategy(BuildStrategy other) : base(other) { }


        /// <inheritdoc cref="BuildStrategy.CanPile" path="//*[not(self::remarks)]"/>
        /// <remarks><paramref name="front"/> and <paramref name="rear"/> needs to be in different colors to be able to return true.</remarks>
        public override bool CanPile(Card rear, Card front)
        {
            return rear.CardSuit.IsRed != rear.CardSuit.IsRed && base.CanPile(rear, front);
        }        
    }


    /// <summary>
    /// Concrete class for build strategy: build without repeating suit
    /// </summary>
    public class DifferentSuitStrategy : BuildStrategy
    {
        /// <summary>
        /// Constructor for <see cref="DifferentSuitStrategy"/>
        /// </summary>
        /// <param name="setting">A <see cref="BuildStrategy.Setting"/> instance.</param>
        public DifferentSuitStrategy(Setting setting) : base(setting) { }

        /// <summary>
        /// Contructs a <see cref="DifferentSuitStrategy"/> a <see cref="BuildStrategy"/> instance.
        /// </summary>
        /// <param name="other">A <see cref="BuildStrategy"/> instance.</param>
        public DifferentSuitStrategy(BuildStrategy other) : base(other) { }


        /// <inheritdoc cref="BuildStrategy.CanPile" path="//*[not(self::remarks)]"/>
        /// <remarks><paramref name="front"/> and <paramref name="rear"/> needs to be in different suits to be able to return true.</remarks>
        public override bool CanPile(Card rear, Card front)
        {
            return rear.CardSuit != front.CardSuit && base.CanPile(rear, front);
        }
    }
}