// Console.WriteLine("Hello, World!");

Encrypt();
Decrypt();


static void Encrypt() {
    Console.WriteLine("1, Encrypy Plain Text = Please enter your plaintext/cleartext");
    var plaintext = Console.ReadLine().Trim();

    Console.WriteLine("Enter Your Secret Key: ");
    var secretKey = Console.ReadLine().Trim();

    string encryptedtext = Convert.ToBase64CharArray(EncryptDecrypt(plaintext, secretkey));
    Console.WriteLine($"The Encrypted Message Is: {encryptedtext}");
}

static void Decrypt() {
    Console.WriteLine("2. Decrtpy Cipher Text - Please enter your ciphertext/encrypted text");
    var ciphertext = Console.ReadLine().Trim();
    ciphertext = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ciphertext));
    
    Console.WriteLine("Enter your secret ket:");
    var secretkey = Console.ReadLine().Trim();

    Console.WriteLine($"The Decrypted Message Is: {System.Text.Encoding.UTF8.GetString(EncryptDecrypt(ciphertext, secretkey))}");

}

static byte[] EncryptDecrypt(string text, string key) {
    var textArray = text.ToCharArray();
    var keyArray = new char[text.Length];
    int k = 0;

    for(int i = 0; i < text.Length; i++)
    {
        if(k >= key.Length; i++)
        {
            k = 0;
        }

        keyArray[i] = key.ToCharArray()[k];
        k++;
    }

    var result = new byte[text.Length];

    for(int i = 0; i < result.Length; i++)
    {
        result[i] = (byte)(textArray[i] ^ keyArray[i]);
    }

    return result;
}








// using System;
// using System.Text;
// using System.Security.Cryptography;


// // class Program
// // {
// //     static void Main(string[] args)
// //     {
//     Console.WriteLine("---Hashing and Message Digests Module---");

//     Console.WriteLine(args.Length + " arguments were passed.");

//     for (int i = 0; i < args.Length; i++)
//     {
//         Console.WriteLine("Argument " + i + ": " + args[i]);
//     }

//     // MD5
//     md5Hash();

//     // SHA1
//     sha1Hash();

//     // SHA256
//     sha256Hash();
//     // }

//     static string ComputeSha1Hash(string input)
//     {
//         using (SHA1 sha1Hash = SHA1.Create())
//         {
//             byte[] sourceBytes = Encoding.UTF8.GetBytes(input);
//             byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
//             string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
    
//             return hash;
//         }
//     }

//     static string ComputeSha256Hash(string input)
//     {
//         // Create a SHA256  
//         using (SHA256 sha256Hash = SHA256.Create())
//         {
//             byte[] sourceBytes = Encoding.UTF8.GetBytes(input);
//             byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
//             string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
    
//             return hash;
//         }
//     }

//     static string ComputeMd5Hash(string cleartext)
//     {
//         using (var md5Hash = MD5.Create())
//         {
//             var sourceBytes = Encoding.UTF8.GetBytes(cleartext);
//             var hashBytes = md5Hash.ComputeHash(sourceBytes);

//             // convert the has byes to a nice string
//             var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

//             return hash;
//         }
//     }

//     static void md5Hash() {
//         Console.WriteLine("--MD5 Hashing--");
//         string? cleartext = null;

//         while(string.IsNullOrEmpty(cleartext)) {
//             Console.WriteLine("Please input your clear text: ");
//             cleartext = Console.ReadLine();
//         }

//         string hash = ComputeMd5Hash(cleartext);

//         Console.WriteLine("The hash of " + cleartext + " is: " + hash + "\n");
//     }


//     static void sha1Hash() {
//         Console.WriteLine("--SHA1 Hashing--");
//         string? cleartext = null;

//         while(string.IsNullOrEmpty(cleartext)) {
//             Console.WriteLine("Please input your clear text: ");
//             cleartext = Console.ReadLine();
//         }

//         string hash = ComputeSha1Hash(cleartext);

//         Console.WriteLine("The hash of " + cleartext + " is: " + hash + "\n");
//     }

//     static void sha256Hash() {
//         Console.WriteLine("--SHA256 Hashing--");
//         string? cleartext = null;

//         while(string.IsNullOrEmpty(cleartext)) {
//             Console.WriteLine("Please input your clear text: ");
//             cleartext = Console.ReadLine();
//         }

//         string hash = ComputeSha256Hash(cleartext);

//         Console.WriteLine("The hash of " + cleartext + " is: " + hash + "\n");
//     }
// // }




// // MD5

// // Console.WriteLine("Please input your clear text: ");

// // String? cleartext = Console.ReadLine();
// // Console.WriteLine(cleartext);


// // using (var md5Hash = MD5.Create())
// // {
// //     var sourceBytes = Encoding.UTF8.GetBytes(cleartext);
// //     var hashBytes = md5Hash.ComputeHash(sourceBytes);

// //     // convert the has byes to a nice string
// //     var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

// //     Console.WriteLine("The hash of " + cleartext + " is: " + hash);
// // }


 
// // string plainText = "Hello World!";
// // Console.WriteLine("Plain text: {0}", plainText);
 
// // string sha1 = ComputeSha1Hash(plainText);
// // Console.WriteLine("SHA1 Hashed Data {0}", sha1);
 
// // string sha256 = ComputeSha256Hash(plainText);
// // Console.WriteLine("SHA256 Hashed Data {0}", sha256);

// //Console.WriteLine("Please enter your data: ");
// //String? abc = Console.ReadLine();
// //Console.WriteLine(abc);
 
// //string name = Environment.GetCommandLineArgs()[1];
// //Console.WriteLine($"Hello, {name}!");



 


// // var dictionary = new string[] {"Fish","Taco","Airplane","Dictionary"};

// // string correctHash = "3030e8ec7633ec1a524bb246aee7dbda6fb3e4bc";

// // //loop through dictionary
// // foreach (string item in dictionary) {
// //     string computed = ComputeSha1Hash(item).ToLower();
// //     // Console.WriteLine(computed);
// //     if (computed.Equals(correctHash)){
// //         Console.WriteLine($"{item}:, {computed} = {correctHash}");
// //     }
// // }