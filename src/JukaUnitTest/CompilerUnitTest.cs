﻿using JukaCompiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.CodeAnalysis;

namespace JukaUnitTest
{
    [TestClass]
    public class CompilerUnitTest
    {

        string sourceAsString =
            @"func main() = 
                {
                    test_func();
                }";


        private string Go(string source)
        {
            Compiler compiler = new Compiler();
            var outputValue = compiler.Go(sourceAsString, false);
            if (compiler.HasErrors())
            {
                throw new Exception("Parser exceptions:\r\n" + String.Join("\r\n", compiler.ListErrors()));
            }

            return outputValue;
        }

        [TestMethod]
        public void PrintLiteral()
        {
            sourceAsString +=
                @"func test_func() = 
                {
                    print(""print""); 
                }";

            var value = Go(sourceAsString);
            Assert.AreEqual("print", value);
        }

        [TestMethod]
        public void PrintVariable()
        {
            sourceAsString +=
                @"func test_func() = 
                {
                    var x = ""print"";
                    print(x); 
                }";

            Assert.AreEqual("print", Go(sourceAsString));
        }

        [TestMethod]
        public void PassVariable()
        {
            sourceAsString +=
                @"func test_func() = 
                {
                    var x = ""print"";
                    varpass(x);
                }
                
                func varpass(var x) = 
                {
                    print(x); 
                }";

            var value = Go(sourceAsString);
            Assert.AreEqual("print", value);
        }

        [TestMethod]
        public void PrintThreeLevelsNesting()
        {
            sourceAsString +=
                @"func test_func() = 
                {
                    var y = ""one two three"";
                    nest1(y);
                }
                
                func nest1(var y) = 
                {
                    nest2(y);
                }

                func nest2(var z) = 
                {
                    print(z);
                }";

            var value = Go(sourceAsString);
            Assert.AreEqual("one two three", value);
        }

        [TestMethod]
        public void TestEmptyComment()
        {
            Compiler compiler = new Compiler();
            sourceAsString += @"func test_func() =
                {
                    var y = ""one two three"";
                    /*nest1(y);*/
                }";

            var outputValue = compiler.Go(sourceAsString, false);
            if (compiler.HasErrors())
            {
                throw new Exception("Parser exceptions:\r\n" + String.Join("\r\n", compiler.ListErrors()));
            }
            Assert.AreEqual("", outputValue);
        }

        [TestMethod]
        public void TestFunctionCall()
        {
            Compiler compiler = new Compiler();
            sourceAsString +=
                @"func test_func(var m) = 
                {
                    var u=m;
                    print(u); 
                }

                func main() = 
                {
                    test_func(3);
                }";


            var outputValue = compiler.Go(sourceAsString, false);
            if (compiler.HasErrors())
            {
                throw new Exception("Parser exceptions:\r\n" + String.Join("\r\n", compiler.ListErrors()));
            }
            Assert.AreEqual("3", outputValue);
        }


        [TestMethod]
        public void TestMultipleVariables()
        {
            sourceAsString += @"
                func test_func() = 
                {
                    var z = 3;
                    var x=""a""; 
                    print(x);
                }";

            Compiler compiler = new Compiler();
            var outputValue = new Compiler().Go(sourceAsString, false);

            if (compiler.HasErrors())
            {
                throw new Exception("Parser exceptions:\r\n" + String.Join("\r\n", compiler.ListErrors()));
            }

            Assert.AreEqual("a" , outputValue);
        }

        [TestMethod]
        public void TestOperationAdd()
        {
            Compiler compiler = new Compiler();
            string sourceAsString = "func test_add() = {var x=32; var y=33; var z=x+y;printLine(z);} test_add();";

            var outputValue = compiler.Go(sourceAsString, false);
            if (compiler.HasErrors())
            {
                throw new Exception("Parser exceptions:\r\n" + String.Join("\r\n", compiler.ListErrors()));
            }
            Assert.AreEqual("65" + Environment.NewLine, outputValue);
        }

        [TestMethod]
        public void TestOperationSubtract()
        {
            Compiler compiler = new Compiler();
            string sourceAsString = "func test_add() = {var x=32; var y=33; var z=x-y;printLine(z);} test_add();";

            var outputValue = compiler.Go(sourceAsString, false);
            if (compiler.HasErrors())
            {
                throw new Exception("Parser exceptions:\r\n" + String.Join("\r\n", compiler.ListErrors()));
            }
            Assert.AreEqual("-1" + Environment.NewLine, outputValue);
        }

        [TestMethod]
        public void TestOperationDivide()
        {
            Compiler compiler = new Compiler();
            string sourceAsString = "func test_add() = {var x=10; var y=3; var z=x/y;printLine(z);} test_add();";

            var outputValue = compiler.Go(sourceAsString, false);
            if (compiler.HasErrors())
            {
                throw new Exception("Parser exceptions:\r\n" + String.Join("\r\n", compiler.ListErrors()));
            }
            Assert.AreEqual("3" + Environment.NewLine, outputValue);
        }

        [TestMethod]
        public void TestOperationMultiply()
        {
            Compiler compiler = new Compiler();
            string sourceAsString = "func test_add() = {var x=3; var y=3; var z=x*y;printLine(z);} test_add();";

            var outputValue = compiler.Go(sourceAsString, false);
            if (compiler.HasErrors())
            {
                throw new Exception("Parser exceptions:\r\n" + String.Join("\r\n", compiler.ListErrors()));
            }
            Assert.AreEqual("9" + Environment.NewLine, outputValue);
        }


        [TestMethod]
        public void TestSourceAsFile()
        {
            Compiler compiler = new Compiler();

            var outputValue = compiler.Go(@"../../../../../examples/test.juk");
            if (compiler.HasErrors())
            {
                var errors = compiler.ListErrors();
                foreach(var error in errors)
                {
                    Assert.IsTrue(false, error);
                }
            }

            //Assert.AreEqual("AsdfA" + Environment.NewLine, outputValue);
        }

        [TestMethod]
        public void TestSourceAsFile2()
        {
            Compiler compiler = new Compiler();
            var outputValue = compiler.Go(@"../../../../../examples/test2.juk");
            if (compiler.HasErrors())
            {
                throw new Exception("Parser exceptions:\r\n" + String.Join("\r\n", compiler.ListErrors()));
            }
            Assert.AreEqual("AsdfA" + Environment.NewLine, outputValue);
        }

        [TestMethod]
        public void TestClass()
        {
            Compiler compiler = new Compiler();
            string sourceAsString = "class x = {  } ";

            var outputValue = compiler.Go(sourceAsString, false);
            if (compiler.HasErrors())
            {
                throw new Exception("Parser exceptions:\r\n" + String.Join("\r\n", compiler.ListErrors()));
            }
            //Assert.AreEqual("", outputValue);
        }

        [TestMethod]
        public void TestMain()
        {
            Compiler compiler = new Compiler();
            string sourceAsString = "func main() = { print(\"Hello World\"); }";

            var outputValue = compiler.Go(sourceAsString, false);
            if (compiler.HasErrors())
            {
                throw new Exception("Parser exceptions:\r\n" + String.Join("\r\n", compiler.ListErrors()));
            }
            //Assert.AreEqual("Hello World", outputValue);
        }
    }
}