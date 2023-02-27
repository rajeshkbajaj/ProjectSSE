// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Oasis.Agent.Models
{
    public class Permission
    {
        public string DeviceType { get; set; } = default;

        public bool UserLogCfgUpload { get; set; } = default;

        public bool UserLicenseUpdate { get; set; } = default;

        public int UserSoftwareAccess { get; set; } = default;

        public bool TestingSoftware { get; set; } = default;

        public int UserDeviceAccess { get; set; } = default;

        public bool ChangeCountryCode { get; set; } = default;
        
        public bool ChangeCountryCodeCovidien { get; set; } = default;
        
        public bool CovidienCountry { get; set; } = default;
        
        public bool ChangeSerialNumber { get; set; } = default;
        
        public bool FeatureEntitlement { get; set; } = default;
        
        public bool LimitedFeatureEntitlement { get; set; } = default;
        
        public bool SaveDeviceSettings { get; set; } = default;
        
        public bool SaveLogs { get; set; } = default;

        public bool TransferSavedSettings { get; set; } = default;

        public bool RotateLogs { get; set; } = default;

        public Notification UpdateNotification { get; set; } = default;
    }
}

/*
 * Following are missing... 
 
      "modetoFeatureDisplayMapping": null,
      "modetoFeatureDisableMapping": null,
      "modetoFeatureEnableMapping": null,
      "hasFT10LimitedDowngradeAbility": false,
      "featurePermissions": {}
 */