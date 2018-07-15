using System;
using System.Collections.Generic;
using System.Text;

namespace RiverStoneBaseLib
{
    /// <summary>
    /// 
    /// </summary>
    public static class RS_String
    {
        /// <summary>
        /// This is a test.
        /// </summary>
        /// <param name="strRiverStone">The test to work on.</param>
        /// <returns>Test result.</returns>
        public static string RS_TestStringExt(this String strRiverStone)
        {
            //TEST BEGIN
            return TestStringExtImplementation(strRiverStone);
            //TEST END
        }

        private static string TestStringExtImplementation(string strImplementation)
        {
            return string.Format("This is a test - {0}", strImplementation);
        }
    }
}
