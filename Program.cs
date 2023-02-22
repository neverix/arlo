﻿using System.Collections.Generic;
using StereoKit;
using System.IO;
using System;


class Program {
    static void Main(string[] args) {
        SKSettings settings = new SKSettings {
            appName = "arlo",
            assetsFolder = "assets",
        };
        if (!SK.Initialize(settings))
            Environment.Exit(1);
        // Pose windowPose = new Pose(0.0f, 0.0f, -0.4f, Quat.LookDir(0, 0, 1));
        NodeSK node = new NodeSK(new Pose(0.0f, 0.0f, -0.4f, Quat.LookDir(0, 0, 1))) { text = "Hi" };
        // Editor completer = new CompleterEditor<OAICompleter>(new OAICompleter(File.ReadAllText("assets/oai_key")));
        Editor completer = new CompleterEditor<DummyCompleter>(new DummyCompleter(File.ReadAllText("assets/sample_completion")));
        MenuSK menu = new MenuSK { pos = new Pose(0.3f, 0.2f, -0.4f, Quat.LookDir(0, 0, 1)), editors = new Editor[] { completer } };

        SK.Run(() => {
            // UI.WindowBegin("Main window", ref windowPose, new Vec2(50, 0) * U.cm, UIWin.Normal);
            // UI.Text("Hello world");
            // UI.WindowEnd();
            Queue<NodeSK> toVisit = new Queue<NodeSK>();
            toVisit.Enqueue(node);
            int nodeVisited = 0;
            while (toVisit.Count > 0) {
                NodeSK next = toVisit.Dequeue();
                UI.PushId(nodeVisited);
                next.Step(menu);
                UI.PopId();
                nodeVisited++;
                foreach (NodeSK n in next.children)
                    toVisit.Enqueue(n);
            }
            menu.Step();
        });
    }
}
