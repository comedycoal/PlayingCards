//using PlayingCards.Primitives;
//using System;
//using System.Collections.Generic;

//namespace PlayingCards.Solitaire.Concrete
//{
//	public class PyramidTableau
//	{
//		#region Public constants
//		//====================//

//		public class Node<T>
//		{
//			private T m_content;
//			private Node<T> m_left;
//			private Node<T> m_right;
//			private Node<T> m_leftparent;
//			private Node<T> m_rightparent;


//			public Node()
//			{
//				m_content = default;
//				m_left = m_right = m_leftparent = m_rightparent = null;
//			}


//			public bool Empty => m_content == null;

//			public T Content => m_content;

//			public bool Removeable => m_left == null && m_right == null;

//			public Node<T> Left
//			{
//				get { return m_left; }
//				set
//				{
//					m_left = value;
//					if (value != null) value.m_rightparent = this;
//				}
//			}

//			public Node<T> Right
//			{
//				get { return m_right; }
//				set
//				{
//					m_right = value;
//					if (value != null) value.m_leftparent = this;
//				}
//			}

//			public Node<T> LeftParent => m_leftparent;

//			public Node<T> RightParent => m_rightparent;


//			public void Attach(T obj)
//			{
//				m_content = obj;
//			}

//			public T Pop()
//			{
//				if (Removeable)
//				{
//					if (m_leftparent.Right == this) m_leftparent.Right = null;
//					if (m_rightparent.Left == this) m_rightparent.Left = null;
//					return m_content;
//				}
//				return default;
//			}
//		}

//		//======================================================================//
//		#endregion

//		#region Fields
//		//==========//

//		private Node<Card> m_root;

//		private List<Node<Card>> m_availableNodes;

//		private int m_count;
//		//======================================================================//
//		#endregion


//		#region Constructors
//		//================//

//		public PyramidTableau(int levelCount)
//		{
//			List<Node<Card>> lastLvl = new List<Node<Card>>();
//			if (levelCount <= 0) levelCount = 1;
//			m_root = new Node<Card>();

//			lastLvl.Add(m_root);

//			for (int i = 2; i <= levelCount; i++)
//			{
//				List<Node<Card>> temp = new List<Node<Card>>();

//				Node<Card> lastNode = lastLvl[0];
//				lastNode.Left = new Node<Card>();

//				for (int j = 1; j < lastLvl.Count; j++)
//				{
//					lastNode.Right = lastLvl[j].Left = new Node<Card>();
//					temp.Add(lastNode);
//					lastNode = lastLvl[j];
//				}

//				lastNode.Right = new Node<Card>();
//				temp.Add(lastNode);

//				lastLvl = temp;
//			}

//			m_availableNodes = lastLvl;
//		}

//		//======================================================================//
//		#endregion

//		#region Static methods
//		//==================//

//		//======================================================================//
//		#endregion


//		#region Overloaded properties
//		//=========================//

//		public virtual int Count => throw new NotImplementedException();
//		//======================================================================//
//		#endregion


//		#region New properties
//		//==================//

//		public virtual bool Cleared => m_root == null;
//		//======================================================================//
//		#endregion


//		#region Overloaded methods
//		//======================//
//		//======================================================================//
//		#endregion


//		#region New methods
//		//===============//
//		//======================================================================//
//		#endregion
//	}
//}
