using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSC_36
{
    public abstract class Primitive
    {
        public List<PointF> VertexList;

        public Primitive()
        {
            VertexList = new List<PointF>();
        }
        abstract public void Fill(Graphics g, Pen DrawPen);
        abstract public bool ThisPgn(int mX, int mY);
        abstract public void Move(int dx, int dy);
        abstract public void ReflectVertical(int dx);
        abstract public void ReflectCentral(int dx, int dy);
        abstract public void Rotate(double ang);
        abstract public List<Point> getxl();
        abstract public List<Point> getxr();
    }
}
