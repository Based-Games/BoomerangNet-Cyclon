# Engine JSON files
Where the game variables are stored.

## `bindings.json`
Used for storing controller bindings. Not really user-editable.

## `bookkeeping.json`
Used for tracking credits and other audit logs. Not really user-editable.

## `config.json`
Used for storing game engine settings. User-editable.

- `system` tag: Test menu settings.
    - `display` tag: Display settings.
        - `screen`: The screen ID. Default: `0`
        - `resolution`: The screen resolution. Not used in full-screen. Default: `1920x1080`
        - `video_mode`: Used for storing the output type.
            - Modes: `full`, `borderless`, `window`. Self-explanatory. Default: `window`
        - `vsync`: Use VSync. Default: `true`
        - `hide_cursor`: Hide the cursor when mousing-over application. Default: `false`
    - `sound` tag: Sound volumes.
        - `attract_volume`: Volume for attract audio. `1-10`. Default: `1.0`
        - `system_volume`: Volume for system audio. `1-10`. Default: `1.0`
        - `music_volume`: Volume for music. `1-10`. Default: `1.0`
        - `sfx_volume`: Volume for sfx. `1-10`. Default: `1.0`
    - `network` tag: Network settings. Not really used ATM.
        - `enable`: Enable or disable the network. Default: `false`
    - `coin` tag: Coin settings.
        - `free_play`: Disable credit settings. Default: `true`
        - `coin_per_credit`: The amount of coins per credit. Default: `1`
        - `remember_credit`: Save current credits so they're usable after a restart. Default: `true`
    - `game` tag: Game settings. Shouldn't be stored in this file. Should be game specific.
        - `load_type`: Keep this at default. Default: `game`
    - `inputs` tag: Input settings. Not really used ATM.
        - `p1_io`: Set Player 1's IO type. Types ATM are just `HID` Default: `HID`
        - `p2_io`: Same as `p1_io`.
        - `connect_port`: The port for things wanting to use IO to connect to. Changing this will change the server and client sides. Default: `59585`
    - `clock` tag: Clock settings. Just used for shop close stuff.
        - `shop_close`: Shop close settings. Leave these alone.
    - `engine` tag: Engine variables. Don't touch these.

## `game.json`
Used for storing game Variables. Mostly user-editable.

- `game` tag: Game variables.
    - `code` Game specific code. Used for networking and file structure. Don't touch!
    - `title`: The game's title.
    - `author`: The game's author.
    - `license`: The game's license.
    - `update_url`: Game specific update server's URL.