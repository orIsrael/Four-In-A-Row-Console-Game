using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogic;

namespace ConsoleUI
{
    class GameFront
    {
        private const char k_Player1Sign = 'X';
        private const char k_Player2Sign = 'O';
        private readonly int r_MinDimension;
        private readonly int r_MaxDimension;

        private GameManager BackEnd { get; set; }

        public GameFront()
        {
            r_MinDimension = GameManager.k_MinDimension;
            r_MaxDimension = GameManager.k_MaxDimension;
        }

        public void Run()
        {
            try
            {
                eGameType type = getGameType();
                int rows = getRows();
                int cols = getCols();
                BackEnd = new GameManager(type, k_Player1Sign, k_Player2Sign, rows, cols);
                startGame();
            }
            catch (ExitGameException exitException)
            {
                IO.PrintMsg(exitException.Message);
            }
        }

        private void startGame()
        {
            int col;
            int row;
            bool isGameOver = false;
            bool isNewGame;

            while (isGameOver == false)
            {
                isNewGame = false;

                printBoard();

                if (BackEnd.CurrentTurn == eTurn.Player1 || BackEnd.GameType == eGameType.HumanVSHuman)
                {
                    IO.getUserPosition(BackEnd, out col, out row);
                    BackEnd.SetMove(row, col);
                }

                else
                {
                  BackEnd.ComputerPlayer.MakeMove();
                }

                printBoard();
                

                if (CheckIfGameOver() == true)
                {
                    isGameOver = true;
                    IO.DisplayPointsStatus(BackEnd.Player1Points, BackEnd.Player2Points);
                    if (IO.CheckIfUserWantToContinue() == true)
                    {
                        BackEnd.ClearGame();
                        isGameOver = false;
                        isNewGame = true;
                    }
                }

                if (isNewGame == false)
                {
                    BackEnd.ChangeTurn();
                }
            }
        }

        private void printBoard()
        {
            int signAmount = 1 + (4 * BackEnd.GameBoard.Cols);
            StringBuilder result = new StringBuilder();

            Ex02.ConsoleUtils.Screen.Clear();
            for (int i = 0; i < BackEnd.GameBoard.Cols ; i++)
            {
                result.AppendFormat("  {0} ", i + 1);
            }
            result.AppendLine();

            for (int i = 0; i < BackEnd.GameBoard.Rows; i++)
            {
                for (int j = 0; j < BackEnd.GameBoard.Cols; j++)
                {
                    result.AppendFormat("| {0} ", BackEnd.GameBoard.GetValue(i, j));
                }

                result.Append("|");
                result.AppendLine();
                result.Append('=', signAmount);
                result.AppendLine();
            }

            Console.WriteLine(result);
        }
        private int getCols()
        {
            const string msg = "Please enter amount of cols (numebr between 4 to 8)";

            return IO.GetIntInputBetweenLimits(msg, r_MinDimension, r_MaxDimension);
        }

        private int getRows()
        {
            const string msg = "Please enter amount of rows (numebr between 4 to 8)";

            return IO.GetIntInputBetweenLimits(msg, r_MinDimension, r_MaxDimension);
        }

        private eGameType getGameType()
        {
            const string msg = 
                @"Please choose game type:
1. Human VS Human
2. Human VS Computer";

            return IO.GetGameTypeFromUser(msg);
        }

        public bool CheckIfGameOver()
        {
            bool isGameOver = false;

            if (BackEnd.CheckIfWon() == true)
            {
                isGameOver = true;

                if (BackEnd.CurrentTurn == eTurn.Player1)
                {
                    BackEnd.Player1Points++;
                    IO.ShowWinner("Player 1");
                }
                else
                {
                    BackEnd.Player2Points++;
                    IO.ShowWinner("Player 2");
                }
            }

            else if (BackEnd.CheckIfDraw() == true)
            {
                isGameOver = true;
                IO.AnounceDraw();
            }

            return isGameOver;
        }
    }
}
