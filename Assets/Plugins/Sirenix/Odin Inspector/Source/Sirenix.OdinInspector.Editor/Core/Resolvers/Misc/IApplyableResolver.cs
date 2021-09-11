//-----------------------------------------------------------------------
// <copyright file="IApplyableResolver.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
#pragma warning disable

    public interface IApplyableResolver
    {
        bool ApplyChanges();
    }
}
#endif