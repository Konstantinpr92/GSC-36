using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GSC_36
{
    public partial class Form1 : Form

    {
        Bitmap myBitmap;
        Point PboMainMousePos = new Point();

        bool checkPgn = false;
        int selector = 0;

        //переменные для хранения настроек прямой
        int WLine = 2;
        Color ColorLine = Color.Black;

        //переменные для хранения настроек кривой
        int WCurve = 2;
        Color ColorCurve = Color.Black;

        //переменные для хранения настроек звезды
        int nForStar = 5;
        Color ColorStar = Color.Black;
        int rForstar = 60;

        //переменные для хранения настроек уголка
        int WforUgl = 20;
        Color ColorUgl = Color.Black;




        // константы для переключения между режимами
        const int DrawLine = 0;
        const int DrawLineStage2 = 100;
        const int DrawCurve = 1;
        const int DravCurveStage2 = 101;
        const int DrawStar = 3;
        const int DrawUgl3 = 2;
        const int SelectAndMove = 5;
        const int SelectAndReflectVertical = 6;
        const int SelectAndReflectVerticalStage2 = 106;
        const int SelectAndReflectCentral = 7;
        const int SelectAndReflectCentralStage2 = 107;
        const int SelectAndRotate = 8;
        int OperationType = 0;

        Graphics g;
        Pen DrawPen = new Pen(Color.Black, 2);
        Pen DrawPenForVecrors = new Pen(Color.Red, 1);

        List<Primitive> PrimitiveList = new List<Primitive>();
        

        public Form1()
        {          
            InitializeComponent();
            g = PboMain.CreateGraphics();
            myBitmap = new Bitmap(PboMain.Width, PboMain.Height);
            g = Graphics.FromImage(myBitmap);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void прямаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationType = 0;
        }

        private void криваяЭрмитаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationType = DrawCurve;
        }

        private void уголок3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationType = DrawUgl3;
        }

        private void звездаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OperationType = DrawStar;
        }

        private void PboMain_MouseDown(object sender, MouseEventArgs e)
        {
            PboMainMousePos = e.Location;
            switch (OperationType)
            {
                case 0:
                    PrimitiveList.Add(new Line(ColorLine, WLine));
                    g.DrawEllipse(new Pen(ColorLine, WLine), e.X, e.Y, 2, 2);
                    OperationType = DrawLineStage2;
                    PrimitiveList[PrimitiveList.Count - 1].VertexList.Add(new PointF(e.X, e.Y));
                    break;
                case 100:
                    g.DrawEllipse(new  Pen(ColorLine, WLine), e.X, e.Y, 2, 2);

                    if (PrimitiveList[PrimitiveList.Count - 1].VertexList.Count == 1)
                    {
                        PrimitiveList[PrimitiveList.Count - 1].VertexList.Add(new PointF(e.X, e.Y));
                        PrimitiveList[PrimitiveList.Count - 1].Fill(g, new Pen(ColorLine, WLine));
                    }
                    OperationType = DrawLine;
                    break;


                case DrawCurve:
                    PrimitiveList.Add(new Ercurve(ColorCurve, WCurve));
                    OperationType = DravCurveStage2;
                    PrimitiveList[PrimitiveList.Count - 1].VertexList.Add(new PointF(e.X, e.Y));
                    g.DrawEllipse(DrawPenForVecrors, e.X, e.Y, 2, 2);
                    break;
                case DravCurveStage2:
                    if (PrimitiveList[PrimitiveList.Count - 1].VertexList.Count == 1)
                    {
                        PrimitiveList[PrimitiveList.Count - 1].VertexList.Add(new PointF(e.X, e.Y));
                        g.DrawLine(DrawPenForVecrors, PrimitiveList[PrimitiveList.Count - 1].VertexList[0], PrimitiveList[PrimitiveList.Count - 1].VertexList[1]);
                        break;
                    }
                    if (PrimitiveList[PrimitiveList.Count - 1].VertexList.Count == 2)
                    {
                        PrimitiveList[PrimitiveList.Count - 1].VertexList.Add(new PointF(e.X, e.Y));
                        g.DrawEllipse(DrawPenForVecrors, e.X, e.Y, 2, 2);
                        break;
                    }
                    if (PrimitiveList[PrimitiveList.Count - 1].VertexList.Count == 3)
                    {
                        PrimitiveList[PrimitiveList.Count - 1].VertexList.Add(new PointF(e.X, e.Y));
                        g.DrawLine(DrawPenForVecrors, PrimitiveList[PrimitiveList.Count - 1].VertexList[3], PrimitiveList[PrimitiveList.Count - 1].VertexList[2]);
                        PrimitiveList[PrimitiveList.Count - 1].Fill(g, DrawPen);
                    }
                    OperationType = DrawCurve;
                    PboMain.Image = myBitmap;

                    Task.Delay(2000).Wait();
                    g.Clear(Color.White);
                    foreach (var p in PrimitiveList)
                    {
                        p.Fill(g, DrawPen);
                    }


                    break;

                case DrawStar:
                    PrimitiveList.Add(new Star(ColorStar, nForStar, rForstar));
                    PrimitiveList[PrimitiveList.Count - 1].VertexList.Add(new PointF(e.X, e.Y));
                    PrimitiveList[PrimitiveList.Count - 1].Fill(g,DrawPen);
                    break;


                case DrawUgl3:
                    PrimitiveList.Add(new Ugl3(ColorUgl, WforUgl));
                    PrimitiveList[PrimitiveList.Count - 1].VertexList.Add(new PointF(e.X, e.Y));
                    PrimitiveList[PrimitiveList.Count - 1].VertexList.Add(new PointF(e.X, e.Y-WforUgl));
                    PrimitiveList[PrimitiveList.Count - 1].VertexList.Add(new PointF(e.X+ WforUgl/3, e.Y- WforUgl/3));
                    PrimitiveList[PrimitiveList.Count - 1].VertexList.Add(new PointF(e.X+ WforUgl, e.Y));
                    PrimitiveList[PrimitiveList.Count - 1].Fill(g, DrawPen);

                    break;

                case SelectAndMove:
                    selector = 0;
                    for (int pgnn = 0; pgnn < PrimitiveList.Count; pgnn ++)
                    {

                        if (PrimitiveList[pgnn].ThisPgn(e.X, e.Y))
                        {
                            selector = pgnn;
                            g.DrawEllipse(new Pen(Color.Blue), e.X - 2, e.Y - 2, 5, 5); ;    // Выбор
                            checkPgn = true; 
                          // MessageBox.Show("Выбран угольник номер " + selector.ToString());
                            break;
                            
                        }
                        else checkPgn = false;

                    }

                    break;

                case SelectAndReflectVertical:
                    selector = 0;
                    for (int pgnn = 0; pgnn < PrimitiveList.Count; pgnn++)
                    {

                        if (PrimitiveList[pgnn].ThisPgn(e.X, e.Y))
                        {
                            selector = pgnn;
                            g.DrawEllipse(new Pen(Color.Blue), e.X - 2, e.Y - 2, 5, 5); ;    // Выбор
                            checkPgn = true;
                            OperationType = SelectAndReflectVerticalStage2;

                            // MessageBox.Show("Выбран угольник номер " + selector.ToString());
                            break;

                        }
                        else checkPgn = false;

                    }

                    break;

                case SelectAndReflectVerticalStage2:
                    PboMain.Image = myBitmap;
                    g.DrawLine(DrawPenForVecrors, e.X, 1000, e.X, -1000);
                    Task.Delay(1500).Wait();
                    PrimitiveList[selector].ReflectVertical(e.X);
                    PrimitiveList[selector].Fill(g, DrawPen);

                    
                    g.Clear(Color.White);
                    PboMain.Image = myBitmap;
                    g.DrawLine(DrawPenForVecrors, e.X, 1000, e.X, -1000);
                    foreach (var p in PrimitiveList)
                    {
                        p.Fill(g, DrawPen);
                    }
                    
                    
                    Task.Delay(1500).Wait();
                    
                    g.Clear(Color.White);
                    PboMain.Image = myBitmap;
                    foreach (var p in PrimitiveList)
                    {
                        p.Fill(g, DrawPen);
                    }

                    OperationType = SelectAndReflectVertical;

                    break;


                case  SelectAndReflectCentral:
                    selector = 0;
                    for (int pgnn = 0; pgnn < PrimitiveList.Count; pgnn++)
                    {

                        if (PrimitiveList[pgnn].ThisPgn(e.X, e.Y))
                        {
                            selector = pgnn;
                            g.DrawEllipse(new Pen(Color.Blue), e.X - 2, e.Y - 2, 5, 5); ;    // Выбор
                            checkPgn = true;
                            OperationType = SelectAndReflectCentralStage2;

                            // MessageBox.Show("Выбран угольник номер " + selector.ToString());
                            break;

                        }
                        else checkPgn = false;

                    };

                    break;

                case SelectAndReflectCentralStage2:

                    PboMain.Image = myBitmap;
                    g.DrawLine(DrawPenForVecrors, e.X, e.Y+20, e.X, e.Y - 20);
                    g.DrawLine(DrawPenForVecrors, e.X +20, e.Y , e.X - 20, e.Y );
                    Task.Delay(1500).Wait();

                    PrimitiveList[selector].ReflectCentral(e.X, e.Y);
                    PrimitiveList[selector].Fill(g, DrawPen);


                    g.Clear(Color.White);
                    PboMain.Image = myBitmap;
                    g.DrawLine(DrawPenForVecrors, e.X, e.Y + 20, e.X, e.Y - 20);
                    g.DrawLine(DrawPenForVecrors, e.X + 20, e.Y, e.X - 20, e.Y);
                    foreach (var p in PrimitiveList)
                    {
                        p.Fill(g, DrawPen);
                    }


                    Task.Delay(1500).Wait();

                    g.Clear(Color.White);
                    PboMain.Image = myBitmap;
                    foreach (var p in PrimitiveList)
                    {
                        p.Fill(g, DrawPen);
                    }

                    OperationType = SelectAndReflectCentral;


                    break;
                case SelectAndRotate:
                    selector = 0;
                    for (int pgnn = 0; pgnn < PrimitiveList.Count; pgnn++)
                    {

                        if (PrimitiveList[pgnn].ThisPgn(e.X, e.Y))
                        {
                            selector = pgnn;
                            g.DrawEllipse(new Pen(Color.Blue), e.X - 2, e.Y - 2, 5, 5); ;    // Выбор
                            checkPgn = true;
                            AskAngForm f = new AskAngForm();

                            DialogResult r = f.ShowDialog();
                            double result;
                            double AnglRotate = 0;
                            if (r == DialogResult.OK)
                            {
                                if (double.TryParse(f.ReturnAnng, out result))
                                {                                   
                                    AnglRotate = result;
                                    PrimitiveList[selector].Rotate(AnglRotate);
                                    g.Clear(Color.White);
                                    PboMain.Image = myBitmap;
                                    foreach (var p in PrimitiveList)
                                    {
                                        p.Fill(g, DrawPen);
                                    }


                                }
                                else
                                {
                                    MessageBox.Show("Неверный ввод");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Отмена операции"); 
                                break;

                            }


                        }





                        else checkPgn = false;

                    };

                    break;

            }
            PboMain.Image = myBitmap;

        }


        private void отчиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PboMain.Image = myBitmap;
            g.Clear(Color.White);
            PrimitiveList = new List<Primitive>();
            OperationType = 0;


        }

        private void настройкаПрямойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsLine formLine = new OptionsLine();
           DialogResult r = formLine.ShowDialog();
            int resultW = 0;
            if (r == DialogResult.OK) 
            {
                if ( int.TryParse(formLine.ReturnWStr,out resultW) ) 
                {
                    if (resultW >= 1 && resultW <= 10)
                    {
                        WLine = resultW;
                        switch (formLine.ReturnColorStr)
                        {
                            case "Черный":
                                ColorLine = Color.Black;
                                break;
                            case "Синий":
                                ColorLine = Color.Blue;
                                break;
                            case "Зеленый":
                                ColorLine = Color.Green;
                                break;
                            default:
                                ColorLine = Color.Black;
                                MessageBox.Show("Неверный цвет. Установлен черный.");
                                break;
                        }

                    }
                    else { MessageBox.Show("Неверный ввод. Толщина линнии от 1 до 10"); }
                }
                else
                {
                    MessageBox.Show("Неверный ввод");
                }
            }
            else
            {
                MessageBox.Show("Отмена операции");
            }

        }

        private void настройкаКривойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsCurve formCurve = new OptionsCurve();
            DialogResult r = formCurve.ShowDialog();
            int resultW = 0;
            if (r == DialogResult.OK)
            {
                if (int.TryParse(formCurve.ReturnWStr, out resultW))
                {
                    if (resultW >= 1 && resultW <= 10)
                    {
                        WCurve = resultW;
                        switch (formCurve.ReturnColorStr)
                        {
                            case "Черный":
                                ColorCurve = Color.Black;
                                break;
                            case "Синий":
                                ColorCurve = Color.Blue;
                                break;
                            case "Зеленый":
                                ColorCurve = Color.Green;
                                break;
                            default:
                                ColorLine = Color.Black;
                                MessageBox.Show("Неверный цвет. Установлен черный.");
                                break;
                        }

                    }
                    else { MessageBox.Show("Неверный ввод. Толщина линнии от 1 до 10"); }
                }
                else
                {
                    MessageBox.Show("Неверный ввод");
                }
            }
            else
            {
                MessageBox.Show("Отмена операции");
            }
        }


        private void настройкаЗвездыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionZV formZv = new OptionZV();
            DialogResult r = formZv.ShowDialog();
            int resultN = 5;
            int resultR = 60;
            if (r == DialogResult.OK)
            {
                if (int.TryParse(formZv.ReturnN, out resultN) && int.TryParse(formZv.ReturnR, out resultR))
                {
                    if ((resultN >= 3 && resultN <= 20) && (resultR >= 10 && resultR <= 100) )
                    {
                        nForStar = resultN;
                        rForstar = resultR;
                        switch (formZv.ReturnColorStr)
                        {
                            case "Черный":
                                ColorStar = Color.Black;
                                break;
                            case "Синий":
                                ColorStar = Color.Blue;
                                break;
                            case "Зеленый":
                                ColorStar = Color.Green;
                                break;
                            default:
                                ColorStar = Color.Black;
                                MessageBox.Show("Неверный цвет. Установлен черный.");
                                break;
                        }

                    }
                    else { MessageBox.Show("Неверный ввод. n от 3 до 20, радиус от 10 до 100"); }
                }
                else
                {
                    MessageBox.Show("Неверный ввод");
                }
            }
            else
            {
                MessageBox.Show("Отмена операции");
            }
        }

        private void настройкаУголкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsUgl3 formUgl3 = new OptionsUgl3();
            DialogResult r = formUgl3.ShowDialog();
            int resultW = 0;
            if (r == DialogResult.OK)
            {
                if (int.TryParse(formUgl3.ReturnWStr, out resultW))
                {
                    if (resultW >= 10 && resultW <= 100)
                    {
                        WforUgl = resultW;
                        switch (formUgl3.ReturnColorStr)
                        {
                            case "Черный":
                                ColorUgl = Color.Black;
                                break;
                            case "Синий":
                                ColorUgl = Color.Blue;
                                break;
                            case "Зеленый":
                                ColorUgl = Color.Green;
                                break;
                            default:
                                ColorUgl = Color.Black;
                                MessageBox.Show("Неверный цвет. Установлен черный.");
                                break;
                        }

                    }
                    else { MessageBox.Show("Неверный ввод. Pазмер от 10 до 100"); }
                }
                else
                {
                    MessageBox.Show("Неверный ввод");
                }
            }
            else
            {
                MessageBox.Show("Отмена операции");
            }

        }



        private void PboMain_MouseMove(object sender, MouseEventArgs e)
        {
            PboMain.Image = myBitmap;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if ( OperationType == 5 & checkPgn)
                {
                    PrimitiveList[selector].Move(e.X - PboMainMousePos.X, e.Y - PboMainMousePos.Y);
                    g.Clear(PboMain.BackColor);

                    PrimitiveList[selector].Fill(g, DrawPen);
                    PboMain.Image = myBitmap;
                    foreach (var p in PrimitiveList)
                    {
                        p.Fill(g, DrawPen);
                    }

                    PboMainMousePos = e.Location;

                }
            }

        }

        private void переместитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationType = SelectAndMove;
        }

        private void отразитьОтноситьльноВертикальнойПрямойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationType = SelectAndReflectVertical;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void отразитьЗеркальноОтносительноЗаданногоЦентраToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationType = SelectAndReflectCentral;
        }

        private void повернутьНаГрадусовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationType = SelectAndRotate;

        }
    }



}
