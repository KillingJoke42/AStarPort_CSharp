using containers;
using mqtt_data;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace a_star_algo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            mqtt_server.connectToServer(mqtt_server.Mqclient).Wait();
            mqtt_server.subscribeToTopic(mqtt_server.Mqclient).Wait();
            string str_src = JsonConvert.SerializeObject(new pair(0,0), Formatting.Indented);
            string str_dest = JsonConvert.SerializeObject(new pair(8,0), Formatting.Indented);
            await mqtt_server.publishJsonData(new source_dest(str_src, str_dest));
            mqtt_server.HandleMsg(mqtt_server.Mqclient);
            Console.WriteLine("Press any key to exit:");
            Console.ReadKey();
        }
    }
}
