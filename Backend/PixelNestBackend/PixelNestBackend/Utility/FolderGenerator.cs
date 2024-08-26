namespace PixelNestBackend.Utility
{
    public class FolderGenerator
    {
       
        public bool CheckIfFolderExists(string folderPath)
        {
            return Directory.Exists(folderPath);
        }
        public void GenerateNewFolder(string folderPath)
        {
            Directory.CreateDirectory(folderPath);
        }
    }
}
