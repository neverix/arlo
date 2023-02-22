using System.Collections.Generic;
using StereoKit;
using System;


interface MenuUI<T> where T : Node<T> {
    void Open(T n);
}

class MenuSK : MenuUI<NodeSK> {

    public Pose pos;
    public NodeSK? nodeSelected;
    float windowWidth = 0.4f;
    float windowHeight = 0.4f;
    public void Open(NodeSK n) {
        pos.position = n.pos.position + n.pos.orientation.Rotate(new Vec3(-windowWidth / 2f - 0.1f, 0.0f, 0.0f));
        nodeSelected = n;
    }

    public void Step() {
        pos.orientation = Quat.LookAt(pos.position, Input.Head.position);
        UI.WindowBegin(nodeSelected == null ? "Open some node" : "Node info", ref pos, new Vec2(windowWidth, windowHeight));
        DrawNode();
        UI.WindowEnd();
    }

    void DrawNode() {
        if (nodeSelected == null) {
            return;
        }
        if (!nodeSelected.actualized) {
            UI.Button("Speak up text");
            UI.SameLine();
            UI.Button("Type text");
            UI.SameLine();
            UI.Button("Generate");
        }
        UI.Text("Text");
        UI.HSeparator();
        List<string> textChunks = new List<string> { nodeSelected.text };
        NodeSK node = nodeSelected;
        while (node.parent != null) {
            textChunks.Insert(0, node.parent.text);
            node = node.parent;
        }
        // UI.Settings = new UISettings {
        //     padding = 0
        // };
        // // TextStyle style = TextStyle.Default;  // TODO?
        // // float widthSoFar = 0.0f;
        // // float padding = 0.0f;  // UI.Settings.padding;
        // foreach (string chunk in textChunks) {
        //     foreach (char c in chunk.ToCharArray()) {
        //         // float newWidth = widthSoFar + Text.Size(word, style).x;
        //         // if (newWidth > windowWidth - padding * 2f) {
        //         //     widthSoFar = 0.0f;
        //         // }
        //         // else if (widthSoFar > 0.0f) {
        //         //     UI.SameLine();
        //         //     widthSoFar = newWidth;
        //         // }
        //         UI.SameLine();
        //         UI.Label(c.ToString(), usePadding: false);
        //     }
        // }
        // UI.Settings = new UISettings {
        //     gutter = 10 * Units.mm2m
        // };
        UI.Text(string.Join("", textChunks));
        if (nodeSelected.actualized) {
            UI.HSeparator();
            if (nodeSelected.parent != null) {
                bool deleted = UI.Button("Delete");
                if (deleted) {
                    nodeSelected.parent.children.Remove(nodeSelected);
                    nodeSelected = null;
                    return;
                }
            }
        }
    }
}