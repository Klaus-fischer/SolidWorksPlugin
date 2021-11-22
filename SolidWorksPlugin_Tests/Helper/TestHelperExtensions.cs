namespace SIM.SolidWorksPlugin.Tests
{
    using System.Reflection;

    public static class TestHelperExtensions
    {
        public static object? GetPrivateObject<T>(this T source, string name)
        {
            return typeof(T).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(source);
        }

        public static object? InvokePrivateMethod<T, TArg>(this T source, string name, TArg arg)
            => typeof(T)
                .GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(source, new object[] { arg });
    }
}
