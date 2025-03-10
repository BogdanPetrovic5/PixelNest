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
using PixelNestBackend.Utility;

namespace PixelNestBackend.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordEncoder _passwordEncoder;
        private readonly TokenGenerator _tokenGenerator;
        private readonly ILogger<AuthenticationRepository> _logger;
        private readonly UserUtility _userUtility;
        public AuthenticationRepository(
                DataContext context,
                IConfiguration configuration,
                PasswordEncoder passwordEncoder,
                TokenGenerator tokenGenerator,
                ILogger<AuthenticationRepository> logger,
                UserUtility userUtility
                

            ) 
        {
            _context = context;
            _configuration = configuration;
            _passwordEncoder = passwordEncoder;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
            _userUtility = userUtility;
        }
        public bool Register(User user)
        {
            try
            {
                _context.Users.Add(user);

                return _context.SaveChanges() > 0;
            }catch(SqlException ex)
            {
                _logger.LogError($"Database error: {ex.Message}");
                return false;
            }catch(Exception ex)
            {
                _logger.LogError($"General error: {ex.Message}");
                return false;
            }
           

        }

        public string ReturnToken(string tokenParameter)
        {
            //string username = _userUtility.GetUserName(tokenParameter);
            //Guid userGUID = _userUtility.GetUserID(username);
            //string userID = userGUID.ToString();
            return _tokenGenerator.GenerateToken(tokenParameter);
        }

        public bool IsEmailRegistered(User user)
        {
            try
            {
                return _context.Users.Any(u => u.Email == user.Email);
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Database error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"General error: {ex.Message}");
                return false;
            }

        }
        public bool IsUsernameRegistered(User user)
        {

            try
            {
                return _context.Users.Any(u => u.Username == user.Username);
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Database error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"General error: {ex.Message}");
                return false;
            }

        }
        public LoginResponse? Login(LoginDto loginDto)
        {
            try
            {
                string? connectionString = _configuration.GetConnectionString("DefaultConnection");
                string emailQuery = "SELECT * FROM Users WHERE Email = @Email";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
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
                                    Response = "Credentials incorrect!",
                                    IsSuccessful = false
                                };
                            }

                            string? hashedPassword = reader["Password"].ToString();
                            string? username = reader["Username"].ToString();
                            string? email = reader["Email"].ToString();
                            string? userID = reader["ClientGuid"].ToString();
                            bool passwordCheck = _passwordEncoder.VerifyPassword(loginDto.Password, hashedPassword);

                            return passwordCheck
                                ? new LoginResponse
                                {
                                    Response = "Successful",
                                    Username = username,
                                    Email = email,
                                    IsSuccessful = true,
                                    ClientGuid = userID
                                }
                                : new LoginResponse
                                {
                                    Response = "Credentials incorrect!",
                                    IsSuccessful = false
                                };
                        }
                    }
                }
            }catch(SqlException ex)
            {
                _logger.LogError($"Database error: {ex.Message}");
                return null;
            }catch(Exception ex)
            {
                _logger.LogError($"General error: {ex.Message}");
                return null;
            }
            
                
        }

    }
}
