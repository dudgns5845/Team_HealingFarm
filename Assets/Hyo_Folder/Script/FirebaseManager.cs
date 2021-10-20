using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    // �̸���
    [SerializeField]
    InputField emailInput;
    // ��й�ȣ
    [SerializeField]
    InputField passInput;
    // ���
    [SerializeField]
    Text resultText;

    
    Firebase.Auth.FirebaseAuth auth;


    void Awake()
    {     
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    // ȸ������
    public void SignUp()
    {
       
        if (emailInput.text.Length != 0 && passInput.text.Length != 0)
        {
            auth.CreateUserWithEmailAndPasswordAsync(emailInput.text, passInput.text).ContinueWith(
                task =>
                {
                    if (!task.IsCanceled && !task.IsFaulted)
                    {
                        resultText.text = "ȸ������ ����!";
                    }
                    else
                    {
                        resultText.text = "ȸ������ ���Ф�";
                    }
                });
        }
    }

    // �α���
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
                        resultText.text = "�α��� ����!";
                    }
                    else
                    {
                        resultText.text = "�α��� ���Ф�";
                    }
                });
        }
    }

}
