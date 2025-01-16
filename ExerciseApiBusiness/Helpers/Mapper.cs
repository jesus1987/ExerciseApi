
using System.Reflection;

namespace ExerciseApiBusiness.Helpers;
public class Mapper
{
    public static TTarget MapProperties<TSource, TTarget>(TSource source) where TTarget : new()
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        TTarget target = new TTarget();

        PropertyInfo[] sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        PropertyInfo[] targetProperties = typeof(TTarget).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var sourceProperty in sourceProperties)
        {
            var targetProperty = targetProperties.FirstOrDefault(tp => tp.Name == sourceProperty.Name
                                                                       && tp.PropertyType == sourceProperty.PropertyType);

            if (targetProperty != null && targetProperty.CanWrite)
            {
                var value = sourceProperty.GetValue(source);
                targetProperty.SetValue(target, value);
            }
        }
        return target;
    }
}
