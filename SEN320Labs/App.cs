using Microsoft.Extensions.Logging;
using EncryptDecryptLibrary.BusinessLogic;
using System;
using DynamodbLibrary.BusinessLogic;
using SEN320Labs.Models;
using System.Diagnostics.Metrics;

namespace SEN320Labs
{
    public class App
    {
        private readonly IUserService _userService;
        private readonly IXORCipher _xORService;
        private readonly ILogger<App> _logger;

        public App(IUserService userService, IXORCipher xORService, ILogger<App> logger)
        {
            _logger = logger;
            _userService = userService;
            _xORService = xORService;

            _logger.LogInformation("App created successfully.");
        }
        
        public void Run()
        {
            bool applicationRunning = true;

            while(applicationRunning)
            {
                PrintLineSeperator();
                Console.WriteLine("Welcome to Connors SEN320 overkill Lab Projects and Libraries!");
                int userInput = GetValidIntegerInputFromUser("Enter a number: [0-2]\n\n0: Quit Console Application\n1: SaltYourCredentialHashL1\n2: XORCipher", 0, 2);
                switch (userInput)
                {
                    case 0:
                        applicationRunning = false;
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
            Environment.Exit(0);
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

        private string GetValidPasswordInputFromUser(string prompt)
        {
            string consoleInput;

            while (true)
            {
                Console.WriteLine(prompt);
                consoleInput = Console.ReadLine();

                if (!string.IsNullOrEmpty(consoleInput) && ValidPassword(consoleInput))
                {
                    return consoleInput;
                } else
                {
                    Console.WriteLine("\nPassword must be at least 8 characters long and contain at least 1 specials, 1 numbers, and 1 upper case.");
                }
            }
        }

        private bool ValidPassword(string consoleInput)
        {
            int specialCount = 0;
            int numberCount = 0;
            int upperCaseCount = 0;

            foreach (char c in consoleInput)
            {
                if (char.IsUpper(c))
                {
                    upperCaseCount++;
                }
                else if (char.IsDigit(c))
                {
                    numberCount++;
                }
                else if (char.IsPunctuation(c))
                {
                    specialCount++;
                }
            }
            if (specialCount >= 1 && numberCount >= 1 && upperCaseCount >= 1 && consoleInput.Length >= 8)
            {
                return true;
            } else {
                return false;
            }
        }

        private void SaltYourCredentialHashLab1()
        {
            bool saltyourCredentialHashLabRunning = true;
            
            PrintLineSeperator();
            Console.WriteLine("SaltYourCredentialHashLab1. Please select an option.");
            while (saltyourCredentialHashLabRunning)
            {
                int userInput = GetValidIntegerInputFromUser("Enter a number: [0-4]\n\n0: Exit\n1: Create User\n2: Validate User\n3: Update User\n4: Delete User", 0, 4);
                switch (userInput)
                {
                    case 0:
                        saltyourCredentialHashLabRunning = false;
                        break;
                    case 1:
                        ChooseActionThenGetUsernameAndPassword(1);
                        break;
                    case 2:
                        ChooseActionThenGetUsernameAndPassword(2);
                        break;
                    case 3:
                        ChooseActionThenGetUsernameAndPassword(3);
                        break;
                    case 4:
                        ChooseActionThenGetUsernameAndPassword(4);
                        break;
                }
            }
        }

        private void ChooseActionThenGetUsernameAndPassword(int option)
        {
            string username = GetValidStringInputFromUser("Enter your username: ");
            string password = "";
            if (option == 1)
            {
                Console.WriteLine("Password must be at least 8 characters long and contain at least 1 specials, 1 numbers, and 1 upper case.");
                password = GetValidPasswordInputFromUser("Enter your password: ");
            } else
            {
                password = GetValidStringInputFromUser("Enter your password: ");
            }
            
            
            switch(option)
            {
                case 1:
                    _userService.CreateAccountAsync(username, password).Wait();
                    break;
                case 2:
                    _userService.ValidateUserCredentialsAsync(username, password).Wait();
                    break;
                case 3:
                    _userService.UpdatePasswordAsync(username, password, GetValidPasswordInputFromUser("Enter your new password: ")).Wait();
                    break;
                case 4:
                    _userService.DeleteUserAsync(username, password).Wait();
                    break;
            }
            PrintLineSeperator();
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
