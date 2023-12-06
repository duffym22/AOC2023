
using System.Net;

namespace AdventOfCode.Solutions.Year2023.Day04;

class Solution : SolutionBase
{
    List<string> lines = new List<string>();
    public Solution() : base(04, 2023, "") { }

    protected override string SolvePartOne()
    {
        lines = Input.Split("\n").ToList().Where(s => !string.IsNullOrEmpty(s)).ToList();
        List<Card> cards;
        ParseCards(lines, out cards);

        //Attempt: 18619 - CORRECT
        return cards.Select(x => x.Points).Sum().ToString();
    }

    private void ParseCards(List<string> lines, out List<Card> cards)
    {
        cards = new List<Card>();
        foreach (string line in lines) 
        {
            //Split on | first to get left and right halves
            string[] firstSp = line.Split('|');

            //Split left half again to get Card ID and winning numbers
            string[] secondSp = firstSp.First().Trim().Split(":");

            int id = int.Parse(secondSp.First().Trim().Replace("Card", ""));

            //Left half - split on the spaces to get the winning numbers
            List<int> winNums = secondSp.Last().Trim().Replace("  ", " ").Split(" ").ToList().ConvertAll(int.Parse);

            //Right half - split on the spaces to get the game numbers
            List<int> gameNums = firstSp.Last().Trim().Replace("  ", " ").Split(" ").ToList().ConvertAll(int.Parse);
            cards.Add(new Card(id, winNums, gameNums));
        }
    }

    protected override string SolvePartTwo()
    {
        return "";
    }
}

class Card
{
    public int ID { get; set; }
    public List<int> WinningNumbers { get; set; }
    public List<int> GameNumbers { get; set; }
    public int Points { get { return (int)(WinningNumbers.Intersect(GameNumbers).Count() == 0 ? 0 : Math.Pow(2, WinningNumbers.Intersect(GameNumbers).Count() - 1)); } }

    public Card(int id, List<int> win, List<int> game)
    {
        ID = id;
        WinningNumbers = win;
        GameNumbers = game;
    }
}
