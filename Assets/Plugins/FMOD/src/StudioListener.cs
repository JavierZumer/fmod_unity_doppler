using FMOD;
using UnityEngine;

namespace FMODUnity
{
    [AddComponentMenu("FMOD Studio/FMOD Studio Listener")]
    public class StudioListener : MonoBehaviour
    {
#if UNITY_PHYSICS_EXIST
        Rigidbody rigidBody;
#endif
#if UNITY_PHYSICS2D_EXIST
        Rigidbody2D rigidBody2D;
#endif

        public GameObject attenuationObject;

        public int ListenerNumber = -1;
        public VelocityVector3 kinematicVelocity = null;
        Vector3 positionLastFrame;

        void OnEnable()
        {
            RuntimeUtils.EnforceLibraryOrder();
#if UNITY_PHYSICS_EXIST
            rigidBody = gameObject.GetComponent<Rigidbody>();
#endif
#if UNITY_PHYSICS2D_EXIST
            rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
#endif
            if (!rigidBody && !rigidBody2D)
            {
                kinematicVelocity = new VelocityVector3();
            }
            ListenerNumber = RuntimeManager.AddListener(this);
        }

        void OnDisable()
        {
            RuntimeManager.RemoveListener(this);
        }

        void Update()
        {
            if (kinematicVelocity != null)
            {
                SetKinematicVelocity();
            }
            
            if (ListenerNumber >= 0 && ListenerNumber < FMOD.CONSTANTS.MAX_LISTENERS)
            {
                SetListenerLocation();
            }
        }
        
        private void SetKinematicVelocity()
        {
            //Get current velocity
            Vector3 currentVel;
            currentVel.x = kinematicVelocity.x;
            currentVel.y = kinematicVelocity.y;
            currentVel.z = kinematicVelocity.z;

            //Update to new velocity
            currentVel = Vector3.Lerp(currentVel, (transform.position - positionLastFrame) / Time.deltaTime, Time.deltaTime * 15);

            //Reassign to CVector3 object
            kinematicVelocity.x = currentVel.x;
            kinematicVelocity.y = currentVel.y;
            kinematicVelocity.z = currentVel.z;

            //Store world position for next frame
            positionLastFrame = transform.position;
        }

        void SetListenerLocation()
        {
#if UNITY_PHYSICS_EXIST
            if (rigidBody)
            {
                RuntimeManager.SetListenerLocation(ListenerNumber, gameObject, rigidBody, attenuationObject);
            }
            else
#endif
#if UNITY_PHYSICS2D_EXIST
            if (rigidBody2D)
            {
                RuntimeManager.SetListenerLocation(ListenerNumber, gameObject, rigidBody2D, attenuationObject);
            }
            else
#endif
            {
                RuntimeManager.SetListenerLocation(ListenerNumber, gameObject, kinematicVelocity);
            }
        }
    }
}