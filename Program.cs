using StereoKit;


class Program {

    static void Main(string[] args) {
        SKSettings settings = new SKSettings {
            appName = "arlo",
            assetsFolder = "assets",
        };
        if (!SK.Initialize(settings))
            Environment.Exit(1);
        Pose windowPose = new Pose(0.0f, 0.0f, -0.4f, Quat.LookDir(0, 0, 1));

        SK.Run(() => {
            UI.WindowBegin("Main window", ref windowPose, new Vec2(50, 0) * U.cm, UIWin.Normal);
            UI.Text("Hello world");
            UI.WindowEnd();
            Mesh.Sphere.Draw(Material.Unlit, Matrix.S(0.1f) * Matrix.T(new Vec3()));
        });
    }
}
