using Microsoft.JSInterop;

namespace BlazorApp.Client
{
    public class LocalStoragrageHelper:IAsyncDisposable
    {
        private Lazy<IJSObjectReference> _helperJsRef = new();
        private readonly IJSRuntime _jsRuntime;

        public LocalStoragrageHelper(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private async Task WaitForReference()
        {
            if (_helperJsRef.IsValueCreated is false)
            {
                _helperJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/LocalStoragrageHelper.js"));
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_helperJsRef.IsValueCreated)
            {
                await _helperJsRef.Value.DisposeAsync();
            }
        }
        public async Task<T> GetValueAsync<T>(string key)
        {
            await WaitForReference();
            var result = await _helperJsRef.Value.InvokeAsync<T>("get", key);

            return result;
        }

        public async Task SetValueAsync<T>(string key, T value)
        {
            await WaitForReference();
            await _helperJsRef.Value.InvokeVoidAsync("set", key, value);
        }

        public async Task Clear()
        {
            await WaitForReference();
            await _helperJsRef.Value.InvokeVoidAsync("clear");
        }

        public async Task RemoveAsync(string key)
        {
            await WaitForReference();
            await _helperJsRef.Value.InvokeVoidAsync("remove", key);
        }
    }
}
