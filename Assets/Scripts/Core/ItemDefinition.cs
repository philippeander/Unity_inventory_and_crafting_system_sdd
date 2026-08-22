namespace MyGame.Core
{
    public readonly struct ItemDefinition
    {
        public string Id { get; }
        public int MaxStackSize { get; }

        public ItemDefinition(string id, int maxStackSize)
        {
            Id = id;
            MaxStackSize = maxStackSize;
        }
    }
}

