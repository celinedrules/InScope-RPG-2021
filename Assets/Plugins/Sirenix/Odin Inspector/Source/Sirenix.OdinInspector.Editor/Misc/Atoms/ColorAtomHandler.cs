//-----------------------------------------------------------------------
// <copyright file="ColorAtomHandler.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
#pragma warning disable

    using UnityEngine;

    [AtomHandler]
    public sealed class ColorAtomHandler : EquatableStructAtomHandler<Color>
    {
    }

    [AtomHandler]
    public sealed class Color32AtomHandler : EquatableStructAtomHandler<Color32>
    {
    }
}
#endif