using MAExtensions.CustomModel;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.Xdb.MarketingAutomation.Core.Activity;
using Sitecore.Xdb.MarketingAutomation.Core.Processing.Plan;

namespace MAExtensions.MAEngineExtensions
{
    /// <summary>
    /// A custom Marketing Automation activity which sets a custom contact facet.
    /// </summary>
    public class SetCustomFacet : IActivity
    {
        /// <summary>
        /// Gets or sets the services available to the activity.
        /// </summary>
        public IActivityServices Services { get; set; }

        /// <summary>
        /// Gets or sets the value to set the Rigel property to.
        /// </summary>
        public string Rigel { get; set; }

        /// <summary>
        /// Invokes the activity.
        /// </summary>
        /// <param name="context">The context for the activity invocation.</param>
        /// <returns>The result of the invocation.</returns>
        public ActivityResult Invoke(IContactProcessingContext context)
        {
            var facet = context.Contact.GetFacet<SurakFacet>(SurakFacet.DefaultFacetName);

            if (facet == null)
                facet = new SurakFacet();

            facet.Rigel = Rigel;

            Services.Collection.SetFacet(context.Contact, facet);
            Services.Collection.Submit();

            return new SuccessMove();
        }
    }
}
