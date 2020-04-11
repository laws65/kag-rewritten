using System;
using UnityEngine;

namespace KAG.Runtime.Types
{
    [Serializable]
    public class KColor : KType
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

        public KColor(string html)
        {
            if (ColorUtility.TryParseHtmlString("#" + html, out Color color))
            {
                Color32 color32 = color;
                r = color32.r;
                g = color32.g;
                b = color32.b;
                a = color32.a;
            }
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
