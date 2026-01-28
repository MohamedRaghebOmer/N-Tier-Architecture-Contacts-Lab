using System;
using System.Data;
using System.Globalization;
using Contacts.Business;

namespace ContactsConsoleApp
{
    internal class Program
    {
        // Using static colors to unify the program's design
        static void Main(string[] args)
        {
            // Set window title and header
            Console.Title = "Contacts Management System (Console Edition)";

            while (true)
            {
                Console.Clear();
                DrawMainHeader();
                ShowMainMenu();
            }
        }

        static void DrawMainHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==============================================");
            Console.WriteLine("         CONTACTS MANAGEMENT SYSTEM");
            Console.WriteLine("==============================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Show All Contacts");
            Console.WriteLine("2. Add New Contact");
            Console.WriteLine("3. Find Contact");
            Console.WriteLine("4. Edit Contact");
            Console.WriteLine("5. Delete Contact");
            Console.WriteLine("6. Exit");
            Console.WriteLine("----------------------------------------------");
            Console.Write("Enter your choice (1-6): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListAllContacts();
                    break;
                case "2":
                    AddNewContact();
                    break;
                case "3":
                    FindContactScreen();
                    break;
                case "4":
                    UpdateContactScreen();
                    break;
                case "5":
                    DeleteContactScreen();
                    break;
                case "6":
                    Environment.Exit(0);
                    break;
                default:
                    PrintMessage("Invalid choice. Please try again.", ConsoleColor.Red);
                    break;
            }
        }

        // ---------------------- Features Implementation ----------------------

        static void ListAllContacts()
        {
            Console.Clear();
            DrawMainHeader();
            Console.WriteLine("--- List of All Contacts ---");
            Console.WriteLine();

            DataTable dt = clsContact.GetAllContacts();

            if (dt == null || dt.Rows.Count == 0)
            {
                PrintMessage("No contacts found in the system.", ConsoleColor.Yellow);
                return;
            }

            // Print table header with formatted spacing
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("{0,-5} | {1,-15} | {2,-15} | {3,-20} | {4,-15} | {5,-15}",
                "ID", "First Name", "Last Name", "Email", "Phone", "Country ID");
            Console.WriteLine(new string('-', 100));
            Console.ResetColor();

            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine("{0,-5} | {1,-15} | {2,-15} | {3,-20} | {4,-15} | {5,-15}",
                    row["ContactID"],
                    row["FirstName"],
                    row["LastName"],
                    row["Email"],
                    row["Phone"],
                    row["CountryID"]);
            }

            Console.WriteLine();
            PrintMessage("End of list. Press any key to go back...", ConsoleColor.White, false);
            Console.ReadKey();
        }

        static void AddNewContact()
        {
            Console.Clear();
            DrawMainHeader();
            Console.WriteLine("--- Add New Contact ---");

            clsContact newContact = new clsContact();

            // Read data from user
            newContact.FirstName = ReadInput("First Name: ");
            newContact.LastName = ReadInput("Last Name: ");
            newContact.Email = ReadInput("Email: ");
            newContact.Phone = ReadInput("Phone: ");
            newContact.Address = ReadInput("Address: ");

            // Read and validate Date of Birth
            newContact.DateOfBirth = ReadDateTime("Date of Birth (yyyy-MM-dd): ");

            // Select country (Simulating a ComboBox)
            newContact.CountryID = SelectCountry();

            // Optional Image Path
            Console.Write("Image Path (Optional, Press Enter to skip): ");
            string imgPath = Console.ReadLine();
            newContact.ImagePath = string.IsNullOrWhiteSpace(imgPath) ? "" : imgPath;

            Console.WriteLine();
            Console.WriteLine("Saving...");

            if (newContact.Save())
            {
                PrintMessage($"Success! Contact Added with ID: {newContact.ID}", ConsoleColor.Green);
            }
            else
            {
                PrintMessage("Error: Failed to add contact.", ConsoleColor.Red);
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        static void FindContactScreen()
        {
            Console.Clear();
            DrawMainHeader();
            Console.WriteLine("--- Find Contact ---");

            int id = ReadInt("Enter Contact ID to find: ");

            clsContact contact = clsContact.Find(id);

            if (contact != null)
            {
                PrintContactCard(contact);
            }
            else
            {
                PrintMessage($"Contact with ID {id} not found.", ConsoleColor.Red);
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        static void UpdateContactScreen()
        {
            Console.Clear();
            DrawMainHeader();
            Console.WriteLine("--- Edit Contact ---");

            int id = ReadInt("Enter Contact ID to update: ");

            clsContact contact = clsContact.Find(id);

            if (contact == null)
            {
                PrintMessage($"Contact with ID {id} not found.", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            PrintContactCard(contact);
            Console.WriteLine("Enter new values (or press Enter to keep current value):");
            Console.WriteLine("-------------------------------------------------------");

            contact.FirstName = ReadInputWithDefault("First Name", contact.FirstName);
            contact.LastName = ReadInputWithDefault("Last Name", contact.LastName);
            contact.Email = ReadInputWithDefault("Email", contact.Email);
            contact.Phone = ReadInputWithDefault("Phone", contact.Phone);
            contact.Address = ReadInputWithDefault("Address", contact.Address);

            // Update Date of Birth
            Console.Write($"Date of Birth [{contact.DateOfBirth:yyyy-MM-dd}]: ");
            string dobInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dobInput))
            {
                if (DateTime.TryParse(dobInput, out DateTime newDate))
                    contact.DateOfBirth = newDate;
                else
                    Console.WriteLine("Invalid date format, keeping old date.");
            }

            // Update Country
            Console.WriteLine($"Current Country ID: {contact.CountryID}");
            Console.Write("Do you want to change the country? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                contact.CountryID = SelectCountry();
            }

            // Update Image Path
            contact.ImagePath = ReadInputWithDefault("Image Path", contact.ImagePath);

            if (contact.Save())
            {
                PrintMessage("Contact updated successfully!", ConsoleColor.Green);
            }
            else
            {
                PrintMessage("Error: Failed to update contact.", ConsoleColor.Red);
            }

            Console.ReadKey();
        }

        static void DeleteContactScreen()
        {
            Console.Clear();
            DrawMainHeader();
            Console.WriteLine("--- Delete Contact ---");

            int id = ReadInt("Enter Contact ID to delete: ");

            if (!clsContact.IsContactExist(id))
            {
                PrintMessage($"Contact with ID {id} does not exist.", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            // Display data for confirmation before deletion
            clsContact contactToDelete = clsContact.Find(id);
            Console.WriteLine($"You are about to delete: {contactToDelete.FirstName} {contactToDelete.LastName}");

            Console.Write("Are you sure? (y/n): ");
            string confirmation = Console.ReadLine();

            if (confirmation.ToLower() == "y")
            {
                if (clsContact.DeleteContact(id))
                {
                    PrintMessage("Contact deleted successfully.", ConsoleColor.Green);
                }
                else
                {
                    PrintMessage("Error: Could not delete contact.", ConsoleColor.Red);
                }
            }
            else
            {
                PrintMessage("Deletion cancelled.", ConsoleColor.Yellow);
            }

            Console.ReadKey();
        }

        // ---------------------- Helper Methods ----------------------

        static string ReadInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        static string ReadInputWithDefault(string prompt, string currentValue)
        {
            Console.Write($"{prompt} [{currentValue}]: ");
            string input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? currentValue : input;
        }

        static int ReadInt(string prompt)
        {
            int number;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out number))
                {
                    return number;
                }
                Console.WriteLine("Invalid number, please try again.");
            }
        }

        static DateTime ReadDateTime(string prompt)
        {
            DateTime dt;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                // Supports multiple date formats
                if (DateTime.TryParse(input, out dt))
                {
                    return dt;
                }
                Console.WriteLine("Invalid date. Please use format yyyy-MM-dd (e.g. 2000-01-30).");
            }
        }

        static int SelectCountry()
        {
            Console.WriteLine("\n--- Select Country ---");
            DataTable dtCountries = clsCountry.GetAllCountries();

            if (dtCountries != null)
            {
                foreach (DataRow row in dtCountries.Rows)
                {
                    Console.WriteLine($"ID: {row["CountryID"],-3} | Name: {row["CountryName"]}");
                }
            }

            // Force user to select an existing Country ID
            while (true)
            {
                int countryID = ReadInt("Enter Country ID from the list above: ");
                if (clsCountry.IsCountryExist(countryID))
                {
                    return countryID;
                }
                Console.WriteLine("Invalid Country ID. Please check the list and try again.");
            }
        }

        static void PrintContactCard(clsContact contact)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------------- DETAILS ----------------");
            Console.ResetColor();
            Console.WriteLine($"ID           : {contact.ID}");
            Console.WriteLine($"Name         : {contact.FirstName} {contact.LastName}");
            Console.WriteLine($"Email        : {contact.Email}");
            Console.WriteLine($"Phone        : {contact.Phone}");
            Console.WriteLine($"Address      : {contact.Address}");
            Console.WriteLine($"Date of Birth: {contact.DateOfBirth:dd/MMM/yyyy}");

            // Fetch Country Name for display instead of just ID
            clsCountry country = clsCountry.Find(contact.CountryID);
            string countryName = (country != null) ? country.CountryName : "Unknown";
            Console.WriteLine($"Country      : {countryName} (ID: {contact.CountryID})");

            Console.WriteLine($"Image Path   : {contact.ImagePath}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-----------------------------------------");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void PrintMessage(string message, ConsoleColor color, bool clear = false)
        {
            if (clear) Console.Clear();
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}