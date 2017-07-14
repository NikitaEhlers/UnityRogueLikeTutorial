using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start () {
        boxCollider = GetComponent<BoxCollider2D>();

        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;
	}

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        //start position to move from
        Vector2 start = transform.position;

        //calculate end position based on the direction parameters 
        Vector2 end = start + new Vector2(xDir, yDir);

        //disable boxCollider so linecast doesn't hit object's own collider
        boxCollider.enabled = false;

        //cast line from start point to end point checking collision on blockingLayer
        hit = Physics2D.Linecast(start, end, blockingLayer);

        //re-enable boxCollider after linecast
        boxCollider.enabled = true;

        //check if anything was hit
        if (hit.transform == null)
        {
            //nothing hit, start smoothMovement co-routine
            StartCoroutine(SmoothMovement(end));

            //return move was successful
            return true;
        }

        //if something was hit, return false
        return false;
    }

    //co routine for moving units from one space to the next
    protected IEnumerator SmoothMovement (Vector3 end)
    {
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //while remaining distance is greater than almost zero
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end based on the move time
            //Vector3.moveTowards moves a point in a straight line towards a target point
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            //moving
            rb2D.MovePosition(newPosition);

            //recalcualte remaining distance after the move
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //wait for the next frame before reevaluating the condition of the loop
            yield return null;
        }
    }

    protected virtual void AttemptMove <T> (int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;

        //check if move is possible
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
        {
            //nothing hit no need to execute further
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        //canMove is false and hitComponent is not null means movingObject is blocked and hit something interactable
        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;




    
}
