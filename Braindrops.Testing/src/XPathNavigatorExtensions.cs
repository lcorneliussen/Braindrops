using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;

namespace Braindrops.Testing
{
    public static class XPathNavigatorExtensions
    {
        public static XPathNavigator SelectFirst(this XPathNavigator xml, string xpath)
        {
            XPathNodeIterator node = xml.Select(xpath);
            node.MoveNext().ShouldBeTrue("{0} did not match any node on {1}", xpath, xml.OuterXml);
            return node.Current;
        }

        public static XPathNavigator SelectSingle(this XPathNavigator xml, string xpath)
        {
            XPathNodeIterator node = xml.Select(xpath);
            node.MoveNext().ShouldBeTrue("{0} did not match any node on {1}", xpath, xml.OuterXml);
            node.Count.ShouldEqual(1, "{0} matches more than one node on {1}", xpath, xml.OuterXml);
            return node.Current;
        }

        public static string SelectText(this XPathNavigator xml, string xpath)
        {
            return xml.SelectTextAs<string>(xpath);
        }

        public static IEnumerable<string> SelectValues(this XPathNavigator xml, string xpath)
        {
            return xml.SelectValuesAs<string>(xpath);
        }

        public static IEnumerable<T> SelectValuesAs<T>(this XPathNavigator xml, string xpath)
        {
            return (from item in xml.Select(xpath).Cast<XPathNavigator>() select item.TextAs<T>()).ToArray();
        }

        public static T SelectTextAs<T>(this XPathNavigator xml, string xpath)
        {
            return xml.SelectSingle(xpath).TextAs<T>();
        }

        public static string SelectFirstText(this XPathNavigator xml, string xpath)
        {
            return xml.SelectFirstTextAs<string>(xpath);
        }

        public static T SelectFirstTextAs<T>(this XPathNavigator xml, string xpath)
        {
            return xml.SelectFirst(xpath).TextAs<T>();
        }

        public static T TextAs<T>(this XPathNavigator xml)
        {
            if (xml.NodeType == XPathNodeType.Element)
            {
                xml.SelectTextAs<T>("text()");
            }

            return (T) xml.ValueAs(typeof (T));
        }

        public static string Text(this XPathNavigator xml)
        {
            return xml.TextAs<string>();
        }
    }
}