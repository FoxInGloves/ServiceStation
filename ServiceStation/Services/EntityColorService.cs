namespace ServiceStation.Services;

public class EntityColorService
{
    public static string GetStatusColor(bool status)
    {
        return status ? "#02b83e" : "#f52020";
    }
}