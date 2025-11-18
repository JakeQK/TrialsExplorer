using FusionExplorer.Services;
using FusionExplorer.Services.Gear;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FusionExplorer.Forms
{
    public partial class ItemsEditor: Form
    {
        public ItemsEditor()
        {
            InitializeComponent();
        }

        private GearService _gearService;
        private string _path;
        private bool _pakMode = false;

        private static int _itemsEntryId = -1899826158;
        private ChangeCacheService _changeCache = new ChangeCacheService();

        private void InitializeUIBaseState()
        {
            foreach(var rider in _gearService.gearData.Riders)
            {
                lbRiders.Items.Add(rider.GenKey);
            }
        }

        private void btnOpenItemsXml_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xml files|*.xml|pak files|*.pak|all files (xml)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileExtension = Path.GetExtension(ofd.SafeFileName);

                switch(fileExtension)
                {
                    case ".xml":
                        _path = ofd.FileName;
                        _pakMode = false;
                        //_gearService = GearData.DeserializeFromFile(_path);
                        _gearService = new GearService();
                        _gearService.Deserialize(XDocument.Load(_path));
                        InitializeUIBaseState();
                        break;
                    case ".pak":
                        _path = ofd.FileName;
                        _pakMode = true;
                        byte[] itemsRaw = ArchiveService.ExtractFile(_path, _itemsEntryId);
                        string itemsXml = Encoding.UTF8.GetString(itemsRaw);
                        _gearService = new GearService();
                        _gearService.Deserialize(XDocument.Parse(itemsXml));
                        InitializeUIBaseState();
                        break;
                }

                
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void lbRiders_SelectedIndexChanged(object sender, EventArgs e)
        {
            var rider = _gearService.riders[lbRiders.SelectedIndex];

            tbRiderName.Text = rider.Name;
            tbRiderGenKey.Text = rider.GenKey;
            tbRiderId.Text = rider.Id;
            tbRiderIcon.Text = rider.Icon;
        }

        private void tbRiderName_TextChanged(object sender, EventArgs e)
        {
            var rider = _gearService.riders[lbRiders.SelectedIndex];
            if (tbRiderName.Text != rider.Name)
            {
                _changeCache.Create($"RIDERS RIDER {lbRiders.SelectedIndex} NAME", rider.Name, tbRiderName.Text);
                rider.Name = tbRiderName.Text;
            }
        }

        private void tbRiderGenKey_TextChanged(object sender, EventArgs e)
        {
            var rider = _gearService.riders[lbRiders.SelectedIndex];
            if (tbRiderGenKey.Text != rider.GenKey)
            {
                rider.GenKey = tbRiderGenKey.Text;
            }
        }

        private void tbRiderId_TextChanged(object sender, EventArgs e)
        {
            var rider = _gearService.riders[lbRiders.SelectedIndex];
            if (tbRiderId.Text != rider.Id)
            {
                rider.Id = tbRiderId.Text;
            }
        }

        private void tbRiderIcon_TextChanged(object sender, EventArgs e)
        {
            var rider = _gearService.riders[lbRiders.SelectedIndex];
            if (tbRiderIcon.Text != rider.Icon)
            {
                rider.Icon = tbRiderIcon.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var change in _changeCache._cachedChanges)
            {
                listBox1.Items.Add(change.Key);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = listBox1.SelectedItem?.ToString();
            if (key != null)
            {
                var change = _changeCache._cachedChanges[listBox1.SelectedItem?.ToString()];
                label5.Text = $"{change.OriginalContent} -> {change.NewContent}";

                
            }
        }

        private void btnSaveItemsXml_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_gearService.items.Count > 0)
            {
                listBox2.Items.AddRange(_gearService.items.Select(i => i.GenKey).ToArray());
            }
        }
    }
}
