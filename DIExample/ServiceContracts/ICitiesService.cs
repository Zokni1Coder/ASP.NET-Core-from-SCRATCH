namespace ServiceContracts
{
    public interface ICitiesService
    {
        List<string> GetCities();

        Guid GetGuid();
    }
}
