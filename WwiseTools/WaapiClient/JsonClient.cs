/******************************************************************************

The content of this file includes portions of the AUDIOKINETIC Wwise Technology
released in source code form as part of the SDK installer package.

Commercial License Usage

Licensees holding valid commercial licenses to the AUDIOKINETIC Wwise Technology
may use this file in accordance with the end user license agreement provided 
with the software or, alternatively, in accordance with the terms contained in a
written agreement between you and Audiokinetic Inc.

Apache License Usage

Alternatively, this file may be used under the Apache License, Version 2.0 (the 
"Apache License"); you may not use this file except in compliance with the 
Apache License. You may obtain a copy of the Apache License at 
http://www.apache.org/licenses/LICENSE-2.0.

Unless required by applicable law or agreed to in writing, software distributed
under the Apache License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES
OR CONDITIONS OF ANY KIND, either express or implied. See the Apache License for
the specific language governing permissions and limitations under the License.

  Copyright (c) 2022 Audiokinetic Inc.

*******************************************************************************/

namespace AK.Wwise.Waapi
{
    /// <summary>
    /// The JsonClient provides an abstraction layer over the base Waapi Client and wraps everything under Newtonsoft.Json.Linq.JObject for convenience.
    /// </summary>
    public class JsonClient
    {
        private AK.Wwise.Waapi.Client client = new AK.Wwise.Waapi.Client();

        public delegate void PublishHandler(Newtonsoft.Json.Linq.JObject json);

        public event Wamp.DisconnectedHandler Disconnected;

        public JsonClient()
        {
            client.Disconnected += Client_Disconnected;
        }

        private void Client_Disconnected()
        {
            if (Disconnected != null)
            {
                Disconnected();
            }
        }

        /// <summary>Connect to a running instance of Wwise Authoring.</summary>
        /// <param name="uri">URI to connect. Usually the WebSocket protocol (ws:) followed by the hostname and port, followed by waapi.</param>
        /// <example>Connect("ws://localhost:8080/waapi")</example>
        /// <param name="timeout">The maximum timeout in milliseconds for the function to execute. Will raise Waapi.TimeoutException when timeout is reached.</param>
        public async System.Threading.Tasks.Task Connect(
            string uri = "ws://localhost:8080/waapi", 
            int timeout = System.Int32.MaxValue)
        {
            await client.Connect(uri, timeout);
        }

        /// <summary>Close the connection.</summary>
        /// <param name="timeout">The maximum timeout in milliseconds for the function to execute. Will raise Waapi.TimeoutException when timeout is reached.</param>
        public async System.Threading.Tasks.Task Close()
        {
            await client.Close();
        }

        /// <summary>
        /// Return true if the client is connected and ready for operations.
        /// </summary>
        public bool IsConnected()
        {
            return client.IsConnected();
        }

        /// <summary>
        /// Call a WAAPI remote procedure. Refer to WAAPI reference documentation for a list of URIs and their arguments and options.
        /// </summary>
        /// <param name="uri">The URI of the remote procedure.</param>
        /// <param name="args">The arguments of the remote procedure. C# anonymous objects will be automatically serialized to Json.</param>
        /// <param name="options">The options the remote procedure. C# anonymous objects will be automatically serialized to Json.</param>
        /// <param name="timeout">The maximum timeout in milliseconds for the function to execute. Will raise Waapi.TimeoutException when timeout is reached.</param>
        /// <returns>A Newtonsoft.Json.Linq.JObject with the result of the Remote Procedure Call.</returns>
        public async System.Threading.Tasks.Task<Newtonsoft.Json.Linq.JObject> Call(
            string uri, object args = null, 
            object options = null, 
            int timeout = System.Int32.MaxValue)
        {
            if (args == null)
                args = new { };
            if (options == null)
                options = new { };

            return await Call(uri, Newtonsoft.Json.Linq.JObject.FromObject(args), Newtonsoft.Json.Linq.JObject.FromObject(options), timeout);
        }

        /// <summary>
        /// Call a WAAPI remote procedure. Refer to WAAPI reference documentation for a list of URIs and their arguments and options.
        /// </summary>
        /// <param name="uri">The URI of the remote procedure.</param>
        /// <param name="args">The arguments of the remote procedure as a Newtonsoft.Json.Linq.JObject</param>
        /// <param name="options">The options the remote procedure as a Newtonsoft.Json.Linq.JObject.</param>
        /// <param name="timeout">The maximum timeout in milliseconds for the function to execute. Will raise Waapi.TimeoutException when timeout is reached.</param>
        /// <returns>A Newtonsoft.Json.Linq.JObject with the result of the Remote Procedure Call.</returns>
        public async System.Threading.Tasks.Task<Newtonsoft.Json.Linq.JObject> Call(
            string uri, Newtonsoft.Json.Linq.JObject args, 
            Newtonsoft.Json.Linq.JObject options, 
            int timeout = System.Int32.MaxValue)
        {
            if (args == null)
                args = new Newtonsoft.Json.Linq.JObject();
            if (options == null)
                options = new Newtonsoft.Json.Linq.JObject();

            string result = await client.Call(uri, args.ToString(), options.ToString(), timeout);

            return Newtonsoft.Json.Linq.JObject.Parse(result);
        }

        /// <summary>
        /// Subscribe to a topic. Refer to WAAPI reference documentation to obtain the list of topics available.
        /// </summary>
        /// <param name="topic">Topic to subscribe</param>
        /// <param name="options">Option for the subscrition.</param>
        /// <param name="publishHandler">Delegate that will be executed when the topic is pusblished.</param>
        /// <param name="timeout">The maximum timeout in milliseconds for the function to execute. Will raise Waapi.TimeoutException when timeout is reached.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<int> Subscribe(
            string topic, object options, 
            PublishHandler publishHandler, 
            int timeout = System.Int32.MaxValue)
        {
            if (options == null)
                options = new { };

            return await Subscribe(topic, Newtonsoft.Json.Linq.JObject.FromObject(options), publishHandler, timeout);
        }

        /// <summary>
        /// Subscribe to a topic. Refer to WAAPI reference documentation to obtain the list of topics available.
        /// </summary>
        /// <param name="topic">Topic to subscribe</param>
        /// <param name="options">Option for the subscrition.</param>
        /// <param name="publishHandler">Delegate that will be executed when the topic is pusblished.</param>
        /// <param name="timeout">The maximum timeout in milliseconds for the function to execute. Will raise Waapi.TimeoutException when timeout is reached.</param>
        /// <returns>The subscription id assigned to the subscription. Store the id to call Unsubscribe.</returns>
        public async System.Threading.Tasks.Task<int> Subscribe(
            string topic, 
            Newtonsoft.Json.Linq.JObject options, 
            PublishHandler publishHandler, 
            int timeout = System.Int32.MaxValue)
        {
            if (options == null)
                options = new Newtonsoft.Json.Linq.JObject();

            return await client.Subscribe(
                topic,
                options.ToString(),
                (string json) =>
                {
                    publishHandler(Newtonsoft.Json.Linq.JObject.Parse(json));
                }, 
                timeout);
        }

        /// <summary>
        /// Unsubscribe from a subscription.
        /// </summary>
        /// <param name="subscriptionId">The subscription id received from the initial subscription.</param>
        /// <param name="timeout">The maximum timeout in milliseconds for the function to execute. Will raise Waapi.TimeoutException when timeout is reached.</param>
        public async System.Threading.Tasks.Task Unsubscribe(
            int subscriptionId, 
            int timeout = System.Int32.MaxValue)
        {
            await client.Unsubscribe(subscriptionId, timeout);
        }
    }
}
