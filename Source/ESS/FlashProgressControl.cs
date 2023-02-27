using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Serilog;

namespace Covidien.CGRS.ESS
{
    public partial class FlashProgressControl : UserControl
    {
        public FlashProgressControl( string cpu)
        {
            InitializeComponent();
            ComponentName.Text = cpu;
        }

        public string Component()
        {
            return ComponentName.Text;
        }

        public void Reset()
        {
            Log.Information($"FlashProgressControl:Reset");
            ProgressBar1.Value = 0;
            ActivityLabel1.Text = "";
            ActivityLabel2.Text = "";
        }

        public void UpdateDone()
        {
            Log.Information($"FlashProgressControl:UpdateDone");
            ProgressBar1.Value = 100;
            ActivityLabel1.Text = Properties.Resources.UPDATE_DONE;
            ActivityLabel2.Text = Properties.Resources.UPDATE_COMPLETE;
        }

        public void Update(string controlComponent, string msg)
        {
            Log.Information($"FlashProgressControl:Update Entry controlComponent:{controlComponent} msg:{msg}");
            try
            {
                if(controlComponent.Equals("PROGRESS"))
                {
                    int percent = int.Parse(msg);
                    ProgressBar1.Value = percent;
                }
                else if (controlComponent.Equals("LABEL_1"))
                {
                     ActivityLabel1.Text = msg;
                }
                else if (controlComponent.Equals("LABEL_2"))
                {
                    ActivityLabel2.Text = FilterLabel2Message(msg);
                }
            }
            catch (Exception e)
            {
                Log.Error($"FlashProgressControl:Update Exception :{e.Message}");
            }
            Log.Information($"FlashProgressControl:Update Exit controlComponent:{controlComponent} msg:{msg}");
        }

        private string FilterLabel2Message(string msg)
        {
            //objective is to a) replace file path with file name b) replace "Viking" to "Sw"
            // 
            //from: Erasing Flash for rfile://192.168.0.10/bin/VikingGui.bin.gz at 0xfd100000
            //to: Erasing Flash for SwGui.bin.gz

            //from: GUI,SET,LABEL_2,Calculating digest for 'rfile://192.168.0.10/bin/BdLoad.bin.gz'
            //to: GUI,SET,LABEL_2,Calculating digest for 'BdLoad.bin.gz'

            //from: Calculating digest for 'rfile://192.168.0.10/config/download.xml'
            //to: Calculating digest for 'download.xml'

            string modifiedMessage = msg;
            Log.Information($"FlashProgressControl:FilterLabel2Message Entry modifiedMessage:{modifiedMessage}");

            try
            {
                //remove address - if present
                int addressFindIndex = -1;
                if ( ((addressFindIndex = modifiedMessage.IndexOf(" at 0x", StringComparison.OrdinalIgnoreCase)) != -1) ||
                     ((addressFindIndex = modifiedMessage.IndexOf(" to 0x", StringComparison.OrdinalIgnoreCase)) != -1) )
                {
                    modifiedMessage = modifiedMessage.Substring(0, addressFindIndex);
                }

                // a) extract filename, replace "Viking" with "Sw" b) only show file name

                string rFilePattern = @"([a-z0-9A-Z]+)://([a-z0-9\-\.]+)/([a-z0-9\-\.]+)/([a-z0-9\-\.]+)";
                Match match = Regex.Match(modifiedMessage, rFilePattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    const int FILE_NAME_INDEX_IN_GROUP = 4;
                    string fileName = match.Groups[FILE_NAME_INDEX_IN_GROUP].Value;
                    //replace "Viking" with "Sw"
                    fileName = Regex.Replace(fileName, "Viking", "Sw", RegexOptions.IgnoreCase);
                    //only show file name
                    modifiedMessage = modifiedMessage.Substring(0, match.Groups[0].Index)
                              + fileName
                              + modifiedMessage.Substring(match.Groups[FILE_NAME_INDEX_IN_GROUP].Index + match.Groups[FILE_NAME_INDEX_IN_GROUP].Value.Length, (modifiedMessage.Length - (match.Groups[FILE_NAME_INDEX_IN_GROUP].Index + match.Groups[FILE_NAME_INDEX_IN_GROUP].Value.Length)));
                }

                return modifiedMessage;
            }
            catch (Exception e)
            {
                Log.Error($"FlashProgressControl:FilterLabel2Message Exception:{e.Message}");
            }
            Log.Information($"FlashProgressControl:FilterLabel2Message Exit modifiedMessage:{modifiedMessage}");
            //return unmodified
            return msg;

        }

        public void Error(string msg)
        {
            Log.Information($"FlashProgressControl:Error msg:{msg}");
            ActivityLabel1.Text = msg;
            ProgressBar1.Value = 100;
            ProgressBar1.ForeColor = System.Drawing.Color.Red;
        }
    }
}
