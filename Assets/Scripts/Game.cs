using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mafia
{
    public class Game : MonoBehaviour
    {
        public static Game ActiveGame { get; private set; }
        public static Settings DefaultSettings { get
            {
                return new Settings
                {
                    FirstDayTimerMinutes = 2f,
                    DayTimerMinutes = 5f,
                    TrialDiscourseTimerMinutes = 2f,
                    TrialSoloDefenseTimerMinutes = 0.5f,
                    TrialExtensionChanceNormalized = 0.5f,
                    TrialExtensionTimeMinutes = 1f,
                    NightDiscussionTimerMinutes = 1f,
                    NightActionTimeoutMinutes = 1f,
                    CanSleepEarly = true,
                    SkipFirstDay = false,
                    PauseDayTimerOnTrial = false,
                };
            }
        }

        public bool Complete { get; private set; }
        public Result EndResults { get; private set; }
        public int DayNightCyclesCompleted { get; private set; }
        public Settings Configuration { get; private set; }
        public bool FirstDay { get { return DayNightCyclesCompleted == 0; } }
        public Player PlayerOnTrial { get; private set; }

        public List<BaseGamePhase> SetupPhases = new List<BaseGamePhase>();
        public List<BaseGamePhase> DayPhases = new List<BaseGamePhase>();
        public List<BaseGamePhase> NightPhases = new List<BaseGamePhase>();
        public List<BaseGamePhase> PostGamePhases = new List<BaseGamePhase>();

        [ShowInInspector, ReadOnly]
        public GameMode Mode { get; private set; }
        [ShowInInspector, ReadOnly]
        public GameMode.Variant Variant { get; private set; }

        private SortedDictionary<int, List<EffectExecution>> effectQueue = new SortedDictionary<int, List<EffectExecution>>();

        [ReadOnly, PropertyOrder(1000)]
        public List<Player> Players = new List<Player>();

        private void Awake()
        {
            if (ActiveGame != null)
            {
                Destroy(gameObject);
            }
            ActiveGame = this;
        }

        private void OnDestroy()
        {
            if (ActiveGame == this)
            {
                ActiveGame = null;
            }
        }

        public void ResetGame(GameMode mode, GameMode.Variant variant, Settings config)
        {
            Players.Clear();
            EndResults = default;
            Mode = mode;
            Variant = variant;
            DayNightCyclesCompleted = 0;
            Complete = false;
            Configuration = config;
            StopAllCoroutines();
        }

        public void Begin()
        {
            if (Mode == null) return;
            StopAllCoroutines();
            StartCoroutine(GameLoop());
        }

        public void PutPlayerOnTrial(Player player)
        {
            PlayerOnTrial = player;
        }

        public void KillPlayerOnTrial()
        {
            if (PlayerOnTrial == null) return;
            PlayerOnTrial.CurrentStatus = Player.Status.Dead;
            PlayerOnTrial = null;
            CheckForVictory();
        }

        public void FreePlayerOnTrial()
        {
            PlayerOnTrial = null;
        }

        public void EneqeueEffect(EffectExecution execution)
        {
            EffectExecutionOrderEntry entry = new EffectExecutionOrderEntry
            {
                Effect = execution.Effect,
                ExecutingAction = execution.Context.ExecutingAction,
                ExecutingRole = execution.Context.ExecutingRole,
            };

            int key = Mode.ExecutionOrder.IndexOf(entry);
            if (key < 0) return;

            if (!effectQueue.ContainsKey(key) || effectQueue[key] == null)
                effectQueue[key] = new List<EffectExecution>();
            effectQueue[key].Add(execution);
        }

        public void ProcessEffectQueue()
        {
            foreach (List<EffectExecution> executions in effectQueue.Values)
            {
                foreach(EffectExecution execution in executions)
                {
                    execution.Effect.Execute(execution.Context);
                }
            }
        }

        public void CheckForVictory()
        {
            List<Player> winners = new List<Player>();
            List<Allegiance> winningTeams = new List<Allegiance>();
            foreach (Player player in Players)
            {
                ConditionContext context = new ConditionContext
                {
                    Game = this,
                    Subject = player,
                    Targets = null,
                };

                if (player.CurrentRole.WinCondition != null && player.CurrentRole.WinCondition.Check(context))
                {
                    winners.Add(player);
                }
                if (player.CurrentAllegiance.WinCondition != null && player.CurrentAllegiance.WinCondition.Check(context))
                {
                    winningTeams.Add(player.CurrentAllegiance);
                }
            }
            if (winners.Count > 0)
            {
                Result result = new Result
                {
                    PlayerWin = true,
                    Tie = winners.Count > 1,
                    WinningPlayers = winners,
                };
                EndResults = result;
                Complete = true;
            }
            else if (winningTeams.Count > 0)
            {
                Result result = new Result
                {
                    AllegianceWin = true,
                    Tie = winningTeams.Count > 1,
                    WinningAllegiances = winningTeams,
                };
                EndResults = result;
                Complete = true;
            }
        }

        private IEnumerator GameLoop()
        {
            // Init Everything
            foreach (BaseGamePhase phase in SetupPhases)
                phase.Init(this);
            foreach (BaseGamePhase phase in DayPhases)
                phase.Init(this);
            foreach (BaseGamePhase phase in NightPhases)
                phase.Init(this);
            foreach (BaseGamePhase phase in PostGamePhases)
                phase.Init(this);

            // Run Setup
            foreach (BaseGamePhase phase in SetupPhases)
            {
                phase.Begin();
                yield return phase.WaitToComplete;
            }

            // Primary Loop
            while (!Complete)
            {
                // Run Day Phases
                if (!FirstDay || !Configuration.SkipFirstDay)
                {
                    foreach (BaseGamePhase phase in DayPhases)
                    {
                        phase.Begin();
                        yield return phase.WaitToComplete;
                    }
                }

                // Run Night Phases
                foreach (BaseGamePhase phase in NightPhases)
                {
                    phase.Begin();
                    yield return phase.WaitToComplete;
                }
                DayNightCyclesCompleted++;
                CheckForVictory();
            }

            foreach (BaseGamePhase phase in PostGamePhases)
            {
                phase.Begin();
                yield return phase.WaitToComplete;
            }

            // RETURN TO MENU OR PLAY AGAIN
        }

        [System.Serializable]
        public struct Result
        {
            public bool GameFinished;
            public bool PlayerWin;
            public bool AllegianceWin;
            public bool Tie;
            public List<Player> WinningPlayers;
            public List<Allegiance> WinningAllegiances;
        }

        [System.Serializable]
        public struct Settings
        {
            public float FirstDayTimerMinutes;
            public float DayTimerMinutes;
            public float TrialDiscourseTimerMinutes;
            public float TrialSoloDefenseTimerMinutes;
            public float TrialExtensionChanceNormalized;
            public float TrialExtensionTimeMinutes;
            public float NightDiscussionTimerMinutes;
            public float NightActionTimeoutMinutes;

            public bool CanSleepEarly;
            public bool SkipFirstDay;
            public bool PauseDayTimerOnTrial;
        }
    }
}
