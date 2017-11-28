using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ArenaFighter
{
    /// <summary>
    /// A collection of static methods used througout the program.
    /// </summary>
    static class Helpers
    {
        /// <summary>
        /// Attempts to serialize and save an object to a file.
        /// </summary>
        /// <param name="path">The path to save to</param>
        /// <param name="o">the object to serialize</param>
        /// <returns>Successful or not</returns>
        static public bool TrySerialize(string path, object o)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = null;
            try
            {
                stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                formatter.Serialize(stream, o);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("PlayerCharacter.TrySerialize: " + e.Message);
            }
            finally
            {
                stream.Close();
            }
            return false;
        }
        /// <summary>
        /// Tries to read and deserialize an object from a file.
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="path">path to file</param>
        /// <param name="loadedObject">where to put the result</param>
        /// <returns>successful deserialazion</returns>
        public static bool TryDeserialize<T>(string path, out T loadedObject)
        {
            loadedObject = Deserialize<T>(path);
            if (loadedObject == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Tries to read and deserialize an object from a file.
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="path">path to file</param>
        /// <returns>The stored object</returns>
        public static T Deserialize<T>(string path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = null;
            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                T loadedPlayer = (T)formatter.Deserialize(stream);
                return loadedPlayer;
            }
            catch (Exception e)
            {
                Console.WriteLine("Helpers.TrySerialize: " + e.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return default(T);
        }

        /// <summary>
        /// Reads and outputs the integer stored in highscore.txt
        /// </summary>
        /// <returns>the saved highscore</returns>
        internal static int ReadHighScore()
        {
            StreamReader sr = default(StreamReader);
            int result = -1;
            try
            {
                sr = new StreamReader("highscore.txt");
                string lineRead = sr.ReadLine();
                result = int.Parse(lineRead);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            finally
            {
                sr.Close();
            }
            return result;
        }

        /// <summary>
        /// Overwrites the content of highscore.txt with a new score
        /// </summary>
        /// <param name="score"></param>
        internal static void WriteHighScore(int score)
        {
            StreamWriter writer = default(StreamWriter);

            try
            {
                writer = new StreamWriter("highscore.txt", false);
                writer.WriteLine(score);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Scans the save folder for available save names
        /// </summary>
        /// <returns>list of save game paths</returns>
        internal static string[] GetSaveNames()
        {
            try
            {
                return Directory.GetDirectories("saves");
            }
            catch(DirectoryNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// A little helper method to 'do nothing' as a menu option
        /// </summary>
        internal static void ExitMenu()
        {
            return;
        }
    }
}
