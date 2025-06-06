﻿using PixelNestBackend.Dto;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IAuthenticationService
    {
        RegisterResponse? Register(RegisterDto registerDto);
        string ReturnToken(string userGuid);
        LoginResponse Login(LoginDto loginDto);

    }
}
