using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary {
  public class Calculator {
    public string evaluate(string equ) {
      if(equ.Length==0)
        return "";
      bool isnum=(equ[0]>='0'&&equ[0]<='9');
      for(int x=1;x<equ.Length;x++) {
        if((equ[x]>='0'&&equ[x]<='9')!=isnum) {
          isnum=!isnum;
          equ=equ.Substring(0,x)+" "+equ.Substring(x);
          x++;
        }
        if(equ[x]=='+'||equ[x]=='-'||equ[x]=='*'||equ[x]=='/'||equ[x]=='^'||equ[x]=='('||equ[x]==')'||equ[x]=='+') {
          equ=equ.Substring(0,x)+" "+equ.Substring(x);
          x++;
        }
      }
      return equ;
    }
  }
}
