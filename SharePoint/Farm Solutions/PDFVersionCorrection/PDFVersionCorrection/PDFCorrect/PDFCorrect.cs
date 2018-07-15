using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.IO;

namespace PDFVersionCorrection.PDFCorrect
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class PDFCorrect : SPItemEventReceiver
    {
        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            try { 
            var requiresProcessing = false;
                if (properties.ListItem[SPBuiltInFieldId.File_x0020_Type].ToString().ToUpper() == "PDF")
                {
                    using (StreamReader sr = new StreamReader(properties.ListItem.File.OpenBinaryStream()))
                    {
                        string tempRead = sr.ReadLine();
                        if (tempRead == @"%PDF-1.3")
                        {
                            Util.LogError($"{properties.ListItem.File.Name} at {properties.WebUrl}/{properties.ListItem.Url} being updated because PDF version was ${tempRead}", Util.ErrorLevel.Info);
                            requiresProcessing = true;
                        }
                    }

                    if (requiresProcessing)
                    {
                        if (Util.InstantiateLicense(properties.Web))
                        {
                            // Read the entire file into a byte array
                            var binaryContent = properties.ListItem.File.OpenBinary();
                            // Increment the version number YAY Magic Numbers!!!
                            binaryContent[7]++;
                            // Read the bytes in as a stream to an Aspose PDF document
                            Aspose.Pdf.Document toBook = new Aspose.Pdf.Document(new MemoryStream(binaryContent));
                            // Reassess the metatable
                            toBook.ProcessParagraphs();
                            // Repair anything that the above line missed
                            toBook.Repair();
                            // Save out the new PDF to a stream
                            using (var ms = new MemoryStream())
                            {
                                toBook.Save(ms);
                                properties.ListItem.File.SaveBinary(ms);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.LogError($"PDF Version Correction for file {properties.WebUrl}/{properties.ListItem.Url}: {ex.Message}");
            }
        }


    }
}