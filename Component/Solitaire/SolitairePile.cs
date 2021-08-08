using System;
using System.Collections.Generic;
using System.Reflection;

using PlayingCards.History;
using PlayingCards.History.Solitaire;
using PlayingCards.Primitives;

namespace PlayingCards.Component.Solitaire
{
	/// <summary>
	/// An abstract class encapsulating a "standard" pile. A pile has a name, and a vairant.
	/// </summary>
	/// <remarks>
	/// A standard pile has the following properties:
	/// <list>1. Forms two consecutive partitions: <strong>Faced-down</strong> cards, and <strong>Faced-ups</strong>.</list>
	/// <list>2. Is built with a <see cref="BuildStrategy"/>.</list>
	/// <list>3. Can  receives a arbitrary but definite set of cards, or nothing before adhering to its <see cref="BuildStrategy"/>.
	/// This property is quantified as <see cref="InitialCount"/></list>
	/// <list>4. Only a set of cards at the end can be moved, those are deemed "available", the rest is consider "blocked".
	/// If a card is moved, all its subsequent cards also follows, or can be moved on their own.</list>
	/// <list>5. A faced-down card should not be read under normal gameplay circumstances,
	/// any queries on these should returns <see cref="Card.FACED_DOWN"/>.</list>
	/// Any subclass must ensure these properties.
	/// </remarks>
	public abstract class SolitairePile
	{
		// Empty
		#region Public constants
		//====================//

		private static Dictionary<System.Type, string[]> c_names;


		static SolitairePile()
		{
			c_names = new Dictionary<Type, string[]>();
		}
		//======================================================================//
		#endregion

		#region Fields
		//==========//

		private int m_initialCount;

		/// <summary></summary>
		protected BuildStrategy m_buildStrategy;

		private Pile<Card> m_cards;
		private IGameContext m_gameContext;
		//======================================================================//
		#endregion


		#region Constructors
		//================//

		/// <summary>
		/// Constructs a <see cref="SolitairePile"/> with <see cref="InitialCount"/> = 0 and <see cref="PileBuildStrategy"/> = <see cref="AnySuitStrategy"/>
		/// </summary>
		protected SolitairePile(IGameContext gameContext)
		{
			m_initialCount = 0;
			m_buildStrategy = new AnySuitStrategy();
			m_cards = new Pile<Card>();
			m_gameContext = gameContext;
		}

		//======================================================================//
		#endregion

		// 1 new
		#region Static methods
		//==================//

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="strs"></param>
		protected static void AddRecord(Type type, params string[] strs)
		{
			SolitairePile.c_names.Add(type, strs);
		}

		/// <summary>
		/// Constructs into <paramref name="context"/> and returns a <see cref="SolitairePile"/> of type <paramref name="type"/>
		/// with properties drew from <paramref name="property"/> using prpperty transfer mode <paramref name="mode"/>.
		/// </summary>
		/// <param name="context">The <see cref="IGameContext"/> that the pile belongs to.</param>
		/// <param name="type">The type of the pile.</param>
		/// <param name="interfaceReturn">Returns whether the pile instantiated is also a interface' instance.</param>
		/// <param name="property">The properties of the pile.</param>
		/// <param name="mode">The <see cref="PileProperty.TransferMode"/> used.</param>
		/// <returns>A <see cref="SolitairePile"/> instance.</returns>
		public static SolitairePile CreatePile(IGameContext context,
										Type type,
										out InterfaceReturn interfaceReturn,
										PileProperty property,
										PileProperty.TransferMode mode = PileProperty.TransferMode.STRICT)
		{
			if (!typeof(SolitairePile).IsAssignableFrom(type))
				throw new NotImplementedException();

			var instance = Activator.CreateInstance(type, context);

			interfaceReturn = new InterfaceReturn();

			if (typeof(IFoundation).IsAssignableFrom(type))
				interfaceReturn.FoundationReference = (IFoundation)instance;
			if (typeof(IDealer).IsAssignableFrom(type))
				interfaceReturn.DealerReference = (IDealer)instance;

			var pile = (SolitairePile)instance;

			bool fieldsGot = c_names.TryGetValue(type, out string[] enforcedFields);
			if (fieldsGot)
			{
				var highestMode = PileProperty.GetMostApplicableTransferMode(property, enforcedFields);
				if (highestMode <= mode)
				{
					if (mode == PileProperty.TransferMode.INFERRED)
						foreach (var fieldName in enforcedFields)
						{
							var fieldInfo = property.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
							if (fieldInfo.GetValue(property) is null)
								fieldInfo.SetValue(property, PileProperty.GetDefaultNoneNullValue(fieldInfo));
						}
					pile.SetProperties(property);
					return pile;
				}
				else
				{
					throw new NotImplementedException();
				}
			}
			else
			{
				// throw something 3
				throw new NotImplementedException();
			}
		}
		//======================================================================//
		#endregion


		// 9 new, 4 of which abstract
		#region New properties
		//==================//

		/// <summary>
		/// Readonly property. Retrieves the <see cref="IGameContext"/> that contains the pile.
		/// </summary>
		public IGameContext GameContext
		{
			get { return m_gameContext; }
			protected set { m_gameContext = value; }
		}

		/// <summary>
		/// Readonly property. Retrieves the number of cards present in the pile.
		/// </summary>
		public virtual int Count => m_cards.Count;

		/// <summary>
		/// Expose the pile's internal card list.
		/// </summary>
		public virtual Pile<Card> Cards { get { return m_cards; } protected set { m_cards = value; } }
        
		/// <summary>
		/// Readonly property. Represent the quantity once the number of cards reach, its <see cref="BuildStrategy"/> takes effect.
		/// </summary>
		/// <remarks>In normal gameplay, this quantity is most often reached during dealing.</remarks>
		public virtual int InitialCount => m_initialCount;

		/// <summary>
		/// Readonly property. Retrieves the index of the FIRST faced-up <see cref="Card"/> instance in the pile.
		/// </summary>
		public abstract int PartitionIndex { get; protected set; }

		/// <summary>
		/// Readonly property. Retrieves the index of the FIRST available <see cref="Card"/> instance in the pile.
		/// </summary>
        public abstract int AvailableIndex { get; protected set; }
        


        /// <summary>
        /// Public readonly property. Retrieves a reference to the top card, if allowed.
        /// </summary>
		/// <remarks>
        /// Calls <see cref="GetCard(int)"/> with index = 0
		/// </remarks>
        public virtual Card TopCard => GetCard(Count == 0 ? 0 : Count - 1);

        /// <summary>
        /// Public readonly property. Retrieves a reference to the bottom card, if allowed.
        /// </summary>
		/// <remarks>
        /// Calls <see cref="GetCard(int)"/> with index = 0
		/// </remarks>
        public virtual Card BottomCard => GetCard(0);

        /// <summary>
        /// Public readonly property. Retrieves the <see cref="BuildStrategy"/> of the pile.
        /// </summary>
        public BuildStrategy PileBuildStrategy => m_buildStrategy;



		/// <summary>
		/// Readonly property. Indicates whether the pile can be "filled", i.e to have a maximum capacity.
		/// </summary>
		public abstract bool Fillable { get; }

		/// <summary>
		/// Readonly property. Indicates whether the pile can be cleared, meaning it looks forward to holding no cards.
		/// </summary>
		public abstract bool Clearable { get; }
        //======================================================================//
        #endregion


		// 6 new, 1 of which abstract
        #region New methods
        //===============//

        /// <summary>
        /// Force retrieval of a <see cref="Card"/> instance with actual data, regardless of its faced-down status.
        /// </summary>
        /// <param name="index">index to retrieve</param>
        /// <returns>A <see cref="Card"/> instance.
		/// <list>May return <see cref="Card.EMPTY"/> if <paramref name="index"/> is <see langword="0"/> and the pile is empty.</list>
		/// <list>May return <see cref="Card.NONE"/> if <paramref name="index"/> is out of range.</list>
		/// </returns>
        protected virtual Card ForceGetCard(int index)
        {
            if (index >= 0 && index < this.Count)
                return this.m_cards[index];

            return GetCard(index);
        }

        /// <summary>
        /// Retrieves the <see cref="Card"/> instance at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index to retrieve.</param>
        /// <returns>A <see cref="Card"/> instance.
		/// <list>May return <see cref="Card.FACED_DOWN"/> if the card at <paramref name="index"/> is not visible.</list>
		/// <list>May return <see cref="Card.EMPTY"/> if <paramref name="index"/> is <see langword="0"/> and the pile is empty.</list>
		/// <list>May return <see cref="Card.NONE"/> if <paramref name="index"/> is out of range.</list>
		/// </returns>
        public virtual Card GetCard(int index)
        {
			if (index < 0)
				throw new System.NotImplementedException();

            if (Count == 0 && index == 0)
                return Card.EMPTY;
            
            if (index >= Count)
                return Card.NONE;

            if (index < PartitionIndex)
                return Card.FACED_DOWN;

            return m_cards[index];
        }

		/// <summary>
		/// Set relevant properties inferred form <paramref name="properties"/>
		/// </summary>
		/// <param name="properties"></param>
		protected virtual void SetProperties(PileProperty properties)
		{
			m_initialCount = properties.InitialCount ?? (int)PileProperty.GetDefaultNoneNullValue("InitialCount");
			m_buildStrategy = properties.PileBuildStrategy ?? (BuildStrategy)PileProperty.GetDefaultNoneNullValue("PileBuildStrategy");
		}

		/// <summary>
		/// Retrieves a <see cref="PileProperty"/>, applicable for instantiation on <see cref="PileProperty.TransferMode.STRICT"/> mode.
		/// </summary>
		/// <returns>A <see cref="PileProperty"/> instance.</returns>
		public virtual PileProperty GetProperties()
		{
			PileProperty property = new PileProperty
			{
				InitialCount = InitialCount,
				PileBuildStrategy = PileBuildStrategy
			};
			return property;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string str = GetType().Name + " { ";
			for (int i = 0; i < Count; ++i)
				str += GetCard(i).ToString() + " ";
			str += "}";
			return str;
		}

		/// <summary>
		/// Modify <paramref name="transferData"/> to include one or more <see cref="IMove"/>, if necessary, 
		/// to be executed after the transfer is performed.
		/// </summary>
		/// <remarks>
		/// Used in conjunction with <see cref="SolitairePile.CreateTransfer"/>, signifies an action made ON the SOURCE pile.
		/// <para>Implementation of this method should only modify <paramref name="transferData"/> via the <see cref="TransferData{T}.AddAction"/> method.</para>
		/// </remarks>
		/// <param name="transferData">Reference to a <see cref="TransferData{Card}"/></param>
		protected abstract void OnAddition(ref TransferData<Card> transferData);

		/// <summary>
		/// Modify <paramref name="transferData"/> to include one or more <see cref="IMove"/>, if necessary, 
		/// to be executed after the transfer is performed.
		/// </summary>
		/// <remarks>
		/// Used in conjunction with <see cref="SolitairePile.CreateTransfer"/>, signifies an action made ON the DESTINATION pile.
		/// <para>Implementation of this method should only modify <paramref name="transferData"/> via the <see cref="TransferData{T}.AddAction"/> method.</para>
		/// </remarks>
		protected abstract void OnExtraction(ref TransferData<Card> transferData);

		/// <summary>
		/// On a solitaire pile level, examine if <paramref name="transferData"/> can be performed to the pile.
		/// </summary>
		/// <param name="transferData">The <see cref="TransferData{Card}"/> to perform.</param>
		/// <returns></returns>
		protected virtual bool AllowTransfer(TransferData<Card> transferData)
		{
			return PileBuildStrategy.CanPile(TopCard, transferData.First);
		}

		/// <summary>
		/// Creates an <see cref="IMove"/> that performs a transfer of <paramref name="count"/> top cards to <paramref name="destination"/>.
		/// </summary>
		/// <remarks>
		/// <see cref="OnAddition"/> is called on <paramref name="destination"/> and <see cref="OnExtraction"/> is called on this pile.
		/// </remarks>
		/// <param name="destination">Recipient of the cards.</param>
		/// <param name="count">How many to extract.</param>
		/// <returns><see langword="null"/> if such a transfer is not possible, otherwise an executable <see cref="IMove"/>.</returns>
		public virtual IMove CreateTransfer(SolitairePile destination, int count)
		{
			IMove move = null;
			var index = Count - count;
				
			if (index >= AvailableIndex)
			{
				TransferData<Card> data = Cards.Peek(count);

				this.OnExtraction(ref data);
				destination.OnAddition(ref data);

				if (destination.AllowTransfer(data))
					move = new TransferMove(destination.Cards, data);
			}
			return move;
		}
		//======================================================================//
		#endregion
	}
}