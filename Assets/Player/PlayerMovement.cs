using System;
using UnityEngine;


namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float WalkStopRadius = 0.2f;
        [SerializeField] float MeleeAttackStopRadius = 0.4f;
        [SerializeField] float RangedAttackStopRadius = 5f;
        private ThirdPersonCharacter character;   // A reference to the ThirdPersonCharacter on the object
        private bool jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        bool ItIsDirectiveMode= false;
        CameraRaycaster cameraRaycaster;

        Vector3 currentClickTarget;
        Vector3 currentDestination, clickPoint;

        private void Start()
        {
          

            // get the third person character ( this should never be null due to require component )
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            character = GetComponent<ThirdPersonCharacter>();
            currentDestination = transform.position;
            currentClickTarget = transform.position;
        }
        private void Update()
        {
            if (!jump)
            {
                jump = Input.GetButtonDown("Jump");
            }
        }
        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
          
            if (Input.GetKeyDown(KeyCode.G))
            {
                ItIsDirectiveMode = !ItIsDirectiveMode;
                currentDestination = transform.position;
                currentClickTarget = transform.position;
            }
            if (ItIsDirectiveMode)
            {
                ProcessDirectMovement();
            }
            else
            {
                ProccessMouseMovement();
            }
        }

        private void ProcessDirectMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);
            // calculate camera relative direction to move:
            Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward + h * Camera.main.transform.right;
            character.Move(move, crouch, jump);
            jump = false;
        }

        private void ProccessMouseMovement()
        {
            
            if (Input.GetMouseButton(0))
            {
                clickPoint = cameraRaycaster.hit.point;
                switch (cameraRaycaster.layerHit)
                {
                    case Layer.Walkable:
                        currentDestination = ShortDestination(clickPoint, WalkStopRadius);  // So not set in default case
                        break;
                    case Layer.Enemy:
                        currentDestination = ShortDestination(clickPoint, MeleeAttackStopRadius);
                        break;
                    default:
                        print("BUG achieved");
                        return;
                }
                
            }
            WalkToDestination();
        }

        void WalkToDestination() {
            bool crouch = Input.GetKey(KeyCode.C);
            var playerToClickPoint = (currentDestination - transform.position);
            if (playerToClickPoint.magnitude >= 0)
            {
                character.Move(playerToClickPoint, crouch, jump);
                jump = false;
            }
            else
            {
                character.Move(Vector3.zero, crouch, jump);
            }

        }

        Vector3 ShortDestination(Vector3 destination, float shortening)
        {
            Vector3 reductionVector = (destination - transform.position).normalized * shortening;
            return destination - reductionVector;
        }
            private void OnDrawGizmos()
            {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, clickPoint);
            Gizmos.DrawSphere(currentDestination, 0.15f);
            Gizmos.DrawSphere(clickPoint, 0.1f);
            // Draw attack sphere
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, MeleeAttackStopRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, RangedAttackStopRadius);
        }
    }
}
