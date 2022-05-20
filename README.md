# TpCon

tpcon - is a **CLI** tool for managing TpLink routers without using web interface

You don't need to use web browser anymore, just use command line

<hr>

## Dependencies

In order to run application you need .NET Framework 4.6+ installed

<hr>

## Installation

### From sources

```
cd src/TpLinkConsole.Console
dotnet publish -c Release
```
Then copy content of build into your folder with Software
On linux:
- Add `export PATH="${YOUR_PATH_TO_EXECUTABLE}:${PATH}"` to end of .bashrc
For example `export PATH="${HOME}/bin/tpcon:${PATH}"`
- Restart terminal
- Run `command -v tpcon` to check successful installation

On Windows:
- Add `setx PATH "YOUR_PATH_TO_EXECUTABLE;%PATH"` for your account. For example: `setx PATH "C:\bin\tpcon;%PATH%"`
- Restart command prompt
- Run `where.exe tpcon.exe` to check success installation

## Commands

### Supported commands:
- **health**

Checks connection to router. 

Prints 'OK' if connection exists or 'Could not connect to router' otherwise

- **refresh** 

Refreshes the router. 
Refresh required after applying new values. 
For example after ```wlan password "P@ssw0rd"```

- **wlan status**

Prints wlan status: ssid, password, on or off

- **wlan enable**

Enables Wi-Fi 

- **wlan disable**

Disables Wi-Fi

- **wlan password "*new password*"**

Set new Wi-Fi password

- **wlan ssid "*new ssid*"**

Set new Wi-Fi SSID (Wi-Fi name)

- **lan status**

Displays lan status: router local ip address, subnet mask, router mac address

### Options
- --address "*router local ip address*"

Address of TpLink router you want to connect. 
Default: 192.168.0.1

- --username "*admin username*"

Username of router admin. 
That is what you enter in username input when enter browser router page.
Default: admin

- --password "*admin password*"

Password of router admin.
That is what you enter in password input when enter browser router page.
Default: admin

- --output "*output style*"

Applied for commands that prints something on screen.

Supported outputs: table, json, xml, plain (key-value).
*For plain type supported option --delimiter (by default set to ": ")*

Example: --output table

Default: plain

### Warning

These commands were tested on [TpLink TLWR741ND](https://www.tp-link.com/ru/home-networking/wifi-router/tl-wr741nd/).
Other versions might not be compatible. 
Use at your own risk. (Anyway you can always reset router)
