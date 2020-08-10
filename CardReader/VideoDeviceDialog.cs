using DirectShowLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardReader
{
    public partial class VideoDeviceDialog : Form
    {
        public int DeviceIndex { get; private set; }

        public DsDevice[] Devices { get; private set; }
        public VideoDeviceDialog()
        {
            InitializeComponent();
            Devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            DeviceList.DataSource = Devices;
            DeviceList.DisplayMember = "Name";
            DeviceList.SelectedIndex = DeviceList.Items.Count - 1;
            DeviceIndex = DeviceList.SelectedIndex;
        }

        private void DeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeviceIndex = DeviceList.SelectedIndex;
        }
    }
}
