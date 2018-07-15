using System.Collections;

namespace SPWebServices.Models
{
    public class SubwebsRequest
    {
     
        /// <summary>
        /// The Parent Web to use as the scope base.
        /// </summary>
        public string Url { get; set; }
        
        /// <summary>
        /// Optional, used to reduce the size of the data returned. 
        /// This is an array of string patterns used to exclude any properties
        /// which have a key containing the patterns.  
        /// </summary>
        public string[] ExcludeKeysWhichContain { get; set; }

        /// <summary>
        /// Optional, used to reduce the size of the data returned.
        /// A maximum number of results to return.  Default is 5000 if unspecified.
        /// </summary>
        public int ResultLimit { get; set; } = 5000;

        /// <summary>
        /// Optional.  Return only sites which have a property set which matches the
        /// values specified in the pattern "PropertyKey", "ValueToMatch".
        /// Alternately, you can indicate more than one value to match on by supplying a
        /// string array for the value.
        /// 
        /// Example: 
        /// "PropertyValueQuery": 
        /// {
        ///   "MyPropertyA" : ["PossibleValue1", "PossibleValue2"],
        ///   "MyPropertyB" : "PossibleValue",
        ///   "MyPropertyC" : ["PossibleValue1", "PossibleValue2", "PossibleValue3"]
        /// }
        /// </summary>
        public Hashtable PropertyValueQuery { get; set; }

        /// <summary>
        /// Optional.  Applies to instances where the PropertyValueQuery was supplied.
        /// Specify true to use case sensitive comparison.  False is default, if not specified.
        /// Note: Case MUST Always match on the Property Key, regardless of this setting.
        /// </summary>
        public bool MatchCaseOnQueryValues { get; set; }

    }
}