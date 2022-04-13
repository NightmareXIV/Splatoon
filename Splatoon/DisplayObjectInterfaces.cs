using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    internal class DisplayObjectDot : DisplayObject
    {
        public float x, y, z, thickness;
        public uint color;

        public DisplayObjectDot(float x, float y, float z, float thickness, uint color)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.thickness = thickness;
            this.color = color;
        }
    }

    internal class DisplayObjectCircle : DisplayObject
    {
        public float x, y, z, radius, thickness;
        public uint color;
        public bool filled;
        public DisplayObjectCircle(float x, float y, float z, float radius, float thickness, uint color, bool filled)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.radius = radius;
            this.thickness = thickness;
            this.color = color;
            this.filled = filled;
        }
    }

    internal class DisplayObjectLine : DisplayObject
    {
        public float ax, ay, az, bx, by, bz, thickness;
        public uint color;

        public DisplayObjectLine(float ax, float ay, float az, float bx, float by, float bz, float thickness, uint color)
        {
            this.ax = ax;
            this.ay = ay;
            this.az = az;
            this.bx = bx;
            this.by = by;
            this.bz = bz;
            this.thickness = thickness;
            this.color = color;
        }
    }

    internal class DisplayObjectText : DisplayObject
    {
        public float x, y, z;
        public string text;
        public uint bgcolor, fgcolor;

        public DisplayObjectText(float x, float y, float z, string text, uint bgcolor, uint fgcolor)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.text = text;
            this.bgcolor = bgcolor;
            this.fgcolor = fgcolor;
        }
    }

    internal class DisplayObjectRect : DisplayObject
    {
        public DisplayObjectLine l1;
        public DisplayObjectLine l2;
    }

    internal class DisplayObjectQuad : DisplayObject
    {
        public Point3[] rect;
        public uint bgcolor;

        public DisplayObjectQuad(Point3[] rect, uint bgcolor)
        {
            this.rect = rect;
            this.bgcolor = bgcolor;
        }
    }

    internal class DisplayObjectPolygon : DisplayObject
    {
        public Element e;
        public DisplayObjectPolygon(Element e)
        {
            this.e = e;
        }
    }

    internal interface DisplayObject { }

}
