using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Adirev.Model;

namespace Adirev.Service
{
    public class FileManager
    {
        #region Public Methods
        public static void SaveFileSQL(string path, string file, string contents, DatabaseManager.TypeScript type)
        {
            try
            {
                System.IO.File.WriteAllText($@"{path}\{GetFolderScript(type)}\{file}.sql", contents);
            }
            catch (Exception ex)
            { }
        }

        public static void CreateDirectory(string path, string directory)
        {
            try
            {
                System.IO.Directory.CreateDirectory($@"{path}\{directory}");
            }
            catch (Exception ex)
            { }
        }

        public static void DeleteDirectory(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);

                if (di.Exists == true)
                { di.Delete(true); }
            }
            catch (Exception ex)
            { }
        }

        public static void WriteToBinaryFile<T>(string filePath, T objetToWrite, bool append = false)
        {
            try
            {
                using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(stream, objetToWrite);
                }
            }
            catch (Exception ex)
            { }
        }

        public static T ReadFromBinaryFile<T>(string filePath)
        {
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return (T)binaryFormatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

        public static List<string> GetDirectories(string directoriesPath)
        {
            List<string> list = new List<string>();
            try
            {
                var directories = Directory.GetDirectories(directoriesPath);

                foreach (var item in directories)
                {
                    list.Add(item.Replace(@$"{directoriesPath}\", ""));
                }
            }
            catch (Exception ex)
            { }

            return list;
        }

        #endregion

        #region Private Methods
        private static string GetFolderScript(DatabaseManager.TypeScript type)
        {
            switch (type)
            {
                case DatabaseManager.TypeScript.FN:
                    return "Functions";
                case DatabaseManager.TypeScript.TR:
                    return "Triggers";
                case DatabaseManager.TypeScript.P:
                    return "Procedures";
                case DatabaseManager.TypeScript.V:
                    return "Views";
                case DatabaseManager.TypeScript.U:
                    return "Tables";
                default:
                    return "X";
            }
        }

        #endregion
    }
}
