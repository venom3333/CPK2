using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CPK.SharedModule.Entities
{
    [Serializable]
    public class ApiException : ApplicationException
    {
        public ApiExceptionCode Code { get; }

        public ApiException(ApiExceptionCode code) : this(code, new Dictionary<string, object>())
        {
        }

        public ApiException(ApiExceptionCode code, Dictionary<string, object> data) : this(code, data, null)
        {
        }

        public ApiException(ApiExceptionCode code, Dictionary<string, object> data, string message) : this(code, data, message, null)
        {
        }

        public ApiException(ApiExceptionCode code, Dictionary<string, object> data, string message, Exception inner) : base(message, inner)
        {
            Code = code;
            data ??= new Dictionary<string, object>();
            foreach (var kvp in data)
            {
                Data[kvp.Key] = kvp.Value;
            }
            Data[nameof(Code)] = code.ToString("G");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Code), Code);
            base.GetObjectData(info, context);
        }
    }
}
