// <copyright file="ListExtensions.cs" company="BovineLabs">
//     Copyright (c) BovineLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace EntitiesNavMeshBuilder.Utility
{
    internal static class ListExtensions
    {
        internal static unsafe void AddRangeNative<T>(this List<T> list, void* arrayBuffer, int length)
            where T : struct
        {
            if (length == 0)
            {
                return;
            }

            int index = list.Count;
            int newLength = index + length;

            // Resize our list if we require
            if (list.Capacity < newLength)
            {
                list.Capacity = newLength;
            }

            T[] items = NoAllocHelpers.ExtractArrayFromList(list);
            int size = UnsafeUtility.SizeOf<T>();

            // Get the pointer to the end of the list
            IntPtr bufferStart = (IntPtr)UnsafeUtility.AddressOf(ref items[0]);
            byte* buffer = (byte*)(bufferStart + size * index);

            UnsafeUtility.MemCpy(buffer, arrayBuffer, length * (long)size);

            NoAllocHelpers.ResizeList(list, newLength);
        }
    }
}