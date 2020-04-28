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

        public KVector3() { }
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

        public KVector2() { }
        public KVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    [Serializable]
    public class KVector3Int
    {
        public int x = 0;
        public int y = 0;
        public int z = 0;

        public KVector3Int() { }
        public KVector3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [Serializable]
    public class KVector2Int
    {
        public int x = 0;
        public int y = 0;

        public KVector2Int() { }
        public KVector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public static class KVectorExtensions
    {
        // Vector3
        public static List<Vector3> ToNative(this IEnumerable<KVector3> runtime)
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

        // Vector2
        public static List<Vector2> ToNative(this IEnumerable<KVector2> runtime)
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

        // Vector3Int
        public static List<Vector3Int> ToNative(this IEnumerable<KVector3Int> runtime)
        {
            List<Vector3Int> native = new List<Vector3Int>();

            foreach (var item in runtime)
            {
                native.Add(item.ToNative());
            }

            return native;
        }

        public static Vector3Int ToNative(this KVector3Int runtime)
        {
            return new Vector3Int(runtime.x, runtime.y, runtime.z);
        }

        // Vector2Int
        public static List<Vector2Int> ToNative(this IEnumerable<KVector2Int> runtime)
        {
            List<Vector2Int> native = new List<Vector2Int>();

            foreach (var item in runtime)
            {
                native.Add(item.ToNative());
            }

            return native;
        }

        public static Vector2Int ToNative(this KVector2Int runtime)
        {
            return new Vector2Int(runtime.x, runtime.y);
        }
    }
}
