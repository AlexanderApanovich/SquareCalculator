using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SquareCalculator;

namespace SquareCalculator
{
    public partial class Form2 : Form
    {

        int currentPlate = 1;
        decimal currentFactor = 0.3M;
        RectanglesArray blankRectangles;
        SquaresArray squares;
        int W, L;
        Pen penSquare = new Pen(Color.Black, 2);
        Pen penRectangle = new Pen(Color.Gray, 4);
        // float kW, kL;

        public Form2(RectanglesArray rectangles, SquaresArray squares, int W, int L)
        {
            blankRectangles = rectangles;
            this.squares = squares;
            this.W = W;
            this.L = L;
            InitializeComponent();
        }

        public void Visualize(Pen penSquare, Pen penRectangle)
        {
            float number;
            float factor;
            //if ((W > panel1.Size.Height) || (L > panel1.Size.Width))
            factor = (float)currentFactor;
            // else factor = 1;

            pictureBox2.Image = new Bitmap(Convert.ToInt32(L * currentFactor) + 10, Convert.ToInt32(W * currentFactor) + 10);

            Graphics g = Graphics.FromImage(pictureBox2.Image);
            g.Clear(Color.White);

            //Прямоугольник
            g.DrawRectangle(penRectangle, 0, 0, L * factor, W * factor);

            //Квадраты
            for (int i = 0; i < squares.length; i++)
                if (squares[i].plate == currentPlate)
                {
                    g.DrawRectangle(penSquare, squares[i].x * factor,
                        squares[i].y * factor, squares[i].size * factor, squares[i].size * factor);

                    number = Convert.ToString(squares[i].size).Length; //* factor;

                    g.DrawString(Convert.ToString(squares[i].size), new Font("Arial", squares[i].size * factor / (number)),
                        Brushes.Black,
                        squares[i].x * factor + squares[i].size * factor / (number * 10),
                        squares[i].y * factor + squares[i].size * factor / 10);
                }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (currentPlate > 1) currentPlate--;
            label3.Text = "Лист " + currentPlate + " из " + blankRectangles.numberOfPlates;
            Visualize(penSquare, penRectangle);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentPlate < blankRectangles.numberOfPlates) currentPlate++;
            label3.Text = "Лист " + currentPlate + " из " + blankRectangles.numberOfPlates;
            Visualize(penSquare, penRectangle);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value <= 0) numericUpDown1.Value = numericUpDown1.Increment; //0.05M;
            currentFactor = numericUpDown1.Value;
            Visualize(penSquare, penRectangle);
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            panel1.AutoScroll = true;
            panel1.Controls.Add(pictureBox2);

            numericUpDown1.Value = currentFactor;
            numericUpDown1.Increment = 0.05M;

            label3.Text = "Лист " + currentPlate + " из " + blankRectangles.numberOfPlates;

            saveFileDialog1.Filter = "Изображения (*.bmp)|*.bmp";
            saveFileDialog1.DefaultExt = "bmp";



            //    label1.Text = "Необходимое количество листов: " + numberOfPlates;






            Visualize(penSquare, penRectangle);


        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            panel1.Size = new Size(Width - 50, Height - 150);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("При недостаточной разборчивости изображения увеличьте масштаб", "Предупреждение");
            if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel) return;
            string pathName = folderBrowserDialog1.SelectedPath;

            currentFactor = 5;
            int tempPlate = currentPlate;

            for (int i = 1; i <= blankRectangles.numberOfPlates; i++)
            {
                currentPlate = i;
                Visualize(penSquare, penRectangle);
                pictureBox2.Image.Save(pathName + "/Лист " + i + ".bmp");
            }

            currentFactor = numericUpDown1.Value;
            currentPlate = tempPlate;
            Visualize(penSquare, penRectangle);

            MessageBox.Show("Файлы сохранены", "Информация");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("При недостаточной разборчивости изображения увеличьте масштаб", "Предупреждение");

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            string fileName = saveFileDialog1.FileName;

            currentFactor = 5;
            Visualize(penSquare, penRectangle);

            pictureBox2.Image.Save(fileName);

            currentFactor = numericUpDown1.Value;
            Visualize(penSquare, penRectangle);

            MessageBox.Show("Файл сохранен", "Информация");
        }
    }
}
