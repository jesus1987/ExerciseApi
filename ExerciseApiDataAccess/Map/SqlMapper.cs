using Microsoft.Data.SqlClient;
using System.Data;
namespace ExerciseApiDataAccess.Map;
public static class SqlMapper
{
    public static T MapToObject<T>(IDataReader reader) where T : new()
    {
        T obj = new T();

        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            if (!reader.HasColumn(property.Name) || reader[property.Name] == DBNull.Value)
            {
                continue;
            }

            property.SetValue(obj, Convert.ChangeType(reader[property.Name], property.PropertyType));
        }

        return obj;
    }

    public static bool HasColumn(this IDataReader reader, string columnName)
    {
        for (int i = 0; i < reader.FieldCount; i++)
        {
            if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

}