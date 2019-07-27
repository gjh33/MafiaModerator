using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public abstract class BaseGamePhase : MonoBehaviour
    {
        private WaitForCompletion _cachedCompletion;
        public WaitForCompletion WaitToComplete
        {
            get
            {
                if (_cachedCompletion == null) _cachedCompletion = new WaitForCompletion(this);
                return _cachedCompletion;
            }
        }

        public bool IsDone { get; private set; }

        public event System.Action Began;
        public event System.Action Completed;

        protected Game game;

        public void Init(Game game)
        {
            this.game = game;
            IsDone = true;
        }

        public void Begin()
        {
            IsDone = false;
            OnBegin();
            Began?.Invoke();
        }

        public void Complete()
        {
            OnComplete();
            IsDone = true;
            Completed?.Invoke();
        }

        protected virtual void OnInitialize() { }
        protected abstract void OnBegin();
        protected abstract void OnComplete();

        public class WaitForCompletion : CustomYieldInstruction
        {
            private BaseGamePhase phase;

            public override bool keepWaiting
            {
                get
                {
                    return !phase.IsDone;
                }
            }

            public WaitForCompletion(BaseGamePhase phase)
            {
                this.phase = phase;
            }
        }
    }
}
