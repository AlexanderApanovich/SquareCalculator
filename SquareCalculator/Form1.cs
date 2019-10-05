using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SquareCalculator
{
    public partial class Form1 : Form
    {
        private int L;
        private int W;
        //private int numberOfPlates { get; set; }
        RectanglesArray blankRectangles;
        SquaresArray squares;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //dataGridView1
            int w1 = 35; //№
            int w2 = 90; //Количество

            var column1 = new DataGridViewColumn();
            column1.HeaderText = "№"; //текст в шапке
            column1.Width = w1; //ширина колонки
            column1.ReadOnly = true; //значение в этой колонке нельзя править
            column1.Name = "number"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

            var column2 = new DataGridViewColumn();
            column2.HeaderText = "Cторона квадрата";
            column2.Name = "side";
            column2.Width = dataGridView1.Size.Width - (w1 + w2);
            column2.CellTemplate = new DataGridViewTextBoxCell();

            var column3 = new DataGridViewColumn();
            column3.HeaderText = "Количество";
            column3.Name = "count";
            column3.Width = w2;
            column3.CellTemplate = new DataGridViewTextBoxCell();

            dataGridView1.Columns.Add(column1);
            dataGridView1.Columns.Add(column2);
            dataGridView1.Columns.Add(column3);

            for (int i = 0; i < numericUpDown1.Value; i++)
                dataGridView1.Rows.Add(i + 1, 50 - i * 10, 1); //второй столбец убрать!

            dataGridView1.RowHeadersVisible = false; //первый столбец не виден
            dataGridView1.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки

            //dataGridView2
            int w4 = 50;

            var column5 = new DataGridViewColumn();
            column5.HeaderText = "№";
            column5.Width = w1;
            column5.ReadOnly = true;
            column5.Name = "number";
            column5.CellTemplate = new DataGridViewTextBoxCell();

            var column6 = new DataGridViewColumn();
            column6.HeaderText = "Cторона квадрата";
            column6.Name = "side";
            column6.ReadOnly = true;
            column6.Width = dataGridView2.Size.Width - (w1 + w4);
            column6.CellTemplate = new DataGridViewTextBoxCell();

            var column7 = new DataGridViewColumn();
            column7.HeaderText = "№ Листа";
            column7.Name = "plate";
            column7.ReadOnly = true;
            column7.Width = w4;
            column7.CellTemplate = new DataGridViewTextBoxCell();

            dataGridView2.Columns.Add(column5);
            dataGridView2.Columns.Add(column6);
            dataGridView2.Columns.Add(column7);

            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AllowUserToAddRows = false;
            button2.Enabled = false;
        }




        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            int nSquares = 0;

            //Ввод данных
            if (!Int32.TryParse((textBox1.Text), out L) || (!Int32.TryParse((textBox2.Text), out W)))
            {
                MessageBox.Show("Введите корректные размеры листа!", "Ошибка");
                return;
            }
            for (int i = 0; i < dataGridView1.RowCount; i++)
                if (Convert.ToInt32(dataGridView1["side", i].Value) > 0)
                    nSquares++;

            if (nSquares == 0)
            {
                MessageBox.Show("Введите хотя бы одну сторону квадрата!", "Ошибка");
                return;
            }

            squares = new SquaresArray(0);

            //Ввод квадратов
            for (int i = 0; i < dataGridView1.RowCount; i++)
                if (Convert.ToInt32(dataGridView1["side", i].Value) > 0)
                {
                    for (int j = 0; j < Convert.ToInt32(dataGridView1["count", i].Value); j++)
                        squares.Add(new Square(0, 0,
                            Convert.ToInt32(dataGridView1["side", i].Value)));
                }
            squares.DecreasingSort();



            //L = Convert.ToInt32(textBox1.Text);
            //W = Convert.ToInt32(textBox2.Text);

            if (squares[0].size > Math.Min(L, W))
            {
                MessageBox.Show("Квадраты не влезают на лист! Увеличьте размеры листа", "Ошибка");
                return;
            }

            blankRectangles = new RectanglesArray(0);

            int k;
            // blankRectangles.numberOfPlates = 0;

            BinaryWriter dataOut = new BinaryWriter(new FileStream("C:/Users/User/Desktop/OutData.txt", FileMode.Create));
            //for (int i = 0; i < squares.length; i++)
            //    dataOut.Write("Вывод: \t x = " + squares[i].x +
            //    ", \t y = " + squares[i].y + ", \t size = " +
            //    squares[i].size + Environment.NewLine);

            for (int i = 0; i < squares.length; i++)
            {
                k = 0;

                dataOut.Write("До сортировки, Итерация № " + i +
                   Environment.NewLine);
                for (int j = 0; j < blankRectangles.length; j++)
                    // if (blankRectangles[j].plate ==1)
                    dataOut.Write("\t x = " + blankRectangles[j].x +
                    ", \t y = " + blankRectangles[j].y +
                    ", \t W = " + blankRectangles[j].w +
                    ", \t L = " + blankRectangles[j].l +
                    ", \t plate = " + blankRectangles[j].plate +
                    Environment.NewLine);

                blankRectangles.Sort();

                dataOut.Write("После сортировки, Итерация № " + i +
                   Environment.NewLine);
                for (int j = 0; j < blankRectangles.length; j++)
                    // if (blankRectangles[j].plate ==1)
                    dataOut.Write("\t x = " + blankRectangles[j].x +
                    ", \t y = " + blankRectangles[j].y +
                    ", \t W = " + blankRectangles[j].w +
                    ", \t L = " + blankRectangles[j].l +
                    ", \t plate = " + blankRectangles[j].plate +
                    Environment.NewLine);

                if ((blankRectangles.length == 0) || (squares[i].size > Math.Min(blankRectangles[blankRectangles.length - 1].w,
                blankRectangles[blankRectangles.length - 1].l)))
                {
                    blankRectangles.numberOfPlates++;
                    blankRectangles.Add(new Rectangle(0, 0, W, L, blankRectangles.numberOfPlates));
                    blankRectangles.Sort();
                }



                while (!blankRectangles.DivideByTwo(k, squares, i)) k++;


                   
            }

            label4.Text = "Необходимое количество листов: " + blankRectangles.numberOfPlates;

            dataOut.Close();

            dataOut = new BinaryWriter(new FileStream("C:/Users/User/Desktop/OutData1.txt", FileMode.Create));

            for (int i = 0; i < squares.length; i++)
                dataOut.Write("Вывод: \t x = " + squares[i].x +
                 ", \t y = " + squares[i].y + ", \t size = " +
                 squares[i].size + ", \t plate = " + squares[i].plate + Environment.NewLine);


            for (int i = 0; i < squares.length; i++)
            {
                dataGridView2.Rows.Add();
            }



            for (int i = 0; i < squares.length; i++)
            {
                dataGridView2["number", i].Value = i + 1;
                dataGridView2["side", i].Value = squares[i].size;
                dataGridView2["plate", i].Value = squares[i].plate;
            }

            //контроль типов

            //сортировка второй таблицы

            //визуализация:
            //5) размер использованного листа (через максимальный x+side и y+side квадрата листа)

            button2.Enabled = true;





            dataOut.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > 0)
            {
                dataGridView1.RowCount = (int)numericUpDown1.Value;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1["number", i].Value = i + 1;
                    if (dataGridView1["count", i].Value == null) dataGridView1["count", i].Value = 1;
                }
            }
            button2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2(blankRectangles, squares, W, L);
            form.Show();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            button2.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }
    }
}
