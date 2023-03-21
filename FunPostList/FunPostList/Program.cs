using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace FunPost
{
    class FunPost
    {
        public static int NumberOfVars = -1;
        
        static void Main() //i'm so post post, i'm so meta me~eta~~
        {
            Greetings();
            
            GetNumberOfVariables();
            
            ExplainPostClasses(WhatPostClasses(GetEvalClever()));
        }

        public static void Greetings()
        {
            Console.WriteLine($"Hello there! I'm a calculator of a Post-Punk's classes");
            Console.WriteLine("I receive eval in the follow format: 1 1 1 0 0 1 0 1");
        }
        
        public static void GetNumberOfVariables()
        {
            while (NumberOfVars < 0)
            {
                try
                {
                    Console.WriteLine("Enter number of variables");
                    NumberOfVars = int.Parse(Console.ReadLine() ?? "");
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Incorrect number of variables!");
                    Console.ResetColor();
                }
            }
        }
        
        public static bool[] GetEvalClever()
        {
            var res = new bool[2];

            bool incorrect = true;
            
            while (incorrect)
            {
                try
                {
                    incorrect = false;
                    Console.WriteLine("Enter an eval:");
                    var input = Console.ReadLine() ?? "";

                    if (!input.All(x => x == '0' || x == '1' || x == ' '))
                        throw new ArgumentException("forbidden symbol detected error");
                    
                    res = Regex.Split(input, @"\s+").Select(x => int.Parse(x) == 1).ToArray();
                    
                    if (res.Length != (int)Math.Pow(2, NumberOfVars))
                        throw new ArgumentException($"incorrect eval length detected error: " +
                                                    $"was {res.Length}. correct is {(int)Math.Pow(2,NumberOfVars)}");
                }
                catch (Exception e)
                {
                    incorrect = true;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{e.Message}");
                    Console.ResetColor();
                }
            }
            
            return res;
        }
        
        public static bool[] GetEvalStupidly()
        {
            var res = new bool[(int)Math.Pow(2,NumberOfVars)];
            Console.WriteLine("Enter an eval:");
            
            for (int i = 0; i < res.Length; i++)
            {
                var input = -1;
                
                while (input != 0 && input != 1)
                {
                    try
                    {
                        input = int.Parse(Console.ReadLine());
                        if (input != 0 && input != 1)
                            Console.WriteLine($"Enter a correct value of eval[{i}] - (0 / 1)");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Only numbers supported!! Sorry!~~");
                    }
                }

                res[i] = input != 0;
            }

            Console.WriteLine("Fine! I've entered current eval:");
            foreach (var elem in res)
            {
                Console.Write($"{(elem ? 1 : 0)} ");
            }
            Console.WriteLine();

            return res;
        }

        public static bool IsP0(bool[] eval)
        {
            return !eval[0];
        }

        public static bool IsP1(bool[] eval)
        {
            return eval[^1];
        }
        
        public static bool IsL(bool[] eval)
        {
            var linearFunCoefficient = new bool[NumberOfVars + 1];
            linearFunCoefficient[0] = eval[0];
            var coefficientLine = 1;
            
            for (int i = 1; i < NumberOfVars + 1; i++)
            {
                linearFunCoefficient[i] = eval[coefficientLine] ^ linearFunCoefficient[0];
                coefficientLine *= 2;
            }

            var linearFunEval = new bool[(int)Math.Pow(2,NumberOfVars)];

            for (int i = 0; i < linearFunEval.Length; i++)
            {
                var icopy = i;
                var coeffnum = 0;
                linearFunEval[i] = linearFunCoefficient[0];
                
                while (icopy > 0)
                {
                    coeffnum++;
                    linearFunEval[i] = linearFunEval[i] ^ ((icopy % 2 == 1) && linearFunCoefficient[coeffnum]);
                    icopy /= 2;
                }
            }

            var res = true;

            for (int i = 0; i < eval.Length; i++)
            {
                res = res && (eval[i] == linearFunEval[i]);
            }
            return res;
        }
        
        public static bool IsS(bool[] eval)
        {
            var antiantieval = eval.Reverse().Select(x => !x).ToArray();
            var equals_questionmark = true;
            
            //TODO aggregate pls (like a senior)
            
            for (int i = 0; i < eval.Length; i++)
                equals_questionmark = equals_questionmark && (antiantieval[i] == eval[i]);

            return equals_questionmark;
        }
        
        public static bool IsM(bool[] eval)
        {
            for (int i = 0; i < eval.Length; i++)
            {
                if (eval[i])
                {
                    var icopy = i;
                    var binpower = 1;
                    while (icopy > 0)
                    {
                        if (icopy % 2 == 0 && (eval[i] ? 1 : 0) > (eval[i + binpower] ? 1 : 0)) // can be simplified lol.
                            return false;
                        icopy /= 2;
                        binpower *= 2;
                    }
                }
            }

            return true;
        }

        public static bool[] WhatPostClasses(bool[] eval)
        {
            var res = new bool[5];
            res[0] = IsP0(eval);
            res[1] = IsP1(eval);
            res[2] = IsL(eval);
            res[3] = IsS(eval);
            res[4] = IsM(eval);
            return res;
        }

        public static void ExplainPostClasses(bool[] postClasses)
        {
            var num = 1;
            Console.WriteLine("\nHello again! Happy to report that your function belongs to these post classes:");
            if (postClasses[0])
            {
                Console.WriteLine($"{num++}: P0 class!");
            }
            if (postClasses[1])
            {
                Console.WriteLine($"{num++}: P1 class!");
            }
            if (postClasses[2])
            {
                Console.WriteLine($"{num++}: L class!");
            }
            if (postClasses[3])
            {
                Console.WriteLine($"{num++}: S class!");
            }
            if (postClasses[4])
            {
                Console.WriteLine($"{num++}: M class!");
            }
            if (num == 1)
                Console.WriteLine("Umm, judging by the fact that it is empty, your function does not belong to any Post class! You have a very punk func!");
            if (num == 5)
                Console.WriteLine("That's impressive!");
        }
        
    }
}