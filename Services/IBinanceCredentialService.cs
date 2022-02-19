using StrzonBinanceTradingBot.Data;

namespace StrzonBinanceTradingBot.Services;

public interface IBinanceCredentialService
{
    void Insert(string apiKey, string apiSecret, string userName);
    void Update(int id, string apiKey, string apiSecret, string userName);
    void Delete(int id);
    void Delete(string userName);
    BinanceCredentialEntity? Get(int id);
    BinanceCredentialEntity? Get(string userName);
}