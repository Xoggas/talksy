namespace Talksy.Api.Services;

public interface IPasswordHashService
{
    string GetPasswordHash(string password);
    bool IsPasswordCorrect(string password, string hashedPassword);
}

public class BCryptPasswordHashService : IPasswordHashService
{
    public string GetPasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 10);
    }
    
    public bool IsPasswordCorrect(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}