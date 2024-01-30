using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Adirev.Model;

namespace Adirev.Service
{
    public class FileManager
    {
        #region Public Methods
        public static void SaveFileSQL(string path, string file, string contents, DatabaseManager.TypeScript type)
        {
            file = DeleteInvalidFileNameChars(file);
            try
            {
                if (!IsFileInUse($@"{path}\{DatabaseManager.GetNameTypeScript(type)}\{file}.sql"))
                { System.IO.File.WriteAllText($@"{path}\{DatabaseManager.GetNameTypeScript(type)}\{file}.sql", contents); }
                else
                {
                    Thread.Sleep(30000);
                    System.IO.File.WriteAllText($@"{path}\{DatabaseManager.GetNameTypeScript(type)}\{file}.sql", contents);
                }
            }
            catch (Exception ex)
            { Logger.SaveError(ex.Message, ex.InnerException?.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " -> " + System.Reflection.MethodBase.GetCurrentMethod()); }
        }

        public static void SaveFile(string path, string file, string extension, string contents)
        {
            file = DeleteInvalidFileNameChars(file);
            try
            {
                if (!IsFileInUse($@"{path}\{file}.{extension}"))
                { System.IO.File.WriteAllText($@"{path}\{file}.{extension}", contents); }
                else
                {
                    Thread.Sleep(30000);
                    System.IO.File.WriteAllText($@"{path}\{file}.{extension}", contents);
                }
            }
            catch (Exception ex)
            { Logger.SaveError(ex.Message, ex.InnerException?.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " -> " + System.Reflection.MethodBase.GetCurrentMethod()); }
        }

        public static void CreateDirectory(string path, string directory)
        {
            directory = DeleteInvalidFileNameChars(directory);
            try
            {
                System.IO.Directory.CreateDirectory($@"{path}\{directory}");
            }
            catch (Exception ex)
            { Logger.SaveError(ex.Message, ex.InnerException?.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " -> " + System.Reflection.MethodBase.GetCurrentMethod()); }
        }

        public static void DeleteFile(string path)
        {
            try
            {
                if (!IsFileInUse(path))
                {
                    if (File.Exists(path))
                    { File.Delete(path); }
                }
                else
                {
                    Thread.Sleep(30000);
                    if (File.Exists(path))
                    { File.Delete(path); }
                }
            }
            catch (Exception ex)
            { Logger.SaveError(ex.Message, ex.InnerException?.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " -> " + System.Reflection.MethodBase.GetCurrentMethod()); }
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
            { Logger.SaveError(ex.Message, ex.InnerException?.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " -> " + System.Reflection.MethodBase.GetCurrentMethod()); }
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
            { Logger.SaveError(ex.Message, ex.InnerException?.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " -> " + System.Reflection.MethodBase.GetCurrentMethod()); }
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
                Logger.SaveError(ex.Message, ex.InnerException?.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " -> " + System.Reflection.MethodBase.GetCurrentMethod());
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

        public static List<string> GetFiles(string path)
        {
            List<string> list = new List<string>();
            try
            {
                string[] files = Directory.GetFiles(path);

                foreach (var item in files)
                {
                    list.Add(item.Replace(@$"{path}\", "").Replace(".sql", ""));
                }
            }
            catch (Exception ex)
            { Logger.SaveError(ex.Message, ex.InnerException?.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " -> " + System.Reflection.MethodBase.GetCurrentMethod()); }

            return list;
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
            { Logger.SaveError(ex.Message, ex.InnerException?.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + " -> " + System.Reflection.MethodBase.GetCurrentMethod()); }

            return list;
        }

        public static string DeleteInvalidFileNameChars(string text)
        {
            string returnText = text;

            foreach (var c in System.IO.Path.GetInvalidFileNameChars())
            {
                returnText = returnText.Replace(c.ToString(), "-");
            }

            return returnText;
        }
        static bool IsFileInUse(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    // The file is not in use by another process
                    return false;
                }
            }
            catch (IOException)
            {
                // The file is in use by another process
                return true;
            }
        }
        #endregion
    }
}
