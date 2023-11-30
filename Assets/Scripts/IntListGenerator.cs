using System;
using System.Collections.Generic;
using System.Linq;

public class IntListGenerator
{
    public static List<int> GenerateIntList(int low, int high, int totalValues, int targetSum)
    {
        // Validation
        if (low > high || totalValues <= 0 || targetSum < low * totalValues || targetSum > high * totalValues)
        {
            throw new ArgumentException("Invalid parameters for generating the list.");
        }

        Random rand = new Random();
        List<int> result = new List<int>();
        int currentSum = 0;

        for (int i = 0; i < totalValues; i++)
        {
            int maxVal = Math.Min(high, targetSum - currentSum - (totalValues - i - 1) * low);
            int minVal = Math.Max(low, targetSum - currentSum - (totalValues - i - 1) * high);
            int nextVal = rand.Next(minVal, maxVal + 1);

            result.Add(nextVal);
            currentSum += nextVal;
        }

        // Shuffle the list to avoid having the larger numbers always at the start
        int n = result.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            int value = result[k];
            result[k] = result[n];
            result[n] = value;
        }

        return result;
    }
}
