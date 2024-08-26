using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Data;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Utility;

namespace PixelNestBackend.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;
        private readonly string _basedFolderPath;
        private readonly FolderGenerator _folderGenerator;
        public PostRepository(
            DataContext dataContext,
            FolderGenerator folderGenerator
            )
        {
            _dataContext = dataContext;
            _basedFolderPath = Path.Combine("wwwroot", "Photos");
            _folderGenerator = folderGenerator;
        }
        public bool ShareNewPost(string email)
        {
            string userFolderName = email;
            string userFolderPath = Path.Combine(_basedFolderPath, userFolderName);
            if(!_folderGenerator.CheckIfFolderExists(userFolderPath))
            {
                _folderGenerator.GenerateNewFolder(userFolderPath);
                return true;
            }return false;
            
        }
    }
}
