using Internal.Runtime;
using Internal.Runtime.CompilerHelpers;
using static Internal.Runtime.EEType;

namespace System.Runtime
{
    public static class TypeCast
    {
        [RuntimeExport("RhTypeCast_CheckCastClass")]
        public static unsafe object CheckCastClass(EEType* pTargetEEType, object obj)
        {
            // a null value can be cast to anything
            if (obj == null)
                return null;

            object result = StartupCodeHelpers.RhTypeCast_IsInstanceOfClass(pTargetEEType, obj);

            if (result == null)
            {
                // Throw the invalid cast exception defined by the classlib, using the input EEType* 
                // to find the correct classlib.

                //throw pTargetEEType->GetClasslibException(ExceptionIDs.InvalidCast);
                return null;
            }

            return result;
        }

        [RuntimeExport("RhTypeCast_CheckCastArray")]
        public static unsafe object CheckCastArray(EEType* pTargetEEType, object obj)
        {
            // a null value can be cast to anything
            if (obj == null)
                return null;

            object result = IsInstanceOfArray(pTargetEEType, obj);

            if (result == null)
            {
                // Throw the invalid cast exception defined by the classlib, using the input EEType* 
                // to find the correct classlib.

                //throw pTargetEEType->GetClasslibException(ExceptionIDs.InvalidCast);
                return null;
            }

            return result;
        }

        [RuntimeExport("RhTypeCast_IsInstanceOfArray")]
        public static unsafe object IsInstanceOfArray(EEType* pTargetType, object obj)
        {
            if (obj == null)
            {
                return null;
            }

            EEType* pObjType = obj.m_pEEType;

            // if the types match, we are done
            if (pObjType == pTargetType)
            {
                return obj;
            }

            // if the object is not an array, we're done
            if (!pObjType->IsArray)
            {
                return null;
            }

            // compare the array types structurally

            if (pObjType->ParameterizedTypeShape != pTargetType->ParameterizedTypeShape)
            {
                // If the shapes are different, there's one more case to check for: Casting SzArray to MdArray rank 1.
                if (!pObjType->IsSzArray || pTargetType->ArrayRank != 1)
                {
                    return null;
                }
            }

            /*
            if (CastCache.AreTypesAssignableInternal(pObjType->RelatedParameterType, pTargetType->RelatedParameterType,
                AssignmentVariation.AllowSizeEquivalence, null))
            {
                return obj;
            }
            */

            return null;
        }
    }
}
