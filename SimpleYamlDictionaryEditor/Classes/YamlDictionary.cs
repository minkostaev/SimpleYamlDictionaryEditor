namespace SimpleYamlDictionaryEditor.Classes
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    class YamlDictionary
    {

        public static Dictionary<string, string> FileToDictionary(string filePath)
        {
            string key = "k";
            string value = "v";
            try
            {
                Dictionary<string, string> result = new Dictionary<string, string>();

                StreamReader stream = new StreamReader(filePath, Encoding.Default);//UTF7

                while (!stream.EndOfStream)
                {
                    string[] items = stream.ReadLine().Split(':');
                    key = items[0];
                    value = items[1];
                    result.Add(key, value);
                }
                stream.Close();
                return result;
            }
            catch
            {
                Dictionary<string, string> crashDictionary = new Dictionary<string, string>();
                crashDictionary.Add(key, value);
                return crashDictionary;
            }
        }

        public static bool DictionaryToFile(Dictionary<string, string> dictionary, string fileName)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(fileName))
                {
                    foreach (var entry in dictionary)
                    {
                        file.WriteLine("{0}:{1}", entry.Key, entry.Value);
                    }
                }
            }
            catch { return false; }
            return true;
        }

        public static Dictionary<string, string> DictionaryWithFiles(string dirPath)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(dirPath))
            {
                return result;
            }
            foreach (string path in Directory.GetFiles(dirPath))
            {
                string ext = Path.GetExtension(path);
                if (ext == ".yml")
                {
                    result.Add(path, Path.GetFileNameWithoutExtension(path));
                }
            }
            return result;
        }

        public static int ReplaceDictionary(Dictionary<string, string> source, Dictionary<string, string> target)
        {
            int missingKeys = 0;
            foreach (string k in source.Keys)
            {
                if (target.ContainsKey(k))
                {
                    target[k] = source[k];
                }
                else
                {
                    missingKeys = missingKeys + 1;
                }
            }
            return missingKeys;
        }

        public static Dictionary<string, string> CombineDictionary(Dictionary<string, string> source, Dictionary<string, string> target)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var kv in source)
            {
                result.Add(kv.Key, kv.Value);
            }
            foreach (var kv in target)
            {
                if (!result.ContainsKey(kv.Key))
                {
                    result.Add(kv.Key, kv.Value);
                }
            }
            return result;
        }

    }
}
