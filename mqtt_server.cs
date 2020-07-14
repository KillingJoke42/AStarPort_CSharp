using System;
using MQTTnet;
using containers;
using MQTTnet.Client;
using System.Threading.Tasks;
using MQTTnet.Client.Options;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using astarsolver;

namespace mqtt_data
{
    public class mqtt_server
    {
        public static int[,] grid = {
                { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1 },
                { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1 },
                { 1, 1, 1, 0, 1, 1, 0, 1, 0, 1 },
                { 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
                { 1, 1, 1, 0, 1, 1, 1, 0, 1, 0 },
                { 1, 0, 1, 1, 1, 1, 0, 1, 0, 0 },
                { 1, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
                { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1 },
                { 1, 1, 1, 0, 0, 0, 1, 0, 0, 1 }
        };

        private static string host = "test.nhance.deviceinet.com";
        private static int port = 1883;
        private static string userid = "vaibhav";
        private static string password = "MGnSTVL!2a!Qp7uK";
        private static string subTopic = "nhance/intern/" + userid;
        public static IMqttClient Mqclient = new MqttFactory().CreateMqttClient();

        public static async Task connectToServer(IMqttClient client)
        {
            var option = new MqttClientOptionsBuilder()
                .WithClientId(userid)
                .WithTcpServer(host, port)
                .WithCredentials(userid, password)
                .WithCleanSession()
                .Build();
            await client.ConnectAsync(option, CancellationToken.None);
        }

        public static async Task subscribeToTopic(IMqttClient client)
        {
            await client.SubscribeAsync(new TopicFilterBuilder().WithTopic(subTopic).Build());
            Console.WriteLine("Subscribed to topic");
        }

        public static async Task publishJsonData(containers.source_dest obj)
        {
            string jsonStr = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var msg = new MqttApplicationMessageBuilder()
                          .WithTopic(subTopic)
                          .WithPayload(jsonStr)
                          .WithRetainFlag(true)
                          .Build();
            await Mqclient.PublishAsync(msg, CancellationToken.None);
        }

        public static async Task publishData(string data)
        {
            var msg = new MqttApplicationMessageBuilder()
                          .WithTopic(subTopic)
                          .WithPayload(data)
                          .WithRetainFlag(true)
                          .Build();
            await Mqclient.PublishAsync(msg, CancellationToken.None);
        }

        public static void HandleMsg(IMqttClient client)
        {
            client.UseApplicationMessageReceivedHandler(async e =>
            {
                try
                {
                    string topic = e.ApplicationMessage.Topic;
                    if (topic == subTopic)
                    {
                        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        Console.WriteLine("Incoming message received" + payload);
                        source_dest get_client_info = JsonConvert.DeserializeObject<source_dest>(payload);
                        pair src = JsonConvert.DeserializeObject<pair>(get_client_info.src);
                        pair dest = JsonConvert.DeserializeObject<pair>(get_client_info.dest);
                        astartsolver algo = new astartsolver("manhattan", 9, 10);
                        algo.aStarSearch(grid, src, dest);
                        await publishData(algo.path_desc);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            });
        }
    }
}
