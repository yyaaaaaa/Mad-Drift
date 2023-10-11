using UnityEngine;
public class ArcadeVehicleController : MonoBehaviour
{
    public enum WhoControls { Player, AI };
    public WhoControls whoControls;
    public LayerMask drivableSurface;
    public GameObject player;
    public float MaxSpeed, accelaration, turn, gravity = 7f, downforce = 5f;
    public bool AirControl = false;
    public Rigidbody rb, carBody;

    [HideInInspector]
    public RaycastHit hit;
    public AnimationCurve frictionCurve;
    public AnimationCurve turnCurve;
    public PhysicMaterial frictionMaterial;
    [Header("Visuals")]
    public Transform BodyMesh;
    public Transform[] FrontWheels = new Transform[2];
    public Transform[] RearWheels = new Transform[2];
    [HideInInspector]
    public Vector3 carVelocity;

    [Range(0, 10)]
    public float BodyTilt;
    [Header("Audio settings")]
    public AudioSource engineSound;
    [Range(0, 1)]
    public float minPitch;
    [Range(1, 3)]
    public float MaxPitch;
    public AudioSource SkidSound;

    [HideInInspector]
    public float skidWidth;


    private float radius, horizontalInput, verticalInput;
    private Vector3 origin;
    private float screen;   

    private void Start()
    {
        screen = Screen.width;
        radius = rb.GetComponent<SphereCollider>().radius;
    }
    private void Update()
    {
        if (whoControls == WhoControls.Player)
        {
            horizontalInput = 0f;
            if (Input.touchCount > 0) 
            { 
                if (Input.GetTouch(0).position.x < screen / 2)
                {
                    horizontalInput = -1f;
                }
                if (Input.GetTouch(0).position.x > screen / 2)
                {
                    horizontalInput = 1f;
                }   
            }
            verticalInput = 1f;     //accelaration input
        }
        if (whoControls == WhoControls.AI)
        {
            HandleAIInput();
        }
        Visuals();
        AudioManager();

    }
    public void AudioManager()
    {
        if (whoControls == WhoControls.Player)
        {
            engineSound.pitch = Mathf.Lerp(minPitch, MaxPitch, Mathf.Abs(carVelocity.z) / MaxSpeed);
            if (Mathf.Abs(carVelocity.x) > 10 && grounded())
            {
                SkidSound.mute = false;
            }
            else
            {
                SkidSound.mute = true;
            }
        }
    }


    void FixedUpdate()
    {
        carVelocity = carBody.transform.InverseTransformDirection(carBody.velocity);

        if (Mathf.Abs(carVelocity.x) > 0)
        {
            //changes friction according to sideways speed of car
            frictionMaterial.dynamicFriction = frictionCurve.Evaluate(Mathf.Abs(carVelocity.x / 100));
        }


        if (grounded())
        {
            //turnlogic
            float sign = Mathf.Sign(carVelocity.z);
            float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / MaxSpeed);
            if (verticalInput > 0.1f || carVelocity.z > 1)
            {
                carBody.AddTorque(Vector3.up * horizontalInput * sign * turn * 100 * TurnMultiplyer);
            }
            else if (verticalInput < -0.1f || carVelocity.z < -1)
            {
                carBody.AddTorque(Vector3.up * horizontalInput * sign * turn * 100 * TurnMultiplyer);
            }

            if (Mathf.Abs(verticalInput) > 0.1f && Input.GetAxis("Jump") < 0.1f)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, carBody.transform.forward * verticalInput * MaxSpeed, accelaration / 10 * Time.deltaTime);
            }
        }

        //body tilt
        carBody.MoveRotation(Quaternion.Slerp(carBody.rotation, Quaternion.FromToRotation(carBody.transform.up, hit.normal) * carBody.transform.rotation, 0.12f));
    }


    public void Visuals()
    {
        //tires
        foreach (Transform FW in FrontWheels)
        {
            FW.localRotation = Quaternion.Slerp(FW.localRotation, Quaternion.Euler(FW.localRotation.eulerAngles.x,
                               30 * horizontalInput, FW.localRotation.eulerAngles.z), 0.1f);
            FW.GetChild(0).localRotation = rb.transform.localRotation;
        }
        RearWheels[0].localRotation = rb.transform.localRotation;
        RearWheels[1].localRotation = rb.transform.localRotation;

        //Body
        if (carVelocity.z > 1)
        {
            BodyMesh.localRotation = Quaternion.Slerp(BodyMesh.localRotation, Quaternion.Euler(Mathf.Lerp(0, -5, carVelocity.z / MaxSpeed),
                               BodyMesh.localRotation.eulerAngles.y, BodyTilt * horizontalInput), 0.05f);
        }
        else
        {
            BodyMesh.localRotation = Quaternion.Slerp(BodyMesh.localRotation, Quaternion.Euler(0, 0, 0), 0.05f);
        }


    }

    public bool grounded() //checks for if vehicle is grounded or not
    {
        origin = rb.position + rb.GetComponent<SphereCollider>().radius * Vector3.up;
        var direction = -transform.up;
        var maxdistance = rb.GetComponent<SphereCollider>().radius + 0.2f;
        if (Physics.SphereCast(origin, radius + 0.1f, direction, out hit, maxdistance, drivableSurface))
        {
            return true;

        }
        else
        {
            return false;
        }

    }
    private void HandleAIInput()
    {
        // ѕолучаем позицию игрока
        Vector3 playerPosition = player.transform.position;

        // ¬ычисл€ем направление к игроку
        Vector3 directionToPlayer = playerPosition - transform.position;
        directionToPlayer.Normalize();

        // –ассчитываем угол между направлением движени€ автомобил€ и направлением к игроку
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // ”станавливаем горизонтальный ввод (поворот автомобил€)
        horizontalInput = angleToPlayer > 10f ? Mathf.Sign(Vector3.Dot(transform.right, directionToPlayer)) : 0f;

        // ”станавливаем вертикальный ввод (ускорение)
        verticalInput = 1f; // ƒвигаемс€ вперед

        // ¬ычисл€ем рассто€ние до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        // ≈сли рассто€ние меньше некоторого порога, то включаем "режим атаки" и пытаемс€ врезатьс€ в игрока
        if (distanceToPlayer < 10f)
        {
            // ћен€ем горизонтальный ввод, чтобы направитьс€ пр€мо в игрока
            horizontalInput = Mathf.Sign(Vector3.Dot(transform.right, directionToPlayer));

            // ≈сли игрок слишком близко, можем использовать торможение или задний ход
            if (distanceToPlayer < 5f)
            {
                verticalInput = -1f; // “ормозим или двигаемс€ назад
            }
        }
    }
}

