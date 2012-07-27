Follow the steps below to install the Platform Infrastructure locally on a machine:

1. **Request a static IP**

	A machine running the Platform Infrastructure locally must have a static IP. This is due to the fact that the infrastructure uses machine names to look up server IP’s and then caches them during startup. If the machine had a dynamic IP, then once it changed the machine would no longer be able to be connected to by other serves unless they were all restarted. 

2. **Have your machine added to the address.dat file**

	This entry is loaded up by the Platform Infrastructure in order identify a machine. 

	You must send a request to Ramesh Kadur with your machine name stating you need to add this machine as a client that will be using the Platform Infrastructure

3. **Install the infrastructure**

	You must install the following MSI installations and follow any steps in the release notes. The installations must be completed in the order they are in. 
	You must wait until the above address.dat change is made before step x as you will need the source numbers from this entry during installation. You must wait until the address.dat file with your machine has been unloaded to the Session servers before you can verify the installations

	- XIPC
	- RTS
	- FCS

	You can find the installs at the following location:

		\\xxxxxxx