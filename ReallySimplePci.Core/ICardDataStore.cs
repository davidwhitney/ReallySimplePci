namespace ReallySimplePci.Core
{
    public interface ICardDataStore
    {
        CardData Get(int id);
        CardData Insert(CardData data);
    }
}
