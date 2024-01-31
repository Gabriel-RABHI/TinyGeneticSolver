namespace TinyGeneticSolver.Tests
{
    public class SpecieGeneticTests
    {
        public static double PointDistance(double x1, double y1, double x2, double y2)
            => Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));

        [Fact(DisplayName ="Find the shortest path from point to point")]
        public void SolveShortestPath()
        {
            var specy = new SpecieGenetic(
                new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                new string[][] { new string[] { "Right", "Left", "Up", "Down" } });
            var pop = new SpeciesPopulation(
                specy,
                1000,
                (genes) =>
                {
                    // -------- Compute the score of the combination :
                    // from 0 to 1 if the point 10,10 is not reached
                    // from 1 to 1+ if point reached, that represent the number of steps (shortest is better)
                    var x = 0;
                    var y = 0;
                    var n = 0;
                    var d = 100d;
                    for (int i = 0; i < genes.Genes.Length; i++)
                    {
                        switch (genes.Genes[i])
                        {
                            case 0:
                                x++; break;
                            case 1:
                                x--; break;
                            case 2:
                                y++; break;
                            case 3:
                                y--; break;
                        }
                        // -------- Compute distance from destinnation point (10,10)
                        d = PointDistance(10, 10, x, y);
                        // -------- Found the destination ?
                        if (d == 0)
                            return 1 + (1d / (double)n);
                        n++;
                    }
                    // -------- Return the score
                    return 1 / d;
                }
                );
            // -------- Train the model
            for (int i = 0; i < 10_000; i++)
                pop.Train();
            Console.WriteLine(pop.BestGenome);
        }

        [Fact(DisplayName = "Find the shortest path using scanning")]
        public void FindShortestPathByScan()
        {
            var specy = new SpecieGenetic(
                new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                new string[][] { new string[] { "Right", "Left", "Up", "Down" } });
            var pop = new SpeciesPopulation(
                specy,
                1000,
                (genes) =>
                {
                    var x = 0;
                    var y = 0;
                    var n = 0;
                    var d = 100d;
                    for (int i = 0; i < genes.Genes.Length; i++)
                    {
                        switch (genes.Genes[i])
                        {
                            case 0:
                                x++; break;
                            case 1:
                                x--; break;
                            case 2:
                                y++; break;
                            case 3:
                                y--; break;
                        }
                        d = PointDistance(5, 3, x, y);
                        if (d == 0)
                            return 1 + (1d / (double)n);
                        n++;
                    }
                    return 1 / d;
                }
                );
            var result = pop.Scan();
        }

        [Fact(DisplayName = "Check the gene incrementor")]
        public void GenomeIncrementTest()
        {
            var specy = new SpecieGenetic(new int[] { 2, 2, 2 }, null);
            var genome = new Genome(specy, false);

            Assert.True(specy.CombinationCount == 8);

            Assert.True(genome.Genes.SequenceEqual(new int[] { 0, 0, 0 }));

            genome.Increment();
            Assert.True(genome.Genes.SequenceEqual(new int[] { 1, 0, 0 }));

            genome.Increment();
            Assert.True(genome.Genes.SequenceEqual(new int[] { 0, 1, 0 }));

            genome.Increment();
            Assert.True(genome.Genes.SequenceEqual(new int[] { 1, 1, 0 }));

            genome.Increment();
            Assert.True(genome.Genes.SequenceEqual(new int[] { 0, 0, 1 }));

            genome.Increment();
            Assert.True(genome.Genes.SequenceEqual(new int[] { 1, 0, 1 }));

            genome.Increment();
            Assert.True(genome.Genes.SequenceEqual(new int[] { 0, 1, 1 }));

            genome.Increment();
            Assert.True(genome.Genes.SequenceEqual(new int[] { 1, 1, 1 }));
        }

        [Fact(DisplayName = "Check that all combinations are testes")]
        public void GenomeScanTest()
        {
            var specy = new SpecieGenetic(new int[] { 2, 2, 2 }, null);
            var genome = new Genome(specy, false);
            var callCount = 0;

            var pop = new SpeciesPopulation(
                specy,
                (genes) =>
                {
                    callCount++;
                    return 1;
                });
            var result = pop.Scan();
            Assert.True(callCount == 8);
        }
    }
}