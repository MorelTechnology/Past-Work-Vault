using System.Collections.Generic;

namespace SPWebServices.Models
{
    /// <summary>
    /// test
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class WebDetails<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public WebDetails() : base() { }
        public WebDetails(int capacity) : base(capacity) { }
    }
}