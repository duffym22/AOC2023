
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2023.Day03;

class Solution : SolutionBase
{

    List<string> lines = new List<string>();
    List<char> validSymbols = new List<char>() { '#', '$', '%', '&', '*', '+', '-', '/', '=', '@' };
    public Solution() : base(03, 2023, "") { }

    protected override string SolvePartOne()
    {
        lines = Input.Split("\n").ToList().Where(s => !string.IsNullOrEmpty(s)).ToList();
        List<Numbers> numbers = new List<Numbers>();  //1219 numbers
        List<Symbols> symbols = new List<Symbols>();  //760 symbols
        try
        {
            AnalyzeTheGrid(out numbers, out symbols);
            DeterminePartNumbers(lines.First().Length, lines.Count(), numbers, symbols);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        //Attempt: 521515 - CORRECT
        return numbers.Where(x => x.IsPartNumber).Select(y => y.Number).ToList().Sum().ToString();
    }

    protected override string SolvePartTwo()
    {
        lines = Input.Split("\n").ToList().Where(s => !string.IsNullOrEmpty(s)).ToList();
        List<Numbers> numbers = new List<Numbers>();  //1219 numbers
        List<Symbols> symbols = new List<Symbols>();  //760 symbols
        AnalyzeTheGrid(out numbers, out symbols);
        DeterminePartNumbers(lines.First().Length, lines.Count(), numbers, symbols);

        //Attempt: 69527306 - CORRECT
        return symbols.Where(x => x.AdjacentPN.Count == 2).Select(y => y.AdjacentPN.FirstOrDefault() * y.AdjacentPN.LastOrDefault()).ToList().Sum().ToString();
    }

    private void DeterminePartNumbers(int maxLineLength, int maxLines, List<Numbers> numbers, List<Symbols> symbols)
    {
        //Up-Left       Up[1]   -   Up[n]       Up-Right
        //Left          X[1]    -   X[n]        Right
        //Down-Left     Down[1] -   Down[n]     Down-Right
        List<Point> symPts = symbols.Select(x => x.Point).ToList();
        foreach (Numbers item in numbers)
        {
            List<Point> checkPoints = new List<Point>();
            if (item.Point.Y == 0)
            {
                //we're on row 0 - don't look up
                checkPoints.Add(new(item.Point.X - 1, item.Point.Y));                           //left
                checkPoints.Add(new(item.Point.X + item.NumString.Length, item.Point.Y));       //right

                checkPoints.Add(new(item.Point.X - 1, item.Point.Y + 1));                       //downleft
                checkPoints.Add(new(item.Point.X + item.NumString.Length, item.Point.Y + 1));   //downright

                //All the "down" points based on the length of the number
                for (int i = 0; i < item.NumString.Length; i++)
                {
                    checkPoints.Add(new(item.Point.X + i, item.Point.Y + 1));
                }
            }
            else if (item.Point.Y == maxLines)
            {
                //we're on the last row - don't look down
                checkPoints.Add(new(item.Point.X - 1, item.Point.Y));                           //left
                checkPoints.Add(new(item.Point.X + item.NumString.Length, item.Point.Y));       //right

                checkPoints.Add(new(item.Point.X - 1, item.Point.Y - 1));                       //upleft
                checkPoints.Add(new(item.Point.X + item.NumString.Length, item.Point.Y - 1));   //upright

                //All the "up" points based on the length of the number
                for (int i = 0; i < item.NumString.Length; i++)
                {
                    checkPoints.Add(new(item.Point.X + i, item.Point.Y - 1));
                }
            }
            else
            {
                //mind the edges - don't try to make points out of bounds
                if (item.Point.X == 0)
                {
                    //don't look left
                    checkPoints.Add(new(item.Point.X + item.NumString.Length, item.Point.Y - 1));   //upright
                    checkPoints.Add(new(item.Point.X + item.NumString.Length, item.Point.Y));       //right
                    checkPoints.Add(new(item.Point.X + item.NumString.Length, item.Point.Y + 1));   //downright

                    //All the "up" points based on the length of the number
                    for (int i = 0; i < item.NumString.Length; i++)
                    {
                        checkPoints.Add(new(item.Point.X + i, item.Point.Y - 1));
                    }

                    //All the "down" points based on the length of the number
                    for (int i = 0; i < item.NumString.Length; i++)
                    {
                        checkPoints.Add(new(item.Point.X + i, item.Point.Y + 1));
                    }

                }
                else if (item.Point.X == maxLineLength)
                {
                    //don't look right
                    checkPoints.Add(new(item.Point.X - 1, item.Point.Y - 1));   //upleft
                    checkPoints.Add(new(item.Point.X - 1, item.Point.Y));       //left
                    checkPoints.Add(new(item.Point.X - 1, item.Point.Y + 1));   //downleft

                    //All the "up" points based on the length of the number
                    for (int i = 0; i < item.NumString.Length; i++)
                    {
                        checkPoints.Add(new(item.Point.X + i, item.Point.Y - 1));
                    }

                    //All the "down" points based on the length of the number
                    for (int i = 0; i < item.NumString.Length; i++)
                    {
                        checkPoints.Add(new(item.Point.X + i, item.Point.Y + 1));
                    }
                }
                else
                {
                    //Middle of the board - go nuts - add ALLLL the points
                    checkPoints.Add(new(item.Point.X - 1, item.Point.Y - 1));                       //upleft
                    checkPoints.Add(new(item.Point.X - 1, item.Point.Y));                           //left
                    checkPoints.Add(new(item.Point.X - 1, item.Point.Y + 1));                       //downleft

                    checkPoints.Add(new(item.Point.X + item.NumString.Length, item.Point.Y - 1));   //upright
                    checkPoints.Add(new(item.Point.X + item.NumString.Length, item.Point.Y));       //right
                    checkPoints.Add(new(item.Point.X + item.NumString.Length, item.Point.Y + 1));   //downright

                    //All the "up" points based on the length of the number
                    for (int i = 0; i < item.NumString.Length; i++)
                    {
                        checkPoints.Add(new(item.Point.X + i, item.Point.Y - 1));
                    }

                    //All the "down" points based on the length of the number
                    for (int i = 0; i < item.NumString.Length; i++)
                    {
                        checkPoints.Add(new(item.Point.X + i, item.Point.Y + 1));
                    }
                }
            }

            //After gathering all the points around the number
            //check to see if any of those points match a known symbol location
            foreach (Point symCheck in checkPoints)
            {
                if (symPts.Contains(symCheck))
                {
                    item.IsPartNumber = true;

                    //Since symCheck is a known symbol location
                    //check to see if it is a gear - if so, increment the adjacent PN count
                    foreach (Symbols s in symbols)
                    {
                        if (s.Point.Equals(symCheck) && s.IsGear)
                            s.AdjacentPN.Add(item.Number);
                    }
                    break;
                }
            }
        }
    }

    private void AnalyzeTheGrid(out List<Numbers> numbers, out List<Symbols> symbols)
    {
        numbers = new List<Numbers>();
        symbols = new List<Symbols>();
        for (int i = 0; i < lines.Count(); i++)
        {
            //i index is the Y coordinate (rows)
            char[] lineChars = lines[i].ToCharArray();
            for (int j = 0; j < lineChars.Length; j++)
            {
                //j index is the X coordinate (columns)
                if (char.IsDigit(lineChars[j]))
                {
                    //Save the coordinate point where the number was first detected
                    Point p = new(j, i);

                    //If we detect a digit, read in the next chars
                    //until we get to the first non-digit char
                    //or until we get to the end of the row
                    string buildNum = string.Empty;
                    do
                    {
                        buildNum += lineChars[j++];
                    } while (j < lineChars.Length && char.IsDigit(lineChars[j]));
                    numbers.Add(new Numbers(p, int.Parse(buildNum), buildNum));

                    if (j >= lineChars.Length)
                        continue;
                }

                //Don't do else-if in case digit detection incremented to a symbol
                if (validSymbols.Contains(lineChars[j]))
                {
                    //Symbols: #$%&*+-/=@
                    //Symbols are only 1 character long
                    symbols.Add(new Symbols(new(j, i), lineChars[j]));
                }

                if (lineChars[j].Equals('.'))
                {
                    //We don't care about storing periods (.)
                    continue;
                }
            }
        }
    }
}

class Numbers
{
    public int Number { get; set; } = -1;
    public Point Point { get; set; }
    public string NumString { get; set; } = "";
    public bool IsPartNumber { get; set; } = false;
    public Numbers(Point p, int num, string str)
    {
        Number = num;
        Point = p;
        NumString = str;
    }
}

class Symbols
{
    public char Symbol { get; set; }
    public Point Point { get; set; }
    public bool IsGear { get { return Symbol.Equals('*'); } }
    public List<int> AdjacentPN { get; set; } = new();
    public Symbols(Point p, char symbol)
    {
        Point = p;
        Symbol = symbol;
    }
}
