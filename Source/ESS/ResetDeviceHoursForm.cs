using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Covidien.CGRS.VTS
{
    public partial class ResetDeviceHoursForm : Form
    {
        public ResetDeviceHoursForm()
        {
            InitializeComponent();
            Setup(DeviceManagement.Instance.GetResetableComponentHoursList());
        }

        private void Setup( List<KeyValuePair<string,KeyValuePair<string,int>>> components )
        {
            int number = components.Count;
            Point pos = new Point(5, 5);
            mComponentList = new List<DeviceHoursSettingControl>();

            foreach (KeyValuePair<string, KeyValuePair<string, int>> component in components)
            {
                DeviceHoursSettingControl control = new DeviceHoursSettingControl(component, pos,
                                                                                  (mComponentList.Count % 2 == 1) ? Color.AliceBlue : Color.LightSteelBlue);
                mComponentList.Add(control);
                pos.Y += control.Size.Height;
            }

            this.Size = new Size(this.Size.Width, this.Size.Height + pos.Y + 25); // why? difference in size between fixed and sizeable
            int newY = mUpdateButton.Location.Y + pos.Y;
            mUpdateButton.Location = new Point(mUpdateButton.Location.X, newY);
            mCancelButton.Location = new Point(mCancelButton.Location.X, newY);

            foreach (DeviceHoursSettingControl control in mComponentList)
            {
                this.Controls.Add(control);
            }
        }

        private void mUpdateButton_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, KeyValuePair<string, int>>> resetList = new List<KeyValuePair<string, KeyValuePair<string, int>>>();

            foreach (DeviceHoursSettingControl control in mComponentList)
            {
                KeyValuePair<string, KeyValuePair<string, int>> component = control.Component();
                resetList.Add(component);
            }

            if (resetList.Count > 0)
            {
                DeviceManagement.Instance.UpdateDeviceHours(resetList);
            }

            this.Close();
        }

        List<DeviceHoursSettingControl> mComponentList;

    }
}
