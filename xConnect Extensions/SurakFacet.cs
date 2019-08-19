using Sitecore.XConnect;

namespace MAExtensions.CustomModel
{
    /// <summary>
    /// A custom contact facet.
    /// </summary>
    public class SurakFacet : Facet
    {
        /// <summary>
        /// The default name of the facet.
        /// </summary>
        public static string DefaultFacetName = "SurakFacet";

        /// <summary>
        /// Gets or sets a numeric property.
        /// </summary>
        public int Vesta { get; set; }

        /// <summary>
        /// Gets or sets a string property.
        /// </summary>
        public string Rigel { get; set; }
    }
}
