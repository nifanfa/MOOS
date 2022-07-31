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

        public bool IsError { get; }

#nullable enable
        public string? Message { get; }
        public string? DiagnosticId { get; set; }
        public string? UrlFormat { get; set; }
#nullable disable
    }
}