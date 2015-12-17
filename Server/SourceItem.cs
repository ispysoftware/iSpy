using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSpyApplication.Server
{
    public class SourceItem
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public int ObjectTypeID { get; set; }

        public int SourceTypeID { get; set; }

        public bool Exclude { get; set; }

        public bool ExcludeFromOnline { get; set; }

        public override string ToString()
        {
            return LocRm.GetString(Name);
        }

        public SourceItem(string name, string category, int objectTypeID, int sourceTypeID, bool excludeFromList = false, bool excludeFromOnline = false)
        {
            Name = name;
            SourceTypeID = sourceTypeID;
            ObjectTypeID = objectTypeID;
            Category = category;
            Exclude = excludeFromList;
            ExcludeFromOnline = excludeFromOnline;
        }

    }
}
