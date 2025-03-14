using System;

namespace NTO24.Net
{
    public class LostUserException : Exception
    {
        private string _userName;

        public override string Message => $"user({_userName}) didn't find";

        public LostUserException(string userName)
            => _userName = userName;
    }
}