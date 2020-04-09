using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PlanS
{
    public class Frame
    {
        public static int Width = 25, Height = 4;
        protected static readonly char[] box = new char[] { '─', '│', '┌', '┐', '┘', '└', '┼' };
        protected static readonly char[] boxc = new char[] { '═', '║', '╔', '╗', '╝', '╚', '╬' };
        public Point StartPoint { get; set; }
        protected bool Chosen { get; set; } = false;


        public Frame(int i)
        {
            StartPoint = Program.GetLocation(i);
        }


        public virtual void Draw() { }


        protected void DrawFrame()
        {
            if (Console.CursorVisible)
            {
                Program.DebugDraw();
            }
            else
            {
                SetColor();
                char[] c;
                if (Chosen)
                    c = boxc;
                else
                    c = box;
                Console.SetCursorPosition(StartPoint.X, StartPoint.Y);
                Console.Write(c[2]);
                for (int i = 0; i < Width; i++)
                {
                    Console.Write(c[0]);
                }
                Console.Write(c[3]);
                Console.SetCursorPosition(StartPoint.X, StartPoint.Y + Height + 1);
                Console.Write(c[5]);
                for (int i = 0; i < Width; i++)
                {
                    Console.Write(c[0]);
                }
                Console.Write(c[4]);
                for (int i = 0; i < Height; i++)
                {
                    Console.SetCursorPosition(StartPoint.X, StartPoint.Y + i + 1);
                    Console.Write(c[1]);
                    Console.SetCursorPosition(StartPoint.X + Width + 1, StartPoint.Y + i + 1);
                    Console.Write(c[1]);
                }
                SetDefColor();
            }
        }


        protected void ReadKeyPress()
        {
            bool stop = false;
            while (!stop)
            {
                ConsoleKey k = Console.ReadKey(true).Key;
                switch (k)
                {
                    case ConsoleKey.Tab:
                        stop = true;
                        Program.SwitchChosen();
                        break;
                    case ConsoleKey.Enter:
                        stop = true;
                        Act();
                        break;
                    case ConsoleKey.Backspace:
                        stop = true;
                        Del();
                        break;
                }
            }
        }


        public virtual void Act()
        {
            ChangeChosen(false);
            Program.AddNewPlan();
        }


        public virtual void Del()
        {
            Program.l.RemoveAt(Program.ActivePlan);
            Program.UpdateLocs(Program.ActivePlan);
            Program.DrawAll();
            Program.SaveToFile();
            ChangeChosen(false);
            Program.l[Program.ActivePlan].ChangeChosen(true);
        }


        public void ChangeChosen(bool b)
        {
            Chosen = b;
            SetColor();
            DrawFrame();
            if (b)
            {
                ReadKeyPress();
            }
        }


        protected virtual void SetColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }


        public static void SetDefColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = Program.BackCol;
        }


        protected void PaintInside()
        {
            SetColor();
            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(StartPoint.X + 1, StartPoint.Y + i + 1);
                for (int j = 0; j < Width; j++)
                {
                    Console.Write(' ');
                }
            }
        }
    }
}
