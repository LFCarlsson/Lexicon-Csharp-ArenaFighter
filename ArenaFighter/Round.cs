using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter
{
    [Serializable] //Need to store rounds to keep battlelog between saves.
    internal class Round
    {
        static Random random = new Random();
        public string roundSummary;

        //list of flavor text to randomly choose from
        static string[] survivedThisRound = { "They live to fight another day", "'tis only a flesh wound", "Their head still hangs on by a thread" };
        static string[] diedThisRound = { "Unfortunately they hit the bucket.", "They will be missed by their cat.", "They died on the way to the hospital.", "They're an angel now." };

        /// <summary>
        /// creates a roundSummary from information about a battle
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <param name="damageDealt"></param>
        /// <param name="damageTaken"></param>
        public Round(Character attacker, Character defender, int damageDealt,int damageTaken)
        {
            bool targetSurvived = defender.Alive;
            roundSummary = attacker.Name + " attacks " + defender.Name + " for " + damageDealt + " damage. \n";
            roundSummary += defender.Name + " takes " + damageTaken + " of it \n";
            if (targetSurvived)
            {
                roundSummary += survivedThisRound[random.Next(survivedThisRound.Length)];
            }
            else
            {
                roundSummary += diedThisRound[random.Next(diedThisRound.Length)];
            }
           
        }
    }
}
