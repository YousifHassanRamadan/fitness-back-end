using System;
using System.Collections.Generic;
using System.Text;

namespace fitness.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(string userId, string email);
    string GenerateRefreshToken();

    string HashRefreshToken(string refreshToken);
}
