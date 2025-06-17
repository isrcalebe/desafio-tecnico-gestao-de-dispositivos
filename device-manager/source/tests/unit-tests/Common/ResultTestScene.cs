using DeviceManager.Common;

namespace DeviceManager.UnitTests.Common;

public class ResultTestScene
{
    [Fact]
    public void Success_CreatesSuccessResult()
    {
        var result = Result<int, string>.Success(42);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Failure_CreatesFailureResult()
    {
        var result = Result<int, string>.Failure("error");

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("error", result.Error);
    }

    [Fact]
    public void Value_ThrowsOnFailure()
    {
        var result = Result<int, string>.Failure("fail");

        Assert.Throws<InvalidOperationException>(() => _ = result.Value);
    }

    [Fact]
    public void Error_ThrowsOnSuccess()
    {
        var result = Result<int, string>.Success(1);

        Assert.Throws<InvalidOperationException>(() => _ = result.Error);
    }

    [Fact]
    public void TryGetValue_ReturnsTrueOnSuccess()
    {
        var result = Result<int, string>.Success(5);

        Assert.True(result.TryGetValue(out var value));
        Assert.Equal(5, value);
    }

    [Fact]
    public void TryGetValue_ReturnsFalseOnFailure()
    {
        var result = Result<int, string>.Failure("fail");

        Assert.False(result.TryGetValue(out var value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryGetError_ReturnsTrueOnFailure()
    {
        var result = Result<int, string>.Failure("fail");

        Assert.True(result.TryGetError(out var error));
        Assert.Equal("fail", error);
    }

    [Fact]
    public void TryGetError_ReturnsFalseOnSuccess()
    {
        var result = Result<int, string>.Success(10);

        Assert.False(result.TryGetError(out var error));
        Assert.Equal(default, error);
    }

    [Fact]
    public void Match_ExecutesOnSuccess()
    {
        var result = Result<int, string>.Success(7);
        var called = 0;

        result.Match(
            v => called = v,
            e => called = -1
        );

        Assert.Equal(7, called);
    }

    [Fact]
    public void Match_ExecutesOnFailure()
    {
        var result = Result<int, string>.Failure("fail");
        var called = "";

        result.Match(
            v => called = "success",
            e => called = e
        );
        Assert.Equal("fail", called);
    }

    [Fact]
    public void MatchFunc_ReturnsCorrectValue()
    {
        var success = Result<int, string>.Success(3);
        var failure = Result<int, string>.Failure("err");

        Assert.Equal("Value: 3", success.Match(v => $"Value: {v}", e => $"Error: {e}"));
        Assert.Equal("Error: err", failure.Match(v => $"Value: {v}", e => $"Error: {e}"));
    }

    [Fact]
    public void Map_TransformsValueOnSuccess()
    {
        var result = Result<int, string>.Success(2);
        var mapped = result.Map(v => v.ToString(CultureInfo.InvariantCulture));

        Assert.True(mapped.IsSuccess);
        Assert.Equal("2", mapped.Value);
    }

    [Fact]
    public void Map_DoesNotTransformOnFailure()
    {
        var result = Result<int, string>.Failure("fail");
        var mapped = result.Map(v => v.ToString(CultureInfo.InvariantCulture));

        Assert.True(mapped.IsFailure);
        Assert.Equal("fail", mapped.Error);
    }

    [Fact]
    public void MapError_TransformsErrorOnFailure()
    {
        var result = Result<int, string>.Failure("fail");
        var mapped = result.MapError(e => e.Length);

        Assert.True(mapped.IsFailure);
        Assert.Equal(4, mapped.Error);
    }

    [Fact]
    public void MapError_DoesNotTransformOnSuccess()
    {
        var result = Result<int, string>.Success(5);
        var mapped = result.MapError(e => e.Length);

        Assert.True(mapped.IsSuccess);
        Assert.Equal(5, mapped.Value);
    }

    [Fact]
    public void Then_ChainsOnSuccess()
    {
        var result = Result<int, string>.Success(2);
        var chained = result.Then(v => Result<string, string>.Success($"Num: {v}"));

        Assert.True(chained.IsSuccess);
        Assert.Equal("Num: 2", chained.Value);
    }

    [Fact]
    public void Then_DoesNotChainOnFailure()
    {
        var result = Result<int, string>.Failure("fail");
        var chained = result.Then(v => Result<string, string>.Success($"Num: {v}"));

        Assert.True(chained.IsFailure);
        Assert.Equal("fail", chained.Error);
    }

    [Fact]
    public void OrElse_ChainsOnFailure()
    {
        var result = Result<int, string>.Failure("fail");
        var chained = result.OrElse(e => Result<int, string>.Success(99));

        Assert.True(chained.IsSuccess);
        Assert.Equal(99, chained.Value);
    }

    [Fact]
    public void OrElse_DoesNotChainOnSuccess()
    {
        var result = Result<int, string>.Success(1);
        var chained = result.OrElse(e => Result<int, string>.Success(99));

        Assert.True(chained.IsSuccess);
        Assert.Equal(1, chained.Value);
    }

    [Fact]
    public void UnwrapOr_ReturnsValueOnSuccess()
    {
        var result = Result<int, string>.Success(5);

        Assert.Equal(5, result.UnwrapOr(10));
    }

    [Fact]
    public void UnwrapOr_ReturnsDefaultOnFailure()
    {
        var result = Result<int, string>.Failure("fail");

        Assert.Equal(10, result.UnwrapOr(10));
    }

    [Fact]
    public void UnwrapOrElse_ReturnsValueOnSuccess()
    {
        var result = Result<int, string>.Success(5);

        Assert.Equal(5, result.UnwrapOrElse(e => 99));
    }

    [Fact]
    public void UnwrapOrElse_UsesFuncOnFailure()
    {
        var result = Result<int, string>.Failure("fail");

        Assert.Equal(99, result.UnwrapOrElse(e => 99));
    }

    [Fact]
    public void ImplicitConversion_Success()
    {
        Result<int, string> result = 123;

        Assert.True(result.IsSuccess);
        Assert.Equal(123, result.Value);
    }

    [Fact]
    public void ImplicitConversion_Failure()
    {
        Result<int, string> result = "fail";

        Assert.True(result.IsFailure);
        Assert.Equal("fail", result.Error);
    }

    [Fact]
    public void Equality_SuccessAndFailure()
    {
        var s1 = Result<int, string>.Success(1);
        var s2 = Result<int, string>.Success(1);

        var f1 = Result<int, string>.Failure("err");
        var f2 = Result<int, string>.Failure("err");

        Assert.True(s1 == s2);
        Assert.True(f1 == f2);

        Assert.False(s1 == f1);
        Assert.True(s1 != f1);
    }

    [Fact]
    public void ToString_OutputsCorrectly()
    {
        var s = Result<int, string>.Success(42);
        var f = Result<int, string>.Failure("fail");

        Assert.Equal("Success(42)", s.ToString());
        Assert.Equal("Failure(fail)", f.ToString());
    }

    [Fact]
    public void StaticResultClass_SuccessAndFailure()
    {
        var s = Result.Success<int, string>(5);
        var f = Result.Failure<int, string>("fail");

        Assert.True(s.IsSuccess);
        Assert.Equal(5, s.Value);

        Assert.True(f.IsFailure);
        Assert.Equal("fail", f.Error);
    }
}
