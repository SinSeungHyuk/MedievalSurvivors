using UnityEngine;


namespace MS.Utils
{
    public static class TransformExtensions
    {
        public static Transform FindChildDeep(this Transform parent, string childName)
        {
            foreach (Transform child in parent)
            {
                if (child.name == childName)
                {
                    return child;
                }

                Transform result = FindChildDeep(child, childName);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}