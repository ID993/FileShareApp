namespace Make_a_Drop.Shared.Services
{
    public interface IClaimService
    {
        string GetUserId();

        string GetClaim(string key);
    }
}
