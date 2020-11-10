using System;
using System.Collections.Generic;
using Maths;
using UnityEngine;

namespace DefaultNamespace {
    public class SnowballCannon : MonoBehaviour {
        [SerializeField]
        private List<Vec3> m_projectileTrajectoryPoints;
        [SerializeField]
        private float m_cannonPower = 5.0f;
        [SerializeField]
        private float m_cannonLaunchAngleDegrees = 45.0f;

        private float m_projectileAirTime = 0.0f;
        private float m_projectileXDisplacement = 0.0f;
        private bool m_isLaunchInProgress = false;
        private int m_currentLaunchStep = 0;
        
        [SerializeField] private int m_totalProjectileSteps = 50; 
        [SerializeField] private GameObject m_cannonBall;
        
        private readonly Vec3 _gravityAcceleration = new Vec3(0, -9.8f, 0);
        private Vec3 m_currentAcceleration = new Vec3();
        private Vec3 m_initialVelocity = new Vec3();

        private float m_remainingAirTime = 0.0f;
        private float m_timeSinceLastStep = 0.0f;
        
        float m_minStepTime;
        
        private void Start() {
            m_minStepTime = m_projectileAirTime / m_totalProjectileSteps;
            // m_cannonBall = transform.Find("Cannonball").gameObject;
            
            CalculateAirTimeAndDisplacement();
            CalculateTrajectory();
            DrawPath();
        }

        private void OnValidate() {
            m_minStepTime = m_projectileAirTime / m_totalProjectileSteps;
            CalculateAirTimeAndDisplacement();
            CalculateTrajectory();
            DrawPath();
        }

        private void Update() {
            
            
            if(Input.GetKeyDown(KeyCode.Space))
                Fire();
            
            if (m_isLaunchInProgress) {
                m_remainingAirTime -= Time.deltaTime;
                CalculateLaunchStep(Time.deltaTime);
                m_cannonBall.transform.position = m_projectileTrajectoryPoints[m_currentLaunchStep].ToUnityVec3();

                if (m_currentLaunchStep >= m_totalProjectileSteps) {
                    m_isLaunchInProgress = false;
                }
            }
        }

        private void Fire() {
            if (!m_isLaunchInProgress) {
                m_isLaunchInProgress = true;
                m_currentLaunchStep = 0;
                m_remainingAirTime = m_projectileAirTime;
                m_cannonBall.transform.position = Vector3.zero;
            }
        }

        private void CalculateLaunchStep(float deltaTime) {
            m_timeSinceLastStep += deltaTime;

            if (m_timeSinceLastStep >= m_minStepTime) {
                m_currentLaunchStep++;
                m_timeSinceLastStep = 0.0f;
            }
        }

        private void CalculateTrajectory() {
            m_projectileTrajectoryPoints = new List<Vec3>();
            Vec3 launchPos = new Vec3(m_cannonBall.transform.position);
            
            m_projectileTrajectoryPoints.Add(launchPos);

            for (int i = 0; i < m_totalProjectileSteps; i++) {
                float simTime = (i / (float) m_totalProjectileSteps) * m_projectileAirTime;
                Vec3 displacement = m_initialVelocity * simTime + _gravityAcceleration * simTime * simTime * 0.5f;
                Vec3 drawPoint = launchPos + displacement;
                drawPoint.x += m_initialVelocity.x * simTime;
                m_projectileTrajectoryPoints.Add(drawPoint);
            }
        }

        private void CalculateAirTimeAndDisplacement() {
            m_initialVelocity.x = m_cannonPower * Mathf.Cos(m_cannonLaunchAngleDegrees * Mathf.Deg2Rad);
            m_initialVelocity.y = m_cannonPower * Mathf.Sin(m_cannonLaunchAngleDegrees * Mathf.Deg2Rad);

            m_projectileAirTime = 2f * (0 - m_initialVelocity.y) / _gravityAcceleration.y;
            m_projectileXDisplacement = m_projectileAirTime * m_initialVelocity.x;
        }
        
        private void DrawPath() {
            for (int i = 0; i < m_projectileTrajectoryPoints.Count - 1; ++i) {
                Debug.DrawLine(m_projectileTrajectoryPoints[i].ToUnityVec3(), m_projectileTrajectoryPoints[i + 1].ToUnityVec3(), Color.blue);
            }
        }
    }
}