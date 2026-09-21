namespace library.Strategies;

public class PercentageDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountRate;

    public PercentageDiscountStrategy(decimal discountRate)
    {
        if (discountRate < 0 || discountRate > 1)
        {
            throw new ArgumentException(
                "Discount rate must be between 0 and 1."
            );
        }

        _discountRate = discountRate;
    }

    public decimal ApplyDiscount(decimal price)
    {
        return price - (price * _discountRate);
    }
}