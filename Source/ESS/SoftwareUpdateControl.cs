// ------------------------------------------------------------------------------
//                    Copyright (c) 2022 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------
namespace Covidien.CGRS.ESS
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Forms;
    using System.IO;
    using System.Collections;
    using Oasis.Agent.Models;
    using Utilties;
    using Serilog;

    public partial class SoftwareUpdateControl : UserControl
    {

        //map of language and list of software packages for that language
        private readonly Hashtable SoftwarePackagesMap = null;
        //map of language and name and hash of that software package
        private readonly Hashtable SoftwareNameAndHashMap = null;
        //map of software package name and list of documents for that language
        private readonly Hashtable DocumentPackagesMap = null;

        private readonly Hashtable DocumentProcessMap = new Hashtable();
        private readonly SoftwareUpload SoftwareUploadControl1;
        private bool IsDeviceRegistered = false;
        private bool AgentDownloadStatusComplete = false;
        private bool RetrieveUpdatePackagesPending = false;
        private readonly DeviceSerialNumberControl DeviceSerialNumberControl1;
        private readonly EssSettings Settings;
        private bool IsDummyDeviceConnected;
        private bool IsLocalPackageSelected;

        public SoftwareUpdateControl(EssSettings essSettings)
        {
            Settings = essSettings;
            InitializeComponent();

            DocumentProcessMap.Clear();

            SelectPackageLabel.Text = Properties.Resources.SW_UPDATE_SELECT_PACKAGE;
            SelectDocumentPackageLabel.Text = Properties.Resources.SW_UPLOAD_SELECT_DOCUMENT_LABEL;
            ViewDocumentButton.Text = Properties.Resources.SW_UPDATE_VIEW_DOCUMENT_BUTTON;
            FlashDeviceButton.Text = Properties.Resources.SW_UPDATE_FLASH_DEVICE_BUTTON;
            DeviceSerialNumberControl1 = new DeviceSerialNumberControl();
            SoftwareUploadControl1 = new SoftwareUpload("", "");
            SoftwareUploadControl1.SoftwareUploadCancelClicked += SoftwareUpdateControlCancelClicked;

            DeviceSerialNumberControl1.DeviceSerialNumberSubmitDone += this.DeviceSerialNumberSubmitted;

            this.Controls.Add(DeviceSerialNumberControl1);
            this.Controls.Add(SoftwareUploadControl1);

            SoftwarePackagesMap = new Hashtable();
            DocumentPackagesMap = new Hashtable();
            SoftwareNameAndHashMap = new Hashtable();

            SoftwareDownLoadProgressBar.Value = 0;
            SoftwareDownLoadProgressBar.Style = ProgressBarStyle.Marquee;
            SoftwareDownLoadProgressBar.Visible = false;

            SoftwareDownloadStatusLabel.Text = "";
            SoftwareDownloadStatusLabel.Visible = true;

            SoftwareUploadControl1.Visible = false;
            IsDeviceRegistered = false;

            SoftwarePackageManagement.Instance.RegisterForNotificationsEvents(NotificationsReceived, 
                                                                                NotificationProcessed, 
                                                                                AllNotificationsProcessed);
        }

        public void EnableControls()
        {
            Log.Information($"SoftwareUpdateControl:EnableControls Entry");
            SelectPackageLabel.Enabled = true;
            PackageSelectionComboBox.Enabled = true;
            FlashDeviceButton.Enabled = true;

            //Document is not supported now as OASIS is not providing these details.
            SelectDocumentPackageLabel.Enabled = false;
            ViewDocumentButton.Enabled = false;
            InitializePackageList();
            Log.Information($"SoftwareUpdateControl:EnableControls Exit");
        }

        public void DisableControls()
        {
            Log.Information($"SoftwareUpdateControl:DisableControls Entry");
            PackageSelectionComboBox.Items.Clear();
            PackageSelectionComboBox.Enabled = false;
            FlashDeviceButton.Enabled = false;

            ViewDocumentButton.Enabled = false;

            DeviceSerialNumberControl1.Initialize();

            SoftwareDownloadStatusLabel.Text = "";
            SoftwareDownLoadProgressBar.Visible = false;
            Log.Information($"SoftwareUpdateControl:DisableControls Exit");
        }

        public void DummyDeviceControls()
        {
            IsDummyDeviceConnected = true;
            IsDeviceRegistered = true;
            FlashDeviceButton.Enabled = false;
            RetrieveUpdatePackagesPending = true;
            SoftwareDownloadStatusLabel.Text = Properties.Resources.SW_UPDATE_REQUESTING_PACKAGE_LIST_STATUS_MSG;

            Log.Information($"SoftwareUpdateControl:DeviceConnected Entry");
            SetControlsToDeviceConnectedState();
            Log.Information($"SoftwareUpdateControl:DeviceConnected Exit");
        }

        public void DeviceConnected()
        {
            IsDummyDeviceConnected = false;
            FlashDeviceButton.Visible = true;

            Log.Information($"SoftwareUpdateControl:DeviceConnected Entry");
            SetControlsToDeviceConnectedState();
            Log.Information($"SoftwareUpdateControl:DeviceConnected Exit");
        }

        private void SetControlsToDeviceConnectedState()
        {
            Log.Information($"SoftwareUpdateControl:SetControlsToDeviceConnectedState Entry");
            this.SuspendLayout();
            ShowControls(true);
            DeviceSerialNumberControl1.Visible = false;
            SoftwareUploadControl1.Visible = false;

            this.ResumeLayout();
            Log.Information($"SoftwareUpdateControl:SetControlsToDeviceConnectedState Exit");
        }

        public void DeviceConnectedInDownloadMode()
        {
            Log.Information($"SoftwareUpdateControl:DeviceConnectedInDownloadMode Entry");
            SetControlsToDeviceConnectedState();
            EnableControls();
            Log.Information($"SoftwareUpdateControl:DeviceConnectedInDownloadMode Exit");
        }

        private void DeviceSerialNumberSubmitted(object sender, EventArgs e)
        {
            //show all controls
            Log.Information($"SoftwareUpdateControl:DeviceSerialNumberSubmitted Entry");
            SetControlsToDeviceConnectedState();
            EnableControls();

            //get software packages
            RetrieveUpdatePackages();
            //            SoftwarePackageManagement.Instance.RetrieveSoftwarePackagesFromServer();
            Log.Information($"SoftwareUpdateControl:DeviceSerialNumberSubmitted Exit");
        }

        private void ShowControls(bool visible)
        {
            Log.Information($"SoftwareUpdateControl:ShowControls");
            foreach (Control ctl in this.Controls)
            {
                ctl.Visible = visible;
            }
        }

        public void DeviceDisconnected()
        {
            Log.Information($"SoftwareUpdateControl:DeviceDisconnected Entry");
            //todo: implement stopping current reader
            //StopDocumentReaderProcess();
            IsDeviceRegistered = false;
            RetrieveUpdatePackagesPending = false;

            //clear all data
            SoftwarePackagesMap.Clear();
            DocumentPackagesMap.Clear();
            SoftwareNameAndHashMap.Clear();

            DeleteDocuments();
            
            DisableControls();
            Log.Information($"SoftwareUpdateControl:DeviceDisconnected Exit");
        }

        public void ApplicationClose()
        {
            Log.Information($"SoftwareUpdateControl:ApplicationClose Entry");
            DeleteDocuments();
            Log.Information($"SoftwareUpdateControl:ApplicationClose Exit");
        }

        private void DeleteDocuments()
        {
            Log.Information($"SoftwareUpdateControl:DeleteDocuments Entry");
            try
            {
                foreach (DictionaryEntry de in DocumentProcessMap)
                {
                    //close processes
                    Process process = (Process)de.Key;
                    process.Close();

                    string documentPath = (string)de.Value;
                    if (File.Exists(documentPath))
                    {
                        File.Delete(documentPath);
                    }
                }

                DocumentProcessMap.Clear();
            }
            catch (Exception ex)
            {
                Log.Error($"SoftwareUpdateControl:DeleteDocuments Exception:{ex.Message}");
            }
            Log.Information($"SoftwareUpdateControl:DeleteDocuments Exit");
        }

        private void AddBrowseLocalFileItem()
        {
            Log.Information($"SoftwareUpdateControl:AddBrowseLocalFileItem Entry");
            if (AuthenticationService.Instance().GetBrowseToSelectSoftwareCredentials() == AuthenticationService.CREDENTIAL_STATUS.APPROVED &&
                IsDummyDeviceConnected == false)
            {
                if (PackageSelectionComboBox.Items.Contains(Properties.Resources.LOAD_LOCAL_FILE) == false)
                {
                    PackageSelectionComboBox.Items.Add(Properties.Resources.LOAD_LOCAL_FILE); // add to resource string list
                    FlashDeviceButton.Enabled = true;
                }
            }
            Log.Information($"SoftwareUpdateControl:AddBrowseLocalFileItem Exit");
        }

        private void RefreshUpdateControls()
        {
            Log.Information($"SoftwareUpdateControl:RefreshUpdateControls Entry");
            if (LanguageSelectionComboBox.Items.Count > 0 &&
                IsDummyDeviceConnected == false)
            {
                if (LanguageSelectionComboBox.SelectedIndex < 0)
                {
                    //if none is selected, set the selected index to first one
                    LanguageSelectionComboBox.SelectedIndex = 0;
                }
                FlashDeviceButton.Enabled = true;
            }
            else if (PackageSelectionComboBox.Items.Count > 0 &&
                IsDummyDeviceConnected == false)
            {
                if (PackageSelectionComboBox.SelectedIndex < 0)
                {
                    //if none is selected, set the selected index to first one
                    PackageSelectionComboBox.SelectedIndex = 0;
                }
                FlashDeviceButton.Enabled = true;
            }
            else
            {
                FlashDeviceButton.Enabled = false;
            }
            UpdateFlashButtonText();
            Log.Information($"SoftwareUpdateControl:RefreshUpdateControls Exit");
        }

        private void UpdateFlashButtonText()
        {
            Log.Information($"SoftwareUpdateControl:UpdateFlashButtonText Entry");
            //if selected index is "Load Local File"-change flash button text to browse
            if (string.Equals((string)PackageSelectionComboBox.SelectedItem, Properties.Resources.LOAD_LOCAL_FILE, StringComparison.OrdinalIgnoreCase))
            {
                //change button text to Browse
                FlashDeviceButton.Text = Properties.Resources.BROWSE_TO_UPDATE_DEVICE;
            }
            else if (IsDummyDeviceConnected)
            {
                FlashDeviceButton.Text = "Disabled";
            }
            else
            {
                FlashDeviceButton.Text = Properties.Resources.SW_UPDATE_FLASH_DEVICE_BUTTON;
            }
            Log.Information($"SoftwareUpdateControl:UpdateFlashButtonText Exit");
        }


        public void UserLoggedIn(bool isOnlineLogin)
        {
            Log.Information($"SoftwareUpdateControl:UserLoggedIn Entry");
            AddBrowseLocalFileItem();
            Log.Information($"SoftwareUpdateControl:UserLoggedIn Exit");
        }

        public void DeviceRegistered(bool registrationStatus)
        {
            Log.Information($"SoftwareUpdateControl:DeviceRegistered Entry registrationStatus:{registrationStatus}");
            //can stiil ask for updates even when device registration fails
            IsDeviceRegistered = true;
            RetrieveUpdatePackagesPending = true;
            //update GUI based on user rights
            AddBrowseLocalFileItem();

            SoftwareDownloadStatusLabel.Text = Properties.Resources.SW_UPDATE_REQUESTING_PACKAGE_LIST_STATUS_MSG;
            RetrieveUpdatePackages();
            Log.Information($"SoftwareUpdateControl:DeviceRegistered Exit registrationStatus:{registrationStatus}");
        }

        public void AgentDownloadComplete()
        {
            Log.Information($"SoftwareUpdateControl:AgentDownloadComplete Entry");
            //only consider agent download complete after device is registered
            if (IsDeviceRegistered == false)
                return;

            AgentDownloadStatusComplete = true;
            RetrieveUpdatePackages();
            Log.Information($"SoftwareUpdateControl:AgentDownloadComplete Exit");
        }

        private void RetrieveUpdatePackages()
        {
            //getheaders - device registered + download time zero
            Log.Information($"SoftwareUpdateControl:RetrieveUpdatePackages Entry Reg:{IsDeviceRegistered} sts:{AgentDownloadStatusComplete} pending:{RetrieveUpdatePackagesPending}");
            if (IsDeviceRegistered && AgentDownloadStatusComplete && RetrieveUpdatePackagesPending )
            {
                RetrieveUpdatePackagesPending = false;

                if (SoftwarePackageManagement.Instance.RetrieveSoftwarePackagesFromServer() == false)
                {
                    RetrieveUpdatePackagesPending = true;
                }
            }
            Log.Information($"SoftwareUpdateControl:RetrieveUpdatePackages Exit mDeviceRegistered:{IsDeviceRegistered} mAgentDownloadStatusComplete:{AgentDownloadStatusComplete} mRetrieveUpdatePackagesPending:{RetrieveUpdatePackagesPending}");

        }

        private void NotificationsReceived(int nNotificationCount)
        {
            Log.Information($"SoftwareUpdateControl:NotificationsReceived Entry nNotificationCount:{nNotificationCount}");
            SoftwareDownLoadProgressBar.Visible = true;
            SoftwareDownloadStatusLabel.Text = Properties.Resources.SW_PACKAGE_DOWNLOAD_INPROGRESS;
            SoftwareDownLoadProgressBar.Value = 0;
            SoftwareDownLoadProgressBar.Maximum = nNotificationCount;
            SoftwareDownLoadProgressBar.Style = ProgressBarStyle.Marquee;
            Log.Information($"SoftwareUpdateControl:NotificationsReceived Exit nNotificationCount:{nNotificationCount}");
        }

        private void NotificationProcessed()
        {
            Log.Information($"SoftwareUpdateControl:NotificationProcessed");
            SoftwareDownLoadProgressBar.Style = ProgressBarStyle.Continuous;
            SoftwareDownLoadProgressBar.Value++;
        }

        private void AllNotificationsProcessed()
        {
            Log.Information($"SoftwareUpdateControl:AllNotificationsProcessed Entry");
            var pkgList = SoftwarePackageManagement.Instance.GetSoftwareUpdatePackages();            
            RetrieveUpdatePackagesPending = (pkgList == null || pkgList.Count ==0);
            
            //now Business layer and RSA have the complete software package information - can now access it            
            if (RetrieveUpdatePackagesPending)
            {
                SoftwareDownloadStatusLabel.Text = Properties.Resources.SW_PACKAGE_DOWNLOAD_FAILURE;
            }
            else
            { 
                PopulateSoftwarePackageMap();
                PopulateDocumentPackageMap();
                SoftwareDownloadStatusLabel.Text = Properties.Resources.SW_PACKAGE_DOWNLOAD_COMPLETE;
                InitializePackageList();
            }

            SoftwareDownLoadProgressBar.Visible = false;
            Log.Information($"SoftwareUpdateControl:AllNotificationsProcessed Exit");
        }

        private void PopulateSoftwarePackageMap()
        {
            Log.Information($"SoftwareUpdateControl:PopulateSoftwarePackageMap Entry");
            SoftwarePackagesMap.Clear();
            SoftwareNameAndHashMap.Clear();
            List<Software> listOfSoftwareUpdates = SoftwarePackageManagement.Instance.GetSoftwareUpdatePackages();

            //build <language, [pkg names]> map
            foreach (var pkg in listOfSoftwareUpdates)
            {
                var listPkgName = SoftwarePackagesMap.ContainsKey(pkg.Language) ?
                    (List<string>)SoftwarePackagesMap[pkg.Language] : new List<string>();

                if (!listPkgName.Contains(pkg.PartNumber + "_" + pkg.Revision))
                {
                    listPkgName.Add(pkg.PartNumber + "_" + pkg.Revision);
                    //software[name, hash] map
                    SoftwareNameAndHashMap[pkg.PartNumber + "_" + pkg.Revision] = pkg.CRC;
                }
                else
                {
                    Log.Warning($"SoftwareUpdateControl:PopulateSoftwarePackageMap duplicate entry {pkg.Language}:{pkg.PartNumber}:{pkg.Revision}:{pkg.FileSize}:{pkg.CRC}");
                }

                SoftwarePackagesMap[pkg.Language] = listPkgName;
            }
            Log.Information($"SoftwareUpdateControl:PopulateSoftwarePackageMap Total Packages: {listOfSoftwareUpdates.Count} and {SoftwareNameAndHashMap.Count} Exit");
        }

        private void PopulateDocumentPackageMap()
        {
        }

        public void InitializePackageList()
        {
            Log.Information($"SoftwareUpdateControl:InitializePackageList Entry");
            // initialize mPackageSelectionComboBox with possible packages
            LanguageSelectionComboBox.Items.Clear();
            PackageSelectionComboBox.Items.Clear();

            //add languages
            ArrayList softwarePackagesMapList = new ArrayList(SoftwarePackagesMap.Keys);
            softwarePackagesMapList.Sort();

            foreach (string language in softwarePackagesMapList)
            {
                LanguageSelectionComboBox.Items.Add(language);
            }

            if (LanguageSelectionComboBox.Items.Count > 0)
            {
                LanguageSelectionComboBox.SelectedIndex = 0;
            }

            InitializeSelectPackageComboBox();
            Log.Information($"SoftwareUpdateControl:InitializePackageList Exit");

        }

       private void InitializeSelectPackageComboBox()
       {
           Log.Information($"SoftwareUpdateControl:InitializeSelectPackageComboBox Entry");
           PackageSelectionComboBox.Items.Clear();
           FlashDeviceButton.Enabled = false;

           if ((LanguageSelectionComboBox.SelectedIndex >= 0) && 
               SoftwarePackagesMap.ContainsKey((string)LanguageSelectionComboBox.SelectedItem))
           {
               List<string> pkgsList = (List<string>)SoftwarePackagesMap[(string)LanguageSelectionComboBox.SelectedItem];
               pkgsList.Sort();

               foreach (string pkg in pkgsList)
               {
                   PackageSelectionComboBox.Items.Add(pkg);
               }
               if (PackageSelectionComboBox.Items.Count > 0)
               {
               		PackageSelectionComboBox.SelectedIndex = 0;
                    if (!IsDummyDeviceConnected)
                    {
                        FlashDeviceButton.Enabled = true;
                    }
                    else
                    {
                        FlashDeviceButton.Enabled = false;
                    }
               }
           }

           AddBrowseLocalFileItem();
           InitializeSelectDocumentComboBox();
		   
           if(IsDummyDeviceConnected == false)
                RefreshUpdateControls();
            Log.Information($"SoftwareUpdateControl:InitializeSelectPackageComboBox Exit");
        }

        private void InitializeSelectDocumentComboBox()
        {
        }

        private void FlashDeviceButton_Click(object sender, EventArgs e)
        {
            Log.Information($"SoftwareUpdateControl:mFlashDeviceButton_Click Entry");
            string package = (string)PackageSelectionComboBox.SelectedItem;
            string displayName = package;
            IsLocalPackageSelected = false;

            if (package == Properties.Resources.LOAD_LOCAL_FILE)
            {
                OpenFileDialog fileDialog = new OpenFileDialog
                {
                    InitialDirectory = "c:\\",
                    Filter = "Package files (*.pkg)|*.pkg|All files (*.*)|*.*"
                };

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = fileDialog.FileName;
                    //package = SoftwarePackageManagement.Instance.ImportSoftwarePackageFile(fileName);
                    package = fileName;
                    displayName = Path.GetFileName(fileName);
                    if (string.IsNullOrEmpty(package))
                    {
                        // problem
                        Debug.Assert(false);
                    }
					// Setting to true only if it is local package.
                    IsLocalPackageSelected = true;
                }
                else
                {
                    return;
                }
                Log.Information($"SoftwareUpdateControl:mFlashDeviceButton_Click Exit");
            }

            ESS_Main.Instance.GetStatusBar().AddDeviceRequestMessage(Properties.Resources.SW_UPDATE_FLASHING_STATUS_MSG,
                                                                    Properties.Resources.SW_UPDATE_FLASHING_STATUS_MSG);


            DisplaySoftwareUploadForm(package, displayName);
            //uploadDialog.ShowDialog();

            ESS_Main.Instance.GetStatusBar().RemoveDeviceRequestMessage(Properties.Resources.SW_UPDATE_FLASHING_STATUS_MSG);
        }

        private void DisplaySoftwareUploadForm(string package, string displayName)
        {
            Log.Information($"SoftwareUpdateControl:DisplaySoftwareUploadForm Entry");
            this.SuspendLayout();
            ShowControls(false);
            SoftwareUploadControl1.ResetView(package, displayName, IsLocalPackageSelected);
            this.ResumeLayout();
            Log.Information($"SoftwareUpdateControl:DisplaySoftwareUploadForm Exit");
        }

        private void SoftwareUpdateControlCancelClicked(object sender, EventArgs e)
        {
            Log.Information($"SoftwareUpdateControl:SoftwareUpdateControlCancelClicked Entry");
            //upload screen cancel clicked - go back to Software Update Controls
            this.SuspendLayout();

            ShowControls(true);

            SoftwareUploadControl1.Visible = false;
            DeviceSerialNumberControl1.Visible = false;

            this.ResumeLayout();
            Log.Information($"SoftwareUpdateControl:SoftwareUpdateControlCancelClicked Exit");
        }

        private void PackageSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFlashButtonText();
            InitializeSelectDocumentComboBox();
        }

        private void LanguageSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeSelectPackageComboBox();
        }

        private void PackageSelectionComboBox_Click(object sender, EventArgs e)
        {
        }
    }
}
