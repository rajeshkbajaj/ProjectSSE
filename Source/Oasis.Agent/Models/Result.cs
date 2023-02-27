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
    public class Result
    {
        /// <summary>
        /// Only used by VLEX, according to Mark S.
        /// </summary>
        public bool Expired { get; set; } = default;

        /// <summary>
        /// Whether or not the request (and this result) is successful
        /// </summary>
        public bool Success { get; set; } = default;

        /// <summary>
        /// Error message if unsuccessful
        /// </summary>
        public string ErrMsg { get; set; } = default;
    }
}