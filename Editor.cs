using System.Threading.Tasks;
using System;


interface Editor {
    string name { get; }
    void DrawUI() { }
    Task<string> Edit(string initialText, Action<string> setText);
}

class CompleterEditor<T> : Editor where T : Completer {
    T completer;
    public string name { get { return "Complete"; } }

    public CompleterEditor(T completer) {
        this.completer = completer;
    }

    public async Task<string> Edit(string initialText, Action<string> setText) {
        string completion = await this.completer.GetCompletion(initialText);
        // setText(completion);
        return completion;
    }
}