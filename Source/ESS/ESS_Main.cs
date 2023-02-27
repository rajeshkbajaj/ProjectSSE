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
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;
    using Covidien.CGRS.PcAgentInterfaceBusiness;
    using Newtonsoft.Json;
    using Serilog;
    using Utilties;

    public partial class ESS_Main : Form
    {
        public enum StateChangeEvent { END_USER_LICENSE_ERROR, END_USER_LICENSE_ACCEPT, USER_TYPE_SELECTED_MED, USER_TYPE_SELECTED_NONMED, GET_USER_TYPE, END_USER_LICENSE_CANCEL, RSA_NOTAVAILABLE, RSA_CONNECTED, SESSION_OPEN_FAILURE, SESSION_OPENED, SESSION_CLOSED, REQUEST_OFFLINE_CODE, USER_AUTHENTICATED, USER_AUTHENTICATION_FAILED, LOGIN_CANCELED, SPLASH_SCREEN_TIMEOUT, DEVICE_CONNECTED, DEVICE_CONNECTED_IN_DOWNLOAD_MODE, DEVICE_DISCONNECTED, LOGOUT, USER_APP_CLOSE, DUMMY_DEVICE_CONNECTED};
        public enum SystemEvent { DEVICE_REGISTRATION_SUCCESSFUL, DEVICE_REGISTRATION_FAILURE, AGENT_DOWNLOAD_STATUS_COMPLETE };
        public enum UserType { USER_TYPE_NONE, USER_TYPE_MEDTRONIC, USER_TYPE_NONMEDTRONIC };
        public enum MainPanel { BLANK, CONNECT_INFO, DEVICE_INFO, LOGS, SW_UPDATE, CONFIG };
        public delegate void ESSEventHandler(SystemEvent e);

        private enum UIstate { APP_START, END_USER_LICENSE, USER_TYPE_SELECT, STARTUP, RSA_INACTIVE, RSA_ACTIVE, SESSION_INACTIVE, SESSION_ACTIVE, SPLASH_SCREEN_DISPLAYED, NOT_CONNECTED, CONNECTED, LOGGING_IN, LOGGING_OUT, APP_CLOSING };

        private delegate void StateChangeEventHandler(StateChangeEvent e);
        private event StateChangeEventHandler StateChange;               
        private event ESSEventHandler HandleEvent;

        /// <summary>
        /// The variable provides the locking object
        /// </summary>
        private static readonly object MSyncRoot = new object();
        private readonly Timer AppCloseTimer;

        // time-limit in days for temp file expiration in application directory
        private readonly int TempFilesExpireDays = 90;
        private UIstate State;
        private readonly EndUserLicenseAgreementControl EulaControl;
        private readonly DeviceInfoPanel DeviceInfoPanel;
        private readonly DeviceConnectInstructions ConnectInstructionsPanel;
        private readonly MainTabControl MainTabPanel;
        private readonly SystemStatusBar StatusBar;
        private SplashScreenControl SplashScreen;
        private readonly LoginRequestControl LoginRequestCntrl;
        private readonly UserTypeControl UserTypeControl;
        private UserType UserTypeSetting;
        private EssSettings Settings;
        private readonly LoginSetPasscode LoginSetPasscodePanel;

        /// <summary>
        /// MSingleton and Instance provides the private variable and public access to it - to provide the Singleton.
        /// The double-check method for lazy evaluation of the Singleton properly supports multi-threaded applications
        /// </summary>
        private static volatile ESS_Main MSingleton;
        public static ESS_Main Instance
        {
            get
            {
                if (null == MSingleton)
                {
                    lock (MSyncRoot)
                    {
                        if (null == MSingleton)
                        {
                            MSingleton = new ESS_Main();
                        }
                    }
                }

                return MSingleton;
            }
        }

        public string GetESSVersion()
        {
            return VersionHelper.ExecutingAssemblyVersion;
        }

        public SystemStatusBar GetStatusBar()
        {
            return StatusBar;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private ESS_Main()
        {
            LoadJson();
            InitializeSeriLog();
            InitializeComponent();
            
            MSingleton = this;
            BusinessServicesBridge.Instance.Initialize(Settings);
            UserTypeSetting = UserType.USER_TYPE_NONE;
            TempFilesExpireDays = Settings.TempFilesExpireDays;

            EulaControl = new EndUserLicenseAgreementControl();
            UserTypeControl = new UserTypeControl();
            StatusBar = new SystemStatusBar
            {
                Location = new System.Drawing.Point(0, 545)
            };

            ConnectInstructionsPanel = new DeviceConnectInstructions();
            ConnectInstructionsPanel.HidePanel();
            this.MainPanel1.Controls.Add(ConnectInstructionsPanel);

            DeviceInfoPanel = new DeviceInfoPanel(Settings)
            {
                Location = new System.Drawing.Point(615, 0),
                Visible = false
            };
            this.Controls.Add(DeviceInfoPanel);

            MainTabPanel = new MainTabControl(Settings);
            MainTabPanel.HidePanel();
            this.MainPanel1.Controls.Add(MainTabPanel);

            LoginRequestCntrl = new LoginRequestControl();
            LoginRequestCntrl.Initialize();
            LoginSetPasscodePanel = new LoginSetPasscode();
            LoginSetPasscodePanel.Initialize();

            this.StateChange += new StateChangeEventHandler(ProcessStateChangeEvents);
            this.HandleEvent += new ESSEventHandler(ProcessSystemEvents);
            this.Show();

            // ESC key closes application
            this.CancelButton = new Button();
            ((Button)this.CancelButton).Click += delegate (object o, EventArgs e) { this.Close(); };

            AppCloseTimer = new Timer();
            AppCloseTimer.Tick += new EventHandler(this.AppCloseTimerExpired);

            //clean up operation - delete old files on PC file system
            BusinessServicesBridge.Instance.DeleteExpiredFilesOnPC(TempFilesExpireDays);

            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            EnterAppStartState();
        }

        /// <summary>
        /// Loads the ESS's json config setting and updates Setting member.
        /// </summary>
        private void LoadJson()
        {
            try
            {
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, "essConfigSettings.json")))
                {
                    var data = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "essConfigSettings.json"));
                    Settings = JsonConvert.DeserializeObject<EssSettings>(data);
                    Log.Information("ESSMain::LoadJson essConfigSettings.json parse completed");
                }
                else
                {
                    Log.Error("ESSMain::LoadJson essConfigSettings.json file not found");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"ESSMain::LoadJson failed to parse essConfigSettings.json {ex.Message}");
            }
        }

        /// <summary>
        /// Initialize SeriLog, this will be used for logging purpose
        /// </summary>
        private void InitializeSeriLog()
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.File(Settings.SeriLogsFilePath, rollingInterval: RollingInterval.Day)
                            .CreateLogger();
        }

        /// <summary>
        /// Called on Application Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                MainTabPanel.ApplicationClose();

                if (BusinessServicesBridge.Instance.IsRSASessionOpened() == true)
                {
                    BusinessServicesBridge.Instance.CloseSession();
                }
                Log.Information($"ESSMain::OnApplicationExit closing application");
            }
            catch (Exception)         
            { }             
        }

        /// <summary>
        /// when app close is hit, 15 seconds are given to stop download, close session, etc. acknowledged by the agent/ server
        /// once the timer expires, application exits. in most cases, this timer would not expire as the cleanup should complete, 
        /// but some times, the server takes too long to complete, and the connection might drop, this timer can be used as a fail-safe in those scnearios
        /// </summary>
        private void StartAppCloseTimer()
        {
            AppCloseTimer.Interval = 15000;
            AppCloseTimer.Enabled = true;
            Log.Information($"ESSMain::StartAppCloseTimer exit timer started {AppCloseTimer.Interval}");
        }

        /// <summary>
        /// application exit timer expired. this means that we were not able to cleanup before 15 seconds.
        /// application is force-exited. this does not happen unless server is too slow/ dead
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppCloseTimerExpired(object sender, EventArgs e)
        {
            AppCloseTimer.Enabled = false;
            Application.Exit();
        }

        /// <summary>
        /// Generates StateChange Event to ESS_Main application
        /// </summary>
        /// <param name="e"></param>
        public void GenerateStateChangeEvent(StateChangeEvent e)
        {
            StateChange?.Invoke(e);
        }

        public void GenerateSystemEvent(SystemEvent e)
        {
            HandleEvent?.Invoke(e);
        }

        /// <summary>
        /// Handles the Process StateChange Events
        /// </summary>
        private void ProcessStateChangeEvents(StateChangeEvent e)
        {
            Log.Information($"ESSMain::ProcessStateChangeEvents entry state: {State} event: {e} ");

            BusinessServicesBridge.Instance.FetchAndStoreCertificate();
            switch (State)
            {
                case UIstate.APP_START:
                    AppStartState(e);
                    break;

                case UIstate.END_USER_LICENSE:
                    EulaState(e);
                    break;

                case UIstate.USER_TYPE_SELECT:
                    UserTypeSelectState(e);
                    break;

                case UIstate.STARTUP:
                    StartUpState(e);
                    break;

                case UIstate.RSA_INACTIVE:
                    RsaNotAvailable(e);
                    break;

                case UIstate.RSA_ACTIVE:
                    RsaConnected(e);
                    break;

                case UIstate.SESSION_ACTIVE:
                    SessionActive(e);
                    break;

                case UIstate.SESSION_INACTIVE:
                    SessionInactive(e);
                    break;

                case UIstate.SPLASH_SCREEN_DISPLAYED:
                    SplashScreenDisplayed(e);
                    break;

                case UIstate.NOT_CONNECTED:
                    DisconnectedDeviceState(e);
                    break;

                case UIstate.CONNECTED:
                    ConnectedDeviceState(e);
                    break;

                case UIstate.LOGGING_OUT:
                    LoggingOutState(e);
                    break;

                case UIstate.APP_CLOSING:
                    AppClosingState(e);
                    break;

                case UIstate.LOGGING_IN:
                    UserLoggingInState(e);
                    break;

                default:
                    // state not handled
                    break;
            }
            Log.Information($"ESSMain::ProcessStateChangeEvents exit state: {State}");
        }


        /// <summary>
        /// Processes the Device/Agent SystemEvents
        /// </summary>
        /// <param name="e"></param>
        private void ProcessSystemEvents(SystemEvent e)
        {
            Log.Information($"ESSMain::ProcessSystemEvents entry event: {e}");
            if (e == SystemEvent.DEVICE_REGISTRATION_SUCCESSFUL)
            {
                DeviceRegistered(true);
            }
            else if (e == SystemEvent.DEVICE_REGISTRATION_FAILURE)
            {
                DeviceRegistered(false);
            }
            else if (e == SystemEvent.AGENT_DOWNLOAD_STATUS_COMPLETE)
            {
                AgentDownloadComplete();
            }
            Log.Information($"ESSMain::ProcessSystemEvents exit");
        }

        /// <summary>
        /// Updates Device Registered status to MainTabPanel
        /// </summary>
        /// <param name="registrationStatus"></param>
        private void DeviceRegistered(bool registrationStatus)
        {
            MainTabPanel.DeviceRegistered(registrationStatus);
        }

        /// <summary>
        /// Updates Agent Download Complete status to MainTabPanel
        /// </summary>
        private void AgentDownloadComplete()
        {
            MainTabPanel.AgentDownloadComplete();
        }

        /// <summary>
        /// Entery piont for Application StartState
        /// </summary>
        private void EnterAppStartState()
        {
            SplashScreen = new SplashScreenControl();
            int y = (this.Size.Height - SplashScreen.Size.Height) / 2 - 35;
            int x = 0;
            SplashScreen.Location = new System.Drawing.Point(x, y);
            this.Controls.Add(SplashScreen);
            SplashScreen.StartTimer(Settings.SplashScreenTimeOutInSec);
            State = UIstate.APP_START;
        }

        /// <summary>
        /// This method shows the End User License Info
        /// </summary>
        private void EnterShowEndUserLicenseState()
        {
            State = UIstate.END_USER_LICENSE;
            this.Controls.Add(EulaControl);
        }

        /// <summary>
        /// This method shows User Type Selection Form
        /// </summary>
        private void EnterUserTypeSelectState()
        {
            State = UIstate.USER_TYPE_SELECT;
            this.Controls.Add(UserTypeControl);
        }


        /// <summary>
        /// This method handles the ESS Starup starte functionality
        /// </summary>
        private void EnterStartUpState()
        {
            State = UIstate.STARTUP;

            this.Controls.Add(StatusBar);
            this.Controls.Add(SplashScreen);
            SplashScreen.StartTimer(Settings.StartUpTimeOutInSec);
            var isSessionEstablished = BusinessServicesBridge.Instance.EstablishSession();
            Log.Information($"ESSMain::EnterStartUpState sessionEstablished: {isSessionEstablished}");

            if (isSessionEstablished)
            {
                HandleLogin();
            }

            DeviceManagement.Instance.RequestSupportedDeviceTypes(null);
        }

        /// <summary>
        /// Handles Login functionality
        /// </summary>
        private void HandleLogin()
        {
            BusinessServicesBridge.Instance.GetServerStatus();
            StatusBar.RSAConnected(true, BusinessServicesBridge.Instance.IsServerAvialable);

            this.Controls.Remove(SplashScreen);
            Log.Information($"ESSMain::EnterStartUpState online: {BusinessServicesBridge.Instance.IsServerAvialable}");

            StatusBar.RSAConnected(true, BusinessServicesBridge.Instance.IsServerAvialable);

            if (BusinessServicesBridge.Instance.IsServerAvialable)
            {
                Log.Information($"ESSMain::EnterStartUpState online mode, selecting user type");
                EnterUserTypeSelectState();
            }
            else
            {
                Log.Information($"ESSMain::EnterStartUpState offline mode, offline login triggered");
                EnterUserOfflineLoginState();
            }
            SplashScreen.DisplayLabel(Properties.Resources.SPLASH_SCREEN_AUTHENITICATE_WAIT);
            this.Controls.Add(SplashScreen);
        }

        /// <summary>
        /// Application StartState method
        /// </summary>
        /// <param name="e"></param>
        private void AppStartState(StateChangeEvent e)
        {
            if (e == StateChangeEvent.SPLASH_SCREEN_TIMEOUT)
            {
                //show EULA
                this.Controls.Remove(SplashScreen);
                EnterShowEndUserLicenseState();
            }
        }

        /// <summary>
        /// End User License State handling
        /// </summary>
        /// <param name="e"></param>
        private void EulaState(StateChangeEvent e)
        {
            if (e == StateChangeEvent.END_USER_LICENSE_ACCEPT)
            {
                this.Controls.Remove(EulaControl);
                EnterStartUpState();
            }
            else if (e == StateChangeEvent.END_USER_LICENSE_CANCEL)
            {
                Log.Error($"ESSMain::EulaState user cancelled license terms");
                DialogResult result = MessageBox.Show(Properties.Resources.APP_CLOSE_MSGBOX_TEXT_EULA_CANCEL,
                          Properties.Resources.ESS_TITLE,
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    GenerateStateChangeEvent(StateChangeEvent.USER_APP_CLOSE);
                }
            }
            else if (e == StateChangeEvent.END_USER_LICENSE_ERROR)
            {
                Log.Error($"ESSMain::EulaState user error in license terms processing");
                MessageBox.Show(Properties.Resources.APP_CLOSE_MSGBOX_TEXT_EULA_ERROR,
                          Properties.Resources.ESS_TITLE,
                          MessageBoxButtons.OK, MessageBoxIcon.Question);

                GenerateStateChangeEvent(StateChangeEvent.USER_APP_CLOSE);
            }
            else if (e == StateChangeEvent.USER_APP_CLOSE)
            {
                Log.Error($"ESSMain::EulaState closing application");
                Application.Exit();
            }
            else
            {
                Debug.Assert(false);
            }
        }

        /// <summary>
        /// Handle Employee/non-Employee UserTypeSelect states.
        /// </summary>
        /// <param name="e"></param>
        private void UserTypeSelectState(StateChangeEvent e)
        {
            Log.Information($"ESSMain::UserTypeSelectState type: {e}");

            if (e == StateChangeEvent.USER_TYPE_SELECTED_MED || e == StateChangeEvent.USER_TYPE_SELECTED_NONMED)
            {
                this.Controls.Remove(UserTypeControl);
                if (BusinessServicesBridge.Instance.IsServerAvialable)
                {
                    UserTypeSetting = (e == StateChangeEvent.USER_TYPE_SELECTED_MED)? UserType.USER_TYPE_MEDTRONIC: UserType.USER_TYPE_NONMEDTRONIC;
                    EnterUserLoginState();
                }
                else
                {
                    Log.Information($"ESSMain::EnterStartUpState offline mode, offline login triggered");
                    EnterUserOfflineLoginState();
                }
            }
            else if (e == StateChangeEvent.USER_APP_CLOSE)
            {
                Application.Exit();
            }
            else
            {
                Log.Error($"ESSMain::UserTypeSelectState invalid user type: {e}");
                Debug.Assert(false);
            }
        }

        /// <summary>
        /// Updates the ESS State member based on event in StartupState
        /// </summary>
        /// <param name="e">StateChangeEvent</param>
        private void StartUpState(StateChangeEvent e)
        {
            if (e == StateChangeEvent.RSA_CONNECTED)
            {
                State = UIstate.RSA_ACTIVE;
            }
            else if (e == StateChangeEvent.SPLASH_SCREEN_TIMEOUT)
            {
                State = UIstate.SPLASH_SCREEN_DISPLAYED;
            }
            else if (e == StateChangeEvent.RSA_NOTAVAILABLE)
            {
                State = UIstate.RSA_INACTIVE;
            }
            else
            { // Illegal transition
                Debug.Assert(false);
            }
        }

        /// <summary>
        /// Handles the state change events when RSA agent is not connected
        /// </summary>
        /// <param name="e"></param>
        private void RsaNotAvailable(StateChangeEvent e)
        {
            Log.Debug($"RsaNotAvailable: {e}");
            ExitStartUpState();
        }

        /// <summary>
        /// Handles the state change events when RSA agent is connected
        /// </summary>
        /// <param name="e"></param>
        private void RsaConnected(StateChangeEvent e)
        {
            if (e == StateChangeEvent.SESSION_OPENED)
            {
                State = UIstate.SESSION_ACTIVE;
                StatusBar.RSAConnected(true, BusinessServicesBridge.Instance.IsServerAvialable);
            }
            else if (e == StateChangeEvent.SPLASH_SCREEN_TIMEOUT)
            {
                State = UIstate.SPLASH_SCREEN_DISPLAYED;
            }
            else if (e == StateChangeEvent.SESSION_OPEN_FAILURE)
            {
                State = UIstate.SESSION_INACTIVE;
            }
            else
            {     // Illegal transition
                Debug.Assert(false);
            }
        }

        /// <summary>
        /// Handles the state change events when Agent Session is active
        /// </summary>
        /// <param name="e"></param>
        private void SessionActive(StateChangeEvent e)
        {
            if (e == StateChangeEvent.SPLASH_SCREEN_TIMEOUT)
            {
                ExitStartUpState();
            }
            else
            {
                // Illegal transition
                Debug.Assert(false);
            }
        }

        /// <summary>
        /// Handles the state change events when Agent Session is inactive
        /// </summary>
        /// <param name="e"></param>
        private void SessionInactive(StateChangeEvent e)
        {
            if (e == StateChangeEvent.SPLASH_SCREEN_TIMEOUT)
            {
                ExitStartUpState();
            }
            else
                // Illegal transition
                Debug.Assert(false);
        }

        /// <summary>
        /// Handles the state change events when SplashScreen is displayed
        /// </summary>
        /// <param name="e"></param>
        private void SplashScreenDisplayed(StateChangeEvent e)
        {
            if (e == StateChangeEvent.RSA_CONNECTED)
            {
                // sit here, do nothing
            }
            else if (e == StateChangeEvent.RSA_NOTAVAILABLE)
            {
                ExitStartUpState();
            }
            else if (e == StateChangeEvent.SESSION_OPENED)
            {
                ExitStartUpState();
                StatusBar.RSAConnected(true, BusinessServicesBridge.Instance.IsServerAvialable);
            }
            else if (e == StateChangeEvent.SESSION_OPEN_FAILURE)
            {
                ExitStartUpState();
            }
            else
            {   // Illegal transition
                Debug.Assert(false);
            }
        }

        /// <summary>
        /// Once Startup state is completed, then splash screen will be removed.
        /// Calls the EnterUserLoginState method
        /// </summary>
        private void ExitStartUpState()
        {
            // go to device disconnected state
            this.Controls.Remove(SplashScreen);
            SplashScreen.Visible = false;
            SplashScreen = null;

            //here we can check for session not open and go to EnterAgentNotPresentState()
            //but we go to loginstate and quit if session is not opened
            EnterUserLoginState();
        }

        /// <summary>
        /// Handles the User Login functionality
        /// </summary>
        private void EnterUserLoginState()
        {
            Log.Information($"ESSMain::EnterUserLoginState entry");

            State = UIstate.LOGGING_IN;

            if (BusinessServicesBridge.Instance.IsRSASessionOpened() == false ||
                BusinessServicesBridge.Instance.IsServerAvialable == false)
            {
                Log.Error($"ESSMain::EnterUserLoginState failed to open session. Closing application.");
                //RSA, show message box, onOK -> exit
                MessageBox.Show(Properties.Resources.NO_AGENT_APPLICATION_CLOSE_MSG1 + Environment.NewLine + Properties.Resources.NO_AGENT_APPLICATION_CLOSE_MSG2,
                          Properties.Resources.ESS_TITLE,
                          MessageBoxButtons.OK, MessageBoxIcon.Stop);

                Application.Exit();
            }
            else
            {
                Log.Information($"ESSMain::EnterUserLoginState OIDC Auth2 login is triggered..");
                OAuth2LoginManager.Instance().Login(BusinessServicesBridge.Instance.GetAuthServerInfo(),
                                                    BusinessServicesBridge.Instance.IsServerAvialable,
                                                    UserTypeSetting);
            }
            Log.Information($"ESSMain::EnterUserLoginState exit");
        }

        /// <summary>
        /// Prompts user with Offline user login form.
        /// </summary>
        private void EnterUserOfflineLoginState()
        {
            Log.Information($"ESSMain::EnterUserOfflineLoginState offline login form triggered...");
            State = UIstate.LOGGING_IN;
            this.Controls.Add(LoginRequestCntrl);
            LoginRequestCntrl.Visible = true;
        }

        /// <summary>
        /// This method is called when when device is not yet connected and Agent is active.
        /// </summary>
        private void EnterAgentNotPresentState()
        {
            State = UIstate.NOT_CONNECTED;

            // enable display elements
            DeviceInfoPanel.Visible = true;
            MainPanel1.Visible = true;

            // update display elements
            //mDeviceInfoPanel.DeviceConnected(false);
            StatusBar.DeviceConnected(false, false);
            DeviceInfoPanel.HandleDeviceDisconnect();
            MainPanelDisplay(MainPanel.CONNECT_INFO);
            MainTabPanel.DeviceDisconnected();
        }

        /// <summary>
        /// Updates the Status bar with Agent, Server as Disconnected
        /// </summary>
        private void HandleRSADisconnectedEvent()
        {
            //System Status bar RSA Disconnect
            StatusBar.RSAConnected(false, false);
        }

        /// <summary>
        /// Handles the state change event in DisconnectedDeviceState
        /// </summary>
        /// <param name="e"></param>
        private void DisconnectedDeviceState(StateChangeEvent e)
        {
            Log.Information($"ESSMain::DisconnectedDeviceState entry state:{State} event: {e}");

            if (e == StateChangeEvent.DEVICE_CONNECTED)
            {
                // go to connected device state
                EnterConnectedDeviceState();
            }
            else if (e == StateChangeEvent.DEVICE_CONNECTED_IN_DOWNLOAD_MODE)
            {
                // go to connected device state
                EnterConnectedInDownloadModeDeviceState();
            }
            else if (e == StateChangeEvent.USER_APP_CLOSE)
            {
                EnterAppCloseState();
            }
            else if (e == StateChangeEvent.DUMMY_DEVICE_CONNECTED)
            {
                EnterDummyDeviceConnectedState();
            }
            else if (e == StateChangeEvent.LOGOUT)
            {
                EnterLogOutState();
            }
            else if (e == StateChangeEvent.SESSION_CLOSED)
            {
                Application.Exit();
            }
            else if (e == StateChangeEvent.RSA_CONNECTED)
            {
                // Occurs when user or anonymous logs in after startup
            }
            else
            {
                // illegal transition
                Log.Error($"ESSMain::DisconnectedDeviceState entry state:{State} invalid event: {e}");
                bool DisconnectedDeviceState = false;
                bool IllegalTransition = true;
                Debug.Assert(DisconnectedDeviceState == IllegalTransition);
            }
            Log.Information($"ESSMain::DisconnectedDeviceState exit state:{State}");
        }

        /// <summary>
        /// When Device is not connected State, greyout Logs, Settings in MainTabPanel and shows config info.
        /// </summary>
        private void EnterDummyDeviceConnectedState()
        {
            State = UIstate.CONNECTED;

            Instance.EnableAllControls();

            StatusBar.DeviceConnected(false, false);
            MainTabPanel.DeviceNotConnectedShowServerInfo();
            MainPanelDisplay(MainPanel.CONFIG);
        }

        /// <summary>
        /// Enters into Device Connected State. Shows the Logs, MainTabPanel info.
        /// </summary>
        private void EnterConnectedDeviceState()
        {
            State = UIstate.CONNECTED;

            Instance.EnableAllControls();

            StatusBar.DeviceConnected(true, false);
            MainTabPanel.DeviceConnected();
            MainPanelDisplay(MainPanel.LOGS);
        }

        /// <summary>
        /// Enters into DownloadMode Device Connected State
        /// </summary>
        private void EnterConnectedInDownloadModeDeviceState()
        {
            State = UIstate.CONNECTED;

            StatusBar.DeviceConnected(true, true);
            MainTabPanel.DeviceConnectedInDownloadMode();
            MainPanelDisplay(MainPanel.SW_UPDATE);
        }

        /// <summary>
        /// Handles the state change event in DeviceConnectedState
        /// </summary>
        /// <param name="e"></param>
        private void ConnectedDeviceState(StateChangeEvent e)
        {
            Log.Information($"ESSMain::ConnectedDeviceState entry state:{State} event: {e}");

            if (e == StateChangeEvent.DEVICE_DISCONNECTED)
            {
                // go to disconnected device state
                DeviceManagement.Instance.DisconnectDevice();
                EnterAgentNotPresentState();
            }
            else if (e == StateChangeEvent.USER_APP_CLOSE)
            {
                EnterAppCloseState();
            }
            else if (e == StateChangeEvent.LOGOUT)
            {
                EnterLogOutState();
            }
            else if (e == StateChangeEvent.SESSION_CLOSED)
            {
                Application.Exit();
            }
            else if (e == StateChangeEvent.RSA_NOTAVAILABLE)
            {
                HandleRSADisconnectedEvent();
            }
            else
            {
                Log.Error($"ESSMain::ConnectedDeviceState state:{State} invalid event: {e}");
                // illegal transition
                bool ConnectedDeviceState = false;
                bool IllegalTransition = true;
                Debug.Assert(ConnectedDeviceState == IllegalTransition);
            }
            Log.Information($"ESSMain::ConnectedDeviceState exit state:{State}");
        }

        /// <summary>
        /// Handles cancel request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ESS_Main_Closing(object sender, CancelEventArgs e )
        {
            Log.Information($"ESSMain::ESS_Main_Closing entry state:{State} event:{e}");

            if ((State == UIstate.CONNECTED) ||
                (State == UIstate.NOT_CONNECTED) ||
                (State == UIstate.LOGGING_IN))
            {
                ConfirmApplicationCloseRequest();
                e.Cancel = true;
            }
            else if (State == UIstate.APP_CLOSING)
            {
                // patience
                e.Cancel = true;
            }
            Log.Information($"ESSMain::ESS_Main_Closing exit state:{State}");
        }

        /// <summary>
        /// ConfirmApplicationCloseRequest
        /// </summary>
        public void ConfirmApplicationCloseRequest()
        {
            DialogResult result = MessageBox.Show(Properties.Resources.APP_CLOSE_MSGBOX_TEXT,
                                      Properties.Resources.ESS_TITLE,
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                GenerateStateChangeEvent(StateChangeEvent.USER_APP_CLOSE);
            }
        }
        
        /// <summary>
        /// called in response to user action of application close
        /// </summary>
        private void EnterAppCloseState()
        {
            State = UIstate.APP_CLOSING;
            this.Cursor = Cursors.WaitCursor;

            MainTabPanel.ApplicationClose();

            //close session
            // log out from server, wait for response
            // upon success, SessionManagement would generate a SESSION_CLOSED state change, which would call App.Exit()
            if (BusinessServicesBridge.Instance.IsRSASessionOpened() == true)
            {
                BusinessServicesBridge.Instance.CloseSession();
                StartAppCloseTimer();
            }
            else
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Handles ESS LogOut State. Closes the RSA sessions
        /// </summary>
        private void EnterLogOutState()
        {
            // log out from server, wait for response
            // upon success, SessionManagement would generate a SESSIONN_CLOSED state change, which would call App.Exit()
            if (BusinessServicesBridge.Instance.IsRSASessionOpened() == true)
            {
                BusinessServicesBridge.Instance.CloseSession();
            }
            else
            {
                Application.Exit();
            }
        }

        private void LoggingOutState(StateChangeEvent e)
        {
            Log.Debug($"{e}");
            // will never get here
            bool LoggingOutState = false;
            bool IllegalTransition = true;
            Debug.Assert(LoggingOutState == IllegalTransition);
        }

        /// <summary>
        /// Called when state changes while in Application closing state
        /// </summary>
        /// <param name="e"></param>
         private void AppClosingState(StateChangeEvent e)
         {
             if (e == StateChangeEvent.SESSION_CLOSED)
             {
                 StatusBar.SessionClosed();
             }

             //just exit the application for all events
             //this is the final state
             Application.Exit();
         }

        /// <summary>
        /// Handles the State Change events in UserLoggingInState
        /// </summary>
        /// <param name="e"></param>
        private void UserLoggingInState(StateChangeEvent e)
         {
            Log.Information($"ESSMain::UserLoggingInState entry state:{State} event:{e}");
            if (e == StateChangeEvent.USER_APP_CLOSE || e == StateChangeEvent.LOGIN_CANCELED)
            {
                EnterAppCloseState();
            }
            else if (e == StateChangeEvent.USER_AUTHENTICATED)
            {
                this.Controls.Remove(LoginSetPasscodePanel);
                LoginSetPasscodePanel.Visible = false;
                this.Controls.Remove(LoginRequestCntrl);
                LoginRequestCntrl.Visible = false;
                this.Controls.Remove(SplashScreen);
                SplashScreen.HideLabel();
                SplashScreen.Visible = false;

				// Show Device Country Panel
                DeviceCountryControl DeviceCountryControlPanel = new DeviceCountryControl() { Visible = true };
                DeviceCountryControlPanel.SetCountryCode(BusinessServicesBridge.Instance.GetUserCountryCode());
                DeviceCountryControlPanel.DeviceCountrySubmitDone += DeviceCountrySubmitted;
                this.Controls.Add(DeviceCountryControlPanel);
            }
            else if (e == StateChangeEvent.REQUEST_OFFLINE_CODE)
            {
                // Show UI form to set offline passcode.
                // Once setoffline is success or fail.. redirect to USER_AUTHENTICATED
                this.Controls.Remove(SplashScreen);
                SplashScreen.Visible = false;
                LoginSetPasscodePanel.SetUserInfo(BusinessServicesBridge.Instance.GetLoggedInUserName());
                this.Controls.Add(LoginSetPasscodePanel);
                LoginSetPasscodePanel.Visible = true;
            }
            else if(e == StateChangeEvent.USER_AUTHENTICATION_FAILED)
            {
                this.Controls.Remove(LoginRequestCntrl);
                this.Controls.Add(SplashScreen);
                UserLogInCancel();

                HandleLogin();
            }
            else
            {
                Log.Error($"ESSMain::UserLoggingInState state:{State} invalid event:{e}");
                // illegal transition
                bool ConnectedDeviceState = false;
                bool IllegalTransition = true;
                Debug.Assert(ConnectedDeviceState == IllegalTransition);
            }
            Log.Information($"ESSMain::UserLoggingInState exit state:{State}");
        }

        /// <summary>
        /// Prompted Device Country Selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceCountrySubmitted(object sender, EventArgs e)
        {
            EnterAgentNotPresentState();
            UserLoggedIn(BusinessServicesBridge.Instance.IsServerAvialable);
        }

        /// <summary>
        /// Displays MainPanel Form and handles the MainPanel events
        /// </summary>
        /// <param name="display"></param>
        public void MainPanelDisplay( MainPanel display)
        {
            Log.Information($"ESSMain::MainPanelDisplay entry state:{State} event:{display}");
            switch (display)
            {
                case MainPanel.CONNECT_INFO:
                    ConnectInstructionsPanel.ShowPanel();
                    MainTabPanel.HidePanel();
                    break;

                case MainPanel.LOGS:
                    ConnectInstructionsPanel.HidePanel();
                    MainTabPanel.ShowPanel();
                    break;

                case MainPanel.SW_UPDATE:
                    ConnectInstructionsPanel.HidePanel();
                    MainTabPanel.ShowSoftwareDownloadPanel();
                    break;

                case MainPanel.CONFIG:
                    ConnectInstructionsPanel.HidePanel();
                    MainTabPanel.ShowConfigPanel();
                    break;

                case MainPanel.DEVICE_INFO:
                    ConnectInstructionsPanel.HidePanel();
                    break;

                default: // BLANK
                    ConnectInstructionsPanel.HidePanel();
                    break;
            }
            Log.Information($"ESSMain::MainPanelDisplay exit state:{State}");
        }

        /// <summary>
        /// Enables All User Controls
        /// </summary>
        public void EnableAllControls()
        {
            DeviceInfoPanel.EnableControls();
            MainTabPanel.EnableControls();
        }

        /// <summary>
        /// Disables All User Controls
        /// </summary>
        public void DisableAllControls()
        {
            DeviceInfoPanel.DisableControls();
            MainTabPanel.DisableControls();
        }

        /// <summary>
        /// Method handles the User Login Request and handover request to DeviceInfoPanel, Status bar and Main Tab Panel.
        /// </summary>
        private void UserLoggedIn(bool isOnlineLogin)
        {
            DeviceInfoPanel.UserLoggedIn();
            StatusBar.UserLoggedIn();
            MainTabPanel.UserLoggedIn(isOnlineLogin);
        }

        /// <summary>
        /// This method updates DeviceInfoPanel, MainTabPanel regarding User Login Cancel Request
        /// </summary>
        private void UserLogInCancel()
        {
            DeviceInfoPanel.UserLoginCancel();
            MainTabPanel.UserLogInCancel();
        }

        /// <summary>
        /// This method returns the Device/Ventilator SerialNumber
        /// </summary>
        /// <returns></returns>
        public string GetVentSerialNumber()
        {
            return DeviceInfoPanel.GetVentSerialNumber();
        }
    }
}

