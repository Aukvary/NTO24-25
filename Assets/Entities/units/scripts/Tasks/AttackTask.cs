using UnityEngine;

namespace NTO24
{
    public class AttackTask : IUnitTask
    {
        private IAttacker _unit;

        private IAnimationable _animable;

        public IHealthable Target { get; private set; }

        public Entity Entity => _unit.EntityReference;

        public bool IsComplete
            => !Target.Alive || (!_unit.CanAttack && Entity is not IMovable);

        public AnimationController.Animations Animation
            => _unit.CanAttack ? AnimationController.Animations.Punch : AnimationController.Animations.Move;

        public AttackTask(IAttacker unit, IHealthable target)
        {
            _unit = unit;
            Target = target;
            _animable = unit.EntityReference is IAnimationable anim ? anim : null;
        }

        public void Enter()
        {
            _unit.Target = Target;
            _animable?.OnAttackEvent.AddListener(Attack);
        }

        public void Update()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            if (Entity is not IMovable movable)
                return;

            if (_unit.CanAttack)
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

        private void Attack()
        {
            Target?.Damage(_unit.Damage, _unit.EntityReference);
        }

        public void Exit()
        {
            if (Entity is IMovable movable)
                movable.Stop();

            Target = null;

            _animable?.OnAttackEvent.RemoveListener(Attack);
            _unit.Target = null;
        }
    }
}