using System;
using System.Reflection;

namespace WorkRequestDataService.Areas.HelpPage.ModelDescriptions
{
#pragma warning disable 1591

    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}