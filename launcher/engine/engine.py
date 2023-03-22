# Main game engine.
# We setup the engine, start a game thread, and stay running with a local API for getting states

import pygame, argparse

# Start importing libs
from engine.common.jsondata import JSONData
from engine.common.validated import ValidatedDict
from engine.common.logger import LogManager
from engine.common.constants import LogConstants
from engine.screen import Screen
from engine.common.asset import AssetManager

# Import scenes
from engine.scenes.testmenu import systemTestMenu

# Init the args
parser = argparse.ArgumentParser()
parser.add_argument('-n', '--no_jingle', help="Disable the jingle.", action="store_true")
parser.add_argument('-l', '--loglevel', help="System loglevel. Positions are 'disable', 'enable', 'debug', and 'errors'.", default='enable', choices=['disable', 'enable', 'debug', 'errors'])
parser.add_argument('-q', '--quickstart', help="Enable system quickstart. Disables file checking and updating. Might break online services.", action="store_true")
args = parser.parse_args()

# Init the logger.
logger = LogManager(args.loglevel)
logger.initLogFile()

path_prefix = './engine/json'
config = JSONData(logger).loadJsonFile(f'{path_prefix}/config.json')
game = JSONData(logger).loadJsonFile(f'{path_prefix}/game.json')

class GameEngine(
    AssetManager
):
    def __init__(self, engine_config: ValidatedDict, game_config: ValidatedDict, args: argparse.Namespace, logger: LogManager):
        AssetManager.__init__(self, config.get_dict('system'), logger)
        self.run = True
        self.framerate = 60
        self.clock = pygame.time.Clock()
        self.args = args
        self.logger = logger

        self.engine_conf = engine_config.get_dict('system')
        self.ver = self.engine_conf.get_dict('engine', ValidatedDict({})).get_str('build')
        logger.writeLogEntry(f"Welcome to BoomerangNet Launcher V{self.ver}!", LogConstants.STATUS_HEADER)
        
        logger.writeLogEntry(f'Startup args: {vars(args)}')

        # Let's start up the game.
        # Init pygame
        pygame.init()

        # Init pygame screen, return
        self.screen, self.resolution = Screen(engine_config.get_dict('system'), self.logger).initScreen()
        pygame.display.update()

        # Current scene/screen state. Here's a list of them.
        # - INIT
        # - WAITING
        # - LOGO_IN
        # - LOADING
        # - ERROR
        # - QUIT
        # - UPDATING
        # - LOADING_TEST
        # We will init this with None so that the engine can decide what to do.

        self.last_state = 'INIT'
        self.current_state = 'INIT'
        self.current_events = None
        self.logo_on_screen = False

        # Empty dict for assets.
        self.assets = {}
        self.load_assets()

        # Now, we begin the loop
        self.engineLoop()

    def load_assets(self) -> None:
        '''
        Load in all needed assets. Run at start.
        '''
        # Images
        image_list = [
            'logo.png'
        ]
        for image in image_list:
            self.assets[image] = AssetManager.loadImage(self, image)

    def eventHandler(self, recursive: bool = False):
        '''
        Handles engine events and a few other things.
        '''
        engine_mode = {
            'INIT': "(init...)",
            'WAITING': "(game loading...)",
            'LOGO_IN': "(loading...)",
            'QUIT': "(goodbye!)",
            'LOADING': "(loading...)",
            'ERROR': "(error!)",
            'UPDATING': "(updating...)",
            'LOADING_TEST': "(loading test menu...)",
            'LEAVE_TEST': "(returning to launcher...)"
        }[self.current_state]

        # Log dat shit
        if self.current_state != self.last_state:
            if not recursive:
                self.logger.writeLogEntry(f'Switching state to {self.current_state}.', LogConstants.STATUS_OK_CYAN)
                pygame.display.set_caption(f'BoomerangNet Launcher V{self.ver} {engine_mode}')

            if self.current_state in ['LOADING_TEST', 'TEST_MODE'] and not recursive:
                if self.logo_on_screen:
                    self.fadeLogoOut()
                self.screen.fill((0, 0, 0))
                pygame.display.update()
                self.sceneChange()

            if not self.logo_on_screen and self.current_state in ['LEAVE_TEST'] and not recursive:
                self.screen.fill((0, 0, 0))
                self.fadeLogoIn()
                pygame.display.update()
                self.current_state = 'LOGO_IN'

            self.last_state = self.current_state

        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self.run = False
                self.current_state = "QUIT"
                if self.logo_on_screen:
                    self.fadeLogoOut()

            pressed = pygame.key.get_pressed()
            if pressed[pygame.K_F1]:
                self.current_state = 'LOADING_TEST'                   

    def fadeLogoIn(self):
        '''
        Render the Logo and the background. Also play a jingle. 
        '''
        # Set the state
        self.current_state = "LOGO_IN"

        # Start by playing the jingle. Check the args.
        if not self.args.no_jingle:
            AssetManager.playSfx(self, 'jingle.wav')

        # Blank the screen
        self.screen.fill((0, 0, 0))

        logo = self.assets.get('logo.png')

        for i in range(50):
            self.eventHandler()
            self.clock.tick(self.framerate)

            # Fade screen to white with the fade in.
            self.screen.fill((160+i, 160+i, 160+i))

            logo.set_alpha(115+(i*2))
            self.screen.blit(logo, ((self.resolution[0]-500)/2, 60-i))
            pygame.display.update()
            i += 1
        self.logo_on_screen = True

    def fadeLogoOut(self):
        '''
        Render the Logo and the background fading out.
        '''
        # Blank the screen
        self.screen.fill((210, 210, 210))

        logo = self.assets.get('logo.png')

        for i in range(55):
            self.eventHandler(True)
            self.clock.tick(self.framerate)

            # Fade screen to white with the fade in.
            self.screen.fill((160-i, 160-i, 160-i))

            logo.set_alpha(115-(i*2))
            self.screen.blit(logo, ((self.resolution[0]-500)/2, 60+i))
            pygame.display.update()
            i += 1
        self.logo_on_screen = False

    def sceneChange(self):
        '''
        Change the current scene.
        '''
        scene = {
            'LOADING_TEST': systemTestMenu
        }[self.current_state]

        pygame.mixer.stop()
        nextScene = scene(self.engine_conf, self.logger, self.screen, self.resolution)
        self.current_state = nextScene.run()

    def engineLoop(self):
        '''
        The main loop of the engine. Everything is started by this loop.
        '''
        logo_rendered = False
        while self.run:
            # First, we should start our loop with the event manager
            self.eventHandler()

            # Now, let's make sure that the game is locked to a framerate.
            self.clock.tick(self.framerate)

            # Load the fade in animation, but only if it's the first loop.
            if not logo_rendered and not self.args.quickstart:
                self.fadeLogoIn()

            # Update screen.
            pygame.display.update()
            # Set logo_rendered.
            logo_rendered = True
        self.logger.writeLogEntry('Goodbye! Thank you for playing.', LogConstants.STATUS_HEADER)

if __name__ == "__main__":
    GameEngine(config, game, args, logger)

    # Close for good luck.
    pygame.display.quit()
    exit()