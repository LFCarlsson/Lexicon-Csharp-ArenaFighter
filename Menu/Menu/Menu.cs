using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuNameSpace
{
    public class Menu
    {
        // Two types of delegates for putting into menus. One without arguments and one with
        // It's very possible that there is a better solution for invoking methods from menus.
        // it get's a bit 'wordy' and technical to use.
        public delegate void NoArg();
        public delegate void HasArg(params object[] args);

        public string description; // The text to be presented above the menu options.
        private MenuPage headEntry; // The first page in the menu.

        public Menu(string description)
        {
            this.description = description;
            headEntry = new MenuPage(description);
        }

        /// <summary>
        /// Add a new MenuEntry to the menu. Propegates through pages until one with room left is found or created.
        /// </summary>
        /// <param name="entry"></param>
        public void AddEntry(MenuEntry entry)
        {
            headEntry.Add(entry);
        }

        /// <summary>
        /// Add a new MenuEntry to the menu. Propegates through pages until one with room left is found or created.
        /// </summary>
        /// <param name="entry"></param>
        public void AddEntry(string description, object caller, Delegate result)
        {
            headEntry.Add(new MenuEntry(description, caller, result));
        }

        /// <summary>
        /// Add a new MenuEntry to the menu. Propegates through pages until one with room left is found or created.
        /// </summary>
        /// <param name="entry"></param>
        public void AddEntry(string description, object caller, Delegate result,params object[] args)
        {
            headEntry.Add(new MenuEntry(description, caller, result, args));
        }


        /// <summary>
        /// Call to display the menu and let the user pick one of the options.
        /// </summary>
        /// <returns>the users choice for more advanced usage</returns>
        public char Prompt()
        {
            return headEntry.Prompt();
        }

    }
}
