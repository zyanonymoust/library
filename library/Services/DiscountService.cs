using library.Models;
using library.Strategies;

namespace library.Services;

public class DiscountService
{
    private readonly IDiscountStrategy _discountStrategy;

    public DiscountService(
        IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public decimal CalculateDiscountedPrice(
        Product product)
    {
        return _discountStrategy.ApplyDiscount(
            product.Price
        );
    }
}