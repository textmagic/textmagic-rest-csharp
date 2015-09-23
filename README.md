# Textmagic:: Rest C# Wrapper

Welcome to your new textmagic-rest-csharp wrapper! This library provides you with an easy solution to send SMS and receive replies by integrating [TextMagic](www.textmagic.com) SMS Gateway to your C# / .NET application. 

In this directory, you'll find the files you need to be able to create the library binaries and to use them in your application. To experiment with that code, several examples are provided.

## What is TextMagic?

TextMagic's application programming interface (API) provides the communication link between your application and TextMagic’s SMS Gateway, allowing you to send and receive text messages and to check the delivery status of text messages you’ve already sent.

https://www.textmagic.com/docs/api/

All these commands can be executed only if you provide a valid username and API password in your requests.

## Installation

Add the library to your project references or install it as [NuGet package](https://www.nuget.org/packages/TextmagicRest/1.0.0/).

## Usage
 
1. Register on TextMagic: https://www.textmagic.com/ 

2. Download TextMagic C# Wrapper class library package and reference it in your project. 

In Visual Studio, you should go to Project - Add Reference window and locate dll by clicking Browse button. 

In SharpDevelop, go to Project - Add Reference - .NET Assembly Browser and choose dll by clicking Browse button. 

That's it! Now you can use TextMagic services as described below. 

3. Have a look at the [TextMagic SMS API Documentation](https://www.textmagic.com/docs/api/c-sharp/) and use the [sandbox](https://rest.textmagic.com/api/v2/doc) to experiment with the different API functions and get more technical details.

4. Write Code like: 

```C#
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
            } else 
            {
                Console.WriteLine("Message was not sent due to following exception: " + link.ClientException.Message);
            }
        }      
    }
}
``` 

Note: replace "replace-with-username", "replace-with-user-api-key", "replace-with-phoneNumber-WithInternationalPrefix" with the credentials you got during your registration on [TextMagic](https://www.textmagic.com/) and a valid phone number.

5. Have fun... :) 
6. More ready to use examples are included in the solution.

## Requirements

.NET 4.0 or higher

[RestSharp](https://www.nuget.org/packages/RestSharp/) (= 105.1.0)

[Json.NET](https://www.nuget.org/packages/Newtonsoft.Json/) (= 7.0.1)

Please note: in case you have a Newtonsoft.Json library version conflict preventing the library to compile, update this package with the NuGet Package Manager Console:

    $ Update-Package Newtonsoft.Json

## Contributing

Bug reports and pull requests are welcome on GitHub at https://github.com/textmagic/textmagic-rest-csharp.

## License

The library is available as open source under the terms of the [MIT License](http://opensource.org/licenses/MIT).