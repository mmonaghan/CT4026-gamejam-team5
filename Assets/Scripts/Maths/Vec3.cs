using UnityEngine;

namespace Maths {
    [System.Serializable]
    public class Vec3 {
        private static readonly Vec3 vectorUp = new Vec3(0, 1, 0);
        private static readonly Vec3 vectorDown = new Vec3(0, -1, 0);
        private static readonly Vec3 vectorLeft = new Vec3(-1, 0, 0);
        private static readonly Vec3 vectorRight = new Vec3(1, 0, 0);
        private static readonly Vec3 vectorForward = new Vec3(0, 0, 1);
        private static readonly Vec3 vectorBack = new Vec3(0, 0, -1);
        
        public float x, y, z;

        public Vec3() {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vec3(float aX, float aY, float aZ) {
            x = aX;
            y = aY;
            z = aZ;
        }

        public Vec3(Vector3 unityVector3) {
            x = unityVector3.x;
            y = unityVector3.y;
            z = unityVector3.z;
        }

        public static Vec3 operator +(Vec3 a, Vec3 b) {
            return new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
    
        public static Vec3 operator -(Vec3 a, Vec3 b) {
            return new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vec3 operator -(Vec3 a) {
            return new Vec3(-a.x, -a.y, -a.z);
        }

        public static Vec3 operator *(float a, Vec3 b) {
            return new Vec3( a * b.x, a * b.y, a * b.z);
        }
    
        public static Vec3 operator *(Vec3 b, float a) {
            return new Vec3( a * b.x, a * b.y, a * b.z);
        }

        public float Magnitude() {
            return Mathf.Sqrt(x * x + y * y + z * z);
        }

        public void Normalise() {
            float magnitude = Magnitude();
            x = x / magnitude;
            y = y / magnitude;
            z = z / magnitude;
        }

        public static Vec3 CrossProduct(Vec3 a, Vec3 b) {
            // double magnitudeA = a.Magnitude();
            // double magnitudeB = b.Magnitude();
            Vec3 resultA = (a.x * Vec3.vectorRight) + (a.y * Vec3.vectorUp) + (a.z * Vec3.vectorForward);
            Vec3 resultB = (b.x * Vec3.vectorRight) + (b.y * Vec3.vectorUp) + (b.z * Vec3.vectorForward);
            
            return new Vec3();
        }

        public Vector3 ToUnityVec3() {
            return new Vector3(x, y, z);
        }
        
    }
}
