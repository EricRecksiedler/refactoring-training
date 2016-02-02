using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        public static void Start(List<User> usrs, List<Product> prods)
        {
            // Write welcome message
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");

            User currUser;
            Users users = new Users(usrs);
            currUser = UserLogin(users);

            // if valid password
            if (currUser != null)
            {

                Shop(prods, currUser);
                SaveData((List<User>)users, prods);
            }

            ShowExitMsg();
        }

        private static void Shop(List<Product> prods, User currUser)
        {
            ShowWelcomeMsg(currUser);

            // Show remaining balance
            double bal = 0;

            if (currUser != null)
            {
                bal = currUser.Bal;

                // Show balance 
                Console.WriteLine();
                Console.WriteLine("Your balance is " + currUser.Bal.ToString("C"));
            }

            // Show product list
            while (true)
            {
                // Prompt for user input
                Console.WriteLine();
                Console.WriteLine("What would you like to buy?");
                for (int i = 0; i < prods.Count; i++)
                {
                    Product prod = prods[i];
                    Console.WriteLine(prod.Description(i));
                }
                Console.WriteLine(prods.Count + 1 + ": Exit");

                // Prompt for user input
                Console.WriteLine("Enter a number:");
                string answer = Console.ReadLine();
                int num = Convert.ToInt32(answer);
                num = num - 1;

                // Check if user entered number that equals product count
                if (num == prods.Count)
                {
                    currUser.Bal = bal;
                    return;
                }
                else if (num > prods.Count)
                {
                    String errText = "Invalid Selection.";
                    ShowErrMsg(errText);
                }
                else
                {
                    BuyItem(prods, ref bal, num);
                }
            }
        }

        private static void ShowWelcomeMsg(User currUser)
        {
            // Show welcome message
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Login successful! Welcome " + currUser.Name + "!");
            Console.ResetColor();
        }

        private static User UserLogin(Users usrs)
        {
            User currUser = null;
            
            do
            {
                // Prompt for user input
                Console.WriteLine();
                Console.WriteLine("Enter Username:");
                string name = Console.ReadLine();

                // Validate Username
                bool valUsr = false; // Is valid user?
                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }


                if (usrs.FindUserByName(name) != null)
                {
                    valUsr = true;
                }
                else
                {
                    valUsr = false;
                    String errText = "You entered an invalid user.";
                    // Invalid User
                    ShowErrMsg(errText);
                }

                // if valid user
                if (valUsr)
                {
                    // Prompt for user input
                    Console.WriteLine("Enter Password:");
                    string pwd = Console.ReadLine();

                    currUser = usrs.FindUser(name, pwd);

                    if (currUser == null)
                    {
                        String errText = "You entered an invalid password.";
                        // Invalid Password
                        ShowErrMsg(errText);
                    }
                }

            } while(currUser == null);

            return currUser;
        }

        private static void BuyItem(List<Product> prods, ref double bal, int num)
        {
            Console.WriteLine();
            Console.WriteLine("You want to buy: " + prods[num].Name);
            Console.WriteLine("Your balance is " + bal.ToString("C"));

            // Prompt for user input
            Console.WriteLine("Enter amount to purchase:");
            string answer = Console.ReadLine();
            int qty = Convert.ToInt32(answer);

            // Check if balance - quantity * price is less than 0
            if (bal - prods[num].Price * qty < 0)
            {
                String errText = "You do not have enough money to buy that.";
                ShowErrMsg(errText);
                return;
            }

            // Check if quantity is less than quantity
            if (prods[num].Qty <= qty)
            {
                String errText = "Sorry, " + prods[num].Name + " is out of stock";
                ShowErrMsg(errText);
                return;
            }

            // Check if quantity is greater than zero
            if (qty > 0)
            {
                // Balance = Balance - Price * Quantity
                bal = bal - prods[num].Price * qty;

                // Quanity = Quantity - Quantity
                prods[num].Qty = prods[num].Qty - qty;

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You bought " + qty + " " + prods[num].Name);
                Console.WriteLine("Your new balance is " + bal.ToString("C"));
                Console.ResetColor();
            }
            else
            {
                String errText = "Purchase cancelled";
                // Quantity is less than zero
                ShowErrMsg(errText);
            }

            return;
        }

        private static void SaveData(List<User> usrs, List<Product> prods)
        {
            // Write out new balance
            string json = JsonConvert.SerializeObject(usrs, Formatting.Indented);
            File.WriteAllText(@"Data/Users.json", json);

            // Write out new quantities
            string json2 = JsonConvert.SerializeObject(prods, Formatting.Indented);
            File.WriteAllText(@"Data/Products.json", json2);
        }

        private static void ShowExitMsg()
        {
            // Prevent console from closing
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }

        private static void ShowErrMsg(String errText)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(errText);
            Console.ResetColor();
        }
    }
}
