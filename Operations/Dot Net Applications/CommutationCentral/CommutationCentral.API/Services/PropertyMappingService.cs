using CommutationCentral.API.Entities;
using CommutationCentral.API.Entities.Lookups;
using CommutationCentral.API.Models.Lookups;
using CommutationCentral.API.Models.Roles;
using CommutationCentral.API.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<ActivityCategoryDTO, ActivityCategory>(_activityCategoryPropertyMapping));
            propertyMappings.Add(new PropertyMapping<UserDTO, ApplicationUser>(_userPropertyMapping));
            propertyMappings.Add(new PropertyMapping<ApplicationRoleDTO, ApplicationRole>(_rolePropertyMapping));
        }

        private Dictionary<string, PropertyMappingValue> _activityCategoryPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id" }) },
                {"Name", new PropertyMappingValue(new List<string>() {"Name" }) }
            };
        private Dictionary<string, PropertyMappingValue> _userPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string> {"Id" }) },
                {"Email", new PropertyMappingValue(new List<string> {"Email" }) },
                {"UserName", new PropertyMappingValue(new List<string> {"UserName" }) }
            };
        private Dictionary<string, PropertyMappingValue> _rolePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string> {"Id" }) },
                {"Name", new PropertyMappingValue(new List<string> {"Name" }) }
            };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)}, {typeof(TDestination)}>.");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields
                // are coming from an orderBy string, this part must be
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
