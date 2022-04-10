# ElderandTest: Varela Juan Pablo
 
Due to the limited time for the Test, I focused my project on a good code structure based on characters that can run external behaviours. This helps decouple code and makes life easier on the long run.
The Gargoyle implements a class FlyingActor, which will be in charge of controlling the state of the player and running external behaviour. By this, I mean actions and extra behaviours, like moving the actor through a path. This ensures that the gargoyle is doing one action at a time, simplifying testing later.
The actions are designed with interfaces and their implementations are made with scriptableObjects, this makes it easier to have the action code decoupled from the actor and gives the possibility to have serialized fields in the action. Every behaviour in the logic of an actor runs through a coroutine, chaining the logic of multiple behaviours when needed.

With all this said, some code still needs polishing, I'll try to enumerate a couple of code smells I already spotted but was unable to finish on time.
- The FlyingActor should derive from a more basic class called Actor, from where the player and other characters could be built.
- The Dive is currently assuming that the gargoyle has a PathTraveler component and the Dive path is pretty much harcoded.
- The fireballs could use a Pool and SpawnManager.
- There are classes that are not being used in the project, this is because I have a lot of the logic for my games exported in unityPackages. With that said, there still should be a little clean up.
- I barely got any time to make the inspector, but wanted to show that I understand how to make editors. With more time, I could recreate the one shown in the test instructions.

Aside from these points, I have a couple of comments regarding the decisions in the design side of the project.
Being the case that the test was sent to be dealt with in a weekend, I didn't have the opportunity to ask questions that came along the development, so I made the best estimate I could. But, in the real world, I would first go to the designer/client to ask for clarifications.
- An exception to the "no repeating actions" rule was made for the Claw action since it's chance is 100%. It still moves when it's done with the attack, but if the player manages to get to it, the claw action will be repeated.
- When the gargoyle dives, it doesn't relocate, like with the other actions, mainly because it felt weird that it would do something like that.
