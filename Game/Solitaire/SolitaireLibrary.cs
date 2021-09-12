using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Threading.Tasks;
using PlayingCards.Game.Solitaire.XmlSerialization;


namespace PlayingCards.Game.Solitaire
{
    public class SolitaireLibrary
    {
        public enum FilterOptions
        {

        }

        public static readonly XName Namespace = "http://playingcards.org/solitaire";

        private readonly XmlSchemaSet m_schemaSets;
        private readonly XmlReader m_reader;
        private readonly XElement m_root;
        private IEnumerable<GameInfo> m_games;
        private IXElementConverter m_xconverter;

        public IXElementConverter XConverter
        {
            get { return m_xconverter; }
            set
            {
                m_xconverter = value == null ? throw new NotImplementedException() : value;
            }
        }

        public SolitaireLibrary(string schemaUri, string xmlUri, bool enableStream = false)
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
            XConverter = new DefaultXElementConverter(Namespace);
        }

        public void LoadGames()
        {
            m_games = from game in m_root.Elements(Namespace + "game") select XConverter.Convert(game);
        }

        public GameInfo FetchGame(int index)
        {
            throw new NotImplementedException();
        }

        public GameInfo Filter()
        {
            throw new NotImplementedException();
        }

        ~SolitaireLibrary()
        {
            m_reader?.Close();
        }

    }
}
