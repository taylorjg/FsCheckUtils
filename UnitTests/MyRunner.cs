using System;
using FsCheck;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;

namespace UnitTests
{
    internal class MyRunner : IRunner
    {
        public void OnStartFixture(Type obj0)
        {
        }

        public void OnArguments(int obj0, FSharpList<object> obj1, FSharpFunc<int, FSharpFunc<FSharpList<object>, string>> obj2)
        {
        }

        public void OnShrink(FSharpList<object> obj0, FSharpFunc<FSharpList<object>, string> obj1)
        {
        }

        public void OnFinished(string obj0, TestResult obj1)
        {
        }
    }
}
