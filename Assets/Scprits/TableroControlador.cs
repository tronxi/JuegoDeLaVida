using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableroControlador : MonoBehaviour
{
    public GameObject celula;
    public GameObject camara;

    private const int FILAS = 100;
    private const int COLUMNAS = 100;
    private GameObject[,] tableroVista = new GameObject[FILAS, COLUMNAS];
    private Modelo modelo;

    // Start is called before the first frame update
    void Start()
    {
        //this.transform.position = new Vector3(FILAS / 2, COLUMNAS / 2, 0);
        modelo = new Modelo();
        CrearTablero();
        camara.transform.position = new Vector3(FILAS / 2, FILAS / 2, -130);
        InvokeRepeating("Jugar", 0f, 0.25f);
        //Jugar();
    }

    // Update is called once per frame
    void Update()
    {
        Dibujar();
    }

    private void Jugar()
    {
        modelo.jugar();
        //Debug.Log(modelo.dibujar());
    }

    private void CrearTablero()
    {
        for(int i = 0; i < FILAS; i++)
        {
            for(int j = 0; j < COLUMNAS; j++)
            {
                GameObject cel = Instantiate(celula, new Vector3(i, j, 0), Quaternion.identity);
                cel.transform.parent = this.transform;
                tableroVista[j, i] = cel;
            }
        }
    }

    private void Dibujar()
    {
        for(int i = 0; i < FILAS; i++)
        {
            for(int j = 0; j < COLUMNAS; j++)
            {
                if(modelo.getCelula(i, j) == 1)
                {
                    tableroVista[i, j].GetComponent<Renderer>().material.color = Color.white;
                }
                else
                {
                    tableroVista[i, j].GetComponent<Renderer>().material.color = Color.black;
                }
            }
        }
    }

    class Modelo
    {
        private int[,] estado;
        private int[,] futuro;
        private const int VIVO = 1;
        private const int MUERTO = 0;

        public Modelo()
        {
            estado =  new int[FILAS, COLUMNAS];
            futuro = new int[FILAS, COLUMNAS];
            inicializarMatriz();
        }
        private void inicializarMatriz()
        {
            for(int i = 0; i < FILAS; i++)
            {
                for(int j = 0; j <COLUMNAS; j++)
                {
                    int aleatorio = Random.Range(0, 101);
                    if(aleatorio <= 90)
                    {
                        estado[i, j] = MUERTO;
                    }
                    else
                    {
                        estado[i, j] = VIVO;
                    }
                }
            }
        }

        private void actualizarEstado()
        {
            for(int i = 0; i < FILAS; i++)
            {
                for(int j = 0; j < COLUMNAS; j++)
                {
                    estado[i, j] = futuro[i, j];
                }
            }
        }

        public int getCelula(int x, int y)
        {
            return estado[x, y];
        }

        public bool jugar()
        {
            for(int i = 0; i <FILAS; i++)
            {
                for(int j = 0; j <COLUMNAS; j++)
                {
                    if(estado[i, j] == MUERTO && vecinasVivas(i, j) == 3)
                    {
                        futuro[i, j] = VIVO;
                    }
                    else if(estado[i,j] == VIVO && (vecinasVivas(i, j) == 3 ||
                        vecinasVivas(i,j)==2))
                    {
                        futuro[i, j] = VIVO;
                    }
                    else
                    {
                        futuro[i, j] = MUERTO;
                    }
                }
            }
            actualizarEstado();
            return true;
        }

        public string dibujar()
        {
            string texto = "";
            for(int i = 0; i <FILAS; i++)
            {
                for(int j = 0; j < COLUMNAS; j++)
                {
                    texto += " " + estado[i, j] + " ";
                }
                texto += "\n";
            }
            return texto;
        }

        private int vecinasVivas(int x, int y)
        {
            int total = 0;

            for(int i = x - 1; i <= x + 1; i++)
            {
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && j >= 0 && i < FILAS && j < COLUMNAS)
                    {
                        total += estado[i, j];
                    }
                }
            }

            return total - estado[x,y];
        }
    }
}
