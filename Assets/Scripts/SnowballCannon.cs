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
        [SerializeField]
        private float m_timeSinceLastStep = 0.0f;
        
        [SerializeField]
        private float m_minStepTime;

        private GameObject backupCannonball;
        private bool isPlayerCannonball = false;
        
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
            if(Input.GetKeyDown(KeyCode.F))
                Fire();
            if (Input.GetKeyDown(KeyCode.O)) {
                m_cannonPower++;
            }
            if (Input.GetKeyDown(KeyCode.P)) {
                m_cannonPower--;
            }
            
            if (Input.GetKey(KeyCode.LeftBracket))
                transform.rotation = transform.rotation * Quaternion.Euler(0, Time.deltaTime * -50, 0);
            if (Input.GetKey(KeyCode.RightBracket))
                transform.rotation = transform.rotation * Quaternion.Euler(0, Time.deltaTime * 50, 0);
            
            if (m_isLaunchInProgress) {
                m_cannonBall.transform.parent = this.gameObject.transform;
                m_remainingAirTime -= Time.deltaTime;
                CalculateLaunchStep(Time.deltaTime);
                m_cannonBall.transform.localPosition = m_projectileTrajectoryPoints[m_currentLaunchStep].ToUnityVec3();
                // m_cannonBall.transform.rotation = transform.rotation;

                if (m_currentLaunchStep >= m_totalProjectileSteps) {
                    m_isLaunchInProgress = false;
                    // m_cannonBall.transform.parent = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.RightAlt)) {
                isPlayerCannonball = !isPlayerCannonball;
                FirePlayer();
            }
        }

        private void FirePlayer() {
            if (isPlayerCannonball) {
                backupCannonball = m_cannonBall;
                m_cannonBall = GameObject.FindWithTag("Player");
                m_cannonBall.GetComponent<Rigidbody>().Sleep();
            }
            else {
                m_cannonBall = backupCannonball;
            }
            
            
        }
        
        private void OnGUI() {
            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = new Color(0f, 0.6f, 0.737f);
            GUI.Label(new Rect(50, 50, 150, 50), "Controls", style);
            GUI.Label(new Rect(50, 70, 150, 50), "[ = Rotate Cannon left", style);
            GUI.Label(new Rect(50, 90, 150, 50), "] = Rotate Cannon right", style);
            GUI.Label(new Rect(50, 110, 150, 50), "F = Fire Cannon", style);
            GUI.Label(new Rect(50, 130, 150, 50), "O = Increase power of cannon", style);
            GUI.Label(new Rect(50, 150, 150, 50), "P = Decrease power of cannon", style);
            
            GUI.Label(new Rect(50, 200, 150, 50), "Current Cannon Power: " + m_cannonPower, style);
            GUI.Label(new Rect(50, 220, 150, 150), "Cannon is ready to fire: " + (!m_isLaunchInProgress).ToString(), style);
        }

        private void Fire() {
            if (!m_isLaunchInProgress) {
                m_isLaunchInProgress = true;
                m_currentLaunchStep = 0;
                m_remainingAirTime = m_projectileAirTime;
                m_cannonBall.transform.parent = transform;
                m_cannonBall.transform.position = Vector3.zero;
                m_cannonBall.transform.rotation = transform.rotation;
                
                CalculateAirTimeAndDisplacement();
                CalculateTrajectory();
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