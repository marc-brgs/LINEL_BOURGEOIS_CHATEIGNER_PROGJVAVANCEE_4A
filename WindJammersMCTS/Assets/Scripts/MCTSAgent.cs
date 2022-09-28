using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MCTSAgent : MonoBehaviour
{
    class MCTSNode
    {
        public MCTSNode parentNode;
        public List<MCTSNode> childrenNodes;
        public string action;
        public GameState State;
        public int nbWin;
        public int nbPlayed;
        public bool isLeaf;
        // private float value = nbWin / nbPlayed;
    }
    
    private float max;
    private string bestAction;
    
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
    
    private string[] GetPossibleAction()
    {
        string[] possibleActions = {"UP", "DOWN", "LEFT", "RIGHT", "SHOOT"};
        return possibleActions;
    }
    
    //#1. Select a node if 1: we have more valid feasible moves or 2: it is terminal 
    private void Selection(MCTSNode node)
    {
        // random exploit explo
       /* while (node.Children.Count > 0)
        {
            exploitation - on descent au plus bas possible en fonction du plus gros score
            exploration - choix random
        }*/
        
    }

    //#2. Expand a node by creating a new move and returning the node
    private void Expansion(MCTSNode selectedNode)
    {
        
        // copie du gamestate
        GameState simulateState = selectedNode.State;
        // playmove
        GameManager.instance.executeAction(selectedNode.action);
        
        // créer nouveau node enfant (définir son parent au selected)
    }
    
    //#3. Roll-out. Simulate a game with a given policy and return the value
    private int Simulation(MCTSNode node, int numSim)
    {
        while (!GameManager.instance.isFinished)
        {
            
        }
        return 0;
    }
    
    private void BackPropogation(MCTSNode nodeToExplore, int numWin, int numPlayed)
    {
        // !Node.isFeuille
        // remonte
    }
    
    // GameManagerInstace.PlayMove(simulationGameState, player, selectedMove)
}