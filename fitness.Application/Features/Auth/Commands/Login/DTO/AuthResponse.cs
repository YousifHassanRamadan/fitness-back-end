using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace fitness.Application.Features.Auth.Commands.Login.DTO;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    [JsonIgnore]
    public string RefreshToken { get; set; } = string.Empty;
}
