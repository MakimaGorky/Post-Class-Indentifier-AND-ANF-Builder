using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ANFbuilder
{

    class builder
    {

        static void Main()
        {
            Greetings();
            
            var numofvar = GetNumberOfVariables();

            ANFWriter(ANFconstructor(ANFcoefficients(GetEvalClever(numofvar)), numofvar));
        }

        public static void Greetings()
        {
            Console.WriteLine("Good day, Kirill Vladimirovich! How was your day?");
            Console.WriteLine("I calculate anf by eval. But I warn you, enter eval carefully, in the follow format: 1 1 1 0 0 1 0 1");
            Console.WriteLine("let's get started (^w^)");
        }
        
        public static int GetNumberOfVariables()
        {
            var numOfVars = -1;
            
            while (numOfVars < 0)
            {
                try
                {
                    Console.WriteLine("Enter number of variables");
                    numOfVars = int.Parse(Console.ReadLine() ?? "");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Incorrect number of variables!");
                }
            }

            return numOfVars;
        }

        public static bool[] GetEvalClever(int numOfVars)
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
                    
                    if (res.Length != (int)Math.Pow(2, numOfVars))
                        throw new ArgumentException($"incorrect eval length detected error: was {res.Length}. correct is {(int)Math.Pow(2,numOfVars)}");
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
        
        public static bool[] GetEvalStupidly(int numOfVars)
        {
            var res = new bool[(int)Math.Pow(2,numOfVars)];
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

        public static bool[] ANFcoefficients(bool[] eval)
        {
            var coefficients = eval;
            
            for (int i = 0; i < eval.Length; i++)
                for (int j = i + 1; j < eval.Length; j++)
                    if ((i & j) == i)
                        coefficients[j] ^= coefficients[i];

                return coefficients;
        }

        public static string ANFconstructor(bool[] coefs, int numOfVars)
        {
            var anf = new StringBuilder();
            var binaryEthalon = coefs.Length / 2;

            //Console.WriteLine(coefs.Length);
            
            for (int i = coefs.Length - 1; i >= 0; i--)
                if (coefs[i])
                {
                    var bintemp = binaryEthalon;
                    var addedSth = false;
                    
                    //Console.WriteLine($"coef: {i}, bineth: {binaryEthalon},bin: {bintemp}");
                    
                    for (int j = 1; j <= numOfVars; j++)
                    {
                        //Console.Write($"{i}, {bintemp}");
                        
                        if ((i & bintemp) == bintemp)
                        {
                            //Console.Write($" {j} lol");
                            
                            addedSth = true;
                            anf.Append($"x_{j}");
                        }

                        //Console.WriteLine();
                        
                        bintemp >>= 1;
                    }

                    if (addedSth)
                        anf.Append($" + ");
                }

            if (coefs[0])
                anf.Append($"1");
            else
                anf.Remove(anf.Length - 3, 3);

            return anf.ToString();
        }

        public static void ANFWriter(string exclusivelyANF)
        {
            Console.WriteLine("Algebraic normalized formula of this bool function itself:");
            Console.WriteLine($"{exclusivelyANF}");
        }

    }
    
}