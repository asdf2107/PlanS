using System;
using System.Collections.Generic;
using System.Text;

namespace PlanS
{
    public class Plan : Frame
    {
        public static TimeSpan RedZone = new TimeSpan(36, 0, 0);
        protected string name;
        protected bool showLabels = true;
        public string Name
        {
            get { return name; }
            protected set { name = value.Length > Width - 2 ? value.ToUpper().Substring(0, Width) : value.ToUpper(); }
        }
        public DateTime DueTime { get; protected set; }


        public Plan(int i, DateTime dueDate, string name) : base(i)
        {
            DueTime = dueDate;
            Name = name;
            SetColor();
            DrawFrame();
            PaintInside();
            DrawLabels();
        }

        public Plan(int i) : base(i)
        {
            SetColor();
            DrawFrame();
            PaintInside();
            SetName();
            DrawName();
            SetDate();
            SetColor();
            Draw();
        }


        public string GetInfoString()
        {
            return DueTime.ToString() + "|" + Name;
        }


        public override void Draw()
        {
            DrawFrame();
            PaintInside();
            DrawLabels();
        }


        protected void SetName()
        {
            Name = GetText(1, Width, 0);
        }


        protected string GetText(int x1, int x2, int y)
        {
            ConsoleColor initColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            string res = "";
            Console.SetCursorPosition(StartPoint.X + 1 + x1, StartPoint.Y + 1 + y);
            Console.CursorVisible = true;
            bool stop = false;
            while (true)
            {
                ConsoleKey k = Console.ReadKey().Key;
                switch (k)
                {
                    case ConsoleKey.Enter:
                        stop = true;
                        break;
                    case ConsoleKey.Escape:
                        goto case ConsoleKey.Enter;
                    case ConsoleKey.Backspace:
                        if (Console.CursorLeft > StartPoint.X)
                        {
                            RemoveLast();
                        }
                        break;
                    default:
                        if (Console.CursorLeft > StartPoint.X + x2)
                        {
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            RemoveLast();
                        }
                        if (k == ConsoleKey.Spacebar)
                            res += " ";
                        else if (k == ConsoleKey.OemPeriod || k == ConsoleKey.Oem2)
                            res += ".";
                        else if (k == ConsoleKey.Oem1)
                            res += ":";
                        else if (k.ToString().Contains("NumPad"))
                            res += k.ToString().Replace("NumPad", "");
                        else if (k.ToString().Contains('D') && k.ToString().Length == 2)
                            res += k.ToString().Substring(1);
                        else
                            res += k.ToString();
                        break;
                }
                if (stop)
                    break;
            }
            Console.CursorVisible = false;
            Console.ForegroundColor = initColor;
            return res;
            void RemoveLast()
            {
                Console.Write(' ');
                if (Console.CursorLeft > StartPoint.X + 1 + x1)
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                if (res.Length > 0)
                    res = res.Substring(0, res.Length - 1);
            }
        }


        protected void SetDate()
        {
            Console.SetCursorPosition(StartPoint.X + 1, StartPoint.Y + 3);
            Console.Write("Due to: DD.MM.YYYY HH:MM");
            string res = GetText(8, Width, 2).Replace('.', '/');
            try
            {
                DueTime = DateTime.Parse(res);
            }
            catch
            {
                SetDate();
            }
        }


        protected void DrawLabels()
        {
            DrawName();
            if (showLabels)
            {
                DrawDueTime();
                DrawTimeLeft();
            }
        }


        protected void DrawName()
        {
            Console.SetCursorPosition(StartPoint.X + 2, StartPoint.Y + 1);
            Console.Write(Name);
        }


        protected void DrawDueTime()
        {
            Console.SetCursorPosition(StartPoint.X + 1, StartPoint.Y + 3);
            Console.Write(Add0(DueTime.Date.Day) + "." + Add0(DueTime.Date.Month) + " " + DueTime.ToShortTimeString() + " (" + DueTime.DayOfWeek.ToString() + ")");
        }

        public static string Add0(int inp)
        {
            if (inp >= 10)
                return inp.ToString();
            return "0" + inp.ToString();
        }


        protected void DrawTimeLeft()
        {
            Console.SetCursorPosition(StartPoint.X + 1, StartPoint.Y + 4);
            TimeSpan left = DueTime - DateTime.Now;
            Console.Write("Left:  " + left.Days + "d " + left.Hours + "h " + left.Minutes + "m");
        }


        protected override void SetColor()
        {
            if (DueTime - DateTime.Now <= -2 * RedZone)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                showLabels = false;
            }
            else if (DueTime - DateTime.Now <= RedZone)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                showLabels = true;
            }
            else if (DueTime - DateTime.Now <= 3 * RedZone)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                showLabels = true;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                showLabels = true;
            }
        }
    }
}
