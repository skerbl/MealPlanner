namespace MPData
{
    public class MessageEventArgs : System.EventArgs
    {
        public string Message { get; set; }

        public MessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
