using StereoKit;

SK.Initialize();
Pose  windowPose = new Pose(0.0f, 0.0f, 0.0f, Quat.LookDir(1,0,1));

bool  showHeader = true;
float slider     = 0.5f;

SK.Run(()=>{
    UI.WindowBegin("Window", ref windowPose, new Vec2(20, 0) * U.cm, showHeader?UIWin.Normal:UIWin.Body);
    UI.Toggle("Show Header", ref showHeader);
    UI.Label("Slide");
    UI.SameLine();
    UI.HSlider("slider", ref slider, 0, 1, 0.2f, 72 * U.mm);
    UI.WindowEnd();
    Mesh.Sphere.Draw(Material.Default, Matrix.S(0.1f));
});