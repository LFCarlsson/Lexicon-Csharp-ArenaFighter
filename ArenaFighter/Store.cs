using MenuNameSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter
{
    [Serializable]
    internal class Store
    {
        List<Gear> stock;
        const int POTIONPRICE = 2; //Price of the potion
        bool visiting = true; //Used to determine if the game should stay in the store menu or not.

        /// <summary>
        /// Fill the store's stock with gear. Prices and balance are nonsense for now
        /// </summary>
        public Store()
        {
            stock = new List<Gear>
            {
                new Gear("Tiny sword", "A little puny sword. Attack +1", 1, (x) => x + 1, (x) => x),
                new Gear("Big sword", "A huge sword. Attack +2", 3, (x) => x + 2, (x) => x),
                new Gear("The Decider", "It's all or nothing..", 1, (x) => x * 1000, (x) => x * 1000),
                new Gear("Helmet", "A hat fit for a fight. Defence +3", 0, (x) => x, (x) => x - 3)
            };

        }

        /// <summary>
        /// Overload of Visit for use in menu.
        /// </summary>
        /// <param name="args"></param>
        public void Visit(object[] args)
        {
            Visit((PlayerCharacter)args[0]);
        }

        /// <summary>
        /// Creates and prompts a store menu object. List all available equipment and lets the user try to buy them.
        /// </summary>
        /// <param name="player"></param>
        public void Visit(PlayerCharacter player)
        {
           visiting = true;
            while (visiting)
            {

                Menu menu = new Menu("Welcome to the store, you have " + player.gold + " gold to spend");
                for (int i = 0; i < stock.Count(); i++)
                {
                    string entryText = stock[i].name + ": " + stock[i].description + ", " + stock[i].value + " gold.";
                    menu.AddEntry(entryText, this, new Menu.HasArg(TryBuy), new object[] { player, i });
                }
                menu.AddEntry("Health potion: Heals you up to max health, " + POTIONPRICE + " gold", this, new Menu.HasArg(BuyPotion), new object[] { player});
                menu.AddEntry("Exit", this, new Menu.NoArg(Exit));

                char c = menu.Prompt();
            }
        }

        /// <summary>
        /// Wrapping visiting == false in a method for easier usage in the menu
        /// </summary>
        private void Exit()
        {
            visiting = false;
        }

        /// <summary>
        /// Overload for calling TryBuy through a Menu object
        /// </summary>
        /// <param name="args">args[0] PlayerCharacter, args[1] int</param>
        void TryBuy(object[] args)
        {
            TryBuy((PlayerCharacter)args[0],(int) args[1]);
        }

        /// <summary>
        /// Overload for calling TryBuy through a Menu object
        /// </summary>
        /// <param name="args">args[0] the player</param>
        void BuyPotion(object[] args)
        {
            BuyPotion((PlayerCharacter)args[0]);
        }

        /// <summary>
        /// The player tries to buy a potion. It succeds if they have enough gold resulting in a full heal
        /// </summary>
        /// <param name="player">the players character</param>
        private void BuyPotion(PlayerCharacter player)
        {
            if (player.gold >= POTIONPRICE)
            {
                player.gold -= POTIONPRICE;
                player.SetMaxHealth();
                Console.WriteLine("You healed up to your max health of {0}hp", player.Health);
            }
            else
            {
                Console.WriteLine("You're too poor for healthcare, no handouts, get out of my store!!");
            }
            Console.ReadKey(true);
        }

        /// <summary>
        /// The player tries to buy a piece of equipment. It succeds if they have enough gold. The item is put in the players inventory.
        /// </summary>
        /// <param name="playerCharacter"></param>
        /// <param name="choice"></param>
        private void TryBuy(PlayerCharacter playerCharacter, int choice)
        {
            Console.Clear();
            Gear chosenGear = stock[choice];
            if(playerCharacter.gold >= chosenGear.value)
            {
                Console.WriteLine("Here you go!");
                playerCharacter.gold -= chosenGear.value;
                playerCharacter.AddInventory(chosenGear);
                stock.Remove(chosenGear);
            }
            else
            {
                Console.WriteLine("You're too poor, get out of here!");
            }
            Console.ReadKey(true);
        }


    }
}
