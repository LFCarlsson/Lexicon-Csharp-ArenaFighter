# Lexicon-Csharp-ArenaFighter
Assignment 3 on Lexicon ASP.net course

Arena Fighter 
Your assignment is to make a simple game, based on dice rolls. In this game, the player creates a character, and proceeds to fight randomly generated opponents until the player character loses, in which case the game should end. At every victory, the player should be offered the chance to retire his character, which should also end the game.

Once battle starts, roll a dice by generating a random number between 1 and 6, and adding it to the strength value of the character. Then compare the total strength values of both characters, and have the character with the highest value deal damage to the other.

You will also be provided with a Infogenerator, that helps you create random names for your enemies.
Required Features:


• The player should be able to create his character, give it a name, and randomize its attribute values.
• The game should randomly create opponents for the player to fight until the player character dies or retires.
• When the player character dies or retires, the game should assign a score to the player based on how many fights their character survived, and whether they are still alive or not.
• A log of each battle should be stored, and be made available to the player at the end of the game.

Optional:


• Add in arena variety, giving bonuses and penalties based on gear and other factors. Which one ends up being used should be random.
• When the player is victorious, award them with in-game currency, which can be spent on upgrading their gear, increasing their statistics.
Code Requirements:


• Required classes:
        ○ Character – for both the player and the opponents
        ○ Battle – for the battle itself; should contain the log of the battle, as well as references to both the player and the opponent
        ○ Round – one round of dice rolling; should correspond to a post in the battle log

Resources:


• http://www.pluralsight.com/courses/c-sharp-fundamentals-with-visual-studio-2015
        ○ Focus on “Classes and Objects” and “Object-Oriented Programming”
• https://app.pluralsight.com/library/courses/encapsulation-solid/table-of-contents (the “Encapsulation” chapter)
Expected Duration: 5 days

Subjects Covered:


• Objects and classes.
        ○ States
        ○ Behaviours
        ○ Object relationships
• Collections
• Random number generation
