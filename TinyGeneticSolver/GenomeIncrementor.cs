using System.Collections.Generic;

namespace TinyGeneticSolver
{
    public class GenomeIncrementor
    {
        private SpecieGenetic _genetic;
        private Genome _lastGenome;

        public GenomeIncrementor(SpecieGenetic genetic)
        {
            _genetic = genetic;
            _lastGenome = new Genome(_genetic, false);
        }

        public IEnumerable<Genome> All()
        {
            for (int i = 0; i < _genetic.CombinationCount; i++)
            {
                var genome = _lastGenome.Increment();
                yield return genome;
            }
        }
    }
}
