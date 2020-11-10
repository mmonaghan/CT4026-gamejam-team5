using UnityEngine;

namespace Maths {
    public class Vec2 {
        public float x, y;
        
        public Vec2() {
            x = 0;
            y = 0;
        }

        public Vec2(float aX, float aY) {
            x = aX;
            y = aY;
        }

        public Vector3 ToVec3() {
            return new Vector3(x, y, 0);
        }

        public static Vec2 operator +(Vec2 a, Vec2 b) {
            return new Vec2(a.x + b.x, a.y + b.y);
        }

        public static Vec2 operator -(Vec2 a) {
            return new Vec2(-a.x, -a.y);
        }

        public static Vec2 operator -(Vec2 a, Vec2 b) {
            return new Vec2(a.x - b.x, a.y - b.y);
        }
        
        // public static Vec2 operator *(float a, Vec2 b) {
        //     return new Vec2(b.x * a, b.y * a);
        // }
        
        public static Vec2 operator *(Vec2 b, float a) {
            return new Vec2(b.x * a, b.y * a);
        }

        public float Magnitude() {
            return Mathf.Sqrt(x * x + y * y);
        }

        public void Normalise() {
            float magnitude = Magnitude();
            x = x / magnitude;
            y = y / magnitude;
        }

        public static float DotProduct(Vec2 a, Vec2 b) {
            return a.x * b.x +  a.y * b.y;
        }

        public Vec2 Perp() {
            return new Vec2(y, -x);
        }
    }
}