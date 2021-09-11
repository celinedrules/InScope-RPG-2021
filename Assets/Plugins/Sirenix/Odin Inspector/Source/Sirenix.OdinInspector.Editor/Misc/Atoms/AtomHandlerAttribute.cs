//-----------------------------------------------------------------------
// <copyright file="AtomHandlerAttribute.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
#pragma warning disable

    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class AtomHandlerAttribute : Attribute
    {
    }
}
#endif