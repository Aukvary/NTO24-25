namespace NTO24
{
    public class PreInitializeAttribute : InitializeAttribute
    {
        public PreInitializeAttribute(string methodName = "PreInitialize") :
            base(methodName)
        { }
    }
}