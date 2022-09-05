﻿using System.Text;
using JukaCompiler.Expressions;
using JukaCompiler.Interpreter;

namespace JukaCompiler.SystemCalls
{
    public enum CallableServices
    {
        GetAvailableMemory,
        FileOpen,
    }

    /// <summary>
    /// Need a way to implement structured exception handling
    /// All this does is catch an exception and returns an object.
    /// </summary>
    internal class SystemCallException : Exception
    {
        internal Exception internalException;
        internal SystemCallException(Exception ex)
        {
            internalException = ex;
        }
    }

    internal class JukaSystemCalls : IJukaCallable
    {
        public static readonly Dictionary<string, Type> kv = new Dictionary<string, Type>()
        {
            {"FileOpen", typeof(IFileOpen)},
        };

        public int Arity()
        {
            throw new NotImplementedException();
        }

        public object? Call(string methodName, JukaInterpreter interpreter, List<object?> arguments)
        {
            if(JukaSystemCalls.kv.TryGetValue(methodName, out var theType))
            { 
                var jukacall = (IJukaCallable)interpreter.ServiceProvider.GetService(theType);
                return jukacall.Call(methodName, interpreter, arguments);
            }

            throw new Exception("");
        }
    }

    internal class FileOpen : IFileOpen, IJukaCallable
    {
        public int Arity()
        {
            return 1;
        }

        public object? Call(string methodName, JukaInterpreter interpreter, List<object?> arguments)
        {
            try
            { 
                foreach(var argument in arguments)
                {
                    if (argument is Expression.LexemeTypeLiteral)
                    {
                        byte[] bytes = File.ReadAllBytes( ((Expression.LexemeTypeLiteral)argument).literal?.ToString() ?? string.Empty);
                        Console.Out.WriteLine(((Expression.LexemeTypeLiteral)argument));
                        return bytes;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new SystemCallException(ex);
            }

            return new byte[0];
        }
    }
}
