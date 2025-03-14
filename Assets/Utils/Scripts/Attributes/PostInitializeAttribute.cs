namespace NTO24
{
    public class PostInitializeAttribute : InitializeAttribute
    {
        public PostInitializeAttribute(string methodName = "PostInitialize") : 
            base(methodName) { }
    }
}