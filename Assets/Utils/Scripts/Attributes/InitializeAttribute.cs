using System;

namespace NTO24
{
    public class InitializeAttribute : Attribute
    {
        public readonly string MethodName;

        public InitializeAttribute(string methodName)
            => MethodName = methodName;
    }
}