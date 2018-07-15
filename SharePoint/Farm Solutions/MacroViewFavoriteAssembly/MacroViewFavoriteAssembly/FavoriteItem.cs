using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMFUtilities
{
    public interface FavoriteItem
    {
        string Title
        {
            get;
            set;
        }
        bool IsFileLocation
        {
            get;
        }
    }
}
