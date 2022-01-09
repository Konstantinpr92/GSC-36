using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSC_36
{
    public class TMOResult : Primitive
    {
        Graphics g;
        List<Point> AllPoints = new List<Point>();
        List<Point> Xl = new List<Point>();
        List<Point> Xr = new List<Point>();
        List<Point> T = new List<Point>();
        int x0;
        int y0;
        public TMOResult(List<Point> XlO, List<Point> XrO)
        {
            this.Xl = XlO;
            this.Xr = XrO;

           int X1 = XlO.Max(point => point.X);
           int X2 = XrO.Max(point => point.X);
           int XMax = Math.Max(X1, X2);

             X1 = XlO.Min(point => point.X);
             X2 = XrO.Min(point => point.X);
            int Xmin = Math.Min(X1, X2);


            int Y1 = XlO.Max(point => point.Y);
            int Y2 = XrO.Max(point => point.Y);
            int YMax = Math.Max(Y1, Y2);

            Y1 = XlO.Min(point => point.Y);
            Y2 = XrO.Min(point => point.Y);
            int Ymin = Math.Min(Y1, Y2);

            x0 = Xmin + (XMax - Xmin) / 2;
            y0 = Ymin + (YMax - Ymin) / 2;

        }
        public override void Fill(Graphics g, Pen DrawPen)
        {
            this.g = g;
            for (int i = 0; i < Xl.Count; i++)
            {
                g.DrawLine(new Pen(Color.Cyan, 2), Xl[i], Xr[i]);
            }
        }

        public override List<Point> getxl()
        {
            return Xl;
        }

        public override List<Point> getxr()
        {
            return Xr;
        }

        public override void Move(int dx, int dy)
        {
            List<Point> XlNew = new List<Point>();
            List<Point> XrNew = new List<Point>();

            for (int i = 0; i < Xr.Count; i++)
            {
                XrNew.Add(new Point(Xr[i].X + dx, Xr[i].Y + dy));
                XlNew.Add(new Point(Xl[i].X + dx, Xl[i].Y + dy));
            }
            Xl = XlNew;
            Xr = XrNew;
            y0 += dy;
            x0 += dx;
        }

        public override void ReflectCentral(int dx, int dy)
        {
            x0 -= dx;
            y0 -= dy;



            x0 = -x0;
            y0 = -y0;




            x0 += dx;
            y0 += dy;


            Point fP = new Point();
            for (int i = 0; i < Xr.Count(); i++)
            {
                fP.Y = Xr[i].Y - dy; ;
                fP.X = Xr[i].X - dx;
                Xr[i] = fP;

                fP.Y = Xl[i].Y - dy; ;
                fP.X = Xl[i].X - dx;
                Xl[i] = fP;
            }

            for (int i = 0; i < Xr.Count(); i++)
            {
                fP.Y = -Xr[i].Y;
                fP.X = -Xr[i].X;
                Xr[i] = fP;
                fP.Y = -Xl[i].Y;
                fP.X = -Xl[i].X;
                Xl[i] = fP;
            }

            for (int i = 0; i < Xr.Count(); i++)
            {
                fP.Y = Xr[i].Y + dy;
                fP.X = Xr[i].X + dx;
                Xr[i] = fP;
                fP.Y = Xl[i].Y + dy;
                fP.X = Xl[i].X + dx;
                Xl[i] = fP;
            }
            T = Xr;
            Xr = Xl;
            Xl = T;
        }

        public override void ReflectVertical(int dx)
        {

            x0 -= dx;

            x0 = -x0;

            x0 += dx;


            Point fP = new Point();
            for (int i = 0; i < Xl.Count(); i++)
            {
                fP.Y = Xr[i].Y;
                fP.X = Xr[i].X - dx;
                Xr[i] = fP;

                fP.Y = Xl[i].Y;
                fP.X = Xl[i].X - dx;
                Xl[i] = fP;
            }

            for (int i = 0; i < Xl.Count(); i++)
            {
                fP.Y = Xr[i].Y;
                fP.X = -Xr[i].X;
                Xr[i] = fP;

                fP.Y = Xl[i].Y;
                fP.X = -Xl[i].X;
                Xl[i] = fP;
            }

            for (int i = 0; i < Xl.Count(); i++)
            {
                fP.Y = Xr[i].Y;
                fP.X = Xr[i].X + dx;
                Xr[i] = fP;

                fP.Y = Xl[i].Y;
                fP.X = Xl[i].X + dx;
                Xl[i] = fP;
            }

            T = Xr;
            Xr = Xl;
            Xl = T;
        }

        public override void Rotate(double ang)
        {
            
            g.DrawRectangle(new Pen(Color.Red, 1), x0, y0, 10, 10);

            double rad = ang * (Math.PI / 180.0);
            for (int i = 0; i < Xr.Count; i++)
            {
                double dx = Xr[i].X - x0;
                double dy = Xr[i].Y - y0;

                double ptX = x0 + (dx * Math.Cos(rad) - dy * Math.Sin(rad));
                double ptY = y0 + (dx * Math.Sin(rad) + dy * Math.Cos(rad));

                Xr[i] = new Point((int)ptX, (int)ptY);

                 dx = Xl[i].X - x0;
                 dy = Xl[i].Y - y0;

                 ptX = x0 + (dx * Math.Cos(rad) - dy * Math.Sin(rad));
                 ptY = y0 + (dx * Math.Sin(rad) + dy * Math.Cos(rad));

                Xl[i] = new Point((int)ptX, (int)ptY);

            }
        }

        public override void RotateMouse(Graphics g, int x1, int y1, int x2, int y2)
        {
            
            Point c = new Point((int)x0, (int)y0);
            Point p0 = new Point((int)x1, (int)y1);
            Point p1 = new Point((int)x2, (int)y2);
            var p0c = Math.Sqrt(Math.Pow(c.X - p0.X, 2) +
                    Math.Pow(c.Y - p0.Y, 2)); // p0->c (b)   
            var p1c = Math.Sqrt(Math.Pow(c.X - p1.X, 2) +
                                Math.Pow(c.Y - p1.Y, 2)); // p1->c (a)
            var p0p1 = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) +
                                 Math.Pow(p1.Y - p0.Y, 2)); // p0->p1 (c)
            double rad = Math.Acos((p1c * p1c + p0c * p0c - p0p1 * p0p1) / (2 * p1c * p0c));

            double ang = rad/((Math.PI / 180.0));
            Rotate(ang);

        }

        public override bool ThisPgn(int mX, int mY)
        {
            AllPoints.Clear();
            for (int k = 0; k < Xr.Count; k++)
            {
                int x, y, dx, dy, Sx = 0, Sy = 0;
                int F = 0, Fx = 0, dFx = 0, Fy = 0, dFy = 0;
               // dx = (int)VertexList[1].X - (int)VertexList[0].X;
               // dy = (int)VertexList[1].Y - (int)VertexList[0].Y;
                dx = (int)Xr[k].X - Xl[k].X;
                dy = (int)Xr[k].Y - Xl[k].Y;
                Sx = Math.Sign(dx);
                Sy = Math.Sign(dy);
                if (Sx > 0) dFx = dy;
                else dFx = -dy;
                if (Sy > 0) dFy = dx;
                else dFy = -dx;
                x = (int)Xl[k].X; y = (int)Xl[k].Y;
                F = 0;

                if (Math.Abs(dx) >= Math.Abs(dy)) // угол наклона <= 45 градусов
                {
                    do
                    { //Вывести пиксель с координатами х, у
                        Point np = new Point(0, 0);
                        np.X = x;
                        np.Y = y;
                        AllPoints.Add(np);
                        // g.DrawRectangle(DrawPen, x, y, 1, 1);
                        if (x == (int)Xr[k].X) break;
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
                       // g.DrawRectangle(DrawPen, x, y, 1, 1);
                        if (y == (int)Xr[k].Y) break;
                        Fy = F - dFy;
                        F = Fy + dFx;
                        y = y + Sy;
                        if (Math.Abs(Fy) < Math.Abs(F)) F = Fy;
                        else x = x + Sx;
                    } while (true);
                }

            }

            bool check = false;

            foreach (var p in AllPoints)
            {
                if (Math.Abs(p.X - mX) < 5 && Math.Abs(p.Y - mY) < 5)
                {
                    check = true;
                    break;
                }
            }

            return check;

        }
    }
}
