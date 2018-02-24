# Knapsack

## Overview
Knapsack is a C# implementation of a dynamic approach to the 0-1 Knapsack problem algorithm. The code itself is embedded with a lot of explanation for each section, related parameters, and output.

Knapsack runs from the command line and accepts either arguments from the command line (passed in at the beginning) or using a user prompt.

## What is the Knapsack Problem?
The Knapsack Problem is an [optimization problem](https://en.wikipedia.org/wiki/Optimization_problem). The "knapsack" refers to the bag a stereotypical thief would use in order to store loot. Given the storage capacity of said bag, what choice of items would maximize the profit he would make while also maintaining the knapsack's structural integrity? The problem is usually divided into three relevant numbers: the items of interest, the value (in the thief's case, most likely monetary), and the limiting factor (usually considered to be the weight of the relevant items).

## Knapsack Problem complexity
The Knapsack Problem is one of several problems described as *NP-Complete*. Such problems can be decided in polynomial (not exponential time) and can be reduced to another NP-Complete problem.

The dynamic programming algorithm runs in O(nW) time for n items and W maximum weight. This looks like polynomial time. However, an increase in integer size, calculated in bits, results in exponential growth of run-time. The Knapsack problem is thus labeled as [weakly NP-complete](https://en.wikipedia.org/wiki/Weak_NP-completeness).

(to be continued)
