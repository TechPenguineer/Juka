using Microsoft.VisualBasic;

namespace JukaCompiler.Exceptions
{
    internal class CompilerError : ICompilerError
    {
        internal List<string> Errors = [];
        internal string sourceFileName = "";

        public CompilerError()
        {
        }

        void ICompilerError.AddError(string errorMessage)
        {
            var myerrors = this.sourceFileName.Split("\r\n");
            string outme = "";
            for (int k = 0; k < myerrors.Length; k++)
            {
                outme += "[blue]" + k + ":[/] " + myerrors[k] + "\r\n";
            }

            Errors.Add(errorMessage + "\r\n\r\nSource Code:\r\n" + outme);
            System.Diagnostics.Debugger.Break();
        }
        void ICompilerError.AddError(string errorMessage,int line, int column)
        {
            string[] myerrors = this.sourceFileName.Split("\r\n");
            string outme = "";
            for (int k=0; k < myerrors.Length; k++)
            {
                if((k+1) == line)
                {
                    string firsthalf = myerrors[k][..column];
                    string secondhalf = "[red]" + myerrors[k][column..] + "[/]";
                    outme += "[red]" + (k + 1) + ":[/] " + firsthalf + secondhalf +"\r\n";
                }
                else
                {
                    outme += "[blue]" + (k + 1) + ":[/] " + myerrors[k] + "\r\n";
                }
                
            }

            Errors.Add(errorMessage + "\r\n\r\nSource Code:\r\n" + outme);
            System.Diagnostics.Debugger.Break();
        }



        bool ICompilerError.HasErrors()
        {
            return Errors.Count > 0;
        }

        List<string> ICompilerError.ListErrors => Errors;

        void ICompilerError.SourceFileName(string fileName)
        {
            sourceFileName = fileName;
        }
    }
}
