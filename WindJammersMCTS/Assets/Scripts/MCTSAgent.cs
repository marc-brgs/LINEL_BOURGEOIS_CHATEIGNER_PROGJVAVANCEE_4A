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
        public bool isTerminated; // Not implemented but should have been
        
        public MCTSNode(GameState state)
        {
            this.State = state;
            this.nbWin = 0;
            this.nbPlayed = 0;
            this.value = 0;
            this.children = new List<MCTSNode> {};
        }
    }

    public GameManager GMInstance; 
    private float bestValue;
    private string bestAction;
    
    private int numIteration = 1; // Debug value
    private int numSim = 1; // Debug value
    private int maxMoves = 10; // Anti infinite loop for Simulation
    
    
    public void ComputeMCTS() // DONE
    {
        MCTSNode startNode = new MCTSNode(GMInstance.State);
        for (int i = 0; i < numIteration; i++)
        {
            MCTSNode selectedNode = Selection(startNode);
            MCTSNode newNode = Expansion(selectedNode);
            int numWin = Simulation(newNode, numSim);
            BackPropogation(newNode, numWin, numSim);
        }

        // Select best action (among 2nd floor nodes)
        bestValue = 0;
        int childrenLength = startNode.children.Count;
        for (int i = 0; i < childrenLength; i++)
        {
            if (startNode.children[i].value > bestValue)
            {
                bestAction = startNode.action;
                bestValue = startNode.children[i].value;
            }
        } 
        
        // Play ennemy move on real GameState with best action
        GMInstance.ExecuteActionForEnnemy(GMInstance.State, bestAction);
        Debug.Log("MCTS move : " + bestAction);
    }

    /**
     *  #1 - Select a node if we have more valid feasible moves or it is terminal
     */ 
    private MCTSNode Selection(MCTSNode node) // DONE
    {
        string mode = "EXPLOITATION";
        
        if(Random.Range(1, 10) > 3) // 70% chance exploring, 30% chance exploitating (replace with curve interpolation for better results)
            mode = "EXPLORATION";

        while (!isLeaf(node)) // or more possible action
        {
            if (mode == "EXPLORATION")
            {
                // Select node randomly
                if (hasPossibleActionLeft(node) && Random.Range(1, 10) > 5) // 50% chance to go wide (create another action)
                {
                    return node;
                }
                else // 50% chance to go deeper (more precised)
                {
                    int i = Random.Range(0, node.children.Count - 1);
                    node = node.children[i];
                }
            }
            else if (mode == "EXPLOITATION")
            {
                // Select node depending of best value (go full deep)
                int bestChild = 0;
                int childrenLength = node.children.Count;
                for (int i = 1; i < childrenLength; i++)
                {
                    if (node.children[i].value > node.children[i].value)
                        bestChild = i;
                }
                
                node = node.children[bestChild];
            }
        }
        
        return node; // tmp
    }
    
    /**
     *  #2 - Expand a node by creating a new move and returning the node
     */
    private MCTSNode Expansion(MCTSNode selectedNode) // DONE
    {
        GameState simulatedState = new GameState(selectedNode.State); // GameState copy

        simulatedState = GMInstance.ExecuteActionForEnnemy(simulatedState, selectedNode.action);
        
        // Create new child node
        MCTSNode newNode = new MCTSNode(simulatedState); // Performance loss (pre-alloc to fix / object pulling)
        newNode.action = getRandomPossibleActionLeft(newNode);

        // Link nodes together
        newNode.parent = selectedNode;
        selectedNode.children.Add(newNode);
        
        return newNode;
    }
    
    /**
     *  #3 - Simulate a game with a given policy and return the number of win
     */
    private int Simulation(MCTSNode node, int numSim) // DONE
    {
        GameState simulatedGameState = new GameState();
        int preEnnemyScore = node.State.ennemyScore; // Used to determined win
        int numWin = 0;

        for (int i = 0; i < numSim; i++)
        {
            int move = 0;
            simulatedGameState.copyGameState(node.State);
            while (!simulatedGameState.isScored && move < maxMoves) // Until goal scored or move limit exceeded
            {
                string[] actions = simulatedGameState.getPossibleAction("ENNEMY");
                string selectedAction = simulatedGameState.getRandomAction(actions);
                simulatedGameState = GMInstance.ExecuteActionForEnnemy(simulatedGameState, selectedAction); // Move ennemy (handle border collisions)
                
                actions = simulatedGameState.getPossibleAction("PLAYER");
                selectedAction = simulatedGameState.getRandomAction(actions);
                simulatedGameState = GMInstance.ExecuteActionForPlayer(simulatedGameState, selectedAction); // Move player (handle border collisions)
                
                // Simulate game interactions
                FrisbeeController.instance.checkBorderCollisions(simulatedGameState);
                FrisbeeController.instance.checkCatch(simulatedGameState);
                FrisbeeController.instance.moveOrStick(simulatedGameState);
                GMInstance.checkGoals(simulatedGameState);

                move++;
            }
            
            if (simulatedGameState.ennemyScore > preEnnemyScore) numWin++;

        }
        return numWin;
    }
    
    /**
     *  #4 - Update score to determine best action
     */
    private void BackPropogation(MCTSNode nodeToExplore, int numWin, int numSim) // DONE
    {
        nodeToExplore.nbWin = numWin;
        nodeToExplore.nbPlayed = numSim;
        nodeToExplore.value = numWin / numSim;
        
        // Recalculate all impacted nodes (all his parents) with all of their own children information
        while (hasParent(nodeToExplore))
        {
            MCTSNode nodeParent = nodeToExplore.parent;
            nodeParent.nbWin = 0;
            nodeParent.nbPlayed = 0;
            
            int childrenLength = nodeParent.children.Count;
            for (int i = 0; i < childrenLength; i++)
            {
                nodeParent.nbWin += nodeParent.children[i].nbWin;
                nodeParent.nbPlayed += nodeParent.children[i].nbPlayed;
                nodeParent.value = nodeParent.nbWin / nodeToExplore.nbPlayed;
            }
        }
    }
    
    private bool isLeaf(MCTSNode node)
    {
        return node.children.Count <= 0;
    }

    private bool hasParent(MCTSNode node)
    {
        return node.parent != null;
    }

    private bool hasPossibleActionLeft(MCTSNode node)
    {
        string[] totalPossibleActions = node.State.getPossibleAction("ENNEMY");
        if(node.children.Count == totalPossibleActions.Length) return false;
        return true;
    }

    private string getRandomPossibleActionLeft(MCTSNode node)
    {
        string[] totalPossibleActions = node.State.getPossibleAction("ENNEMY");

        int numPossibleAction = totalPossibleActions.Length;
        int childrenLength = node.children.Count;

        for (int i = 0; i < numPossibleAction; i++)
        {
            bool found = false;
            for (int j = 0; j < childrenLength; j++)
            {
                if (totalPossibleActions[i] == node.children[j].action)
                {
                    found = true;
                    break;
                }
            }

            if (!found) return totalPossibleActions[i];
        }
        
        return "";
    }
}
