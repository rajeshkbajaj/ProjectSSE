namespace Oasis.Agent
{
    /// <summary>
    /// The official correct style for PertinentType is "dev classification/msg type classification"
    /// However, there was once an issue/expectation of "{dev classification}/{msg type classification}"
    /// Hence, handling either style... for now... can remove old / bad style later.  Need it to work for integration now!
    /// </summary>
    public class PertinentType
    {
        public string DeviceClassification { get; set; }
        public string MsgTypeClassification { get; set; }

        public PertinentType( string devClass, string msgClass )
        {
            DeviceClassification = devClass;
            MsgTypeClassification = msgClass;
        }



    }
}
