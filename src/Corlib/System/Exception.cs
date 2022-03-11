// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
namespace System
{
    public partial class Exception
    {
        private readonly string _message;
        private readonly Exception _innerException;

#nullable enable
        public Exception(string? message)
#nullable disable
        {
            _message = message;
        }
#nullable enable
        public Exception(string? message, Exception? innerException)
#nullable disable
        {
            _message = message;
            _innerException = innerException;
        }


        public virtual string Message => _message;

        public virtual Exception GetBaseException()
        {
#nullable enable
            Exception? inner = InnerException;
#nullable disable
            Exception back = this;

            while (inner != null)
            {
                back = inner;
                inner = inner.InnerException;
            }

            return back;
        }

#nullable enable
        public Exception? InnerException => _innerException;
#nullable disable

        public override string ToString()
        {
            return _message;
        }
    }
}
