using FusionExplorer.Models.Gear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace FusionExplorer.Services.Gear
{
    class GearService
    {
        private readonly RiderManager _riderManager;
        private readonly BikeManager _bikeManager;
        private readonly SetManager _setManager;
        private readonly ItemManager _itemManager;
        private readonly RiderDefaultManager _riderDefaultManager;
        private readonly BikeDefaultManager _bikeDefaultManager;
        private readonly LocalMPManager _localMPManager;

        public GearData gearData { get; private set; }

        public List<Rider> riders => gearData.Riders;
        public List<Bike> bikes => gearData.Bikes;
        public List<Set> sets => gearData.Sets;
        public List<Item> items => gearData.Items;

        public GearService()
        {
            _riderManager = new RiderManager();
            _bikeManager = new BikeManager();
            _setManager = new SetManager();
            _itemManager = new ItemManager();
            _riderDefaultManager = new RiderDefaultManager();
            _bikeDefaultManager = new BikeDefaultManager();
            _localMPManager = new LocalMPManager();

            gearData = new GearData();
        }

        public void Deserialize(XDocument gearDoc)
        {
            GearData gear = new GearData();

            gear.Riders = _riderManager.Deserialize(gearDoc.Root.Element("riders"));
            gear.Bikes = _bikeManager.Deserialize(gearDoc.Root.Element("bikes"));
            gear.Sets = _setManager.Deserialize(gearDoc.Root.Element("sets"));
            gear.Items = _itemManager.Deserialize(gearDoc.Root.Element("items"));
            gear.RiderDefaults = _riderDefaultManager.Deserialize(gearDoc.Root.Element("riderDefaults"));
            gear.BikeDefaults = _bikeDefaultManager.Deserialize(gearDoc.Root.Element("bikeDefaults"));
            gear.LocalMP = _localMPManager.Deserialize(gearDoc.Root.Element("localMP"));

            gearData = gear;
        }

        public XDocument Serialize()
        {
            XElement gearElement = new XElement("gear");

            gearElement.Add(_riderManager.Serialize(gearData.Riders));
            gearElement.Add(_bikeManager.Serialize(gearData.Bikes));
            gearElement.Add(_setManager.Serialize(gearData.Sets));
            gearElement.Add(_itemManager.Serialize(gearData.Items));
            gearElement.Add(_riderDefaultManager.Serialize(gearData.RiderDefaults));
            gearElement.Add(_bikeDefaultManager.Serialize(gearData.BikeDefaults));
            gearElement.Add(new XElement("regionalVariations"));
            gearElement.Add(_localMPManager.Serialize(gearData.LocalMP));

            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"), gearElement);
        }

        public void Serialize(string path)
        {
            XDocument gearDoc = Serialize();

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = false,
                OmitXmlDeclaration = true,
                Encoding = new UTF8Encoding(false)
            };

            using (var writer = XmlWriter.Create(path, settings))
            {
                gearDoc.Save(writer);
            }
        }

        public void AddItem(Item item)
        {

        }

        public void RemoveItem(Item item)
        {

        }

        public void AddSet(Set set)
        {

        }

        public void RemoveSet(Set set)
        {

        }
    }
}
