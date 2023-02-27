// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Utilties
{
    using System;

    public class Response
    {
        public bool Success { get; }

        public string Message { get; protected set; }

        public Exception Exception { get; protected set; }

        protected Response(bool success)
        {
            Success = success;
        }

        protected Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static Response Failed(string message)
        {
            return Failed(message, null);
        }

        public static Response Failed(string message, Exception exception)
        {
            return new Response(false) { Message = message, Exception = exception };
        }

        public static Response Succeeded()
        {
            return new Response(true);
        }

        public static Response Succeeded(string message)
        {
            return new Response(true, message);
        }
    }

    public class Response<T> : Response
    {
        public T Value { get; private set; }

        private Response(bool success, T value)
            : base(success)
        {
            Value = value;
        }

        private Response(bool success, T value, string message) : base(success, message)
        {
            Value = value;
        }

        public new static Response<T> Failed(string message)
        {
            return Failed(message, null);
        }

        public static Response<T> Failed(string message, T value)
        {
            return new Response<T>(false, value) { Message = message, Value = value };
        }

        public new static Response<T> Failed(string message, Exception exception)
        {
            return new Response<T>(false, default(T)) { Message = message, Exception = exception };
        }

        public static Response<T> Succeeded(T value)
        {
            return new Response<T>(true, value);
        }

        public static Response<T> Succeeded(T value, string message)
        {
            return new Response<T>(true, value, message);
        }
    }
}