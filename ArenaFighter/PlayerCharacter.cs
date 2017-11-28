using MenuNameSpace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter
{
    [Serializable]
    internal class PlayerCharacter : Character
    {

        public int battlesSurvived = 0; //used together with Character.alive to calculate final score.
        public List<List<Round>> battleLogs = new List<List<Round>>(); //Possibly weird place to store the battle logs, but reduces number of objects to save.
        public int gold = 0;
        private List<Gear> inventory = new List<Gear>(); //list of gear owned by the player
        private int level = 0; //not really used for anything other than flavor.
        private int statPoints = STARTSTATS; //stores the ammount of unused stat points the player has

        internal int Level { get => level; private set => level = value; }
        internal int StatPoints { get => statPoints; private set => statPoints = value; }

        private delegate void MenuOption();
        private delegate void MenuOptionWithArgs(object[] args);

        /// <summary>
        /// Called after the player survives a battle. Increase level and statpoints
        /// </summary>
        internal void LevelUp()
        {
            Console.WriteLine("DING! Level up");
            Level++;
            StatPoints++;
        }

        /// <summary>
        /// Overload of AddStat for use in menu
        /// </summary>
        /// <param name="args">args[0] string (str or vit)</param>
        internal void AddStat(object[] args)
        {
            AddStat((string) args[0]);
        }

        /// <summary>
        /// Prompts the user about how many points they want to add to the stat represented by the argument string
        /// </summary>
        /// <param name="stat">str or vit</param>
        internal void AddStat(string stat)
        {
            Console.Clear();
            Console.WriteLine("How many points?");
            int points = 0;

            while (true)
            {
                try
                {
                    points = int.Parse(Console.ReadLine());
                    if (points <= statPoints)
                    {
                        statPoints -= points;
                        break;
                    }
                    else
                    {
                        Console.Write("Number not allowed ");
                    }
                }
                catch
                {
                    Console.Write("Bad input ");
                }
                Console.WriteLine("try again");
            }
            if (stat == "str")
            {
                stats.strenght += points;
            }
            else if (stat == "vit")
            {
                stats.vitality += points;
                Health += points;
            }
        }

        /// <summary>
        /// Calculates the score a PlayerCharacter has accumulated. Dying halves the score.
        /// </summary>
        /// <returns>score</returns>
        internal int CalculateScore()
        {
            int score = battlesSurvived * 1000;
            score /= Alive ? 1 : 2;
            return score;
        }

        /// <summary>
        /// Adds a Gear object to the players inventory
        /// </summary>
        /// <param name="gear"> the item to add</param>
        internal void AddInventory(Gear gear)
        {
            inventory.Add(gear);
        }

        //prints
        internal void ShowStats()
        {
            Console.Clear();
            Console.WriteLine("Name: " + name);
            Console.WriteLine("Level: " + level);
            Console.WriteLine("Health:" + Health);
            Console.WriteLine("Unused statpoints: " + statPoints);
            Console.WriteLine("Strenght: " + stats.strenght);
            Console.WriteLine("Vitality: " + stats.vitality);
            Console.WriteLine("Gold: " + gold);
            Console.WriteLine("Equipment: ");

            foreach(Gear gear in inventory)
            {
                Console.WriteLine(gear.name + " : " + gear.description);
            }

            Console.WriteLine("(Press any key to continue)");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Equips the user with a randomly chosen piece of gear from their inventory
        /// </summary>
        internal void EquipGear()
        {
            if(inventory.Count > 0)
                equippedGear = inventory[random.Next(inventory.Count)];
        }

        /// <summary>
        /// Calculates actual damage done after a given diceroll, strength and current equipped gear.
        /// </summary>
        /// <param name="diceRoll"> a random integer between 1 and 6</param>
        /// <returns>the damage output</returns>
        public override int DealDamage(int diceRoll)
        {
            int attack = diceRoll + stats.strenght;
            if (equippedGear != null)
            {
                //Console.WriteLine("{0} uses {1}!",name,equippedGear.name);
                attack = equippedGear.attack(attack);
            }

            return attack;
        }

        /// <summary>
        /// Prompts the user to name their character
        /// </summary>
        protected override void NameCharacter()
        {
            Console.WriteLine("What's your mighty warriors name?");
            Name = Console.ReadLine();
            Console.WriteLine(Name + ".. What a silly name!");
        }


        bool menuRunning = false; // used to keep the level up screen alive

        //creates and promts a menu for adding stat points to the player.
        internal void LevelUpScreen()
        {
            menuRunning = true;
            while (menuRunning)
            {
                Menu levelUpScreen = new Menu(" unassigned stat points: " + this.StatPoints);
                levelUpScreen.AddEntry("Add strength", this, new MenuOptionWithArgs(this.AddStat), new object[] { "str" });
                levelUpScreen.AddEntry("Add vitatlity", this, new MenuOptionWithArgs(this.AddStat), new object[] { "vit" });
                levelUpScreen.AddEntry("Return", this, new MenuOption(ExitMenu));
                levelUpScreen.Prompt();
            }
               
        }

        /// <summary>
        /// sets menuRunning to false to exit the level up screen
        /// </summary>
        private void ExitMenu()
        {
            menuRunning = false;
        }

        /// <summary>
        /// Prompts the user to enter stat points instead of randomly assigning them.
        /// </summary>
        protected override void GenerateStats()
        {
            LevelUpScreen();

            
        }


    }
}
