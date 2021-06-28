using Assets.Game.Scripts.Common.Enum;
using UnityEngine;
using D = System.Diagnostics.Debug;

namespace Assets.Game.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerControls : MonoBehaviour
    {
        private readonly bool CanMove = true;
        public Animator Anim;
        public Direction CurrentFacingDirection;
        public bool FacingRight = true;
        private Rigidbody2D PlayerRigidbody2D;
        public float Speed = 2.0f;

        private void Start()
        {
            CurrentFacingDirection = Direction.Down;
            PlayerRigidbody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
            Anim = (Animator)GetComponent(typeof(Animator));
        }

        private void Update()
        {
            if (CanMove)
            {
                CheckInput();
            }
        }

        private void CheckInput()
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            var moveVector = new Vector2(h * Speed, v * Speed);
            PlayerRigidbody2D.velocity = new Vector2(moveVector.x, moveVector.y);

            D.Assert(Anim != null, "Missing Animator");
            CurrentFacingDirection = GetFacingDirection(moveVector, CurrentFacingDirection);

            Anim.SetInteger("FaceDirection", (int)CurrentFacingDirection);

            //Anim.SetFloat("Speed", moveVector.x != 0.0f ? 1.0f : 0.0f);

            var tempMoveVectorVal = GetSpeed(moveVector);
            Debug.Log("tempMoveVectorVal is set to " + tempMoveVectorVal);
            Anim.SetFloat("Speed", tempMoveVectorVal);
            D.Assert(Anim.GetFloat("Speed") == tempMoveVectorVal, "Speed Does not match");
        }

        private void Flip()
        {
            FacingRight = !FacingRight;
            var scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            scale.x *= -1;

            transform.localScale = scale;
        }

        private Direction GetFacingDirection(Vector2 moveDelta, Direction previous = Direction.Down)
        {
            var x = Mathf.FloorToInt(moveDelta.x);
            var y = Mathf.FloorToInt(moveDelta.y);

            //no change
            if (x == 0 && y == 0) return previous;
            if (x > 0 && y == 0) return Direction.Right;
            if (x < 0 && y == 0) return Direction.Left;
            if (x == 0 && y > 0) return Direction.Up;
            if (x == 0 && y < 0) return Direction.Down;

            return previous;
        }

        private Direction GetFacingDirectionFix(Vector2 moveDelta, Direction previous = Direction.Down)
        {
            var x = moveDelta.x;
            var y = moveDelta.y;

            //no change
            if (x == 0 && y == 0) return previous;
            if (x > 0 && y == 0) return Direction.Right;
            if (x < 0 && y == 0) return Direction.Left;
            if (x == 0 && y > 0) return Direction.Up;
            if (x == 0 && y < 0) return Direction.Down;

            return previous;
        }

        public float GetSpeed(Vector2 moveDelta) =>
            (moveDelta.x != 0f || moveDelta.y != 0f) ? 1.0f : 0.0f;

    }
}