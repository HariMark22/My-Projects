using static Driver.Program;
using System;
using System.Collections.Generic;

namespace CAB301_Project
{
    // Boilerplate User Interface from CAB201 Year 2021. Every student in the class received a copy to use. Functions not used 
    // for CAB301 Assessment 3 will be removed.
    public class UserInterface
    {
        // Presents a list of options for the user to choose from
        public static void DisplayList<T>(string title, IList<T> list)
        {
            Console.WriteLine("=====================" + title + "=====================");
            Console.Write("\n\nEnter your choice ==> (");
            int i = 0;
            foreach (var item in list)
            {
                i++;
                if (i > 1) Console.Write("/");
                Console.Write(i);

            }
            Console.Write(") \n");

            if (list.Count == 0)
                Console.WriteLine("  None");
            else
                for (i = 0; i < list.Count; i++)
                    Console.WriteLine("  {0}) {1}", i + 1, list[i].ToString());

            Console.WriteLine();
        }

        // Gets choice from user from CLI
        // Precondition: none
        // Postcondition: Returns an integer of the valid choice the user has made.
        public static int getOption(int min, int max)
        {
            while (true)
            {
                var key = Console.ReadKey(true);
                var option = key.KeyChar - '0';
                if (min <= option && option <= max)
                    return option - 1;
                else
                    UserInterface.Error("Invalid option");
            }
        }

        // Retrieve input from the user via CLI
        // Precondtion: none
        // Postcondition: Returns a string of input from the user.
        public static string GetInput(string prompt)
        {
            Console.Write("{0}: ", prompt);
            return Console.ReadLine();
        }

        // Retrieve an integer from the user via CLI
        // Precondtion: none
        // Postcondition: Returns the input from the user as an integer. If the input is not an integer, the user is notified.
        public static int GetInteger(string prompt)
        {
            while (true)
            {
                var response = UserInterface.GetInput(prompt);
                int integer;
                if (int.TryParse(response, out integer))
                    return integer;
                else
                    Error("\nInvalid number");
            }
        }

        // Retrieves string input from the user and displays (*) in place of the characters entered for confidentiality
        // Precondition: none
        // Postcondition: Returns a string of input from the user, assumed to be a valid password to be used elsewhere.
        public static string GetPassword(string prompt)
        {
            Console.Write("{0}: ", prompt);
            var password = new System.Text.StringBuilder();
            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);
                var key = keyInfo.Key;

                if (key == ConsoleKey.Enter)
                    break;
                else if (key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        Console.Write("\b \b");
                        password.Remove(password.Length - 1, 1);
                    }
                }
                else
                {
                    Console.Write("*");
                    password.Append(keyInfo.KeyChar);
                }
            }
            Console.WriteLine();
            return password.ToString();
        }
        // Standard Error message that includes a newline
        public static void Error(string msg)
        {
            Console.WriteLine($"{msg}, please try again");
            Console.WriteLine();
        }

    }

    // Boilerplate Menu class from CAB201 Year 2021. Every student in the class received a copy to use. Sections not used 
    // for CAB301 Assessment 3 will be removed.
    public class Menu
    {
        private string title;

        public Menu(string title)
        {
            this.title = title;
        }

        class MenuItem
        {
            private string item;
            private Action selected;


            public MenuItem(string item, Action eventHandler)
            {
                this.item = item;
                selected = eventHandler;
            }

            public void select()
            {
                selected();
            }

            public override string ToString()
            {
                return item;
            }
        }

        private List<MenuItem> items = new List<MenuItem>();

        public void Add(string menuItem, Action eventHandler)
        {
            items.Add(new MenuItem(menuItem, eventHandler));
        }

        public void Display()
        {
            UserInterface.DisplayList(title, items);
            var option = UserInterface.getOption(1, items.Count);
            items[option].select();

        }


    }
}