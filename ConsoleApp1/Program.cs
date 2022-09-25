using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Checkers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Game();
        }
    }

    public class Checker
    {
        public string symbol;
        public int[] position;
        public string color;

        public Checker(string color, int[] position)
        {
            string checkerColor;

            if (color == "white")
            {
                checkerColor = "б";
                Color = "white";
            }
            else
            {
                checkerColor = "ч";
                Color = "black";
            }
            this.Symbol = checkerColor;
            this.Position = position;
            this.Title = "checker";
        }

        public string Symbol
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public int[] Position
        {
            get;
            set;
        }

        public string Color
        {
            get;
            set;
        }
    }

    public class Board
    {
        public string[][] grid;
        public List<Checker> checkers;

        public Board()
        {
            this.Checkers = new List<Checker>();
            this.CreateBoard();
            return;
        }
        public string[][] Grid
        {
            get;
            set;
        }
        public List<Checker> Checkers
        {
            get;
            set;
        }

        public void CreateBoard()
        {
            this.Grid = new string[][]
            {
          new string[] {" ", " ", " ", " ", " ", " ", " ", " "},
          new string[] {" ", " ", " ", " ", " ", " ", " ", " "},
          new string[] {" ", " ", " ", " ", " ", " ", " ", " "},
          new string[] {" ", " ", " ", " ", " ", " ", " ", " "},
          new string[] {" ", " ", " ", " ", " ", " ", " ", " "},
          new string[] {" ", " ", " ", " ", " ", " ", " ", " "},
          new string[] {" ", " ", " ", " ", " ", " ", " ", " "},
          new string[] {" ", " ", " ", " ", " ", " ", " ", " "},
            };
            return;
        }

        public void GenerateCheckers()
        {
            int[][] blackPositions = new int[][]
           {
          new int[] { 0, 1 }, new int[] { 0, 3 }, new int[] { 0,  5 }, new int[] { 0, 7 },
          new int[] { 1, 0 }, new int[] { 1, 2 }, new int[] { 1,  4 }, new int[] { 1,  6 },
          new int[] { 2, 1 }, new int [] { 2, 3 }, new int[] { 2,  5 }, new int[] { 2, 7}
           };

            int[][] whitePositions = new int[][]
            {
          new int[] { 5, 0 }, new int[] { 5, 2 }, new int[] { 5, 4 }, new int[] { 5,  6 },
          new int[] { 6, 1 }, new int[] { 6, 3 }, new int[] { 6, 5 }, new int[] { 6, 7 },
          new int[] { 7, 0 }, new int[] { 7, 2 }, new int [] { 7, 4 }, new int[] { 7,  6 }
            };

            for (int i = 0; i < 12; i++)

            {
                Checker white = new Checker("white", whitePositions[i]);
                Checker black = new Checker("black", blackPositions[i]);
                Checkers.Add(white);
                Checkers.Add(black);
            }
            return;
        }

        public void PlaceCheckers()
        {
            foreach (var checker in Checkers)
            {
                this.Grid[checker.Position[0]][checker.Position[1]] = checker.Symbol;
            }
            return;
        }

        public void DrawBoard()
        {
            CreateBoard();
            PlaceCheckers();
            Console.WriteLine("  0 1 2 3 4 5 6 7 ");
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine(i + " " + String.Join(" ", this.Grid[i]));
            }
            return;
        }

        public Checker SelectChecker(int row, int column)
        {
            return Checkers.Find(x => x.Position.SequenceEqual(new List<int> { row, column }));
        }

        public void RemoveChecker(Checker checker)
        {
            Checkers.Remove(checker);
            return;
        }

        public bool CheckForWin()
        {
            return Checkers.All(x => x.Color == "white") || !Checkers.Exists(x => x.Color == "white");
        }
    }

    public class Game
    {
        public int CheckerCounter(int row, int column, int newRow, int newColumn, Board board)
        {
            int k = 0;
            if (newRow > row && newColumn > column)
            {
                for (int i = 0; i < newRow - row; i++)
                {
                    if (board.SelectChecker(++row, ++column) != null)
                        k++;
                }
            }
            if (newRow > row && newColumn < column)
            {
                for (int i = 0; i < newRow - row; i++)
                {
                    if (board.SelectChecker(++row, --column) != null)
                        k++;
                }
            }
            if (newRow < row && newColumn > column)
            {
                for (int i = 0; i < row - newRow; i++)
                {
                    if (board.SelectChecker(--row, ++column) != null)
                        k++;
                }
            }
            if (newRow < row && newColumn < column)
            {
                for (int i = 0; i < row - newRow; i++)
                {
                    if (board.SelectChecker(--row, --column) != null)
                        k++;
                }
            }
            return k;
        }
        public Game()
        {
            Board board = new Board();
            board.GenerateCheckers();
            board.DrawBoard();

            Console.WriteLine();
            Console.WriteLine("Первыми ходят белые (б).");
            Console.WriteLine("б - белая шашка, ч - чёрная шашка, Б - белая дамка, Ч - чёрная дамка.");
            Console.WriteLine();
            Console.WriteLine("Введите одну из команд: move (переместить) или jump (съесть).");

            string choice = Console.ReadLine();

            while (choice != "move" && choice != "jump")
            {
                Console.WriteLine("Команда не распознана.");
                Console.WriteLine("Введите одну из команд: move (переместить) или jump (съесть).");
                choice = Console.ReadLine();
            }
            int turn = 1;
            do
            {
                switch (choice)
                {
                    case "move":

                        Console.WriteLine("Введите ряд шашки:");                                                            //Вводим координаты шашки, которую собираемся двигать
                        int row = int.Parse(Console.ReadLine());
                        Console.WriteLine("Введите столбец шашки:");
                        int column = int.Parse(Console.ReadLine());

                        while (board.SelectChecker(row, column) == null)                                                    //Проверяем, есть ли шашка на заданном месте
                        {
                            Console.WriteLine("На этих координатах нет шашки.");
                            Console.WriteLine("Введите ряд шашки:");
                            row = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите столбец шашки:");
                            column = int.Parse(Console.ReadLine());
                        }

                        Checker checker = board.SelectChecker(row, column);

                        if (turn % 2 == 1 && checker.Color == "black" || turn % 2 == 0 && checker.Color == "white")
                        {
                            Console.WriteLine(turn % 2 == 0 ? "Сейчас ход чёрных" : "Сейчас ход белых");
                            break;
                        }

                        string title = checker.Title;

                        switch (title)
                        {
                            case "checker":
                                Console.WriteLine("Move to which Row: ");
                                int newRow = int.Parse(Console.ReadLine());                                                     //Вводим координаты, на которые собираемся переместить шашку
                                Console.WriteLine("Move to which Column: ");
                                int newColumn = int.Parse(Console.ReadLine());

                                while ((newRow + newColumn) % 2 == 0 || board.SelectChecker(newRow, newColumn) != null || turn % 2 == 1 && newRow - row != -1 || turn % 2 == 0 && newRow - row != 1 || Math.Abs(newColumn - column) != 1)
                                {                                                                                               //Первым условием проверяем, на тот же ли цвет перемещается шашка
                                    Console.WriteLine("Такой ход невозможен.");                                                 //(у ячеек одного цвета сумма координат имеет одну чётность).
                                    Console.WriteLine("Enter a valid Row:");                                                    //Вторым условием проверяем, занято ли новое место.
                                    newRow = int.Parse(Console.ReadLine());                                                     //Третьим и четвёртым условием - допустимость хода.
                                    Console.WriteLine("Enter a valid Column:");
                                    newColumn = int.Parse(Console.ReadLine());
                                }

                                if (newRow == 8 && turn % 2 == 0 || newRow == 0 && turn % 2 == 1)
                                {
                                    checker.Title = "queen";
                                    checker.Symbol = turn % 2 == 0 ? "Ч" : "Б";
                                }
                                
                                checker.Position = new int[] { newRow, newColumn };                                             //В случае успеха отрисовываем доску с новым расположением шашки.
                                board.DrawBoard();
                                turn++;
                                break;

                            case "queen":
                                Console.WriteLine("Move to which Row: ");
                                newRow = int.Parse(Console.ReadLine());                                                         //Вводим координаты, на которые собираемся переместить шашку
                                Console.WriteLine("Move to which Column: ");
                                newColumn = int.Parse(Console.ReadLine());

                                while (Math.Abs(newRow - row) != Math.Abs(newColumn - column) || CheckerCounter(row, column, newRow, newColumn, board) != 0)
                                {
                                    Console.WriteLine("Такой ход невозможен.");
                                    Console.WriteLine("Введите ряд, на который следует переместить дамку:");
                                    newRow = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Введите столбец:");
                                    newColumn = int.Parse(Console.ReadLine());
                                }

                                checker.Position = new int[] { newRow, newColumn };                                             //В случае успеха отрисовываем доску с новым расположением шашки.
                                board.DrawBoard();
                                turn++;
                                break;
                            default:
                                break;
                        }
                        break;

                    case "jump":

                        Console.WriteLine("Выберите ряд шашки: ");
                        int removeRow = int.Parse(Console.ReadLine());
                        Console.WriteLine("Выберите столбец шашки: ");
                        int removeColumn = int.Parse(Console.ReadLine());
                        checker = board.SelectChecker(removeRow, removeColumn);

                        if (turn % 2 == 1 && checker.Color == "black" || turn % 2 == 0 && checker.Color == "white")
                        {
                            Console.WriteLine(turn % 2 == 0 ? "Сейчас ход чёрных" : "Сейчас ход белых");
                            break;
                        }

                        title = checker.Title;
                        
                        switch (title)
                        {
                            case "checker":
                                int k = 0;
                                string options = "";
                                do
                                {
                                    options = "1) ";
                                    int[][][] opts = new int[4][][];
                                    if (board.SelectChecker(removeRow + 1, removeColumn - 1) != null && board.SelectChecker(removeRow + 2, removeColumn - 2) == null &&
                                        board.SelectChecker(removeRow + 1, removeColumn - 1).Color != board.SelectChecker(removeRow, removeColumn).Color
                                        && removeRow + 2 >= 0 && removeColumn - 2 >= 0 && removeRow + 2 <= 8 && removeColumn - 2 <= 8)
                                    {
                                        options += "ряд ";
                                        options += (removeRow + 2).ToString();
                                        options += " столбец ";
                                        options += (removeColumn - 2).ToString();
                                        opts[0] = new int[][] { new int[] { removeRow + 2, removeColumn - 2 }, new int[] { removeRow + 1, removeColumn - 1 } };
                                    }
                                    options += '\n';
                                    options += "2) ";

                                    if (board.SelectChecker(removeRow + 1, removeColumn + 1) != null && board.SelectChecker(removeRow + 2, removeColumn + 2) == null &&
                                        board.SelectChecker(removeRow + 1, removeColumn + 1).Color != board.SelectChecker(removeRow, removeColumn).Color
                                        && removeRow + 2 >= 0 && removeColumn + 2 >= 0 && removeRow + 2 <= 8 && removeColumn + 2 <= 8)
                                    {
                                        options += "ряд ";
                                        options += (removeRow + 2).ToString();
                                        options += " столбец ";
                                        options += (removeColumn + 2).ToString();
                                        opts[1] = new int[][] { new int[] { removeRow + 2, removeColumn + 2 }, new int[] { removeRow + 1, removeColumn + 1 } };
                                    }
                                    options += '\n';
                                    options += "3) ";

                                    if (board.SelectChecker(removeRow - 1, removeColumn - 1) != null && board.SelectChecker(removeRow - 2, removeColumn - 2) == null &&
                                        board.SelectChecker(removeRow - 1, removeColumn - 1).Color != board.SelectChecker(removeRow, removeColumn).Color
                                        && removeRow - 2 >= 0 && removeColumn - 2 >= 0 && removeRow - 2 <= 8 && removeColumn - 2 <= 8)
                                    {
                                        options += "ряд ";
                                        options += (removeRow - 2).ToString();
                                        options += " столбец ";
                                        options += (removeColumn - 2).ToString();
                                        opts[2] = new int[][] { new int[] { removeRow - 2, removeColumn - 2 }, new int[] { removeRow - 1, removeColumn - 1 } };
                                    }
                                    options += '\n';
                                    options += "4) ";

                                    if (board.SelectChecker(removeRow - 1, removeColumn + 1) != null && board.SelectChecker(removeRow - 2, removeColumn + 2) == null &&
                                        board.SelectChecker(removeRow - 1, removeColumn + 1).Color != board.SelectChecker(removeRow, removeColumn).Color
                                        && removeRow - 2 >= 0 && removeColumn + 2 >= 0 && removeRow - 2 <= 8 && removeColumn + 2 <= 8)
                                    {
                                        options += "ряд ";
                                        options += (removeRow - 2).ToString();
                                        options += " столбец ";
                                        options += (removeColumn + 2).ToString();
                                        opts[3] = new int[][] { new int[] { removeRow - 2, removeColumn + 2 }, new int[] { removeRow - 1, removeColumn + 1 } };
                                    }

                                    if (options != "1) \n2) \n3) \n4) ")
                                    {
                                        k = 1;
                                        Console.WriteLine("Выберите из возможных вариантов куда следует переместить шашку:" + '\n' + options);
                                        Console.WriteLine("Выберите номер интересующего вас варианта:");
                                        int number = int.Parse(Console.ReadLine());
                                        checker.Position = opts[number - 1][0];
                                        removeRow = opts[number - 1][0][0];
                                        removeColumn = opts[number - 1][0][1];
                                        board.RemoveChecker(board.SelectChecker(opts[number - 1][1][0], opts[number - 1][1][1]));
                                        board.DrawBoard();
                                    }
                                    else if (k == 0)
                                        Console.WriteLine("Данная шашка не может съесть другую.");
                                    else
                                        turn++;
                                }
                                while (options != "1) \n2) \n3) \n4) ");
                                break;

                            case "queen":
                                Console.WriteLine("Move to which Row: ");
                                int newRow = int.Parse(Console.ReadLine());                                                         //Вводим координаты, на которые собираемся переместить шашку
                                Console.WriteLine("Move to which Column: ");
                                int newColumn = int.Parse(Console.ReadLine());

                                while (Math.Abs(newRow - removeRow) != Math.Abs(newColumn - removeColumn) || board.SelectChecker(newRow, newColumn) != null || CheckerCounter(removeRow, removeColumn, newRow, newColumn, board) != 1)
                                {
                                    Console.WriteLine("Такой ход невозможен.");
                                    Console.WriteLine("Введите ряд, на который следует переместить дамку:");
                                    newRow = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Введите столбец:");
                                    newColumn = int.Parse(Console.ReadLine());
                                }

                                Console.WriteLine("Введите координаты шашки, которая будет съедена при данном перемещении дамки:");
                                Console.WriteLine("Ряд");
                                int killRow = int.Parse(Console.ReadLine());
                                Console.WriteLine("Столбец");
                                int killColumn = int.Parse(Console.ReadLine());

                                while (board.SelectChecker(killRow, killColumn) == null)
                                {
                                    Console.WriteLine("Здесь нет шашки.");
                                    Console.WriteLine("Введите ряд, на котором есть шашка:");
                                    newRow = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Введите столбец:");
                                    newColumn = int.Parse(Console.ReadLine());
                                }

                                checker.Position = new int[] { newRow, newColumn };                                                 //В случае успеха отрисовываем доску с новым расположением шашки.
                                checker = board.SelectChecker(killRow, killColumn);
                                board.RemoveChecker(checker);
                                board.DrawBoard();
                                turn++;
                                break;
                            default:
                                break;
                        }
                        break;

                    default:

                        Console.WriteLine("Invalid input.");
                        break;
                }
                Console.WriteLine("Введите одну из команд: move (переместить) или jump (съесть)");
                choice = Console.ReadLine();

                while (choice != "move" && choice != "jump")
                {
                    Console.WriteLine("Команда не распознана");
                    Console.WriteLine("Введите одну из команд: move (переместить) или jump (съесть)");
                    choice = Console.ReadLine();
                }
            }
            while (board.CheckForWin() != true);
        }
    }
}