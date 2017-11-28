using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuNameSpace
{
    public class MenuEntry
    {
        string description; // The text show to the user on the corresponding line on a MenuPage
        Delegate action; // The effect of picking the entry in a menu
        object[] arguments; // arguments to call the delegate with
        object caller; // on what object to call the delegate

        public MenuEntry(string description, object caller, Delegate action)
        {
            this.description = description;
            this.action = action;
            this.caller = caller;
        }
        public MenuEntry(string description, object caller, Delegate action, object[] arguments)
        {
            this.description = description;
            this.arguments = arguments;
            this.action = action;
            this.caller = caller;
        }
        public override string ToString()
        {
            return description;
        }

        /// <summary>
        /// Invokes the stored delegate
        /// </summary>
        public void PerformAction()
        {
            if (arguments == null)
                action.Method.Invoke(caller, null);
            else
                action.Method.Invoke(caller,new object[] { arguments } );
        }
    }
}
