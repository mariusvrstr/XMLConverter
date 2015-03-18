
namespace XMLConverter.Model.Workers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Xml.XPath;
    using Exceptions;
    using System.IO;
    using Contracts;

    public static class XmlWorker
    {
        public static XPathNavigator FindXmlElement(XPathDocument document, string xNodePath)
        {
            var navigator = document.CreateNavigator();

            return navigator.SelectSingleNode(xNodePath);
        }

        private static bool UpdateXmlElementValue(XmlDocument document, string xNodePath, string newValue)
        {
            var navigator = document.CreateNavigator();
            var node = navigator.SelectSingleNode(xNodePath);

            if (node == null) return false;

            node.SetValue(newValue);
            return true;
        }

        public static T DeserializeXmlFile<T>(string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            T response;

            using (var reader = XmlReader.Create(filePath))
            {
                response = (T)serializer.Deserialize(reader);
            }

            return response;
        }

        public static Dictionary<XmlToXml, string> VerifyXmlElementsExistInSourceFiles(List<XmlToXml> settings)
        {
            var sourceFileLocations = settings.ToDictionary(setting => setting, setting => string.Empty);
            string msg;

            foreach (var sourceFile in FileWorker.SourceFiles)
            {
                var xmlDocument = new XPathDocument(sourceFile.Value);
                
                foreach (var setting in settings)
                {
                    if (!string.IsNullOrEmpty(setting.SourceFileName) && setting.SourceFileName != sourceFile.Key) continue;

                    var found = FindXmlElement(xmlDocument, setting.SourceXPath);
                    if ((found != null) && (string.IsNullOrEmpty(sourceFileLocations[setting])))
                    {
                        sourceFileLocations[setting] = sourceFile.Key;
                    }
                    else if (found != null)
                    {
                        msg = string.Format(@"Expected node [{0}] was found in more than one source file. Either ensure the source file is specified or remove duplicate setting", setting.SourceXPath);
                        throw new NodeValidationException(NodeValidationResultType.ExistMoreThanOnce, msg);
                    }
                }
            }

            var firstNotFoundSeting = sourceFileLocations.FirstOrDefault(set => string.IsNullOrEmpty(set.Value)).Key;
            if (firstNotFoundSeting == null) return sourceFileLocations;
            

            if (string.IsNullOrEmpty(firstNotFoundSeting.SourceFileName))
            {
                msg = string.Format(@"Expected node [{0}] was not found in any of the source files",
                    firstNotFoundSeting.SourceXPath);
            }
            else
            {
                msg = string.Format(@"Expected node [{0}] was not found in expected [{1}]",
                    firstNotFoundSeting.SourceXPath, firstNotFoundSeting.SourceFileName);
            }

            throw new NodeValidationException(NodeValidationResultType.DoesNotExist, msg);
        }

        public static NodeValidationResultType VerifyXmlElementExistInFile(string xmlFilePath, IEnumerable<string> nodePaths)
        {
            var xmlDocument = new XPathDocument(xmlFilePath);
            var nodeWithError = nodePaths.FirstOrDefault(nodePath => FindXmlElement(xmlDocument, nodePath) == null);

            if (nodeWithError == null) return NodeValidationResultType.Success;

            var msg = string.Format(@"Expected node [{0}] does not exist in [{1}]", nodeWithError,
                Path.GetFileName(xmlFilePath));

            throw new NodeValidationException(NodeValidationResultType.DoesNotExist, msg);
        }

        public static void UpdateXmlElement(ref XmlDocument targetDocument, string sourceFilePath, XmlToXml setting)
        {
            var sourceDocument = new XPathDocument(sourceFilePath);
            var newValue = FindXmlElement(sourceDocument, setting.SourceXPath).Value;

            if (newValue == null) return;

            if (!UpdateXmlElementValue(targetDocument, setting.DestinationXPath, newValue))
            {
                throw new NodeValidationException(NodeValidationResultType.UnkownFailure, 
                    string.Format("Unable to update node [{0}] with [{1}]", setting.DestinationXPath, newValue));
            }
        }

        public static void UpdateXmlElement(ref XmlDocument targetDocument, ConstantToXml setting)
        {
            if (!UpdateXmlElementValue(targetDocument, setting.DestinationXPath, setting.Value))
            {
                throw new NodeValidationException(NodeValidationResultType.UnkownFailure,
                    string.Format("Unable to update node [{0}] with [{1}]", setting.DestinationXPath, setting.Value));
            }
        }

    }
}
