using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Net.Http;
using System.Text;
using System.IO;
using StereoKit;
using System;


class VUI : Editor {
    public string name { get { return "Record voice"; } }
    bool isActive = false;
    List<float> samples = new List<float>();
    MyWhisperAPIClient whisperAPIClient = new MyWhisperAPIClient();

    public void DrawUI() {
        if (isActive) {
            UI.HSeparator();
            if (UI.Button("Stop")) {
                isActive = false;
            }
        }
    }

    public async Task<string> Edit(string initialText, Action<string> setText) {
        await Task.Run(() => Microphone.Start());
        samples.Clear();
        float[] buf = new float[24000];
        isActive = true;
        while (isActive) {
            int unreadSamples = Microphone.Sound.UnreadSamples;
            int readSamples = Microphone.Sound.ReadSamples(ref buf);
            samples.AddRange(buf[0..readSamples]);
            if (unreadSamples > readSamples)
                await Task.Delay(300);
        }
        Microphone.Stop();
        string result = await TranscribeAudio();
        Console.WriteLine(result);
        return result;
    }

    public async Task<string> TranscribeAudio() {
        byte[] waveArray = ExportWav();
        return await whisperAPIClient.SendRequest(waveArray);
    }

    byte[] ExportWav() {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter bw = new BinaryWriter(stream)) {
            int bitsPerSample = 32;  // Float
            Int32 numChannels = 1, sampleRate = 48000;  // For StereoKit

            bw.Write(Encoding.ASCII.GetBytes("RIFF"));
            bw.Write((Int32)(36 + samples.Count * bitsPerSample / 8));
            bw.Write(Encoding.ASCII.GetBytes("WAVE"));

            bw.Write(Encoding.ASCII.GetBytes("fmt "));
            bw.Write((Int32)16);
            bw.Write((Int16)1);
            bw.Write((Int16)numChannels);
            bw.Write(sampleRate);
            bw.Write(sampleRate * numChannels * bitsPerSample / 8);
            bw.Write((Int16)(numChannels * bitsPerSample / 8));
            bw.Write((Int16)(bitsPerSample));

            bw.Write(Encoding.ASCII.GetBytes("data"));
            bw.Write((Int32)(samples.Count * bitsPerSample / 8));
            foreach (float s in samples)
                bw.Write(s);
        }
        return stream.ToArray();
    }
}


class MyWhisperAPIClient {
    HttpClient client = new HttpClient();
    public String apiEndpoint = "http://127.0.0.1:7936/transcribe";
    public async Task<string> SendRequest(byte[] waveArray) {
        MultipartFormDataContent formData = new MultipartFormDataContent();
        formData.Add(new ByteArrayContent(waveArray), "wav", "upload.wav");
        HttpResponseMessage response;
        try {
            response = await client.PostAsync(apiEndpoint, formData);
        }
        catch (HttpRequestException) {
            return "Looks like the audio transcription server isn't configured properly. Check it and rerun.";
        }
        if (!response.IsSuccessStatusCode) {
            return "The audio transcription server is responding with an error. Check it and rerun.";
            // throw new HttpRequestException();
        }
        JsonNode node = JsonNode.Parse(await response.Content.ReadAsStreamAsync())!;
        return ((string)node["result"]!);
    }
}
