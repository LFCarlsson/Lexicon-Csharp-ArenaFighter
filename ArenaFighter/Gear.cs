using System;

namespace ArenaFighter
{
   [Serializable]
    internal class Gear
    {
        public string name;
        public string description;
        public int value;

        //A gear contains two delegate parameters. One delegate for applying when defending and one for attacking
        public delegate int Modifier(int input);
        public Modifier attack;
        public Modifier defence;

        public Gear(string name, string description, int value, Modifier attack, Modifier defence)
        {
            this.name = name;
            this.description = description;
            this.value = value;
            this.attack = attack;
            this.defence = defence;
        }
    }
}