using System.Collections.Generic;
using StereoKit;


class Node<T> where T : Node<T> {
    public string text = "";
    public List<Node<T>> children = new List<Node<T>>();
    public T? parent;
}

class NodeSK : Node<NodeSK> {
    public Pose pos;
    static Mesh mesh = Mesh.GenerateSphere(0.1f);
    bool actualized = false;
    static NodeSK() { }
    public NodeSK(Pose? startPos = null, bool actualized = true) {
        pos = startPos ?? Pose.Identity;
        if (actualized) {
            AddBaby();
        }
    }
    public void AddBaby() {
        children.Add(new NodeSK(actualized: false) { pos = new Pose(pos.position + pos.orientation.Rotate(new Vec3(0.0f, 0.0f, 0.2f)), pos.orientation), parent = this });
    }
    public void Step(MenuUI<NodeSK> menu) {
        bool isHandled = UI.HandleBegin("Node", ref pos, mesh.Bounds, drawHandle: false, UIMove.FaceUser);
        mesh.Draw(Material.Unlit, Matrix.Identity);
        if (parent != null)
            Lines.Add(Hierarchy.ToLocal(pos.position), Hierarchy.ToLocal(parent.pos.position), new Color32 { r = 55, g = 55, b = 55, a = 255 }, U.cm);
        UI.HandleEnd();
        if (isHandled) {
            menu.Open(this);
        }
    }
}
