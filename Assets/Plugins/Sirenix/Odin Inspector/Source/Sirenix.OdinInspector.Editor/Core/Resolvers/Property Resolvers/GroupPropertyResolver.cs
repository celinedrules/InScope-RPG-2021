//-----------------------------------------------------------------------
// <copyright file="GroupPropertyResolver.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
#pragma warning disable

    using Sirenix.Utilities.Editor;
    using System.Collections.Generic;

    [ResolverPriority(-5)]
    public class GroupPropertyResolver : OdinPropertyResolver
    {
        private InspectorPropertyInfo[] groupInfos;
        private Dictionary<StringSlice, int> nameToIndexMap = new Dictionary<StringSlice, int>(StringSliceEqualityComparer.Instance);

        public override bool CanResolveForPropertyFilter(InspectorProperty property)
        {
            return property.Info.PropertyType == PropertyType.Group;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.groupInfos = this.Property.Info.GetGroupInfos();

            for (int i = 0; i < this.groupInfos.Length; i++)
            {
                this.nameToIndexMap[this.groupInfos[i].PropertyName] = i;
            }
        }

        public override int ChildNameToIndex(string name)
        {
            int index;
            if (this.nameToIndexMap.TryGetValue(name, out index)) return index;
            return -1;
        }

        public override int ChildNameToIndex(ref StringSlice name)
        {
            int index;
            if (this.nameToIndexMap.TryGetValue(name, out index)) return index;
            return -1;
        }

        public override InspectorPropertyInfo GetChildInfo(int childIndex)
        {
            return this.groupInfos[childIndex];
        }

        protected override int CalculateChildCount()
        {
            return this.groupInfos.Length;
        }
    }
}
#endif