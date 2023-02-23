
using System.Threading.Tasks;
using StereoKit;
using System;


class VUI : Editor {
    public string name { get { return "Record voice"; } }
    bool isActive = false;
    // Surely this will never overflow!
    float[] samples = new float[48000 * 60];
    int usedUp = 0;

    public void DrawUI() {
        if (isActive) {
            UI.HSeparator();
            if (UI.Button("Stop")) {
                isActive = false;
            }
        }
    }

    public async Task<string> Edit(string initialText, Action<string> setText) {
        Microphone.Start();
        usedUp = 0;
        float[] buf = new float[48000];
        isActive = true;
        while (isActive) {
            int unreadSamples = Microphone.Sound.UnreadSamples;
            int readSamples = Microphone.Sound.ReadSamples(ref buf);
            Array.Copy(buf, 0, samples, usedUp, readSamples);
            usedUp += readSamples;
            if (unreadSamples > readSamples)
                await Task.Delay(300);
        }
        Microphone.Stop();
        return "";
    }
}