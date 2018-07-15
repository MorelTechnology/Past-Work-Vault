using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentService.Models
{
    /**
     * These are the document MIME types currently supported in RiverStone Systems (i.e. GuideWire Claimcenter)
     * and their (most likely) associated extensions.
     * 
     * TODO Determine more elegant approach for these settings.
     * Dictionary is the best/fastest collection to use for mapping these
     * properties, however it is not serializable by default, which precludes it
     * from being used in a settings file.  Later we can extend Dictionary class to
     * support this, but for now, it's easy enough to place here.
     * **/

    class MIMETypes : IDisposable
    {
        #region IDisposable Implementation Support
        // Disposed already called?
        bool disposed = false;

        // Public Dispose pattern callable from unmanaged objects.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected Dispose Pattern 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
        // Destructor
        ~MIMETypes()
        {
            Dispose(false);
        }

        #endregion

        public Dictionary<string, List<string>> ExtensionMapping = new Dictionary<string, List<string>>
       {
            {"application/csv", new List<string>(new string[] {".csv" }) },
            {"application/msword", new List<string>(new string[] {".doc" }) },
            {"application/octet-stream", new List<string>(new string[] {"unknown" }) },
            {"application/pdf", new List<string>(new string[] {".pdf" }) },
            {"application/postscript", new List<string>(new string[] {".ps" }) },
            {"application/rtf", new List<string>(new string[] {".rtf"}) },
            {"application/vnd.ms-excel", new List<string>(new string[] {".xls", ".csv" }) },
            {"application/vnd.ms-powerpoint", new List<string>(new string[] {".ppt", ".pps" }) },
            {"application/vnd.openxmlformats-officedocument.presentationml.presentation", new List<string>(new string[] {".pptx" }) },
            {"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", new List<string>(new string[] {".xlsx" }) },
            {"application/vnd.openxmlformats-officedocument.wordprocessingml.document", new List<string>(new string[] {".docx" }) },
            {"application/x-msexcel", new List<string>(new string[] {".xls" }) },
            {"audio/wav", new List<string>(new string[] {".wav" }) },
            {"audio/x-ms-wma", new List<string>(new string[] {".wma" }) },
            {"image/bmp", new List<string>(new string[] {".bmp" }) },
            {"image/gif", new List<string>(new string[] {".gif" }) },
            {"image/jpeg", new List<string>(new string[] {".jpg" }) },
            {"image/pjpeg", new List<string>(new string[] {".jpg", ".jpeg" }) },
            {"image/tiff", new List<string>(new string[] {".tif", ".tiff" }) },
            {"image/vnd.ms-modi", new List<string>(new string[] {"mdi" }) },
            {"image/x-png", new List<string>(new string[] {".png" }) },
            {"text/html", new List<string>(new string[] {".html", ".htm" }) },
            {"text/plain", new List<string>(new string[] {".txt" }) },
            {"text/richtext", new List<string>(new string[] {".rtx" }) },
            {"text/xml", new List<string>(new string[] {".xml" }) },
            {"video/mpeg", new List<string>(new string[] {".mpg", ".mpeg" }) },
            {"video/quicktime", new List<string>(new string[] {".mov" }) },
            {"video/x-msvideo", new List<string>(new string[] {".avi" }) },
            {"application/vnd.ms-outlook", new List<string>(new string[] {".msg" }) },
            {"application/zip", new List<string>(new string[] {".zip" }) },
            {"multipart/x-zip", new List<string>(new string[] {".zip" }) },
            {"application/x-zip-compressed", new List<string>(new string[] {".zip" }) },
            {"application/x-compressed", new List<string>(new string[] {".zip" }) },
            {"application/vnd.ms-xpsdocument", new List<string>(new string[] {".xps" }) },
            {"application/vnd.openxmlformats-officedocument.wordprocessingml.template", new List<string>(new string[] {".dotx" }) },
            {"application/onenote", new List<string>( new string[] {".one"}) }
      };

        public bool ContainsExtension(string value)
        {
            foreach (KeyValuePair<string, List<string>> entry in ExtensionMapping)
            {
                if (entry.Value.Contains(value))
                {
                    return true;
                }
            }
            return false;
        }
        public bool ContainsMimetype(string value)
        {
            return ExtensionMapping.Keys.Contains(value);
        }
    }
}

