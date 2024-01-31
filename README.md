# TinyGeneticSolver

This library is a simplified implementation of machine learning Genetic Algorithm. This implementation mimics the biological selection process to find the most efficient parameter set of a system.

Imagine you have a class that generate outputs from inputs. This class have a set of parameters that change computations. This parameters permit to tune the system to make it efficient. The efficiency of a parameters set can be measured using a test function that score the output. If there is ont single parameter with four possible values, you can test each value to find the parameter value that make the best output. If you have many parameters with lot of possible values, you may have an infinite values combinations and test all combinations may takes years of computations.

To find the best parameter set, you can use a Genetic Algorithm : it permit to randomly set parameters, measure the performance of each set, and sellect the parameters changes that permit to have a better output. A suite of random mixes permit to maintain a good creativity to find combinations.

## The process

![image](https://github.com/Gabriel-RABHI/TinyGeneticSolver/assets/8116286/54f23e9a-0b08-4c55-bd68-d13789c9cf18)

The refinement process is based on a loop :
- First, the algorithm create a population with various randomly defined parameters, named Specie Genetic.
- We bench instances. Each instance of the Specie is tested : we compute a score (higher is better).
- A selection of the best instances is done.
- A combination of the parameters of the best instances is done in the less efficient instances.
- Some random mutations are done in the Genome, to search new combinations.
- Redo bench.

So, the process is a combination of random parameter set and selection of the best parameters.

## How to use it

There is 3 components :
- **SpecieGenetic** : it describe the genes topology - how many genes (or parameters) have the specie ? And what is the number of different values each gene (parameter) can have ?
- **SpeciesPopulation** : it a set of instances of the Specie, to refine.
- **Genome** : it is the genetic set (or parameter set) for a given Specie instance.
### Defining the SpecieGenetic

Here a sample to define a SpecieGenetic :

```c#
var specy = new SpecieGenetic(new int[] { 2, 3 });
```

Here, there is two Genes (or parameters) because the int array is 2 length. The first Gene can have 2 values, the second one 3 values. It mean that the values of gene one can goes from 0 to 1, and the second one from 0 to 2 : in other words, the Gene value can be viewed as index (0..N).

Optionally, you can specif string Labels for the Genome.ToString() string rendering :

```c#
var specy = new SpecieGenetic(new int[] { 2, 3 }, new string[][] { new string[] { "Right", "Left" }, new string[] { "Zero", "One", "Two" }  });
```

Each Gene value will be replaced by the corresponding label.

You can specify a label for a Gene, ending with "=" char output the value.

```c#
var specy = new SpecieGenetic(new int[] { 2, 3 }, new string[][] { new string[] { "Right", "Left" }, new string[] { "Second=" }  });
```

### Defining the SpeciesPopulation

A population is like a Specie Instance Collection used to perform evolution and selection of the best parameters. The constructor need the SpecieGenetic, the population individual count, and a function to text each Genome.

```c#
public SpeciesPopulation(SpecieGenetic genetic, int count, Func<Genome, double> measure)
```

The count minimal value is defined in constant MINIMAL_POPULATION. The measure function had a Genome as parameter and return a score. Higher is the score, better is the output. A SpeciesPopulation permit to Train the system to search the mot efficient Genome, or to Scan all combinations. The best Genome is in the BestGenome property.

### Converting Genome to parameters, and parameters to Genome

In any Machine Learning solution, what's drive the success of the result (the efficiency of learning algorithm) is the quality of the problem modeling. The modeling is the most important thing, and measure the efficiency of the learning is included in this process of modeling. Here, there is two main questions :

1. How to define a SpecieGenetic ? How many Genes with how many distinct values ?
2. How to score a Genome ?

A gene is a parameter. It is an Int variable that vary from 0 to N, where N is the number of distinct values the parameter can have. You'll think that in many cases, you'll have some parameters that have a limited range integers. That's true. But it will be necessary to map Gene 0..N range to a parameter value. If the parameter is an Enum, you simply have to set the Gene to the number of entries in the Enum : it is the simplest situation. If the parameter is a continuous double value, you'll need to find a mapping principle with a level of discretization.

For example, here a mapping to a double value. Here the Gene is defined as [0..200] :

```c#
double paramValue = (genome.Genes[0] - 100)/100d;
```

This permit to convert a [0..200] Gene in a double that vary from -1 to 1 by step of 0.01.

An another example,a mapping to a int value from 0 to 10 000 defined as a gene [0..100] :

```c#
int paramValue = genome.Genes[0] - genome.Genes[0];
```

This permit to convert a [0..100] Gene in a int that vary from 0 to 10 000 with an exponential scale.

## Full sample

In this sample, the goal is to find a Genome representing a step by step path to move from point (0,0) to a point (10, 10). Each Gene value represent a move (Up, Down, Left, Right). The Genetic is composed of 24 Genes representing the moves. The result is computed by cumulate all the moves to goes to the destination point.

There is 281 474 976 710 656 combinations. The training is done by a 10 000 iteration call to Train method in few seconds. The score is the shortest number of steps to reach the destinnation.

```c#
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
```

