
using System.Collections.Generic;
using System.Net;

namespace AdventOfCode.Solutions.Year2023.Day04;

class Solution : SolutionBase
{
    List<string> lines = new List<string>();
    public Solution() : base(04, 2023, "" ) { }

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

        List<int> ogCards = new List<int>();
        cards.ForEach(x => ogCards.Add(x.CardCopiesWon));

        //Dictionary
        //  Key: Card Number
        //  Value: Count of card copies (not including originals)
        List <int> copyCards = new List<int>();

        foreach (int cardID in ogCards)
        {
            //Initialize the list X times with a count of 0 
            copyCards.Add(0);
        }

        /// Order of Operations
        ///     1. Process current card and increase/add card copy counts
        ///     2. Process card copies
        ///     3. Go to next card
        for (int i = 0; i < ogCards.Count; i++)
        {
            // 1. Process current card
            ProcessCard(i, ogCards[i], ogCards.Count, ref copyCards);

            // 2. Process EACH card copies (if any)
            for (int j = 0; j < copyCards[i]; j++)
            {
                ProcessCard(i, ogCards[i], ogCards.Count, ref copyCards);
            }
        }

        //Attempt 1: 2410 (too low)
        //Attempt 2: 8063216 - CORRECT
        return (ogCards.Count + copyCards.Sum(x => x)).ToString();
    }

    private void ProcessCard(int currentCardID, int ogCopiesWon, int ogMaxCards, ref List<int> copyCards)
    {
        int range = currentCardID + ogCopiesWon;

        // 1.1 Add card copy counts
        // Range check to make sure we dont go past the total number of cards
        // Current Position + Card Copies > Max cards = Stop processing cards at max card count
        // Current Position + Card Copies <= Max cards = Safe to process cards up to the number of card copies won
        int limit = range > ogMaxCards ? ogMaxCards : range;

        for (int j = currentCardID + 1; j <= limit; j++)
        {
            copyCards[j]++;
        }
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