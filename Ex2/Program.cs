const int x = 100;
const int n = 2;

var task1 = Task.Run(() => { Console.WriteLine("PowerSum: " + PowerSum(x, n)); });
var task2 = Task.Run(() => { Console.WriteLine("PowerSumDP: " + PowerSumDp(x, n)); });

Task.WaitAll(task1, task2);
return;

// PowerSum with Dynamic Programming for better performance
static ulong PowerSumDp(int x, int n)
{
    var dp = new ulong[x + 1];
    dp[0] = 1;

    for (var num = 1; num <= Math.Pow(x, 1.0 / n); num++)
    {
        var numToN = (ulong)Math.Pow(num, n);
        for (var i = (ulong)x; i >= numToN; i--)
        {
            dp[i] += dp[i - numToN];
        }
    }

    return dp[x];
}

static long PowerSum(long x, int n, int num = 1)
{
    var value = (long)(x - Math.Pow(num, n));
    return value switch
    {
        < 0 => 0,
        0 => 1,
        _ => PowerSum(value, n, num + 1) + PowerSum(x, n, num + 1)
    };
}