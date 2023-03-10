using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using StereoKit;


interface MenuUI<T> where T : Node<T> {
    void Open(T n);
}

class MenuSK : MenuUI<NodeSK> {

    public Pose pose;
    public NodeSK? nodeSelected;
    public Editor[] editors = { };
    bool editorActive = false;
    float windowWidth = 0.4f;
    float windowHeight = 0.4f;
    public void Open(NodeSK n) {
        pose.position = n.pose.position + n.pose.orientation.Rotate(new Vec3(-windowWidth / 2f - 0.1f, 0.0f, 0.0f));
        nodeSelected = n;
    }

    public void Step() {
        pose.orientation = Quat.LookAt(pose.position, Input.Head.position);
        UI.WindowBegin(nodeSelected == null ? "Open some node" : "Node info", ref pose, new Vec2(windowWidth, windowHeight));
        DrawNode();
        UI.WindowEnd();
    }

    void DrawNode() {
        if (nodeSelected == null) {
            return;
        }

        if (nodeSelected.parent == null) {
            UI.Text("Root node");
            UI.HSeparator();
        }

        // UI.Text("Text");
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

        if (nodeSelected.actualizedChildren == 0) {
            UI.HSeparator();
            bool syncEditorActive = editorActive;
            UI.PushEnabled(!syncEditorActive);
            foreach (var (editor, index) in editors.WithIndex()) {
                if (index > 0) UI.SameLine();
                if (UI.Button(editor.name)) {
                    editorActive = true;
                    Task.Run(async () => {
                        string text = await editor.Edit(string.Join("", textChunks), (string newText) => {
                            nodeSelected.text = newText;
                        });
                        editorActive = false;
                        NodeSK? baby = nodeSelected.Actualize(nodeSelected.text + text);
                        if (baby != null)
                            nodeSelected = baby;
                    });
                }
            }
            UI.PopEnabled();
            foreach (Editor editor in editors) {
                editor.DrawUI();
            }
        }
        if (nodeSelected.actualized) {
            UI.HSeparator();
            if (nodeSelected.parent != null) {
                if (UI.Button("Delete")) {
                    if (editorActive) return;
                    nodeSelected.Delete();
                    nodeSelected = null;
                    return;
                }
            }
        }
    }
}


// Credit: https://stackoverflow.com/a/39997157
// Technically unnecessary but more comfortable
public static class EnumExtension {
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
       => self.Select((item, index) => (item, index));
}
