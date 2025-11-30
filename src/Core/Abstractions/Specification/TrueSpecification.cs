using System.Linq.Expressions;

namespace Core.Abstractions.Specification;

public class TrueSpecification<T> : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        return x => true;
    }
}
