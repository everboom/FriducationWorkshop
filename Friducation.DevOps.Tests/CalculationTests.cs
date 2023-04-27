using Friducation.DevOps.Shared;

namespace Friducation.DevOps.Tests
{
    [TestClass]
    public class CalculationTests
    {
        [TestMethod]
        public void TestAddNumbers()
        {
            int input0 = 100;
            int input1 = 5;

            var result = CalculationHelper.AddNumbers(input0, input1);

            Assert.AreEqual(105, result);
        }

        [TestMethod]
        public void TestBuggedFunction()
        {
            CalculationHelper.BuggedFunction();
        }
    }
}