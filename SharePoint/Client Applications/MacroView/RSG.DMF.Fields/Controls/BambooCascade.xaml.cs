using MacroView.DMF.Interop;
using MacroView.DMF.Interop.Types;
using Microsoft.SharePoint.Client;
using RSG.DMF.Fields;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace RSG.DMF.Fields.Controls
{
	public partial class BambooCascade : UserControl
	{
		private bool _isInitialised;

		private FieldValue ParentFieldValue;

		private bool _cascading;

		private string _lookupListName;

		private string _lookupSiteURL;

		private string _lookupColumn;

		private string _filterColumn;

		private string _parentLookupColumn;

		private Guid _lookupListGD;

		private string _valueFronSharePoint;

		private ObservableCollection<FilteredLookupItem> LookupValues;

		private ClientContext _ClientContext;

		public CustomFieldElementContext FieldContext
		{
			get;
			private set;
		}

		public bool SetDefaults
		{
			get;
			set;
		}

		public BambooCascade()
		{
			this.InitializeComponent();
			this._isInitialised = false;
			this.FieldContext = new CustomFieldElementContext(this);
			this.FieldContext.Validating += new EventHandler<CustomFieldValidatingEventArgs>(this.FieldContext_Validating);
			this.FieldContext.Loaded += new EventHandler(this.FieldContext_Loaded);
			this.LookupValues = new ObservableCollection<FilteredLookupItem>();
			this.BambooLookup.IsTextSearchEnabled = false;
			this.BambooLookup.IsEditable = false;
			this.BambooLookup.ItemsSource = this.LookupValues;
			this.BambooLookup.DisplayMemberPath = "Value";
			this.BambooLookup.SelectedValuePath = "LookupValue";
		}

		private void BambooCascade_Loaded(object sender, RoutedEventArgs e)
		{
			if (!this.FieldContext.Field.Required)
			{
				this.Asterisk.Visibility = System.Windows.Visibility.Hidden;
			}
			else
			{
				this.Asterisk.Visibility = System.Windows.Visibility.Visible;
			}
		}

		private void FieldContext_Loaded(object sender, EventArgs e)
		{
			if (!this._isInitialised)
			{
                this.ColumnDisplayName.Text = this.FieldContext.Field.DisplayName;
                string selectedValue = Convert.ToString(this.BambooLookup.SelectedValue);
                this._cascading = Convert.ToBoolean(this.FieldContext.Field.GetCustomProperty("enableRelationship"));
                this._lookupListName = this.FieldContext.Field.GetCustomProperty("listName");
                this._lookupSiteURL = this.FieldContext.Field.GetCustomProperty("siteURL");
                this._lookupColumn = this.FieldContext.Field.GetCustomProperty("displayColumn");
                this._filterColumn = Convert.ToString(this.FieldContext.Field.GetCustomProperty("filterColumn"));
                this._parentLookupColumn = Convert.ToString(this.FieldContext.Field.GetCustomProperty("parentColumn"));
                this._lookupListGD = new Guid(this.FieldContext.Field.GetCustomProperty("list"));
                this._ClientContext = new ClientContext(this.FieldContext.List.Url.Substring(0, this.FieldContext.List.Url.LastIndexOf("/")));
                this._ClientContext.ExecuteQuery();
                Web web = this._ClientContext.Web;
                ListCollection lists = web.Lists;
                List byTitle = lists.GetByTitle(this._lookupListName);
                this._ClientContext.Load<Web>(web, new Expression<Func<Web, object>>[0]);
                this._ClientContext.Load<ListCollection>(lists, new Expression<Func<ListCollection, object>>[0]);
                this._ClientContext.Load<List>(byTitle, new Expression<Func<List, object>>[0]);
                this._ClientContext.Load<FieldCollection>(byTitle.Fields, new Expression<Func<FieldCollection, object>>[0]);
                this._ClientContext.ExecuteQuery();
                if (this._cascading)
                {
                    this.ParentFieldValue = this.FieldContext.FieldValues.FirstOrDefault((FieldValue f) => f.Field.InternalName == this._parentLookupColumn);
                    this.ParentFieldValue.PropertyChanged += new PropertyChangedEventHandler(this.ParentFieldValue_PropertyChanged);
                    this.GetFilteredValues(byTitle);
                }
                else
                {
                    this.GetLookupValues(byTitle);
                }
                this.BambooLookup.SelectedValue = selectedValue;
                this._isInitialised = true;
			}
		}

		private void FieldContext_Validating(object sender, CustomFieldValidatingEventArgs e)
		{
            if (!this.FieldContext.Field.Required)
            {
                e.IsValid = true;
                return;
            }
            if (!string.IsNullOrEmpty(Convert.ToString(this.BambooLookup.SelectedValue)))
            {
                e.IsValid = true;
                return;
            }
            e.IsValid = false;
            e.Message = "Please select a value.";
		}

		private void GetFilteredValues(List lookupList)
		{
			this.LookupValues.Clear();
			if (!string.IsNullOrEmpty(this.ParentFieldValue.DisplayValue))
			{
                string text = this.ParentFieldValue.DisplayValue;
                text = text.Substring(text.IndexOf('#') + 1, text.Length - text.IndexOf('#') - 1);
                Microsoft.SharePoint.Client.Field byInternalNameOrTitle = lookupList.Fields.GetByInternalNameOrTitle(this._lookupColumn);
                Microsoft.SharePoint.Client.Field byInternalNameOrTitle2 = lookupList.Fields.GetByInternalNameOrTitle(this._filterColumn);
                this._ClientContext.Load<Microsoft.SharePoint.Client.Field>(byInternalNameOrTitle, new Expression<Func<Microsoft.SharePoint.Client.Field, object>>[0]);
                this._ClientContext.Load<Microsoft.SharePoint.Client.Field>(byInternalNameOrTitle2, new Expression<Func<Microsoft.SharePoint.Client.Field, object>>[0]);
                this._ClientContext.ExecuteQuery();
                ListItemCollection items = lookupList.GetItems(new CamlQuery
                {
                    ViewXml = string.Concat(new string[]
					{
						"<View> <Query> <Where> <Eq> <FieldRef Name='",
						this._filterColumn,
						"' /> <Value Type='Text'>",
						text,
						"</Value> </Eq> </Where> </Query> </View>"
					})
                });
                this._ClientContext.Load<ListItemCollection>(items, new Expression<Func<ListItemCollection, object>>[]
				{
					(ListItemCollection li) => li.ListItemCollectionPosition,
					(ListItemCollection li) => li.IncludeWithDefaultProperties(new Expression<Func<ListItem, object>>[]
					{
						(ListItem it) => it.DisplayName,
						(ListItem it) => it.FieldValuesAsText,
						(ListItem it) => it[this._lookupColumn],
						(ListItem it) => it[this._filterColumn],
						(ListItem it) => (object)it.Id
					})
				});
                this._ClientContext.ExecuteQuery();
                foreach (ListItem current in (IEnumerable<ListItem>)items)
                {
                    this.LookupValues.Add(new FilteredLookupItem(current.Id.ToString(), Convert.ToString(current[this._lookupColumn])));
                }
			}
		}

		private void GetLookupValues(List lookupList)
		{
            this.LookupValues.Clear();
            Microsoft.SharePoint.Client.Field byInternalNameOrTitle = lookupList.Fields.GetByInternalNameOrTitle(this._lookupColumn);
            this._ClientContext.Load<Microsoft.SharePoint.Client.Field>(byInternalNameOrTitle, new Expression<Func<Microsoft.SharePoint.Client.Field, object>>[0]);
            this._ClientContext.ExecuteQuery();
            ListItemCollection items = lookupList.GetItems(new CamlQuery
            {
                ViewXml = "<View/>"
            });
            this._ClientContext.Load<ListItemCollection>(items, new Expression<Func<ListItemCollection, object>>[]
			{
				(ListItemCollection li) => li.ListItemCollectionPosition,
				(ListItemCollection li) => li.IncludeWithDefaultProperties(new Expression<Func<ListItem, object>>[]
				{
					(ListItem it) => it.DisplayName,
					(ListItem it) => it.FieldValuesAsText,
					(ListItem it) => it[this._lookupColumn],
					(ListItem it) => (object)it.Id
				})
			});
            this._ClientContext.ExecuteQuery();
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (ListItem current in (IEnumerable<ListItem>)items)
            {
                string text = Convert.ToString(current[this._lookupColumn]);
                if (!dictionary.Keys.Contains(text))
                {
                    dictionary.Add(text, current.Id);
                }
            }
            foreach (string current2 in dictionary.Keys)
            {
                this.LookupValues.Add(new FilteredLookupItem(dictionary[current2].ToString(), current2));
            }
		}

		private void ParentFieldValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
            if (string.Equals(e.PropertyName, "DisplayValue", StringComparison.InvariantCultureIgnoreCase))
            {
                Web web = this._ClientContext.Web;
                ListCollection lists = web.Lists;
                List byTitle = lists.GetByTitle(this._lookupListName);
                this._ClientContext.Load<Web>(web, new Expression<Func<Web, object>>[0]);
                this._ClientContext.Load<ListCollection>(lists, new Expression<Func<ListCollection, object>>[0]);
                this._ClientContext.Load<List>(byTitle, new Expression<Func<List, object>>[0]);
                this._ClientContext.Load<FieldCollection>(byTitle.Fields, new Expression<Func<FieldCollection, object>>[0]);
                this._ClientContext.ExecuteQuery();
                this.GetFilteredValues(byTitle);
            }
		}
	}
}