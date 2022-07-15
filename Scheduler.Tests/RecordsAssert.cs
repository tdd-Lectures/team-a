using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace Scheduler.Tests
{
    public static class RecordsAssert
    {
        public static void AreEqual<T>(T actual, T expected)
        {
            var compareLogic = new CompareLogic
            {
                Config =
                {
                    IgnoreObjectTypes = true
                }
            };
            var result = compareLogic.Compare(expected, actual);
            if (!result.AreEqual)
                throw new AssertionException(result.DifferencesString);
        }
    }
}