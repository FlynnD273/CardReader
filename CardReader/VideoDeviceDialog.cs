using DirectShowLib;
using Microsoft.Kinect;
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

        public string[] Devices { get; private set; }
        public int Cameras { get; private set; }
        public VideoDeviceDialog()
        {
            InitializeComponent();
            string[] temp = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).Select(o => o.Name).ToArray();
            Cameras = temp.Length;
            Devices = new string[temp.Length + KinectSensor.KinectSensors.Count];
            for (int i = 0; i < temp.Length; i++)
            {
                Devices[i] = temp[i];
            }
            for (int i = 0; i < KinectSensor.KinectSensors.Count; i++)
            {
                Devices[i + temp.Length] = "Kinect Device: " + KinectSensor.KinectSensors[i].DeviceConnectionId;
            }

            DeviceList.DataSource = Devices;
            DeviceList.SelectedIndex = DeviceList.Items.Count - 1;
            DeviceIndex = DeviceList.SelectedIndex;
        }

        private void DeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeviceIndex = DeviceList.SelectedIndex;
        }
    }
}
