using UnityEngine;

public class Marker : MonoBehaviour {
    [SerializeField] float m_RotationSpeed;
    [SerializeField] float m_MaxScaleFactor;
    [SerializeField] float m_MinScaleFactor;
    [SerializeField] float m_ScaleSpeed;

    Renderer m_Renderer;

    private void Awake() {
        m_Renderer = GetComponent<Renderer>();
    }

    public void Set(bool flag) {
        m_Renderer.enabled = flag;
    }

    void Update() {
        Rotate();
        Scale();
    }

    void Rotate() {
        transform.Rotate(new Vector3(0, 0, m_RotationSpeed));
    }

    void Scale() {
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            (m_MinScaleFactor + Mathf.PingPong(Time.time * m_ScaleSpeed, m_MaxScaleFactor - m_MinScaleFactor))
        );
    }

}
