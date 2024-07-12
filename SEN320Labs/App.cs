using Microsoft.Extensions.Logging;
using SaltedHashLibrary.BusinessLogic;
using EncryptDecryptLibrary.BusinessLogic;
using System;

namespace SEN320Labs
{
    public class App
    {
        private readonly IHashService _hashService;
        private readonly IXORCipher _xORService;
        private readonly ILogger<App> _logger;

        public App(IHashService hashService, IXORCipher xORService, ILogger<App> logger)
        {
            _logger = logger;
            _hashService = hashService;
            _xORService = xORService;

            _logger.LogInformation("App created successfully.");
        }
        
        public void Run()
        {
            PrintLineSeperator();
            Console.WriteLine("Welcome to Connors SEN320 overkill Lab Projects and Libraries!");
            int userInput = GetValidIntegerInputFromUser("Enter a number: [0-1]\n\n0: Quit Console Application\n1: SaltYourCredentialHashL1\n2: XORCipher", 0, 2);

            switch (userInput)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    SaltYourCredentialHashLab1();
                    break;
                case 2:
                    XOREncryptionExcercise();
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    break;
            }
        }

        private void PrintLineSeperator()
        {
            Console.WriteLine("---------------------------------------------------------------------\n");
        }

        private int GetValidIntegerInputFromUser(string prompt, int min, int max)
        {
            int validInteger;
            string consoleInput;

            while(true)
            {
                Console.WriteLine(prompt);
                consoleInput = Console.ReadKey().KeyChar.ToString();

                if (!string.IsNullOrEmpty(consoleInput) && int.TryParse(consoleInput, out validInteger) && 
                    validInteger <= max   && validInteger >= min)
                {
                    Console.WriteLine("\n");
                    return validInteger;
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Please enter a valid integer.");
                }
            }
        }

        private string GetValidStringInputFromUser(string prompt)
        {
            string consoleInput;

            while(true)
            {
                Console.WriteLine(prompt);
                consoleInput = Console.ReadLine();

                if (!string.IsNullOrEmpty(consoleInput))
                {
                    return consoleInput;
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Please enter a valid string.");
                }
            }
        }

        private void SaltYourCredentialHashLab1()
        {
            // get password from user
            PrintLineSeperator();
            Console.WriteLine("Please enter a password to hash.");
            string password = GetValidStringInputFromUser("Password: ");

            // create SHA256SaltedHash object
            var saltedHash = _hashService.SHA256SaltedHash(password);
            Console.WriteLine($"\nHash: {saltedHash.GetHash()}, Salt: {saltedHash.GetSalt()}");

            // store the hash and salt in a relational database
        }

        private void XOREncryptionExcercise()
        {
            int userInput = GetValidIntegerInputFromUser("Enter a number: [0-2]\n\n0: Exit\n1: Encrypt Plain Text\n2: Decrypt Cipher Text", 0, 2);

            switch(userInput)
            {
                case 0:
                    break;
                case 1:
                    EncryptPlainText();
                    break;
                case 2:
                    DecryptCipherText();
                    break;
            }
            Run();
        }

        private void DecryptCipherText()
        {
            PrintLineSeperator();
            Console.WriteLine("Decrypt Cipher Text. Please enter your cipher text.");
            string secretText = GetValidStringInputFromUser("Cipher Text");
            Console.WriteLine("Enter your secret key.");
            string key = GetValidStringInputFromUser("Key: ");

            Console.WriteLine(_xORService.Decrypt(secretText, key));
        }

        private void EncryptPlainText()
        {
            PrintLineSeperator();
            Console.WriteLine("Encrypt Plain Text. Please enter your plain text.");
            string plainText = GetValidStringInputFromUser("Plain Text: ");
            Console.WriteLine("Enter your secret key.");
            string key = GetValidStringInputFromUser("Key: ");

            Console.WriteLine(_xORService.Encrypt(plainText, key));
        }
    }
}
