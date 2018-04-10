﻿using UnityEngine;

namespace UniGenVR.Player {
    [ExecuteInEditMode]
    public class PlayerHead : MonoBehaviour {
        PlayerBody m_Body;

        void OnEnable() {
            if (m_Body == null) {
                m_Body = GameObject.FindObjectOfType<PlayerBody>();
                m_Body.headTransform = transform;
            }
        }

        void Update() {
            transform.position = m_Body.transform.position + Vector3.up * m_Body.height;
            var bodyOrientation = m_Body.transform.eulerAngles;
            m_Body.transform.eulerAngles = new Vector3(
                bodyOrientation.x,
                transform.eulerAngles.y,
                bodyOrientation.z
            );
        }
    }
}