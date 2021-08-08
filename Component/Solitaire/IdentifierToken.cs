using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingCards.Component.Solitaire
{
	/// <summary>
	/// Struct provides an identifier for an object with type identifiable by a <see cref="string"/>,
	/// given a <see cref="IdentifierTokenContext"/>.
	/// </summary>
	/// <remarks>
	/// In the same context, two <see cref="IdentifierToken"/> is deemed identifying the same "object" if
	/// they have the same internal "type" and "id".
	/// </remarks>
	public struct IdentifierToken
	{
		private static IdentifierTokenContext InternalContext;
		private static IdentifierTokenContext DefaultContext;

		/// <summary>
		/// A "null" identifier that does not refer to an entity.
		/// </summary>
		public static IdentifierToken None;

		static IdentifierToken()
		{
			InternalContext = new IdentifierTokenContext();
			DefaultContext = new IdentifierTokenContext();

			var dummyContext = new IdentifierTokenContext();
			None = new IdentifierToken("", dummyContext);
		}

		/// <summary>
		/// Register an <see cref="IdentifierTokenContext"/> as the internal context. 
		/// </summary>
		/// <param name="context">A non-null context.</param>
		public static void AttachContext(IdentifierTokenContext context)
		{
			if (context == null)
				throw new NotImplementedException();
			InternalContext = context;
		}

		/// <summary>
		/// Remove the internal context. Assign a new, anonymous context if specified by <paramref name="renew"/>.
		/// </summary>
		/// <param name="renew">if <see langword="true"/>, assign to the internal context a new, empty one, otherwise leave it null.</param>
		/// <returns></returns>
		public static IdentifierTokenContext ReleaseContext(bool renew=false)
		{
			var a = InternalContext;
			InternalContext = null;
			if (renew) InternalContext = new IdentifierTokenContext();
			return a;
		}

		private IdentifierTokenContext m_context;
		public string Type { get; set; }
		public int Id { get; set; }

		/// <summary>
		/// Readonly property. Retrieves the <see cref="IdentifierTokenContext"/> that this token belongs to.
		/// </summary>
		/// <remarks>
		/// This property returns null on <see cref="IdentifierToken.None"/>
		/// </remarks>
		public IdentifierTokenContext Context => this == None ? null : m_context;

		/// <summary>
		/// Instantiate an <see cref="IdentifierToken"/> into <paramref name="context"/> from an "identifier" string.
		/// </summary>
		/// <param name="str">Identifier string.</param>
		/// <param name="context">An <see cref="IdentifierTokenContext"/> instance.</param>
		/// <remarks>
		/// A pile's <see cref="IdentifierToken"/> should only be instantiated by Game-level classes,
		/// an individual pile does not need to know its identifier nor it should.
		/// <para>
		/// An <see cref="IdentifierToken"/> is always instantiated INTO an <see cref="IdentifierTokenContext"/>,
		/// one should be specified upon creation of a new <see cref="IdentifierToken"/>, if not, a specified "internal", or "default" context will be provided.
		/// </para>
		/// <para>
		/// <list type="bullet">
		/// <item>The internal context is specified by <see cref="AttachContext"/> and removed by <see cref="ReleaseContext"/>.
		/// It takes precedent to the default context.</item>
		/// <item>The default context is constant and cannot be changed to ensure all instances of <see cref="IdentifierToken"/> has a context.</item>
		/// </list>
		/// </para>
		/// </remarks>
		public IdentifierToken(string str, IdentifierTokenContext context=null)
		{
			context ??= InternalContext is null ? DefaultContext : InternalContext;
			Type = str;
			m_context = context;
			Id = context.Add(str);
		}

		/// <summary>
		/// Determines whether the token identifies the same "Type" as <paramref name="other"/> in the same context.
		/// </summary>
		/// <remarks>
		/// "Type" is determined by the <see cref="string"/> passing during the construction.</remarks>
		/// <param name="other"></param>
		/// <returns><see langword="true"/> if the token identifies the same type as <paramref name="other"/>,
		/// and they are in the same context, else <see langword="false"/>.</returns>
		public bool TypeEqual(IdentifierToken other)
		{
			return this != None && other != None && (m_context == other.m_context && Type == other.Type);
		}

		/// <summary>
		/// Determines whether the token identifies a object of <paramref name="typeName"/>.
		/// </summary>
		/// <param name="typeName"></param>
		/// <returns><see langword="true"/> if the token identifies <paramref name="typeName"/>,
		/// else <see langword="false"/>.</returns>
		public bool IsType(string typeName)
		{
			return this != None && Type == typeName;
		}

		/// <inheritdoc cref="object.Equals(object?)"/>
		public override bool Equals(object obj)
		{
			return this == (IdentifierToken)obj;
		}

		/// <inheritdoc cref="object.GetHashCode"/>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <inheritdoc cref="object.ToString"/>
		public override string ToString()
		{
			if (this == None) throw new NotImplementedException();
			return m_context.GetSuggestedName(Type, Id).ToString();
		}

		/// <summary>
		/// Overloaded == operator. Performs equivalent check on two <see cref="IdentifierToken"/> instances.
		/// </summary>
		/// <remarks>
		/// Two <see cref="IdentifierToken"/> are equal if they identifies the same object, and in the same context.
		/// </remarks>
		/// <param name="a">Left-hand <see cref="IdentifierToken"/> instance operand</param>
		/// <param name="b">Right-hand <see cref="IdentifierToken"/> instance operand</param>
		/// <returns><see langword="true"/> if the two instances are equal, otherwise <see langword="false"/>.</returns>
		public static bool operator ==(IdentifierToken a, IdentifierToken b)
		{
			return  a.TypeEqual(b) && a.Id == b.Id;
		}

		/// <summary>
		/// Overloaded != operator. Performs equivalent check on two <see cref="IdentifierToken"/> instances.
		/// </summary>
		/// <param name="a">Left-hand <see cref="IdentifierToken"/> instance operand</param>
		/// <param name="b">Right-hand <see cref="IdentifierToken"/> instance operand</param>
		/// <returns><see langword="true"/> if the two instances are not equal, otherwise <see langword="false"/>.</returns>
		public static bool operator !=(IdentifierToken a, IdentifierToken b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Converts a "string" into an <see cref="IdentifierToken"/> in the current "internal" context, if specified beforehand, or the "default" one.
		/// </summary>
		/// <param name="a"></param>
		public static explicit operator IdentifierToken(string a)
		{
			return new IdentifierToken(a);
		}
	}
}
