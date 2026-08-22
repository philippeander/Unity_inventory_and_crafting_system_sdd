namespace MyGame.Core
{
    public readonly struct InventoryOperationResult
    {
        public bool IsSuccess { get; }
        public int RejectedAmount { get; }
        public string ErrorMessage { get; }

        public InventoryOperationResult(bool isSuccess, int rejectedAmount = 0, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            RejectedAmount = rejectedAmount;
            ErrorMessage = errorMessage;
        }

        public static InventoryOperationResult Success() => new InventoryOperationResult(true, 0, null);

        public static InventoryOperationResult Failure(int rejectedAmount, string errorMessage) => new InventoryOperationResult(false, rejectedAmount, errorMessage);
    }
}
