//-----------------------------------------------------------------------
// <copyright file="IRecursiveOnChildStateChangedNotification.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
#pragma warning disable

    public interface IRecursiveOnChildStateChangedNotification
    {
        void OnChildStateChanged(InspectorProperty child, string state);
    }
}
#endif