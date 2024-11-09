using UnityEngine;

public class KayakController : MonoBehaviour
{
    public float moveSpeed = 5f;           // Velocidad de movimiento del kayak
    public float turnSpeed = 20f;          // Velocidad de giro del kayak
    public float slightForwardSpeed = 2f;  // Pequeño avance al girar
    public float brakeTorque = 2f;         // Factor de freno para reducir el giro residual suavemente
    public Vector3 controlOsc;             // Vector que almacena los datos OSC
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.8f;                    // Drag ajustado para una inercia moderada en el avance
        rb.angularDrag = 2f;               // Angular drag ajustado para una inercia moderada en el giro
    }

    void FixedUpdate()
    {
        // Calcula el avance basado en la inclinación en el eje X, avanzando solo si está en el rango específico
        float moveInput = 0f;

        if (controlOsc.x > 20 && controlOsc.x <= 50)         // Inclinado hacia adelante
        {
            moveInput = (controlOsc.x - 20) / 30f;           // Normaliza para que esté entre 0 y 1
        }
        else if (controlOsc.x < -20 && controlOsc.x >= -50)  // Inclinado hacia atrás
        {
            moveInput = (controlOsc.x + 20) / 30f;           // Normaliza para que esté entre 0 y -1
        }

        // Calcula el giro basado en el movimiento en el eje Z
        float turnInput = Mathf.Clamp(controlOsc.z / 20f, -1f, 1f);  // Normaliza para que esté entre -1 y 1

        // Movimiento hacia adelante en función de la inclinación
        Vector3 forwardMovement = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMovement);

        // Si hay giro significativo, aplica el torque de giro
        if (Mathf.Abs(turnInput) > 0.05f)  // Zona muerta más ajustada para evitar giros accidentales
        {
            float torque = turnInput * turnSpeed;
            rb.angularVelocity = new Vector3(0, torque, 0);  // Asignar directamente la velocidad angular en Y
        }
        else
        {
            // Desaceleración suave del giro cuando no hay entrada de giro
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, brakeTorque * Time.fixedDeltaTime);
        }
    }

    // Método para asignar el vector de control OSC desde otro script
    public void asignarVector(Vector3 v) 
    {
        controlOsc = v;
    }
}






// using UnityEngine;

// public class KayakController : MonoBehaviour
// {
//     public float moveSpeed = 5f;      // Velocidad de movimiento del kayak
//     public float turnSpeed = 20f;     // Velocidad de giro del kayak
//     public float slightForwardSpeed = 2f;  // Pequeño avance al girar
//     public Vector3 controlOsc;
//     private Rigidbody rb;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//     }

//     void FixedUpdate()
//     {
//         // Captura el movimiento del kayak
//         float moveInput = Input.GetAxis("Vertical");     // Entrada de avance (W/S o flechas arriba/abajo)
//         float turnInput = Input.GetAxis("Horizontal");   // Entrada de giro (A/D o flechas izquierda/derecha)

//         // Calcula el movimiento hacia adelante
//         Vector3 forwardMovement = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
//         rb.MovePosition(rb.position + forwardMovement);

//         // Aplica una leve fuerza de avance si hay giro
//         if (turnInput != 0)
//         {
//             Vector3 slightForwardMovement = transform.forward * slightForwardSpeed * Time.fixedDeltaTime;
//             rb.MovePosition(rb.position + slightForwardMovement);
//         }

//         // Calcula y aplica la rotación del kayak
//         float torque = turnInput * turnSpeed * Time.fixedDeltaTime;
//         rb.AddTorque(0f, torque, 0f, ForceMode.VelocityChange); // Añadir torque sobre el eje Y
//     }

//     public void asignarVector(Vector3 v) {
//         controlOsc = v;
//     }
// }
