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
            StartCoroutine(Register(emailInput.text, passInput.text));            
        }
    }

    IEnumerator Register(string email, string password)
    {
        var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            resultText.text = "ȸ������ ����!";
        }
        else
        {
            resultText.text = "ȸ������ ���Ф�" + task.Exception;
        }
    }

    // �α���
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
            resultText.text = "�α��� ����!";
        }
        else
        {
            resultText.text = "�α��� ���Ф�" + task.Exception;
        }
    }

   

}
