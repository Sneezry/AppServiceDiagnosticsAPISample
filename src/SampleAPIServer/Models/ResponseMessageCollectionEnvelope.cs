using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPIServer.Models
{
    public class ResponseMessageCollectionEnvelope<T>
    {
        /// <summary>
        /// Default constructor of collection response message object.
        /// </summary>
        /// <param name="value">Collection of resources</param>
        /// <param name="nextLink">Link to next page of resources</param>
        [JsonConstructor]
        public ResponseMessageCollectionEnvelope(List<T> value, string nextLink = null, string collectionResourceId = null)
        {
            this.Value = value;
            this.NextLink = nextLink;
            this.Id = collectionResourceId;
        }

        /// <summary>
        /// Collection of resources.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<T> Value { get; set; }

        /// <summary>
        /// Link to next page of resources.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public string NextLink { get; set; }

        /// <summary>
        /// ID of parent resource.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public string Id { get; set; }
    }
}
