using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    class ExitGameException : Exception
    {
        public ExitGameException()
        {
        }

        public ExitGameException(char i_exitChar) : base("Exiting game.Thank you for playing")
        {
                
        }
    }
}
