# Installing Multichain  

Assuming Ubuntu 18.04 host.  

Run the script in this directory `installMultichain.sh`  
This will download the build and runtime dependencies for multichain.  
Download the src, build then install in /usr/local/bin

# Running Multichain on system boot  

Assuming Multichain works when started manually with `$multichaind MainChain -daemon`  
Ubuntu uses systemd for init and service manager.  To autostart daemons a systemd unit file must be created.

In this directory is a file `multichain.service` this service file is the unit file that describes requirements and actions to systemd.  
1. Copy this file into `/etc/systemd/system/`  
2. Double check it's there by `$cat /etc/systemd/system/multichain.service` which should print the file to the terminal.  
3. Reload systemd's unit file cache (this needs to be done if the unit file is ever updated) `$sudo systemctl daemon-reload`  

To one-start multichain using the service `$sudo systemctl start multchain.service`  
To have mutlichain start on boot (won't start till boot) `$sudo systemctl enable multichain.service`  
To start immediately and have it start on boot.  Run both the above commands.

### Some Gotchas  
The included unit file assumes  
- multichain was set up with the user `ubuntu`, if multichain was setup with a different user.  Replace with that username.  
- multichain is setup to connect to the MainChain network. 


