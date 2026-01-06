using System;
using System.Collections.Generic;
using System.Text;

namespace RewardSystem_Application.Interfaces.Auth
{
    
        // Authentication operations used by API layer (login / register).
        public interface IAuthService
        {
            // Authenticate a user and return JWT token.
            Task<string> LoginAsync(string email, string password, CancellationToken ct = default);

            // Register a new user and optionally return a token.
            Task<string> RegisterAsync(string name, string email, string employeeId, string password, string role = "User", CancellationToken ct = default);
        }
    }
