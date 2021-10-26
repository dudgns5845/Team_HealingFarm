using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Auth;

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
    [SerializeField]
    Text loginResult;

    public GameObject PopUp;
    [SerializeField]
    InputField NickName;
    [SerializeField]
    InputField MapName;

    FirebaseAuth auth;
    public static FirebaseUser authUser;

    void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        //auth.StateChanged += AuthStateChanged;

        emailInput.text = "a@naver.com";
        passInput.text = "123456";
    }

    //void AuthStateChanged(object sender, System.EventArgs eventArgs)
    //{
    //    if (auth.CurrentUser != authUser)
    //    {
    //        bool signedIn = authUser != auth.CurrentUser && auth.CurrentUser != null;
    //        if (!signedIn && authUser != null)
    //        {
    //            Debug.Log("Signed out " + authUser.UserId);
    //        }
    //        authUser = auth.CurrentUser;
    //        if (signedIn)
    //        {
    //            Debug.Log("Signed in " + authUser.UserId);
    //            loginResult.text = authUser.UserId + " Active ";

    //        }
    //    }
    //}

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

            DataBaseManager.instance.GetUserInfo(GetUserText);
            //DataBaseManager.instance.SaveUser()

        }
        else
        {
            resultText.text = "Log In Failed";
        }
    }

    void GetUserText(bool result)
    {
        if(DataBaseManager.instance.User.nickName.Length > 0)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            PopUp.SetActive(true);
        }
        
    }
    
    //save user info
    public void SaveBtn()
    {
        DataBaseManager.instance.User.nickName = NickName.text;
        DataBaseManager.instance.User.mapName = MapName.text;

        DataBaseManager.instance.SaveUser(GetUserText);
    }



}
