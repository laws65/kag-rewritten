using System;
using Jint;
using Jint.Native;
using Jint.Native.Object;
using Jint.Runtime.Interop;
using UnityEngine;

namespace KAG.Runtime.Types
{
    [Serializable]
    public class KColor
    {
        public byte r = 0;
        public byte g = 0;
        public byte b = 0;
        public byte a = 0;

        public KColor(byte r, byte g, byte b, byte a = 255)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public KColor(Color32 color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }

        public string ToHtmlRGB()
        {
            return ColorUtility.ToHtmlStringRGB(new Color32(r, g, b, a));
        }

        public string ToHtmlRGBA()
        {
            return ColorUtility.ToHtmlStringRGBA(new Color32(r, g, b, a));
        }
    }
}
