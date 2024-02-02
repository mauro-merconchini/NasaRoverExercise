# NASA Rover Exercise

Code exercise to ingest a set of rover instructions and provide an expected output after simulating each rover's execution of the provided instructions

### Program Input

To modify the input for the program, please change the `input.txt` file located in the `NasaRoverExercise` Project folder. The text file will be automatically copied to the build location of the executable file so that it can find it easily.

### Assumptions

- Plateau grid size will <u>always</u> be provided as two numbers followed by a space and will <u>always</u> be the first line of the input file.
- Rover starting conditions will <u>always</u> be formatted as an X coordinate, a Y coordinate, and a cardinal direction.
- Rover starting conditions will <u>always</u> precede the rover instructions.
- It is <u>possible</u> for a rover's instructions to cause it to go out-of-bounds.
- It is <u>possible</u> for a rover's instructions to cause it to crash into another rover.

### Implementation Details

I wanted to create a well-structured solution to the problem that would not only give the correct output, but would be performant, maintainable, and demonstrate good object-oriented design. I created a `RoverUtils` class as a helper utility which would contain useful functionality, such as custom enums to make the cardinal directions and rover instructions more readable in the rest of the source code, and custom exceptions with self-documenting names and custom error messages. I also created an `IRover` interface which could be theoretically used for implementing different kinds of rovers which would not all share the same functionality, but would all share a minimum set of shared features (such as an instruction set). 

A `RoverController` object is in charge of managing one or multiple rovers, and their associated instructions. The primary functionality of this class is to parse the information from the `input.txt` file to create rovers with instructions, execute those instructions for each rover sequentially, and keep an eye on the rover's safety (avoiding collisions or out-of-bounds moves). Finally, the `Rover` class is meant to represent a rover, implementing at least the basic functionality of the `IRover` interface as well as the functionality inherent to its instruction set. Its instruction set and axes of movement are pre-defined in the constructor, and they are are mapped to dictionaries for fast O(1) lookups. Rotations to the left or right nudge the index on the compass, which is implemented as a circular array to mimic the "circular" nature of real-world compasses and rotations.

This solution includes Unit Tests which can be found in the `NasaRoverExerciseTests` project.