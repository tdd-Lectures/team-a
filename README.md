# TEAM A

## What've seen so far:


---

### SOLID
#### Single Responsibility Principle

 - SchedulerManager
   - This class is responsible for everything related to scheduling.
  
---

### Break between What & How

 - `Creating recurrent appointment within 1 day period generates 1 appointment`
   - What is the intent, how is the implementation. Creating is not going to actually generate the appointment. Our implementation is going to just store data that in turn will be translated into multiple appointments, as needed.
  
---

### C#

 - Records - Classes that are generated behing the scenes with equality comparers. They don't compare sequence types by value, only by reference.
 - yield return - generates a new sequence when the method is called.
 - Prefer type initialization `new Xpto { Y = 1 }` over constructor `new Xpto(1)` in data types.
 
---

### Test Driven Development

 - One field at a time, so when the test fails for an unexpected reason we can identify it quickly.
   - `Assert.That(appointment.attendees, Is.EqualTo(new [] { "user1" }));`
   
 
 - Divide & Conquer
   - When I need a piece of software with its own set of rules that doesn't exist yet, we should stop and develop that piece.
   
   
#### Three Laws
 - You are not allowed to write production code without a failing test.
 - Write the minimum of a test to make it fail. Not compiling is failing.
 - Write the easiest or smallest production code to make the tests pass.


---

### Design Patterns

  - [Factory](https://refactoring.guru/design-patterns/factory-method), is used to create instances of a given type.
   
---

### OOP

 - Prefer abstract types over specific types. e.g. `execute(List<int> x)` replace with `execute(IEnumerable<int> x)`.
 - IEnumerable<T> represents a sequence, it could be infinite. A List is finite.
 - [Law of Demeter (LoD)](https://en.wikipedia.org/wiki/Law_of_Demeter)
 - 
