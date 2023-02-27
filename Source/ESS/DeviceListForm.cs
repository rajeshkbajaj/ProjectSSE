using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Covidien.CGRS.ESS
{
    public partial class DeviceListForm : Form
    {
        private List<DeviceTypeStatusBox> ModelStatusList;
        private List<DeviceStatusBox> DeviceStatusList;

        public DeviceListForm()
        {
            InitializeComponent();
            InitializeDeviceList();

            this.Text = Properties.Resources.DEVICE_WORK_LIST_TITLE;
            AddDeviceButton.Text = Properties.Resources.ADD_DEVICE_BUTTON;
            ViewDocumentButton.Text = Properties.Resources.VIEW_BUTTON;
            CloseButton.Text = Properties.Resources.CLOSE_BUTTON;
            DeviceTypesGroupBox.Text = Properties.Resources.DEVICE_TYPES_LABEL;
            IndividualDevicesGroupBox.Text = Properties.Resources.INDIVIDUAL_DEVICES_LABEL;

            ViewDocumentButton.Enabled = false; // only enable when documents are cached and ready to view
        }

        private void InitializeDeviceList()
        {
            Log.Information($"DeviceListForm:InitializeDeviceList Entry ");
            ModelStatusList = new List<DeviceTypeStatusBox>();
            DeviceStatusList = new List<DeviceStatusBox>();

            // call bridge to get list of already cached devices
            List<KeyValuePair<string, string>> devices = DeviceManagement.Instance.GetKnownDevices();
            devices.ForEach(delegate(KeyValuePair<string, string> device)
            {
                AddDevice(device.Key, device.Value);
            });
            Log.Information($"DeviceListForm:InitializeDeviceList Exit ");
        }

        private void AddDeviceButton_Click(object sender, EventArgs e)
        {
            Log.Information($"DeviceListForm:mAddDeviceButton_Click Entry ");
            if (AuthenticationService.Instance().GetServerAccessCredentials() == AuthenticationService.CREDENTIAL_STATUS.APPROVED)
            {
                AddDeviceForm dialog = new AddDeviceForm(AddDevice);
                dialog.ShowDialog();
            }
            else
                MessageBox.Show(Properties.Resources.FUNCTION_RESTRICTED_MSG, Properties.Resources.ESS_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            Log.Information($"DeviceListForm:mAddDeviceButton_Click Exit ");
        }

        private void ViewDocumentButton_Click(object sender, EventArgs e)
        {
            Log.Information($"DeviceListForm:mViewDocumentButton_Click Entry ");
            ViewDocumentDialog dialog = new ViewDocumentDialog();
            dialog.ShowDialog();
            Log.Information($"DeviceListForm:mViewDocumentButton_Click Exit ");
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            // check status of all devices (check RSA status for busy)
            // if one is still retrieving data, display confirmation pop-up
            Log.Information($"DeviceListForm:mCloseButton_Click Entry ");
            bool agentBusy = false;

            DeviceStatusList.ForEach(delegate(DeviceStatusBox device)
            {
                if (device.RequestPending())
                {
                    agentBusy = true;
                }
                
            });

            if (agentBusy)
            {
                DialogResult result = MessageBox.Show(Properties.Resources.RSA_ALL_REQUESTS_NOT_COMPLETED_WARNING,
                                                      Properties.Resources.RSA_WAITING_ON_SERVER, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            else
                this.Close();
            Log.Information($"DeviceListForm:mCloseButton_Click Exit  agentBusy:{agentBusy} ");
        }

        // callback to be invoked by the add device pop-up
        private void AddDevice(string modelNumber, string serialNumber)
        {
            bool needNewModelNumber = true;
            int yVal = 0;
            Log.Information($"DeviceListForm:AddDevice Entry  modelNumber:{modelNumber} serialNumber:{serialNumber}");
            if (ModelStatusList.Count > 0)
            {
                ModelStatusList.ForEach(delegate(DeviceTypeStatusBox model)
                {
                    if (model.ModelNumber() == modelNumber)
                    {
                        needNewModelNumber = false;
                    }
                    // update yVal to be yCoordinate of last model status box
                    yVal = model.Location.Y + model.Size.Height;
                });
            }

            if (needNewModelNumber)
            {
                // create new status box
                DeviceTypeStatusBox box = new DeviceTypeStatusBox(modelNumber)
                {
                    Location = new Point(0, yVal)
                };
                ModelStatusList.Add(box);
                DeviceTypePanel.Controls.Add(box);
            }

            // make sure serial number is not being entered twice
            bool addDevice = true;
            yVal = 0;
            DeviceStatusList.ForEach(delegate(DeviceStatusBox device)
            {
                if (device.SerialNumber() == serialNumber)
                {
                    addDevice = false;
                }
                // update yVal to be yCoordinate of last device status box
                yVal = device.Location.Y + device.Size.Height;
            });

            if (addDevice)
            {
                // create new status box
                DeviceStatusBox box = new DeviceStatusBox(modelNumber, serialNumber)
                {
                    Location = new Point(0, yVal),
                    BackColor = (yVal % 2 == 1) ? Color.AliceBlue : Color.LightSteelBlue
                };
                box.CheckDeviceStatus();
                DeviceStatusList.Add(box);
                DevicePanel.Controls.Add(box);
                StartTimer();
            }
            Log.Information($"DeviceListForm:AddDevice Exit  modelNumber:{modelNumber} serialNumber:{serialNumber}");
        }

        void CheckDeviceStatus(DeviceStatusBox device)
        {
            Log.Information($"DeviceListForm:CheckDeviceStatus Entry device :{device} ");
            bool? status = device.CheckDeviceStatus();
            if (status == true)
            {
                ModelStatusList.ForEach(delegate(DeviceTypeStatusBox model)
                {
                    if (model.ModelNumber() == device.Model )
                    {
                        // need to check actual loaded packaged to know if this is true
                        if (SoftwarePackageManagement.Instance.SoftwarePackageCachedForDevice(device.Model, device.SerialNumber()))
                        {
                            model.SoftwarePackageIsLoaded();
                        }
                        if (SoftwarePackageManagement.Instance.DocumentsCachedForModel(device.Model, device.SerialNumber()))
                        {
                            model.DocumentsAreLoaded();
                            ViewDocumentButton.Enabled = true;
                        }
                    }
                });
            }
            Log.Information($"DeviceListForm:CheckDeviceStatus Exit device :{device} ");
        }

        private void StartTimer()
        {
            Timer1.Enabled = true;
            Timer1.Interval = 2000;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            bool timerEnabled = false;

            DeviceStatusList.ForEach(delegate(DeviceStatusBox device)
            {
                CheckDeviceStatus(device);
                if (device.RequestPending())
                {
                    timerEnabled = true;
                }
            });

            Timer1.Enabled = timerEnabled;
        }
    }
}
