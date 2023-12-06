
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
        lines = Input.Split("\n").ToList().Where(s => !string.IsNullOrEmpty(s)).ToList();
        List<Card> cards;
        ParseCards(lines, out cards);

        Dictionary<int, int> cardsWon = new Dictionary<int, int>();
        Dictionary<int, int> cardInstances = new Dictionary<int, int>();
        cards.ForEach(x => cardsWon.Add(x.ID, x.CardCopiesWon));
        cards.ForEach(x => cardInstances.Add(x.ID, 1));

        for (int i = 1; i < cardsWon.Keys.Count - 1; i++)
        {
            int incr = cardsWon[i];
            int idx;
            //Count as many times up as the value unless it goes out of bounds
            for (int j = 0; j < incr; j++)
            {
                idx = i + 1 + j;
                if(idx <= cardsWon.Keys.Count)
                    cardInstances[idx]++;
            }
        }

        //This is not it - it doesn't cascade to account for newly added cards during processing
        return cardInstances.Values.Sum().ToString();
    }
}

class Card
{
    public int ID { get; set; }
    public List<int> WinningNumbers { get; set; }
    public List<int> GameNumbers { get; set; }
    public int Points { get { return (int)(WinningNumbers.Intersect(GameNumbers).Count() == 0 ? 0 : Math.Pow(2, WinningNumbers.Intersect(GameNumbers).Count() - 1)); } }
    public int CardCopiesWon { get { return WinningNumbers.Intersect(GameNumbers).Count(); } }

    public Card(int id, List<int> win, List<int> game)
    {
        ID = id;
        WinningNumbers = win;
        GameNumbers = game;
    }
}
