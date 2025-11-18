using FusionExplorer.Models.Gear.LocalMP;
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
    class LocalMPManager
    {
        public LocalMP Deserialize(XElement localMPElement)
        {
            if (localMPElement == null)
            {
                return null;
            }
            LocalMP localMP = new LocalMP();

            foreach (var riderElement in localMPElement.Elements("rider"))
            {
                var rider = new Models.Gear.LocalMP.Rider.Rider();

                foreach (var setElement in riderElement.Elements("set"))
                {
                    var set = new Models.Gear.LocalMP.Rider.Set
                    {
                        SetId = (string)setElement.Attribute("setId"),
                        Color = (string)setElement.Attribute("color")
                    };


                    rider.Sets.Add(set);
                }

                localMP.Riders.Add(rider);
            }

            foreach (var bikeElement in localMPElement.Elements("bike"))
            {
                var bike = new Models.Gear.LocalMP.Bike.Bike();

                foreach (var setElement in bikeElement.Elements("set"))
                {
                    var set = new Models.Gear.LocalMP.Bike.Set
                    {
                        Slot = (string)setElement.Attribute("slot"),
                        Color = (string)setElement.Attribute("color")
                    };

                    bike.Sets.Add(set);
                }

                localMP.Bikes.Add(bike);
            }

            return localMP;
        }

        public XElement Serialize(LocalMP localMP)
        {
            var localMPElement = new XElement("localMP");

            foreach (var rider in localMP.Riders)
            {
                var riderElement = new XElement("rider");

                foreach (var set in rider.Sets)
                {
                    var setElement = new XElement("set");

                    XmlUtilities.AddAttributeIfNotNull(setElement, "setId", set.SetId);
                    XmlUtilities.AddAttributeIfNotNull(setElement, "color", set.Color);

                    riderElement.Add(setElement);
                }

                localMPElement.Add(riderElement);
            }

            foreach (var bike in localMP.Bikes)
            {
                var bikeElement = new XElement("bike");

                foreach (var set in bike.Sets)
                {
                    var setElement = new XElement("set");

                    XmlUtilities.AddAttributeIfNotNull(setElement, "slot", set.Slot);
                    XmlUtilities.AddAttributeIfNotNull(setElement, "color", set.Color);

                    bikeElement.Add(setElement);
                }

                localMPElement.Add(bikeElement);
            }

            return localMPElement;
        }
    }
}
