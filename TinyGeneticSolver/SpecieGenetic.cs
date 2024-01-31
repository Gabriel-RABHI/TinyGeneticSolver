using System;

namespace TinyGeneticSolver
{
    public class SpecieGenetic
    {
        private int[] _maxes;
        private string[][] _labels;

        public SpecieGenetic(int[] maxes, string[][] labels)
        {
            _maxes = maxes;
            _labels = labels;
            foreach (var g in maxes)
                if (g < 2)
                    throw new Exception("A gene must have at least two values.");
        }

        public long CombinationCount
        {
            get
            {
                long n = _maxes[0];
                if (_maxes.Length > 1)
                    for (int i = 1; i < _maxes.Length; i++)
                        n *= _maxes[i];
                return n;
            }
        }

        public int[] Maximums => _maxes;

        public string[][] Labels => _labels;
    }
}
