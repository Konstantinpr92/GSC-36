using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSC_36
{
   public class Ercurve : Primitive
    {
        int WCurve = 2;
        Color ColorCurve = Color.Black;
        List<Point> Xl = new List<Point>();
        List<Point> Xr = new List<Point>();
        List<PointF> AllPoints = new List<PointF>();

        public Ercurve(Color c, int w)
        {
            VertexList = new List<PointF>();
            ColorCurve = c;
            WCurve = w;
        }

        public override void Fill(Graphics g, Pen DPen)
        {
            Xl.Clear();
            Xr.Clear();
            AllPoints.Clear();



            Pen DrawPen = new Pen(ColorCurve, WCurve);
            double h0, h1, h2, h3;
            for (double t = 0; t <= 1; t += 0.0001)
            {
                h0 = 2 * t * t * t - 3 * t * t + 1;
                h1 = -2 * t * t * t + 3 * t * t;
                h2 = t * t * t - 2 * t * t + t;
                h3 = t * t * t - t * t;

                double ptx = VertexList[0].X * h0 + VertexList[2].X * h1 + (VertexList[1].X - VertexList[0].X) * h2 + (VertexList[3].X - VertexList[2].X) * h3;


                double pty = VertexList[0].Y * h0 + VertexList[2].Y * h1 + (VertexList[1].Y - VertexList[0].Y) * h2 + (VertexList[3].Y - VertexList[2].Y) * h3;

                PointF np = new PointF(0, 0);
                np.X = (float)ptx;
                np.Y = (float)pty;
                AllPoints.Add(np);

                Xl.Add(new Point((int)ptx, (int)pty));
                Xr.Add(new Point((int)ptx+2, (int)pty));

                Xl = Xl.Distinct().ToList();
                Xr = Xr.Distinct().ToList();

                g.DrawRectangle(DrawPen, (float)ptx, (float)pty, 1, 1);
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
            double x0 = (VertexList[0].X + VertexList[2].X) / 2.0;
            double y0 = (VertexList[0].Y + VertexList[2].Y) / 2.0;
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
            throw new NotImplementedException();
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
