using MacroView.DMF.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RSG.DMF.Fields
{
	public class FieldProvider : ICustomFieldFactory
	{
		public FieldProvider()
		{
		}

		public IEnumerable<CustomField> GetFieldTypes()
		{
			List<CustomField> customFields = new List<CustomField>();
			ResourceDictionary resourceDictionaries = new ResourceDictionary()
			{
				Source = new Uri("/RSG.DMF.Fields;component/FieldDataTemplates.xaml", UriKind.Relative)
			};
			IEnumerable<DictionaryEntry> dictionaryEntries = 
				from x in resourceDictionaries.OfType<DictionaryEntry>()
				where (!(x.Key is string) ? false : x.Value is DataTemplate)
				select x;
			foreach (DictionaryEntry dictionaryEntry in dictionaryEntries)
			{
				customFields.Add(new CustomField((string)dictionaryEntry.Key, string.Concat((string)dictionaryEntry.Key, "Template"), (DataTemplate)dictionaryEntry.Value));
			}
			return customFields.AsEnumerable<CustomField>();
		}
	}
}