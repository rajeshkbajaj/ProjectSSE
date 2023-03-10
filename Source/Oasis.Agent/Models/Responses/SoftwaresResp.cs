// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Oasis.Agent.Models.Responses
{
    using System.Collections.Generic;

    public class SoftwaresResp
    {
        public Result Result { get; set; }

        public List<Software> Softwares { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SoftwaresResp" /> class.
        /// </summary>
        public SoftwaresResp()
        {
            Result = new Result();
            Softwares = new List<Software>();
        }
    }
}