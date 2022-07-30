﻿using JukaCompiler.Exceptions;
using JukaCompiler.Statements;
using System.Reflection;

namespace JukaCompiler.Interpreter
{
    internal class JukaFunction : IJukaCallable
    {
        private Stmt.Function? declaration;
        private JukaEnvironment? closure;
        private bool isInitializer;

        internal JukaFunction(Stmt.Function declaration, JukaEnvironment closure, bool isInitializer)
        {
            this.isInitializer = isInitializer;
            this.declaration = declaration;
            this.closure = closure;
        }

        internal JukaFunction Bind(JukaInstance instance)
        {
            JukaEnvironment env = new(closure);
            env.Define("this", instance);

            return new JukaFunction(declaration, env, isInitializer);
        }

        public int Arity()
        {
            if(declaration == null)
            {
                return 0;
            }

            return declaration.Params.Count;
        }

        public object Call(JukaInterpreter interpreter, List<object> arguments)
        {
            JukaEnvironment environment = new(closure);

            for (int i = 0; i < declaration.Params.Count; i++)
            {
                string? name = declaration.Params[i].parameterName.ToString();
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("unable to call function");
                }
                object value = arguments[i];
                environment.Define(name, value);
            }

            try
            {
                interpreter.ExecuteBlock(declaration.body, environment);
            }
            catch(Return ex)
            {
                if (isInitializer)
                {
                    return closure.GetAt(0, "this");
                }

                return ex.value;
            }

            if (isInitializer)
            {
                return closure.GetAt(0, "this");
            }

            return null;
        }
    }
}
