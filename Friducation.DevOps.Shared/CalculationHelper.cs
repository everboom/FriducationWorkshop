namespace Friducation.DevOps.Shared
{
    public static class CalculationHelper
    {
        public static int AddNumbers(int number0, int number1) => number0 + number1;


        public static int BuggedFunction()
        {
            throw new InvalidOperationException("Throwing an exception for no good reason.");
        }

    }
}