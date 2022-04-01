/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

namespace System
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum |
        AttributeTargets.Interface | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Delegate,
        Inherited = false)]
    public sealed class ObsoleteAttribute : Attribute
    {
        public ObsoleteAttribute()
        {
            Message = null;
            IsError = false;
        }

#nullable enable
        public ObsoleteAttribute(string? message)
#nullable disable
        {
            Message = message;
            IsError = false;
        }

#nullable enable
        public ObsoleteAttribute(string? message, bool error)
#nullable disable
        {
            Message = message;
            IsError = error;
        }

#nullable enable
        public string? Message { get; }
#nullable disable

        public bool IsError { get; }

#nullable enable
        public string? DiagnosticId { get; set; }
#nullable disable

#nullable enable
        public string? UrlFormat { get; set; }
#nullable disable
    }
}
