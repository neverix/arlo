using System.Threading.Tasks;


interface Completer {
    Task<string> GetCompletion(string prefix);
}

class DummyCompleter : Completer {
    string text;
    public DummyCompleter(string textToWrite) {
        text = textToWrite;
    }

    public async Task<string> GetCompletion(string prefix) {
        return text;
    }
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