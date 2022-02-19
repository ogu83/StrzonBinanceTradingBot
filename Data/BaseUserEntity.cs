using Microsoft.AspNetCore.Identity;

namespace StrzonBinanceTradingBot.Data;

public abstract class BaseUserEntity : BaseEntity
{
    public string? IdentityUserName { get; set; }
    public IdentityUser? IdentityUser { get; set; }
}