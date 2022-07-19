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
            }
        }

        public static string ReadFromNotepad(string path)
        {
            return System.IO.File.ReadAllText(path);
        }

        public static void DeleteFilesFromFolder(string path)
        {
            var di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public static string GetMostRecentFileFromFolder(string path)
        {
            var directory = new DirectoryInfo(path);
            var myFile = directory.GetFiles()
             .OrderByDescending(f => f.LastWriteTime)
             .First();

            return myFile.FullName;
        }
    }
}
