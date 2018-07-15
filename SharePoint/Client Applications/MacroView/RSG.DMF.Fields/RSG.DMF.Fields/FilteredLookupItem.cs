using System;
using System.Runtime.CompilerServices;

namespace RSG.DMF.Fields
{
	public class FilteredLookupItem
	{
		public string ID
		{
			get;
			private set;
		}

		public string LookupValue
		{
			get
			{
				return string.Concat(this.ID, ";#", this.Value);
			}
		}

		public string Value
		{
			get;
			private set;
		}

		public FilteredLookupItem(string id, string value)
		{
			this.ID = id;
			this.Value = value;
		}
	}
}