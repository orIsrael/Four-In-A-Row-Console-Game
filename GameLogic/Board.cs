using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Board
    {
        public char[,] Matrix { get; set; }

        public Move LastMove { get; set; }

        public Board(int i_Rows, int i_Cols)
        {
            Rows = i_Rows;
            Cols = i_Cols;
            initializeMatrix();
        }

        private void initializeMatrix()
        {
            Matrix = new char[Rows, Cols];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Matrix[i, j] = ' '; 
                }
            }
        }

        public int Rows { get; }
        public int Cols { get; }

        public Char GetValue(int i_Row, int i_Col)
        {
            return Matrix[i_Row, i_Col];
        }

        public void SetValue(int i_Row, int i_Col, char i_Sign)
        {

            Matrix[i_Row, i_Col] = i_Sign;
            LastMove = new Move(i_Row, i_Col, i_Sign); 
        }
    }
}
