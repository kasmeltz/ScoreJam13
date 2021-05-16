namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class ProbabilityChooser<T>
    {
        #region Constructors

        public ProbabilityChooser()
        {
            Probabilities = new List<float>();
            Items = new List<T>();
        }

        #endregion

        #region Members

        protected List<float> Probabilities { get; set; }

        protected List<T> Items { get; set; }

        protected float MaximumProbability { get; set; }

        #endregion

        #region Public Methods

        public void AddItem(T item, float probability)
        {
            Items
                .Add(item);

            MaximumProbability += probability;

            Probabilities
                .Add(MaximumProbability);
        }

        public T ChooseItem()
        {
            if (Items.Count == 0)
            {
                return default(T);
            }

            float roll = Random
                .Range(0, MaximumProbability);

            for (int i = 0; i < Probabilities.Count; i++)
            {
                if (roll <= Probabilities[i])
                {
                    return Items[i];
                }
            }

            return Items
                .LastOrDefault();
        }

        #endregion
    }
}