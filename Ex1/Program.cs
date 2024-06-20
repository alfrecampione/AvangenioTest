int Solution(int[] numbers)
{
    int small = int.MaxValue; // fix from int small = 0 because it will return 0 if all numbers are positive
    for (int i = 0; i < numbers.Length; i++) // fix from int i = 1 because it will skip the first element
    {
        if (numbers[i] < small)
        {
            small = numbers[i];
        }
    }

    return small;
}


Console.WriteLine(Solution(new int[] { -1, 1, -2, 2 }));