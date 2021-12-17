using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace SZ_BydKeyboard
{

    public static class HeightMeasure
    {
        public static void GenMeasurePoints(double CurrentX, double CurrentY,
                double[] OffsetX, double[] OffsetY, out List<double> MeasurePointX, out List<double> MeasurePointY)
        {
            MeasurePointX = new List<double> { };
            MeasurePointY = new List<double> { };
            for (int i = 0; i < OffsetX.Length; i++)
            {
                MeasurePointX.Add(CurrentX - OffsetX[i] + Common.Data.OffsetAltimeterToCameraX);
                MeasurePointY.Add(CurrentY - OffsetY[i] + Common.Data.OffsetAltimeterToCameraY);
            }
        }

        public static void MeasurePlat2Plat(List<double> X, List<double> Y, List<double> Z, out double Height)
        {
            List<Point3D> up = new List<Point3D>();
            List<Point3D> down = new List<Point3D>();
            for (int i = 0; i < 3; i++)
            {
                up.Add(new Point3D() { x = X[i], y = Y[i], z = Z[i] });
                down.Add(new Point3D() { x = X[i + 3], y = Y[i + 3], z = Z[i + 3] });
            }
            Height = DistancePlaneToPlane(up, down);
        }


        /// <summary>
        /// 获取一个点到3个点拟合的平面的距离,每个点都是一个数组，包括x,y,z坐标
        /// </summary>
        /// <param name="a">平面点A</param>
        /// <param name="b">平面点B</param>
        /// <param name="c">平面点C</param>
        /// <param name="H">平面外点H</param>
        /// <returns></returns>
        public static double GetHeight(float[] a, float[] b, float[] c, float[] H)
        {
            int[] que = new int[3];
            for (int i = 0; i < que.Length; i++)
            {
                que[i] = i;
            }
            double var = 0;
            double[] ce = new double[4];

            float[] x = new float[3];
            float[] y = new float[3];
            float[] z = new float[3];

            TransferRowToColumn(a, b, c, ref x, ref y, ref z);

            Calculate(x, y, z, H[0], H[1], H[2], que, 3, ref var, ref ce);

            double h = (ce[0] * H[0] + ce[1] * H[1] + ce[3]) / (-ce[2]);

            return (H[2] - h);
        }

        /// <summary>
        /// 行列转换
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void TransferRowToColumn(float[] a, float[] b, float[] c, ref float[] x, ref float[] y, ref float[] z)
        {
            x[0] = a[0];
            x[1] = b[0];
            x[2] = c[0];
            y[0] = a[1];
            y[1] = b[1];
            y[2] = c[1];
            z[0] = a[2];
            z[1] = b[2];
            z[2] = c[2];
        }

        private static void GenerateCoef(float[] xn, float[] yn, float[] zn, ref double[] coef)
        {
            coef[0] = (yn[1] - yn[0]) * (zn[2] - zn[0])
                - (zn[1] - zn[0]) * (yn[2] - yn[0]);

            coef[1] = (zn[1] - zn[0]) * (xn[2] - xn[0])
                - (xn[1] - xn[0]) * (zn[2] - zn[0]);

            coef[2] = (xn[1] - xn[0]) * (yn[2] - yn[0])
                - (yn[1] - yn[0]) * (xn[2] - xn[0]);

            coef[3] = zn[0] * ((yn[1] - yn[0]) * (xn[2] - xn[0]) - (xn[1] - xn[0]) * (yn[2] - yn[0]))
                + xn[0] * ((zn[1] - zn[0]) * (yn[2] - yn[0]) - (yn[1] - yn[0]) * (zn[2] - zn[0]))
                + yn[0] * ((xn[1] - xn[0]) * (zn[2] - zn[0]) - (zn[1] - zn[0]) * (xn[2] - xn[0]));
        }

        public static double Calculate(float[] x, float[] y, float[] z, float xN, float yN, float zN, int[] pointSerial,
    int pointNum, ref double var, ref double[] coef)
        {
            float[] xn = new float[3];
            float[] yn = new float[3];
            float[] zn = new float[3];

            for (int i = 0; i < 3; i++)
            {
                xn[i] = x[pointSerial[i]];
                yn[i] = y[pointSerial[i]];
                zn[i] = z[pointSerial[i]];
            }

            GenerateCoef(xn, yn, zn, ref coef);

            double denom = System.Math.Sqrt(coef[0] * coef[0] + coef[1] * coef[1] + coef[2] * coef[2]);

            var = (coef[0] * xN + coef[1] * yN + coef[2] * zN + coef[3]) / denom;
            /*
			if (false)
            {
                return WikiStandard(x, y, z, coef, pointNum, ref var);
            }
            else
            {
                return GBStandard(x, y, z, coef, pointNum, ref var);
            }
			*/

            return var;
        }

        public struct Point3D
        {
            public double x;
            public double y;
            public double z;
        }

        public static double DistancePlaneToPlane(List<Point3D> upPointList, List<Point3D> downPointList)
        {
            int size1 = upPointList.Count;
            double[] x_up = new double[size1];
            double[] y_up = new double[size1];
            double[] z_up = new double[size1];
            for (int i = 0; i < upPointList.Count; i++)
            {
                x_up[i] = upPointList[i].x;
                y_up[i] = upPointList[i].y;
                z_up[i] = upPointList[i].z;
            }

            int size2 = downPointList.Count;
            double[] x_down = new double[size1];
            double[] y_down = new double[size1];
            double[] z_down = new double[size1];
            for (int i = 0; i < downPointList.Count; i++)
            {
                x_down[i] = downPointList[i].x;
                y_down[i] = downPointList[i].y;
                z_down[i] = downPointList[i].z;
            }

            double[] upBestPlaneParam = PanelFit(x_up, y_up, z_up);
            double[] downBestPlaneParam = PanelFit(x_down, y_down, z_down);

            double xMax = upPointList[0].x;
            double xMin = upPointList[0].x;
            double yMax = upPointList[0].y;
            double yMin = upPointList[0].y;

            for (int i = 1; i < upPointList.Count(); ++i)
            {
                if (xMax < upPointList[i].x)
                {
                    xMax = upPointList[i].x;
                }
                if (xMin > upPointList[i].x)
                {
                    xMin = upPointList[i].x;
                }
                if (yMax < upPointList[i].y)
                {
                    yMax = upPointList[i].y;
                }
                if (yMin > upPointList[i].y)
                {
                    yMin = upPointList[i].y;
                }
            }

            double centerX = (xMax + xMin) / 2.0;
            double centerY = (yMax + yMin) / 2.0;

            double upZ = -(upBestPlaneParam[0] * centerX + upBestPlaneParam[1] * centerY + upBestPlaneParam[3]) / upBestPlaneParam[2];
            double downZ = -(downBestPlaneParam[0] * centerX + downBestPlaneParam[1] * centerY + downBestPlaneParam[3]) / downBestPlaneParam[2];

            double disWithPlane = Math.Abs(upZ - downZ);
            return disWithPlane;
        }

        /// <summary>
        /// 平面方程拟合，ax+by+cz+d=0,其中a=result[0],b=result[1],c=result[2],d=result[3]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double[] PanelFit(double[] x, double[] y, double[] z)
        {
            double[] result = new double[4];
            int n = x.Length;
            double[,] A = new double[n, 3];
            double[,] E = new double[n, 1];
            for (int i = 0; i < n; i++)
            {
                A[i, 0] = x[i] - z[i];
                A[i, 1] = y[i] - z[i];
                A[i, 2] = 1;
                E[i, 0] = -z[i];
            }
            double[,] AT = MatrixInver(A);
            double[,] ATxA = MatrixMultiply(AT, A);
            double[,] OPPAxTA = MatrixOpp(ATxA);
            double[,] OPPATAxAT = MatrixMultiply(OPPAxTA, AT);
            double[,] DP = MatrixMultiply(OPPATAxAT, E);
            result[0] = DP[0, 0];
            result[1] = DP[1, 0];
            result[2] = 1 - result[0] - result[1];
            result[3] = DP[2, 0];
            return result;
        }

        /// <summary>
        /// 矩阵转置
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static double[,] MatrixInver(double[,] matrix)
        {
            double[,] result = new double[matrix.GetLength(1), matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(1); i++)
                for (int j = 0; j < matrix.GetLength(0); j++)
                    result[i, j] = matrix[j, i];
            return result;
        }
        /// <summary>
        /// 矩阵相乘
        /// </summary>
        /// <param name="matrixA"></param>
        /// <param name="matrixB"></param>
        /// <returns></returns>
        private static double[,] MatrixMultiply(double[,] matrixA, double[,] matrixB)
        {
            double[,] result = new double[matrixA.GetLength(0), matrixB.GetLength(1)];
            for (int i = 0; i < matrixA.GetLength(0); i++)
            {
                for (int j = 0; j < matrixB.GetLength(1); j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < matrixB.GetLength(0); k++)
                    {
                        result[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 矩阵的逆
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static double[,] MatrixOpp(double[,] matrix)
        {
            double X = 1 / MatrixSurplus(matrix);
            double[,] matrixB = new double[matrix.GetLength(0), matrix.GetLength(1)];
            double[,] matrixSP = new double[matrix.GetLength(0), matrix.GetLength(1)];
            double[,] matrixAB = new double[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    for (int m = 0; m < matrix.GetLength(0); m++)
                        for (int n = 0; n < matrix.GetLength(1); n++)
                            matrixB[m, n] = matrix[m, n];
                    {
                        for (int x = 0; x < matrix.GetLength(1); x++)
                            matrixB[i, x] = 0;
                        for (int y = 0; y < matrix.GetLength(0); y++)
                            matrixB[y, j] = 0;
                        matrixB[i, j] = 1;
                        matrixSP[i, j] = MatrixSurplus(matrixB);
                        matrixAB[i, j] = X * matrixSP[i, j];
                    }
                }
            return MatrixInver(matrixAB);
        }
        /// <summary>
        /// 矩阵的行列式的值  
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static double MatrixSurplus(double[,] matrix)
        {
            double X = -1;
            double[,] a = matrix;
            int i, j, k, p, r, m, n;
            m = a.GetLength(0);
            n = a.GetLength(1);
            double temp = 1, temp1 = 1, s = 0, s1 = 0;

            if (n == 2)
            {
                for (i = 0; i < m; i++)
                    for (j = 0; j < n; j++)
                        if ((i + j) % 2 > 0) temp1 *= a[i, j];
                        else temp *= a[i, j];
                X = temp - temp1;
            }
            else
            {
                for (k = 0; k < n; k++)
                {
                    for (i = 0, j = k; i < m && j < n; i++, j++)
                        temp *= a[i, j];
                    if (m - i > 0)
                    {
                        for (p = m - i, r = m - 1; p > 0; p--, r--)
                            temp *= a[r, p - 1];
                    }
                    s += temp;
                    temp = 1;
                }

                for (k = n - 1; k >= 0; k--)
                {
                    for (i = 0, j = k; i < m && j >= 0; i++, j--)
                        temp1 *= a[i, j];
                    if (m - i > 0)
                    {
                        for (p = m - 1, r = i; r < m; p--, r++)
                            temp1 *= a[r, p];
                    }
                    s1 += temp1;
                    temp1 = 1;
                }

                X = s - s1;
            }
            return X;
        }





    }
}
