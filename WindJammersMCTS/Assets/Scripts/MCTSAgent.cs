using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MCTSAgent : MonoBehaviour
{
    
    class MCTSNode
    {
        public MCTSNode parent;
        public List<MCTSNode> children;
        public string action;
        public GameState State;
        public int nbWin;
        public int nbPlayed;
        public float value; // nbWin/nbPlayed

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
        MCTSNode startNode = new MCTSNode(GMInstance.GetCurrentGameState());
        for (int i = 0; i < numIteration; i++)
        {
            MCTSNode selectedNode = Selection(startNode);
            MCTSNode newNode = Expansion(selectedNode);
            int numWin = Simulation(newNode, numSim);
            BackPropogation(newNode, numWin, numSim);
        }

        GMInstance.ExecuteActionForEnnemy(GMInstance.State, bestAction); // Play ennemy move on real GameState
    }

    // 1 Select a node if we have more valid feasible moves or it is terminal 
    private MCTSNode Selection(MCTSNode node)
    {
        string mode = "EXPLOITATION";
        
        if(Random.Range(1, 10) > 3) // 70% de chance d'exploration et 30 de chance d'exploitation
            mode = "EXPLORATION";

        while (!isLeaf(node))
        {
            if (mode == "EXPLORATION")
            {
                // Exploration - Choix random
            }
            else if (mode == "EXPLOITATION")
            {
                // Exploitation - On descent au plus bas possible en fonction du plus gros score
            }
        }
        
        return node; // tmp
    }

    // 2 Expand a node by creating a new move and returning the node
    private MCTSNode Expansion(MCTSNode selectedNode)
    {
        GameState simulateState = selectedNode.State; // GameState copy
        GameManager.instance.ExecuteActionForEnnemy(selectedNode.State, selectedNode.action);
        
        // Créer nouveau node enfant (définir son parent au selected)
        return selectedNode; // tmp
    }
    
    // Simulate a game with a given policy and return the number of win
    private int Simulation(MCTSNode node, int numSim)
    {
        int numWin = 0;
        for (int i = 0; i < numSim; i++)
        {
            int ennemyScore = node.State.ennemyScore;
            while (!node.State.isScored) // Until goal scored
            {
                string[] actions = node.State.getPossibleAction("ENNEMY");
                string selectedAction = node.State.getRandomAction(actions);
                GMInstance.ExecuteActionForEnnemy(node.State, selectedAction); // Move ennemy (handle border collisions)
                
                actions = node.State.getPossibleAction("PLAYER");
                selectedAction = node.State.getRandomAction(actions);
                GMInstance.ExecuteActionForPlayer(node.State, selectedAction); // Move player (handle border collisions)
                
                FrisbeeController.instance.checkBorderCollisions(node.State);
                FrisbeeController.instance.checkCatch(node.State);
                FrisbeeController.instance.moveOrStick(node.State);
                GMInstance.checkGoals(node.State);
            }
            
            if (node.State.ennemyScore > ennemyScore) numWin++;

        }
        return numWin;
    }
    
    // 4 Update score to determine best action
    private void BackPropogation(MCTSNode nodeToExplore, int numWin, int numSim)
    {
        // !Node.isFeuille
        // remonte
        
        nodeToExplore.parent.nbWin = 0;
        nodeToExplore.parent.nbPlayed = 0;
        for (int i = 0; i < nodeToExplore.parent.children.Count; i++) {
            nodeToExplore.parent.nbWin += nodeToExplore.parent.children[i].nbWin;
            nodeToExplore.parent.nbPlayed += nodeToExplore.parent.children[i].nbPlayed;
        }
        
        nodeToExplore.parent.value = nodeToExplore.parent.nbWin / nodeToExplore.parent.nbPlayed;
    }
    
    private bool isLeaf(MCTSNode node)
    {
        return node.children.Count <= 0;
    }

    private bool hasParent(MCTSNode node)
    {
        return node.parent != null;
    }
}
