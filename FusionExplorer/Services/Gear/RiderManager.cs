using FusionExplorer.Models.Gear;
using FusionExplorer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace FusionExplorer.Services.Gear
{
    class RiderManager
    {
        public List<Rider> Deserialize(XElement ridersElement)
        {
            if (ridersElement == null)
            {
                return null;
            }
            
            var riders = new List<Rider>();

            foreach (var riderElem in ridersElement.Elements("rider"))
            {
                var rider = new Rider
                {
                    Name = (string)riderElem.Attribute("name"),
                    GenKey = (string)riderElem.Attribute("genKey"),
                    Id = (string)riderElem.Attribute("id"),
                    Icon = (string)riderElem.Attribute("icon"),
                };

                var groupsElement = ridersElement.Element("groups");
                if (groupsElement != null)
                {
                    foreach (var groupElement in groupsElement.Elements("group"))
                    {
                        var group = new Group
                        {
                            Name = (string)groupElement.Attribute("name")
                        };

                        foreach (var setElement in groupElement.Elements("set"))
                        {
                            group.Sets.Add((string)setElement.Attribute("id"));
                        }

                        rider.Groups.Add(group);
                    }
                }

                riders.Add(rider);
            }

            return riders;
        }
    
        public XElement Serialize(List<Rider> riders)
        {
            var ridersElement = new XElement("riders");
            foreach (var rider in riders)
            {
                var riderElement = new XElement("rider");

                XmlUtilities.AddAttributeIfNotNull(riderElement, "name", rider.Name);
                XmlUtilities.AddAttributeIfNotNull(riderElement, "genKey", rider.GenKey);
                XmlUtilities.AddAttributeIfNotNull(riderElement, "id", rider.Id);
                XmlUtilities.AddAttributeIfNotNull(riderElement, "icon", rider.Icon);

                var groupsElement = new XElement("groups");
                foreach (var group in rider.Groups)
                {
                    var groupElement = new XElement("group");
                    XmlUtilities.AddAttributeIfNotNull(groupElement, "name", group.Name);

                    foreach (var setId in group.Sets)
                    {
                        var setElement = new XElement("set");
                        XmlUtilities.AddAttributeIfNotNull(setElement, "id", setId);
                        groupElement.Add(setElement);
                    }

                    groupsElement.Add(groupElement);
                }

                riderElement.Add(groupsElement);
                ridersElement.Add(riderElement);
            }

            return ridersElement;
        }
    }
}
