using System.Security.Authentication;
using ArabaKirala.API.Abstractions.Token;
using ArabaKirala.API.Models;
using Microsoft.AspNetCore.Identity;

namespace ArabaKirala.API.Abstractions.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenHandler _tokenHandler;

    public AuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _signInManager = signInManager;
    }

    public async Task<Dtos.Token.Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
    {
        AppUser user = await _userManager.FindByNameAsync(usernameOrEmail) ?? 
                       await _userManager.FindByEmailAsync(usernameOrEmail);

        if (user == null)
            throw new AuthenticationException("Invalid username or password.");

        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (result.Succeeded)
        {
            Dtos.Token.Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
            return token;
        }

        throw new AuthenticationException("Invalid username or password.");
    }
}