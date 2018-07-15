using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using System.Collections.Generic;

namespace Report.Layouts.Report
{
    public partial class ReservesPnote : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            List<SPListItem> matchingItems = new List<SPListItem>();
            using (var parentWeb = new SPSite("https://rivernet2ndev.trg.com/sites/litman/Matters").OpenWeb())
            {
                foreach (SPWeb web in parentWeb.Webs)
                {
                    SPList list = web.Lists.TryGetList("Pnotes");
                    try
                    {
                        list.Fields.GetField("Body");
                        list.Fields.GetField("Title");
                    }
                    catch { continue; }
                    
                    if (list != null)
                    {

                        SPQuery query = new SPQuery
                        {
                            Query = @"<Where><Geq><FieldRef Name='Modified' /><Value Type='DateTime'><Today Offset='-180' /></Value></Geq></Where>"
                        };

                        foreach (SPListItem item in list.GetItems(query))
                            matchingItems.Add(item);
                    }
                }
            }

            //List<object> tableItems = new List<object>();
            //foreach (var item in matchingItems)
            //{
            //    var displayItem =
            //        new
            //        {
            //            Item = item["Title"],
            //            Matter = item.Web.Name,
            //            Created = item["Created"],
            //            Modified = item["Modified"],
            //            Author = item["Created By"],
            //            webName = item["Body"] 
            //        };
            //    tableItems.Add(displayItem);
            //}
            //if (tableItems.Count == 0)
            //    tableItems.Add(new { Result = "No results were found." });
            //Results.DataSource = tableItems;
            Results.DataSource = matchingItems;
            Results.DataBind();
        }

    }
}
