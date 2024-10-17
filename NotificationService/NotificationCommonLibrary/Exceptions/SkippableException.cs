namespace NotificationCommonLibrary.Exceptions
{
    public class SkippableException : Exception
    {
        public SkippableException(string message) : base(message) { }
    }
}