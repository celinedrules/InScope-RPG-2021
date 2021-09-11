//-----------------------------------------------------------------------
// <copyright file="IRefreshableResolver.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
#pragma warning disable

    public interface IRefreshableResolver
    {
        bool ChildPropertyRequiresRefresh(int index, InspectorPropertyInfo info);
    }
}
#endif