using System;

namespace SquareCalculator
{
    public struct Square
    {
        public int x, y, size,plate;
        public Square(int x, int y, int size) { this.x = x; this.y = y; this.size = size; plate = 0; }
    }

    public class SquaresArray
    {
        Square[] Squares;
        public int length { get; set; }

        public SquaresArray(int Length)
        {
            Squares = new Square[Length];
            length = Length;
        }

        public Square this[int index]
        {
            get
            {
                if (index >= length) throw new Exception("Выход за пределы массива!");
                return Squares[index];
            }
            set
            {
                if (index >= length) throw new Exception("Выход за пределы массива!");
                Squares[index] = value;
            }
        }

        public void Add(Square value)
        {
            //Вспомогательный массив
            Square[] temp = new Square[length];
            for (int i = 0; i < length; i++)
            {
                temp[i].x = Squares[i].x;
                temp[i].y = Squares[i].y;
                temp[i].size = Squares[i].size;
                temp[i].plate = Squares[i].plate;
            }
            length++;
            Squares = new Square[length];
            for (int i = 0; i < length - 1; i++)
            {
                Squares[i].x = temp[i].x;
                Squares[i].y = temp[i].y;
                Squares[i].size = temp[i].size;
                Squares[i].plate = temp[i].plate;
            }
            Squares[length - 1] = value;
        }

        public void Swap(ref Square a, ref Square b)
        {
            Square temp;

            temp.x = a.x;
            temp.y = a.y;
            temp.size = a.size;
            temp.plate = a.plate;

            a.x = b.x;
            a.y = b.y;
            a.size = b.size;
            a.plate = b.plate;

            b.x = temp.x;
            b.y = temp.y;
            b.size = temp.size;
            b.plate = temp.plate;
        }

        public void DecreasingSort()
        {
            //Сортировка по убыванию
            if (length <= 1) return;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length - 1; j++)
                {
                    if (Squares[j].size < Squares[j + 1].size)
                        Swap(ref Squares[j + 1], ref Squares[j]);
                }
            }
        }

        public void SetPlate(int index, int value)
        {
            Squares[index].plate = value;
        }

        public void SetCoordinates(int index, int x, int y)
        {
            Squares[index].x = x;
            Squares[index].y = y;
        }

        public void RemoveOne(int index)
        {
            if (index >= length) throw new Exception("Выход за пределы массива!");
            if (length <= 0) return;
            if (length == 1) { length--; return; }
            for (int i = index; i < length - 1; i++)
            {
                Squares[i].x = Squares[i + 1].x;
                Squares[i].y = Squares[i + 1].y;
                Squares[i].size = Squares[i + 1].size;
            }
            length--;
        }
    }
}
