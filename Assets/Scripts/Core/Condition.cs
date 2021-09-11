using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    public enum Predicates
    {
        None,
        HasQuest,
        CompletedQuest,
        HasInventoryItem
    }

    [Serializable]
    public class Condition
    {
        [LabelText("Requirements")]
        [SerializeField] private Disjunction[] and;

        public Disjunction[] AND
        {
            get => and;
            set => and = value;
        }

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators) =>
            AND.All(disjunction => disjunction.Check(evaluators));

        [Serializable]
        public class Disjunction
        {
            [LabelText("@GetLabel()")]
            [SerializeField] private Predicate[] or;

            public Predicate[] Or
            {
                get => or;
                set => or = value;
            }

            private string GetLabel()
            {
                if (or.Length == 1)
                    return "This is required";

                return "One of these is required";
            }

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators) =>
                Or.Any(predicate => predicate.Check(evaluators));
        }

        [Serializable]
        public class Predicate
        {
            [SerializeField] private Predicates conditionPredicate;
            [SerializeField] private string parameter;
            [SerializeField] private bool negate;

            public Predicates ConditionPredicate
            {
                get => conditionPredicate;
                set => conditionPredicate = value;
            }

            public string Parameter
            {
                get => parameter;
                set => parameter = value;
            }

            public bool Negate
            {
                get => negate;
                set => negate = value;
            }

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(conditionPredicate, parameter);

                    if (result == null)
                        continue;

                    if (result == Negate)
                        return false;
                }

                return true;
            }
        }
    }
}