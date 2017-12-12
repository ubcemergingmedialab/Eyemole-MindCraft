![image](https://user-images.githubusercontent.com/13011161/33869617-8f98b5d2-debe-11e7-9973-2dc96b437544.png)  

## What Is Mindcraft?   

Mindcraft (not affiliated with a certain other world-building game) is the closest approximation we have to the dream of being able to control entire worlds with your mind. In more technical terms, it is a virtual reality environment whose features are modulated by the input from an EEG headband that the user wears.  

### Currently Implemented Features   

- Brain-painted cubes: 
    - The environment has EEG-responsive cubes scattered around, as well as a button to spawn more cubes 
    - When the user picks up a cube, 4 of its faces will be painted with dynamically generated spectrograms for each of the 4 sensors on the headband   
    - The texture is saved when the user releases the cube   
- Modifying terrain with your mind: 
    - Pressing the touchpad on the left controller will modify the terrain at the location the controller points to
    - The direction (raising or lowering) and rate of change of the terrain are based on the relative alpha power averaged over the 4 sensors of the headband   
- Switching environments:  
    - The experience contains a main menu where the user can switch to one of the implemented environments (currently only one is implemented)   

## Setup Instructions  

### First Time Setup
In order to get the full Mindcraft experience, you need a [Muse headband](http://www.choosemuse.com) (the EML has one of those). When you set up Mindcraft on your machine for the first time, follow the steps outlined below:  

1. For the headband to be able to send data to Unity, you will need to install Muse Direct. This app can be downloaded for free through the Windows Store.
2. One Muse Direct is installed, open it and you should see a screen like this (you will probably not see any devices in the list): 
![image](https://user-images.githubusercontent.com/13011161/33870255-db6563d6-dec1-11e7-808f-da5e757c21c0.png)  
3. Click the 'Add' button next to 'Output To'. Set the nickname to 'Unity' and the destination to 'OSC UDP'.  
4. You will now see a menu like this one:   
![image](https://user-images.githubusercontent.com/13011161/33870351-37f7fe2e-dec2-11e7-96c2-ec8591fc04db.png)  
    - Change the **IP** to **127.0.0.1** - this is a shortcut for the local IP, since the computer that receives Muse data is the same computer as the one that has Mindcraft running.
    - Change the **Port** to **5000** - this is a bit arbitrary - if you're running Mindcraft from Unity, it doesn't matter what this port is as long as it matches the Input Port of the OSC script. If you're running from the build, the port will be set to 5000, so just leave it at that.  
    - For the **Prefix**, choose the **Custom Static Text** option and set it to **/muse** - it is important to have the slash in front so that the message is recognized as a valid OSC message, and addresses are set to start with '/muse' by default. 
    - For **Output Data** and **Output Algorithm**, check the **EEG Data** and **Absolute Band Powers** boxes as these are the values that Mindcraft currently uses - you could also just check all the boxes to be futureproof. 

In the end, your settings should look like this:   
![image](https://user-images.githubusercontent.com/13011161/33870573-4cc2ffce-dec3-11e7-8b0d-5655494e5a3b.png) 

### Running Mindcraft With Muse Direct  

Once you have the Muse Direct output set up, you will need to follow the steps below to run it:   

1. Click on the Muse headband you're curently wearing in the list of devices to select it   
2. Uncheck the first two outputs and check the output you created so that it looks like this:  
![image](https://user-images.githubusercontent.com/13011161/33870716-fd0142ce-dec3-11e7-9c13-99dee7a8d8e1.png) 
3. Click on the Bluetooth button next to your headset name to start connecting  
4. Check the 'Info' tab to see if you're connected or not - if you are, you can now run Mindcraft! :sunglasses:  

### Troubleshooting Setup    

#### I don't have Windows 10! / The Windows Store says I need to upgrade Windows 10 to install the app!

Unfortunately, Muse Direct only works on recent updates of Windows 10 :(
#### Muse Direct keeps connecting and disconnecting!  

There seem to be persistent Bluetooth connection problems with the Muse Direct app - we think it might be a hardware issue so there is not much we can do about that. However, if you lose connection, the following steps usually help reconnect:   

1. Close Muse Direct.
2. Go to your computer's Settings --> Bluetooth & Other Devices.
3. Choose your Muse headband in the list of paired devices (it will be called Muse-{4-letter code}) and click 'Remove device'.
4. Turn Bluetooth off and on.
5. Restart Muse Direct and try reconnecting. 



