using UnityEngine;

namespace NTO24
{
    public class InteractTask : IUnitTask
    {
        private IInteractor _unit;

        private IAnimationable _animable;

        public IInteractable Target { get; private set; }

        public Entity Entity => _unit.EntityReference;

        public bool IsComplete => !Target.IsInteractable(_unit);

        public AnimationController.Animations Animation
            => _unit.CanInteract ? AnimationController.Animations.Punch : AnimationController.Animations.Move;

        public InteractTask(IInteractor unit, IInteractable target)
        {
            _unit = unit;
            Target = target;
            _animable = unit.EntityReference is IAnimationable anim ? anim : null;
        }

        public void Update()
        {
            Move();
            Rotate();
        }

        public void Enter()
        {
            _unit.Target = Target;
            _animable?.OnAttackEvent.AddListener(Interact);
        }

        private void Move()
        {
            if (Entity is not IMovable movable)
                return;

            if (_unit.CanInteract)
                movable.Stop();
            else
                movable.MoveTo(Target.EntityReference.transform.position);

        }

        public void Rotate()
        {
            if (Entity is not IMovable movable || movable.HasPath)
                return;

            Vector3 direction = Target.EntityReference.transform.position - Entity.transform.position;

            direction.y = Entity.transform.position.y;

            var angle = Quaternion.LookRotation(direction);
            angle.x = Entity.transform.rotation.x;
            angle.z = Entity.transform.rotation.z;

            Entity.transform.rotation = Quaternion.RotateTowards(
                Entity.transform.rotation,
                angle,
                Time.deltaTime * movable.AngularSpeed
                );
        }

        private void Interact()
        {
            Target.Interact(_unit);
        }

        public void Exit()
        {
            if (Entity is IMovable movable)
                movable.Stop();

            _animable?.OnAttackEvent.RemoveListener(Interact);
            _unit.Target = null;
        }
    }
}