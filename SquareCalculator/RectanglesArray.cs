using System;
using System.IO;

namespace SquareCalculator
{
    public struct Rectangle
    {
        public int x, y, w, l, plate;
        public Rectangle(int x, int y, int w, int l, int plate) { this.x = x; this.y = y; this.w = w; this.l = l; this.plate = plate; }
    }
    public class RectanglesArray
    {
        Rectangle[] Rectangles;
        public int length { get; set; }
        public int numberOfPlates { get; set; } = 0;

        public RectanglesArray(int Length)
        {
            Rectangles = new Rectangle[Length];
            length = Length;
        }

        public Rectangle this[int index]
        {
            get
            {
                if (index >= length) throw new Exception("Выход за пределы массива!");
                return Rectangles[index];
            }
            set
            {
                if (index >= length) throw new Exception("Выход за пределы массива!");
                Rectangles[index] = value;
            }
        }

        public void Add(Rectangle value)
        {
            //Вспомогательный массив
            Rectangle[] temp = new Rectangle[length];
            for (int i = 0; i < length; i++)
            {
                temp[i].x = Rectangles[i].x;
                temp[i].y = Rectangles[i].y;
                temp[i].w = Rectangles[i].w;
                temp[i].l = Rectangles[i].l;
                temp[i].plate = Rectangles[i].plate;
            }
            length++;
            Rectangles = new Rectangle[length];
            for (int i = 0; i < length - 1; i++)
            {
                Rectangles[i].x = temp[i].x;
                Rectangles[i].y = temp[i].y;
                Rectangles[i].w = temp[i].w;
                Rectangles[i].l = temp[i].l;
                Rectangles[i].plate = temp[i].plate;
            }
            Rectangles[length - 1] = value;
        }

        public void Swap(ref Rectangle a, ref Rectangle b)
        {
            Rectangle temp;

            temp.x = a.x;
            temp.y = a.y;
            temp.w = a.w;
            temp.l = a.l;
            temp.plate = a.plate;

            a.x = b.x;
            a.y = b.y;
            a.w = b.w;
            a.l = b.l;
            a.plate = b.plate;

            b.x = temp.x;
            b.y = temp.y;
            b.w = temp.w;
            b.l = temp.l;
            b.plate = temp.plate;
        }

        public void Sort()              
        {
            //Сортировка сначала по возрастанию номера листа
            if (length <= 1) return;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length - 1; j++)
                {
                    if (Rectangles[j].plate > Rectangles[j + 1].plate)
                        Swap(ref Rectangles[j + 1], ref Rectangles[j]);
                }
            }
            //Затем по возрастанию меньшей стороны
            int iL = 0;
            int iR = 0;

            while (iR < length)
            {
                while (true)
                {
                    if ((iR < length - 1) && (Rectangles[iR].plate == Rectangles[iR + 1].plate))
                        iR++;
                    else break;
                }
                for (int i = iL; i <= iR; i++)
                {
                    for (int j = iL; j < iR; j++)
                    {
                        if (Math.Min(Rectangles[j].w, Rectangles[j].l) >
                        Math.Min(Rectangles[j + 1].w, Rectangles[j + 1].l))
                            Swap(ref Rectangles[j + 1], ref Rectangles[j]);
                    }
                }
                iR++;
                iL = iR;
            }
        }

        public void RemoveOne(int index)
        {
            if (index >= length) throw new Exception("Выход за пределы массива!");
            if (length <= 0) return;
            if (length == 1) { length--; return; }
            for (int i = index; i < length - 1; i++)
            {
                Rectangles[i].x = Rectangles[i + 1].x;
                Rectangles[i].y = Rectangles[i + 1].y;
                Rectangles[i].w = Rectangles[i + 1].w;
                Rectangles[i].l = Rectangles[i + 1].l;
                Rectangles[i].plate = Rectangles[i + 1].plate;
            }
            length--;
        }

        private bool Combine (int rectangleIndex, int side)
        {
            bool combined = false;

            //Объединение прямоугольников
            if (side > Rectangles[rectangleIndex].w && side <= Rectangles[rectangleIndex].l)
            {
                for (int i = 0; i < length; i++)
                    if (i != rectangleIndex)
                    {
                        if (Rectangles[rectangleIndex].plate == Rectangles[i].plate
                            && (Rectangles[rectangleIndex].w + Rectangles[i].w >= side)
                            && Rectangles[rectangleIndex].l == Rectangles[i].l
                            && Rectangles[rectangleIndex].x == Rectangles[i].x
                            && (Rectangles[rectangleIndex].y > Rectangles[i].y && Rectangles[rectangleIndex].y - Rectangles[i].y == Rectangles[i].w
                            || Rectangles[rectangleIndex].y < Rectangles[i].y && Rectangles[i].y - Rectangles[rectangleIndex].y == Rectangles[rectangleIndex].w))
                        {
                            Rectangles[rectangleIndex].w += Rectangles[i].w;

                            if (Rectangles[rectangleIndex].y > Rectangles[i].y)
                                Rectangles[rectangleIndex].x -= Rectangles[i].w;

                            RemoveOne(i);
                            combined = true;
                            if (rectangleIndex > i) rectangleIndex--;
                            break;
                        }
                    }
                if (!combined) return false;
            }

            if (side > Rectangles[rectangleIndex].l && side <= Rectangles[rectangleIndex].w)
            {
                for (int i = 0; i < length; i++)
                    if (i != rectangleIndex)
                    {
                        if (Rectangles[rectangleIndex].plate == Rectangles[i].plate
                            && (Rectangles[rectangleIndex].l + Rectangles[i].l >= side)
                            && Rectangles[rectangleIndex].w == Rectangles[i].w
                            && Rectangles[rectangleIndex].y == Rectangles[i].y
                            && (Rectangles[rectangleIndex].x > Rectangles[i].x && Rectangles[rectangleIndex].x - Rectangles[i].x == Rectangles[i].x
                            || Rectangles[rectangleIndex].x < Rectangles[i].x && Rectangles[i].x - Rectangles[rectangleIndex].x == Rectangles[rectangleIndex].l))
                        {
                            Rectangles[rectangleIndex].l += Rectangles[i].l;

                            if (Rectangles[rectangleIndex].x > Rectangles[i].x)
                                Rectangles[rectangleIndex].x -= Rectangles[i].l;

                            RemoveOne(i);
                            combined = true;
                            if (rectangleIndex > i) rectangleIndex--;
                            break;
                        }
                    }
                if (!combined) return false;
            }
            return true;
        }

        public bool DivideByTwo(int rectangleIndex, SquaresArray squares, int squareIndex)
        {
            if (rectangleIndex >= length) throw new Exception("Выход за пределы массива!");

            int side = squares[squareIndex].size;
            
            //Проверка возможности соединить соседние прямоугольники
            if (side > Rectangles[rectangleIndex].w && side > Rectangles[rectangleIndex].l) return false;
            if (!Combine(rectangleIndex, side)) return false;           

            //Сохранение номера пластины и координат
            squares.SetPlate(squareIndex, Rectangles[rectangleIndex].plate);
            squares.SetCoordinates(squareIndex, Rectangles[rectangleIndex].x, Rectangles[rectangleIndex].y);
            
            if ((side == Rectangles[rectangleIndex].w) && (side == Rectangles[rectangleIndex].l))
            {
                RemoveOne(rectangleIndex);
                return true;
            }

            int x = Rectangles[rectangleIndex].x;
            int y = Rectangles[rectangleIndex].y;
            int w = Rectangles[rectangleIndex].w;
            int l = Rectangles[rectangleIndex].l;

            if (w == side)
            {
                Rectangles[rectangleIndex].x += side;
                Rectangles[rectangleIndex].l -= side;
                return true;
            }

            if (l == side)
            {
                Rectangles[rectangleIndex].y += side;
                Rectangles[rectangleIndex].w -= side;
                return true;
            }

            if (l >= w)
            {
                Rectangles[rectangleIndex].x += side;
                Rectangles[rectangleIndex].l -= side;
                Add(new Rectangle(x, y + side, w - side, side, Rectangles[rectangleIndex].plate));
            }

            if (l < w)
            {
                Rectangles[rectangleIndex].y += side;
                Rectangles[rectangleIndex].w -= side;
                Add(new Rectangle(x + side, y, side, l - side, Rectangles[rectangleIndex].plate));
            }
            return true;
        }
    }
}

//Добавление пространства за счет соседнего прямоугольника
#region
//if (side == Rectangles[rectangleIndex].l && side > Rectangles[rectangleIndex].w)
//    for (int i = 0; i < length; i++)
//        if (i != rectangleIndex)
//        {
//            if (Rectangles[rectangleIndex].plate == Rectangles[i].plate
//                && (Rectangles[rectangleIndex].w + Rectangles[i].w >= side)
//                && Rectangles[rectangleIndex].l == side
//                && Rectangles[rectangleIndex].x >= Rectangles[i].x
//                && Rectangles[rectangleIndex].x <= Rectangles[i].x + Rectangles[i].l
//                && (Rectangles[rectangleIndex].y == Rectangles[i].y - Rectangles[rectangleIndex].w
//                || Rectangles[rectangleIndex].y == Rectangles[i].y + Rectangles[i].w))
//            {
//                if (Rectangles[i].y < Rectangles[rectangleIndex].y)
//                {
//                    if (Rectangles[rectangleIndex].y - (side - Rectangles[rectangleIndex].w) == Rectangles[i].y)
//                    {
//                        Add(new Rectangle(Rectangles[rectangleIndex].x + side, 
//                            Rectangles[i].y,
//                            Rectangles[i].w,
//                            Rectangles[i].l - (Rectangles[rectangleIndex].x + side - Rectangles[i].x),
//                            Rectangles[i].plate));
//                    }
//                }

//                if (Rectangles[i].y > Rectangles[rectangleIndex].y)

//                //if (Rectangles[i].x <= Rectangles[rectangleIndex].x)

//                //if (Rectangles[i].x == Rectangles[rectangleIndex].x)
//                {
//                    if (Rectangles[rectangleIndex].y - (side - Rectangles[rectangleIndex].w) == Rectangles[i].y)
//                    {
//                        Add(new Rectangle(Rectangles[rectangleIndex].x + side,
//                            Rectangles[i].y,
//                            Rectangles[i].w,
//                            Rectangles[i].l - (Rectangles[rectangleIndex].x + side - Rectangles[i].x),
//                            Rectangles[i].plate));
//                    }
//                    Add(new Rectangle());
//                }


//                Rectangles[i].l = Rectangles[rectangleIndex].x - Rectangles[i].x;
//                RemoveOne(i);                                
//            }
//        }
#endregion
//Деление на 3 прямоугольника
#region
//if (l >= w)
//{
//    Add(new Rectangle(x, y + side, w - side, side, Rectangles[rectangleIndex].plate));
//    Add(new Rectangle(x + side, y + side, w - side, l - side, Rectangles[rectangleIndex].plate));

//    Rectangles[rectangleIndex].x += side;
//    Rectangles[rectangleIndex].l -= side;
//    Rectangles[rectangleIndex].w = side;
//}

//if (l < w)
//{
//    Add(new Rectangle(x + side, y, side, l - side, Rectangles[rectangleIndex].plate));
//    Add(new Rectangle(x + side, y + side, w - side, l - side, Rectangles[rectangleIndex].plate));

//    Rectangles[rectangleIndex].y += side;
//    Rectangles[rectangleIndex].w -= side;
//    Rectangles[rectangleIndex].l = side;
//}
#endregion
//Сортировка только по меньшей стороне
#region
//if (length <= 1) return;
//for (int i = 0; i < length; i++)
//{
//    for (int j = 0; j < length - 1; j++)
//    {
//        if (Math.Min(Rectangles[j].w, Rectangles[j].l) >
//            Math.Min(Rectangles[j + 1].w, Rectangles[j + 1].l))
//            Swap(ref Rectangles[j + 1], ref Rectangles[j]);
//    }
//}
#endregion