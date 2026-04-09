# Composition and Error Handling

Composition and inheritance, Result type pattern, and testing patterns for modern C#.

## Contents

- [Composition And Inheritance](#composition-and-inheritance)
- [Result Type Pattern (Railway-Oriented Programming)](#result-type-pattern-railway-oriented-programming)
- [Testing Patterns](#testing-patterns)

## Composition and Inheritance

These are both techniques in object-oriented programming for code reuse. Inheritance establishes an "is-a" relationship by allowing a class to inherit properties and behaviours from another class, while composition creates a "has-a" relationship by combining existing classes to form more complex objects, often leading to more flexible and maintainable code.
Do NOT follow mantras as: "favour composition over inheritance". 

Use inheritance for:

* Stable hierarchies 
* Clear “is-a” relationships
* Shared, consistent behaviour

Use composition for:

* Flexibility
* Reuse without tight coupling
* Clear “has-a” relationships
* Changing behaviour dynamically

## Result Type Pattern (Railway-Oriented Programming)

For expected errors, use a `Result<T, TError>` type instead of exceptions.

```csharp
// Simple Result type as readonly record struct
public readonly record struct Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;
    private readonly bool _isSuccess;

    private Result(TValue value)
    {
        _value = value;
        _error = default;
        _isSuccess = true;
    }

    private Result(TError error)
    {
        _value = default;
        _error = error;
        _isSuccess = false;
    }

    public bool IsSuccess => _isSuccess;
    public bool IsFailure => !_isSuccess;

    public TValue Value => _isSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access Value of a failed result");

    public TError Error => !_isSuccess
        ? _error!
        : throw new InvalidOperationException("Cannot access Error of a successful result");

    public static Result<TValue, TError> Success(TValue value) => new(value);
    public static Result<TValue, TError> Failure(TError error) => new(error);

    public Result<TOut, TError> Map<TOut>(Func<TValue, TOut> mapper)
        => _isSuccess
            ? Result<TOut, TError>.Success(mapper(_value!))
            : Result<TOut, TError>.Failure(_error!);

    public Result<TOut, TError> Bind<TOut>(Func<TValue, Result<TOut, TError>> binder)
        => _isSuccess ? binder(_value!) : Result<TOut, TError>.Failure(_error!);

    public TValue GetValueOr(TValue defaultValue)
        => _isSuccess ? _value! : defaultValue;

    public TResult Match<TResult>(
        Func<TValue, TResult> onSuccess,
        Func<TError, TResult> onFailure)
        => _isSuccess ? onSuccess(_value!) : onFailure(_error!);
}

// Error type as readonly record struct
public readonly record struct OrderError(string Code, string Message);

// Usage example
public sealed class OrderService(IOrderRepository repository)
{
    public async Task<Result<Order, OrderError>> CreateOrderAsync(
        CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        // Validate
        var validationResult = ValidateRequest(request);
        if (validationResult.IsFailure)
            return Result<Order, OrderError>.Failure(validationResult.Error);

        // Check inventory
        var inventoryResult = await CheckInventoryAsync(request.Items, cancellationToken);
        if (inventoryResult.IsFailure)
            return Result<Order, OrderError>.Failure(inventoryResult.Error);

        // Create order
        var order = new Order(
            OrderId.New(),
            new CustomerId(request.CustomerId),
            request.Items);

        await repository.SaveAsync(order, cancellationToken);

        return Result<Order, OrderError>.Success(order);
    }

    // Pattern matching on Result
    public IActionResult MapToActionResult(Result<Order, OrderError> result)
    {
        return result.Match(
            onSuccess: order => new OkObjectResult(order),
            onFailure: error => error.Code switch
            {
                "VALIDATION_ERROR" => new BadRequestObjectResult(error.Message),
                "INSUFFICIENT_INVENTORY" => new ConflictObjectResult(error.Message),
                "NOT_FOUND" => new NotFoundObjectResult(error.Message),
                _ => new ObjectResult(error.Message) { StatusCode = 500 }
            }
        );
    }
}
```

**When to use Result vs Exceptions:**
- **Use Result**: Expected errors (validation, business rules, not found)
- **Use Exceptions**: Unexpected errors (network failures, system errors, programming bugs)

## Testing Patterns

```csharp
// Use record for test data builders
public record OrderBuilder
{
    public OrderId Id { get; init; } = OrderId.New();
    public CustomerId CustomerId { get; init; } = CustomerId.New();
    public Money Total { get; init; } = new Money(100m, "USD");
    public IReadOnlyList<OrderItem> Items { get; init; } = Array.Empty<OrderItem>();

    public Order Build() => new(Id, CustomerId, Total, Items);
}

// Use 'with' expression for test variations
[Fact]
public void CalculateDiscount_LargeOrder_AppliesCorrectDiscount()
{
    // Arrange
    var baseOrder = new OrderBuilder().Build();
    var largeOrder = baseOrder with
    {
        Total = new Money(1500m, "USD")
    };

    // Act
    var discount = _service.CalculateDiscount(largeOrder);

    // Assert
    discount.Should().Be(new Money(225m, "USD")); // 15% of 1500
}

// Span-based testing
[Theory]
[InlineData("ORD-12345", true)]
[InlineData("INVALID", false)]
public void TryParseOrderId_VariousInputs_ReturnsExpectedResult(
    string input,
    bool expected)
{
    // Act
    var result = OrderIdParser.TryParse(input.AsSpan(), out var orderId);

    // Assert
    result.Should().Be(expected);
}

// Testing with value objects
[Fact]
public void Money_Add_SameCurrency_ReturnsSum()
{
    // Arrange
    var money1 = new Money(100m, "USD");
    var money2 = new Money(50m, "USD");

    // Act
    var result = money1.Add(money2);

    // Assert
    result.Should().Be(new Money(150m, "USD"));
}

[Fact]
public void Money_Add_DifferentCurrency_ThrowsException()
{
    // Arrange
    var usd = new Money(100m, "USD");
    var eur = new Money(50m, "EUR");

    // Act & Assert
    var act = () => usd.Add(eur);
    act.Should().Throw<InvalidOperationException>()
        .WithMessage("*different currencies*");
}
```
