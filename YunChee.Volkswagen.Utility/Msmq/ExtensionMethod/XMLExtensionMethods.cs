using System.Xml;

namespace Yunchee.Volkswagen.Utility.ExtensionMethod
{
    public static class XMLExtensionMethods
    {
        public static bool IsExists(this XmlNodeList list, string key)
        {
            bool i = false;
            foreach (XmlNode item in list)
            {
                if (item.Name == key)
                    return true;
            }
            return i;
        }

        public static XmlNode GetNode(this XmlNodeList list, string key)
        {
            foreach (XmlNode item in list)
            {
                if (item.Name == key)
                    return item;
            }
            return null;
        }
    }

}
