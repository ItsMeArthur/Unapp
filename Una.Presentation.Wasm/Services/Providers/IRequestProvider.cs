namespace Una.Presentation.Wasm.Services.Providers
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri);
        Task<TResult> PostAsync<TResult>(string uri, object data, string header = "");
        Task<TResult> PutAsync<TResult>(string uri, object data, string header = "");
        Task<bool> DeleteAsync(string uri);
    }
}

