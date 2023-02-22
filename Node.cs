using StereoKit;
using System.Collections.Generic;


class Node<T> where T : Node<T> {
    public string text = "";
    public List<Node<T>> children = new List<Node<T>>();
    public T? parent;
}


class VisualNode : Node<VisualNode> {
    public Vec3 pos;

    public void Step() {
        Mesh.Sphere.Draw(Material.Unlit, Matrix.S(0.1f) * Matrix.T(pos));
        if (parent != null)
            Lines.Add(pos, parent.pos, new Color32 { r = 55, g = 55, b = 55, a = 255 }, U.cm);
    }
}