using System;
using System.Collections.Generic;
using System.Text;

namespace RobotArm.Entidades
{
    public class Angulo
    {
        public string Motor { get; set; }
        public int Valor { get; set; }
        public string Tipo { get; set; }
        public override string ToString()
        {
           return Motor + "=" + Valor.ToString().PadLeft(3);
        }
    }
}
