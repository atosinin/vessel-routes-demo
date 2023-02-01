namespace DotNetApi.Helpers.Exceptions
{
    public class UserMessageOnlyException : Exception
    {
        public UserMessageOnlyException() : base() { }
        public UserMessageOnlyException(string message) : base(message) { }
    }

    public class UserMessageLoggedException : Exception
    {
        public UserMessageLoggedException() : base() { }
        public UserMessageLoggedException(string message) : base(message) { }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException() : base() { }
        public BadRequestException(string message) : base(message) { }
    }
}
