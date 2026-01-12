namespace Either_Implementation.Either;

public class Example
{
    public async Task ExampleWithEither()
    {
        var operation = EitherAsync<string, int>.FromRight(10)
            .Map(n => n * 2)
            .FlatMap( n => Task.FromResult(Either<string, int>.ToRight(n + 5)));

        var result = await operation.Run();
        
        var message = result.Match(
            error=>$"Error:{error}",
            value => $"Value:{value}");
        Console.WriteLine(message);
    }
}