﻿using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;

namespace PixelNestBackend.Gateaway
{
    public class FileUpload : IFileUpload
    {
        private readonly DataContext _dataContext;

        public FileUpload(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> StoreImages(PostDto? postDto, StoryDto? storyDto, ProfileDto? profileDto, string userFolder, Guid? folder, Guid? userID)
        {
            try
            {
                if (postDto != null)
                {
                    await _storePostImages(postDto, userFolder, folder);
                }
                else if (storyDto != null)
                {
                    await _storeStoryImage(storyDto, userFolder, folder);
                }
                else if(postDto == null && storyDto == null && profileDto == null && folder == null)
                {
                    await _storeGooglePath(userFolder, userID);
                }
                else
                {
                    await _storeProfileImage(userID, profileDto.ProfilePicture, userFolder);
                }

                return true;
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error storing images: {ex.Message}");
                return false;
            }
        }
        private async Task _storeGooglePath(string userFolder, Guid? userID)
        {
            try
            {
                var existingImagePath = await _dataContext.ImagePaths.Where(u => u.UserGuid == userID).FirstOrDefaultAsync();
                if (existingImagePath == null)
                {
                    var newImagePath = new ImagePath
                    {
                        UserGuid = userID,
                        Path = userFolder,
                        PhotoDisplay = "cover"
                    };
                    _dataContext.ImagePaths.Add(newImagePath);
                }
                else
                {
                    existingImagePath.Path = userFolder;
                    _dataContext.ImagePaths.Update(existingImagePath);
                }
               
                _dataContext.SaveChanges();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private async Task _storePostImages(PostDto postDto, string userFolder, Guid? folder)
        {
            string postFolderPath = _createFolder(userFolder, folder.ToString());

            foreach (var formFile in postDto.Photos)
            {
                if (formFile != null && formFile.Length > 0)
                {
                    string filePath = await _saveFileAsync(formFile, postFolderPath);
                    string relativePath = filePath.Replace("wwwroot\\Photos\\", string.Empty).Trim();
                    var imagePath = new ImagePath
                    {
                        PostGuid = folder,
                        PhotoDisplay = postDto.PhotoDisplay,
                        Path = relativePath
                    };

                    _dataContext.ImagePaths.Add(imagePath);
                }
            }

            await _dataContext.SaveChangesAsync();
        }
        private async Task _storeProfileImage(Guid? userID, IFormFile profilePicture, string userFolder)
        {
           if(profilePicture != null)
           {
                string filePath = await _saveFileAsync(profilePicture, userFolder);

               
                var existingImagePath = await _dataContext.ImagePaths
                    .FirstOrDefaultAsync(ip => ip.UserGuid == userID);
                string relativePath = filePath.Replace("wwwroot\\Photos\\", string.Empty).Trim();
                if (existingImagePath != null)
                {

                    existingImagePath.Path = relativePath;
                    existingImagePath.PhotoDisplay = "cover";
                    _dataContext.ImagePaths.Update(existingImagePath);
                }
                else
                {
                   
                    var newImagePath = new ImagePath
                    {
                        UserGuid = userID,
                        Path = relativePath,
                        PhotoDisplay = "cover"
                    };
                    await _dataContext.ImagePaths.AddAsync(newImagePath);
                }

              
                await _dataContext.SaveChangesAsync(); ;     
           }

        }
        private async Task _storeStoryImage(StoryDto storyDto, string userFolder, Guid? folder)
        {
            if (storyDto.StoryImage != null)
            {
                string storyFolderPath = _createFolder(userFolder, folder.ToString());
                string filePath = await _saveFileAsync(storyDto.StoryImage, storyFolderPath);
                string relativePath = filePath.Replace("wwwroot\\Photos\\", string.Empty).Trim();
                var imagePath = new ImagePath
                {
                    StoryGuid = folder,
                    PhotoDisplay = storyDto.PhotoDisplay,
                    Path = relativePath
                };

                _dataContext.ImagePaths.Add(imagePath);
                await _dataContext.SaveChangesAsync();
            }
        }

        private string _createFolder(string baseFolder, string folderName)
        {
            string folderPath = Path.Combine(baseFolder, folderName);
            Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        private async Task<string> _saveFileAsync(IFormFile formFile, string folderPath)
        {
            string filePath = Path.Combine(folderPath, formFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}

