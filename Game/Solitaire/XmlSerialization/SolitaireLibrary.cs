using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Linq;


namespace PlayingCards.Game.Solitaire.XmlSerialization
{
	public class SolitaireLibrary
	{
		public static readonly XName Namespace = "http://playingcards.org/solitaire";

		private readonly XmlSchemaSet m_schemaSets;
		private readonly XmlReader m_reader;
		private readonly XElement m_root;
		private readonly bool m_streamEnabled;

		//private IEnumerable<XElement> GetXElementEnumerable()
		//{
		//	if (!m_streamEnabled)
		//		throw new NotImplementedException();

		//	m_reader.MoveToContent();
		//	while (m_reader.Read())
		//	{
		//		if (m_reader.NodeType == XmlNodeType.Element)
		//		{
		//			if (m_reader.Name == "game")
		//			{
		//				XElement ele = XNode.ReadFrom(m_reader) as XElement;
		//				if (ele != null)
		//				{
		//					yield return ele;
		//				}
		//			}
		//		}
		//	}
		//}

		public SolitaireLibrary(string schemaUri, string xmlUri, bool enableStream=false)
		{
			m_schemaSets = new XmlSchemaSet();
			m_schemaSets.Add(Namespace.NamespaceName, schemaUri);
			if (enableStream)
			{
				m_reader = XmlReader.Create(xmlUri);
				m_root = new XElement("library", null);
			}
			else
			{
				var doc = XDocument.Load(xmlUri);
				doc.Validate(m_schemaSets, null); // Throw errors on invalid
				m_root = doc.Element(Namespace + "library");
				m_reader = null;
			}
		}

		public XElement Fetch(int index)
		{
			return m_root.Elements().ElementAt(index);
		}

		~SolitaireLibrary()
		{
			m_reader?.Close();
		}

	}
}
