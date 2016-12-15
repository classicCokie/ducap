# C# Network Sniffer

####Worked time:
First Proof of Concept and further Research started on 28th of November 2016.
Since then 24 hours (approximately 8 hours a week) have been invested in the Project. Further time investments may be seen in the commit history of the Project.

####The Vision
The Vision is to create a Network Sniffer (similar to Ettercap) in C# with the .NET Framework.

####Requirements & User Stories

The user can Scan his local wifi for Hosts that are online
The user can select one of the Hosts the scan displays, from a list.
The user can choose to arp poison the selected host
The user can listen to the traffic of the host he arp poisoned
The user can set a filter which is listening for unencrypted passwords

####Current Status
The Application is able to send a manipulated ARP Request which is meant to initialize the ARP Poisoning.
The Application is also able to forward incoming traffic by passing it through a Proxy which is implemented inside the Application itself.
The Application is able to Scan the local network for Hosts that are online. However so far it only shows non Phone Devices.

####Schedule
The schedule is Scrum based which means there is no long time waterfall planning. The goal until December 22th is to have the Proxy-Server Working Stable and the Arp Poisoning working probably with any host the user selects. Also the user should be able to scan his network for hosts which are online. Further plans are going to developed by reviewing learnings form the current sprint.

####Mockups
Find the Mockup picture in the Mockup folder.

####Deadline
The Project is due January 9th. Expected to finish on January 8th.
