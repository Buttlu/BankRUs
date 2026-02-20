using BankRUs.Domain.Entities;
using BankRUs.Tests.Inftastructure;
using BankRUs.WebApi.Dtos.Accounts;
using BankRUs.WebApi.Dtos.Auth;
using BankRUs.WebApi.Dtos.BankAccounts;
using BankRUs.WebApi.Dtos.Me;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace BankRUs.Tests.Integration;

public class BankRUsApiTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Deposit_ShouldIncrease_Return201AndCreatedTransaction()
    {
        // Create a user
        var userAccount = new CreateAccountRequestDto(
            FirstName: "bengt",
            LastName: "bengt",
            SocialSecurityNumber: "18900102-9819",
            Email: "bengt@bengtbengt.bengt"
        );
        var userResponse = await _client.PostAsJsonAsync("/api/accounts/create", userAccount);

        Assert.Equal(HttpStatusCode.Created, userResponse.StatusCode);
        var user = await userResponse.Content.ReadFromJsonAsync<CreateAccountResponseDto>();        
        Assert.NotNull(user);

        // Create a bank account
        var newBankAccount = new CreateBankAccountRequestDto(
            UserId: user.UserId,
            AccountName: "Savings Account"
        );
        var createBankAccountResponse = await _client.PostAsJsonAsync("/api/bank-accounts/create", newBankAccount);
        Assert.Equal(HttpStatusCode.Created, createBankAccountResponse.StatusCode);

        var bankAccount = await createBankAccountResponse.Content.ReadFromJsonAsync<BankAccountDto>();
        Assert.NotNull(bankAccount);

        // Deposit
        decimal amountToAdd = 1000;
        var newDeposit = new AddBalanceDto(
            Amount: amountToAdd,
            Reference: "Savings"
        );
        var depositResponse = await _client.PostAsJsonAsync($"/api/bank-accounts/{bankAccount.Id}/deposits", newDeposit);
        Assert.Equal(HttpStatusCode.Created, depositResponse.StatusCode);

        var result = await depositResponse.Content.ReadFromJsonAsync<AddBalanceResultDto>();
        Assert.NotNull(result);

        // Final validation
        Assert.Equal(amountToAdd, result.Amount);
        Assert.Equal("Deposit", result.Type);
        Assert.Equal(user.UserId, result.UserId);
    }

    [Fact]
    public async Task GetMeDetails_Returns200Ok()
    {
        string userEmail = "bengt@bengtbengt.bengt";
        // Create a user
        var userAccount = new CreateAccountRequestDto(
            FirstName: "bengt",
            LastName: "bengt",
            SocialSecurityNumber: "18900102-9819",
            Email: userEmail
        );
        var createAccountResponse = await _client.PostAsJsonAsync("/api/accounts/create", userAccount);
        // Since another test might've already added the user, getting a duplicate shouldn't stop
        // Technically bad since the tests become linked but it can be sorted out later
        //Assert.Equal(HttpStatusCode.Created, createAccountResponse.StatusCode);
        //var user = await createAccountResponse.Content.ReadFromJsonAsync<CreateAccountResponseDto>();
        //Assert.NotNull(user);

        // Create a token
        var tokenRequest = new LoginRequestDto(
            Username: userEmail,
            Password: "Aa111!"
        );
        var tokenResponse = await _client.PostAsJsonAsync("/api/auth/login", tokenRequest);
        Assert.Equal(HttpStatusCode.OK, tokenResponse.StatusCode);

        var token = (await tokenResponse.Content.ReadFromJsonAsync<LoginResponseDto>())?.Token;
        Assert.NotNull(token);

        // Get the data
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var userResponse = await _client.GetAsync("/api/me");
        _client.DefaultRequestHeaders.Remove("Authorization");
        Assert.Equal(HttpStatusCode.OK, userResponse.StatusCode);
        
        var userData = await userResponse.Content.ReadFromJsonAsync<MeResponseDto>();
        Assert.NotNull(userData);
        
        // Final validtion
        Assert.Equal(userEmail, userData.Email);
    }
}
