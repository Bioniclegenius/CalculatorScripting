using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary {
  public class Expression {

    #region OoO
    /*
     * ORDER OF OPERATIONS TO FOLLOW
     * -----------------------------
     * 
     * Parentheses
     * Functions (including factorial, represented with '!')
     * Exponentiation
     * Multiplication and Division
     * Addition and Subtraction
    */
    #endregion

    string expr;
    bool isParent;

    public Expression(string exprin="",bool parent=false) {//constructor
      expr=exprin;
      isParent=parent;
    }

    public bool isOperator(char check){//checks to see if a character in a string is an operator
      if(check=='^'||check=='*'||check=='/'||check=='+'||check=='-'||check=='=')
        return true;
      return false;
    }

    public string evaluate() {//the big mess. Warning.

      #region Declarations
      //================================================================================================================================
      List<Expression> exprs=new List<Expression>();
      List<char> ops=new List<char>();
      string validChars="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890()^*/+-!=";
      int numParen=0;
      //================================================================================================================================
      #endregion

      #region Invalid Expression Checks
      //================================================================================================================================
      #region Invalid Characters Found
      //--------------------------------------------------------------------------------------------------------------------------------
      for(int x=0;x<expr.Length;x++) {
        bool found=false;
        for(int y=0;y<validChars.Length;y++)
          if(validChars[y]==expr[x])
            found=true;
        if(!found)
          return "Invalid expression: invalid characters found.";
      }
      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion

      #region Mismatching Parentheses and Misordered Parentheses
      //--------------------------------------------------------------------------------------------------------------------------------
      for(int x=0;x<expr.Length;x++){//check to see if parentheses are matched and ordered
        if(expr[x]=='(')
          numParen++;
        if(expr[x]==')')
          numParen--;
        if(numParen<0)
          return "Invalid expression: closing parenthese found before matching open.";
      }
      if(numParen!=0)
        return "Invalid expression: mismatched parentheses.";
      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion

      #region No Double Operators
      //--------------------------------------------------------------------------------------------------------------------------------
      for(int x=0;x<expr.Length-1;x++)
        if(isOperator(expr[x]))
          if(isOperator(expr[x+1]))
            return "Invalid expression: double operator found.";
      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion
      
      #region No Closing Parentheses after Operator
      //--------------------------------------------------------------------------------------------------------------------------------
      for(int x=0;x<expr.Length-1;x++)
        if(isOperator(expr[x]))
          if(expr[x+1]==')')
            return "Invalid expression: invalid character after operator found.";
      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion

      #region Operator or Secondary Closing Parentheses after Closing Parentheses
      //--------------------------------------------------------------------------------------------------------------------------------
      for(int x=0;x<expr.Length-1;x++)
        if(expr[x]==')') {
          if(!isOperator(expr[x+1])&&expr[x+1]!=')')
            return "Invalid expression: invalid character found after a closing parenthese.";
        }
      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion

      #region Expression Starts with Operator
      //--------------------------------------------------------------------------------------------------------------------------------
      if(isOperator(expr[0]))
        return "Invalid expression: expression starts with operator.";
      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion

      #region Expression Ends with Operator or Open Parenthese
      //--------------------------------------------------------------------------------------------------------------------------------
      if(expr[expr.Length-1]=='('||isOperator(expr[expr.Length-1]))
        return "Invalid expression: invalid final character found.";
      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion

      #region Blank Expression
      //--------------------------------------------------------------------------------------------------------------------------------
      if(expr=="")
        return "Invalid expression: expression is blank.";
      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion
      //================================================================================================================================
      #endregion

      #region Break Expression into SubExpressions
      //================================================================================================================================
      int startpoint=-1;
      for(int x=0;x<expr.Length;x++) {//initial scan and breaking apart of equation
        if(expr[x]=='(') {
          numParen++;
          if(startpoint==-1)
            startpoint=x;//start counting here
        }
        else if(expr[x]==')') {
          numParen--;
          if(numParen==0){
            if(startpoint!=-1) {
              if(startpoint!=0) {
                int offset=0;
                if(isOperator(expr[startpoint-1])) {
                  offset=1;
                  ops.Add(expr[startpoint-1]);
                }
                else
                  ops.Add('*');//infer multiplication if stuff runs together
                exprs.Add(new Expression(expr.Substring(0,startpoint-offset)));
              }
              exprs.Add(new Expression(expr.Substring(startpoint+1,x-startpoint-1)));
              if(x<expr.Length-1) {
                if(isOperator(expr[x+1])) {
                  ops.Add(expr[x+1]);
                  x++;
                }
                expr=expr.Substring(x+1);
              }
              x=-1;
              startpoint=-1;
            }
          }
        }
        else if(isOperator(expr[x])&&startpoint==-1){
          exprs.Add(new Expression(expr.Substring(0,x)));
          ops.Add(expr[x]);
          expr=expr.Substring(x+1);
          x=-1;
        }
      }
      exprs.Add(new Expression(expr));
      expr="";
      //================================================================================================================================
      #endregion

      #region Return
      //================================================================================================================================
      if(isParent) {
        string tempexpr=expr;
        expr="";

        for(int x=0;x<exprs.Count()-1;x++)
          expr+="("+exprs[x].expr+") "+ops[x]+" ";

        expr+="("+exprs[exprs.Count()-1].expr+")";

        if(tempexpr!="")
          expr+=" | Unparsed: "+tempexpr;
      }
      return expr;
      //================================================================================================================================
      #endregion

    }
  }
}
