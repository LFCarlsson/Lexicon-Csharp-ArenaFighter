using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter
{
    [Serializable]
    struct Stats
    {
        public int strenght;
        public int vitality;


        public Stats(int str, int vit)
        {
            this.strenght = str;
            this.vitality = vit;

        }
    }
}
