using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuNameSpace
{
    internal class MenuPage
    {
        const int MAXENTRIES = 5; //maximum of menu items on one page

        private string description; // The text to be presented above the menu options. Gets assigned from the corresponding Menu object.
        MenuPage previous, next;    //Think node in a doubly linked list

        List<MenuEntry> entries; // A list of the different options given to a user watching this page.

        public MenuPage(string description, MenuPage previous) : this(description)
        {
            this.previous = previous;
            previous.next = this;
        }
        public MenuPage(string description)
        {
            this.description = description;
            previous = null;
            next = null;
            entries = new List<MenuEntry>();
        }

        /// <summary>
        /// Attempts to push another entry into the page. If full it asks the next page to add it. 
        /// If no next page exists, one is created.
        /// </summary>
        /// <param name="entry"></param>
        public void Add(MenuEntry entry)
        {
            if (entries.Count() < MAXENTRIES)
            {
                entries.Add(entry);
            }
            else
            {
                if (next == null)
                {
                    next = new MenuPage(this.description,this);
                }
                next.Add(entry);
            }
        }

        //TODO: implement remove. Could be a little tricky to do right, but a simple solution would work for now

        /// <summary>
        /// Prints out the menu page on console
        /// </summary>
        private void Print()
        {
            Console.Clear();

            Console.WriteLine(description + "\n");
            if(entries != null)
                for(int i = 0; i < entries.Count(); i++)
                {
                    Console.WriteLine("\n" + i + ") " + entries[i]);
                }
            if(previous != null)
            {
                Console.WriteLine( "\n p) previous page");
            }
            if (next != null)
            {
                Console.WriteLine( "\n n) next page");
            }
        }

        /// <summary>
        /// Prints out the page and polls the user for a menu choice.
        /// </summary>
        // abuse of next and previous options could lead to a lot of method calls on the stack,
        // but should be okay since the speed of calls are limited to user inputs.
        internal char Prompt()
        {
            Print();
            while (true)
            {
                char key = Console.ReadKey(true).KeyChar;

                if (key == 'n' && next != null)
                {
                    next.Prompt();
                    return key;
                }
                if (key == 'p' && previous != null)
                {
                    previous.Prompt();
                    return key;
                }
                int val = (int) Char.GetNumericValue(key);
                if (val >= 0 && val < entries.Count())
                {
                    entries[val].PerformAction();
                    return key;
                }
                    }
        }




    }
}
