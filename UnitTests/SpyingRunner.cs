using System;
using FsCheck;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;

namespace UnitTests
{
    internal class SpyingRunner : IRunner
    {
        private readonly IRunner _runner;

        public TestResult TestResult { get; private set; }

        public SpyingRunner(IRunner runner)
        {
            _runner = runner;
        }

        public void OnStartFixture(Type obj0)
        {
            _runner.OnStartFixture(obj0);
        }

        public void OnArguments(int obj0, FSharpList<object> obj1, FSharpFunc<int, FSharpFunc<FSharpList<object>, string>> obj2)
        {
            _runner.OnArguments(obj0, obj1, obj2);
        }

        public void OnShrink(FSharpList<object> obj0, FSharpFunc<FSharpList<object>, string> obj1)
        {
            _runner.OnShrink(obj0, obj1);
        }

        public void OnFinished(string obj0, TestResult obj1)
        {
            TestResult = obj1;
            _runner.OnFinished(obj0, obj1);
        }
    }
}
