using System;
using Klarna.Rest.Core.Commuication.Dto;
using Klarna.Rest.Core.Serialization;

namespace Klarna.Rest.Core.Commuication
{
    /// <summary>
    ///  A client store to keep API results
    /// </summary>
    [Obsolete("Use Communication namespace instead of Commuication")]
    public abstract class BaseStore : Communication.BaseStore
    {
        /// <summary>
        /// BaseStore Constructor
        /// </summary>
        /// <param name="apiSession">The API session instance used to communicate with Klarna APIs</param>
        /// <param name="apiControllerUri">The API endpoint to call</param>
        /// <param name="jsonSerializer">The JSON Serializer instance to use when sending / receiving data</param>
        /// <returns></returns>
        protected BaseStore(ApiSession apiSession, string apiControllerUri, IJsonSerializer jsonSerializer) :
            base(apiSession, apiControllerUri, jsonSerializer)
        { }
    }
}