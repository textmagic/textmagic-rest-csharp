using System;
using System.Collections.Generic;
using TextmagicRest;
using TextmagicRest.Model;

namespace textmagicsample
{
    class Program
    {
        public static void Main(string[] args)
        {
            sendMessage();

            Console.WriteLine("Check your phone ;-)");
            Console.WriteLine("Press any key to continue . . . ");
            Console.ReadKey(true);
        }

        public static void sendMessage()
        {
            var client = new Client("replace-with-username", "replace-with-user-api-key");
            var link = client.SendMessage("Hello from TextMagic API C# Wrapper demo application", "replace-with-phoneNumber-WithInternationalPrefix");

            if (link.Success)
            {
                Console.WriteLine("Message session {0} successfully sent", link.Id);
            }
            else
            {
                Console.WriteLine("Message was not sent due to following exception: " + link.ClientException.Message);
            }
        }
    }
}
