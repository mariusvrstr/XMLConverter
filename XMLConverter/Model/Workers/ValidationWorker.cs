
namespace XMLConverter.Model.Workers
{
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;
    using Exceptions;

    public class ValidationWorker
    {
        public static FileValidationResultType ValidateXmlFile(string xmlfilePath, string xsdFilePath = null)
        {
            try
            {
                if (!File.Exists(xmlfilePath))
                {
                    throw new FileValidationException(FileValidationResultType.FileDoesNotExist, string.Format("File does not exist [{0}].", Path.GetFileName(xmlfilePath)));
                }
                if (!string.IsNullOrEmpty(xsdFilePath) && !File.Exists(xsdFilePath))
                {
                    throw new FileValidationException(FileValidationResultType.FileDoesNotExist, string.Format("File does not exist [{0}].", Path.GetFileName(xsdFilePath)));
                }

                ValidateXmlFileContent(xmlfilePath, xsdFilePath);

            }
            catch (FileValidationException exception)
            {
                return exception.ErrorType;
            }

            return FileValidationResultType.Success;
        }


        private static void ValidateXmlFileContent(string xmlfilePath, string xsdFilePath)
        {
            try
            {

                XmlReaderSettings settings;

                if (string.IsNullOrEmpty(xsdFilePath))
                {
                    settings = new XmlReaderSettings();
                }
                else
                {
                    XmlSchema schema;

                    using (var schemaReader = XmlReader.Create(xsdFilePath))
                    {
                        schema = XmlSchema.Read(schemaReader, ValidationEventHandler);
                    }

                    var schemas = new XmlSchemaSet();
                    schemas.Add(schema);

                    settings = new XmlReaderSettings
                    {
                        ValidationType = ValidationType.Schema,
                        Schemas = schemas,
                        ValidationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints |
                                          XmlSchemaValidationFlags.ReportValidationWarnings
                    };
                }

                settings.ValidationEventHandler += ValidationEventHandler;
           
                using (var validationReader = XmlReader.Create(xmlfilePath, settings))
                {
                    while (validationReader.Read()) { }
                }
            }
            catch (XmlException exception)
            {
                var ex = new FileValidationException(FileValidationResultType.FileIsNotAValidXmlFile, exception.Message);
                throw ex;
            }
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity != XmlSeverityType.Error) return;

            throw new FileValidationException(FileValidationResultType.FailedXsdValidation, args.Exception.Message);
        }
    }
}
