using System;

namespace TinyGeneticSolver
{

    public class Genome
    {
        private Random _random = new Random(0);
        private int[] _genes;
        private SpecieGenetic _genetic;

        public Genome(SpecieGenetic genetic, bool random = true)
        {
            _genetic = genetic;
            _genes = new int[_genetic.Maximums.Length];
            if (random)
                for (int i = 0; i < _genes.Length; i++)
                    _genes[i] = _random.Next(_genetic.Maximums[i]);
        }

        public double Score { get; set; } = 0;

        public string Infos { get; set; } = "";

        public int[] Genes => _genes;

        public Genome(SpecieGenetic genetic, int[] genes)
        {
            _genetic = genetic;
            _genes = (int[])genes.Clone();
        }

        public Genome Increment()
        {
            lock (this)
            {
                Increment(0);
                return new Genome(_genetic, Genes);
            }
        }

        private void Increment(int i)
        {
            if (Genes[i] == _genetic.Maximums[i] - 1)
            {
                Genes[i] = 0;
                if (i < Genes.Length - 1)
                    Increment(i + 1);
            }
            else
            {
                Genes[i]++;
            }
        }

        public Genome Clone => new Genome(_genetic, (int[])_genes.Clone());

        public override string ToString()
        {
            string s = $"Score = {Score} <{Infos}> ";
            if (_genetic.Labels != null)
            {
                s += " [ ";
                int n = 0;
                foreach (int g in _genes)
                {
                    int l = _genetic.Labels[Math.Min(n, _genetic.Labels.Length - 1)].Length;
                    string added = _genetic.Labels[Math.Min(n, _genetic.Labels.Length - 1)][Math.Min(g, l - 1)];
                    if (added.EndsWith("="))
                        s += added + " " + g.ToString() + " - ";
                    else
                        s += added + " - ";
                    n++;
                }
                s += " ] ";
            }
            return s;
        }
    }
}
