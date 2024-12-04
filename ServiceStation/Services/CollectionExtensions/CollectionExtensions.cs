using System.Collections.ObjectModel;

namespace ServiceStation.Services.CollectionExtensions;

public static class CollectionExtensions
{
    public static bool Update<T>(this ObservableCollection<T> collection, Predicate<T> predicate, T newValue)
    {
        var index = collection.ToList().FindIndex(predicate);
        
        if (index < 0) return false;
        
        collection[index] = newValue;
        return true;
    }
}