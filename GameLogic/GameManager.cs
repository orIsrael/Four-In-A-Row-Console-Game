using System;

namespace GameLogic
{
    public class GameManager
    {
        private readonly char r_Player1Sign;
        private readonly char r_Player2Sign;

        public const int k_MinDimension = 4;
        public const int k_MaxDimension = 8;

        public eGameType GameType { get; }

        public Board GameBoard { get; set; }

        public Computer ComputerPlayer { get; set; }

        public int Player1Points { get; set; }

        public int Player2Points { get; set; }

        public eTurn CurrentTurn { get; set; }



        public GameManager(eGameType i_GameType,
            char i_Player1Sign, char i_Player2Sign,
            int i_Rows, int i_Cols)
        {
            r_Player1Sign = i_Player1Sign;
            r_Player2Sign = i_Player2Sign;
            GameBoard = new Board(i_Rows, i_Cols);
            GameType = i_GameType;

            if (i_GameType == eGameType.HumanVSComputer)
            {
                ComputerPlayer = new Computer(i_Player2Sign, GameBoard);
            }

            CurrentTurn = eTurn.Player1;
        }

        public bool CheckIfColValid(int i_Col)
        {
            bool validCol = false;
            if (i_Col >= 0 && i_Col < GameBoard.Cols)
            {
                validCol = true;
            }

            return validCol;
        }

        public bool GetNextFreeRow(int i_Col, out int o_Row)
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

        public void SetMove(int i_Row, int i_Col)
        {
            if (CurrentTurn == eTurn.Player1)
            {
                GameBoard.SetValue(i_Row, i_Col, r_Player1Sign);
                GameBoard.LastMove = new Move(i_Row, i_Col, r_Player1Sign);
            }

            else
            {
                GameBoard.SetValue(i_Row, i_Col, r_Player2Sign);
                GameBoard.LastMove = new Move(i_Row, i_Col, r_Player2Sign);
            }
        }

        public void ChangeTurn()
        {
            if(CurrentTurn == eTurn.Player1)
            {
                CurrentTurn = eTurn.Player2;
            }
            else
            {
                CurrentTurn = eTurn.Player1;
            }
        }

        public void ClearGame()
        {
            clearBaoard();
            CurrentTurn = eTurn.Player1;
        }

        private void clearBaoard()
        {
            for (int i = 0; i < GameBoard.Rows; i++)
            {
                for (int j = 0; j < GameBoard.Cols; j++)
                {
                    GameBoard.SetValue(i, j, ' ');
                }
            }
            
        }

        public bool CheckIfDraw()
        {
            bool isDraw = true;
            for (int i = 0; i < GameBoard.Cols; i++)
            {
                if(GameBoard.GetValue(0, i) == ' ')
                {
                    isDraw = false;
                }
            }

            return isDraw;
        }

        public bool CheckIfWon()
        {
            bool isWon = false;
            if (CheckRow() == true)
            {
                isWon = true;
            }
            if (checkDiagonals() == true)
            {
                isWon = true;
            }
            else if (checkColum() == true)
            {
                isWon = true;
            }

            return isWon;
        }

        private bool checkColum()
        {
            bool stopCheckingDown = false;
            bool isThereFourCoins = false;
            int coinsCount = 1;
            Move currentPosition = GameBoard.LastMove;

            while (stopCheckingDown != true)
            {
                currentPosition.AddValues(1, 0);
                if (stopCheckForAFour(currentPosition) != true)
                {
                    coinsCount++;
                }
                else
                {
                    stopCheckingDown = true;
                }

            }
            if (coinsCount >= 4)
            {
                isThereFourCoins = true;
            }

            return isThereFourCoins;
        }

        private bool checkDiagonals()
        {
            bool isWin = false;
            bool leftDiagonal = checkLeftDiagonal();
            bool rightDiagonal = checkRightDiagonal();
            if (leftDiagonal == true || rightDiagonal == true)
            {
                isWin = true;
            }
            return isWin;
        }

        private bool checkRightDiagonal()
        {

            int signCount = 1;
            bool checkRightUp = true, checkLeftDown = true, diagonalFourInARow = false;
            Move currentPosition = GameBoard.LastMove;

            while (checkRightUp == true)
            {
                currentPosition.AddValues(1, -1);
                checkRightUp = !stopCheckForAFour(currentPosition);
                if (checkRightUp == true)
                {
                    signCount++;
                }
            }

            currentPosition = GameBoard.LastMove;
            while (checkLeftDown == true)
            {
                currentPosition.AddValues(-1, 1);
                checkLeftDown = !stopCheckForAFour(currentPosition);
                if (checkLeftDown == true)
                {
                    signCount++;
                }
            }

            if (signCount >= 4)
            {
                diagonalFourInARow = true;
            }

            return diagonalFourInARow;
        }

        private bool checkLeftDiagonal()
        {
            int signCount = 1;
            bool checkRightDown = true, checkUpRLeft = true, diagonalFourInARow = false;
            Move currentPosition = GameBoard.LastMove;

            while (checkRightDown == true)
            {
                currentPosition.AddValues(1, 1);
                checkRightDown = !stopCheckForAFour(currentPosition);
                if (checkRightDown == true)
                {
                    signCount++;
                }
            }

            currentPosition = GameBoard.LastMove;
            while (checkUpRLeft == true)
            {
                currentPosition.AddValues(-1, -1);
                checkUpRLeft = !stopCheckForAFour(currentPosition);
                if (checkUpRLeft == true)
                {
                    signCount++;
                }
            }

            if (signCount >= 4)
            {
                diagonalFourInARow = true;
            }

            return diagonalFourInARow;
        }

        private bool stopCheckForAFour(Move i_CurrentPosition)
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

        private bool isOnBoard(Move i_MatrixPosition)
        {

                bool onBoard = false;
                bool isValidColum = CheckIfColValid(i_MatrixPosition.Col);
                bool isValidRow = checkIfRowValid(i_MatrixPosition.Row);

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

        private bool CheckRow()
        {
            bool isThereFourCoins = false;
            int coinsCount = 0;
            int currentCol = GameBoard.LastMove.Col;
            int i = 0;

            while (i <= 3 && currentCol < GameBoard.Rows)
            {
                while (currentCol >= 0 && GameBoard.GetValue(GameBoard.LastMove.Row, currentCol) == GameBoard.LastMove.Sign)
                {
                    coinsCount++;
                    currentCol--;
                    if (coinsCount == 4)
                    {
                        isThereFourCoins = true;
                    }
                }

                i++;
                currentCol = GameBoard.LastMove.Col + i;
                coinsCount = 0;

            }

            return isThereFourCoins;

        }


    }
}

