using MenuNameSpace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ArenaFighter
{
    public class Arena
    {
        const int ROUNDDELAY = 500; //Used when printing the battles. Ms delay between rounds
        bool running = true; //Used for keeping track of if the current menu should loop or not (Probably not the best solution, but works for now
        public static int highscore; 
        PlayerCharacter player = null;
        Store store = null;

        // Two types of delegates for putting into menus. One without arguments and one with
        // It's very possible that there is a better solution for invoking methods from menus.
        // it get's a bit 'wordy' and technical to use.
        private delegate void MenuOption();
        private delegate void MenuOptionWithArgs(object[] args);

        /// <summary>
        /// The entry point for starting a game of Arena. Call on an Arena object to start the game.
        /// </summary>
        public void Run()
        {
            while (running)
            {
                Console.Clear();
                StartScreen();
            }

        }

        public Arena()
        {
            highscore = Helpers.ReadHighScore();
            StartScreen();
        }
 
        /// <summary>
        /// The "Start menu" allows the user to choose between loading, starting a new game and exiting the game.
        /// </summary>
        private void StartScreen()
        {
            Menu startScreen = new Menu("Welcome to Arena Fighter, the current High Score is " + highscore);
            startScreen.AddEntry("Start New Game", this, new MenuOption(CreateGame));
            startScreen.AddEntry("Load Game", this, new MenuOption(LoadingScreen));
            startScreen.AddEntry("Exit", this, new MenuOption(ExitMenu));
            startScreen.Prompt();
        }

        /// <summary>
        /// The main menu of a game in progress. Allows 'Hunt' for an opponenet, 'Retire' the player and end game.
        /// Checking and changing the players 'Stats'. 'Visit store' to spend gold on equipment or healing.
        /// 'Load' and 'Save' game and 'Exit' back to the start menu.
        /// </summary>
        private void MainScreen()
        {
            Menu mainScreen = new Menu("What do you want to do, " + player.Name + "?");
            mainScreen.AddEntry("Hunt", this, new MenuOption(PlayRound));
            mainScreen.AddEntry("Retire", this, new MenuOption(Retire));
            mainScreen.AddEntry("Stats", this, new MenuOption(StatsScreen));
            mainScreen.AddEntry("Visit store", store, new MenuOptionWithArgs(store.Visit), new object[] { player });
            mainScreen.AddEntry("Load Game", this, new MenuOption(LoadingScreen));
            mainScreen.AddEntry("Save Game", this, new MenuOption(SaveScreen));
            mainScreen.AddEntry("Exit", this, new MenuOption(ExitMenu));

            //using the global variable 'running' to keep the main menu looping (or not)
            while (running)
            {
                if (player.Alive)
                {                   
                    mainScreen.Prompt();
                }
                else
                {
                    GameOverScreen();
                }
            }
        }

        /// <summary>
        /// If the player has one or more stat points to distribute it contains the 'Assign skillpoints' option.
        /// Otherwise only the 'Show status' and 'Return' option.
        /// </summary>
        private void StatsScreen()
        {

            running = true;
            while (true)
            {
                Menu statsScreen = new Menu(player.Name + " lvl." + player.Level + " unassigned stat points: " + player.StatPoints);
                if (player.StatPoints > 0)
                {
                    statsScreen.AddEntry("Assign Statpoints", player, new MenuOption(player.LevelUpScreen));
                }
                statsScreen.AddEntry("Show status", player, new MenuOption(player.ShowStats));
                statsScreen.AddEntry("Return", this, new MenuOption(ExitMenu));
                statsScreen.Prompt();

                //again a questionable but working solution for keeping the right menu alive.
                //running is set to true again on exit to keep the previous menu alive.
                if (!running)
                {
                    running = true;
                    break;
                }
            }
        }

        /// <summary>
        /// The menu shown when the player has lost either by dying or retiring. 
        /// It runs the logic to calculate the final score and checks if it's the new highscore.
        /// 
        /// The menu let's you watch 'Battle Logs', 'Play again' to return to start menu or 'Quit' to exit the game.
        /// </summary>
        private void GameOverScreen()
        {
            Console.Clear();
            int score = player.CalculateScore();
            Console.WriteLine("GAME OVER! Final score " + score);
            if (score > highscore)
            {
                Console.WriteLine("NEW HIGHSCORE!");
                highscore = score;
                Helpers.WriteHighScore(highscore);
            }
            else
            {
                Console.WriteLine("Highscore was " + highscore);
            }
            Console.WriteLine("(Press any key to continue)");
            Console.ReadKey(true);

            Menu gameOverScreen = new Menu("GAME OVER! Final Score: " + score);
            gameOverScreen.AddEntry("Battle Logs", this, new MenuOption(BattleLogScreen));
            gameOverScreen.AddEntry("Play Again", this, new MenuOption(StartScreen));
            gameOverScreen.AddEntry("Exit", this, new MenuOption(ExitMenu));

            running = true;
            while (running)
            {
                gameOverScreen.Prompt();
            }
        }

        /// <summary>
        /// The menu gets filled with all available battle logs. 
        /// The user can view as many as they like and then 'Exit' back to the Game Over menu.
        /// </summary>
        private void BattleLogScreen()
        {
            running = true;

            //TODO: the BattleLog also needs to be serialized. Let it belong to the player?
            Menu battleLogScreen = new Menu("Read the tales of your glorious battles (since this save unfortunately)");
            for (int i = 0; i < player.battleLogs.Count(); i++)
            {
                battleLogScreen.AddEntry("Battle " + (i + 1), this, new MenuOptionWithArgs(PrintBattle), new object[] { player.battleLogs.ElementAt(i) });
            }

            battleLogScreen.AddEntry("Exit", this, new MenuOption(ExitMenu));

            while (running)
            {
                battleLogScreen.Prompt();
                //HACK this exits the battle log screen without also exiting the previous menu. This is maybe something that the Menu class could handle if extended a bit
                if (running == false)
                {
                    running = true;
                    break;
                }
            }

        }

        /// <summary>
        /// Prompts the user to save their game either in an existing slot or to create a 'New Save'.
        /// Optionally the user can 'Return' to the Main screen without performing a save.
        /// </summary>
        private void SaveScreen()
        {
            string[] saveFiles = Helpers.GetSaveNames();
            Menu saveMenu = new Menu("Choose existing save or create a new one");
            Menu.HasArg save = SaveGame;
            MenuOptionWithArgs saveOption = new MenuOptionWithArgs(SaveGame);
            foreach (string saveFile in saveFiles)
            {
                string root = @"saves/";
                string saveName = saveFile.Remove(0, root.Length);
                saveMenu.AddEntry(saveName, this, saveOption, new object[] { saveName });
            }

            saveMenu.AddEntry("New Save", this, new MenuOption(SavePrompt));
            saveMenu.AddEntry("Return", this, new MenuOption(MainScreen));

            saveMenu.Prompt();

        }

        /// <summary>
        /// Prompts the user to pick a saved game to load.
        /// Optionally the user can return to the previous menu.
        /// </summary>
        private void LoadingScreen()
        {
            Console.Clear();
            Menu loadingScreen = new Menu("Choose a save file");
            string[] saveFiles = Helpers.GetSaveNames();
            MenuOptionWithArgs load = LoadGame;
            if (saveFiles != null)
            {
                foreach (string saveFile in saveFiles)
                {
                    string root = @"saves/";
                    loadingScreen.AddEntry(saveFile.Remove(0, root.Length), this, load, new[] { saveFile, "test" });
                }
            }

            //disaster if the player backs out from the load screen into the main screen coming from the start screen. 
            //if we don't have a player, we came from the start menu
            if (player == null)
            {
                loadingScreen.AddEntry("Exit", this, new MenuOption(StartScreen));
            }
            else
            {
                loadingScreen.AddEntry("Exit", this, new MenuOption(MainScreen));
            }

            loadingScreen.Prompt();
            MainScreen();
        }

        /// <summary>
        /// A little helper method to control the 'running' field. Can be used as a menu option for 'Exit' options.
        /// </summary>
        private void ExitMenu()
        {
            running = false;
        }

        /// <summary>
        /// Prints a little flavor text and goes to the game over menu with a still alive player.
        /// </summary>
        private void Retire()
        {
            Console.WriteLine(player.Name + " has retired");
            Console.WriteLine("(Press any key to continue)");
            Console.ReadKey();
            GameOverScreen();
        }

        /// <summary>
        /// Saves the game with name 'name' if it doesen't exist create it.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool SaveGame(string name)
        {
            string fileNamePlayer = @"saves\" + name + @"\player.bin";
            string fileNameStore = @"saves\" + name + @"\store.bin";
            if (!Helpers.GetSaveNames().Contains(Path.GetDirectoryName(fileNamePlayer)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileNamePlayer));
            }

            bool savedStore = Helpers.TrySerialize(fileNameStore, store);
            bool savedPlayer = Helpers.TrySerialize(fileNamePlayer, player);

            return savedStore && savedPlayer;
        }

        /// <summary>
        /// An overload SaveGame for use as a menu option.
        /// </summary>
        /// <param name="args">Should contain a string</param>
        private void SaveGame(object[] args)
        {
            SaveGame((string)args[0]);
        }

        /// <summary>
        /// Loads a previously stored player and store from the folder with the name used as argument.
        /// </summary>
        /// <param name="args">Should contain a string</param>
        private void LoadGame(object[] args)
        {
            string fileNamePlayer = (string)args[0] + "\\player.bin";
            string fileNameStore = (string)args[0] + "\\store.bin";

            bool loadedStore = false;
            bool loadedPlayer = false;
            int attempts = 0;
            while (attempts < 3 && (!loadedStore || !loadedPlayer))
            {

                loadedStore = Helpers.TryDeserialize(fileNameStore, out store);
                loadedPlayer = Helpers.TryDeserialize(fileNamePlayer, out player);
                attempts++;
            }



        }

        /// <summary>
        /// A little method for prompting the user for a save name, then creating the save
        /// </summary>
        private void SavePrompt()
        {
            Console.Write("Enter save name: ");
            string saveName = Console.ReadLine();
            SaveGame(saveName);
        }

        /// <summary>
        /// Contains most of the logic actually performing a round of the game. It's what called in the 'Hunt' menu option.
        /// </summary>
        private void PlayRound()
        {
            //Initialization
            Character winner = null;

            Console.Clear();
            Character enemy = new Character();

            //If the player has any equipment, equip one and show a little notification about it
            player.EquipGear();
            if (player.equippedGear != null)
            {
                Console.WriteLine("{0} has equipped {1} for their next fight! \n", player.Name, player.equippedGear.name);
            }

            //create a new battle between the player and enemy, run and print it.
            Battle battle = new Battle(player, enemy);
            winner = battle.DoBattle();
            player.battleLogs.Add(battle.battleLog);
            PrintBattle(battle.battleLog);

            //the player get rewards for winning a battle
            if (winner == player)
            {
                player.battlesSurvived++;
                player.gold++;
                player.LevelUp();
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Creates a new game. Creates a new player and store, and then enters the main screen.
        /// </summary>
        private void CreateGame()
        {
            store = new Store();
            player = new PlayerCharacter();
            MainScreen();
        }

        /// <summary>
        /// Prints out a battle. Delay between rounds is set by the global constant ROUNDDELAY
        /// </summary>
        /// <param name="battleLog"> A list of battle rounds. Stored in the player object (for saving purposes).</param>
        private void PrintBattle(List<Round> battleLog)
        {
            foreach (Round i in battleLog)
            {
                Console.WriteLine(i.roundSummary);
                Console.WriteLine("");
                Thread.Sleep(ROUNDDELAY);
            }
            Console.WriteLine("(Press any key)");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Overload for easier calling through an menu
        /// </summary>
        /// <param name="args">A List of Rounds</param>
        private void PrintBattle(object[] args)
        {
            Console.Clear();
            PrintBattle((List<Round>)args[0]);
        }

    }
}