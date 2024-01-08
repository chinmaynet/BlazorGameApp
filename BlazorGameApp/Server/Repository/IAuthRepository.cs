﻿using BlazorGameApp.Shared;

namespace BlazorGameApp.Server.Repository
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password, int startUnitId);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<bool> UserExists(string email);
    }
}
