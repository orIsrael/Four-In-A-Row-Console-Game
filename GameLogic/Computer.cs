using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Computer
    {
        private Random rand;
        public Computer(char i_Sign, Board i_GameBoard)
        {
            Sign = i_Sign;
            GameBoard = i_GameBoard;
            rand = new Random();
        }

        public char Sign { get; }
        public Board GameBoard { get; }

        public void MakeMove()
        {
            Move blockPosition = new Move();

            if (tryGetBlockPosition(ref blockPosition) == true)
            {
                block(ref blockPosition);
            }
            else
            {
                makeRandomMove();
            }
        }

        private void makeRandomMove()
        { 
            bool isValidCol= false;
            int col = -1;
            int row = -1;

            while (isValidCol == false)
            {
                col = rand.Next(0, GameBoard.Cols - 1);
                isValidCol = getNextFreeRow(col, out row);
            }

            GameBoard.SetValue(row, col, Sign);
            GameBoard.LastMove = new Move(row, col, Sign);
        }
        private bool getNextFreeRow(int i_Col, out int o_Row)
        {
            int CurrentRow = 0;
            bool validRowFound = false;

            o_Row = -1;
            for (int i = 0; i < GameBoard.Rows; i++)
            {
                CurrentRow = GameBoard.Rows - i - 1;
                if (GameBoard.GetValue(CurrentRow, i_Col) == ' ')
                {
                    validRowFound = true;
                    o_Row = CurrentRow;
                    break;
                }
            }

            return validRowFound;
        }



        private void block(ref Move i_BlockPosition)
        {
            GameBoard.SetValue(i_BlockPosition.Row, i_BlockPosition.Col, i_BlockPosition.Sign);
            GameBoard.LastMove = i_BlockPosition;
        }

        private bool tryGetBlockPosition(ref Move o_BlockPosition)
        {
            bool isFound = false;
            Move blockPosition = new Move();

            if(tryToGetoPositionForLastMoveCol(ref blockPosition))
            {
                o_BlockPosition = blockPosition;
                isFound = true;
            }

            return isFound;
        }

        private bool tryToGetoPositionForLastMoveCol(ref Move io_BlockPosition)
        {
            bool isFound = false;
            int row = -1;

            Move currentPosition = GameBoard.LastMove;

            if (getNextFreeRow(currentPosition.Col, out row) && row == 0)
            {
                io_BlockPosition.Col = currentPosition.Col;
                io_BlockPosition.Row = row;
                io_BlockPosition.Sign = Sign;
                isFound = true;
            }
            
            return isFound;
        }

        private bool tryToGetoPositionForLastMoveDiagonal(ref Move io_BlockPosition)
        {
            bool isWin = false;
            bool leftDiagonal = checkLeftDiagonal(ref io_BlockPosition);
            bool rightDiagonal = checkRightDiagonal(ref io_BlockPosition);
            if (leftDiagonal == true || rightDiagonal == true)
            {
                isWin = true;
            }
            return isWin;
        }

        private bool checkRightDiagonal(ref Move io_BlockPosition)
        {
            int signCount = 1;
            bool checkRightUp = true, checkLeftDown = true, diagonalThreeInARow = false;
            bool isThereFreeRoom = false;
            Move currentPosition = GameBoard.LastMove;

            while (checkRightUp == true)
            {
                currentPosition.AddValues(1, -1);
                checkRightUp = !stopCheckForBlock(currentPosition);
                if (checkRightUp == true)
                {
                    signCount++;
                }
            }

            currentPosition = GameBoard.LastMove;
            while (checkLeftDown == true)
            {
                currentPosition.AddValues(-1, 1);
                checkLeftDown = !stopCheckForBlock(currentPosition);
                if (checkLeftDown == true)
                {
                    signCount++;
                }

                if (GameBoard.GetValue(currentPosition.Row, currentPosition.Col) == ' ')
                {
                    io_BlockPosition.Row = currentPosition.Row;
                    io_BlockPosition.Col = currentPosition.Col;
                    io_BlockPosition.Sign = Sign;
                    isThereFreeRoom = true;
                }
            }

        

            if (signCount == 3)
            {
                diagonalThreeInARow = true;
            }

            return diagonalThreeInARow && isThereFreeRoom;
        }

        private bool checkLeftDiagonal(ref Move io_BlockPosition)
        {
            int signCount = 1;
            bool checkDownright = true, checkUpRLeft = true, isThereDiagonalToBlock = false;
            bool isThereFreeRoom = false;
            Move currentPosition = GameBoard.LastMove;

            while (checkDownright == true)
            {
                currentPosition.AddValues(1, 1);
                checkDownright = !stopCheckForBlock(currentPosition);
                if (checkDownright == true)
                {
                    signCount++;
                    if (GameBoard.GetValue(currentPosition.Row, currentPosition.Col) == ' ')
                    {
                        io_BlockPosition.Row = currentPosition.Row;
                        io_BlockPosition.Col = currentPosition.Col;
                        io_BlockPosition.Sign = Sign;
                        isThereFreeRoom = true;
                    }
                }
            }

            currentPosition = GameBoard.LastMove;
            while (checkUpRLeft == true)
            {
                currentPosition.AddValues(-1, -1);
                checkUpRLeft = !stopCheckForBlock(currentPosition);
                if (checkUpRLeft == true)
                {
                    signCount++;
                    if (GameBoard.GetValue(currentPosition.Row, currentPosition.Col) == ' ')
                    {
                        io_BlockPosition.Row = currentPosition.Row;
                        io_BlockPosition.Col = currentPosition.Col;
                        io_BlockPosition.Sign = Sign;
                        isThereFreeRoom = true;
                    }
                }
            }

            

            if (signCount == 3)
            {
                isThereDiagonalToBlock = true;
            }

            return isThereDiagonalToBlock && isThereFreeRoom;

        }

        private bool stopCheckForBlock(Move i_CurrentPosition)
        {
            bool stopCheck = !isOnBoard(i_CurrentPosition);

            if (stopCheck == false)
            {
                if (GameBoard.GetValue(i_CurrentPosition.Row, i_CurrentPosition.Col) != GameBoard.LastMove.Sign)
                {
                    stopCheck = true;
                }
            }

            return stopCheck;
        }

        private bool isOnBoard(Move i_CurrentPosition)
        {
            bool onBoard = false;
            bool isValidColum = checkIfColValid(i_CurrentPosition.Col);
            bool isValidRow = checkIfRowValid(i_CurrentPosition.Row);

            if (isValidColum == true && isValidRow == true)
            {
                onBoard = true;
            }

            return onBoard;
        }

        private bool checkIfRowValid(int i_Row)
        {
            bool validRow = false;

            if (i_Row > 0 && i_Row < GameBoard.Rows)
            {
                validRow = true;
            }

            return validRow;
        }

        private bool checkIfColValid(int i_Col)
        {
            bool validCol = false;
            if (i_Col >= 0 && i_Col < GameBoard.Cols)
            {
                validCol = true;
            }

            return validCol;
        }

        private bool tryToGetPositionForLastMoveRow(Move o_BlockPosition)
        {
            bool isNotBlocked = false;
            bool isThereThreeCoins = false;
            bool isFoundBlockPosition = false;
            int coinsCount = 0;
            int currentCol = GameBoard.LastMove.Col;
            int i = 0;

            while (i <= 3 && currentCol < GameBoard.Rows)
            {
                while (currentCol >= 0 && GameBoard.GetValue(GameBoard.LastMove.Row, currentCol) !=
                    Sign)
                {
                    if(GameBoard.GetValue(GameBoard.LastMove.Row, currentCol) == GameBoard.LastMove.Sign)
                    {
                        coinsCount++;
                    }
                    
                    if (coinsCount == 3)
                    {
                        isThereThreeCoins = true;
                    }

                    currentCol--;

                    if (currentCol >= 0 && GameBoard.GetValue(GameBoard.LastMove.Row, currentCol) == ' ')
                    {
                        o_BlockPosition.Col = currentCol;
                        o_BlockPosition.Row = GameBoard.LastMove.Row;
                        o_BlockPosition.Sign = Sign;
                        isNotBlocked = true;
                    }

                    isFoundBlockPosition = isNotBlocked && isThereThreeCoins;
                }

                if(isFoundBlockPosition == true)
                {
                    break;
                }

                i++;
                isNotBlocked = false;
                currentCol = GameBoard.LastMove.Col + i;
                coinsCount = 0;

            }

            return isFoundBlockPosition;

        }
    }
}
