using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using Microsoft.Data.SqlClient;
using PixelNestBackend.Models;
using CarWebShop.Security;
using PixelNestBackend.Security;
using PixelNestBackend.Responses;
using Azure;

namespace PixelNestBackend.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordEncoder _passwordEncoder;
        private readonly TokenGenerator _tokenGenerator;
        public AuthenticationRepository(
                DataContext context, 
                IConfiguration configuration,
                PasswordEncoder passwordEncoder,
                TokenGenerator tokenGenerator
            
            ) 
        {
            _context = context;
            _configuration = configuration;
            _passwordEncoder = passwordEncoder;
            _tokenGenerator = tokenGenerator;
        }
        public bool Register(User registerDto)
        {
            Console.WriteLine(registerDto.Username);
            _context.Users.Add(registerDto);
            
            return _context.SaveChanges() > 0;

        }
        public bool IsEmailRegistered(User user)
        {
            return _context.Users.Any(u =>u.Email ==  user.Email);
        }
        public bool IsUsernameRegistered(User user)
        {
            return _context.Users.Any(u => u.Username == user.Username);
        }
        public LoginResponse Login(LoginDto loginDto)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string emailQuery = "SELECT * FROM Users WHERE Email = @Email";
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                using(SqlCommand command = new SqlCommand(emailQuery, connection))
                {
                    command.Parameters.AddWithValue("@Email", loginDto.Email);
                    connection.Open();
                   using(SqlDataReader reader = command.ExecuteReader())
                   {
                        if (reader.Read())
                        {
                            string hashedPassword = reader["Password"].ToString();
                            string username = reader["Username"].ToString();
                        
                            bool passwordCheck = _passwordEncoder.VerifyPassword(loginDto.Password, hashedPassword);
                            if(passwordCheck)
                            {
                                string token = _tokenGenerator.GenerateToken(loginDto.Email);


                                reader.Close();
                                reader.Dispose();
                                connection.Close();
                                return new LoginResponse { 
                                    Response = "Succesfull",
                                    Token = token,
                                    Username = username,
                                    IsSuccessful = true
                                    
                                };

                            }
                            reader.Close();
                            reader.Dispose();
                            connection.Close();
                            return new LoginResponse
                            {
                                Response = "Password incorrect!",
                                IsSuccessful = false
                            };
                        }
                        reader.Close();
                        reader.Dispose();
                        connection.Close();
                        return new LoginResponse
                          {
                            Response = "Email incorrect!",
                            IsSuccessful = false
                           
                          };
                    }
                }
            }
        }
    }
}
