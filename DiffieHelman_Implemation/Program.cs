using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffieHelman_Implemation
{
    class Program
    {
        public static long FastModularExponetiation(long baseNumber, long power, long groupSize)
        {
            long result = 1;
            string binaryOfPower = Convert.ToString(power, 2);
            List<long> factorsToMultiplicate = new List<long>();

            for (int i = binaryOfPower.Length - 1; i >= 0; i--)
            {
                if (binaryOfPower[i] == '1')
                {
                    factorsToMultiplicate.Add(baseNumber);
                }
                baseNumber = (baseNumber * baseNumber) % (long)groupSize;
            }

            foreach (var factor in factorsToMultiplicate)
            {
                result = (result * (long)factor) % groupSize;
            }

            return result;
        }

        static void Main(string[] args)
        {
            Random rnd = new Random();
            long primeNumber = 0;

            #region Setting primeNumber
            {
                bool isNotPrime;
                Console.WriteLine("Tell me your prime number:");
                do
                {
                    isNotPrime = false;

                    #region Get primeNumber

                    while (!long.TryParse(Console.ReadLine(), out primeNumber) || primeNumber < 2)
                    {
                        Console.WriteLine("It is not a number I wanted ! Try again:");
                    }

                    #endregion

                    #region Check if primeNumber is prime

                    for (long i = 2; i <= primeNumber / 2; i++)
                    {
                        if (primeNumber % i == 0)
                        {
                            Console.WriteLine("Given number is not prime number ! Try again:");
                            isNotPrime = true;
                            break;
                        }
                    }

                    #endregion

                } while (isNotPrime);
            }
            #endregion

            int generator = 0;

            #region Finding Generator
            {
                var dividersOfGroupPower = new Queue<int>();
                int maxDivider = 0;

                #region Filling dividersOfGroupPower
                {
                    for (int i = 2; i <= primeNumber / 2; i++)
                    {
                        if ((primeNumber - 1) % i == 0)
                        {
                            dividersOfGroupPower.Enqueue(i);
                            maxDivider = i;
                        }
                    }
                }
                #endregion

                var powerModN = new Dictionary<int, long>();

                for (long possibleGenerator = 2; possibleGenerator < primeNumber; possibleGenerator++)
                {
                    #region filling factorialModN
                    {
                        powerModN.Add(1, possibleGenerator);
                        for (int i = 1; i < maxDivider / 3; i *= 2)
                        {
                            powerModN.Add(i * 2, (powerModN[i] * powerModN[i]) % primeNumber);
                        }
                    }
                    #endregion

                    long result = 0;
                    int exponent;

                    #region Checking All Dividers For possibleGenerator

                    while (dividersOfGroupPower.Count > 0 && result != 1)
                    {
                        result = 1;
                        exponent = 0;
                        int curExponent;
                        while (exponent != dividersOfGroupPower.Peek())
                        {
                            curExponent = powerModN.Keys.Where(x => (x <= dividersOfGroupPower.Peek() - exponent)).Max();
                            result *= powerModN[curExponent];
                            result %= primeNumber;
                            exponent += curExponent;
                        }

                        if (result == 1)
                        {
                            break;
                        }
                        else
                        {
                            dividersOfGroupPower.Dequeue();
                        }
                    }

                    #endregion

                    if (dividersOfGroupPower.Count == 0)
                    {
                        generator = (int)possibleGenerator;
                        break;
                    }
                    else
                    {
                        powerModN.Clear();
                    }
                }
            }
            #endregion

            long userResult;

            #region Getting userResult
            { 
                Console.WriteLine("Ok, the generator of our group is " + generator + Environment.NewLine +
                    "Now I need you to raise generator to a random power and give me the result of calculation done in our group");
                
                Console.Write("Result: ");
                while (!long.TryParse(Console.ReadLine(), out userResult) || userResult < 1 || userResult >= primeNumber)
                {
                    Console.WriteLine("It is not a number I wanted. Try again.");
                }
            }
            #endregion


            long computerResult;
            long computerPower;

            #region Getting computerResult
            {
                computerPower = (long)rnd.NextDouble() * primeNumber;

                computerResult = (int)FastModularExponetiation((long)generator, computerPower, (long)primeNumber);
            }
            #endregion

            Console.WriteLine("Ok, here is my result: " + computerResult.ToString() + Environment.NewLine +
                "Now rise my result to your power and we will get same number!");

            int finalResult = (int)FastModularExponetiation((long)userResult, computerPower, (long)primeNumber);

            Console.WriteLine("My calculations are done :)" + Environment.NewLine +
                "The final result is: " + finalResult.ToString() + Environment.NewLine +
                "Have you got the same value? ;)");

            Console.ReadKey();
        }

        
    }
}
