namespace JukaCompiler.Exceptions
{
    internal interface ICompilerError
    {
        internal void SourceFileName(string fileName);
        internal void AddError(string errorMessage);
        internal void AddError(string errorMessage, int line, int column);
        internal bool HasErrors();
        internal List<string> ListErrors { get; }
    }
}
