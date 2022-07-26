
namespace System
{
    public abstract class Delegate
    {
        public object m_firstParameter;
        public object m_helperObject;
        public IntPtr m_extraFunctionPointerOrData;
        public IntPtr m_functionPointer;

        public static Delegate? Combine(Delegate? a, Delegate? b)
        {
            return b;
        }

        public static Delegate? Remove(Delegate? source, Delegate? value)
        {
            return null;
        }

        // This function is known to the compiler backend.
        protected void InitializeOpenStaticThunk(object firstParameter, IntPtr functionPointer, IntPtr functionPointerThunk)
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
