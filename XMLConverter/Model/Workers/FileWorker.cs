
namespace XMLConverter.Model.Workers
{
    using System;
    using System.IO;
    using System.Xml.Linq;
    using Properties;
    using System.Collections.Generic;
    using System.Linq;

    public class FileWorker
    {
        private static Dictionary<string, string> _sourceFiles;

        public static Dictionary<string, string> SourceFiles
        {
            get
            {
                return _sourceFiles ?? (_sourceFiles = GetSourceFiles());
            }
        }

        private static Dictionary<string, string> GetSourceFiles()
        {
            var files = Directory.GetFiles(GetFullFilePathFromRelativePath(@"Source"));

            return files.ToDictionary(filePath => Path.GetFileName(filePath) ?? "N/A");
        }
        
        public static string GetOutputXmlFileLocation
        {
            get { return GetFullFilePathFromRelativePath(Global.Default.OutputXmlFile); }
        }

        public static string GetXmlTemplateLocation
        {
            get { return GetFullFilePathFromRelativePath(Global.Default.XmlTemplate); }
        }

        public static string GetXmlMapper
        {
            get { return GetFullFilePathFromRelativePath(Global.Default.XmlMapperFile); }
        }


        public static string GetFullFilePathFromRelativePath(string relativePath)
        {
            return Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + relativePath);
        }


        public static XDocument GetXmlFile(string filePath)
        {
            return XDocument.Load(filePath);
        }
    }
}
