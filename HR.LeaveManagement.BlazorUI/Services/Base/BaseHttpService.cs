using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace HR.LeaveManagement.BlazorUI.Services.Base;

public class BaseHttpService
{
    protected IClient Client;
    protected readonly ILocalStorageService LocalStorage;
    public BaseHttpService(IClient client, ILocalStorageService localStorage)
    {
        Client = client;
        LocalStorage = localStorage;
    }

    protected Response<Guid> ConvertApiExceptions<Guid>(ApiException ex)
    {
        return ex.StatusCode switch
        {
            400 => new Response<Guid>()
            {
                Message = "Invalid data was submitted",
                ValidationErrors = ex.Response,
                Success = false
            },
            404 => new Response<Guid>() { Message = "The record was not found.", Success = false },
            _ => new Response<Guid>() { Message = "Something went wrong, please try again later.", Success = false }
        };
    }

    protected async Task AddBearerToken()
    {
        if (await LocalStorage.ContainKeyAsync("token"))
            Client.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await LocalStorage.GetItemAsync<string>("token"));
    }

}