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

        public ObsoleteAttribute(string? message)
        {
            Message = message;
            IsError = false;
        }

        public ObsoleteAttribute(string? message, bool error)
        {
            Message = message;
            IsError = error;
        }

        public string? Message { get; }

        public bool IsError { get; }

        public string? DiagnosticId { get; set; }

        public string? UrlFormat { get; set; }
    }
}