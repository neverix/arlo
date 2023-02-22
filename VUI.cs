
using System.Threading.Tasks;
using System;


class VUI : Editor {
    public string name { get { return "Record voice"; } }

    public async Task<string> Edit(string initialText, Action<string> setText) {
        return " This does not currently work.";
    }
}