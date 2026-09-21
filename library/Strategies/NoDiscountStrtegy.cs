namespace library.Strategies;

public class NoDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal price)
    {
        return price;
    }
}