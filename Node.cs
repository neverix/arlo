using StereoKit;
using System.Collections.Generic;


class Node<T> where T : Node<T> {
    public string text = "";
    public List<Node<T>> children = new List<Node<T>>();
    public T? parent;
}


class VisualNode : Node<VisualNode> {
    public Pose pos;
    static Mesh mesh = Mesh.GenerateSphere(0.1f);

    static VisualNode() { }
    public void Step() {
        UI.HandleBegin("sphere", ref pos, mesh.Bounds, drawHandle: false, UIMove.FaceUser);
        mesh.Draw(Material.Unlit, Matrix.Identity);
        if (parent != null)
            Lines.Add(new Vec3(), parent.pos.position - pos.position, new Color32 { r = 55, g = 55, b = 55, a = 255 }, U.cm);
        UI.HandleEnd();
    }
}