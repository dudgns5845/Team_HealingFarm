using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    // 이메일
    [SerializeField]
    InputField emailInput;
    // 비밀번호
    [SerializeField]
    InputField passInput;
    // 결과
    [SerializeField]
    Text resultText;

    
    Firebase.Auth.FirebaseAuth auth;

    
    void Awake()
    {     
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    
    // 회원가입
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
            resultText.text = "회원가입 성공!";
        }
        else
        {
            resultText.text = "회원가입 실패ㅠ" + task.Exception;
        }
    }

    // 로그인
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
            resultText.text = "로그인 성공!";
        }
        else
        {
            resultText.text = "로그인 실패ㅠ" + task.Exception;
        }
    }

   

}
