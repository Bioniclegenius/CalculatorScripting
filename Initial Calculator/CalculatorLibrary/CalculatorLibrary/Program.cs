﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary {
  class Program {
    static public Calculator cl;
    static void Main(string[] args) {
      cl=new Calculator();
      while(true) {//Infinitely accept expressions, for now
        Console.Out.Write("Please input an expression to evaluate: ");
        Console.Out.WriteLine(cl.evaluate(Console.ReadLine()));
      }
    }
  }
}
