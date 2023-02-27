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
    public class Feature
    {
        public string Name { get; set; } = default;

        public string Id { get; set; } = default;

        public string Status { get; set; } = default;// Active, Available

        public LicenseTerm LicenseTerm { get; set; } = default;
    }
}