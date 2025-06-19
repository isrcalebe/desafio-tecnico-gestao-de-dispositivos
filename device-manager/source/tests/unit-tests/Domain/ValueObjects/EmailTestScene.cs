using DeviceManager.Domain.ValueObjects;

namespace DeviceManager.UnitTests.Domain.ValueObjects;

public class EmailTestScene
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("USER@EXAMPLE.COM")]
    [InlineData("user.name+tag@sub.domain.co.uk")]
    public void Create_ValidEmail_ReturnsSuccess(string validEmail)
    {
        var result = Email.Create(validEmail);

        Assert.True(result.IsSuccess);
        Assert.Equal(validEmail.ToLowerInvariant(), result.Value.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_EmptyOrWhitespace_ReturnsError(string invalidEmail)
    {
        var result = Email.Create(invalidEmail);

        Assert.True(result.IsFailure);
        Assert.Equal("Email cannot be empty or whitespace.", result.Error.Message);
    }

    [Theory]
    [InlineData("plainaddress")]
    [InlineData("missingatsign.com")]
    [InlineData("missingdomain@")]
    [InlineData("@missingusername.com")]
    [InlineData("user@.com")]
    [InlineData("user@com")]
    [InlineData("user@domain,com")]
    [InlineData("user@domain")]
    public void Create_InvalidFormat_ReturnsError(string invalidEmail)
    {
        var result = Email.Create(invalidEmail);

        Assert.True(result.IsFailure);
        Assert.Equal("Email format is invalid.", result.Error.Message);
        Assert.Contains("example@domain.com", result.Error.AdditionalInfo[0]);
    }

    [Fact]
    public void ToString_ReturnsEmailValue()
    {
        var email = Email.Create("test@domain.com").Value;

        Assert.Equal("test@domain.com", email.ToString());
    }

    [Fact]
    public void Equals_And_HashCode_WorkCorrectly()
    {
        var email1 = Email.Create("test@domain.com").Value;
        var email2 = Email.Create("TEST@DOMAIN.COM").Value;
        var email3 = Email.Create("other@domain.com").Value;

        Assert.True(email1.Equals(email2));
        Assert.False(email1.Equals(email3));
        Assert.Equal(email1.GetHashCode(), email2.GetHashCode());
        Assert.NotEqual(email1.GetHashCode(), email3.GetHashCode());
    }

    [Fact]
    public void ImplicitOperator_ReturnsEmailValue()
    {
        var email = Email.Create("user@domain.com").Value;
        string value = email;

        Assert.Equal("user@domain.com", value);
    }
}
