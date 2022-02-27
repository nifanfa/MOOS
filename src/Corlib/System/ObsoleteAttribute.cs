// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
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
