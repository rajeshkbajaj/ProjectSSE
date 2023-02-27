namespace Oasis.Agent
{
    public class RsaStatus
    {
        public enum RSA_JOB_STATUS
        {
            UNKNOWN,
            COMPLETE,
            SCHEDULED,
            RUNNING,
            ERROR
        };
        public enum RSA_AGENT_STATUS
        {
            UNKNOWN,
            IDLE,
            ACTIVE,
            TRANSMITTING,
            RECEIVING
        };

        public enum RSA_SERVER_STATUS
        {
            UNKNOWN,
            CONNECTED,
            DISCONNECTED
        };

        public RSA_SERVER_STATUS ServerStatus { get; set; }
        public RSA_SERVER_STATUS AgentStatus { get; set; }
        public float UploadTime { get; set; }
        public float DownloadTime { get; set; }
        public bool DataReady { get; set; }

        public bool InsufficientSpace { get; set; }
    }
}
