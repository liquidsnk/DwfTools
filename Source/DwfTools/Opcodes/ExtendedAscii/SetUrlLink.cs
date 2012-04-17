using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace DwfTools.W2d.Opcodes
{
    public class SetUrlLink : IOpcode
    {
        public struct UrlLink : IOpcode
        {
            public class Factory : OpcodeFactory
            {
                public override IOpcode ReadOpcode(Stream stream)
                {
                    AdvanceThroughWhitespace(stream);

                    var index = ReadIntegerString(stream);

                    AdvanceThroughWhitespace(stream);

                    var address = ReadString(stream);
                    if (ExtendedAsciiHasEnded(stream)) return new UrlLink(index, address);

                    AdvanceThroughWhitespace(stream);

                    var name = ReadString(stream);
                    AdvanceThroughWhitespace(stream);
                    ExtendedAsciiHasEnded(stream);

                    return new UrlLink(index, address, name);
                }
            }

            public UrlLink(long index, string address)
                : this()
            {
                Index = index;
                Address = address;
            }

            public UrlLink(long index, string address, string name)
                : this()
            {
                Index = index;
                Address = address;
                Name = name;
            }

            public readonly long Index;

            public readonly string Address;

            public readonly string Name;

            public override string ToString()
            {
                string linkString = string.Format("{0} {1} {2}", Index, Address, Name).Trim();
                return string.Format("({0})", linkString);
            }

            public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
        }

        public class Factory : OpcodeFactory, IExtendedAsciiOpcodeFactory
        {
            public string OpcodeId { get { return SetUrlLink.Id; } }

            public override IOpcode ReadOpcode(Stream stream)
            {
                AdvanceThroughWhitespace(stream);
                if (ExtendedAsciiHasEnded(stream)) return new SetUrlLink();
                AdvanceThroughWhitespace(stream);

                long index = 0;
                var urlLinksStarted = ExtendedAsciiIsBeginning(stream);
                if (!urlLinksStarted)
                {
                    index = ReadIntegerString(stream);
                }

                AdvanceThroughWhitespace(stream);
                if (!urlLinksStarted && !ExtendedAsciiIsBeginning(stream))
                {
                    return new SetUrlLink(index);
                }
                AdvanceThroughWhitespace(stream);

                var urlLinkFactory = new UrlLink.Factory();
                var urlLinks = new List<UrlLink>();
                do
                {

                    UrlLink urlLink = (UrlLink)urlLinkFactory.ReadOpcode(stream);
                    urlLinks.Add(urlLink);
                    AdvanceThroughWhitespace(stream);
                }
                while (!ExtendedAsciiHasEnded(stream));

                return new SetUrlLink(index, urlLinks);
            }
        }

        public static readonly string Id = "URL";

        public SetUrlLink()
        {
            Links = new UrlLink[0];
        }

        public SetUrlLink(long index)
            : this()
        {
            Index = index;
        }

        public SetUrlLink(long index, IEnumerable<UrlLink> links)
        {
            Index = index;
            Links = links.ToArray();
        }

        public readonly long Index;

        public readonly UrlLink[] Links;
        
        public override string ToString()
        {
            string linksString = Links.Length != 0 ? string.Join(",", Links) : string.Empty;
            return string.Format("(Index: {0} Elements: {1})", Index, linksString).Trim();
        }

        public CoordinatesType CoordinatesType { get { return CoordinatesType.Undefined; } }
    }
}
