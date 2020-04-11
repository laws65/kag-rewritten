using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAG.Runtime.Types
{
    [Serializable]
    public class KVector3
    {
        public float x = 0;
        public float y = 0;
        public float z = 0;

        public KVector3(float x, float y, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [Serializable]
    public class KVector2
    {
        public float x = 0;
        public float y = 0;

        public KVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public static class KVectorExtensions
    {
        public static IEnumerable<Vector3> ToNative(this IEnumerable<KVector3> runtime)
        {
            List<Vector3> native = new List<Vector3>();
            foreach (var item in runtime)
            {
                native.Add(item.ToNative());
            }
            return native;
        }

        public static Vector3 ToNative(this KVector3 runtime)
        {
            return new Vector3(runtime.x, runtime.y, runtime.z);
        }

        public static IEnumerable<Vector2> ToNative(this IEnumerable<KVector2> runtime)
        {
            List<Vector2> native = new List<Vector2>();
            foreach (var item in runtime)
            {
                native.Add(item.ToNative());
            }
            return native;
        }

        public static Vector2 ToNative(this KVector2 runtime)
        {
            return new Vector2(runtime.x, runtime.y);
        }
    }
}
