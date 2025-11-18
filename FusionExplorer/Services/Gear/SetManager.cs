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
    class SetManager
    {
        // Create
        public void AddSet(List<Set> sets, Set set)
        {
            // Duplicate check on ID
            if (sets.FirstOrDefault(i => i.Id == set.Id) == null)
            {
                sets.Add(set);
            }
        }

        // Read
        public Set GetSetById(List<Set> sets, string id)
        {
            return sets.FirstOrDefault(i => i.Id == id);
        }

        public Set GetSetByGenKey(List<Set> sets, string genKey)
        {
            return sets.FirstOrDefault(i => i.GenKey == genKey);
        }

        public List<Set> GetSetsByName(List<Set> sets, string name)
        {
            return sets.FindAll(i => i.Name == name);
        }

        public List<Set> GetSetsByType(List<Set> sets, string type)
        {
            return sets.FindAll(i => i.Type == type);
        }

        public List<Set> GetSetsByCameraTarget(List<Set> sets, string cameraTarget)
        {
            return sets.FindAll(i => i.CameraTarget == cameraTarget);
        }

        public List<Set> GetSetsByColorCategory(List<Set> sets, string colorCategory)
        {
            return sets.FindAll(i => i.ColorCategory == colorCategory);
        }

        public List<Set> GetAllSets(List<Set> sets)
        {
            return sets;
        }

        // Update
        public bool ChangeSetId(List<Set> sets, string oldId, string newId)
        {
            Set set = sets.FirstOrDefault(i => i.Id == oldId);

            if (set == null)
            {
                // old id doesn't exist
                return false;
            }

            if (sets.FirstOrDefault(i => i.Id == newId) != null)
            {
                // new id already exists
                return false;
            }

            set.Id = newId;
            return true;
        }

        public void AddItemToSet(Set set, Item item)
        {
            if (set.Items.FirstOrDefault(i => i.Id == item.Id) == null)
            {
                set.Items.Add(item);
            }
        }

        public void RemoveItemFromSet(Set set, Item item)
        {
            if (set.Items.FirstOrDefault(i => i.Id == item.Id) != null)
            {
                set.Items.Remove(item);
            }
        }

        public void RemoveItemFromSet(Set set, string itemId)
        {
            Item item = set.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                set.Items.Remove(item);
            }
        }

        // Delete
        public bool RemoveSetById(List<Set> sets, string id)
        {
            Set set = sets.FirstOrDefault(i => i.Id == id);

            if (set == null)
            {
                return false;
            }

            sets.Remove(set);

            return true;
        }


        public List<Set> Deserialize (XElement setsElement)
        {
            if (setsElement == null)
            {
                return null;
            }

            var sets = new List<Set>();

            foreach (var setElement in setsElement.Elements("set"))
            {
                var set = new Set
                {
                    Id = (string)setElement.Attribute("id"),
                    Name = (string)setElement.Attribute("name"),
                    GenKey = (string)setElement.Attribute("genKey"),
                    Icon = (string)setElement.Attribute("icon"),
                    DefaultColor = (string)setElement.Attribute("defaultColor"),
                    Type = (string)setElement.Attribute("type"),
                    Price = (string)setElement.Attribute("price"),
                    ExpLevelReq = (string)setElement.Attribute("expLevelReq"),
                    MedalReq = (string)setElement.Attribute("medalReq"),
                    AlwaysLocked = (string)setElement.Attribute("alwaysLocked"),
                    Hidden = (string)setElement.Attribute("Hidden"),
                    CameraTarget = (string)setElement.Attribute("cameraTarget"),
                    ColorSlotId = (string)setElement.Attribute("colorSlotId"),
                    ColorCategory = (string)setElement.Attribute("colorCategory")
                };

                foreach (var itemElement in setElement.Elements("item"))
                {
                    set.Items.Add(new Item
                    {
                        Id = (string)itemElement.Attribute("id"),
                        GenKey = (string)itemElement.Attribute("genKey")
                    });
                }

                sets.Add(set);
            }

            return sets;
        }

        public XElement Serialize(List<Set> sets)
        {
            var setsElement = new XElement("sets");
            foreach (var set in sets)
            {
                var setElement = new XElement("set");

                XmlUtilities.AddAttributeIfNotNull(setElement, "id", set.Id);
                XmlUtilities.AddAttributeIfNotNull(setElement, "name", set.Name);
                XmlUtilities.AddAttributeIfNotNull(setElement, "genKey", set.GenKey);
                XmlUtilities.AddAttributeIfNotNull(setElement, "icon", set.Icon);
                XmlUtilities.AddAttributeIfNotNull(setElement, "defaultColor", set.DefaultColor);
                XmlUtilities.AddAttributeIfNotNull(setElement, "type", set.Type);
                XmlUtilities.AddAttributeIfNotNull(setElement, "price", set.Price);
                XmlUtilities.AddAttributeIfNotNull(setElement, "expLevelReq", set.ExpLevelReq);
                XmlUtilities.AddAttributeIfNotNull(setElement, "medalReq", set.MedalReq);
                XmlUtilities.AddAttributeIfNotNull(setElement, "alwaysLocked", set.AlwaysLocked);
                XmlUtilities.AddAttributeIfNotNull(setElement, "Hidden", set.Hidden);
                XmlUtilities.AddAttributeIfNotNull(setElement, "cameraTarget", set.CameraTarget);
                XmlUtilities.AddAttributeIfNotNull(setElement, "colorSlotId", set.ColorSlotId);
                XmlUtilities.AddAttributeIfNotNull(setElement, "colorCategory", set.ColorCategory);

                foreach (var item in set.Items)
                {
                    XElement itemElement = new XElement("item");

                    XmlUtilities.AddAttributeIfNotNull(itemElement, "id", item.Id);
                    XmlUtilities.AddAttributeIfNotNull(itemElement, "genKey", item.GenKey);

                    setElement.Add(itemElement);
                }

                setsElement.Add(setElement);
            }

            return setsElement;
        }
    }
}
