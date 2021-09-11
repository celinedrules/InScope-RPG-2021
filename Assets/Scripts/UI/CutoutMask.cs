using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI
{
    public class CutoutMask : Image
    {
        public override Material materialForRendering
        {
            get
            {
                Material materialCopy = new Material(base.materialForRendering);
                materialCopy.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
                return materialCopy;
            }
        }
    }
}