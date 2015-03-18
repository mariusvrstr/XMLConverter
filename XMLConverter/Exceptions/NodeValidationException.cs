
namespace XMLConverter.Exceptions
{
    using System;

    public class NodeValidationException : Exception
    {
        public NodeValidationResultType ErrorType { get; set; }

        public NodeValidationException(NodeValidationResultType errorType) 
        {
            this.ErrorType = errorType;
        }

        public NodeValidationException(NodeValidationResultType errorType, string details)
            : base(details)
        {
            this.ErrorType = errorType;
        }
    }
}
