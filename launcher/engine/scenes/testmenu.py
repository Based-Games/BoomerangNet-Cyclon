# System test and settings menu
import pygame, sys

# Start importing libs
from engine.common.validated import ValidatedDict
from engine.common.logger import LogManager
from engine.common.constants import LogConstants
from engine.common.asset import AssetManager

class systemTestMenu (
    AssetManager
):
    '''
    Class for the system test menu. Lets you change settings of the game and whatnot. 
    Applies them to the database.
    '''

    def __init__(self, engine_config: ValidatedDict, logger: LogManager, surface: pygame.surface.Surface, resolution: list[int]) -> None:
        self.ver = engine_config.get_dict('engine', ValidatedDict({})).get_str('build')

        self.tool = 'TEST_MGR'
        self.current_state = 'TEST_MODE'
        self.last_state = 'LOADING_TEST'
        self.testing = True
        self.screen = surface
        self.resolution = resolution

        self.loaded = {}
        self.logger = logger

        # Button stuff
        self.max_in_row = 4
        self.buttons = {}

        # Report that we're alive.
        self.logger.writeLogEntry('Welcome to the System Test Menu!', tool=self.tool)

    def eventHandler(self, recursive: bool = False):
        '''
        Handles engine events and a few other things.
        '''
        engine_mode = {
            'QUIT': "(goodbye!)",
            'LOADING_TEST': "(loading test menu...)",
            'TEST_MODE': "(in test menu)",
            'LEAVE_TEST': "(returning to launcher...)"
        }[self.current_state]

        # Log dat shit
        if self.current_state != self.last_state:
            self.logger.writeLogEntry(f'Switching state to {self.current_state}.', LogConstants.STATUS_OK_CYAN)
            self.last_state = self.current_state
            pygame.display.set_caption(f'BoomerangNet Launcher V{self.ver} {engine_mode}')

        if self.current_state == 'QUIT':
            self.logger.writeLogEntry('Goodbye! Thank you for playing.', LogConstants.STATUS_HEADER)
            sys.exit()

        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self.current_state = "QUIT"

            if event.type == pygame.MOUSEBUTTONDOWN:
                '''
                Handle mouse/touch.
                '''
                mouse_pos = pygame.mouse.get_pos()
                for i in self.buttons.keys():
                    if self.buttons[i] == None:
                        continue
                    if self.buttons[i][0]+130 > mouse_pos[0] > self.buttons[i][0]-130 and self.buttons[i][1]+65 > mouse_pos[1] > self.buttons[i][1]-65:
                        print(i)

            pressed = pygame.key.get_pressed()
            if pressed[pygame.K_ESCAPE]:
                self.current_state = 'LEAVE_TEST'
                self.testing = False

    def renderButtons(self, buttons: list):
        i = 0
        x = 30
        y = 150
        for button in buttons:
            if i == self.max_in_row:
                x = 30
                y += 140
                i = 1
            
            buttonAsset = self.loadImage('TestButton.png')
            self.drawTexture(buttonAsset, (x, y), (1150, 650))
            self.drawText(button, (10, 10, 10), self.screen, x+145, y+65, 50, 1, self.resolution)
            self.buttons[button] = (x+145, y+65)
            
            x += 280
            i += 1

    def run(self):
        '''
        Top level scene loop
        '''
        while self.testing:
            self.eventHandler()
            self.drawText('Test Menu', (200, 200, 200), self.screen, self.resolution[0]/2, 50, 60, 1, self.resolution, 'system2')

            testbuttons = ['Screen', 'Coin', 'Shit', 'Fuck', 'Bitch', 'Internet', 'Say Gex', 'Balls', 'Whar']
            self.renderButtons(testbuttons)

            pygame.display.update()

        self.logger.writeLogEntry('Returning to the launcher', tool=self.tool)
        return self.current_state