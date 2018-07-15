using System.Collections.ObjectModel;

namespace WorkRequestDataService.Areas.HelpPage.ModelDescriptions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public class ComplexTypeModelDescription : ModelDescription
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public ComplexTypeModelDescription()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Properties = new Collection<ParameterDescription>();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Collection<ParameterDescription> Properties { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}