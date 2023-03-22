# IO engine stuff

## What is this?

These are the files to interface with the IO the engine will need.

The main IO manager will be a UDP network socket on port set in the config. The IO will be interfaced with files in this directory and states can be received and sent using the manager. This will also allow the game to connect directly, rather than needing to use another set of files. 

This will be ran as a thread, started when the engine starts up. In this document, every component used for IO will be documented. This will range from JVS support, to HID support, to dumb LED signs. 