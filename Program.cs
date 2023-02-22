using System.Collections.Generic;
using StereoKit;


class Program {
    static void Main(string[] args) {
        SKSettings settings = new SKSettings {
            appName = "arlo",
            assetsFolder = "assets",
        };
        if (!SK.Initialize(settings))
            Environment.Exit(1);
        // Pose windowPose = new Pose(0.0f, 0.0f, -0.4f, Quat.LookDir(0, 0, 1));
        VisualNode node = new VisualNode { text = "Hello!", pos = new Vec3(0.0f, 0.0f, -0.4f) };

        SK.Run(() => {
            // UI.WindowBegin("Main window", ref windowPose, new Vec2(50, 0) * U.cm, UIWin.Normal);
            // UI.Text("Hello world");
            // UI.WindowEnd();
            Queue<VisualNode> toVisit = new Queue<VisualNode>();
            toVisit.Enqueue(node);
            while (toVisit.Count > 0) {
                VisualNode next = toVisit.Dequeue();
                next.Step();
                foreach (VisualNode n in next.children)
                    toVisit.Enqueue(n);
            }
        });
    }
}
