namespace iSpyApplication.Sources
{
    public class SourceItem
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public int ObjectTypeID { get; set; }

        public int SourceTypeID { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public SourceItem(string name, string category, int objectTypeID, int sourceTypeID)
        {
            Name = name;
            SourceTypeID = sourceTypeID;
            ObjectTypeID = objectTypeID;
            Category = category;
        }

    }
}
