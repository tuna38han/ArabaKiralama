using ArabaKirala.API.Models;

namespace ArabaKirala.API.Abstractions.Token;

public interface ITokenHandler
{
    Dtos.Token.Token CreateAccessToken(int second, AppUser user);
}