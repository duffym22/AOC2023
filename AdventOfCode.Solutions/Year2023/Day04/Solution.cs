
using System.Net;

namespace AdventOfCode.Solutions.Year2023.Day04;

class Solution : SolutionBase
{
    List<string> lines = new List<string>();
    public Solution() : base(04, 2023, "", true) { }

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

        Dictionary<int, int> ogCards = new Dictionary<int, int>();
        cards.ForEach(x => ogCards.Add(x.ID, x.CardCopiesWon));

        //Dictionary
        //  Key: Card Number
        //  Value: Count of card copies (not including originals)
        Dictionary<int, int> copyCards = new Dictionary<int, int>();

        foreach (int key in ogCards.Keys)
        {
            //Initialize the dictionary with a count of 0 
            copyCards.Add(key, 0);
        }

        /// Order of Operations
        ///     1. Process current card and increase/add card copy counts
        ///     2. Process card copies
        ///     3. Go to next card
        for (int i = ogCards.Keys.First(); i <= ogCards.Keys.Count; i++)
        {
            // 1. Process current card
            ProcessCard(i, ref ogCards, ref copyCards);

            // 2. Process EACH card copies (if any)
            for (int j = 0; j < copyCards[i]; j++)
            {
                ProcessCard(i, ref ogCards, ref copyCards);
            }
        }

        //Attempt 1: 2410 (too low)
        return (ogCards.Sum(x => x.Value) + copyCards.Sum(x => x.Value)).ToString();
    }

    //Returns 
    private void ProcessCard(int currentCardID, ref Dictionary<int, int> cardsWon, ref Dictionary<int, int> cardCopies)
    {
        int copiesWon = cardsWon[currentCardID];
        int range = currentCardID + copiesWon;

        // 1.1 Add card copy counts
        // Range check to make sure we dont go past the total number of cards
        // Current Position + Card Copes <= Max cards = Safe to process cards up to the number of card copies won
        // Current Position + Card Copes > Max cards = Stop processing cards at max card count
        int limit = range <= cardsWon.Keys.Count ? range : cardsWon.Keys.Count;

        for (int j = currentCardID + 1; j <= limit; j++)
        {
            cardCopies[j]++;
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


//for (int i = 1; i < cardsWon.Keys.Count - 1; i++)
//{
//    int incr = cardsWon[i];
//    int idx;
//    //Count as many times up as the value unless it goes out of bounds
//    for (int j = 0; j < incr; j++)
//    {
//        idx = i + 1 + j;
//        if (idx <= cardsWon.Keys.Count)
//            cardInstances[idx]++;
//    }
//}
