using System;

using Slack.API.Services;
using SlackBot.Portable.Services;

namespace PlasticSlackBotCli
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Usage();
                return;
            }

            string token = args[0];

            PlasticCMDService plasticCmd = new PlasticCMDService();

            ChatInterpreter chatInterpreter = new ChatInterpreter(plasticCmd);

            SlackRTMService service = new SlackRTMService(chatInterpreter);

            service.SlackDataReceived += slackService_SlackDataReceived;

            service.ConnectPlasticSlackBot(token);

            Console.WriteLine("Type ENTER to exit");

            Console.ReadLine();
        }

        static void slackService_SlackDataReceived(object sender, SlackBot.Portable.Model.SlackEventArgs e)
        {
            Console.WriteLine(e.Data.Text);
        }

        static void Usage()
        {
            Console.WriteLine("PlasticSlackBotCli is a Slack bot to run Plastic commands hosted on a CLI app");
            Console.WriteLine("Usage: PlasticSlackBotCli slacktoken");
        }
    }
}
