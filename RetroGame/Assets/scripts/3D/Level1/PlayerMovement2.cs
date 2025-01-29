using Cinemachine;
using DG.Tweening;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement2 : MonoBehaviour
{
    [Header("Ship parameters")]
    [ Space ]
    public float xySpeed = 8;
    public float lookSpeed = 30f;
    public float forwardSpeed = 6;
    public float turnSpeed = 60f;
    public float maxAngle = 30.0f;
    public float rotationAmount = 75f;
    //public CinemachineDollyCart dolly;
    [ Space ]
    [Header("Others")]
    public Transform cameraParent;
    public int invert = -1;
    private Transform playerModel;
    public Transform aimTargetObject;
    public CinemachineDollyCart dolly;
    private Transform aimTarget;
    private float originalFov;

    [Space]
    [Header("Boost")]

    public float boostAmount = 100; // Quantidade inicial de boost
    public float boostCost = 10f; // Custo por uso do boost
    public float boostRechargeRate = 5f; // Taxa de recarga do boost
    public bool totalCharge = true;
    public bool isBoosting = false;

    private float rotationSpeed = 60f;
    private float maxRotation = 60f;
    private float currentRotation = 0f;
    private float inputDelay = 1f;
    private float lastInputTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        playerModel = transform.GetChild(0);
        SetSpeed(forwardSpeed);
        originalFov = Camera.main.fieldOfView;
        aimTarget = aimTargetObject.transform;
        //Debug.Log(originalFov.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerModel != null)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontal, invert * vertical, 0);
            LocalMove(horizontal, vertical, xySpeed);
            RotationLookPath(horizontal, vertical, lookSpeed);
        
            HorizontalLean(playerModel, horizontal, 50, .1f);
        
        // Recarregar o boost
        if (boostAmount < 100f)
        {
            boostAmount += boostRechargeRate * Time.deltaTime;
            if (boostAmount > 100f)
            {
                boostAmount = 100f;
                totalCharge = true;
            }
        }
        if (totalCharge)
        {
            if (Input.GetButtonDown("Fire3"))
            {
                Boost(true);
                isBoosting = true;
            }

            if (Input.GetButtonUp("Fire3"))
            {
                Boost(false); isBoosting = false;
            }

            if (Input.GetButtonDown("Fire4"))
            {
                Break(true); isBoosting = true;
            }


            if (Input.GetButtonUp("Fire4"))
            {
                Break(false); isBoosting = false;
            }

            if (isBoosting && totalCharge)
            {
                // Boost(isBoosting);
                boostAmount -= boostCost * Time.deltaTime;
                if (boostAmount <= 0) {
                    totalCharge = false;
                    isBoosting = false;
                    Boost(isBoosting);
                }
            }

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            {
                int dir = Input.GetKeyDown(KeyCode.Q) ? -1 : 1;
                QuickSpin(dir);
            }
        }
        if (Time.time - lastInputTime > inputDelay)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                currentRotation -= rotationSpeed * Time.deltaTime;
                lastInputTime = Time.time;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                currentRotation += rotationSpeed * Time.deltaTime;
                lastInputTime = Time.time;
            }
        }
        currentRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentRotation);
        }

    }

    public void QuickSpin(int dir)
    {
        if (!DOTween.IsTweening(playerModel))
        {
            playerModel.DOLocalRotate(new Vector3(playerModel.localEulerAngles.x, playerModel.localEulerAngles.y, 360 * -dir), .4f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
        }
    }
    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, invert * y, 0) * speed * Time.deltaTime;
        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void RotationLookPath(float horizontal, float vertical, float speed)
    {
        Renderer aimTargetRenderer = aimTarget.GetComponent<Renderer>();
        if (aimTargetRenderer != null)
        {
            aimTargetRenderer.enabled = false;
        }
        aimTarget.parent.position = Vector3.zero;
        aimTarget.localPosition = new Vector3(horizontal,invert * vertical, 1);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTarget.position), Mathf.Deg2Rad * speed * Time.deltaTime);
    }

    void HorizontalLean(Transform target, float axis, float leanlimit, float lerpTime)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * leanlimit, lerpTime));
    }

    void SetSpeed(float forwardSpeed)
    {
        dolly.m_Speed = forwardSpeed;
    }

    void SetCameraZoom(float zoom, float duration)
    {
        cameraParent.DOLocalMove(new Vector3(0, 0, zoom), duration);
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aimTarget.position, .5f);
        Gizmos.DrawSphere(aimTarget.position, .15f);
    }*/

    void DistortionAmount(float x)
    {
        Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<LensDistortion>().intensity.value = x;
    }

    void FieldOfView(float fov)
    {
        cameraParent.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = fov;
    }

    void Chromatic(float x)
    {
        PostProcessVolume postProcessVolume = Camera.main.GetComponent<PostProcessVolume>();
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.GetSetting<ChromaticAberration>().intensity.value = x;
        }
    }

    void Boost(bool state)
    {
        if (!state)
            Camera.main.fieldOfView = originalFov;
        if (state)
        {
        float origFov = state ? 50 : 60;
        float endFov = state ? 60 : 60;
        float origChrom = state ? 0 : 1;
        float endChrom = state ? 1 : 0;
        float origDistortion = state ? 0 : -30;
        float endDistorton = state ? -30 : 0;
        float speed = state ? forwardSpeed * 2 : forwardSpeed;
        float zoom = state ? -7 : 0;

        DOVirtual.Float(origChrom, endChrom, .5f, Chromatic);
        DOVirtual.Float(origFov, endFov, 0.5f, FieldOfView);
        DOVirtual.Float(origDistortion, endDistorton, .5f, DistortionAmount);
        DOVirtual.Float(dolly.m_Speed, speed, .15f, SetSpeed);
        SetCameraZoom(zoom, .4f);
        }
        else if (!state)
            SetCameraZoom(0, .4f);
    }

    void Break(bool state)
    {
        float speed = state ? forwardSpeed / 3 : forwardSpeed;
        float zoom = state ? 3 : 0;
        DOVirtual.Float(dolly.m_Speed, speed, .15f, SetSpeed);
        SetCameraZoom(zoom, .4f);
    }
}
