using System;

public class CopyFiletoSharepoint
{
   public CopyFiletoSharepoint()
    {
       try
       {
           SPSite site = new SPSite("https://rivernetqa.trg.com");
           SPWeb web = site.OpenWeb();
           web.AllowUnsafeUpdated = true;
           SPFolder folder = web.Folders["Production Support Reports/Finance/"];
       }
    }
}
