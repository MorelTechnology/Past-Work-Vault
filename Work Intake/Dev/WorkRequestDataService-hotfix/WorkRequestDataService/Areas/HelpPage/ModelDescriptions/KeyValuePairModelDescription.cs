namespace WorkRequestDataService.Areas.HelpPage.ModelDescriptions
{
#pragma warning disable 1591

    public class KeyValuePairModelDescription : ModelDescription
    {
        public ModelDescription KeyModelDescription { get; set; }

        public ModelDescription ValueModelDescription { get; set; }
    }
}