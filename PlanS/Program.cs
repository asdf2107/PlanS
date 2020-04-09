using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;


namespace PlanS
{
    class Program
    {
        public static int Interval = 1, Width = 84, Height = 30, ActivePlan = 0;
        public static List<Frame> l = new List<Frame>() { new PlanPlus(0) };
        public static ConsoleColor BackCol = ConsoleColor.Black;


        static void Main(string[] args)
        {
            Setup();
            try
            {
                LoadFromFile();
            }
            catch
            {
                NewFile();
            }
            ActivePlan = 0;
            l[0].ChangeChosen(true);
            Console.ReadKey(true);
        }


        private static void Setup()
        {
            Console.Title = "PlanS";
            Console.CursorVisible = false;
            Console.WindowHeight = Height;
            Console.WindowWidth = Width;
            Console.BufferHeight = Height;
            Console.BufferWidth = Width;
        }


        public static void SaveToFile()
        {
            List<string> p = new List<string>();
            for (int i = 0; i < l.Count - 1; i++)
            {
                p.Add(((Plan)l[i]).GetInfoString());
            }
            File.WriteAllLines("plans.txt", p);
        }


        private static void LoadFromFile()
        {
            string[] s = File.ReadAllLines("plans.txt");
            bool[] nameUsed = new bool[s.Length];
            List<DateTime> d = new List<DateTime>();
            for (int i = 0; i < s.Length; i++)
            {
                d.Add(DateTime.Parse(s[i].Split("|").First()));
            }
            d.Sort();

            foreach (DateTime dt in d)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i].Split("|").First() == dt.ToString() && !nameUsed[i])
                    {
                        AddPlan(new Plan(l.Count - 1, dt, s[i].Split("|").Last()));
                        nameUsed[i] = true;
                    }
                }
            }
        }


        public static void NewFile()
        {
            File.WriteAllText("plans.txt", "");
        }


        private static void AddPlan(Plan p)
        {
            l.Insert(l.Count - 1, p);
            for (int i = 0; i < l.Count; i++)
            {
                l[i].StartPoint = GetLocation(i);
                l[i].Draw();
            }
        }


        public static void AddNewPlan()
        {
            AddPlan(new Plan(l.Count - 1));
            SaveToFile();
            l.Last().ChangeChosen(true);
        }


        public static Point GetLocation(int count)
        {
            Point res = new Point(0, 0);
            int i = 0;
            for (int y = 0; y <= count; y++)
            {
                res.Y = y * (Frame.Height + 2 + Interval);
                for (int x = 0; x <= count; x++)
                {
                    if ((x + 1) * (Frame.Width + 1 + Interval) > Width)
                    {
                        break;
                    }
                    res.X = x * (Frame.Width + 2 + Interval);
                    if (i == count)
                        return res;
                    i++;
                }
            }
            throw new Exception();
        }


        public static void SwitchChosen()
        {
            l[ActivePlan].ChangeChosen(false);
            if (ActivePlan + 1 == l.Count)
                ActivePlan = 0;
            else
                ActivePlan++;
            l[ActivePlan].ChangeChosen(true);
        }


        public static void UpdateLocs(int startI)
        {
            for (int i = startI; i < l.Count; i++)
            {
                l[i].StartPoint = GetLocation(i);
            }
        }


        public static void DrawAll()
        {
            foreach (Frame f in l)
            {
                f.Draw();
            }
            DrawEndBlank();
        }


        public static void DebugDraw()
        {
            Frame.SetDefColor();
            Console.CursorVisible = false;
            Blank();
            DrawAll();
        }


        public static void Blank()
        {
            Console.Clear();
        }


        private static void DrawEndBlank()
        {
            Point p = GetLocation(l.Count);
            Console.BackgroundColor = BackCol;
            for (int y = p.Y; y < p.Y + Frame.Height + 2; y++)
            {
                for (int x = p.X; x < p.X + Frame.Width + 2; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(' ');
                }
            }
        }
    }
}