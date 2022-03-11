// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence

namespace System.Runtime.InteropServices
{
    public sealed class FieldOffsetAttribute : Attribute
    {
        public FieldOffsetAttribute(int offset)
        {
            Value = offset;
        }

        public int Value { get; }
    }
}
