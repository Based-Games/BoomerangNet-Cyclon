import pygame, platform
from screeninfo import get_monitors

from engine.common.validated import ValidatedDict
from engine.common.constants import LogConstants
from engine.common.logger import LogManager

class Screen():
    def __init__(self, system_config: ValidatedDict, logger: LogManager):
        # Load the screen config and the game config
        self.screen_conf = system_config.get_dict('display')
        self.engine_conf = system_config.get_dict('engine')
        self.logger = logger

        # Sanity checks on file
        if self.screen_conf == None:
            self.logger.writeLogEntry('Null screen config, check your files!', LogConstants.STATUS_FAIL, tool="SCREEN_MGR")

        if self.engine_conf == None:
            self.logger.writeLogEntry('Null engine config, check your files!', LogConstants.STATUS_FAIL, tool="SCREEN_MGR")

        # Get the system so that we can load a larger icon for MacOS.
        system = platform.system()
        self.logger.writeLogEntry(f'You\'re running on {system}', LogConstants.STATUS_OK_GREEN)
        if system == 'Darwin':
            self.icon = './engine/assets/icons/icon_high.png'
        else:
            self.icon = './engine/assets/icons/icon_low.png'

    def initScreen(self):
        # start display
        pygame.display.init()

        # this can be 
        # - full
        # - borderless
        # - window

        mode = self.screen_conf.get_str('video_mode', 'window')
        screen_id = self.screen_conf.get_int('screen')
        resolution = self.screen_conf.get_str('resolution', '1920x1080').split('x')
        
        # Sanity check.
        if len(resolution) != 2:
            resolution = ['1920', '1080']

        window_height = int(resolution[1])
        window_width = int(resolution[0])
        pygame_flags = 0

        if mode == 'full':
            monitors = get_monitors()
            if len(monitors) < screen_id:
                raise Exception('Your screen variable is too large!')
            
            display = monitors[screen_id]

            window_height = display.height
            window_width = display.width
            pygame_flags = pygame.FULLSCREEN|pygame.NOFRAME

        elif mode == 'borderless':
            pygame_flags = pygame.NOFRAME

        # We should init the caption and icon before the screen runs.
        self.logger.writeLogEntry(f'Video mode: ({window_height}x{window_width}), {mode}', LogConstants.STATUS_OK_GREEN, tool="SCREEN_MGR")

        ver = self.engine_conf.get_str('build')
        pygame.display.set_caption(f'BoomerangNet Launcher V{ver} (init...)')
        pygame.display.set_icon(pygame.image.load(self.icon))

        # Start the screen
        screen = pygame.display.set_mode((window_width, window_height), pygame_flags, display=screen_id, vsync=self.screen_conf.get_bool('vsync'))
        return (screen, (window_width, window_height))