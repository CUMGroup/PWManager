using PWManager.Domain.Entities;

namespace PWManager.UnitTests.Domain.Entities; 

public class GroupTests {

    private Group _sut;

    public GroupTests() {
        _sut = new Group("test-userId");
    }

    #region AddAccount
    [Fact]
    public void Group_Should_AddNewAccount() {
        var acc = new Account("Hello", "name","Password");
        _sut.AddAccount(acc);
        
        Assert.True(_sut.Accounts.Count == 1);
        Assert.Equal(acc, _sut.Accounts[0]);
    }
    #endregion
    #region GetAccount
    [Fact]
    public void Group_Should_GetValidAccount() {
        AddAccountsToGroup(_sut, 5);
        var acc = _sut.Accounts[3];

        var returnedAccount = _sut.GetAccount(acc.Id);
        
        Assert.Equal(acc, returnedAccount);
    }

    [Fact]
    public void Group_ShouldNot_GetNonExistingAccount() {
        AddAccountsToGroup(_sut, 5);

        var returnedAccount = _sut.GetAccount("$NON_EXISTENT_ID$");
        
        Assert.Null(returnedAccount);
    }

    [Fact]
    public void Group_ShouldNot_GetAccountOnEmptyAccounts() {
        var returnedAccount = _sut.GetAccount("$NON_EXISTING_ID$");
        
        Assert.Null(returnedAccount);
    }
    #endregion
    #region Remove Account
    [Fact]
    public void Group_Should_RemoveAccountById() {
        AddAccountsToGroup(_sut, 5);
        var acc = _sut.Accounts[3];

        bool ret = _sut.RemoveAccount(acc.Id);
        
        Assert.True(ret);
        Assert.True(_sut.Accounts.Count == 4);
        Assert.DoesNotContain(_sut.Accounts, account => account.Id.Equals(acc.Id));
    }
    [Fact]
    public void Group_Should_RemoveAccount() {
        AddAccountsToGroup(_sut, 5);
        var acc = _sut.Accounts[3];

        var ret = _sut.RemoveAccount(acc);
        
        Assert.True(ret);
        Assert.True(_sut.Accounts.Count == 4);
        Assert.DoesNotContain(_sut.Accounts, account => account.Equals(acc));
    }

    [Fact]
    public void Group_ShouldNot_RemoveNonExistingAccount() {
        AddAccountsToGroup(_sut, 5);

        var ret = _sut.RemoveAccount("$NON_EXISTING_ID$");
        
        Assert.False(ret);
        Assert.True(_sut.Accounts.Count == 5);
    }
    [Fact]
    public void Group_ShouldNot_RemoveEmptyAccounts() {
        var ret = _sut.RemoveAccount("$NON_EXISTING_ID$");
        
        Assert.False(ret);
    }
    #endregion
    
    private static void AddAccountsToGroup(Group g, int numAccounts) {
        for (int i = 0; i < numAccounts; ++i) {
            g.Accounts.Add(new Account($"Cool Account Name {i}", "loginname", "very secure password"));
        }
    }
}