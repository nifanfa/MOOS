namespace System
{
    public abstract class Delegate
    {
        protected internal object m_firstParameter;
        protected internal object m_helperObject;
        protected internal IntPtr m_extraFunctionPointerOrData;
        protected internal IntPtr m_functionPointer;

#nullable enable
        public static Delegate? Combine(Delegate? _, Delegate? b)
#nullable disable
        {
            return b;
        }

#nullable enable
        public static Delegate? Remove(Delegate? source, Delegate? _)
#nullable disable
        {
            return source;
        }

        // This function is known to the compiler backend.
        protected void InitializeOpenStaticThunk(object _, IntPtr functionPointer, IntPtr functionPointerThunk)
        {
            // This sort of delegate is invoked by calling the thunk function pointer with the arguments to the delegate + a reference to the delegate object itself.
            m_firstParameter = this;
            m_functionPointer = functionPointerThunk;
            m_extraFunctionPointerOrData = functionPointer;
        }

        // This function is known to the IL Transformer.
        protected void InitializeClosedInstance(object firstParameter, IntPtr functionPointer)
        {
            if (firstParameter == null)
            {
                m_firstParameter = this;
            }
            else
            {
                m_firstParameter = firstParameter;
            }

            m_functionPointer = functionPointer;
        }
    }
}
