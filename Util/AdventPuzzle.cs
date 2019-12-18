using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Util
{
    public abstract class AdventPuzzle
    {
        private readonly string inputPath = "../../../input.txt";
        public object input;
        //public T ReadInput<T>()
        //{
        //    switch (default(T))
        //    {
        //        case int[] _:
        //        case double[] _:
        //        case char[] _:
        //        case string[] _:
        //            return (T)File.ReadAllLines(inputPath).Select(l => Convert.ChangeType( l, typeof(T)));                
        //        case int _:
        //        case double _:
        //        case char _:
        //        case string _:
        //            return (T)Convert.ChangeType(File.ReadAllText(inputPath), typeof(T));
        //        case object _:
        //            return ParseInput<T>();
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}

        public T[] ReadInputArray<T>()
        {
            return File.ReadAllLines(inputPath).Select(l => (T)Convert.ChangeType(l, typeof(T))).ToArray();
        }
        public T ReadInputText<T>()
        {
            return (T)Convert.ChangeType(File.ReadAllText(inputPath), typeof(T));
        }

        public abstract string SolveFirstPuzzle();

        public abstract string SolveSecondPuzzle();

        public void Solve(Puzzle puzzle = Puzzle.Both)
        {

            Stopwatch watch = new Stopwatch();

            string toClipBoard = null;

            switch (puzzle)
            {
                case Puzzle.First:
                    SolveSingle("# FIRST PUZZLE", SolveFirstPuzzle, ref toClipBoard); break;
                case Puzzle.Second:
                    SolveSingle("# SECOND PUZZLE", SolveSecondPuzzle, ref toClipBoard); break;
                case Puzzle.Both:
                    SolveSingle("# FIRST PUZZLE", SolveFirstPuzzle, ref toClipBoard);
                    SolveSingle("# SECOND PUZZLE", SolveSecondPuzzle, ref toClipBoard);
                    break;
            }


            if (toClipBoard != null)
            {
                Console.WriteLine("Copy the result? Y/N");
                //var key = Console.ReadLine();

                //if (key.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
                //    Clipboard.SetText(toClipBoard);
            }
            else
            {
                Console.ReadLine();
            }

        }

        private void SolveSingle(string name, Func<string> SolveFunction, ref string toClipBoard)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            try
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine(name);
                Console.ResetColor();

                string solution = SolveFunction();
                toClipBoard = solution;
                TimeSpan secondsElapsed = watch.Elapsed;

                PrettyPrintResults(secondsElapsed, solution);


            }
            catch (Exception e)
            {
                PrettyPrintError(e.Message);
            }
            finally
            {
                watch.Stop();
                Console.WriteLine();
                Console.ResetColor();
            }
        }

        private void PrettyPrintResults(TimeSpan duration, string result)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"Duration: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(duration.TotalSeconds.ToString());

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"Solution: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(result);
        }

        private void PrettyPrintError(string error)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Error: ");
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(error);
        }
    }

    public enum Puzzle
    {
        First,
        Second,
        Both
    }


}
