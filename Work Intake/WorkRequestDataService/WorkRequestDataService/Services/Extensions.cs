using System.Collections.Generic;
using System.Reflection;
using WorkRequestDataService.Models;

namespace WorkRequestDataService.Services
{
    internal static class Extensions
    {
        #region Internal Methods

        internal static List<Variance> DetailedCompare<T>(this T originalObject, T updatedObject)
        {
            PropertyInfo[] pi = originalObject.GetType().GetProperties();
            List<Variance> variances = new List<Variance>();
            foreach (PropertyInfo p in pi)
            {
                Variance v = new Variance();
                v.Property = p.Name;

                v.OriginalValue = p.GetValue(originalObject)
                    == null ? string.Empty : p.GetValue(originalObject);  // Added ternary statements
                v.UpdatedValue = p.GetValue(updatedObject)                // to avoid null exceptions.
                    == null ? string.Empty : p.GetValue(updatedObject);
                if (!v.OriginalValue.Equals(v.UpdatedValue))
                    variances.Add(v);
            }
            return variances;
        }

        #endregion Internal Methods
    }
}