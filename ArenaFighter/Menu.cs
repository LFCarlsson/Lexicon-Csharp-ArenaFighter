using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter
{
    class Menu
    {
        const int MAXITEMS = 5; //maximum of menu items on one page

        public delegate void MenuOption(int choice);

        private string flavorText;
        public List<Tuple<string, Delegate, object[]>> menuItems = new List<Tuple<string, Delegate, object[]>> ();
        private List<List<Tuple<string, Delegate, object[]>>> menu = new List<List<Tuple<string, Delegate, object[]>>>();

        public Menu(string flavorText)
        {
            this.flavorText = flavorText;
        }
        
        public void Add(string description, Delegate menuOption,object[] args)
        {
            menuItems.Add(new Tuple<string, Delegate, object[]>(description,menuOption, args)) ;
        }

        public void Add(string description, Delegate menuOption)
        {
            Add(description, menuOption, null);
        }


        override public string ToString()
        {
            string result = flavorText + "\n\n";
            int i = 0;
            foreach(Tuple<string, Delegate, object[]> menuEntry in menuItems)
            {
                result += i + ") " + menuEntry.Item1 + "\n";
                i++;
            }
            return result;
        }

        public void GenerateMenu()
        {
            int numberOfPages = (menuItems.Count() / MAXITEMS) + 1;

            for(int i = 0; i < numberOfPages; i += MAXITEMS)
            {
                menu.Add(menuItems.Take(MAXITEMS).ToList());
            }

        
        }

        public void PromptUser()
        {
            Console.WriteLine(this);
            while (true) {
                Console.Write(":");
                int option = 0;
                try
                {
                    option = int.Parse(Console.ReadKey(false).KeyChar.ToString());
                    Console.WriteLine("");
                }
                catch
                {
                    Console.WriteLine("Not integer, try again!");
                }
                if(option < 0 || option > menuItems.Count)
                {
                    Console.WriteLine("Option does not exist, try again");
                }
                else
                {
                    ExecuteMenuItem(option);
                    return;
                }
            }
        }

        private void ExecuteMenuItem(int option)
        {
            object[] args = menuItems.ElementAt(option).Item3;
            if (args == null)
                menuItems.ElementAt(option).Item2.DynamicInvoke();
            else
                menuItems.ElementAt(option).Item2.DynamicInvoke(new object[] { args });
        }
    }
}
