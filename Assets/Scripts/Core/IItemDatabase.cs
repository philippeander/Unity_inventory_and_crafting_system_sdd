namespace MyGame.Core
{
    public interface IItemDatabase
    {
        bool TryGetItem(string itemId, out ItemDefinition item);
        ItemDefinition GetItem(string itemId);
    }
}

