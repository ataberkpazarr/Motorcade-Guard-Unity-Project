# 3D Motorcade Guard game
Repository for 3D Motorcade Guard game with Unity 2D

Note: Main skeleton of the game is taken from https://www.youtube.com/watch?v=iXa0riFpWGQ playlist and then customized. The functionality changed a lot and only the road (highway) sprite is stayed same.

The main logic is guarding a main car, with 6 Hummer jeep guards, in the traffic. The cars in the traffic may harm our convoy if a collision occurs. Also there exists police cars which comes from opposite way and they are able to harm our convoy too.

Main Menu: ![resim](https://user-images.githubusercontent.com/55497058/116768757-c040cb00-aa41-11eb-9bec-df04000ec945.png)

Select Car: ![resim](https://user-images.githubusercontent.com/55497058/116768776-d77fb880-aa41-11eb-871a-5d0dfa9ad7b7.png) ![resim](https://user-images.githubusercontent.com/55497058/116768780-e23a4d80-aa41-11eb-8a72-d58b67ac66a0.png)

Game Play: 

 As it can seen from below Picture, 6 Hummer type jeep guard an done sport car is my 
convoy. The sport car can be changed, there are 6 options in the selection panel.
![resim](https://user-images.githubusercontent.com/55497058/116768811-2594bc00-aa42-11eb-8e8d-c72cd30a2ed6.png) 

There are two types of enemies in my code, their type are enemycontroller an done is the 
normal cars which moves on traffic, if our guard or our main car collides into them, only our cars 
get harmed and being destroyed.

Also It is seen the current level and total passed way information top right corner.
In our convoy, main car is not allowed to go outside of the road, it cant do it. But Guards are able 
to do it and if they go outside of the road from right or left, then those guards are also being 
destroyed.
The level logic is simple, in every 200 meters that user passes, congrulations panel occurs with 
next level button Level Passing:![resim](https://user-images.githubusercontent.com/55497058/116769102-6d1b4800-aa42-11eb-8c28-4aebe603aa1f.png)

And there is also one more type of a enemy which is in police appearance I call it attacker type of 
enemy and it is also in type of class enemycontroller. If it collides into our cars, both our car and 
the attacker car are being destroyed

More attacker type of enemies : ![resim](https://user-images.githubusercontent.com/55497058/116769135-ad7ac600-aa42-11eb-8dd5-be2207cd5b1f.png)




GameOver: ![resim](https://user-images.githubusercontent.com/55497058/116769150-c5524a00-aa42-11eb-9fd6-88d5004a334b.png)




