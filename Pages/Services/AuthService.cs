using Blazored.LocalStorage;

public class AuthService
{
    private readonly ILocalStorageService _localStorage;

    private const string TokenKey = "authToken";
    private const string RedirectValue = "redirected";

    public event Action OnAuthStateChanged;

    public AuthService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<bool> Login(string username, string password)
    {
        if (username == "admin" && password == "password")
        {
            await _localStorage.SetItemAsync(TokenKey, "fake-jwt-token");
            AuthStateChanged();
            return true;
        }

        return false;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        AuthStateChanged();
    }

    public async Task<bool> IsAuthenticated()
    {
        return !string.IsNullOrWhiteSpace(await _localStorage.GetItemAsync<string>(TokenKey));
    }

    public async Task Redirect()
    {
        await _localStorage.SetItemAsync(RedirectValue, true);
    }

    public async Task<bool> IsRedirected()
    {
        return await _localStorage.GetItemAsync<bool>(RedirectValue);
    }

    public async Task RemoveRedirect()
    {
        await _localStorage.RemoveItemAsync(RedirectValue);
    }

    private void AuthStateChanged()
    {
        OnAuthStateChanged.Invoke();
    }
}