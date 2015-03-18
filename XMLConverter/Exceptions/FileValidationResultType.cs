
namespace XMLConverter.Exceptions
{
    public enum FileValidationResultType
    {
        FileDoesNotExist,
        FileIsNotAValidXmlFile,
        FailedXsdValidation,
        FailedExpectedXmlElementDoesNotExist,
        Success
    }
}
