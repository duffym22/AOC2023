using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions.Year2023.Day01;

class Solution : SolutionBase
{
    public Solution() : base(01, 2023, "") { }

    protected override string SolvePartOne()
    {
        List<string> lines = Input.Split("\n").ToList().Where(s => !string.IsNullOrEmpty(s)).ToList();
        List<CalVal> calVals = new List<CalVal>();

        try
        {
            foreach (string line in lines)
            {
                calVals.Add(Parse(line, false));
            }

            int sum = 0;
            foreach (CalVal item in calVals)
            {
                sum += item.Value;
            }

            //Attempt: 56048 - too high
            //Attempt: 55971 - CORRECT
            return sum.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.InnerException);
            Console.WriteLine(ex.StackTrace);
            return "";
        }
    }

    protected override string SolvePartTwo()
    {
        List<string> lines = Input.Split("\n").ToList().Where(s => !string.IsNullOrEmpty(s)).ToList();
        List<CalVal> calVals = new List<CalVal>();

        try
        {
            foreach (string line in lines)
            {
                calVals.Add(Parse(line, true));
            }

            int sum = 0;
            foreach (CalVal item in calVals)
            {
                sum += item.Value;
            }

            //Attempt: 54303 - Too Low
            //Attempt: 54395 - Too Low
            //Attempt: 54764 - Too High
            //Attempt: 54633
            //Attempt: 54699
            return sum.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.InnerException);
            Console.WriteLine(ex.StackTrace);
            return "";
        }
    }

    private string Refactor(string input)
    {
        //oneight (1ight)
        //eightwo (8wo)
        return input.ToLower().Replace("oneight", "1ight").Replace("eightwo", "8wo").Replace("twone", "2").Replace("one", "1").Replace("two", "2").Replace("three", "3").Replace("four", "4").Replace("five", "5").Replace("six", "6").Replace("seven", "7").Replace("eight", "8").Replace("nine", "9");
    }

    private CalVal Parse(string line, bool refactor)
    {
        char
            first = char.MinValue,
            second = char.MinValue;


        string newLine = refactor ? Refactor(line) : line;
        foreach (char c in newLine)
        {
            if (char.IsDigit(c))
            {
                first = c;
                break;
            }
        }

        string revLine = refactor ? Refactor(line).Reverse() : line.Reverse();
        foreach (char c in revLine)
        {
            if (char.IsDigit(c))
            {
                second = c;
                break;
            }
        }

        //Dont add a value if it wasn't assigned
        if (first.Equals(char.MinValue) || second.Equals(char.MinValue))
        {
            Console.WriteLine($"No number found on line [{line}] ");
        }
        return new CalVal(first, second);
    }
}

public class CalVal
{
    public char First { get; set; }
    public char Second { get; set; }
    public int Value { get { return int.Parse(string.Concat(First, Second)); } }

    public CalVal(char first, char second)
    {
        First = first;
        Second = second;
    }
}
