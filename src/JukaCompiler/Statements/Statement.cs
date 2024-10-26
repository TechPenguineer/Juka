using JukaCompiler.Expressions;
using JukaCompiler.Lexer;
using JukaCompiler.Parse;

namespace JukaCompiler.Statements
{
    internal abstract class Statement
    {
        internal interface IVisitor<R>
        {
            R VisitBlockStmt(Block stmt);
            R VisitFunctionStmt(Function stmt);
            R VisitClassStmt(Class stmt);
            R VisitExpressionStmt(Expression stmt);
            R VisitIfStmt(If stmt);
            R VisitPrintLine(PrintLine stmt);
            R VisitPrint(Print stmt);
            R VisitReturnStmt(Return stmt);
            R VisitVarStmt(Var stmt);
            R VisitWhileStmt(While stmt);
            R VisitBreakStmt(Break stmt);

            R VisitForStmt(For stmt);
        }
        internal abstract R Accept<R>(Statement.IVisitor<R> vistor);
        private Lexeme stmtLexeme = new Lexeme();

        internal Lexeme StmtLexeme
        {
            get => stmtLexeme;
            set => stmtLexeme = value;
        }

        internal string StmtLexemeName
        {
            get => stmtLexeme.ToString();
        }

        internal class Block : Statement
        {
            internal Block(List<Statement> statements)
            {
                this.statements = statements;
            }

            internal List<Statement> statements;
            internal override R Accept<R>(IVisitor<R> vistor)
            {
                return vistor.VisitBlockStmt(this);
            }
        }
        internal class Function : Statement
        {
            internal List<Statement> body = new List<Statement>();
            internal List<TypeParameterMap> typeParameterMaps;

#pragma warning disable CS0659
            public override bool Equals(object? obj)
#pragma warning restore CS0659
            {
                return this.StmtLexemeName.Equals(obj);
            }
            internal Function(Lexeme stmtLexeme, List<TypeParameterMap> parametersMap, List<Statement> body)
            {
                if (!(stmtLexeme.ToString().All(c => char.IsLetterOrDigit(c) || c == '_')))
                {
                    throw new Exception("Function {ExpressionLexeme.ToString()} has an invalid ExpressionLexeme");
                }
                this.StmtLexeme = stmtLexeme;
                this.typeParameterMaps = parametersMap;
                this.body = body;
            }

            internal override R Accept<R>(IVisitor<R> vistor)
            {
                return vistor.VisitFunctionStmt(this);
            }

            internal List<TypeParameterMap> Params
            {
                get { return this.typeParameterMaps; }
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
        internal class Class : Statement
        {
            internal Lexeme name;
            internal List<Statement.Function> methods;
            internal List<Statement> variableDeclarations;
            internal Expr.Variable? superClass;

            internal Class(Lexeme name, List<Statement.Function> methods, List<Statement> variableDeclarations)
            {
                this.name = name;
                this.methods = methods;
                this.variableDeclarations = variableDeclarations;
                this.superClass = null;
            }

            internal override R Accept<R>(IVisitor<R> vistor)
            {
                return vistor.VisitClassStmt(this);
            }
        }
        internal class Expression : Statement
        {
            internal Expr Expr;

            internal Expression(Expr expr)
            {
                this.Expr = expr;
            }

            internal override R Accept<R>(IVisitor<R> vistor)
            {
                return vistor.VisitExpressionStmt(this);
            }
        }
        internal class If : Statement
        {
            internal Expr condition;
            internal Statement thenBranch;
            internal Statement elseBranch;

            internal If(Expr condition, Statement thenBranch, Statement elseBranch)
            {
                this.condition = condition;
                this.thenBranch = thenBranch;
                this.elseBranch = elseBranch;
            }

            internal override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitIfStmt(this);
            }

        }
        internal class PrintLine : Statement
        {
            internal Expr? expr;

            internal PrintLine(Expr expr)
            {
                this.expr = expr;
            }

            internal PrintLine()
            {
            }

            internal override R Accept<R>(IVisitor<R> vistor)
            {
                return vistor.VisitPrintLine(this);
            }
        }
        internal class Print : Statement
        {
            internal Expr? expr;

            internal Print(Expr expr)
            {
                this.expr = expr;
            }

            internal Print()
            {
            }

            internal override R Accept<R>(IVisitor<R> vistor)
            {
                return vistor.VisitPrint(this);
            }
        }
        internal class Var : Statement
        {
            internal Lexeme? name;
            internal Expr? exprInitializer;
            internal bool isInitalizedVar = false;

            internal Var(Lexeme name, Expr expr)
            {
                if (!(name.ToString().All(c => char.IsLetterOrDigit(c) || c == '_')))
                {
                    throw new Exception("Variable {ExpressionLexeme.ToString()} has an invalid ExpressionLexeme");
                }
                this.name = name;
                this.exprInitializer = expr;
                this.exprInitializer.initializerContextVariableName = name;
                this.isInitalizedVar = true;

            }

            internal Var(Lexeme name)
            {
                if (!(name.ToString().All(c => char.IsLetterOrDigit(c) || c == '_')))
                {
                    throw new Exception("Variable {ExpressionLexeme.ToString()} has an invalid ExpressionLexeme");
                }
                this.name = name;
                exprInitializer = null;
            }

            internal Var()
            {
            }

            internal override R Accept<R>(IVisitor<R> vistor)
            {
                return vistor.VisitVarStmt(this);
            }
        }

        internal class While : Statement
        {
            internal Expr condition;
            internal Statement whileBlock;

            internal While(Expr condition, Statement whileBlock)
            {
                this.condition = condition;
                this.whileBlock = whileBlock;
            }

            internal override R Accept<R>(IVisitor<R> vistor)
            {
                return vistor.VisitWhileStmt(this);
            }
        }

        internal class For : Statement
        {
            private Statement.Var init;
            private Expr breakExpr;
            private Expr incExpr;
            private Statement forBody;

            internal For(Statement.Var init, Expr breakExpr, Expr incExpr, Statement forBody)
            {
                this.init = init;
                this.breakExpr = breakExpr;
                this.incExpr = incExpr;
                this.forBody = forBody;
            }

            internal Statement.Var Init => init;
            internal Expr BreakExpr => breakExpr;
            internal Expr IncExpr => incExpr;
            internal Statement ForBody => forBody;

            internal override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitForStmt(this);
            }
        }
        internal class Return : Statement
        {
            internal Lexeme? keyword;
            internal Expr? expr;
            internal Return(Lexeme keyword, Expr expr)
            {
                this.keyword = keyword;
                this.expr = expr;
            }

            internal Return()
            {
            }

            internal override R Accept<R>(IVisitor<R> vistor)
            {
                return vistor.VisitReturnStmt(this);
            }
        }
        internal class Break : Statement
        {
            internal Expr expr;

            internal Break(Expr expr)
            {
                this.expr = expr;
            }

            internal override R Accept<R>(IVisitor<R> vistor)
            {
                return vistor.VisitBreakStmt(this);
            }
        }
        internal class DefaultStatement : Statement
        {
            // Does nothing used for return values;
            internal override R Accept<R>(IVisitor<R> vistor)
            {
                throw new NotImplementedException();
            }
        }

        internal class LiteralLexemeExpression : Statement
        {
            internal Expr.LexemeTypeLiteral ltl;

            internal LiteralLexemeExpression(Expr.LexemeTypeLiteral lexemeTypeLiteral)
            {
                this.ltl = lexemeTypeLiteral;
            }

            internal override R Accept<R>(IVisitor<R> vistor)
            {
                throw new NotImplementedException();
            }
        }
    }
}
