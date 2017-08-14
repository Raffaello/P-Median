# P-Median
Solve the P-Median Problem with a Math Heuristic.

Using a Wrapper to call CoinMP.

## Introduction

The problem of locating P "facilities" relative to a set of "customers" such that
the sum of the shortest demand weighted distance between "customers" and "facilities" is minimized.

Solving this problem, is non-trivial. 
To see this, consider that the number of possible solutions to any given instance of a P-Median problem is:

![equation](http://latex.codecogs.com/gif.latex?%5Cbinom%7BN%7D%7BP%7D%3D%5Cfrac%7BN%21%7D%7BP%21%28N-P%29%21%7D)

where **N** is the number of "customers" and **P** is the number of facilities to be located. 

As an example, for N = 20, and P = 5, the resulting number of possibilities is 15,504. 

For N = 50, and P = 10, a problem that is not large by most standards, the resulting number of possibilities is 10E10!

This method use a [Matheuristics](https://en.wikipedia.org/wiki/Matheuristics) approach.

## Metaheuristc

Generate multiple feasible solutions.

## Mathematical Programming

from the generated solutions it will resolve a [Set Cover Problem](https://en.wikipedia.org/wiki/Set_cover_problem). 

## Properties

- Parallel
- Library it is implemented as a WebService and WCF Service too.
- Desktop with Splash Screen
- Batch Processing

## Related Project

[PMedFlexClient](https://github.com/Raffaello/PMedFlexClient)

## Solution Projects

- **PMedLib:** Main Library For solving P-Median problem with Math-Heuristic. 
- **PMedianForm_SpalshScreen:** GUI for the Lib.
- **PMedLibWCFService:** WCF Service for the Lib.
- **PMedLibWebService:** Web Service for the Lib.

### Library Dependencies

- CoinMP.dll included.
