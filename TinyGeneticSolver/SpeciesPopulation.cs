using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TinyGeneticSolver
{
    public class SpeciesPopulation
    {
        public const int MINIMAL_POPULATION = 300;

        public List<Genome> _population = new List<Genome>();
        public SpecieGenetic _genetic;
        public Func<Genome, double> _measure;
        private Random _rnd = new Random();

        public SpeciesPopulation(SpecieGenetic genetic, int count, Func<Genome, double> measure)
        {
            _genetic = genetic;
            count = Math.Max(count, MINIMAL_POPULATION);
            for (int i = 0; i < count; i++)
                _population.Add(new Genome(genetic));
            _measure = measure;
        }

        public SpeciesPopulation(SpecieGenetic genetic, Func<Genome, double> measure)
        {
            _genetic = genetic;
            for (int i = 0; i < MINIMAL_POPULATION; i++)
                _population.Add(new Genome(genetic));
            _measure = measure;
        }

        public Genome BestGenome => _population[0];

        public void Train()
        {
            // -------- Bench
            Parallel.ForEach(_population, g =>
            {
                g.Score = _measure(g);
            });
            // -------- Sellection : sort, to get the highest score on left
            _population.Sort((a, b) => b.Score.CompareTo(a.Score));
            // -------- Combination  /crossover
            for (int i = 0; i < _population.Count / 2; i++)
            {
                var src = _rnd.Next(0, _population.Count / 100);
                var dst = _rnd.Next(_population.Count / 100, _population.Count);
                var gene = _rnd.Next(_genetic.Maximums.Length);
                _population[dst].Genes[gene] = _population[src].Genes[gene];
            }
            // -------- Perform random mutations
            for (int i = 0; i < _population.Count / 100; i++)
            {
                var g = _rnd.Next(0, _population.Count);
                var gene = _rnd.Next(_genetic.Maximums.Length);
                if (_rnd.Next(1001) > 500)
                {
                    _population[g].Genes[gene] += 1;
                    if (_population[g].Genes[gene] > _genetic.Maximums[gene])
                        _population[g].Genes[gene] = 0;
                }
                else
                {
                    _population[g].Genes[gene] -= 1;
                    if (_population[g].Genes[gene] < 0)
                        _population[g].Genes[gene] = _genetic.Maximums[gene];
                }
            }
            // -------- Shuffle and sort to get the best genome diversity
            Shuffle(_population);
            _population.Sort((a, b) => b.Score.CompareTo(a.Score));
        }

        public Genome Scan()
        {
            var _lock = new object();
            var incr = new GenomeIncrementor(_genetic);
            double _score = double.MinValue;
            Genome winner = null;
            Parallel.ForEach(incr.All(), g =>
            {
                g.Score = _measure(g);
                if (g.Score > _score)
                    lock (_lock)
                    {
                        if (g.Score > _score)
                        {
                            winner = g;
                            _score = g.Score;
                        }
                    }
            });
            return winner;
        }

        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
