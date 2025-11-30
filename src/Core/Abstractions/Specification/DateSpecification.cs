using System.Linq.Expressions;

namespace Core.Abstractions.Specification;

public class DateIntervalSpecification<T>(
    Expression<Func<T, DateTime?>> keySelector, 
    DateTime? dateStart, DateTime? dateEnd, bool defaultValue = false) : Specification<T>
    where T : class
{
    private readonly Expression<Func<T, DateTime?>> keySelector = keySelector;
    private readonly DateTime? dateStart = dateStart;
    private readonly DateTime? dateEnd = dateEnd;


    public override Expression<Func<T, bool>> ToExpression()
    {
        var fieldName = ((MemberExpression)keySelector.Body).Member.Name;
        var param = Expression.Parameter(typeof(T), fieldName);
       
        var expressionNull = Expression.Constant(null);

        if (dateStart.HasValue && !dateEnd.HasValue)
        {
            var dateFrom = GetStartDate(dateStart.Value);
            var expressionFrom = Expression.Constant(dateFrom);

            var orElse = Expression.OrElse(
                            GreaterThanNulable(Expression.Property(param, fieldName), expressionFrom),
                            Expression.Equal(Expression.Property(param, fieldName), expressionNull));

            return Expression.Lambda<Func<T, bool>>(orElse,param);
        }

        if (dateEnd.HasValue && !dateStart.HasValue)
        {
            var dateTo = GetEndDate(dateEnd.Value);
            
            var expressionTo = Expression.Constant(dateTo);

            var orElse = Expression.OrElse(
                    GreaterThanNulable(expressionTo, Expression.Property(param, fieldName)),
                    Expression.Equal(Expression.Property(param, fieldName), expressionNull));

            return Expression.Lambda<Func<T, bool>>(orElse, param);
        }

        if (dateEnd.HasValue && dateStart.HasValue)
        {
            var dateFrom = GetStartDate(dateStart.Value);
            var expressionFrom = Expression.Constant(dateFrom);

            var dateTo = GetEndDate(dateEnd.Value);
            var expressionTo = Expression.Constant(dateTo);

            var twoDateFilterExpression = Expression.AndAlso(
                    GreaterThanNulable(Expression.Property(param, fieldName), expressionFrom),
                    GreaterThanNulable(expressionTo,Expression.Property(param, fieldName)));

            var orElse = Expression.OrElse(
                    twoDateFilterExpression,
                    Expression.Equal(Expression.Property(param, fieldName), expressionNull));

            return Expression.Lambda<Func<T, bool>>(orElse, param);
        }

        return x => defaultValue;
    }

    private static DateTime GetStartDate(DateTime requestTo) =>
        DateTime.SpecifyKind(requestTo.Date, DateTimeKind.Utc);

    private static DateTime GetEndDate(DateTime requestFrom) =>
        DateTime.SpecifyKind(requestFrom.Date.AddDays(1), DateTimeKind.Utc);

    private static BinaryExpression GreaterThanNulable(Expression left, Expression right)
    {
        if (IsNullableType(left.Type) && !IsNullableType(right.Type))
        {
            right = Expression.Convert(right, left.Type);

            return Expression.GreaterThan(left, right);
        }
        
        if (!IsNullableType(left.Type) && IsNullableType(right.Type))
        {
            left = Expression.Convert(left, right.Type);
        }

        return Expression.GreaterThan(left, right);
    }

    private static bool IsNullableType(Type type) => 
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
}