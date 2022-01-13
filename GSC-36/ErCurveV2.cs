using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSC_36
{
    public class ErcurveV2 : Primitive
    {
        public double x0;
        public double y0;

        int WCurve = 2;
        Color ColorCurve = Color.Black;
        List<Point> Xl = new List<Point>();
        List<Point> Xr = new List<Point>();
        List<PointF> AllPoints = new List<PointF>();

        public ErcurveV2(Color c, int w)
        {
            VertexList = new List<PointF>();
            ColorCurve = c;
            WCurve = w;
        }
        public override void Fill(Graphics g, Pen DrPen)
        {
            DrPen = new Pen(ColorCurve, WCurve);
            PointF[] P = VertexList.ToArray();
            PointF[] L = new PointF[4]; // Матрица вещественных коэффициентов
            PointF Pv1 = P[0];
            PointF Pv2 = P[0];
            const double dt = 0.0001;
            double t = 0;
            double xt, yt;
            PointF Ppred = P[0], Pt = P[0];
            // Касательные векторы
            Pv1.X = (int)(4 * (P[1].X - P[0].X));
            Pv1.Y = (int)(4 * (P[1].Y - P[0].Y));
            Pv2.X = (int)(4 * (P[3].X - P[2].X));
            Pv2.Y = (int)(4 * (P[3].Y - P[2].Y));
            // Коэффициенты полинома
            L[0].X = 2 * P[0].X - 2 * P[2].X + Pv1.X + Pv2.X; // Ax
            L[0].Y = 2 * P[0].Y - 2 * P[2].Y + Pv1.Y + Pv2.Y; // Ay
            L[1].X = -3 * P[0].X + 3 * P[2].X - 2 * Pv1.X - Pv2.X; // Bx
            L[1].Y = -3 * P[0].Y + 3 * P[2].Y - 2 * Pv1.Y - Pv2.Y; // By
            L[2].X = Pv1.X; // Cx
            L[2].Y = Pv1.Y; // Cy
            L[3].X = P[0].X; // Dx
            L[3].Y = P[0].Y; // Dy
            while (t < 1 + dt / 2)
            {
                xt = ((L[0].X * t + L[1].X) * t + L[2].X) * t + L[3].X;
                yt = ((L[0].Y * t + L[1].Y) * t + L[2].Y) * t + L[3].Y;
                Pt.X = (int)Math.Round(xt);
                Pt.Y = (int)Math.Round(yt);
                g.DrawLine(DrPen, Ppred, Pt);
                //g.DrawRectangle(DrPen, Pt.X, Pt.Y, 1, 1);
                AllPoints.Add(Ppred);

                Xl.Add(new Point((int)Ppred.X, (int)Ppred.Y));
                Xr.Add(new Point((int)Ppred.X + 2, (int)Ppred.Y));

                Xl = Xl.Distinct().ToList();
                Xr = Xr.Distinct().ToList();

                Ppred = Pt;
                t = t + dt;

                double difference = Math.Abs(t * .001);
                PointF np = new PointF(0, 0);
                if (Math.Abs(t - (0.5+dt/4)) <= difference)
                {
                    x0 = Pt.X;
                    y0 = Pt.Y;
                }
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
                fP.X = VertexList[i].X + dx;
                VertexList[i] = fP;
            }
        }

        public override void Rotate(double ang)
        {
            double rad = ang * (Math.PI / 180.0);
               //double x0 = (VertexList[0].X + VertexList[2].X) / 2.0;
               //double y0 = (VertexList[0].Y + VertexList[2].Y) / 2.0;
            for (int i = 0; i < VertexList.Count; i++)
            {
                int dx = (int)Math.Round(VertexList[i].X - x0);
                int dy = (int)Math.Round(VertexList[i].Y - y0);

                double ptX = x0 + (dx * Math.Cos(rad) - dy * Math.Sin(rad));
                double ptY = y0 + (dx * Math.Sin(rad) + dy * Math.Cos(rad));

                VertexList[i] = new PointF((int)Math.Round(ptX), (int)Math.Round(ptY));

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

            double ang = rad / ((Math.PI / 180.0));
            Rotate(ang);
        }

        public override bool ThisPgn(int mX, int mY)
        {
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
