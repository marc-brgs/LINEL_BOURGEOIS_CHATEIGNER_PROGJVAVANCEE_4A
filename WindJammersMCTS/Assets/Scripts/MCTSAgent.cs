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
        private float value; // nbWin/nbPlayed

        public MCTSNode(GameState state)
        {
            this.State = state;
        }
    }

    private GameManager GMInstance = GameManager.instance; 
    private float max;
    private string bestAction;
    
    private int numIteration = 100; 
    private int numSim = 10;
    
    

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

    private void ComputeMCTS()
    {
        /*MCTSNode startNode = new MCTSNode(GMInstance.GetCurrentGameState());
        for (int i = 0; i < numIteration; i++)
        {
            MCTSNode selectedNode = Selection(startNode);
            MCTSNode newNode = Expansion(selectedNode);
            int numWin = Simulation(newNode, numSim);
            BackPropogation(newNode, numWin, numSim);
        }*/
        
        // GameManagerInstance.PlayMove(simulationGameState, player, selectedMove)
    }
    
    private bool checkIsLeaf(MCTSNode node)
    {
        return true;
    }
    
    //#1. Select a node if 1: we have more valid feasible moves or 2: it is terminal 
    private MCTSNode Selection(MCTSNode node)
    {
        // random exploit explo
       /* while (node.Children.Count > 0)
        {
            exploitation - on descent au plus bas possible en fonction du plus gros score
            exploration - choix random
        }*/

       return node; // tmp
    }

    //#2. Expand a node by creating a new move and returning the node
    private MCTSNode Expansion(MCTSNode selectedNode)
    {
        
        // copie du gamestate
        GameState simulateState = selectedNode.State;
        // playmove
        GameManager.instance.ExecuteActionForEnnemy(selectedNode.State, selectedNode.action);
        
        // créer nouveau node enfant (définir son parent au selected)
        return selectedNode; // tmp
    }
    
    //#3. Roll-out. Simulate a game with a given policy and return the value
    private int Simulation(MCTSNode node, int numSim)
    {
       /* int numWin = 0;
        for (int i = 0; i < numSim; i++)
        {
            while (!node.State.getScored()) // but
            {
            }
        }
        return numWin;*/


        int numWin = 0;
        for (int i = 0; i < numSim; i++)
        {
            while (!node.State.isFinished) // but
            {
                // ExecuteAction(State.getRandomAction);
            }
        }
        return numWin;



    }
    
    private void BackPropogation(MCTSNode nodeToExplore, int numWin, int numSim)
    {
        // !Node.isFeuille
        // remonte
    }
}
