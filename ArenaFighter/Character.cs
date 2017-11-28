using Lexicon.CSharp.InfoGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter
{
    [Serializable]
    internal class Character
    {
        //a random entry in the list is appended to the characters name
        static string[] titles = { " the Pulverizer", " of the Round Table", " the Bloodstained", " the Cuddly", " of the Lady of the Lake of the Blood of the Grail",
                                   " the smalltown girl"," of Rivia",", known by many names", " the meek"};

        protected const int STARTSTATS = 10; //The number of stat points a character is created with


        protected static InfoGenerator infoGenerator = new InfoGenerator();
        protected static Random random = new Random();

        protected Stats stats;
        protected string name;
        protected int health;
        public Gear equippedGear = null;

        private bool alive = true;
        public int Health { get => health; set {health = value; Alive = health > 0; } } // Setting health below 0 also sets alive to false;

        public string Name { get => name; protected set => name = value ; }
        public bool Alive { get => alive; private set => alive = value; }

        /// <summary>
        /// Standard constructor. Derived classes should probably implement own constructors on own risk. 
        /// </summary>
        public Character()
        {
            NameCharacter();
            GenerateStats();
            SetMaxHealth();

        }

        /// <summary>
        /// Generates and sets a random name for the character.
        /// </summary>
       virtual protected void NameCharacter()
        {
            Name = infoGenerator.NextFullName();
            Name += titles[random.Next(titles.Length)];
        }

        /// <summary>
        /// Sets the characters health to it's max value calculated by vitality.
        /// </summary>
        public void SetMaxHealth()
        {
            //TODO: figure out a moderately balanced formula
            Health = 10 + stats.vitality;
        }

        /// <summary>
        /// Randomly spreads STARTSTATS statpoints between the fields in the stats struct.
        /// </summary>
        virtual protected void GenerateStats()
        {
            int stat1 = random.Next(STARTSTATS - 10) + 6; //even out possible stat allocation a bit
            stats.strenght = stat1;
            stats.vitality = STARTSTATS - stat1;
        }

        /// <summary>
        /// Print out an representation of the character
        /// </summary>
        public void Present()
        {
            Console.Write(name + ", with a strenght of {0} and vitality of {1}. Their current health is {2}hp",
                                        stats.strenght,stats.vitality,Health);
        }

        public override string ToString()
        {
            return Name;
        }


        /// <summary>
        /// Calculates damagage dealt depending on an incoming diceroll and characters strength 
        /// </summary>
        /// <param name="diceRoll"> A random number between 1 and 6 </param>
        /// <returns>Damage as an integer</returns>
        virtual public int DealDamage(int diceRoll)
        {
            return stats.strenght + diceRoll;
        }

        /// <summary>
        /// Calculates how much damage a character takes after gear has been accounted for
        /// </summary>
        /// <param name="damage">The opposing characters damage output</param>
        /// <returns>Actual damage taken</returns>
        public int TakeDamage(int damage)
        {
            if (equippedGear != null)
            {
          
                damage = equippedGear.defence(damage);
            }

            Health -= damage;
            return damage;
        }
    }
}
