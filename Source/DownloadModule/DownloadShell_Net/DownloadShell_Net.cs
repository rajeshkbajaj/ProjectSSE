using System;
using System.Collections;

namespace DownloadShell_Net
{
    public delegate void OnStartDownloadDelegate(string cpu);
    public delegate void OnStatusComponentUpdateDelegate(string cpu, string component, string message);
    public delegate void OnDownloadCompleteDelegate(string status);

    public class DownloadShell_Net
    {

        //delegate FlashProgressCallback(string component, string message, int percentComplete, bool stillGood);


        public event OnStartDownloadDelegate            OnStartDownload;
        public event OnStatusComponentUpdateDelegate    OnDownloadStatusUpdate;
        public event OnDownloadCompleteDelegate         OnDownloadComplete;

        private Boolean mIsDeviceReady;

        private VikingConsoleDriver m_vikingConsoleDriver;
        
        private System.Threading.Thread m_downloadThread;

        private static object MSyncRoot = new Object();

        private bool m_bRunning;
        // Following are not used currently.. so Commenting these members.
        //private string m_guiComponent;
        //private string m_bdComponent;
        //private string m_guiAction;
        //private string m_bdAction;
        //private int m_guiPercent;
        //private int m_bdPercent;

        private Hashtable mDeviceInfoTable;


        public DownloadShell_Net(string path)
        {
            m_bRunning = false;
            mIsDeviceReady = false;

            //m_bdComponent = string.Empty;
            //m_bdAction    = string.Empty;
            //m_bdPercent   = 0;

            //m_guiComponent = string.Empty;
            //m_guiAction    = string.Empty;
            //m_guiPercent   = 0;

            mDeviceInfoTable = new Hashtable();

            m_vikingConsoleDriver = new VikingConsoleDriver(path);

            if (m_vikingConsoleDriver.setDownloadServerWorkingPath(path).Equals(-1))
            {
                //BArf
            }

            m_vikingConsoleDriver.setDelay(0);
            m_vikingConsoleDriver.reset();

            m_downloadThread = new System.Threading.Thread(new
                    System.Threading.ThreadStart(ThreadProc));

            m_downloadThread.Start();
        }

        ~DownloadShell_Net()
        {
            m_bRunning = false;
        }

        public bool IsDeviceReady()
        {
            return mIsDeviceReady;
        }

        public void Start()
        {
            m_vikingConsoleDriver.startDownloadServer();
            m_vikingConsoleDriver.enableTrigger();
        }

        public void ListenForConfig()
        {
            m_vikingConsoleDriver.ListenForConfig();
        }

        public void Stop()
        {
            m_vikingConsoleDriver.stopDownloadServer();
        }

        public void Dispose()
        {
            m_bRunning = false;
        }


        private void ThreadProc()
        {

          //  Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            m_bRunning = true;

            while (m_bRunning)
            {

                VikingConsoleToken token;

                if ((token = m_vikingConsoleDriver.getNextToken()) != null)
                {
                    int nElements = token.getLength();

                    if (nElements == 0) continue;

                    string tokenBuilder = "";
                    for(int i = 0; i < nElements; i++)
                    {
                        tokenBuilder += token.getValue(i) + ",";
                    }

                    string str = token.getValue(0);

                    try
                    {
                        if (token.getValue(0).Equals("GUI"))
                        {
                            ProcessGUIMessage(token, nElements);
                            mIsDeviceReady = true;
                        }

                        else if (token.getValue(0).Equals("BD"))
                        {
                            ProcessBDMessage(token, nElements);
                            mIsDeviceReady = true;
                        }
                    }
                    catch (System.Exception)
                    {
                        /* Do Nothing*/
                    }
                }
            }

            lock (MSyncRoot)
            {
                mDeviceInfoTable.Clear();
            }

            m_vikingConsoleDriver.Dispose();
        }

        private void ProcessBDMessage(VikingConsoleToken token, int nElements)
        {
            if (token.getValue(1).Equals("SET") && nElements > 3)
            {
                if ((token.getValue(2).Equals("LABEL_1")) ||
                    (token.getValue(2).Equals("PROGRESS")) ||
                    (token.getValue(2).Equals("LABEL_2")))
                {
                    OnDownloadStatusUpdate?.Invoke("BD", token.getValue(2), token.getValue(3));
                }
            }
            else if (token.getValue(1).Equals("FINISH"))
            {
                OnDownloadComplete?.Invoke("Success");
            }
            else if (token.getValue(1).Equals("START"))
            {
                m_vikingConsoleDriver.disableTrigger();

                OnStartDownload?.Invoke("BD");
            }
            else if (token.getValue(1).Equals("CONFIG"))
            {
                //get //device_identity or //component/name put them in map
                PopulateDeviceInfoTable("BD", token.getValue(2));
            }
        }

        private void ProcessGUIMessage(VikingConsoleToken token, int nElements)
        {
            if (token.getValue(1).Equals("SET") && nElements > 3)
            {
                //0 - GUI, 1 - SET, 2 - PROGRESS/LABEL1/LABEL2, 3 - update string
                if ((token.getValue(2).Equals("LABEL_1")) ||
                    (token.getValue(2).Equals("PROGRESS")) ||
                    (token.getValue(2).Equals("LABEL_2")))
                {
                    OnDownloadStatusUpdate?.Invoke("GUI", token.getValue(2), token.getValue(3));
                }
            }
            else if (token.getValue(1).Equals("FINISH"))
            {
                OnDownloadComplete?.Invoke("Success");
            }
            else if (token.getValue(1).Equals("START"))
            {
                m_vikingConsoleDriver.disableTrigger();

                OnStartDownload?.Invoke("GUI");
            }
            else if (token.getValue(1).Equals("CONFIG"))
            {
                //get //device_identity or //component/name put them in map
                PopulateDeviceInfoTable("GUI", token.getValue(2));
            }
        }

        void PopulateDeviceInfoTable(string guiBd, string deviceInfoXml)
        {
            lock (MSyncRoot)
            {
                mDeviceInfoTable.Add(guiBd, deviceInfoXml);
            }
        }

        public bool IsDeviceInfoRetrieved()
        {
            lock (MSyncRoot)
            {
                return ((mDeviceInfoTable["BD"] != null) && (mDeviceInfoTable["GUI"] != null));
            }
        }

        public Hashtable GetDeviceInfo()
        {
            Hashtable deviceInfoTable = new Hashtable();

            lock (MSyncRoot)
            {
                foreach (DictionaryEntry pair in mDeviceInfoTable)
                {
                    deviceInfoTable.Add(pair.Key, pair.Value);
                }
            }

            return deviceInfoTable;
        }
    }
}
