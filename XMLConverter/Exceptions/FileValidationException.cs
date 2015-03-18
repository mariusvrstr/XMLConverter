
namespace XMLConverter.Exceptions
{
    using System;

    public class FileValidationException : Exception
    {
        public FileValidationResultType ErrorType { get; set; }

        public FileValidationException(FileValidationResultType errorType) 
        {
            this.ErrorType = errorType;
        }
        
        public FileValidationException(FileValidationResultType errorType, string details)
            : base(details)
        {
            this.ErrorType = errorType;
        }
    }
}
