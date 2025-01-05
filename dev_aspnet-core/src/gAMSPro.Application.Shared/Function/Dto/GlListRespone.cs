namespace GSOFTcore.gAMSPro.Functions
{
    public class GlListRespone
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

            private GlListResponse glListResponseField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.alsb.com/")]
            public GlListResponse GlListResponse
            {
                get
                {
                    return this.glListResponseField;
                }
                set
                {
                    this.glListResponseField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.alsb.com/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.alsb.com/", IsNullable = false)]
        public partial class GlListResponse
        {

            private GlLstLstGl[] glLstField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute(Namespace = "")]
            [System.Xml.Serialization.XmlArrayItemAttribute("LstGl", IsNullable = false)]
            public GlLstLstGl[] GlLst
            {
                get
                {
                    return this.glLstField;
                }
                set
                {
                    this.glLstField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class GlLstLstGl
        {

            private string glCodeField;

            private string glDescField;

            /// <remarks/>
            public string GlCode
            {
                get
                {
                    return this.glCodeField;
                }
                set
                {
                    this.glCodeField = value;
                }
            }

            /// <remarks/>
            public string GlDesc
            {
                get
                {
                    return this.glDescField;
                }
                set
                {
                    this.glDescField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class GlLst
        {

            private GlLstLstGl[] lstGlField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("LstGl")]
            public GlLstLstGl[] LstGl
            {
                get
                {
                    return this.lstGlField;
                }
                set
                {
                    this.lstGlField = value;
                }
            }
        }


    }
}