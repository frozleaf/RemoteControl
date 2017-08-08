namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Runtime.CompilerServices;

    internal delegate TResult MethodCall<T, TResult>(T target, params object[] args);
}

