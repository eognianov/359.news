namespace NewsSystem.Services.Contracts
{
    public interface ISlugGenerator
    {
        string GenerateSlug(string str);
    }
}