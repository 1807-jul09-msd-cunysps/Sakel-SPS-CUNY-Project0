using System;
using System.IO;
using System.Runtime.Serialization.Json;
using ContactLibrary;

namespace ContactClient
{
    class Program
    {
        static string FileName = "";
        static FileStream fStream = null;
        static ContactDirectory cDirectory = null;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Phone APP, Press 1 to create a file and continue with the application!");
            PrintIntroPrompt();
            Console.WriteLine();
            while (true)
            {
                PrintPrompt();
                Console.WriteLine();
            }
        }

        static void PrintIntroPrompt()
        {
            Console.WriteLine("1. Create file for contact directory");
            switch (Console.ReadLine())
            {
                case "1": // new file
                    Console.Write("Enter the name of the file: ");
                    FileName = Console.ReadLine();
                    fStream = File.Create(FileName);
                    cDirectory = new ContactDirectory();
                    fStream.Close(); 
                    break;
                default: // invalid arg
                    Console.WriteLine("Please try again.\n");
                    PrintIntroPrompt();
                    break;
            }
        }

        static void PrintPrompt()
        {
            int i, j;
            Console.WriteLine("1. Add person to the directory");
            Console.WriteLine("2. Read person from the directory");
            Console.WriteLine("3. Delete person from the directory");
            Console.WriteLine("4. Update person from the directory");
            Console.WriteLine("5. Search person from the directory");
            Console.WriteLine("6. Exit the application");
            Console.Write("Choose from the above choice: ");
            switch (Console.ReadLine())
            {
                case "1": // add
                    Person p = new Person();
                    askforid: Console.Write("Id: ");
                    p.Pid = Int64.Parse(Console.ReadLine());
                    foreach (Person per in cDirectory.People)
                    {
                        if (per.Pid == p.Pid)
                        {
                            Console.WriteLine("Please try again.There is already a person with the same ID");
                            goto askforid;
                        }
                    }
                    p.Address.Pid = p.Pid;
                    p.Phone.Pid = p.Pid;
                    Console.Write("Enter First Name: ");
                    p.FirstName = Console.ReadLine();
                    Console.Write("Enter Last Name: ");
                    p.LastName = Console.ReadLine();
                    Console.Write("Enter House Number: ");
                    p.Address.HouseNum = Console.ReadLine();
                    Console.Write("Enter Street Number: ");
                    p.Address.Street = Console.ReadLine();
                    Console.Write("Enter City Name: ");
                    p.Address.City = Console.ReadLine();
                    Console.Write("Enetr State Name: ");
                    p.Address.State = Console.ReadLine();
                    Console.Write("Enter Country Name: ");
                    p.Address.Country = Console.ReadLine();
                    Console.Write("Enter Zip Code: ");
                    p.Address.ZipCode = Console.ReadLine();
                    Console.Write("Enter Country Code: ");
                    p.Phone.CountryCode = Console.ReadLine();
                    Console.Write("Enetr Area Code: ");
                    p.Phone.AreaCode = Console.ReadLine();
                    Console.Write("Enter Phone Number: ");
                    p.Phone.Number = Console.ReadLine();                  
                    cDirectory.AddPerson(p);
                    SerializeToFile(cDirectory, FileName);
                    Console.WriteLine("Person Information successfully added to the application.");
                    break;
                case "2": // read
                    Console.Write("Enter the id: ");
                    try
                    {
                        Console.WriteLine(cDirectory.ReadPerson(Int32.Parse(Console.ReadLine())).ToString());    
                    }
                    catch (NullReferenceException e)
                    {
                        Console.WriteLine("The person does not exist with the ID provided.");
                    }
                    break;
                case "3": // delete
                    Console.Write("Enter the id: ");
                    cDirectory.DeletePerson(Int32.Parse(Console.ReadLine()));
                    SerializeToFile(cDirectory, FileName);
                    Console.WriteLine("Person information Successfully deleted from the directory.");
                    break;
                case "4": // update
                    Console.Write("Enter the id: ");
                    i = Int32.Parse(Console.ReadLine());
                    Console.WriteLine("Select the information need to be updated!");
                    Console.WriteLine("1. First Name");
                    Console.WriteLine("2. Last Name");
                    Console.WriteLine("3. House Number");
                    Console.WriteLine("4. Street");
                    Console.WriteLine("5. City");
                    Console.WriteLine("6. State");
                    Console.WriteLine("7. Country");
                    Console.WriteLine("8. Zip Code");
                    Console.WriteLine("9. Country Code");
                    Console.WriteLine("10. Area Code");
                    Console.WriteLine("11. Phone Number");
                    Console.Write("Choose from the above choice: ");
                    j = Int32.Parse(Console.ReadLine());
                    Console.Write("Enter the updated information: ");
                    cDirectory.UpdatePerson(i, j, Console.ReadLine());
                    SerializeToFile(cDirectory, FileName);
                    Console.WriteLine("Information successfully updated in the directory.");
                    break;
                case "5": // search
                    Console.WriteLine("How do you want to search the person?");
                    Console.WriteLine("1. First Name");
                    Console.WriteLine("2. Last Name"); 
                    Console.Write("Choose from the above choice: ");
                    i = Int32.Parse(Console.ReadLine());
                    Console.Write("Enter the information: ");
                    foreach (Person person in cDirectory.SearchPerson(i, Console.ReadLine()))
                    {
                        Console.WriteLine(person.ToString());
                    }
                    break;
                case "6": // exit app
                    fStream.Close();
                    Environment.Exit(0);
                    break;
                default: // invalid arg
                    Console.WriteLine("Invalid choice, please try again.\n");
                    PrintPrompt();
                    break;
            }
        }

        private static void SerializeToFile(ContactDirectory cd, string filename)
        {
            FileStream fs = File.Create(filename);
            DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(ContactDirectory));
            s.WriteObject(fs, cd);
            fs.Close();
        }

        private static ContactDirectory DeserializeFromFile(FileStream fs)
        {
            DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(ContactDirectory));
            return (ContactDirectory)s.ReadObject(fs);
        }
    }
}