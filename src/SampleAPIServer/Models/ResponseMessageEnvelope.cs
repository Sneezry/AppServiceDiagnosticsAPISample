using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPIServer.Models
{
    /// <summary>
    /// Message envelope that contains the common Azure resource manager properties and the resource provider specific content.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseMessageEnvelope<T>
    {
        /// <summary>
        /// Constructor for every basic envelope
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="location"></param>
        /// <param name="properties"></param>
        public ResponseMessageEnvelope(string id, string name, string type, string location, T properties)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
            this.Location = location;
            this.Properties = properties;
        }
        
        /// <summary>
        /// Resource Id. Typically ID is populated only for responses to GET requests. Caller is responsible for passing in this
        /// value for GET requests only.
        /// For example: /subscriptions/{subscriptionId}/resourceGroups/{resourceGroupId}/providers/Microsoft.Web/sites/{sitename}
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of resource.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of resource e.g "Microsoft.Web/sites".
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        
        /// <summary>
        /// Geographical region resource belongs to e.g. SouthCentralUS, SouthEastAsia.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Location { get; set; }
        
        /// <summary>
        /// Resource specific properties.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Properties { get; set; }
    }
}
