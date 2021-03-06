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
        List<Point> Xl = new List<Point>();
        List<Point> Xr = new List<Point>();

        //список для хранения вспомогательного многоугольника для получения правых и левых границ для ТМО
        List<Point> auxilVertexLixtForTMO = new List<Point>();

        public Boolean FirstPoint = true;
        int WLine = 2;
        Color ColorLine = Color.Black;

        List<Point> AllPoints = new List<Point>();
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
            int counterForPoints = 0;
            //4 точки для вспомогательного многоугольника 
            Point AuxPoint1 = new Point();

            Point AuxPoint2 = new Point();

            Point AuxPoint3 = new Point();

            Point AuxPoint4 = new Point();

            auxilVertexLixtForTMO.Clear();

            AllPoints.Clear();
            Pen DrawPen = new Pen(ColorLine, WLine);

            //из методических указаний
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

                    if (counterForPoints == 0)
                    {
                        AuxPoint1 = new Point(x, y + WLine / 2);
                        AuxPoint2 = new Point(x, y - WLine / 2);
                        counterForPoints++;
                    }


                    g.DrawRectangle(DrawPen, x, y, 1, 1);

                    /*              for (int i = 0; i < WLine; i++)

                                  {
                                      Xl.Add(new Point(x, y + i ));
                                      Xl.Add(new Point(x, y - i ));
                                      Xr.Add(new Point(x , y + i ));
                                      Xr.Add(new Point(x, y - i ));
                                  }*/


                    if (x == VertexList[1].X)
                    {
                        AuxPoint4 = new Point(x, y + WLine / 2);
                        AuxPoint3 = new Point(x, y - WLine / 2);

                        break;
                    };
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
                    if (counterForPoints == 0)
                    {
                        AuxPoint1 = new Point(x - WLine / 2, y);
                        AuxPoint2 = new Point(x + WLine / 2, y);
                        counterForPoints++;
                    }
                    g.DrawRectangle(DrawPen, x, y, 1, 1);

                    /*                    Xl.Add(new Point(x - WLine, y));
                                        Xr.Add(new Point(x + WLine, y));*/

                    if (y == VertexList[1].Y)
                    {
                        AuxPoint4 = new Point(x - WLine / 2, y);
                        AuxPoint3 = new Point(x + WLine / 2, y);

                        break;
                    }
                    Fy = F - dFy;
                    F = Fy + dFx;
                    y = y + Sy;
                    if (Math.Abs(Fy) < Math.Abs(F)) F = Fy;
                    else x = x + Sx;
                } while (true);
            }
            //получение многоугольника и его правой и левой  границ для ТМО
            auxilVertexLixtForTMO.Add(AuxPoint1);
            auxilVertexLixtForTMO.Add(AuxPoint2);
            auxilVertexLixtForTMO.Add(AuxPoint3);
            auxilVertexLixtForTMO.Add(AuxPoint4);
            Xr.Clear();
            Xl.Clear();
            // преобразование координат в int
            List<Point> PointL = new List<Point>();
            Point P1, P2;
            P1 = new Point();
            int n = auxilVertexLixtForTMO.Count() - 1, k = 0;
            int Ymin = (int)(auxilVertexLixtForTMO[0].Y);
            int Ymax = Ymin, Y = 0, X;
            for (int i = 0; i <= n; i++)
            {
                P1.X = (int)(auxilVertexLixtForTMO[i].X);
                P1.Y = (int)(auxilVertexLixtForTMO[i].Y);
                PointL.Add(P1);
                if (P1.Y < Ymin) Ymin = P1.Y;
                if (P1.Y > Ymax) Ymax = P1.Y;
            }

            List<int> Xb = new List<int>();
            double xx;
            P1.X = 0; P1.Y = 0; P2 = P1;

            for (Y = Ymin; Y <= Ymax; Y++)
            {
                Xb.Clear();
                for (int i = 0; i <= n; i++)
                {
                    if (i < n) k = i + 1; else k = 0;
                    if ((PointL[i].Y < Y) & (PointL[k].Y >= Y) | (PointL[i].Y >= Y) & (PointL[k].Y < Y))
                    {
                        xx = (Y - PointL[i].Y) * (PointL[k].X - PointL[i].X) / (PointL[k].Y - PointL[i].Y) + PointL[i].X;
                        X = (int)Math.Round(xx);
                        Xb.Add(X);
                    }
                }
                Xb.Sort();  // по умолчанию по возрастанию
                for (int i = 0; i < Xb.Count; i = i + 2)
                {
                    P1.X = Xb[i]; P1.Y = Y;
                    P2.X = Xb[i + 1]; P2.Y = Y;
                    Xl.Add(P1);
                    Xr.Add(P2);
                }
            }
            PointL.Clear();

        }

        public override List<Point> getxl()
        {
            return Xl;
        }

        public override List<Point> getxr()
        {
            return Xr;
        }

        //перемещение - простое изменение координат кажной точки-вершины на переданную разницу в методе PboMain_MouseMove
        public override void Move(int dx, int dy)
        {
            //int n = VertexList.Count() - 1;
            //PointF fP = new PointF();
            //for (int i = 0; i <= n; i++)
            //{
            //    fP.X = VertexList[i].X + dx;
            //    fP.Y = VertexList[i].Y + dy;
            //    VertexList[i] = fP;
            //}

            //переделаем вычисления с испольованием матрицы преобразования
            // создадим матрицы - многомерные массивы, с страница 49 лекций
            //матрица исходных координат
            MatrixCalc calc = new MatrixCalc();
            double[,] matrixC = new double[1, 3];
            //матрица преобразований для перемещения
            double[,] matrixW = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { dx, dy, 1 } };
            //матрица новых координат, после преобразования
            double[,] matrixCresul = new double[1, 3];


            List<PointF> newVertexList = new List<PointF>();

            for (int i = 0; i < VertexList.Count(); i++)
            {
                matrixC = new double[,] { { VertexList[i].X, VertexList[i].Y, 1 } };

                matrixCresul = new double[1, 3];

                matrixCresul = calc.MatrixMult(matrixC, matrixW);
                newVertexList.Add(new PointF((float)matrixCresul[0, 0], (float)matrixCresul[0, 1]));
            }
            VertexList = newVertexList;
        }


        /*  
         *  https://habr.com/ru/post/426387/
         *  Зеркальное отражение относительно оси абсцисс приведет к тому, что координата x точки не меняется, а координата y меняет знак на противоположный
 x' = x, y' = - y.
 При отражении относительно оси ординат знак меняется у координаты x, а у координаты y знак не изменяется
 x' = - x, y' = y. (4)
 Отражение относительно начала координат изменяет знаки на противоположные у обеих координат:
 x' = - x, y' = - y. (5)
 В том случае, если отражение производится относительно произвольной оси, то до выполнения отражения необходимо будет выполнить параллельный перенос объекта и оси на вектор a так, чтобы ось отражения совпала с одной из координатных осей, затем выполнить отражение, а после этого выполнить параллельный перенос на вектор(− a).
 При отражении относительно произвольного полюса вначале выполняется параллельный перенос, совмещающий полюс с началом координат, затем отражение относительно начала координат и после этого параллельный перенос для возврата полюса в первоначальное положение.*/
        public override void ReflectCentral(int dx, int dy)
        {


            //PointF fP = new PointF();
            //for (int i = 0; i < VertexList.Count(); i++)
            //{
            //    fP.Y = VertexList[i].Y - dy; ;
            //    fP.X = VertexList[i].X - dx;
            //    VertexList[i] = fP;
            //}

            //for (int i = 0; i < VertexList.Count(); i++)
            //{
            //    fP.Y = -VertexList[i].Y;
            //    fP.X = -VertexList[i].X;
            //    VertexList[i] = fP;
            //}

            //for (int i = 0; i < VertexList.Count(); i++)
            //{
            //    fP.Y = VertexList[i].Y + dy;
            //    fP.X = VertexList[i].X + dx;
            //    VertexList[i] = fP;
            //}

            //переделаем вычисления с испольованием матрицы преобразования
            // создадим матрицы - многомерные массивы, с страница 49 лекций
            //матрица исходных координат
            double[,] matrixC = new double[1, 3];
            //матрица преобразований.
            double[,] matrixW = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { -dx, -dy, 1 } };
            //матрица новых координат, после преобразования
            double[,] matrixCresul = new double[1, 3];

            List<PointF> newVertexList = new List<PointF>();

            MatrixCalc calc = new MatrixCalc();
            for (int i = 0; i < VertexList.Count(); i++)
            {
                matrixC = new double[,] { { VertexList[i].X, VertexList[i].Y, 1 } };

                matrixCresul = new double[1, 3];
                matrixCresul = calc.MatrixMult(matrixC, matrixW);
                newVertexList.Add(new PointF((float)matrixCresul[0, 0], (float)matrixCresul[0, 1]));
            }
            //матрица преобразований для отражения 
            matrixW = new double[,] { { -1, 0, 0 }, { 0, -1, 0 }, { 0, 0, 1 } };
            List<PointF> newVertexList2 = new List<PointF>();
            for (int i = 0; i < newVertexList.Count(); i++)
            {
                matrixC = new double[,] { { newVertexList[i].X, newVertexList[i].Y, 1 } };

                matrixCresul = new double[1, 3];
                matrixCresul = calc.MatrixMult(matrixC, matrixW);
                newVertexList2.Add(new PointF((float)matrixCresul[0, 0], (float)matrixCresul[0, 1]));
            }
            //матрица преобразований для перемещения для компенсации первого перемещения
            matrixW = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { +dx, +dy, 1 } };

            List<PointF> newVertexList3 = new List<PointF>();
            for (int i = 0; i < newVertexList.Count(); i++)
            {
                matrixC = new double[,] { { newVertexList2[i].X, newVertexList2[i].Y, 1 } };

                matrixCresul = new double[1, 3];
                matrixCresul = calc.MatrixMult(matrixC, matrixW);
                newVertexList3.Add(new PointF((float)matrixCresul[0, 0], (float)matrixCresul[0, 1]));
            }

            VertexList = newVertexList3;

        }

        //https://habr.com/ru/post/426387/
        public override void ReflectVertical(int dx)
        {
            //PointF fP = new PointF();
            //for (int i = 0; i < VertexList.Count(); i++)
            //{
            //    fP.Y = VertexList[i].Y;
            //    fP.X = VertexList[i].X - dx;
            //    VertexList[i] = fP;
            //}

            //for (int i = 0; i < VertexList.Count(); i++)
            //{
            //    fP.Y = VertexList[i].Y;
            //    fP.X = -VertexList[i].X;
            //    VertexList[i] = fP;
            //}

            //for (int i = 0; i < VertexList.Count(); i++)
            //{
            //    fP.Y = VertexList[i].Y;
            //    fP.X = VertexList[i].X + dx ;
            //    VertexList[i] = fP;
            //}

            //переделаем вычисления с испольованием матрицы преобразования
            // создадим матрицы - многомерные массивы, с страница 49 лекций
            //матрица исходных координат
            double[,] matrixC = new double[1, 3];
            //матрица преобразований для перемещения для совпадения заданной прямой с осью Y 
            double[,] matrixW = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { -dx, 0, 1 } };
            //матрица новых координат, после преобразования
            double[,] matrixCresul = new double[1, 3];

            List<PointF> newVertexList = new List<PointF>();

            MatrixCalc calc = new MatrixCalc();
            for (int i = 0; i < VertexList.Count(); i++)
            {
                matrixC = new double[,] { { VertexList[i].X, VertexList[i].Y, 1 } };

                matrixCresul = new double[1, 3];
                matrixCresul = calc.MatrixMult(matrixC, matrixW);
                newVertexList.Add(new PointF((float)matrixCresul[0, 0], (float)matrixCresul[0, 1]));
            }
            //матрица преобразований для отражения относительно Y
            matrixW = new double[,] { { -1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            List<PointF> newVertexList2 = new List<PointF>();
            for (int i = 0; i < newVertexList.Count(); i++)
            {
                matrixC = new double[,] { { newVertexList[i].X, newVertexList[i].Y, 1 } };

                matrixCresul = new double[1, 3];
                matrixCresul = calc.MatrixMult(matrixC, matrixW);
                newVertexList2.Add(new PointF((float)matrixCresul[0, 0], (float)matrixCresul[0, 1]));
            }
            //матрица преобразований для перемещения для компенсации первого перемещения
            matrixW = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { +dx, 0, 1 } };

            List<PointF> newVertexList3 = new List<PointF>();
            for (int i = 0; i < newVertexList.Count(); i++)
            {
                matrixC = new double[,] { { newVertexList2[i].X, newVertexList2[i].Y, 1 } };

                matrixCresul = new double[1, 3];
                matrixCresul = calc.MatrixMult(matrixC, matrixW);
                newVertexList3.Add(new PointF((float)matrixCresul[0, 0], (float)matrixCresul[0, 1]));
            }

            VertexList = newVertexList3;
        }

        //вращение относительно центра
        public override void Rotate(double ang)
        {

            double rad = ang * (Math.PI / 180.0);
            //ищем координаты центра
            double x0 = (VertexList[0].X + VertexList[1].X) / 2.0;
            double y0 = (VertexList[0].Y + VertexList[1].Y) / 2.0;
            //https://habr.com/ru/post/342674/
            //https://blog.foolsoft.ru/c-povorot-figury-otnositelno-zadannoj-tochki-ili-nachala-koordinat/
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
            //  https://stackoverflow.com/questions/1211212/how-to-calculate-an-angle-from-three-points

            //ищем координаты центра
            double x0 = (VertexList[0].X + VertexList[1].X) / 2.0;
            double y0 = (VertexList[0].Y + VertexList[1].Y) / 2.0;
            Point c = new Point((int)x0, (int)y0);

            //вчпомогательная точка
            //double x1 = (VertexList[0].X + VertexList[1].X) / 2.0;
            //double y1 = (VertexList[0].Y + VertexList[1].Y) / 2.0 - 1000;

            Point p0 = new Point((int)x1, (int)y1);
            Point p1 = new Point((int)x2, (int)y2);

            var p0c = Math.Sqrt(Math.Pow(c.X - p0.X, 2) +
                                Math.Pow(c.Y - p0.Y, 2)); // p0->c (b)   
            var p1c = Math.Sqrt(Math.Pow(c.X - p1.X, 2) +
                                Math.Pow(c.Y - p1.Y, 2)); // p1->c (a)
            var p0p1 = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) +
                                 Math.Pow(p1.Y - p0.Y, 2)); // p0->p1 (c)
            double ang = Math.Acos((p1c * p1c + p0c * p0c - p0p1 * p0p1) / (2 * p1c * p0c));

            double rad = ang; //* (Math.PI / 180.0);
            g.DrawLine(new Pen(Color.Green), c.X, c.Y, p0.X, p0.Y);
            g.DrawLine(new Pen(Color.Green), c.X, c.Y, p1.X, p1.Y);
            // Task.Delay(1500).Wait();


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
