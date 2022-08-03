namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public sealed class FieldOffsetAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the FieldOffsetAttribute class with the offset in the structure to the beginning of the field.
        /// </summary>
        /// <param name="offset">The offset in bytes from the beginning of the structure to the beginning of the field.</param>
        public FieldOffsetAttribute(int offset)
        {
            Value = offset;
        }

        /// <summary>
        /// The offset from the beginning of the structure to the beginning of the field.
        /// </summary>
        public int Value
        {
            get;
            internal set;
        }
    }
}