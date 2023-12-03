using System.Collections.Generic;
using System.Net;

namespace AdventOfCode.Solutions.Year2023.Day02;

class Solution : SolutionBase
{
    const int MAX_BLUE = 14;
    const int MAX_GREEN = 13;
    const int MAX_RED = 12;

    public Solution() : base(02, 2023, "") { }

    protected override string SolvePartOne()
    {
        List<string> lines = lines = Input.Split("\n").ToList().Where(s => !string.IsNullOrEmpty(s)).ToList();
        List<Game> games = ParseGames(lines);

        //Attempt: 2176 - CORRECT
        return games.Where(x => x.Possible).Sum(x => x.ID).ToString();
    }

    protected override string SolvePartTwo()
    {
        List<string> lines = lines = Input.Split("\n").ToList().Where(s => !string.IsNullOrEmpty(s)).ToList();
        List<Game> games = ParseGames(lines);

        return games.Sum(x => x.Power).ToString();
    }

    private List<Game> ParseGames(List<string> lines)
    {
        List<Game> games = new List<Game>();
        foreach (string item in lines)
        {
            Game current = new Game();
            string[] gameSplit = item.Split(":");
            current.ID = int.Parse(gameSplit[0].Trim().Replace("Game", ""));
            current.Hand = ParseHand(gameSplit[1].Trim());
            current.Possible = VerifyHands(current.Hand);
            current.MaxCubes = CalcMaxValid(current.Hand);
            current.Power = current.MaxCubes.red * current.MaxCubes.blue * current.MaxCubes.green;
            games.Add(current);
        }
        return games;
    }

    private List<Cubes> ParseHand(string theHand)
    {
        List<Cubes> hand = new List<Cubes>();
        string[] sets = theHand.Split(";");
        foreach (string item in sets)
        {
            string[] cubes = item.Split(",");
            Cubes colors = new Cubes();
            foreach (string kuub in cubes)
            {
                string[] cubeColorCount = kuub.Trim().Split(' ');
                switch (cubeColorCount[1])
                {
                    case "green":
                        colors.green = int.Parse(cubeColorCount[0]);
                        break;
                    case "blue":
                        colors.blue = int.Parse(cubeColorCount[0]);
                        break;
                    case "red":
                        colors.red = int.Parse(cubeColorCount[0]);
                        break;
                }
            }
            hand.Add(colors);
        }
        return hand;
    }

    private bool VerifyHands(List<Cubes> hand)
    {
        //LINQ query - not valid
        //return hand.Where(x => (x.blue > MAX_BLUE || x.red > MAX_RED || x.green > MAX_GREEN)).Count() > 0;
        bool valid = true;
        foreach (Cubes item in hand)
        {
            valid &= (item.blue <= MAX_BLUE && item.red <= MAX_RED && item.green <= MAX_GREEN);
        }
        return valid;
    }

    private Cubes CalcMaxValid(List<Cubes> hand)
    {
        int maxBlue = 0, maxRed = 0, maxGreen = 0;
        foreach (Cubes item in hand)
        {
            maxBlue = item.blue > maxBlue ? item.blue : maxBlue;
            maxRed = item.red > maxRed ? item.red : maxRed;
            maxGreen = item.green > maxGreen ? item.green : maxGreen;
        }
        return new Cubes() { green = maxGreen, blue = maxBlue, red = maxRed };
    }
}

class Game
{
    public int ID { get; set; } = 0;
    public List<Cubes> Hand { get; set; } = new List<Cubes>();
    public bool Possible { get; set; } = false;
    public Cubes MaxCubes { get; set; } = new Cubes();
    public int Power { get; set; }
}

struct Cubes
{
    public int blue;
    public int red;
    public int green;
}
