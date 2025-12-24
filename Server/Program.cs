using Server.Serveces;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HttpService.Start();
            Task.Delay(-1).Wait();
        }
    }
}
