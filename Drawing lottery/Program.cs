using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;


namespace FirstProject
{
    class Program
    {
        static Random random = new Random();
        static int jackpot;
        static int couponPrice = 3;
        static int startMoney = 6;
        static ConsoleKey userKey;

        static void Main(string[] args)
        {

            do
            {
                int money = startMoney;
                int day = 0;
                bool outOfMoney = false;

                do
                {
                    day++;
                    List<int[]> AllCouponList = new List<int[]>();
                    jackpot = random.Next(1, 38) * 1000000;

                    if (money < 1)
                    {
                        outOfMoney = true;
                        break;
                    }

                    do
                    {
                        #region gameStats
                        Console.WriteLine($"DAY: {day}\n");
                        Console.WriteLine($"Welcome to the National Lottery game!, today's jackpot is {jackpot}!");

                        Console.WriteLine($"Your account balance is: {money} $.\n");


                        if (AllCouponList.Count > 0)
                        {
                            showBets(AllCouponList);
                        }
                        else
                        {
                            Console.WriteLine("You don't have any bets.");
                        }
                        #endregion gameStats
                        #region menu

                        if (money >= 3 && AllCouponList.Count < 8)
                        {
                            Console.WriteLine($"\n1 - Place a bet! - -3 $ [{AllCouponList.Count}/8]");
                        }

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("2 - Lottery!");
                        Console.ResetColor();



                        Console.WriteLine("3 - End the game");
                        userKey = Console.ReadKey().Key;

                        if (userKey == ConsoleKey.D1)
                        {
                            List<int> betNumbers = new List<int>();
                            betNumbers = MakeABet();
                            betNumbers.Sort();
                            AllCouponList.Add(betNumbers.ToArray());
                            money -= couponPrice;
                        }
                        else if (userKey == ConsoleKey.D2)
                        {
                            if (AllCouponList.Count > 0)
                            {
                                int winValue = 0;
                                winValue = Draw(AllCouponList);
                                if (winValue == 0)
                                {
                                    Console.WriteLine("Unfortunately you don't win!");
                                    Console.ReadKey();
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.WriteLine($"Congratulations! You win {winValue} $!");
                                    money += winValue;
                                    Console.ReadKey();
                                    Console.Clear();
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nYou don't have any bets in this draw!");
                                Console.ReadKey();

                                Console.Clear();
                            }

                        }
                        else
                        {
                            Console.Clear();
                        }

                        #endregion menu


                    } while (userKey == ConsoleKey.D1);


                } while (userKey != ConsoleKey.D3);

                #region endScreen
                Console.Clear();
                if (outOfMoney)
                {
                    Console.WriteLine("You're out of money! Game end!");
                }
                else
                {
                    Console.WriteLine("You left the lottery. Game End!");
                }

                Console.WriteLine($"Your winnings record is  {money - startMoney} $ in {day} days.");
                Console.WriteLine("To restart the game press ENTER; to exit press any key.");
                userKey = Console.ReadKey().Key;
                Console.Clear();

                #endregion endScreen


            } while (userKey == ConsoleKey.Enter);

        }
        #region Drawing lottery
        private static int Draw(List<int[]> AllCouponList)
        {
            Console.Clear();

            int[] winTable = new int[4];

            int sum = 0;

            List<int> winningNumbers = new List<int>();


            for (int i = 0; i < 6; i++)
            {
                int value = random.Next(1, 50);
                if (winningNumbers.Contains(value))
                {
                    i--;
                    continue;

                }
                else
                {
                    winningNumbers.Add(value);
                }
            }
            winningNumbers.Sort();

            Console.Write("Winning numbers: ");
            foreach (var item in winningNumbers)
            {
                Console.Write(item + ", ");
            }

            int couponNumber = 0;
            Console.WriteLine("\nYour bets: ");
            foreach (int[] coupon in AllCouponList)
            {
                couponNumber++;
                int match = 0;
                Console.Write($"{ couponNumber}: ");
                foreach (int number in coupon)
                {
                    if (winningNumbers.Contains(number))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        match++;
                        Console.Write(number + ", ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(number + ", ");
                    }

                }
                Console.WriteLine($"- Matches: {match}/6");

                switch (match)
                {
                    case 3:
                        winTable[0]++;
                        break;
                    case 4:
                        winTable[1]++;
                        break;
                    case 5:
                        winTable[2]++;
                        break;
                    case 6:
                        winTable[3]++;
                        break;
                }

            }

            if (winTable[0] > 0)
            {

                sum += 24 * winTable[0];
                Console.WriteLine($"\n3: {winTable[0].ToString()} matches for  + {sum} $.");
            }
            if (winTable[1] > 0)
            {
                sum += 240 * winTable[1];
                Console.WriteLine($"\n4: {winTable[1].ToString()} matches for   + {sum} $.");
            }
            if (winTable[2] > 0)
            {
                sum += 7000 * winTable[2];
                Console.WriteLine($"\n5: {winTable[2].ToString()} matches for   + {sum} $.");
            }
            if (winTable[3] > 0)
            {
                sum += (jackpot * winTable[3]) / (winTable[3] * random.Next(0, 3));
                Console.WriteLine($"\n6: {winTable[3].ToString()} matches for   + {sum} $.");
            }

            return sum;
        }
        #endregion lottery

        #region Making bet
        private static List<int> MakeABet()
        {
            int index = 0;
            List<int> userNumbers = new List<int>();
            Console.Clear();

            do
            {

                Console.Write("Your numbers: ");
                foreach (var item in userNumbers)
                {

                    if (item != 0)
                    {
                        Console.Write(item + ", ");
                    }

                }

                Console.WriteLine("\nEnter a number between 1 and 49");


                Console.WriteLine($"{index + 1}/6");



                int userInput = -1;
                bool check = int.TryParse(Console.ReadLine(), out userInput);

                if (userNumbers.Contains(userInput))
                {
                    Console.WriteLine("You selected again the same number!");
                    Console.ReadKey();
                    Console.WriteLine("Press any key to continue...");
                }
                else if (userInput > 0 && userInput <= 49 && check == true)
                {

                    userNumbers.Add(userInput);
                    index++;
                }
                else
                {
                    Console.WriteLine("You selected wrong value!");
                    Console.ReadKey();
                    Console.WriteLine("Press any key to continue...");

                }


                Console.Clear();

            } while (index < 6); return userNumbers;

        }
        #endregion bet

        #region Showing bets
        private static void showBets(List<int[]> AllCouponList)
        {

            int couponNum = 0;
            Console.WriteLine("Your bets: ");
            foreach (var los in AllCouponList)
            {
                couponNum++;
                Console.Write($"{couponNum}: ");
                foreach (var item in los)
                {
                    Console.Write(item + ", ");
                }
                Console.WriteLine();
            }

        }
        #endregion showBets
    }


}