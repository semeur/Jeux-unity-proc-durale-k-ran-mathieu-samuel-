using UnityEngine;


public enum state
{
    Disponible,
    Actuel,
    Complete
}
public class Controlbase : MonoBehaviour
{

    [SerializeField] GameObject[] murs;
    [SerializeField] MeshRenderer sol;

    public void Remove_wall(int wall_to_remove)
    {
        murs[wall_to_remove].gameObject.SetActive(false);
    }

    public void set_state(state stat)
    {
        switch (stat)
        {
            case state.Disponible:
                sol.material.color = Color.white;
                break;
            case state.Actuel:
                sol.material.color = Color.yellow;
                break;
            case state.Complete:
                sol.material.color = Color.blue;
                break;
        }
    }
   
}
