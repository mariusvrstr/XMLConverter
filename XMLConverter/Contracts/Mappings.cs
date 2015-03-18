
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XMLConverter.Contracts
{
    [XmlRoot(ElementName = "Mappings", Namespace="http://www.w3schools.com")]
    public class Mappings
    {
        public List<ConstantToXml> ConstantToXmlList { get; set; }

        public List<XmlToXml> XmlToXmlList { get; set; }
    }
}
