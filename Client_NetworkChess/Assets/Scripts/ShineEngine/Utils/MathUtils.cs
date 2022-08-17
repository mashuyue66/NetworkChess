using System;
using UnityEngine;
using Random = System.Random;

namespace ShineEngine
{
    /// <summary>
    /// ��ѧ����
    /// </summary>
    public static class MathUtils
    {
        //float PI
        public const float fPI = Mathf.PI;
        //float PI*2
        public const float fPI2 = Mathf.PI + Mathf.PI;
        //float PI/2
        public const float fPI_half = Mathf.PI / 2;

        //����ת�Ƕ�
        public const float RadToDeg = 180f / fPI;
        //�Ƕ�ת����
        public const float DegToRad = fPI / 180f;

        private static Random _rand = new Random();

        //float ��ȱȽ�
        public static bool floatEquals(float a, float b)
        {
            if (a == b)
                return true;
            float d = a - b;
            return d > -0.000001f && d < 0.000001f;
        }

        //�Ƿ�2����
        public static bool isPowerOf2(int n)
        {
            return (n & (n - 1)) == 0;
        }

        //ȡ2Ϊ�׶���
        public static int log2(int i)
        {
            int r = 0;
            while ((i >>= 1) != 0) { r++; }
            return r;
        }

        //��ȡ�����ֵ��ֽ�ʹ��λ��
        public static int getIntBitNum(int n)
        {
            return (int)(32 - numberOfLeadingZeros((uint)n));
        }

        public static uint numberOfLeadingZeros(uint i)
        {
            if (i == 0u)
                return 32u;

            uint n = 1u;
            if (i >> 16 == 0u)
            {
                n += 16u;
                i <<= 16;
            }
            if (i >> 24 == 0u)
            {
                n += 8u;
                i <<= 8;
            }
            if (i >> 28 == 0u)
            {
                n += 4u;
                i <<= 4;
            }
            if (i >> 30 == 0u)
            {
                n += 2u;
                i <<= 2;
            }
            n -= i >> 31;
            return n;
        }

        //��ȡ�����ֵ�2����
        public static int getPowerOf2(int n)
        {
            if ((n & (n - 1)) == 0)
                return 1;

            int count = (int)(32U - numberOfLeadingZeros(Convert.ToUInt32(n)));

            return 1 << count;
        }

        //ѭ��ȡģ����
        //(������%���㣬����absMode(-1,3) = 2)
        public static int absMod(int a, int b)
        {
            return a - (int)Math.Floor((float)a / b) * b;
        }

        //ѭ��ȡģ����
        //(������%���㣬����absMode(-1,3) = 2)
        public static float absMod(float a, float b)
        {
            return a - (int)Math.Floor(a / b) * b;
        }

        //����һ�� 0-1 ��double
        public static double random()
        {
            return _rand.NextDouble();
        }

        //��� 0-1 ��float
        public static float randomFloat()
        {
            return (float)_rand.NextDouble();
        }

        //���һ����ȫ������
        public static int randomInt()
        {
            return _rand.Next();
        }

        //���һ����ȫ��long
        public static long randomLong()
        {
            return _rand.Next();
        }

        //����һ������  0<=value<range
        public static int randomInt(int len)
        {
            return _rand.Next(len);
        }

        //����һ������
        public static int randomRange(int start, int end)
        {
            if (end <= start)
                return -1;

            return start + randomInt(end - start);
        }

        //����һ������  start<= value < end
        public static int randomRange2(int start, int end)
        {
            return randomRange(start, end + 1);
        }

        //����һ��float   start <= value < end
        public static float randomRangeF(float start, float end)
        {
            if (end <= start)
                return 0f;

            return start + randomFloat() * (end - start);
        }

        //���һ������
        public static float randomDirection()
        {
            return randomFloat() * fPI - fPI2;
        }

        //������¸���
        public static float randomOffSet(float value, int offset)
        {
            return value - offset + randomInt(offset << 1);
        }

        //������¸���
        public static float randomOffSetF(float value, int offset)
        {
            return value - offset + (randomFloat() * offset * 2);
        }

        //������¸���(ǧ�ֱ�)
        public static int randomOffSetPercent(int value, int offset)
        {
            return (int)(value * (1f + (randomInt(offset << 1) - offset) / 1000f));
        }

        //���bool
        public static bool randomBoolean()
        {
            return _rand.NextDouble() >= 0.5f;
        }

        //�ж�ǧ��λ����
        public static bool randomProb(int prob)
        {
            return randomProb(prob, 1000);
        }

        //�ж�����
        public static bool randomProb(int prob, int max)
        {
            if (prob >= max)
            {
                return true;
            }

            if (prob <= 0)
            {
                return false;
            }

            return randomFloat() < (float)prob / max;
        }

        //�����޻�ȡ
        public static int clamp(int value, int min, int max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        //�����޻�ȡ
        public static float clamp(float value, float min, float max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        //�����޻�ȡ
        public static double clamp(double value, double min, double max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        //--������ͼ�����--//

        //���ȹ鵽 0 - 2PI
        public static float cutRadian(float value)
        {
            while (value < 0)
            {
                value += fPI2;
            }

            while (value > fPI2)
            {
                value -= fPI2;
            }

            return value;
        }

        //����vector3
        public static void copyVec3(ref Vector3 vec, in Vector3 value)
        {
            vec.Set(value.x, value.y, value.z);
        }

        //����ľ���
        public static float distanceBetweenPoint(Vector2 v0, Vector2 v1)
        {
            float dx = v1.x - v0.x;
            float dy = v1.y - v0.y;
            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        //��������
        public static float distanceBetweenPoint(float x0, float y0, float x1, float y1)
        {
            float dx = x1 - x0;
            float dy = y1 - y0;
            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        //���ȹ�λ(-PI   ---    PI)
        public static float directionCut(float direction)
        {
            while (direction > fPI) { direction -= fPI2; }
            while (direction < -fPI) { direction += fPI2; }

            return direction;
        }

        //�Ƕȹ�λ
        public static float angleCut(float angle)
        {
            while(angle > 180) { angle -= 360; }
            while(angle < -180) { angle += 360; }

            return angle;
        }

        //����
        public static float directionInverse(float direction)
        {
            return directionCut(direction + fPI);
        }

        //����ת�Ƕ�  ��cut
        public static float directionToAngle(float direction)
        {
            return angleCut(direction * RadToDeg);
        }

        //�Ƕ�ת����  ��cut
        public static float angleToDirection(float angle)
        {
            return directionCut(angle * DegToRad);
        }

        //������ ����1  �� ����2  ֮�� ���� cut
        public static float getDirectionD(float direction0, float direction1)
        {
            return angleCut(direction1 - direction0);
        }

        //������ ����1  �� ����2  ֮�����ֵ
        public static float getDirectionDAbs(float direction0, float direction1)
        {
            return Math.Abs(angleCut(direction1 - direction0));
        }


        public static int intCompare(int x, int y)
        {
            return (x < y) ? -1 : ((x == y) ? 0 : 1);
        }

        public static int doubleCompare(double x, double y)
        {
            return (x < y) ? -1 : ((x == y) ? 0 : 1);
        }

        public static int longCompare(long x, long y)
        {
            return (x < y) ? -1 : ((x == y) ? 0 : 1);
        }

        //�� pos ������rect ��
        public static void makePosInRect(ref Vector2 pos, Rect rect)
        {
            if(pos.x < rect.x)
                pos.x = rect.x;

            if(pos.x > rect.xMax)
                pos.x = rect.xMax;

            if(pos.y < rect.y)
                pos.y = rect.y;

            if(pos.y > rect.yMax)
                pos.y = rect.yMax;
        }

        //������ͬ
        public static bool sameSymbol(float a, float b)
        {
            return (a < 0 && b < 0) || (a > 0 && b > 0);
        }

        public static bool doubleIsFinish(double d)
        { 
            return (BitConverter.DoubleToInt64Bits(d) & long.MaxValue) < 9218868437227405312L; 
        }

        public static unsafe bool floatInFinite(float d)
        {
            int a = *(int*)&d;

            return (a & int.MaxValue) < 2139095040;
        }
    }
}