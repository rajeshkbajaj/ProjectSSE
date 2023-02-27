// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Oasis.Agent.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CouldNotEstablishSessionException : Exception
    {
        public CouldNotEstablishSessionException()
        {
        }

        public CouldNotEstablishSessionException(string message) : base(message)
        {
        }

        public CouldNotEstablishSessionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CouldNotEstablishSessionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}