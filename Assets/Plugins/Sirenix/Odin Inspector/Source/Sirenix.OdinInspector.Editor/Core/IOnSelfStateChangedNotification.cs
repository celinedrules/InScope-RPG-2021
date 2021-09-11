//-----------------------------------------------------------------------
// <copyright file="IOnSelfStateChangedNotification.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
#pragma warning disable

    public interface IOnSelfStateChangedNotification
    {
        void OnSelfStateChanged(string state);
    }
}
#endif