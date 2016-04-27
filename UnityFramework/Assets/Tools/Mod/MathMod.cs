using UnityEngine;
using System.Collections;

namespace SPSGame.Tools
{
    public class MathMod
    {

        public static int Min(int p1, int p2)
        {
            return p1 <= p2 ? p1 : p2;
        }

        public static int Max(int p1, int p2)
        {
            return p1 >= p2 ? p1 : p2;
        }

        public static float Min(float p1, float p2)
        {
            return p1 <= p2 ? p1 : p2;
        }

        public static float Max(float p1, float p2)
        {
            return p1 >= p2 ? p1 : p2;
        }

        public static double Min(double p1, double p2)
        {
            return p1 <= p2 ? p1 : p2;
        }

        public static double Max(double p1, double p2)
        {
            return p1 >= p2 ? p1 : p2;
        }

        static public Vector3 Round(Vector3 v)
        {
            v.x = Mathf.Round(v.x);
            v.y = Mathf.Round(v.y);
            v.z = Mathf.Round(v.z);
            return v;
        }


        static public float VectorToAngle( Vector2 dir, bool inDegree = true)
        {
            float angle = Mathf.Atan2(dir.x, dir.y);

            if (inDegree)
            {
                return angle * Mathf.Rad2Deg;
            }
            else
            {
                return angle;
            }
        }

        static public bool Near(float param1,float param2)
        {
            return Mathf.Abs(param1- param2) < 0.01f;
        }

        public static Quaternion Slerp(Quaternion from, Quaternion to, float t)
        {
            Quaternion ret;

            float fCos = Quaternion.Dot(from, to);

            if ((1f + fCos) > 0.00001)
            {
                float fCoeff0, fCoeff1;

                if ((1f - fCos) > 0.00001)
                {
                    float omega = Mathf.Acos(fCos);
                    float invSin = 1f / Mathf.Sin(omega);
                    fCoeff0 = Mathf.Sin((1f - t) * omega) * invSin;
                    fCoeff1 = Mathf.Sin(t * omega) * invSin;
                }
                else
                {
                    fCoeff0 = 1f - t;
                    fCoeff1 = t;
                }

                ret.x = fCoeff0 * from.x + fCoeff1 * to.x;
                ret.y = fCoeff0 * from.y + fCoeff1 * to.y;
                ret.z = fCoeff0 * from.z + fCoeff1 * to.z;
                ret.w = fCoeff0 * from.w + fCoeff1 * to.w;
            }
            else
            {
                float fCoeff0 = Mathf.Sin((1f - t) * Mathf.PI * .5f);
                float fCoeff1 = Mathf.Sin(t * Mathf.PI * .5f);

                ret.x = fCoeff0 * from.x - fCoeff1 * from.y;
                ret.y = fCoeff0 * from.y + fCoeff1 * from.x;
                ret.z = fCoeff0 * from.z - fCoeff1 * from.w;
                ret.w = from.z;
            }

            return ret;
        }
    }
}