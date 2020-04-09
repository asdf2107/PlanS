using System;
using System.Collections.Generic;
using System.Text;

namespace PlanS
{
    public class PlanPlus : Frame
    {
        public PlanPlus(int i) : base(i)
        {
            Draw();
        }


        public override void Draw()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            DrawFrame();
            PaintInside();
            DrawPlus();
        }


        protected void DrawPlus()
        {
            Console.SetCursorPosition(StartPoint.X + 13, StartPoint.Y + 2);
            Console.Write(box[1]);
            Console.SetCursorPosition(StartPoint.X + 13, StartPoint.Y + 4);
            Console.Write(box[1]);
            Console.SetCursorPosition(StartPoint.X + 11, StartPoint.Y + 3);
            Console.Write(box[0]);
            Console.SetCursorPosition(StartPoint.X + 12, StartPoint.Y + 3);
            Console.Write(box[0]);
            Console.SetCursorPosition(StartPoint.X + 14, StartPoint.Y + 3);
            Console.Write(box[0]);
            Console.SetCursorPosition(StartPoint.X + 15, StartPoint.Y + 3);
            Console.Write(box[0]);
            Console.SetCursorPosition(StartPoint.X + 13, StartPoint.Y + 3);
            Console.Write(box[6]);
        }


        public override void Del()
        {
            ChangeChosen(true);
        }
    }
}
