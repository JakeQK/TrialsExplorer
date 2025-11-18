using FusionExplorer.Models.Gear.Default;
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
    class BikeDefaultManager
    {
        public List<Bike> Deserialize(XElement bikeDefaultsElement)
        {
            if (bikeDefaultsElement == null)
            {
                return null;
            }

            List<Bike> bikeDefaults = new List<Bike>();

            foreach (XElement bikeDefaultElement in bikeDefaultsElement.Elements("bike"))
            {
                Bike bikeDefault = new Bike
                {
                    BikeId = (string)bikeDefaultElement.Attribute("bikeId")
                };

                XElement defaultSetsElement = bikeDefaultElement.Element("defaultSets");
                foreach (XElement defaultSetElement in defaultSetsElement.Elements("defaultSet"))
                {
                    DefaultSet defaultSet = new DefaultSet
                    {
                        Slot = (string)defaultSetElement.Attribute("slot"),
                        SetId = (string)defaultSetElement.Attribute("setId")
                    };

                    bikeDefault.DefaultSets.Add(defaultSet);
                }

                XElement defaultColorsElement = bikeDefaultElement.Element("defaultColors");
                foreach (XElement defaultColorElement in defaultColorsElement.Elements("defaultColor"))
                {
                    DefaultColor defaultColor = new DefaultColor
                    {
                        Slot = (string)defaultColorElement.Attribute("slot"),
                        Color = (string)defaultColorElement.Attribute("color")
                    };

                    bikeDefault.DefaultColors.Add(defaultColor);
                }

                bikeDefaults.Add(bikeDefault);
            }

            return bikeDefaults;
        }
    
        public XElement Serialize(List<Bike> bikeDefaults)
        {
            var bikeDefaultsElement = new XElement("bikeDefaults");

            foreach (var bike in bikeDefaults)
            {
                var defaultBikeElement = new XElement("bike");
                XmlUtilities.AddAttributeIfNotNull(defaultBikeElement, "bikeId", bike.BikeId);

                var defaultSetsElement = new XElement("defaultSets");
                foreach (var defaultSet in bike.DefaultSets)
                {
                    var defaultSetElement = new XElement("defaultSet");
                    XmlUtilities.AddAttributeIfNotNull(defaultSetElement, "slot", defaultSet.Slot);
                    XmlUtilities.AddAttributeIfNotNull(defaultSetElement, "setId", defaultSet.SetId);

                    defaultSetsElement.Add(defaultSetElement);
                }
                defaultBikeElement.Add(defaultSetsElement);

                var defaultColorsElement = new XElement("defaultColors");
                foreach (var defaultColor in bike.DefaultColors)
                {
                    var defaultColorElement = new XElement("defaultColor");
                    XmlUtilities.AddAttributeIfNotNull(defaultColorElement, "slot", defaultColor.Slot);
                    XmlUtilities.AddAttributeIfNotNull(defaultColorElement, "color", defaultColor.Color);

                    defaultColorsElement.Add(defaultColorElement);
                }
                defaultBikeElement.Add(defaultColorsElement);

                bikeDefaultsElement.Add(defaultBikeElement);
            }

            return bikeDefaultsElement;
        }
    }
}
