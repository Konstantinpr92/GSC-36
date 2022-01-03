using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSC_36
{
    class Line : Primitive
    {
        
       public Boolean FirstPoint = true;
       int WLine = 2;
       Color ColorLine = Color.Black;

        List <Point> AllPoints = new List<Point> ();
        public Line(Color c, int w)
        {
            ColorLine = c;
            WLine = w;
            VertexList = new List<PointF>();
        }
        public void Add(Point NewVertex)
        {
            VertexList.Add(NewVertex);
        }

        public override void Fill(Graphics g, Pen DPen)
        {
            AllPoints.Clear();
            Pen DrawPen = new Pen(ColorLine, WLine);

            int x, y, dx, dy, Sx = 0, Sy = 0;
            int F = 0, Fx = 0, dFx = 0, Fy = 0, dFy = 0;
            dx = (int)VertexList[1].X - (int)VertexList[0].X;
            dy = (int)VertexList[1].Y - (int)VertexList[0].Y;
            Sx = Math.Sign(dx);
            Sy = Math.Sign(dy);
            if (Sx > 0) dFx = dy;
            else dFx = -dy;
            if (Sy > 0) dFy = dx;
            else dFy = -dx;
            x = (int)VertexList[0].X; y = (int)VertexList[0].Y;
            F = 0;
            
            if (Math.Abs(dx) >= Math.Abs(dy)) // угол наклона <= 45 градусов
            {
                do
                { //Вывести пиксель с координатами х, у
                    Point np = new Point(0, 0);
                    np.X = x;
                    np.Y = y;
                    AllPoints.Add(np);
                    g.DrawRectangle(DrawPen, x, y, 1,1);
                    if (x == VertexList[1].X) break;
                    Fx = F + dFx;
                    F = Fx - dFy;
                    x = x + Sx;
                    if (Math.Abs(Fx) < Math.Abs(F)) F = Fx;
                    else y = y + Sy;
                } while (true);
            }
            else // угол наклона > 45 градусов
            {
                do
                { //Вывести пиксель с координатами х, у
                    Point np = new Point(0, 0);
                    np.X = x;
                    np.Y = y;
                    AllPoints.Add(np);
                    g.DrawRectangle(DrawPen, x, y, 1, 1);
                    if (y == VertexList[1].Y) break;
                    Fy = F - dFy;
                    F = Fy + dFx;
                    y = y + Sy;
                    if (Math.Abs(Fy) < Math.Abs(F)) F = Fy;
                    else x = x + Sx;
                } while (true);
            }
        }

        public override List<Point> getxl()
        {
            throw new NotImplementedException();
        }

        public override List<Point> getxr()
        {
            throw new NotImplementedException();
        }

        public override void Move(int dx, int dy)
        {
            int n = VertexList.Count() - 1;
            PointF fP = new PointF();
            for (int i = 0; i <= n; i++)
            {
                fP.X = VertexList[i].X + dx;
                fP.Y = VertexList[i].Y + dy;
                VertexList[i] = fP;
            }
        }

        public override void ReflectCentral(int dx, int dy)
        {


            PointF fP = new PointF();
            for (int i = 0; i < VertexList.Count(); i++)
            {
                fP.Y = VertexList[i].Y - dy; ;
                fP.X = VertexList[i].X - dx;
                VertexList[i] = fP;
            }

            for (int i = 0; i < VertexList.Count(); i++)
            {
                fP.Y = -VertexList[i].Y;
                fP.X = -VertexList[i].X;
                VertexList[i] = fP;
            }

            for (int i = 0; i < VertexList.Count(); i++)
            {
                fP.Y = VertexList[i].Y + dy;
                fP.X = VertexList[i].X + dx;
                VertexList[i] = fP;
            }
        }

        public override void ReflectVertical(int dx)
        {
            PointF fP = new PointF();
            for (int i = 0; i < VertexList.Count(); i++)
            {
                fP.Y = VertexList[i].Y;
                fP.X = VertexList[i].X - dx;
                VertexList[i] = fP;
            }

            for (int i = 0; i < VertexList.Count(); i++)
            {
                fP.Y = VertexList[i].Y;
                fP.X = -VertexList[i].X;
                VertexList[i] = fP;
            }

            for (int i = 0; i < VertexList.Count(); i++)
            {
                fP.Y = VertexList[i].Y;
                fP.X = VertexList[i].X + dx ;
                VertexList[i] = fP;
            }


        }

        public override void Rotate(double ang)
        {
            double rad = ang * (Math.PI / 180.0);

            double x0 = (VertexList[0].X + VertexList[1].X) / 2.0;
            double y0 = (VertexList[0].Y + VertexList[1].Y) / 2.0;
            for (int i = 0; i < VertexList.Count; i++)
            {
                int dx = (int)Math.Round(VertexList[i].X - x0);
                int dy = (int)Math.Round(VertexList[i].Y - y0);

                double ptX = x0 + (dx * Math.Cos(rad) - dy * Math.Sin(rad));
                double ptY = y0 + (dx * Math.Sin(rad) + dy * Math.Cos(rad));

                VertexList[i] = new PointF((int)Math.Round(ptX), (int)Math.Round(ptY));

            }
        }

        public override bool ThisPgn(int mX, int mY)
        {
  
            bool check = false;

            foreach (var p in AllPoints)
            {
                if (Math.Abs(p.X - mX) < 5 && Math.Abs(p.Y - mY)<5 ) {
                    check = true;
                    break;
                }
            }

            return check;
        }
    }
}
