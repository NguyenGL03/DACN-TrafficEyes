using System;

namespace gAMSPro.Procedures.Attributes
{
    public class StoreParamAttribute : Attribute
    {
        public StoreParamAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
