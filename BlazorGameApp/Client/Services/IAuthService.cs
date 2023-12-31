﻿using BlazorGameApp.Shared;

namespace BlazorGameApp.Client.Services
{
    public interface IAuthService 
    {
            Task<ServiceResponse<int>> Register(UserRegister request);

            Task<ServiceResponse<string>> Login(UserLogin request);

        }
}
