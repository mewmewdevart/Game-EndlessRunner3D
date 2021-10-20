using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;

    //Controle da movimentação
    public float speed;
    public float jumpHeight;
    private float jumpVelocity;
    public float gravity;

    public float rayRadius;

    public LayerMask layer; //Selecionar a layer
    public LayerMask coinLayer;//Moedinhas
    public float horizontalSpeed;

    //Corrigir o problema da pessoa ficar clicando varias vezes no botao
    private bool isMovingLeft;
    private bool isMovingRight;

    public bool isDead;
    public Animator anim;

    private GameController gc;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gc = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

        //Variavel Local recebe o Vector3.Forward que estará adicionando 1 ao eixo 0 do Z que é o responsavel pela profundidade (Locomoção do personagem para frente e para trás)
        Vector3 direction = Vector3.forward * speed;


        //Se o personagem está em contato com o chão, faça com que o JumpVelocity receba a força do JumpHeight
        if(controller.isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                jumpVelocity = jumpHeight;
            }
        }

    //Se eu pressionar a seta direita &&Checando mais condições para evitar que o personagem saia da tela E cortando a quantidade de apertamentos da tecla
        if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < 3f && !isMovingRight) //Sinal de ! antes de um variavel booleana quer dizer que é == false
        {
            isMovingRight = true;
            //Chamando a corrotina
            StartCoroutine(RightMove());
        }

    //Se eu pressionar a seta esqurda
        if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > -3f && !isMovingLeft)//Sinal de ! antes de um variavel booleana quer dizer que é == false
        {
            isMovingLeft = true;
            //Chamando a corrotina
            StartCoroutine(LeftMove());
        }



        //Mas se o personagem estiver no ar, o jumpVelocity será subtraido pelo Gravity - Como se o jumpVelocity = jumpVelocity - gravity
        else
        {
            jumpVelocity -= gravity;
        }

        OnColission ();
        direction.y = jumpVelocity;

        //O delta multiplica pelo tempo no jogo - Otimiza a movimentação do personagem porque utiliza o tempo como base
        controller.Move(direction * Time.deltaTime);


    }
    


    //Corrotinas: Pode ser controlada por tempo
    //Lembrando x é o eixo horizontal, e o y vertical
    //Movimentação em Fração de segundo enquanto  movemos o personagem
    IEnumerator LeftMove()
    {
        //A cada rodada, o personagem é movido para o lado
        for(float i = 0; i < 10; i+= 0.1f)
        {
            controller.Move(Vector3.left *Time.deltaTime * horizontalSpeed);
            yield return null; //Para sair do For
        }
        isMovingLeft = false;
    }
    IEnumerator RightMove()
    {
        for(float i = 0; i < 10; i += 0.1f)
        {   
            controller.Move(Vector3.right * Time.deltaTime * horizontalSpeed);
            yield return null;
        }
        isMovingRight = false;
    }


//Quando ele colidir com alguma coisa - Raycast 
    private void OnColission()
    {
        RaycastHit hit;
        //Checar se o personagem está colidindo com algo na frente dele
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayRadius, layer) && !isDead) //Pede tres argumentos base, um Vector 3 de origem e precisa dizer a direão do raio
        //Estou informando uma direção de origem e outra de destino (Vector3.forward | O forward é o valor de 1 no eixo z) em seguida informei o tamanho do raio 10f no z e o "out hit" é para armazenar o objeto 
        {
            //Chama GameOver : Chamando o trigger
            anim.SetTrigger("die");
            speed = 0;
            jumpHeight = 0;
            horizontalSpeed = 0;

            //Chamando o gameover com o delay
            Invoke("GameOver", 3f); //Tempo para chamar a img do gameover

            isDead = true;//Esse if é executado só uma vez
        }

        //Quando o ray detectar a colisao
        RaycastHit coinHit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + new Vector3(0, 1f, 0)), out coinHit, rayRadius, coinLayer)) //O 1f é a altura para pegar a moeda
        {
        //AO BATER NA MOEDA
        gc.AddCoin();
            Destroy(coinHit.transform.gameObject);
        }
    }
    void GameOver()
    {
        gc.ShowGameOver();
    }
}
