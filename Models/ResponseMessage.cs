namespace ISAM4332.Assignment05.Models
{
    public class ResponseMessage
    {
        public ResponseMessageType MessageType { get; set; } = ResponseMessageType.Information;

        public string Header { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public ResponseMessage()
        {
        }

        public ResponseMessage(ResponseMessageType messageType, string message)
        {
            MessageType = messageType;
            Header = string.Empty;
            Message = message;
        }

        public ResponseMessage(ResponseMessageType messageType, string header, string message)
        {
            MessageType = messageType;
            Header = header;
            Message = message;
        }

    }
}