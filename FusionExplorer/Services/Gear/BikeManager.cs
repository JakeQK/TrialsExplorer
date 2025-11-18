using FusionExplorer.Models.Gear;
using FusionExplorer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FusionExplorer.Services.Gear
{
    class BikeManager
    {
        public List<Bike> Deserialize(XElement bikesElement)
        {
            if (bikesElement == null)
            {
                return null;
            }

            var bikes = new List<Bike>();

            foreach (var bikeElement in bikesElement.Elements("bikes"))
            {
                var bike = new Bike
                {
                    Name = (string)bikeElement.Attribute("name"),
                    GenKey = (string)bikeElement.Attribute("genKey"),
                    Id = (string)bikeElement.Attribute("id"),
                    ObjectId = (string)bikeElement.Attribute("objectId"),
                };

                var groupsElement = bikesElement.Element("groups");
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

                        bike.Groups.Add(group);
                    }
                }

                bikes.Add(bike);
            }

            return bikes;
        }

        public XElement Serialize(List<Bike> bikes)
        {
            var ridersElement = new XElement("bikes");
            foreach (var bike in bikes)
            {
                var bikeElement = new XElement("bike");

                XmlUtilities.AddAttributeIfNotNull(bikeElement, "name", bike.Name);
                XmlUtilities.AddAttributeIfNotNull(bikeElement, "genKey", bike.GenKey);
                XmlUtilities.AddAttributeIfNotNull(bikeElement, "id", bike.Id);
                XmlUtilities.AddAttributeIfNotNull(bikeElement, "objectId", bike.ObjectId);

                var groupsElement = new XElement("groups");
                foreach (var group in bike.Groups)
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

                bikeElement.Add(groupsElement);
                ridersElement.Add(bikeElement);
            }

            return ridersElement;
        }
    }
}
