using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public struct Move
    {
        public Move(int i_Row, int i_Col, char i_Sign)
        {
            Row = i_Row;
            Col = i_Col;
            Sign = i_Sign;
        }

        public int Row { get; set; }

        public int Col { get; set; }

        public char Sign { get; set; }

        internal void AddValues(int i_Row, int i_Col)
        {
            Row += i_Row;
            Col += i_Col;
        }
    }
}
