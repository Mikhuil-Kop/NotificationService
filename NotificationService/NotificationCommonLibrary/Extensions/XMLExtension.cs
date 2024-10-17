using System.Xml.Linq;

namespace NotificationCommonLibrary.Extensions
{
    public static class XMLExtension
    {
        public static XAttribute? InnerAttribute(this XElement parent, string name)
        {
            var nameSplit = name.Split('.')!;
            var element = parent;

            for (int i = 0; i < nameSplit.Length - 1; i++)
            {
                element = element?.Element(nameSplit[i]);
            }

            return element?.Attribute(nameSplit[^1]);
        }

        public static XElement? InnerElement(this XElement parent, string name)
        {
            var nameSplit = name.Split('.');
            var element = parent;

            for (int i = 0; i < nameSplit.Length; i++)
            {
                element = element?.Element(nameSplit[i]);
            }

            return element;
        }

        public static IEnumerable<XElement>? InnerElements(this XElement parent, string name)
        {
            var nameSplit = name.Split('.')!;
            var element = parent;

            for (int i = 0; i < nameSplit.Length - 1; i++)
            {
                element = element?.Element(nameSplit[i]);
            }

            return element?.Descendants(nameSplit[^1]);
        }
    }
}
