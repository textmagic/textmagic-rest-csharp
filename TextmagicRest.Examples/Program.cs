using System;

namespace TextmagicRest.Examples
{
    public class Program
    {
        public static void Main()
        {
            SendMessage();

            Console.WriteLine("Check your phone ;-)");
            Console.WriteLine("Press any key to continue . . . ");
            Console.ReadKey(true);
        }

        public static void SendMessage()
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
