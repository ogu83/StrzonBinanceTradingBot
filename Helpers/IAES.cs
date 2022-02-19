namespace StrzonBinanceTradingBot.Helpers;

public interface IAES
{
    string Encrypt(string text, string salt);
    string Decrypt(string text, string salt);
}
