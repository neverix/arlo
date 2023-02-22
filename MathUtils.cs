using StereoKit;
using System;


class MathUtils {
    // Source: https://gist.github.com/tansey/1444070
    public static double Gaussian(Random random, double mean = 0.0, double stddev = 1.0) {
        // The method requires sampling from a uniform random of (0,1]
        // but Random.NextDouble() returns a sample of [0,1).
        double x1 = 1 - random.NextDouble();
        double x2 = 1 - random.NextDouble();

        double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
        return y1 * stddev + mean;
    }

    public static Quat RandomQuaternion(Random? random = null) {
        if (random == null)
            random = new Random();
        double x = Gaussian(random), y = Gaussian(random), z = Gaussian(random);
        return Quat.LookAt(Vec3.Zero, new Vec3((float)x, (float)y, (float)z), Vec3.Up);
    }
}