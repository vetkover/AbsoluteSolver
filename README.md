# AbsoluteSolver
AbsoluteSolver - easier access to Windows system permission level 
#
I found a way to get NT AUTHORITY/SYSTEM rights, now if I find a way how to implement it without other side loaders then there will be an application ^-^
# 5/30/2023
![screenshot](https://github.com/vetkover/AbsoluteSolver/blob/main/image.png?raw=true)
the service with system rights became available to me, now I need to understand how to make the system environment application interact with the user environment (without using the file system for application communication :/ )
# 5/31/2023
These two environments cannot be friends (even through dll) Ill try to find a way to create a safe environment for the application to work through the registry -_-
# 6/01/2023
![screenshot](https://github.com/vetkover/AbsoluteSolver/blob/main/Screenshot%202023-06-01%20111149.png?raw=true)
I accidentally made my virtual system invalid when trying to reorganize the rights in the system... at the moment Im worse than a monkey with a grenade :D
