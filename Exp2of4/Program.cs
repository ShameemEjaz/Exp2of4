using System;
using System.Diagnostics;
//using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
//using System.Text;
//using System.Threading.Tasks;

namespace Exp2of4
{
    class Calculate
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCP(uint wCodePageID);


        static void Main(string[] args)
        {
            SetConsoleOutputCP(65001);
            SetConsoleCP(65001);

            var cmd = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            cmd.Start();

            cmd.StandardInput.WriteLine("chcp 65001");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            Console.OutputEncoding = System.Text.Encoding.UTF8;




            Console.BackgroundColor = ConsoleColor.Cyan;
            SetUpMenu TheMenu = new SetUpMenu();
            TheMenu.TheMenu();
        }
    }
    class SetUpMenu
    {
        string FuncType = string.Empty;
        /// <summary>
        /// Routine to Disable /Enable windows buttons
        /// </summary>
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        /////////////////////
        public void TheMenu()
        {        
            ConsoleKeyInfo cki;            
            while (true)
            {
                //   string MenuStr = string.Empty;
                //   MenuStr = string.Format("\tSelect Function Number\n\t----------------------\n\t1. Summation\n\t2. Substraction\n\t3. Multiplication\n\n");
                DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
                Console.Clear();
                Console.Title = "Calculate";
                Console.SetWindowSize(80, 25);
                Console.SetBufferSize(80, 25);
                Console.SetCursorPosition(10, 1);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Select Function Number");
          //      Console.ResetColor();
                Console.SetCursorPosition(10, 2);
             //   Console.WriteLine("----------------------");
                for (int u = 0; u < 22; u++) Console.Write("\u2550");
                Console.SetCursorPosition(10, 4);
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("1. Summation");
                Console.SetCursorPosition(10, 5);
                Console.WriteLine("2. Subtraction");
                Console.SetCursorPosition(10, 6);
                Console.WriteLine("3. Multiplication");
                Console.SetCursorPosition(10, 7);
                Console.WriteLine("4. Division");
                Console.SetCursorPosition(10, 8);
                Console.WriteLine("9. Exit");
                Console.CursorVisible = false;
                //   char option = ((char)Console.ReadKey(true).Key);
                cki = Console.ReadKey(true);
                Console.CursorVisible = true;
                switch (cki.KeyChar)
                {
                    case '1':
                        FuncType = "[Summation]";
                        compute("sum");
                        break;
                    case '2':
                        FuncType = "[Subtraction]";
                        compute("sub");
                        break;
                    case '3':
                        FuncType = "[Multiplication]";
                        compute("mul");
                        break;
                    case '4':
                        FuncType = "[Division]";
                        compute("div");
                        break;
                    case '9': Console.CursorVisible = true; Environment.Exit(0);  break;
                }
                Console.SetCursorPosition(10, 22);                
                DeleteMenu(GetSystemMenu(GetConsoleWindow(), true), SC_MAXIMIZE, MF_BYCOMMAND);
            }
        }
        public void compute(string query)
        {
            bool GoAhead = true;
            bool quit = false;
            string TheInput = string.Empty;
            double[] TheArray = null;
            string[] StrArr = null;
            double res = 0;
            Functions func = new Functions();
            Console.SetCursorPosition(10, 10);
            Console.WriteLine(FuncType);                   //\u25BA
            Console.SetCursorPosition(10, 11);
            Console.WriteLine("Give comma separated numbers...[q {0} quit]\n", (char)'\u25BA');
            Console.SetCursorPosition(10, 13);
            TheInput = Console.ReadLine();
            if (TheInput == "q" | TheInput == "Q")
            {
                quit = true;
            }
                if (!quit)
                {
                    try
                    {
                        StrArr = TheInput.Split(',');
                        TheArray = new double[StrArr.Length];
                        for (int i = 0; i < TheArray.Length; i++)
                        {
                            //TheArray[i] = Convert.ToInt32(StrArr[i]);
                            TheArray[i] = Double.Parse(StrArr[i]);
                        }
                    }
                    catch (Exception ex)
                    {
                        GoAhead = false;
                        Console.CursorVisible = false;
                        Console.SetCursorPosition(10, 20);
                        Console.WriteLine(ex.Message);
                        Console.SetCursorPosition(10, 21);
                        Console.WriteLine("Press any key!");
                        Console.ReadKey();
                    }
                    if (GoAhead)
                    {
                        switch (query)
                        {
                            case "sum": res = func.Add(TheArray); break;
                            case "sub": res = func.Sub(TheArray); break;
                            case "mul": res = func.Mul(TheArray); break;
                            case "div": res = func.Div(TheArray); break;
                        }
                        Console.SetCursorPosition(10, 14);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Result: " + res);
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.SetCursorPosition(10, 22);
                        Console.Write("Press any key!");
                        Console.CursorVisible = false;
                        Console.ReadKey();
                    }
                }
        } 
    }
    class Functions
    {
        public double Add(params double[] iarr)
        {
            double sum = 0;
            foreach (int i in iarr)
            {
                sum += i;
            }
            return sum;
        }
        public double Mul(params double[] iarr)
        {
            //Initialising the Mul factor with first item in the array
            double mul = iarr[0];
            for (int x = 1; x < iarr.Length; x++)
            {
                //Checking the first item in the array with the next item in the array
                mul = mul * iarr[x];
            }
            return mul;
        }
        public double Sub(params double[] iarr)
        {
            Sort s = new Sort(iarr);
            double[] result = s.SortIt();
            double sub = result[0];
            for (int i = 1; i < result.Length; i++)
            {
                sub = sub - result[i];
            }
            return sub;
        }
        public double Div(params double[] iarr)
        {
            Sort s = new Sort(iarr);
            double[] result = s.SortIt();
            double div = result[0];
            for (int i = 1; i < result.Length; i++)
            {
                div = div / result[i];
            }
            return div;
        }
    }
    class Sort
    {
        private double[] TheArray = null;
        public Sort(double[] TheArr2Sort)
        {
            TheArray = TheArr2Sort;
        }
        //Sort the int array into descending order for subtraction
        public double[] SortIt()
        {
            double temp = 0;
            for (int i = 0; i < TheArray.Length - 1; i++)
            {
                for (int j = 0; j < TheArray.Length - 1; j++)
                {
                    //Less then or greater then makes it descending or ascending
                    if (TheArray[j] < TheArray[j + 1])
                    {
                        temp = TheArray[j + 1];
                        TheArray[j + 1] = TheArray[j];
                        TheArray[j] = temp;
                    }
                }
            }
            //for (int arr = 0; arr < TheArray.Length; arr++)
            //{
            //    Console.WriteLine(TheArray[arr]);
            //}
            return TheArray;
        }
    }
}
