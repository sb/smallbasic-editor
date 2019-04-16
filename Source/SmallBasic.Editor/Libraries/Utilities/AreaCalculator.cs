using System;
namespace SmallBasic.Editor.Libraries.Utilities
{
    public class AreaCalculator
    {
        //Method for calculating the area of a triangle with the given vertices
        //Using Heron's method.
        public static double AreaTri(decimal X1, decimal Y1, decimal X2, decimal Y2, decimal X3, decimal Y3)
        {
            double s1 = Math.Sqrt((double)((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1)));
            double s2 = Math.Sqrt((double)((X3 - X2) * (X3 - X2) + (Y3 - Y2) * (Y3 - Y2)));
            double s3 = Math.Sqrt((double)((X1 - X3) * (X1 - X3) + (Y1 - Y3) * (Y1 - Y3)));
            double s = (s1 + s2 + s3) / 2;
            return Math.Sqrt(s * (s - s1) * (s - s2) * (s - s3));

        }

        //Method for calculating the area of a rectangle with the given height and width.
        public static double AreaRect(decimal Height, decimal Width)
        {
            return (double)(Height * Width);
        }

        public static double AreaEllipse(decimal R1, decimal R2)
        {
            return (Math.PI * (double)(R1 / 2) * (double)(R2 / 2));
        }
    }
}
