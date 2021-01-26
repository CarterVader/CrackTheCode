using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    //Main game instance for loading levels
    public Game game;

    //Controls camera pitch
    float cameraPitch = 0.0f;

    //Mouse movement controls
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    float mouseSensitivity = 2.5f;

    //Remaining lives
    public int lives;

    //Holds interactable touched by raycast
    RaycastHit whatIHit;
    bool touchingInteractable;

    //Controls distance to interact with objects
    float distanceToSee = 3.0f;

    //Controls player movement speeds
    float walkingSpeed = 5.5f;
    float jumpSpeed = 4.5f;
    float gravity = 20.0f;

    //Max speed of held object
    public float maxHoldableSpeed = 1f;

    //Current active input UI
    public InputUI activeInput;

    //Player rotation
    private float rotY;
    private float rotX;

    //Player UI overlay
    public Transform canvas;

    //Player camera turn angle limits
    float minTurnAngle = -90.0f;
    float maxTurnAngle = 90.0f;

    //Player camera transform
    public Transform playerCamera;

    //Current movement inputs
    Vector2 moveVector;
    bool jump;

    //Current held object
    public GameObject heldObject;

    //Player controller and velocity
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;

    //controls if player can move or interact
    public bool canMove = false;
    public bool canInteract = false;

    //Player input mappings
    public InputActionMap playerActions;

    //Mask containing held object
    public LayerMask heldObjectMask;

    //Timer
    public float timer = 0;
    public Text timerText;
    private bool timerActive;
    private bool alertActive;

    //Holds whether or not player is reading
    private bool reading;

    //Notification strings
    private string locked = "[Locked]";
    private string unlocked = "[Unlocked]";
    private string lives_lost = "[-1]";

    //Coroutine to send alerts
    Coroutine alert;

    void Start()
    {
        //Assign variables on level start
        characterController = GetComponent<CharacterController>();
        playerCamera = transform.GetChild(1);
        canvas = transform.GetChild(2);
        heldObjectMask = LayerMask.GetMask(new string [] {"Held Object"});
        playerActions = GetComponent<PlayerInput>().currentActionMap;
        timerText = canvas.GetChild(7).GetComponent<Text>();
    }

    public void StartGame(int currentLevelIndex)
    {
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Set level loader
        game = transform.parent.gameObject.GetComponent<Game>();

        //Let player move
        canMove = true;
        canInteract = true;

        //Start timer
        timer = Levels.levels[currentLevelIndex].time;
        timerActive = true;
    }

    //WASD INPUT
    public void Move(InputAction.CallbackContext context)
    {
        //Get movement inputs (WASD keys)
        moveVector = context.action.ReadValue<Vector2>();
    }

    public void LoseLife()
    {
        //Lower life
        lives -= 1;
        canvas.GetChild(9).GetComponent<LivesUI>().UpdateLives(lives);

        //Check if lost
        if (lives <= 0)
        {
            Levels.lossReason = "You ran out of lives";
            Lose();
        }
    }

    //Called on level lose
    public void Lose()
    {
        //Turn off timer and lives
        canvas.GetChild(9).gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        //Send to loss screen
        StartCoroutine(LoseCoroutine());
    }

    IEnumerator LoseCoroutine()
    {
        //Fade out
        Image image = canvas.GetChild(8).GetComponent<Image>();
        image.gameObject.SetActive(true);
        image.CrossFadeAlpha(0.0f, 0.0f, false);
        image.CrossFadeAlpha(1.0f, 1.0f, false);

        //Wait for fade and load loss screen
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(6);
    }

    //Sets crosshair shape
    public void SetCrosshairActive(bool active)
    {
        if (active)
        {
            canvas.GetChild(2).gameObject.SetActive(false);
            canvas.GetChild(3).gameObject.SetActive(true);
        } else
        {
            canvas.GetChild(2).gameObject.SetActive(true);
            canvas.GetChild(3).gameObject.SetActive(false);
        }
    }

    //E INPUT
    public void Interact(InputAction.CallbackContext context)
    {
        //check if can interact
        if (context.performed && gameObject.scene.IsValid() && canInteract) {
            //Check if holding object
            if (heldObject == null)
            {
                //Check if touching interactable
                if (touchingInteractable)
                {
                    //Use interactable
                    whatIHit.collider.GetComponent<IInteractable>().Interact(this, null);
                }
            } else
            {
                //Check if touching interactable
                if (touchingInteractable)
                {
                    //use held object on interactable
                    whatIHit.collider.GetComponent<IInteractable>().Interact(this, heldObject);
                } else DropHeldObject(); //Drop held object if not touching interactable
            }
        }
    }

    //Set held object
    public void SetHeldObject(GameObject obj)
    {
        heldObject = obj;
        heldObject.transform.rotation = transform.rotation;
    }

    //Drop held object
    public void DropHeldObject()
    {
        heldObject.GetComponent<Holdable>().Drop();
        heldObject = null;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //Jump on key press
        if (context.ReadValueAsButton())
        {
            jump = true;
        } else if (!context.ReadValueAsButton())
        {
            jump = false;
        }
    }

    //Input UI keypad inputs
    public void Input0ToUI()
    {
        activeInput.Input('0');
    }
    public void Input1ToUI()
    {
        activeInput.Input('1');
    }
    public void Input2ToUI()
    {
        activeInput.Input('2');
    }
    public void Input3ToUI()
    {
        activeInput.Input('3');
    }
    public void Input4ToUI()
    {
        activeInput.Input('4');
    }
    public void Input5ToUI()
    {
        activeInput.Input('5');
    }
    public void Input6ToUI()
    {
        activeInput.Input('6');
    }
    public void Input7ToUI()
    {
        activeInput.Input('7');
    }
    public void Input8ToUI()
    {
        activeInput.Input('8');
    }
    public void Input9ToUI()
    {
        activeInput.Input('9');
    }
    public void UISubmit()
    {
        activeInput.Submit();
    }
    public void UIBackSpace()
    {
        activeInput.BackSpace();
    }
    public void CloseUI()
    {
        activeInput.Close();
    }

    //ESC KEY PRESS
    public void Exit()
    {
        //Close game
        Application.Quit();
    }

    //Pause/Unpause game
    public void Pause()
    {
        if (game.running)
        {
            //prevent player from moving
            reading = true;
            canMove = false;
            canInteract = false;
            game.PauseGame();

            //Open pause screen and unlock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canvas.GetChild(0).gameObject.SetActive(true);
            canvas.GetChild(1).gameObject.SetActive(false);
            canvas.GetChild(2).gameObject.SetActive(false);
            canvas.GetChild(3).gameObject.SetActive(false);
        }
        else
        {
            //Let player move
            reading = false;
            canMove = true;
            canInteract = true;
            game.UnPauseGame();

            //Close pause screen and lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canvas.GetChild(4).GetChild(0).gameObject.SetActive(false);
            canvas.GetChild(0).gameObject.SetActive(false);
            canvas.GetChild(1).gameObject.SetActive(true);
            canvas.GetChild(2).gameObject.SetActive(true);
            canvas.GetChild(3).gameObject.SetActive(true);
        }
    }

    //Handles displaying readables
    public void Resume()
    {
        //Resumes game
        if (!reading)
        {
            if (game.running)
            {
                //prevent player from moving
                canMove = false;
                canInteract = false;
                timerActive = false;
                game.PauseGame();

                //Open readable screen and unlock cursor\
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                canvas.GetChild(2).gameObject.SetActive(false);
                canvas.GetChild(3).gameObject.SetActive(false);
                canvas.GetChild(11).gameObject.SetActive(true);
            }
            else
            {
                //Let player move
                canMove = true;
                canInteract = true;
                timerActive = true;
                game.UnPauseGame();

                //Close readable screen and lock cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                canvas.GetChild(4).GetChild(0).gameObject.SetActive(false);
                canvas.GetChild(2).gameObject.SetActive(true);
                canvas.GetChild(3).gameObject.SetActive(true);
                canvas.GetChild(11).gameObject.SetActive(false);
            }
        }        
    }

    //Sends message to player
    public void Alert(string message)
    {
        //Stop active alerts
        if (alert != null)
        {
            StopCoroutine(alert);
        }

        //Send alert
        alert = StartCoroutine(AlertCoroutine(message));
    }

    public IEnumerator AlertCoroutine(string message)
    {
        //Check for message type
        if (!alertActive && message.Equals(locked))
        {
            //Display locked message
            Debug.Log("Alert: " + message);
            canvas.GetChild(10).GetComponent<Text>().text = message;
            canvas.GetChild(10).GetComponent<Text>().color = Color.red;
            canvas.GetChild(10).GetComponent<Text>().CrossFadeAlpha(1.0f, 0.0f, false);
            canvas.GetChild(10).gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            canvas.GetChild(10).GetComponent<Text>().CrossFadeAlpha(0.0f, 1.0f, false);
            yield return new WaitForSeconds(1);
            canvas.GetChild(10).gameObject.SetActive(false);
        }
        else if (!alertActive && message.Equals(unlocked))
        {
            //Display unlocked message
            Debug.Log("Alert: " + message);
            canvas.GetChild(10).GetComponent<Text>().text = message;
            canvas.GetChild(10).GetComponent<Text>().color = Color.green;
            canvas.GetChild(10).GetComponent<Text>().CrossFadeAlpha(1.0f, 0.0f, false);
            canvas.GetChild(10).gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            canvas.GetChild(10).GetComponent<Text>().CrossFadeAlpha(0.0f, 1.0f, false);
            yield return new WaitForSeconds(1);
            canvas.GetChild(10).gameObject.SetActive(false);
        }
        else if (!alertActive && message.Equals(lives_lost))
        {
            //Display live lost message
            Debug.Log("Alert: " + message);
            canvas.GetChild(12).GetComponent<Text>().text = message;
            canvas.GetChild(12).GetComponent<Text>().color = Color.red;
            canvas.GetChild(12).GetComponent<Text>().CrossFadeAlpha(.5f, 0.0f, false);
            canvas.GetChild(12).gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            canvas.GetChild(12).GetComponent<Text>().CrossFadeAlpha(0.0f, .5f, false);
            yield return new WaitForSeconds(.5f);
            canvas.GetChild(12).gameObject.SetActive(false);
        }
    }


    void Update()
    {
        if (timerActive)
        {
            //Tick down timer
            timer -= Time.deltaTime;
            timerText.text = "Time Remaining: " + timer.ToString().Split('.')[0];

            //Check if time has run out
            if (timer <= 0)
            {
                timerActive = false;
                Pause();
                Levels.lossReason = "You ran out of time";
                Lose();
            }
        }

        //Camera Movement
        if (canMove)
        {
            //Get mouse movement
            Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            //Smooth movement via SmoothDamp to avoid choppy camera movement
            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

            //Calculate new camera pitch and prevent from looking too high
            cameraPitch -= currentMouseDelta.y * mouseSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch, minTurnAngle, maxTurnAngle);

            //Rotate camera
            playerCamera.localEulerAngles = Vector3.right * cameraPitch;
            transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        }
    }

    void FixedUpdate()
    {
        //Convert from local to world
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        //Calculate Speed
        float curSpeedX = walkingSpeed * moveVector.x;
        float curSpeedY = walkingSpeed * moveVector.y;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedY) + (right * curSpeedX);

        //Jump
        if (jump && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
        //Gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //Move player
        if (canMove)
        {
            characterController.Move(moveDirection * Time.deltaTime);
        }

        

        if (heldObject != null)
        {
            //Movement speed
            float step = 10.0f * Time.deltaTime;

            //Calculate object movement to keep in front of player
            Vector3 point = Vector3.MoveTowards(heldObject.transform.position, transform.GetChild(1).position + transform.GetChild(1).forward * 2, step);
            Quaternion rot = Quaternion.RotateTowards(heldObject.transform.rotation, transform.GetChild(0).rotation, step * 10f);

            //Reset object movement
            heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            heldObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            //Check if object is lower than player
            if (heldObject.transform.position.y > transform.position.y)
            {
                //Move object to stay in front of player
                heldObject.GetComponent<Rigidbody>().MoveRotation(rot);
                heldObject.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(point - heldObject.transform.position) * Vector3.Distance(heldObject.transform.position, transform.GetChild(1).position + transform.GetChild(1).forward * 2) * 1000f);
            }
            else
            {
                //Move object but limit vertical movement to prevent player from flying on object
                Vector3 forceVector = Vector3.Normalize(point - heldObject.transform.position) * Vector3.Distance(heldObject.transform.position, transform.GetChild(1).position + transform.GetChild(1).forward * 2) * 1000f;
                forceVector.y = 0;
                heldObject.GetComponent<Rigidbody>().MoveRotation(rot);
                heldObject.GetComponent<Rigidbody>().AddForce(forceVector);
            }
        }

        //Check if player can interact
        if (canInteract)
        {
            //Get object player is looking at via raycast
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out whatIHit, distanceToSee, ~heldObjectMask))
            {
                //Check if object is interactable
                if (whatIHit.collider.GetComponent<IInteractable>() != null)
                {
                    //Player is touching interactable
                    touchingInteractable = true;

                    //Display interact text based on object and change crosshair
                    if (heldObject == null)
                    {
                        
                        canvas.GetChild(1).GetComponent<Text>().text = "[" + playerActions.FindAction("Interact").GetBindingDisplayString() + "] to " + whatIHit.collider.GetComponent<IInteractable>().InteractText();
                        canvas.GetChild(1).gameObject.SetActive(true);
                        SetCrosshairActive(true);
                    }
                    else
                    {
                        if (whatIHit.collider.GetComponent<Readable>() == null)
                        {
                            canvas.GetChild(1).GetComponent<Text>().text = ("[" + playerActions.FindAction("Interact").GetBindingDisplayString() + "] to use " + heldObject.GetComponent<ObjectLang>().GetName() + " on " + whatIHit.transform.gameObject.GetComponent<ObjectLang>().GetName()).Replace("(Clone)", "");
                            canvas.GetChild(1).gameObject.SetActive(true);
                            SetCrosshairActive(true);
                        }
                        else
                        {
                            canvas.GetChild(1).GetComponent<Text>().text = "[" + playerActions.FindAction("Interact").GetBindingDisplayString() + "] to " + whatIHit.collider.GetComponent<IInteractable>().InteractText();
                            canvas.GetChild(1).gameObject.SetActive(true);
                            SetCrosshairActive(true);
                        }
                    }
                }
                else
                {
                    //Player is not touching interactable
                    touchingInteractable = false;

                    //Set crosshair back to default
                    if (heldObject == null)
                    {
                        canvas.GetChild(1).gameObject.SetActive(false);
                        SetCrosshairActive(false);
                    }
                    else
                    {
                        //If holding object display drop text
                        canvas.GetChild(1).GetComponent<Text>().text = "[" + playerActions.FindAction("Interact").GetBindingDisplayString() + "] to drop";
                        canvas.GetChild(1).gameObject.SetActive(true);
                        SetCrosshairActive(true);
                    }
                }
            }
            else
            {
                //Player is not touching interactable
                touchingInteractable = false;

                //Set crosshair back to default
                if (heldObject == null)
                {
                    canvas.GetChild(1).gameObject.SetActive(false);
                    SetCrosshairActive(false);
                }
                else
                {
                    //If holding object display drop text
                    canvas.GetChild(1).GetComponent<Text>().text = "[" + playerActions.FindAction("Interact").GetBindingDisplayString() + "] to drop";
                    canvas.GetChild(1).gameObject.SetActive(true);
                    SetCrosshairActive(true);
                }
            }
        }
    }
}
