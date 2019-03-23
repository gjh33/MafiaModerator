using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Action")]
    public class Action : ScriptableObject
    {
        public enum TargetType { Self, Player, MultiplePlayers } // Effects the options the player gets when executing
        public string DisplayName;
        public TargetType Targeting = TargetType.Self;

        public List<PlayerCondition> Conditions = new List<PlayerCondition>();
        public List<BaseEffect> Effects = new List<BaseEffect>();

        public bool CheckConditions(ConditionContext context)
        {
            foreach (PlayerCondition condition in Conditions)
            {
                if (!condition.Check(context)) return false;
            }
            return true;
        }

        /// <summary>
        /// Queues up the effects to the current night
        /// </summary>
        public void EnqueueEffects(ActionContext context)
        {
            // if (game.CurrentExecutionPipeline == null) return;
            foreach (BaseEffect effect in Effects)
            {
                EffectContext eContext = new EffectContext
                {
                    Game = context.Game,
                    ExecutingPlayer = context.PerformingPlayer,
                    ExecutingRole = context.PerformingRole,
                    ExecutingAction = this,
                    Targets = context.Targets,
                };
                // context.Game.CurrentExecutionPipeline.QueueEffect(effect, eContext);
            }
        }
    }
}
