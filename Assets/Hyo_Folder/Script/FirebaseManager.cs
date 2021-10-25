using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    // email
    [SerializeField]
    InputField emailInput;
    // PW
    [SerializeField]
    InputField passInput;
    // result
    [SerializeField]
    Text resultText;

    public GameObject PopUp;
    [SerializeField]
    InputField NickName;


    Firebase.Auth.FirebaseAuth auth;


    void Awake()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    private void Start()
    {
        
    }

    // Sign up
    public void SignUp()
    {
        if (emailInput.text.Length != 0 && passInput.text.Length != 0)
        {
            StartCoroutine(Register(emailInput.text, passInput.text));
        }
    }

    IEnumerator Register(string email, string password)
    {
        var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            resultText.text = "Register Success";
        }
        else
        {
            resultText.text = "Register Failed";
        }
    }



    // Sign In
    public void SignIn()
    {
        if (emailInput.text.Length != 0 && passInput.text.Length != 0)
        {
            StartCoroutine(Login(emailInput.text, passInput.text));
        }
    }


    IEnumerator Login(string email, string password)
    {
        var task = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            Firebase.Auth.FirebaseUser newUser = task.Result;
            resultText.text = "Welcome";
            //scene load
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(1);

        }
        else
        {
            resultText.text = "Log In Failed";
        }
    }

   

}
