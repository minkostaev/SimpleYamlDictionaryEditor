namespace SimpleYamlDictionaryEditor.Classes
{
    using System.Text;
    using System.IO;

    public class DirectorySelection
    {
        private string FileName => "dir.txt";
        public string SelectedDirectory { get; set; }

        public void WriteFile()
        {
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, FileName);
            if (!Directory.Exists(SelectedDirectory))
            {
                SelectedDirectory = dir;
            }
            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(SelectedDirectory);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
            catch {; }
        }
        public void ReadFile()
        {
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, FileName);
            if (!File.Exists(path))
            {
                SelectedDirectory = dir;
                return;
            }
            try
            {
                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        SelectedDirectory = s;
                    }
                }
            }
            catch {; }
            if (!Directory.Exists(SelectedDirectory))
            {
                SelectedDirectory = dir;
            }
        }

    }
}
