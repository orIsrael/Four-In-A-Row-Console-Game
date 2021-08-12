using GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    internal static class IO
    {
        private const string k_InputValidMsg = "Input is invalid, Please try again";

        private const string k_ExitString = "Q";

        internal static eGameType GetGameTypeFromUser(string i_Msg)
        {
            bool inputValid = false;
            eGameType result = eGameType.HumanVSComputer;
            
            Console.WriteLine(i_Msg);
            
            while (inputValid == false)
            {
                string strInput = Console.ReadLine();
                
                if (strInput == k_ExitString)
                {
                    throw new ExitGameException();
                }

                bool tryParseResult = int.TryParse(strInput, out int resultInt);
                

                if (tryParseResult == true)
                {
                    if (Enum.IsDefined(typeof(eGameType), resultInt))
                    {
                        result = (eGameType)resultInt;
                        inputValid = true;
                    }
                }
                
                if(inputValid == false)
                {
                    Console.WriteLine(k_InputValidMsg);
                }
            }

            return result;
        }

        internal static void getUserPosition(GameManager i_GameManager, out int o_Col, out int o_Row)
        {
            bool isValidCol = false;
            int intColInput = -1;

            o_Row = -1;
            o_Col = -1;

            while (isValidCol == false)
            {
                string strColInput = System.Console.ReadLine();
                int.TryParse(strColInput, out intColInput);
                int matrixCol = intColInput - 1;

                if (i_GameManager.checkIfColValid(matrixCol) == true &&
                    i_GameManager.GetNextFreeRow(matrixCol,out o_Row) == true)
                {
                    isValidCol = true;
                   o_Col = intColInput - 1;

                }

                else
                {
                    PrintMsg("invalid column!");
                }
            }

            
        }

        internal static bool CheckIfUserWantToContinue()
        { 
            Console.WriteLine("Would you like To Play /another one?");
            Console.WriteLine("For yes: Press y\n For not: Press n");

            return GetUserChoiceToContinue();
        }

        internal static void DisplayPointsStatus(int i_Player1Points, int i_Player2Points)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("Player 1: {0}\nPlayer 2: {1} ", i_Player1Points, i_Player2Points);
            Console.WriteLine(result);
        }

        private static bool GetUserChoiceToContinue()
        {
            bool isValidInput = false;
            bool isWantToContinue = false;
            char input;

            while(isValidInput == false)
            {
                input = Console.ReadLine().ElementAt(0);
                if(input == 'y')
                {
                    isWantToContinue = true;
                    isValidInput = true;
                }
                else if(input == 'n')
                {
                    isValidInput = true;
                }
            }

            return isWantToContinue;
        }

        internal static void PrintMsg(string i_Msg)
        {
            Console.WriteLine(i_Msg);
        }

        internal static int GetIntInputBetweenLimits(string i_Msg, int i_MinDimension, int i_MaxDimension)
        {
            bool inputValid = false;
            int result = 4;

            Console.WriteLine(i_Msg);

            while (inputValid == false)
            {
                string strInput = Console.ReadLine();

                if(strInput == k_ExitString)
                {
                    throw new ExitGameException();
                }

                bool tryParseResult = int.TryParse(strInput, out result);

                if (tryParseResult == true)
                {
                    if(result >= i_MinDimension && result <= i_MaxDimension)
                    {
                        inputValid = true;
                    }
                }

                if (inputValid == false)
                {
                    Console.WriteLine(k_InputValidMsg);
                }
            }

            return result;
        }

        internal static void ShowWinner(string i_Winner)
        {
            Console.WriteLine("Game Over!");
            Console.WriteLine($"The winner is: {i_Winner}!" );
        }

        internal static void AnounceDraw()
        {
            Console.WriteLine("Game Over!");
            Console.WriteLine("It's a draw!!!");
        }
    }
}
