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
     * Equals (return 1 or 0 for true or false, that way it can be used with other functions if encapsulated in parentheses)
    */
    #endregion

    string expression;
    bool isParent;
    List<Expression> exprs;
    List<char> ops;
    List<Expression> tempexprs;
    List<char> tempops;

    public Expression(string exprin="",bool parent=false) {//constructor
      expression=exprin;
      isParent=parent;
      tempexprs=new List<Expression>();
      tempops=new List<char>();
    }

    public bool isOperator(char check){//checks to see if a character in a string is an operator
      if(check=='^'||check=='*'||check=='/'||check=='+'||check=='-'||check=='=')
        return true;
      return false;
    }

    public bool isLetter(char check) {
      if((check>='a'&&check<='z')||(check>='A'&&check<='Z'))
        return true;
      return false;
    }

    public bool isNumeric(char check) {
      if(check>='0'&&check<='9')
        return true;
      return false;
    }

    public bool isNumeric(string check) {
      for(int x=0;x<check.Length;x++)
        if(!isNumeric(check[x])&&check[x]!='.'&&check[x]!='-')
          return false;
      return true;
    }

    public bool isVariable(string check) {
      for(int x=0;x<check.Length;x++)
        if(!isLetter(check[x]))
          return false;
      return true;
    }

    public bool collapse() {
      if(exprs.Count()==1) {
        if(!isNumeric(exprs[0].expression)&&!isVariable(exprs[0].expression))
          exprs[0].evaluate();
        expression=exprs[0].expression;
        exprs.Clear();
        return true;
      }
      return false;
    }

    public void exponentiate(string expr) {
      if(isNumeric(expression)&&isNumeric(expr)) {
        double num1=Convert.ToDouble(expression);
        double num2=Convert.ToDouble(expr);
        num1=Math.Pow(num1,num2);
        expression=Convert.ToString(num1);
      }
      else
        expression=expression+"^"+expr;
    }

    public void multiply(Expression expr) {
      tempexprs=new List<Expression>();
      tempops=new List<char>();
      if(expr.exprs.Count()==0) {
        if(isNumeric(expression)&&isNumeric(expr.expression)) {
          double num1=Convert.ToDouble(expression);
          double num2=Convert.ToDouble(expr.expression);
          num1*=num2;
          expression=Convert.ToString(num1);
        }
        else
          expression=expression+"*"+expr.expression;
      }
      else {
        List<Expression> exprBufs=expr.exprs;
        if(exprs.Count()==0&&isNumeric(expression)) {
          for(int x=0;x<exprBufs.Count;x++)
            if(isNumeric(exprBufs[x].expression)) {
              double num1=Convert.ToDouble(expression);
              double num2=Convert.ToDouble(exprBufs[x].expression);
              num1*=num2;
              exprBufs[x].expression=Convert.ToString(num1);
            }
          for(int x=0;x<exprBufs.Count();x++)
            tempexprs.Add(exprBufs[x]);
          for(int x=0;x<expr.ops.Count();x++)
            tempops.Add(expr.ops[x]);
        }
      }
    }

    public void divide(Expression expr) {
      if(isNumeric(expression)&&isNumeric(expr.expression)) {
        double num1=Convert.ToDouble(expression);
        double num2=Convert.ToDouble(expr.expression);
        num1=num1/num2;
        expression=Convert.ToString(num1);
      }
      else
        expression=expression+"/"+expr;
    }

    public string evaluate() {//the big mess. Warning.

      #region Declarations
      //================================================================================================================================
      exprs=new List<Expression>();
      ops=new List<char>();
      string validChars="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890.()^*/+-!=";
      int numParen=0;
      string expr=expression;
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
          return "Invalid expression: invalid characters found: '"+expr[x]+"'";
      }
      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion

        #region Mismatching Parentheses and Misordered Parentheses
      //--------------------------------------------------------------------------------------------------------------------------------
      bool tooLowFlag=false;
      for(int x=0;x<expr.Length;x++){//check to see if parentheses are matched and ordered
        if(expr[x]=='(')
          numParen++;
        if(expr[x]==')')
          numParen--;
        if(numParen<0)
          tooLowFlag=true;
      }
      if(numParen!=0)
        return "Invalid expression: mismatched parentheses: "+
          (numParen>0?Convert.ToString(numParen)+" extra open":Convert.ToString(Math.Abs(numParen))+" extra close")+" parentheses.";
      if(tooLowFlag)
        return "Invalid expression: closing parenthese found before matching open.";
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

        #region Invalid character after closing parentheses
      //--------------------------------------------------------------------------------------------------------------------------------
      for(int x=0;x<expr.Length-1;x++)
        if(expr[x]==')') {
          if(!isOperator(expr[x+1])&&expr[x+1]!=')'&&expr[x+1]!='(')
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

        #region Can't infer variable operation
      //--------------------------------------------------------------------------------------------------------------------------------
      for(int x=0;x<expr.Length-1;x++) {
        if(isLetter(expr[x])&&isNumeric(expr[x+1]))
          return "Expression error: can't infer variable operation.";
      }
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
              }
              expr=expr.Substring(x+1);
              x=-1;
              startpoint=-1;
            }
          }
        }
        else if(startpoint==-1){
          if(isOperator(expr[x])) {//3+<whatever> -> (3) + <whatever>
            exprs.Add(new Expression(expr.Substring(0,x)));
            ops.Add(expr[x]);
            expr=expr.Substring(x+1);
            x=-1;
          }
          else if(x>0) {
            if(isLetter(expr[x])&&!isLetter(expr[x-1])) {//3x -> (3) * (x)
              exprs.Add(new Expression(expr.Substring(0,x)));
              ops.Add('*');
              expr=expr.Substring(x);
              x=-1;
            }
          }
        }
      }
      if(expr.Length>0)
        exprs.Add(new Expression(expr));
      expr="";
      if(collapse())
        expr=expression;
      //================================================================================================================================
      #endregion

      #region Evaluate Lowers
      //================================================================================================================================
      for(int x=0;x<exprs.Count();x++) {
        string response=exprs[x].evaluate();
        if(response.Contains("Invalid expression:"))
          return (response+" Found in: "+exprs[x].expression);
      }
      //================================================================================================================================
      #endregion

      #region Secondary Invalid Expression Checks
      //================================================================================================================================
        #region Extra Decimals Found
      //--------------------------------------------------------------------------------------------------------------------------------
      if(exprs.Count()==0&&expr!=""&&isNumeric(expr)) {//if it's as parsed as it can be
        int decCount=0;
        for(int x=0;x<expr.Length;x++)
          if(expr[x]=='.')
            decCount++;
        if(decCount>1)
          return "Invalid expression: multiple decimals in a number found.";
      }
      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion
      //================================================================================================================================
      #endregion

      #region Operations
      //================================================================================================================================
        #region Exponentiation
      //--------------------------------------------------------------------------------------------------------------------------------
      for(int x=0;x<ops.Count();x++)//Exponentiation is weird, come back later.
        if(ops[x]=='^') {
          int cur=x;
          for(int y=x+1;y<ops.Count();y++) {
            if(ops[y]=='^')
              x=y;
            else
              break;
          }
          for(int y=x;y>=cur;y--) {
            exprs[y].exponentiate(exprs[y+1].expression);
            exprs.RemoveAt(y+1);
            ops.RemoveAt(y);
            x--;
          }
        }
      //if(collapse())
      //  expr=expression;
      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion

        #region Multiplication and Division
      //--------------------------------------------------------------------------------------------------------------------------------
      for(int x=0;x<ops.Count();x++) {
        if(ops[x]=='*') {
          exprs[x].multiply(exprs[x+1]);
          exprs.RemoveAt(x+1);
          if(exprs[x].tempexprs.Count()!=0) {
            ops.InsertRange(x+1,exprs[x].tempops);
            exprs.InsertRange(x+1,exprs[x].tempexprs);
            exprs.RemoveAt(x);
            ops.RemoveAt(x);
          }
        }
        else if(ops[x]=='/') {
          exprs[x].divide(exprs[x+1]);
          exprs.RemoveAt(x+1);
          ops.RemoveAt(x);
          x--;
        }
      }
      //if(collapse())
      //  expr=expression;
      //--------------------------------------------------------------------------------------------------------------------------------
        #endregion

        #region Addition and Subtraction
      //--------------------------------------------------------------------------------------------------------------------------------

      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion

        #region Equals
      //--------------------------------------------------------------------------------------------------------------------------------

      //--------------------------------------------------------------------------------------------------------------------------------
      #endregion
      #endregion

      #region Recollapse
      //================================================================================================================================
      if(exprs.Count()==1) {
        expression=exprs[0].expression;
        expr=expression;
        exprs.Clear();
      }
      //================================================================================================================================
      #endregion

      #region Return
      //================================================================================================================================
      if(exprs.Count()>0) {
        string tempexpr=expr;//Error catching in case it's not finished off
        expr="";
        if(exprs.Count()>1&&ops.Count>0)
          for(int x=0;x<exprs.Count()-1;x++)
            expr+="("+exprs[x].evaluate()+") "+ops[x]+" ";
        expr+="("+exprs[exprs.Count()-1].evaluate()+")";

        if(tempexpr!="")
          expr+=" | Unparsed: "+tempexpr;//anything that was missed for some reason.
      }
      return expr;
      //================================================================================================================================
      #endregion

    }
  }
}
