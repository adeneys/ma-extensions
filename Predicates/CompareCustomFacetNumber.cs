using System;
using System.Linq.Expressions;
using MAExtensions.CustomModel;
using Sitecore.Framework.Rules;
using Sitecore.XConnect;
using Sitecore.XConnect.Segmentation.Predicates;

namespace MAExtensions.Predicates
{
    /// <summary>
    /// A predicate used in the MA Engine to compare the numeric "Vesta" property of the custom "Surak" facet.
    /// </summary>
    public class CompareCustomFacetNumber : ICondition, IContactSearchQueryFactory
    {
        /// <summary>
        /// Gets or sets the operation to use for the comparison.
        /// </summary>
        public NumericOperationType Comparison { get; set; }

        /// <summary>
        /// Gets or sets the value to compare the facet value to.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Evaluates the condition.
        /// </summary>
        /// <param name="context">The context to perform the evaluation with.</param>
        /// <returns>The outcome of the evaluation.</returns>
        public bool Evaluate(IRuleExecutionContext context)
        {
            var contact = context.Fact<Contact>();

            if(contact == null)
                return false;

            var facet = contact.GetFacet<SurakFacet>(SurakFacet.DefaultFacetName);

            // The facet might be null if it's never been set.
            if(facet == null)
                return false;

            var result = Comparison.Evaluate(facet.Vesta, Value);
            return result;
        }

        /// <summary>
        /// Creates a search query to find contacts matching the predicate.
        /// </summary>
        /// <param name="context">The context in which the query is being created.</param>
        /// <returns>An expression used to search for contacts.</returns>
        public Expression<Func<Contact, bool>> CreateContactSearchQuery(IContactSearchQueryContext context)
        {
            return contact =>
                Comparison.Evaluate(contact.GetFacet<SurakFacet>(SurakFacet.DefaultFacetName).Vesta, Value);
        }
    }
}
