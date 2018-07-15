using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using WorkRequestDataService.Models;

namespace WorkRequestDataService.Services
{
    /// <summary>
    /// Utility Service
    /// Contains various reusable code methods employed throughout this application.
    /// </summary>
    internal static class Utility
    {
        #region Internal Methods

        internal static DataTable ArrayToDataTable(object source)
        {
            XmlSerializer serializer = new XmlSerializer(source.GetType());
            System.IO.StringWriter sw = new System.IO.StringWriter();
            serializer.Serialize(sw, source);
            System.Data.DataSet ds = new System.Data.DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();
            System.IO.StringReader reader = new System.IO.StringReader(sw.ToString());
            ds.ReadXml(reader);
            return ds.Tables[0];
        }

        internal static T ConvertDictionaryTo<T>(IDictionary<string, object> dictionary) where T : new()
        {
            Type type = typeof(T);
            T ret = new T();

            foreach (var keyValue in dictionary)
            {
                // determine the object storage type
                Type storeType = ret.GetType().GetProperties().Where(property => property.Name.ToLower() == keyValue.Key.ToLower()).FirstOrDefault().PropertyType;
                PropertyInfo objectProperty = type.GetProperties().Where(property => property.Name.ToLower() == keyValue.Key.ToLower()).FirstOrDefault();
                try
                {
                    objectProperty.SetValue(ret, Convert.ChangeType(keyValue.Value, storeType), null);
                }
                catch (Exception)
                {
                    objectProperty.SetValue(ret, (WorkRequestStatus)Enum.Parse(typeof(WorkRequestStatus), keyValue.Value as string), null);
                }
            }

            return ret;
        }

        internal static string Description(this Enum value)
        {
            // variables
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return
            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }

        internal static string escape(string stringToEscape)
        {
            return System.Web.HttpUtility.JavaScriptStringEncode(stringToEscape);
        }

        internal static int GetEnumFromDescription(string description, Type enumType)
        {
            foreach (var field in enumType.GetFields())
            {
                DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute == null)
                    continue;
                if (attribute.Description == description)
                {
                    return (int)field.GetValue(null);
                }
            }
            return 0;
        }

        internal static string objectToSql(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        internal static object sqlToObject(object objString, [Optional]Type type)
        {
            if (type != null)
                return JsonConvert.DeserializeObject(objString as string, type);
            else
                return JsonConvert.DeserializeObject(objString as string);
        }

        internal static string unescape(string stringToUnescape)
        {
            if (string.IsNullOrEmpty(stringToUnescape))
                return null;
            //return System.Web.HttpUtility.HtmlDecode(stringToUnescape);
            return System.Text.RegularExpressions.Regex.Unescape(stringToUnescape);
        }

        #endregion Internal Methods
    }
}