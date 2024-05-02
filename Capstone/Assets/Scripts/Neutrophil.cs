using UnityEngine;

public class Neutrophil : Tower
{
    protected override void Attack()
    {   
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, germsLayer);
        
        if (colliders.Length == 0)
            return;
        else if (colliders.Length == 1)
            AttractGerm(colliders[0].GetComponent<Germ>());
        else
        {
            Germ furthestGerm = null;
            int furthestWaypointIndex = -1;

            foreach (Collider2D collider in colliders)
            {
                Germ germ = collider.GetComponent<Germ>();
                if (germ.currentWaypointIndex > furthestWaypointIndex)
                {
                    furthestGerm = germ;
                    furthestWaypointIndex = germ.currentWaypointIndex;
                }
            }

            AttractGerm(furthestGerm);
        }

        base.Attack();
    }

    private void AttractGerm(Germ germ)
    {
        if (germ.currentWaypointIndex == int.MaxValue - 1)
            return;
        
        germ.currentWaypointIndex = int.MaxValue - 1;
        germ.currentWaypoint = transform.position;
    }
}
