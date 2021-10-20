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
            auth.CreateUserWithEmailAndPasswordAsync(emailInput.text, passInput.text).ContinueWith(
                task =>
                {
                    if (!task.IsCanceled && !task.IsFaulted)
                    {
                        resultText.text = "회원가입 성공!";
                    }
                    else
                    {
                        resultText.text = "회원가입 실패ㅠ";
                    }
                });
        }
    }

    // 로그인
    public void SignIn()
    {
        
        if (emailInput.text.Length != 0 && passInput.text.Length != 0)
        {
            auth.SignInWithEmailAndPasswordAsync(emailInput.text, passInput.text).ContinueWith(
                task =>
                {
                    if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                    {
                        Firebase.Auth.FirebaseUser newUser = task.Result;
                        resultText.text = "로그인 성공!";
                    }
                    else
                    {
                        resultText.text = "로그인 실패ㅠ";
                    }
                });
        }
    }

}
