using System.Linq.Expressions;

namespace KafkaWriter.Helpers
{
    public static class ReflectionHelpers
    {
        public static string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
        {
            if (propertyLambda.Body is not MemberExpression me)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return me.Member.Name;
        }
    }
}
