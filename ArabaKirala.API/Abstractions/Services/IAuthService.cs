namespace ArabaKirala.API.Abstractions.Services;

public interface IAuthService
{
    Task<Dtos.Token.Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime);
}