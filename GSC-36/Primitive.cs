using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSC_36
{
    // абстрактный класс для определения методов
    public abstract class Primitive
    {
        //список точек-вершин
        public List<PointF> VertexList;

        public Primitive()
        {
            VertexList = new List<PointF>();
        }

        abstract public void Fill(Graphics g, Pen DrawPen);

        abstract public bool ThisPgn(int mX, int mY);

       abstract public void Move(int dx, int dy);

        abstract public void ReflectVertical(int dx);
        
        //отражение относительно заданного центра
        abstract public void ReflectCentral(int dx, int dy);

        //вращение
        abstract public void Rotate(double ang);
        abstract public void RotateMouse(Graphics g, int x1, int y1, int x2, int y2);
        //массив точек левой границы
        abstract public List<Point> getxl();

        //массив точек правой граниы
        abstract public List<Point> getxr();
    }
}
