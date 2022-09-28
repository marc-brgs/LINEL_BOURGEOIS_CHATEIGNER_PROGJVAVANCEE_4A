using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MCTSAgent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* MCTSNode;
        
         IsFullyExpanded
         GetPossibleMove
         IsValidPlay(GameState)
         IsPlayerTurn(GameState, PlayerClass)
         GameManager -> Is dead
        
         Sélection
         Expansion
         Simulation
         Back Propagation
        
         Random (exploitation, exploration)
        
         Simule le jeu jusqu'à la fin (fonction retourne le nombre de victoire pour n parties jouées)
         On remonte le taux de réussite
         
         parent (x + x' / y + y') = child1 (x / y) + child2  (x' / y')
        */
    }

    private void GetPossibleAction()
    {
        
    }
    
    private void Selection()
    {
        
    }
    
    private void Expansion(Node node)
    {
        
    }

    private void Simulation()
    {
        while (!GameManager.instance.isFinished)
        {
            
        }
    }
    
    private void BackPropogation(Node nodeToExplore)
    {
        
    }
}
