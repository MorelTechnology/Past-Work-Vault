using System.Collections.ObjectModel;

namespace WorkRequestDataService.Areas.HelpPage.ModelDescriptions
{
#pragma warning disable 1591

    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values { get; private set; }
    }
}