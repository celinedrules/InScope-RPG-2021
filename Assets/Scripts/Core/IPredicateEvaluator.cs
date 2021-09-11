namespace Core
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(Predicates predicate, string parameter);
    }
}