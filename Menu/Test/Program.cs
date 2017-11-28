using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuNameSpace;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu("This is a test menu");
            menu.AddEntry("First test", new NoArg(test1));
            menu.AddEntry("SecondTest", new HasArg(test2), "this is a test string");
            menu.AddEntry("First test", new NoArg(test1));
            menu.AddEntry("SecondTest", new HasArg(test2), "this is a test string");
            menu.AddEntry("First test", new NoArg(test1));
            menu.AddEntry("Do i have a page?", new HasArg(test2), "this is a test string");
            while (true)
                menu.Prompt();
        }
        public delegate void NoArg();
        public delegate void HasArg(string teststr);
        public static void test1()
        {
            Console.WriteLine("this is test method 1");
            Console.ReadKey(true);
        }

        public static void test2(string teststr)
        {
            Console.WriteLine("test2: " + teststr);
            Console.ReadKey(true);
        }
    }
}
