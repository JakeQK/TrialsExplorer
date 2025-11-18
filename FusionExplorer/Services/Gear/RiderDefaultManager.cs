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
    class RiderDefaultManager
    {
        public RiderDefaults Deserialize(XElement riderDefaultsElement)
        {
            if (riderDefaultsElement == null)
            {
                return null;
            }

            RiderDefaults riderDefaults = new RiderDefaults();

            var defaultSetsElement = riderDefaultsElement.Element("defaultSets");
            foreach (var defaultSetElement in defaultSetsElement.Elements("defaultSet"))
            {
                var defaultSet = new DefaultSet
                {
                    Slot = (string)defaultSetElement.Attribute("slot"),
                    SetId = (string)defaultSetElement.Attribute("setId")
                };

                riderDefaults.DefaultSets.Add(defaultSet);
            }

            var defaultColorsElement = riderDefaultsElement.Element("defaultColors");
            foreach (var defaultSetElement in defaultColorsElement.Elements("defaultColor"))
            {
                var defaultColor = new DefaultColor
                {
                    Slot = (string)defaultSetElement.Attribute("slot"),
                    Color = (string)defaultSetElement.Attribute("color")
                };

                riderDefaults.DefaultColors.Add(defaultColor);
            }

            return riderDefaults;
        }
    
        public XElement Serialize(RiderDefaults riderDefaults)
        {
            var riderDefaultsElement = new XElement("riderDefaults");

            // DefaultSets
            var defaultSetsElement = new XElement("defaultSets");
            foreach (var defaultSet in riderDefaults.DefaultSets)
            {
                var defaultSetElement = new XElement("defaultSet");
                XmlUtilities.AddAttributeIfNotNull(defaultSetElement, "slot", defaultSet.Slot);
                XmlUtilities.AddAttributeIfNotNull(defaultSetElement, "setId", defaultSet.SetId);
                defaultSetsElement.Add(defaultSetElement);
            }
            riderDefaultsElement.Add(defaultSetsElement);

            // DefaultColors
            var defaultColorsElement = new XElement("defaultColors");
            foreach (var defaultColor in riderDefaults.DefaultColors)
            {
                var defaultColorElement = new XElement("defaultColor");
                XmlUtilities.AddAttributeIfNotNull(defaultColorElement, "slot", defaultColor.Slot);
                XmlUtilities.AddAttributeIfNotNull(defaultColorElement, "color", defaultColor.Color);
                defaultColorsElement.Add(defaultColorElement);
            }
            riderDefaultsElement.Add(defaultColorsElement);

            return riderDefaultsElement;
        }
    }
}
