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
    class ItemManager
    {
        // Create
        public void AddItem(List<Item> items, Item item)
        {
            // Duplicate check on ID
            if (items.FirstOrDefault(i => i.Id == item.Id) == null)
            {
                items.Add(item);
            }
        }

        // Read
        public Item GetItemById(List<Item> items, string id)
        {
            return items.FirstOrDefault(i => i.Id == id);
        }

        public Item GetItemByGenKey(List<Item> items, string genKey)
        {
            return items.FirstOrDefault(i => i.GenKey == genKey);
        }

        public List<Item> GetItemsByBindId(List<Item> items, string bindId)
        {
            return items.FindAll(i => i.BindId == bindId);
        }

        public List<Item> GetItemsByObjectId(List<Item> items, string objectId)
        {
            return items.FindAll(i => i.ObjectId == objectId);
        }

        public List<Item> GetAllItems(List<Item> items)
        {
            return items;
        }

        // Update
        public bool ChangeItemId(List<Item> items, string oldId, string newId)
        {
            Item item = items.FirstOrDefault(i => i.Id == oldId);

            if (item == null)
            {
                // old id doesn't exist
                return false;
            }

            if (items.FirstOrDefault(i => i.Id == newId) != null)
            {
                // new id already exists
                return false;
            }

            item.Id = newId;
            return true;
        }

        // Delete
        public bool RemoveItemById(List<Item> items, string id)
        {
            Item item = items.FirstOrDefault(i => i.Id == id);

            if (item == null)
            {
                return false;
            }

            items.Remove(item);

            return true;
        }

        // Base
        public List<Item> Deserialize(XElement itemsElement)
        {
            if (itemsElement == null)
            {
                return null;
            }

            var items = new List<Item>();

            foreach (var itemElement in itemsElement.Elements("item"))
            {
                var item = new Item
                {
                    Id = (string)itemElement.Attribute("id"),
                    GenKey = (string)itemElement.Attribute("genKey"),
                    ColorPartId = (string)itemElement.Attribute("colorPartId"),
                    BindId = (string)itemElement.Attribute("bindId"),
                    ObjectId = (string)itemElement.Attribute("objectId"),
                    Shared = (string)itemElement.Attribute("shared"),
                    VariationId = (string)itemElement.Attribute("variationId")
                };

                items.Add(item);
            }

            return items;
        }

        public XElement Serialize(List<Item> items)
        {
            var itemsElement = new XElement("items");
            foreach (var item in items)
            {
                var itemElement = new XElement("item");
                XmlUtilities.AddAttributeIfNotNull(itemElement, "id", item.Id);
                XmlUtilities.AddAttributeIfNotNull(itemElement, "genKey", item.GenKey);
                XmlUtilities.AddAttributeIfNotNull(itemElement, "colorPartId", item.ColorPartId);
                XmlUtilities.AddAttributeIfNotNull(itemElement, "bindId", item.BindId);
                XmlUtilities.AddAttributeIfNotNull(itemElement, "objectId", item.ObjectId);
                XmlUtilities.AddAttributeIfNotNull(itemElement, "shared", item.Shared);
                XmlUtilities.AddAttributeIfNotNull(itemElement, "variationId", item.VariationId);

                itemsElement.Add(itemElement);
            }

            return itemsElement;
        }
    }
}
