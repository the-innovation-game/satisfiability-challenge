# The Innovation Game - Satisfiability Challenge

## Objective
**Boolean Satisfiability** is a classical problem in logic and computer science. 

The challenge is to develop an algorithm to solve 3SAT problems. i.e. determine if there exists a combination of TRUE / FALSE values that satisfies a Boolean formula.

## Repo Structure

* `Satisfiability.Algorithms` contains all the uploaded algorithms for this challenge. Any algorithm you develop should go in here
* `Satisfiability.Challenge` contains the logic for running & verifying algorithms for this challenge
* `Satisfiability.Runner` contains an executable program for debugging / running your algorithms during development
* `Satisfiability.Tests` contains tests for the challenge logic 

## Getting Started

1. [Install Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

2. Fork and clone this repo

3. Create a branch for your algorithm
```
git checkout -b <team name>/algorithm/<algorithm name>
```

3. Open `Satisfiability.sln` with Visual Studio 2022

4. Set `Satisfiability.Runner` as your startup project

![](assets/set-startup-project.png)

5. Make a copy of `Satisfiability.Algorithms\Template.cs` and rename filename & class to your algorithm name

![](assets/my-first-algo.png)

6. Modify `Satisfiability.Runner\Program.cs` to use your algorithm and start developing / debugging!

![](assets/start-debugging.png)

7. During the allowed window, push up your branch and open a pull request to merge your branch to the master repository 


## Support
[Join our Discord](http://discord.the-innovation-game.com)
