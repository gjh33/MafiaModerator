using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    [CreateAssetMenu(menuName = "Mafia/Effects/Switch")]
    public class SwitchEffect : BaseEffect
    {
        public List<BaseCondition> Conditions;
        public List<BaseEffect> EffectsIfConditionsPass;
        public List<BaseEffect> EffectsIfConditionsFails;
        public override void Execute(EffectContext context)
        {
            ConditionContext cContext = new ConditionContext
            {
                Game = context.Game,
                Subject = context.ExecutingPlayer,
                Targets = context.Targets,
            };

            bool pass = true;
            foreach (BaseCondition condition in Conditions)
            {
                if (!condition.Check(cContext))
                {
                    pass = false;
                    break;
                }
            }

            if (pass)
            {
                foreach(BaseEffect effect in EffectsIfConditionsPass)
                {
                    effect.Execute(context);
                }
            }
            else
            {
                foreach (BaseEffect effect in EffectsIfConditionsFails)
                {
                    effect.Execute(context);
                }
            }
        }
    }
}
