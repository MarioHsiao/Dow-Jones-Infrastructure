
namespace EMG.widgets.ui.encryption
{
    interface ITokenGenerator
    {
        string GenerateToken(string userId, string accountId, string productId, string data1);
    }
}
