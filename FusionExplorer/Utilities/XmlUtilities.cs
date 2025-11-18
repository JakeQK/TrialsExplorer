using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FusionExplorer.Utils
{
    public static class XmlUtilities
    {
        public static void AddAttributeIfNotNull(XElement element, string name, string value)
        {
            if (value != null)
            {
                element.Add(new XAttribute(name, value));
            }
        }
    }
}
