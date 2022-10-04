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
git checkout -b <your handle>/algorithm/<algorithm name>
```

3. Open `Satisfiability.sln` with Visual Studio 2022

4. Set `Satisfiability.Runner` as your startup project

![](assets/set-startup-project.png)

5. Make a copy of `Satisfiability.Algorithms\Template.cs` and rename filename & class to your algorithm name

![](assets/my-first-algo.png)

* Modify `Satisfiability.Runner\Program.cs` to use your algorithm and start developing / debugging!

![](assets/start-debugging.png)

## Rules

You should adhere to the following rules if you want to "upload" an algorithm:

1. See `Uploading Algorithms` in the [main docs](https://test.the-innovation-game.com/get-involved) for the non-challenge specific rules

2. Your algorithm must implement the base class in `Satisfiability.Algorithms\Abstract.cs`:
    * `override` the `Solve` function
    * You must invoke `WriteAlgoIdentifier` every so often with an int that cannot be guessed without running your algorithm
    ```
    // e.g.
    WriteAlgoIdentifier(Random.Next() * input.LastIndexOf(true) * input.LastIndexOf(false));
    ```
    * if you need randomness, use the provided `System.Random` instance: `this.Random`

3. If you want to give up on the challenge (e.g. maybe its unsolvable), you should return an empty `List`

4. Your algorithm class name and filename must be `<algorithm_name>.cs`

5. All your utility classes should be contained in a separate namespace `Satisfiability.Algorithms.<algorithm_name>Utils`

6. If you are improving an existing algorithm, make a copy of the file before making modifications


## Support
[Join our Discord](https://discord.gg/YTJGVpZD)

## License

TBC
