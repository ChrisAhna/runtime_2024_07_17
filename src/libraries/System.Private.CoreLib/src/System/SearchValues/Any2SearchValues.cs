﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    internal sealed class Any2SearchValues<T, TImpl> : SearchValues<T>
        where T : struct, IEquatable<T>
        where TImpl : struct, INumber<TImpl>
    {
        private readonly TImpl _e0, _e1;

        public unsafe Any2SearchValues(ReadOnlySpan<TImpl> values)
        {
            Debug.Assert(sizeof(T) == sizeof(TImpl));
            Debug.Assert(values.Length == 2);
            (_e0, _e1) = (values[0], values[1]);
        }

        internal override unsafe T[] GetValues()
        {
            TImpl e0 = _e0, e1 = _e1;
            return new[] { *(T*)&e0, *(T*)&e1 };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override unsafe bool ContainsCore(T value) =>
            *(TImpl*)&value == _e0 || *(TImpl*)&value == _e1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override int IndexOfAny(ReadOnlySpan<T> span) =>
            SpanHelpers.NonPackedIndexOfAnyValueType<TImpl, SpanHelpers.DontNegate<TImpl>>(ref Unsafe.As<T, TImpl>(ref MemoryMarshal.GetReference(span)), _e0, _e1, span.Length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override int IndexOfAnyExcept(ReadOnlySpan<T> span) =>
            SpanHelpers.NonPackedIndexOfAnyValueType<TImpl, SpanHelpers.Negate<TImpl>>(ref Unsafe.As<T, TImpl>(ref MemoryMarshal.GetReference(span)), _e0, _e1, span.Length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override int LastIndexOfAny(ReadOnlySpan<T> span) =>
            SpanHelpers.LastIndexOfAnyValueType(ref Unsafe.As<T, TImpl>(ref MemoryMarshal.GetReference(span)), _e0, _e1, span.Length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override int LastIndexOfAnyExcept(ReadOnlySpan<T> span) =>
            SpanHelpers.LastIndexOfAnyExceptValueType(ref Unsafe.As<T, TImpl>(ref MemoryMarshal.GetReference(span)), _e0, _e1, span.Length);
    }
}
