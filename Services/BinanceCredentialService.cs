using StrzonBinanceTradingBot.Data;
using StrzonBinanceTradingBot.Helpers;

namespace StrzonBinanceTradingBot.Services;

public class BinanceCredentialService : IBinanceCredentialService
{
    private readonly ApplicationDbContext _context;

    private readonly IAES _aes;

    public BinanceCredentialService(ApplicationDbContext context, IAES aes)
    {
        _context = context;
        _aes = aes;
    }

    private BinanceCredentialEntity Encrypt(BinanceCredentialEntity entity)
    {
        var salt = entity.IdentityUserName ?? "";
        if (!string.IsNullOrEmpty(entity.ApiKey))
        {
            entity.ApiKey = _aes.Encrypt(entity.ApiKey, salt);
        }
        if (!string.IsNullOrEmpty(entity.ApiSecret))
        {
            entity.ApiSecret = _aes.Encrypt(entity.ApiSecret, salt);
        }
        return entity;
    }

    private BinanceCredentialEntity Decrypt(BinanceCredentialEntity entity)
    {
        var salt = entity.IdentityUserName ?? "";
        if (!string.IsNullOrEmpty(entity.ApiKey))
        {
            entity.ApiKey = _aes.Decrypt(entity.ApiKey, salt);
        }
        if (!string.IsNullOrEmpty(entity.ApiSecret))
        {
            entity.ApiSecret = _aes.Decrypt(entity.ApiSecret, salt);
        }
        return entity;
    }

    public void Delete(int id)
    {
        var credential = _context.BinanceCredentials?.SingleOrDefault(x => x.Id == id);
        if (credential != null)
        {
            _context.Remove(credential);
            _context.SaveChanges();
        }
    }

    public void Delete(string userName)
    {
        var credential = _context.BinanceCredentials?.SingleOrDefault(x => x.IdentityUserName == userName);
        if (credential != null)
        {
            _context.Remove(credential);
            _context.SaveChanges();
        }
    }

    public BinanceCredentialEntity? Get(int id)
    {
        var credential = _context.BinanceCredentials?.SingleOrDefault(x => x.Id == id);
        if (credential != null)
        {
            credential = Decrypt(credential);
        }
        return credential;
    }

    public BinanceCredentialEntity? Get(string userName)
    {
        var credential = _context.BinanceCredentials?.SingleOrDefault(x => x.IdentityUserName == userName);
        if (credential != null)
        {
            credential = Decrypt(credential);
        }
        return credential;
    }

    public void Insert(string apiKey, string apiSecret, string userName)
    {
        var credential = new BinanceCredentialEntity { IdentityUserName = userName, ApiKey = apiKey, ApiSecret = apiSecret };
        if (credential != null)
        {
            credential = Encrypt(credential);
            _context.BinanceCredentials?.Add(credential);
            _context.SaveChanges();
        }
    }

    public void Update(int id, string apiKey, string apiSecret, string userName)
    {
        var credential = _context.BinanceCredentials?.SingleOrDefault(x => x.Id == id && x.IdentityUserName == userName);
        if (credential != null)
        {
            credential = Decrypt(credential);
            credential.ApiKey = apiKey;
            credential.ApiSecret = apiSecret;
            credential = Encrypt(credential);
            _context.BinanceCredentials?.Update(credential);
            _context.SaveChanges();
        }
    }
}