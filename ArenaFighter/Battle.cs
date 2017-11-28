using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArenaFighter
{
    internal class Battle
    {
        Character combatant1;
        Character combatant2;

        public List<Round> battleLog = new List<Round>(); //description of the battle

        private Random dice = new Random();

        public Battle(Character combatant1, Character combatant2)
        {
            this.combatant1 = combatant1;
            this.combatant2 = combatant2;
        }

        /// <summary>
        /// Print information about the battle.
        /// </summary>
        private void Introduction()
        {
            Console.WriteLine("Welcome to fight night!");
            Console.Write("In this corner: ");
            combatant1.Present();
            Console.Write("\nAnd in the other corner: ");
            combatant2.Present();
            Console.WriteLine("\nLet's get ready to rumble! (press 'any key') \n");
            Console.ReadKey();
        }

        /// <summary>
        /// Perform a battle between the two characters.
        /// </summary>
        /// <returns>The winning Character object</returns>
        public Character DoBattle()
        {
            Introduction();
            while(combatant1.Health > 0 && combatant2.Health > 0)
            {
                int damageCombatant1 = combatant1.DealDamage(dice.Next(1,7));
                int damageCombatant2 = combatant2.DealDamage(dice.Next(1, 7));

                //Fudge the police, defining who hits should not be dependant on strenght. Game breaking mechanic.
                if (damageCombatant1 > damageCombatant2)
                {
                    int damageDealt = damageCombatant1;
                    int damageTaken = combatant2.TakeDamage( damageDealt );
                    battleLog.Add(new Round(combatant1, combatant2, damageDealt,damageTaken));
                }
                else
                {
                    int damageDealt = damageCombatant2;
                    int damageTaken = combatant1.TakeDamage( damageDealt );
                    battleLog.Add(new Round(combatant2, combatant1, damageDealt,damageTaken));
                }
            }
            return combatant1.Health > 0 ? combatant1 : combatant2;
            
        }

        
    }
}
