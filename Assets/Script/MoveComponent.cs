using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    //player srcipt ref
    [SerializeField] private Player player;

    //duration to go from a platform to another
    public float MoveDuration;

    //bool to check if all move coroutine are finished in player manager
    public bool IsCoroutineMoveRunning = false;

    //Add all movement that affect the player to move only once to final destination.
    public Vector2Int CumulMove = Vector2Int.zero;

    private MoveOrigin Origin;

    /// <summary>
    /// Increase CumulMove  and update future coordinates
    /// </summary>
    /// <param name="dir">Direction to move</param>
    /// <param name="origin">Origin of the move</param>
    public void Move(Direction dir, MoveOrigin origin)
    {
        switch (dir)
        {
            case Direction.Up:
                CumulMove += Vector2Int.up;
                break;
            case Direction.Down:
                CumulMove += Vector2Int.down;
                break;
            case Direction.Left:
                CumulMove += Vector2Int.left;
                break;
            case Direction.Right:
                CumulMove += Vector2Int.right;
                break;
            default:
                Debug.LogError("Direction null in MoveComp " + gameObject.name);
                break;
        }

        Origin = origin;

        player.FuturCoordinates = player.Coordinates + CumulMove;
    }

    /// <summary>
    /// Make the player move smoothly to it's destination. Check for collision with other player and update movement according to it.
    /// </summary>
    /// <remarks>Set IsCoroutineMoveRunning to True at the start of the function and to false once finished.</remarks>
    public IEnumerator MoveVisualCoroutine()
    {
        IsCoroutineMoveRunning = true; //coroutine start

        //activate right trigger for animation when move from push or normal move
        if (Origin == MoveOrigin.Push)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3Int(-CumulMove.x, 0, -CumulMove.y));
            player.AnimatorGetPushedTrigger();
        }
        else if (Origin == MoveOrigin.Normal)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3Int(CumulMove.x, 0, CumulMove.y));
            player.AnimatorJumpTrigger();
        }

        //damage the platform the player quit
        ArenaManager.Instance.DamagePlatformAtCoordinate(player.Coordinates);

        
        Vector3 startPos = transform.position;//world position of player
        float playerHalfSize = player.PlayerCapsule.bounds.extents.x;//half the player size
        Vector3 dist = ArenaManager.Instance.GetPositionAtCoordinates(player.FuturCoordinates) - ArenaManager.Instance.GetPositionAtCoordinates(player.LastCoordinates);//world distance between start position and wanted end position
        Vector3 moveDirection = dist.normalized; //direction of the move

        Player playerCollideMove = PlayerManager.Instance.CheckCollisionDuringMove(player);
        Player playerCollidePresent = PlayerManager.Instance.CheckCollisionOnDestinationPlatformWithAlreadyPresentPlayer(player);
        List<Player> playersCollideMoving = PlayerManager.Instance.CheckCollisionOnDestinationPlatformWithMovingPlayer(player);
        //Check if the player collide with another player. (p1 go to platform2 and p2 go to platform1)
        if (playerCollideMove)
        {
            //reduce distance to not completely overlap other player when colliding
            Vector3 offset = moveDirection * playerHalfSize;

            //players collide between the 2 platforms and go back to starting platform
            yield return StartCoroutine(CollisionMoveCoroutine(startPos, startPos + dist/2f - offset, MoveDuration, playerCollideMove));

            //when player go back to starting platform, check if another player moved here
            if (!PlayerManager.Instance.CheckPlayerDie(player))
            {
                yield return CheckEmptyStartingPositionCoroutine();
            }
        }
        
        //Check if the player go to an already occupied platform.
        else if (playerCollidePresent)
        {
            //reduce distance to not completely overlap other player when colliding
            Vector3 offset = moveDirection * playerHalfSize * 2; //other player not moving so 2*playerSize offset

            //players collide on destination platforms and go back to starting platform
            yield return StartCoroutine(CollisionMoveCoroutine(startPos, startPos + dist - offset, MoveDuration, playerCollidePresent));

            //when player go back to starting platform, check if another player moved here
            if (!PlayerManager.Instance.CheckPlayerDie(player))
            {
                yield return CheckEmptyStartingPositionCoroutine();
            }
        }
        
        //Check if the player go to a platform at the same time as another player.
        else if (playersCollideMoving.Count != 0)
        {
            //reduce distance to not completely overlap other player when colliding
            Vector3 offset = moveDirection * playerHalfSize;

            //players collide on destination platforms and go back to starting platform
            yield return StartCoroutine(CollisionMoveCoroutine(startPos, startPos + dist - offset, MoveDuration, playersCollideMoving));

            //when player go back to starting platform, check if another player moved here
            if (!PlayerManager.Instance.CheckPlayerDie(player))
            {
                yield return CheckEmptyStartingPositionCoroutine();
            }
        }
        
        //else, no collision, player move normally to destination platform
        else
        {
            yield return StartCoroutine(SingleMoveCoroutine(startPos, startPos + dist, MoveDuration));

            player.Coordinates = player.FuturCoordinates;
            PlayerManager.Instance.CheckPlayerDie(player);
        }

        IsCoroutineMoveRunning = false; //coroutine end

        //reset CumulMove
        CumulMove = Vector2Int.zero;
    }

    //// IMPORTANT ////
    //The following 2 s call themselves back and forth if player chain-collide.
    //However, it is finite as the maximum is reached once all player goes back to their starting platform.

    /// <summary>
    /// Check if the starting platform of the player is occupied after colliding with another player.
    /// If another player if found, make him go back to his starting platform
    /// </summary>
    private IEnumerator CheckEmptyStartingPositionCoroutine()
    {
        Player other = PlayerManager.Instance.GetPlayerAtPosition(player.LastCoordinates, player);
        if (other)
        {
            //other player is present on starting platform. get pushed back to his starting platform.
            player.CreateParticleCollide(new List<Player>() {other});
            yield return StartCoroutine(other.MoveComp.GoBackCoroutine());
        }
    }

    /// <summary>
    /// Player smoothly move back to it's starting platform.
    /// Then check if a player occupy this platform.
    /// </summary>
    public IEnumerator GoBackCoroutine()
    {
        Vector3 start = transform.position;
        Vector3 end = ArenaManager.Instance.GetPositionAtCoordinates(player.LastCoordinates);
        yield return StartCoroutine(SingleMoveCoroutine(start, end, MoveDuration));

        player.Coordinates = player.LastCoordinates;
        //when player go back to starting platform, check if another player moved here
        yield return StartCoroutine(CheckEmptyStartingPositionCoroutine());
    }

    /// <summary>
    /// Smoothly move a player from <paramref name="Start"/> to <paramref name="End"/> over <paramref name="Duration"/>.
    /// </summary>
    /// <param name="Start">Starting point of the move.</param>
    /// <param name="End">Ending point of the move.</param>
    /// <param name="Duration">Duration of the move.</param>
    private IEnumerator SingleMoveCoroutine(Vector3 Start, Vector3 End, float Duration)
    {
        float alpha = 0;
        do
        {
            alpha += Time.deltaTime / Duration;
            transform.position = Vector3.Lerp(Start, End, alpha);
            yield return null;
        } while (alpha < 1);
    }

    /// <summary>
    /// Smoothly move a player from <paramref name="Start"/> to <paramref name="End"/> then back to <paramref name="Start"/> over <paramref name="Duration"/>.
    /// </summary>
    /// <param name="Start">Starting point and ending point of the move.</param>
    /// <param name="Collision">Point where collision between player occur.</param>
    /// <param name="Duration">Duration of the entire move.</param>
    private IEnumerator CollisionMoveCoroutine(Vector3 Start, Vector3 Collision, float Duration, List<Player> players)
    {
        yield return StartCoroutine(SingleMoveCoroutine(Start, Collision, Duration / 2f));

        player.CreateParticleCollide(players);

        yield return StartCoroutine(SingleMoveCoroutine(Collision, Start, Duration / 2f));

        yield return new WaitForSeconds(0.03f);//All collision should last a bit longer than a normal move to avoid a
                                              //problem where collision player get back on starting platform before another
                                              //player normal causing the 2 of them to be on the same,platform
    }

    private IEnumerator CollisionMoveCoroutine(Vector3 Start, Vector3 Collision, float Duration, Player player)
    {
        yield return StartCoroutine(CollisionMoveCoroutine(Start, Collision, Duration, new List<Player>() {player}));
    }
}

//direction enum
public enum Direction {Up, Down, Left, Right }

//direction enum
public enum MoveOrigin {Normal, Push}