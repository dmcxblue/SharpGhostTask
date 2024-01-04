# SharpGhostTask
A C# port from Invoke-GhostTask

## Description

Tampering with Scheduled Task has been known and already worked with, in simply editing the tasks using the GUI interface or just the schtasks command when this happens they will leave an EventLog behind (4698)

![image](https://github.com/dmcxblue/SharpGhostTask/assets/41899653/f0794e99-4565-4f19-b151-b5398128dc1f)

When editing a task this will also leave an EventLog behind (4702) we can see in the screenshot below that there was an update on a Task

![image](https://github.com/dmcxblue/SharpGhostTask/assets/41899653/7ccf5ee1-794b-49d1-8b9d-4165293e1a82)

Scheduled Tasks can be edited in a more complicated way via the Registry Keys, that's where [Invoke-GhostTask](https://gist.github.com/Workingdaturah/991de2d176b4b8c8bafd29cc957e20c2) by [@SchrodingersAV](https://twitter.com/SchrodingersAV) comes in handy. SharpGhostTask basically uses the method from Invoke-GhostTask to edit the Registry Keys manipulating the binary values of the Task that is targetted. 

We can see below how this looks in the Registry Keys

![image](https://github.com/dmcxblue/SharpGhostTask/assets/41899653/7aaa8467-cbf6-47b2-87ca-381efb3b531c)

SharpGhostTask will replace the binary value without breaking the rest of the Scheduled Task. This way replacing it with a payload that we control, in the following example we see the replaced binary value this time pointing to ```calc```

![image](https://github.com/dmcxblue/SharpGhostTask/assets/41899653/1094048a-7d78-4b29-a4c8-51d8f6c8beab)

By replacing this value via Registry Keys we also avoid the (4702) log from the Event Viewer, but monitoring the Registry Keys can be a giveaway. And this also comes with Challenges you will need SYSTEM Access to be able to edit these Registry Key Tasks. I've had luck executing the Task once it was changed, but to be safe a Restart is required.

## Demo

![SharpGhostTask](https://github.com/dmcxblue/SharpGhostTask/assets/41899653/d2045f62-cb50-4197-9205-78d285b4858b)


