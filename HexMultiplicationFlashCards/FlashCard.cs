
namespace HexMultiplicationFlashCards
{
    class FlashCard
    {
        public int multiplier { get; private set; }
        public int multiplicand { get; private set; }
        public int product { get { return multiplier * multiplicand; } }
        public FlashCard(int multiplier, int multiplicand)
        {
            this.multiplier = multiplier;
            this.multiplicand = multiplicand;
        }
    }
}