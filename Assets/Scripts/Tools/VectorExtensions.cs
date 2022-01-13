using UnityEngine;

namespace VectorExtensions
{
    public static class VectorExtension
    {
        // 扩展Vector3方法
        public static Vector2 XY(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }
    }
}