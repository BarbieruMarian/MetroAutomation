namespace TestFramework.Helper
{
    public class FileManager
    {
        public static void CreateTextFile(string folderPath, string textFileName)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string path = Path.Combine(folderPath, textFileName);
            if (!File.Exists(path))
            {
                File.Create(path);
                using (StreamWriter sw = File.CreateText(path))
                {

                }
            }
        }

        public static string ReadFromNotepad(string path)
        {
            return System.IO.File.ReadAllText(path);
        }
    }
}
