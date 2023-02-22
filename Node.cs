using System.Collections.Generic;
using StereoKit;
using System;


class Node<T> where T : Node<T> {
    public string text = "";
    public List<Node<T>> children = new List<Node<T>>();
    public T? parent;
    public bool actualized = false;

    public void Delete() {
        Delete(deleteSelf: true);
    }
    void Delete(bool deleteSelf = true) {
        // Can I do this if the invalid argument is `this`? Is an assert better?
        if (parent == null)
            throw new ArgumentException();
        foreach (Node<T> child in children) {
            child.Delete(deleteSelf: false);
        }
        if (deleteSelf)
            parent.children.Remove(this);
        return;
    }
}

class NodeSK : Node<NodeSK> {
    public Pose pose;
    Vec3 vel;
    Vec3 acc;
    float targetLength = 0.2f;
    float springiness = 2e+3f;
    float damping = 1e+2f;
    float maxVelocity = 1.0f;
    static Mesh mesh = Mesh.GenerateSphere(0.1f);
    static NodeSK() { }
    public NodeSK(Pose? startPose = null, bool actualized = true) {
        this.actualized = actualized;
        pose = startPose ?? Pose.Identity;
        if (actualized) {
            AddBaby();
        }
    }
    void AddBaby() {
        Quat orientation = MathUtils.RandomQuaternion();  // pos.orientation;
        children.Add(new NodeSK(actualized: false) {
            pose = new Pose(pose.position + orientation.Rotate(new Vec3(0.0f, 0.0f, 0.2f)), orientation),
            parent = this,
            text = "... Nothing here yet"
        });
    }
    public void Actualize(string text) {
        this.text = text;
        actualized = true;
        AddBaby();
    }
    public void Step(MenuUI<NodeSK> menu) {
        bool isHandled = UI.HandleBegin("Node", ref pose, mesh.Bounds, drawHandle: false, UIMove.FaceUser);
        mesh.Draw(Material.Default, Matrix.Identity,
                  actualized ? new Color { r = 200, g = 55, b = 200, a = 255 } : new Color { r = 200, g = 200, b = 55, a = 200 });
        if (parent != null)
            Lines.Add(Hierarchy.ToLocal(pose.position), Hierarchy.ToLocal(parent.pose.position),
                      new Color32 { r = 55, g = 55, b = 55, a = 255 }, U.cm);
        UI.HandleEnd();
        if (isHandled) {
            menu.Open(this);
            vel *= 0;
            acc *= 0;
        }
        else {
            // TODO should physics run in a different thread?
            if (parent != null) {
                Vec3 offset = parent.pose.position - pose.position;
                double x = offset.Length - targetLength;
                if (x < 0) {
                    double inv = 1 / (1 + x / targetLength) - 1;
                    x = Math.Sign(x) * inv * inv;
                }
                acc += offset * (float)(x) * springiness;
            }
            acc += (-vel) * damping;
            vel += acc * Time.Elapsedf;
            if (vel.Length > maxVelocity) {
                vel = vel * maxVelocity / vel.Length;
            }
            pose.position += vel * Time.Elapsedf;
            acc *= 0;
        }
    }
}
