
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace XMLConverter.Model
{
    using System;
    using System.Linq;
    using Contracts;
    using Workers;
    using Properties;
    using Instrumentation;

    public class Model : IModel
    {
        public void GenerateNewFile()
        {
            try
            {
                Logger.Instance.Info("<<<<<<<<<<<< New conversion started >>>>>>>>>>>>>>>");

                ValidateFiles();

                var mapper = XmlWorker.DeserializeXmlFile<Mappings>(
                     FileWorker.GetFullFilePathFromRelativePath(Global.Default.XmlMapperFile));
                
                ValidateSettings(mapper);

                var outputDocument = new XmlDocument();
                outputDocument.Load(FileWorker.GetFullFilePathFromRelativePath(Global.Default.XmlTemplate));

                var sourceFilesForSettings = GetSourceFilesForSettings(mapper);
                
                foreach (var setting in mapper.XmlToXmlList)
                {
                    var sourceFile = FileWorker.SourceFiles[sourceFilesForSettings[setting]];

                    XmlWorker.UpdateXmlElement(ref outputDocument, sourceFile, setting);
                }

                foreach (var setting in mapper.ConstantToXmlList)
                {
                    XmlWorker.UpdateXmlElement(ref outputDocument, setting);
                }
                
                var writerSettings = new XmlWriterSettings
                {
                    NewLineOnAttributes = false,
                    NewLineHandling = NewLineHandling.None,
                    Indent = true
                };

                using (var xw = XmlWriter.Create(FileWorker.GetFullFilePathFromRelativePath(Global.Default.OutputXmlFile), writerSettings))
                {
                    var documentNav = outputDocument.CreateNavigator();  
                    documentNav.WriteSubtree(xw);
                    xw.Close();
                }

                Console.WriteLine("Successfully generated XML file from template.");
                Logger.Instance.Info("<<<<<<<<<<<< Conversion successful >>>>>>>>>>>>>>>");

            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to generate the XML file. Please refer to log file for details.");
                Logger.Instance.Error(exception, "Failed to generate new XML file");
            }
        }
        
        public void ValidateFiles()
        {
            var mapperFilePath = FileWorker.GetFullFilePathFromRelativePath(Global.Default.XmlMapperFile);
            var mapperSchemaFilePath = FileWorker.GetFullFilePathFromRelativePath(Global.Default.MapperSchemaFile);
            var templateFilePath = FileWorker.GetFullFilePathFromRelativePath(Global.Default.XmlTemplate);

            foreach (var sourceFile in FileWorker.SourceFiles)
            {
                ValidationWorker.ValidateXmlFile(sourceFile.Value);
            }

            ValidationWorker.ValidateXmlFile(mapperFilePath, mapperSchemaFilePath);
            ValidationWorker.ValidateXmlFile(templateFilePath);
        }

        public void ValidateSettings(Mappings mappings)
        {
            var listOfXmlDestinationElements = mappings.XmlToXmlList.Select(xml => xml.DestinationXPath)
                                                 .Union(mappings.ConstantToXmlList.Select(xml => xml.DestinationXPath));

            GetSourceFilesForSettings(mappings);

            XmlWorker.VerifyXmlElementExistInFile(
                 FileWorker.GetFullFilePathFromRelativePath(Global.Default.XmlTemplate), listOfXmlDestinationElements);
        }
        
        private Dictionary<XmlToXml, string> GetSourceFilesForSettings(Mappings mappings)
        {
           return XmlWorker.VerifyXmlElementsExistInSourceFiles(mappings.XmlToXmlList);
        }
    }
}
