namespace AdventOfCode.Solutions.Year2023.Day03;

class Solution : SolutionBase
{
    public Solution() : base(03, 2023, "") { }

    protected override string SolvePartOne()
    {
        List<string> lines = Input.Split("\n").ToList().Where(s => !string.IsNullOrEmpty(s)).ToList();

        char[,] TheGrid = new char[lines.First().Length, lines.Count()];
        for (int i = 0; i < lines.Count(); i++)
        {
            //i = y Coordinate (rows)
            TheGrid = BuildTheGrid(TheGrid, lines[i], i);
        }
        return "";
    }

    protected override string SolvePartTwo()
    {
        return "";
    }

    private char[,] BuildTheGrid(char[,] theGrid, string line, int yIdx)
    {
        //i index is the X coordinate (columns)
        //yIdx is the Y coordinate
        for (int i = 0; i < line.Length; i++)
        {
            //We don't care about storing periods (.)
            if(line[i].Equals('.') == false)
            {
                theGrid[i,yIdx] = line[i]; //assign the char value to the grid assignment
            }
        }
        return theGrid;
    }
}
