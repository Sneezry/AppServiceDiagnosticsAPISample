using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleAPIServer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SampleAPIServer.Helpers
{
    public class CSMHelper
    {
        public async Task<ResponseMessageCollectionEnvelope<ResponseMessageEnvelope<JToken>>> CreateCollectionEnvelopedResponse(HttpResponseMessage responseMessage, string resourceUri, string location, string resourceType, Func<JToken, string> nameFunction)
        {
            return await CreateCollectionEnvelopedResponseInternal(responseMessage, resourceUri, resourceType, location, nameFunction);
        }

        public async Task<ResponseMessageEnvelope<JToken>> CreateEnvelopedResponse(HttpResponseMessage responseMessage, string resourceUri, string location, string resourceType, string resourceName)
        {
            return await CreateEnvelopedResponseInternal(responseMessage, resourceUri, resourceType, location, resourceName);
        }

        public string GetNameFromJToken(JToken token)
        {
            var metadata = token["metadata"] ?? token["Metadata"];
            if (metadata != null)
            {
                return metadata.Value<string>("id") ?? metadata.Value<string>("Id");
            }

            return null;
        }

        private async Task<ResponseMessageCollectionEnvelope<ResponseMessageEnvelope<JToken>>> CreateCollectionEnvelopedResponseInternal(HttpResponseMessage responseMessage, string id, string type, string location, Func<JToken, string> nameFunction)
        {
            JArray content = default(JArray);
            Stream responseStream = await responseMessage.Content.ReadAsStreamAsync();

            if (responseStream != null && responseMessage.IsSuccessStatusCode)
            {
                var stream = new StreamReader(responseStream);
                var jsonTextReader = new JsonTextReader(stream);
                content = JToken.ReadFrom(jsonTextReader) as JArray;
            }

            var childEnvelopes = content.Select(arrayMember =>
            {
                var name = nameFunction(arrayMember) ?? string.Empty;
                return new ResponseMessageEnvelope<JToken>(id: id + "/" + name, name: name, type: type, location: location, properties: arrayMember);
            }).ToList();

            var collectionEnvelope = new ResponseMessageCollectionEnvelope<ResponseMessageEnvelope<JToken>>(childEnvelopes, null, id);
            return collectionEnvelope;
        }

        private async Task<ResponseMessageEnvelope<JToken>> CreateEnvelopedResponseInternal(HttpResponseMessage responseMessage, string id, string type, string location, string resourceName)
        {
            JToken content = default(JToken);
            Stream responseStream = await responseMessage.Content.ReadAsStreamAsync();

            if (responseStream != null)
            {
                var stream = new StreamReader(responseStream);
                var jsonTextReader = new JsonTextReader(stream);
                content = JToken.ReadFrom(jsonTextReader);
            }

            var envelope = new ResponseMessageEnvelope<JToken>(id: id, name: resourceName, type: type, location: location, properties: content);
            return envelope;
        }

        private HttpResponseMessage CreateHttpResponseInternal<T>(T responseContent, HttpStatusCode statusCode, HttpResponseHeaders headers = null) where T : class
        {
            var response = new HttpResponseMessage(statusCode);
            if (responseContent != null)
            {
                response.Content = new ObjectContent(responseContent.GetType(), responseContent, new JsonMediaTypeFormatter());
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            foreach (var header in headers)
            {
                response.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return response;
        }
    }
}
