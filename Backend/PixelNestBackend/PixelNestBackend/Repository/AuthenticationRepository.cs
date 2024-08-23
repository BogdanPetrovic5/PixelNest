using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using Microsoft.Data.SqlClient;
using PixelNestBackend.Models;

namespace PixelNestBackend.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AuthenticationRepository(
                DataContext context, 
                IConfiguration configuration
            ) 
        {
            _context = context;
            _configuration = configuration;
        }
        public bool Register(User registerDto)
        {
            Console.WriteLine(registerDto.Username);
            _context.Users.Add(registerDto);
            
            return _context.SaveChanges() > 0;

        }
        public bool IsRegistered(User user)
        {
            return _context.Users.Any(u =>u.Email ==  user.Email);
        }

  
    }
}
