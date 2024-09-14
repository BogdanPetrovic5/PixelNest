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
            try
            {
                _context.Users.Add(registerDto);

                return _context.SaveChanges() > 0;
            }catch(SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
           

        }

        public string ReturnToken(string tokenParameter)
        {
            return _tokenGenerator.GenerateToken(tokenParameter);
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

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(emailQuery, connection))
            {
                command.Parameters.AddWithValue("@Email", loginDto.Email);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return new LoginResponse
                        {
                            Response = "Email incorrect!",
                            IsSuccessful = false
                        };
                    }

                    string hashedPassword = reader["Password"].ToString();
                    string username = reader["Username"].ToString();
                    string email = reader["Email"].ToString();
                    bool passwordCheck = _passwordEncoder.VerifyPassword(loginDto.Password, hashedPassword);

                    return passwordCheck
                        ? new LoginResponse
                        {
                            Response = "Successful",
                            Username = username,
                            Email = email,
                            IsSuccessful = true
                        }
                        : new LoginResponse
                        {
                            Response = "Password incorrect!",
                            IsSuccessful = false
                        };
                }
            }
        }

    }
}
