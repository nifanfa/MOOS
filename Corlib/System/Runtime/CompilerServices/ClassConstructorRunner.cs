using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    // A class responsible for running static constructors. The compiler will call into this
    // code to ensure static constructors run and that they only run once.
    [McgIntrinsics]
    internal static class ClassConstructorRunner
    {
        private static unsafe IntPtr CheckStaticClassConstructionReturnNonGCStaticBase(ref StaticClassConstructionContext context, IntPtr nonGcStaticBase)
        {
            CheckStaticClassConstruction(ref context);
            return nonGcStaticBase;
        }

        private static unsafe object CheckStaticClassConstructionReturnGCStaticBase(ref StaticClassConstructionContext context, object gcStaticBase)
        {
            CheckStaticClassConstruction(ref context);
            return gcStaticBase;
        }

        private static unsafe void CheckStaticClassConstruction(ref StaticClassConstructionContext context)
        {
            // Very simplified class constructor runner. In real world, the class constructor runner
            // would need to be able to deal with potentially multiple threads racing to initialize
            // a single class, and would need to be able to deal with potential deadlocks
            // between class constructors.

            if (context.initialized == 1)
                return;

            context.initialized = 1;

            // Run the class constructor.
            ((delegate*<void>)context.cctorMethodAddress)();
        }
    }
}