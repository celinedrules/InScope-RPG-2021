//-----------------------------------------------------------------------
// <copyright file="IOrderedCollectionResolver.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
#pragma warning disable

    public interface IOrderedCollectionResolver : ICollectionResolver
    {
        void QueueRemoveAt(int index);

        void QueueRemoveAt(int index, int selectionIndex);

        void QueueInsertAt(int index, object[] values);

        void QueueInsertAt(int index, object value, int selectionIndex);
    }
}
#endif