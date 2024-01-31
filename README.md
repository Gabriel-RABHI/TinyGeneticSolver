# TinyGeneticSolver

This library is a simplified implementation of machine learning Genetic Algorythm. This implementation mimics the biological sellection process to find the most efficient parameter set of a system.

Imagine you have a class that generate outputs from inputs. This class have a set of parameters that change computations. This parameters permit to tune the system to make it efficient. The efficiency of a parameters set can be measured using a test function that score the output. If there is ont single parameter with four possible values, you can test each value to find the parameter value that make the best output. If you have many parameters with lot of possible values, you may have an infinit values combinations and test all combinations may takes years of computations.

To find the best parameter set, you can use a Genetic Algorythm : it permit to randomly set parameters, measure the performance of each set, and sellect the parameters changes that permit to have a better output. A suite of random mixes permit to maintain a good creativity to find combinations.

## The process

The refinement process is based on a loop.

![image](https://github.com/Gabriel-RABHI/TinyGeneticSolver/assets/8116286/f893117d-0109-4381-bca8-4cb15549b630)

- First, the algorythm create a population with various randomly defined parameters, nammed Specie Genetic.
- We bench instances. Each instance of the Specie is tested : we compute a score (higher is better).
- A sellection of the best instances is done.
- A combination of the parameters of the best instances is done in the less efficient instances.
- Some random mutations are done in the Genome, to search new combinations.

So, the process is a combination of random parameter set and sellection of the best parameters.

## How to use it
