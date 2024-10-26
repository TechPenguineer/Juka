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

            Errors.Add(errorMessage + " in Source File"+this.sourceFileName);
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
