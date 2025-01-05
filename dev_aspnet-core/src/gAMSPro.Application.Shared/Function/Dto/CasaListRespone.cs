using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSOFTcore.gAMSPro.Functions
{
    public class CasaListRespone
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {

            private object headerField;

            private EnvelopeBody bodyField;

            /// <remarks/>
            public object Header
            {
                get
                {
                    return this.headerField;
                }
                set
                {
                    this.headerField = value;
                }
            }

            /// <remarks/>
            public EnvelopeBody Body
            {
                get
                {
                    return this.bodyField;
                }
                set
                {
                    this.bodyField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {

            private CasaListCheckResponse casaListCheckResponseField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.alsb.com/")]
            public CasaListCheckResponse CasaListCheckResponse
            {
                get
                {
                    return this.casaListCheckResponseField;
                }
                set
                {
                    this.casaListCheckResponseField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.alsb.com/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.alsb.com/", IsNullable = false)]
        public partial class CasaListCheckResponse
        {

            private ListOutListCasa[] listOutField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute(Namespace = "")]
            [System.Xml.Serialization.XmlArrayItemAttribute("ListCasa", IsNullable = false)]
            public ListOutListCasa[] ListOut
            {
                get
                {
                    return this.listOutField;
                }
                set
                {
                    this.listOutField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ListOutListCasa
        {

            private string accountNoField;

            private string accountDescField;

            /// <remarks/>
            public string AccountNo
            {
                get
                {
                    return this.accountNoField;
                }
                set
                {
                    this.accountNoField = value;
                }
            }

            /// <remarks/>
            public string AccountDesc
            {
                get
                {
                    return this.accountDescField;
                }
                set
                {
                    this.accountDescField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class ListOut
        {

            private ListOutListCasa[] listCasaField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ListCasa")]
            public ListOutListCasa[] ListCasa
            {
                get
                {
                    return this.listCasaField;
                }
                set
                {
                    this.listCasaField = value;
                }
            }
        }
    }
}