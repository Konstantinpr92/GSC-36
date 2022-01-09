using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSC_36
{

    public class Star : Primitive
    {
        int N = 5;
        int r = 60;
        Color ColorStar = Color.Black;
        double x0;
        double y0;
        PointF[] points;
        int countFfill = 0;
        List<Point> Xl = new List<Point>();
        List<Point> Xr = new List<Point>();

        public Star(Color C, int N, int R)
        {
            Xl = new List<Point>();
            Xr = new List<Point>();

            ColorStar = C;
            this.N = N;
            r = R;
            VertexList = new List<PointF>();
        }
        public void Add(Point NewVertex)
        {
            VertexList.Add(NewVertex);
        }

        //https://www.cyberforum.ru/windows-forms/thread1112848.html
        public override void Fill(Graphics g, Pen DPen)
        {
            Xr.Clear();
            Xl.Clear();
            Pen DrawPen = new Pen(ColorStar, 1);
            int nV = N;               // число вершин
            double R = r/2.0;   // радиус внутренний
            double alpha = 0;        // поворот
            if (countFfill == 0)
            { x0 = VertexList[0].X; y0 = VertexList[0].Y;  // центр

            points = new PointF[2 * nV + 1];
            double a = alpha, da = Math.PI / nV, l;
            for (int i = 0; i < 2 * nV + 1; i++)
            {
                l = i % 2 == 0 ? r : R;
                points[i] = new PointF((float)(x0 + l * Math.Cos(a)), (float)(y0 + l * Math.Sin(a)));
                a += da;
            }

            g.DrawLines(DrawPen, points);
                    countFfill++;
                VertexList.Clear();
                foreach (PointF p in points) {
                    VertexList.Add(p);
                }
            }

            List<Point> PointL = new List<Point>();
            Point P1, P2;
            P1 = new Point();
            int n = VertexList.Count() - 1, k = 0;
            int Ymin = (int)Math.Round(VertexList[0].Y);
            int Ymax = Ymin, Y = 0, X;
            for (int i = 0; i <= n; i++)
            {
                P1.X = (int)Math.Round(VertexList[i].X);
                P1.Y = (int)Math.Round(VertexList[i].Y);
                PointL.Add(P1);
                if (P1.Y < Ymin) Ymin = P1.Y;
                if (P1.Y > Ymax) Ymax = P1.Y;
            }

            List<int> Xb = new List<int>();
            double x;
            P1.X = 0; P1.Y = 0; P2 = P1;

            for (Y = Ymin; Y <= Ymax; Y++)
            {
                Xb.Clear();
                for (int i = 0; i <= n; i++)
                {
                    if (i < n) k = i + 1; else k = 0;
                    if ((PointL[i].Y < Y) & (PointL[k].Y >= Y) | (PointL[i].Y >= Y) & (PointL[k].Y < Y))
                    {
                        x = (Y - PointL[i].Y) * (PointL[k].X - PointL[i].X) / (PointL[k].Y - PointL[i].Y) + PointL[i].X;
                        X = (int)Math.Round(x);
                        Xb.Add(X);
                    }
                }
                Xb.Sort();  // по умолчанию по возрастанию
                for (int i = 0; i < Xb.Count; i = i + 2)
                {
                    P1.X = Xb[i]; P1.Y = Y;
                    P2.X = Xb[i + 1]; P2.Y = Y;

                    g.DrawLine(DrawPen, P1, P2);
                    Xl.Add(P1);
                    Xr.Add(P2);
                }
            }
            PointL.Clear();

        }

        public override void Move(int dx, int dy)
        {
            int n = VertexList.Count() - 1;
            PointF fP = new PointF();
            x0 = x0 + dx;
            y0 = y0 + dy;

            for (int i = 0; i <= n; i++)
            {
                fP.X = VertexList[i].X + dx;
                fP.Y = VertexList[i].Y + dy;
                VertexList[i] = fP;
                
            }

           
        }

        public override void ReflectCentral(int dx, int dy)
        {

            x0 -= dx;
            y0 -= dy;



            x0 = -x0;
            y0 = -y0;




            x0 += dx;
            y0 += dy;


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
            
                x0  -= dx;

                x0 = -x0;

                x0 += dx;


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
                fP.X = VertexList[i].X + dx;
                VertexList[i] = fP;
            }



        }

        public override void Rotate(double ang)
        {
            double rad = ang * (Math.PI / 180.0);
            for (int i = 0; i < VertexList.Count; i++)
            {
                double dx = VertexList[i].X - x0;
                double dy = VertexList[i].Y - y0;

                double ptX = x0 + (dx * Math.Cos(rad) - dy * Math.Sin(rad));
                double ptY = y0 + (dx * Math.Sin(rad) + dy * Math.Cos(rad));

                VertexList[i] = new PointF((float)ptX, (float)ptY);

            }
        }

        public override bool ThisPgn(int mX, int mY)
        {
            // преобразование координат в int
            List<Point> PointL = new List<Point>();
            Point P1;
            P1 = new Point();
            int n = VertexList.Count() - 1, k = 0;
            int Y = mY, X;
            double x;
            List<int> Xb = new List<int>(); // буфер сегментов
            bool check = false;

            for (int i = 0; i <= n; i++)
            {
                P1.X = (int)Math.Round(VertexList[i].X);
                P1.Y = (int)Math.Round(VertexList[i].Y);
                PointL.Add(P1);
            }

            Xb.Clear();
            for (int i = 0; i <= n; i++)
            {
                if (i < n) k = i + 1; else k = 0;
                if ((PointL[i].Y < Y) & (PointL[k].Y >= Y) | (PointL[i].Y >= Y) & (PointL[k].Y < Y))
                {
                    x = (Y - PointL[i].Y) * (PointL[k].X - PointL[i].X) / (PointL[k].Y - PointL[i].Y) + PointL[i].X;
                    X = (int)Math.Round(x);
                    Xb.Add(X);
                }
            }
            if (Xb.Count() > 0)
            {
                Xb.Sort();  // по умолчанию по возрастанию
                for (int i = 0; i < Xb.Count; i = i + 2)
                {
                    if (mX >= Xb[i] & mX <= Xb[i + 1]) { check = true; break; }
                }
            }
            PointL.Clear();
            return check;
        }

        public override List<Point> getxl()
        {
            return Xl;
        }

        public override List<Point> getxr()
        {
            return Xr;
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

            double ang = rad / ((Math.PI / 180.0));
            Rotate(ang);
        }
    }
}
