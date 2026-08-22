namespace MyGame.Core
{
    public readonly struct InventoryOperationResult
    {
        public bool Success { get; }
        public int RemainingAmount { get; }
        public string ErrorMessage { get; }

        public InventoryOperationResult(bool success, int remainingAmount = 0, string errorMessage = null)
        {
            Success = success;
            RemainingAmount = remainingAmount;
            ErrorMessage = errorMessage;
        }

        public static InventoryOperationResult Ok(int remainingAmount = 0) => new InventoryOperationResult(true, remainingAmount);

        public static InventoryOperationResult Fail(string errorMessage, int remainingAmount = 0) => new InventoryOperationResult(false, remainingAmount, errorMessage);
    }
}

