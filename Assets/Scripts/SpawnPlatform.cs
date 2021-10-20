using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    //List ou Array
    //List de GameObjects para armazenar os prefabs
    public List<GameObject> platforms = new List<GameObject>();
    //List de Transform para os Objetos Instanciados
    public List<Transform> currentPlatforms = new List<Transform>();

    public int offset; //Variavel que nos permite saber a distancia de uma plataforma para outra

    private Transform player;
    private Transform currentPlatformPoint;

    private int platformIndex;
    void Start()
    {
        //Procurar na cena algum objeto que tenha a tag player
        player = GameObject.FindGameObjectWithTag("Player").transform;

        for(int i = 0; i < platforms.Count; i++)
        { //Estamos passando o numero deste objeto a partir do 0, sendo o elemento 0 a primeira plataforma
            Transform p = Instantiate(platforms[i], new Vector3(0, 0, i * 86), transform.rotation).transform;
            currentPlatforms.Add(p);
            offset += 86; //Incrementar
        }

        //Recebe o ponto da primeira plataforma - A
        currentPlatformPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
    }

    void Update()
    {   //Armazenando a posição z do personagem e a posição z do objeto finalPoint, quando o personagem passar por cima, o distance receberá o valor de zero. Até então o valor é negativo, quando ele passar pelo point ele será positivo.
        float distance = player.position.z - currentPlatformPoint.position.z;
        
        if(distance >= 5)
        {
            Recycle(currentPlatforms[platformIndex].gameObject);
            platformIndex++;

            //Quando passarmos da terceira plataforma, a gente zera o index
            if (platformIndex > currentPlatforms.Count -1)
            {
                platformIndex = 0;
            }

            currentPlatformPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
        }
    }

    public void Recycle (GameObject platform)
    {
        platform.transform.position = new Vector3(0, 0, offset);
        offset += 86;
    }
}
