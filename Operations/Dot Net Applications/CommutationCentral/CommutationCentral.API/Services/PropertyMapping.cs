using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Services
{
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            _mappingDictionary = mappingDictionary;
        }

        public Dictionary<string, PropertyMappingValue> _mappingDictionary { get; set; }
    }
}
