using System.Threading.Tasks;


interface Completer {
    Task<string> GetCompletion(string prefix);
}

class OAICompleter : Completer {
    OpenAI_API.OpenAIAPI api;
    public OAICompleter(string apiKey) {
        api = new OpenAI_API.OpenAIAPI(apiKey);
    }

    public async Task<string> GetCompletion(string prefix) {
        return await api.Completions.GetCompletion(prefix);
    }
}