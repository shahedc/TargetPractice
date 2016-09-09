using System;
using HoloToolkit;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GestureManager : Singleton<GestureManager>
{
    public GameObject LastTappedObject { get; set; }
    public GameObject ProjectileToShoot;

    // Tap and Navigation gesture recognizer.
    public GestureRecognizer NavigationRecognizer { get; private set; }

    // Manipulation gesture recognizer.
    public GestureRecognizer ManipulationRecognizer { get; private set; }

    // Currently active gesture recognizer.
    public GestureRecognizer ActiveRecognizer { get; private set; }

    public bool IsNavigating { get; private set; }

    public Vector3 NavigationPosition { get; private set; }

    public bool IsManipulating { get; private set; }

    public Vector3 ManipulationPosition { get; private set; }

    void Awake()
    {
        /* TODO: DEVELOPER CODING EXERCISE 2.b */

        // 2.b: Instantiate the NavigationRecognizer.
        NavigationRecognizer = new GestureRecognizer();

        // 2.b: Add Tap and NavigationX GestureSettings to the NavigationRecognizer's RecognizableGestures.
        NavigationRecognizer.SetRecognizableGestures(
            GestureSettings.Tap |
            GestureSettings.NavigationX);

        // 2.b: Register for the TappedEvent with the NavigationRecognizer_TappedEvent function.
        NavigationRecognizer.TappedEvent += NavigationRecognizer_TappedEvent;
        // 2.b: Register for the NavigationStartedEvent with the NavigationRecognizer_NavigationStartedEvent function.
        NavigationRecognizer.NavigationStartedEvent += NavigationRecognizer_NavigationStartedEvent;
        // 2.b: Register for the NavigationUpdatedEvent with the NavigationRecognizer_NavigationUpdatedEvent function.
        NavigationRecognizer.NavigationUpdatedEvent += NavigationRecognizer_NavigationUpdatedEvent;
        // 2.b: Register for the NavigationCompletedEvent with the NavigationRecognizer_NavigationCompletedEvent function. 
        NavigationRecognizer.NavigationCompletedEvent += NavigationRecognizer_NavigationCompletedEvent;
        // 2.b: Register for the NavigationCanceledEvent with the NavigationRecognizer_NavigationCanceledEvent function. 
        NavigationRecognizer.NavigationCanceledEvent += NavigationRecognizer_NavigationCanceledEvent;

        // Instantiate the ManipulationRecognizer.
        ManipulationRecognizer = new GestureRecognizer();

        // Add the ManipulationTranslate GestureSetting to the ManipulationRecognizer's RecognizableGestures.
        ManipulationRecognizer.SetRecognizableGestures(
            GestureSettings.ManipulationTranslate);

        // Register for the Manipulation events on the ManipulationRecognizer.
        ManipulationRecognizer.ManipulationStartedEvent += ManipulationRecognizer_ManipulationStartedEvent;
        ManipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognizer_ManipulationUpdatedEvent;
        ManipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognizer_ManipulationCompletedEvent;
        ManipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognizer_ManipulationCanceledEvent;

        ResetGestureRecognizers();
    }

    void OnDestroy()
    {
        // 2.b: Unregister the Tapped and Navigation events on the NavigationRecognizer.
        NavigationRecognizer.TappedEvent -= NavigationRecognizer_TappedEvent;

        NavigationRecognizer.NavigationStartedEvent -= NavigationRecognizer_NavigationStartedEvent;
        NavigationRecognizer.NavigationUpdatedEvent -= NavigationRecognizer_NavigationUpdatedEvent;
        NavigationRecognizer.NavigationCompletedEvent -= NavigationRecognizer_NavigationCompletedEvent;
        NavigationRecognizer.NavigationCanceledEvent -= NavigationRecognizer_NavigationCanceledEvent;

        // Unregister the Manipulation events on the ManipulationRecognizer.
        ManipulationRecognizer.ManipulationStartedEvent -= ManipulationRecognizer_ManipulationStartedEvent;
        ManipulationRecognizer.ManipulationUpdatedEvent -= ManipulationRecognizer_ManipulationUpdatedEvent;
        ManipulationRecognizer.ManipulationCompletedEvent -= ManipulationRecognizer_ManipulationCompletedEvent;
        ManipulationRecognizer.ManipulationCanceledEvent -= ManipulationRecognizer_ManipulationCanceledEvent;
    }

    /// <summary>
    /// Revert back to the default GestureRecognizer.
    /// </summary>
    public void ResetGestureRecognizers()
    {
        // Default to the navigation gestures.
        Transition(NavigationRecognizer);
    }

    /// <summary>
    /// Transition to a new GestureRecognizer.
    /// </summary>
    /// <param name="newRecognizer">The GestureRecognizer to transition to.</param>
    public void Transition(GestureRecognizer newRecognizer)
    {
        if (newRecognizer == null)
        {
            return;
        }

        if (ActiveRecognizer != null)
        {
            if (ActiveRecognizer == newRecognizer)
            {
                return;
            }

            ActiveRecognizer.CancelGestures();
            ActiveRecognizer.StopCapturingGestures();
        }

        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
    }

    private void NavigationRecognizer_NavigationStartedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        // 2.b: Set IsNavigating to be true.
        IsNavigating = true;

        // 2.b: Set NavigationPosition to be relativePosition.
        NavigationPosition = relativePosition;
    }

    private void NavigationRecognizer_NavigationUpdatedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        // 2.b: Set IsNavigating to be true.
        IsNavigating = true;

        // 2.b: Set NavigationPosition to be relativePosition.
        NavigationPosition = relativePosition;
    }

    private void NavigationRecognizer_NavigationCompletedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        // 2.b: Set IsNavigating to be false.
        IsNavigating = false;
    }

    private void NavigationRecognizer_NavigationCanceledEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        // 2.b: Set IsNavigating to be false.
        IsNavigating = false;
    }

    private void ManipulationRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        if (HandsManager.Instance.FocusedGameObject != null)
        {
            IsManipulating = true;

            ManipulationPosition = position;

            HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationStart", position);
        }
    }

    private void ManipulationRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        if (HandsManager.Instance.FocusedGameObject != null)
        {
            IsManipulating = true;

            ManipulationPosition = position;

            HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationUpdate", position);
        }
    }

    private void ManipulationRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = false;
    }

    private void ManipulationRecognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = false;
    }

    private void NavigationRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray ray)
    {

        // shoot projectile
        SpawnProjectile(); 
    }



    void SpawnProjectile()
    {
        Vector3 ProjectilePosition = Camera.main.transform.position + Camera.main.transform.forward * 0.85f;
        Vector3 ProjectileDirection = Camera.main.transform.forward;

        ShootProjectile(ProjectilePosition, ProjectileDirection);        
    }

    void ShootProjectile(Vector3 start, Vector3 direction)
    {
        //ProjectileToShoot = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Projectile.prefab", typeof(GameObject));
        //GameObject spawnedProjectile = (GameObject)Instantiate(ProjectileToShoot);

        GameObject spawnedProjectile = Instantiate(ProjectileToShoot);
        spawnedProjectile.transform.position = start;
        float force = 100.0f;
        spawnedProjectile.GetComponent<Rigidbody>().AddForce(direction * force);
        
    }

}