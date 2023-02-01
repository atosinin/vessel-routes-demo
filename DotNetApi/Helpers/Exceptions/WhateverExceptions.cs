namespace DotNetApi.Helpers.Exceptions
{
    public class WhateverUserMessageOnlyException : Exception
    {
        public WhateverUserMessageOnlyException() : base() { }
        public WhateverUserMessageOnlyException(string message) : base(message) { }
    }

    public class WhateverUserMessageLoggedException : Exception
    {
        public WhateverUserMessageLoggedException() : base() { }
        public WhateverUserMessageLoggedException(string message) : base(message) { }
    }

    public class WhateverBadRequestException : Exception
    {
        public WhateverBadRequestException() : base() { }
        public WhateverBadRequestException(string message) : base(message) { }
    }
}
