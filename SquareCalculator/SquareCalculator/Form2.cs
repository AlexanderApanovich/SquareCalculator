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
        decimal currentFactor = 1M;
        RectanglesArray blankRectangles;
        SquaresArray squares;
        int W, L;
        Pen penSquare = new Pen(Color.Black, 2);
        Pen penRectangle = new Pen(Color.Gray, 4);

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
            float factor = (float)currentFactor; //Множитель масштаба

            pictureBox2.Image = new Bitmap(Convert.ToInt32(L * currentFactor) + 10, Convert.ToInt32(W * currentFactor) + 10);

            Graphics g = Graphics.FromImage(pictureBox2.Image);
            g.Clear(Color.White);

            //Прямоугольник (лист)
            g.DrawRectangle(penRectangle, 0, 0, L * factor, W * factor);

            //Квадраты
            for (int i = 0; i < squares.length; i++)
                if (squares[i].plate == currentPlate)
                {
                    g.DrawRectangle(penSquare, squares[i].x * factor,
                        squares[i].y * factor, squares[i].size * factor, squares[i].size * factor);

                    //Количество цифр
                    number = Convert.ToString(squares[i].size).Length;

                    //Отображение внутри квадратов их размера
                    g.DrawString(Convert.ToString(squares[i].size), new Font("Arial", squares[i].size * factor / (number)),
                        Brushes.Black,
                        squares[i].x * factor + squares[i].size * factor / (number * 10),
                        squares[i].y * factor + squares[i].size * factor / 10);
                }
        }

        void SetUsedSize()
        {
            int maxW = 0, maxL = 0;
            for (int j = 0; j < squares.length; j++)
                if (squares[j].plate == currentPlate)
                {
                    if (squares[j].y + squares[j].size > maxW) maxW = squares[j].y + squares[j].size;
                    if (squares[j].x + squares[j].size > maxL) maxL = squares[j].x + squares[j].size;
                }
            label2.Text = "Размеры задействованного листа: длина = " + maxL + ", ширина = " + maxW;
        }

        private void button1_Click(object sender, EventArgs e) //Предыдущий лист
        {
            if (currentPlate > 1) currentPlate--;
            label3.Text = "Лист " + currentPlate + " из " + blankRectangles.numberOfPlates;
            Visualize(penSquare, penRectangle);

            SetUsedSize(); //Установка в Label размеров задействованного листа
        }

        private void button2_Click(object sender, EventArgs e) //Следующий лист
        {
            if (currentPlate < blankRectangles.numberOfPlates) currentPlate++;
            label3.Text = "Лист " + currentPlate + " из " + blankRectangles.numberOfPlates;
            Visualize(penSquare, penRectangle);

            SetUsedSize(); //Установка в Label размеров задействованного листа
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            //Добавление полос прокрутки для изображения
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            panel1.AutoScroll = true;
            panel1.Controls.Add(pictureBox2);

            textBox1.Text = Convert.ToString(currentFactor);

            label3.Text = "Лист " + currentPlate + " из " + blankRectangles.numberOfPlates;

            saveFileDialog1.Filter = "Изображения (*.bmp)|*.bmp";
            saveFileDialog1.DefaultExt = "bmp";

            //Установка в Label размеров задействованного листа
            SetUsedSize(); 
            //Визуализация
            Visualize(penSquare, penRectangle);
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            panel1.Size = new Size(Width - 50, Height - 150);
        }

        private void button4_Click(object sender, EventArgs e) //Сохранить все...
        {
            MessageBox.Show("При недостаточной разборчивости изображения увеличьте масштаб", "Предупреждение");
            if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel) return;

            string pathName = folderBrowserDialog1.SelectedPath; //Путь к папке
            currentFactor = 5; //Увеличиваем масштаб для увеличения разрешения изображения
            int tempPlate = currentPlate;

            for (int i = 1; i <= blankRectangles.numberOfPlates; i++) //Сохраняем все листы
            {
                currentPlate = i;
                Visualize(penSquare, penRectangle);
                pictureBox2.Image.Save(pathName + "/Лист " + i + ".bmp");
            }

            //Устанавливаем старые значения
            currentFactor = Convert.ToDecimal(textBox1.Text);
            currentPlate = tempPlate;
            Visualize(penSquare, penRectangle);

            MessageBox.Show("Файлы сохранены", "Информация");
        }

        private void button5_Click(object sender, EventArgs e) //Применить (масштаб)
        {
            string temp = textBox1.Text;
            string text = "";
            
            //Проверка корректности введенных данных
            for (int i = 0; i < temp.Length; i++) if (temp[i] == '.') text += ",";
                else text += temp[i];
            textBox1.Text = text;
            try
            {
                if (Convert.ToDecimal(textBox1.Text) <= 0) textBox1.Text = "0,1";
                currentFactor = Convert.ToDecimal(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("Введите корректный множитель масштаба!", "Ошибка");
                return;
            }
            Visualize(penSquare, penRectangle);
        }

        private void button3_Click(object sender, EventArgs e) //Сохранить изображение
        {
            MessageBox.Show("При недостаточной разборчивости изображения увеличьте масштаб", "Предупреждение");

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            string fileName = saveFileDialog1.FileName; //Путь к файлу

            //Увеличиваем масштаб для увеличения разрешения изображения
            currentFactor = 5; 
            Visualize(penSquare, penRectangle);

            pictureBox2.Image.Save(fileName);
            //Устанавливаем старые значения
            currentFactor = Convert.ToDecimal(textBox1.Text);
            Visualize(penSquare, penRectangle);

            MessageBox.Show("Файл сохранен", "Информация");
        }
    }
}

//Изображение пустых прямоугольников
#region   
////blankRectangle
//for (int i = 0; i<blankRectangles.length; i++)
//    if (blankRectangles[i].plate == currentPlate)
//    {
//        g.DrawRectangle(new Pen(Color.Blue, 2), blankRectangles[i].x* factor,
//            blankRectangles[i].y* factor, blankRectangles[i].l* factor, blankRectangles[i].w* factor);
//    }
#endregion