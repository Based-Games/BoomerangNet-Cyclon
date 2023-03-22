import pygame, os, time

from game.db import gameDatabaseAccess
from game.validated import ValidatedDict

class systemTestMenu:
    '''
    Class for the system test menu. Lets you change settings of the game and whatnot. 
    Applies them to the database.
    '''

    def __init__(self, surface: pygame.surface.Surface, resolution: tuple, clock: pygame.time.Clock, framerate: int) -> None:
        self.testing = True
        self.current_select = 0
        self.len_settings = 0
        self.test_state = None # If the test state is None, return the main test menu. Otherwise, render the test menu that we care about.
        self.current_value = [0]*64
        self.len_values = 0
        self.disable_esc = False
        self.needs_enter = False
        self.esc_go_back = False
        self.kill_ud = False
        self.index = 0
        self.set_value = 0
        self.enter_pressed = False
        self.last_setting = 0

        self.text_color = (255, 255, 255)
        self.surface = surface
        self.resolution = resolution
        self.clock = clock
        self.framerate = framerate
        self.header_text = ''

        # Now, we should load the system font path into a var. We'll do a simple check on it to be safe.
        font_path = './assets/fonts/testmenu.ttf'
        if os.path.exists(font_path):
            self.system_font = font_path
        else: raise Exception(f"Can't load the system test menu font! Please check that {font_path} exists!")

        pygame.display.set_caption('BasedFighter V0.1 (Test Menu)')
        self.mainTestMenu()

    def drawTestMenuText(self, text: str, color: tuple, surface: pygame.surface.Surface, x: int, y: int, size: int, align: int):
        font = pygame.font.Font(self.system_font, int(size*self.resolution[1]/768))
        textobj = font.render(text, 1, color)
        textrect = textobj.get_rect()

        if align == 0:
            textrect.topleft = (x, y)
        elif align == 1:
            textrect.center = (x, y)
        elif align == 2:
            textrect.topright = (x, y)
        else: raise Exception('Unknown font position! Please use 0, 1, 2!')

        surface.blit(textobj, textrect)

    def drawHeader(self, add_ud: bool, add_sel: bool, add_lr: bool, add_esc: bool):
        # I keep needing these so i made it a func
        self.drawTestMenuText(self.header_text, self.text_color, self.surface, self.resolution[0]/2, int(30*self.resolution[1]/768), 50, 1)

        footers = []
        if add_ud:
            footers.append('UP/DOWN/F2 to move')
        if add_lr:
            footers.append('LEFT/RIGHT to change value')
        if add_sel:
            footers.append('ENTER to select')
        if add_esc:
            footers.append('ESC to go back')

        buffer = 600
        for footer in footers:
            self.drawTestMenuText(footer, self.text_color, self.surface, self.resolution[0]/2, int(buffer*self.resolution[1]/768), 35, 1)
            buffer += 50

    def eventHandler(self):
        '''
        Handles game events.
        '''
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self.testing = False
                print('thank you for playing!')
                pygame.display.quit()
                exit()

            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_F2 and self.testing:
                    self.current_select = self.current_select+1 if self.current_select+1 < self.len_settings else 0
                if event.key == pygame.K_DOWN and self.testing:
                    if not self.kill_ud:
                        self.current_select = self.current_select+1 if self.current_select+1 < self.len_settings else 0
                if event.key == pygame.K_UP and self.testing:
                    if not self.kill_ud:
                        self.current_select = self.current_select-1 if self.current_select-1 >= 0 else self.len_settings-1
                if event.key == pygame.K_RIGHT and self.testing:
                    self.current_value[self.set_value] = self.current_value[self.set_value]+1 if self.current_value[self.set_value]+1 < self.len_values else 0
                if event.key == pygame.K_LEFT and self.testing:
                    self.current_value[self.set_value] = self.current_value[self.set_value]-1 if self.current_value[self.set_value]-1 >= 0 else self.len_values-1
                if event.key == pygame.K_RETURN and self.testing:
                    if not self.needs_enter:
                        self.last_setting = self.current_select
                        self.test_state = self.current_select
                        self.current_select = 0
                    else:
                        self.enter_pressed = True
                        self.esc_go_back = True
                        self.kill_ud = True
                if event.key == pygame.K_F1 and self.testing:
                    if not self.needs_enter:
                        self.last_setting = self.current_select
                        self.test_state = self.current_select
                        self.current_select = 0
                    else:
                        self.enter_pressed = True
                        self.esc_go_back = True
                        self.kill_ud = True
                if event.key == pygame.K_ESCAPE and self.testing:
                    if self.esc_go_back:
                        self.enter_pressed = False
                        self.kill_ud = False
                        self.esc_go_back = False
                    elif not self.disable_esc:
                        self.needs_enter = False
                        self.disable_esc = False
                        self.current_select = self.last_setting
                        self.current_value[self.index] = 0
                        self.test_state = None

    def mainTestMenu(self):
        # First off, let's make sure that the screen has been wiped.
        self.surface.fill((0, 0, 0))
        
        test_options = [
            'Input Test',
            'Game Options',
            'Coin Options',
            'Network Options',
            'Input Options',
            'All Factory Settings',
            'Leave Test Mode'
        ]

        factory_states = [
            {
                'id': 1,
                'name': 'Game Options',
                'data': [
                    {
                        'name': 'Game Timer',
                        'values': [
                            '30 Secs.',
                            '45 Secs.',
                            '60 Secs.',
                            '70 Secs.',
                            '80 Secs.',
                        ],
                        'default': 2,
                        'set_to': None
                    },
                    {
                        'name': 'Game Mode',
                        'values': [
                            'Home',
                            'Arcade',
                            'Event Mode',
                        ],
                        'default': 0,
                        'set_to': None
                    },
                    {
                        'name': 'Menu Timers',
                        'values': [
                            'Standard',
                            '10 Secs. More',
                            '20 Secs. More',
                            'Off',
                        ],
                        'default': 0,
                        'set_to': None
                    },
                ]
            },
            {
                'id': 2,
                'name': 'Coin Options',
                'data': [
                    {
                        'name': 'Coin Mode',
                        'values': [
                            'Freeplay',
                            '1 Coin = 1 Credit',
                            '2 Coins = 1 Credit',
                            '3 Coins = 1 Credit',
                            '4 Coins = 1 Credit',
                        ],
                        'default': 0,
                        'set_to': None
                    },
                    {
                        'name': 'Continue',
                        'values': [
                            'Free',
                            '1 Coin',
                            '2 Coins',
                            '3 Coins',
                        ],
                        'default': 1,
                        'set_to': None
                    },
                ]
            },
            {
                'id': 3,
                'name': 'Network Options',
                'data': [
                    {
                        'name': 'Use Network',
                        'values': [
                            'No',
                            'Yes'
                        ],
                        'default': 0,
                        'set_to': None
                    }
                ]
            },
            {
                'id': 4,
                'name': 'Input Options',
                'data': [
                    {
                        'name': 'Input Mode',
                        'values': [
                            'Keyboard',
                            'Controller',
                        ],
                        'default': 0,
                        'set_to': None
                    },
                    {
                        'name': 'Button Count',
                        'values': [
                            '2 Buttons',
                            '3 Buttons',
                            '4 Buttons',
                            '5 Buttons',
                            '6 Buttons',
                        ],
                        'default': 2,
                        'set_to': None
                    },
                ]
            },
        ]

        self.len_settings = len(test_options)
        self.current_select = 0

        while self.testing != False:
            # Tick the clock for good luck!
            self.clock.tick(self.framerate)

            # Let's do an event check
            self.eventHandler()

            # Because test menu is in default state, load the main test menu.
            self.surface.fill((0, 0, 0))
            self.header_text = "Test Menu"
            self.drawHeader(True, True, False, False)

            buffer = int(100*self.resolution[1]/768)
            index = 0
            self.len_settings = len(test_options)
            for option in test_options:
                if index == self.current_select:
                    self.text_color = (255, 0, 0)
                self.drawTestMenuText(option, self.text_color, self.surface, self.resolution[0]/2, buffer, 25, 1)
                buffer += int(30*self.resolution[1]/768)
                self.text_color = (255, 255, 255)
                index +=1

            if self.test_state == 6:
                # We will now leave test mode.
                self.index = 0
                self.test_state = None
                self.surface.fill((0, 0, 0))
                print('Now leaving test mode...')
                self.testing = False

            elif self.test_state == 0:
                # IO test shit
                self.index = 0
                self.surface.fill((0, 0, 0))
                self.header_text = 'IO Test Menu'
                self.drawHeader(False, False, False, True)

            elif self.test_state == 5:
                # All factory settings type shit
                self.index = 0
                self.surface.fill((0, 0, 0))
                self.header_text = 'All Factory Settings'
                self.drawHeader(False, True, True, False)
                self.drawTestMenuText('Are you sure you want to erase all settings?', self.text_color, self.surface, self.resolution[0]/2, int(300*self.resolution[1]/768), 25, 1)
                values = ['No', 'Yes']
                self.drawTestMenuText(values[self.current_value[self.index]], (255, 0, 0), self.surface, self.resolution[0]/1.75, int(360*self.resolution[1]/768), 25, 1)
                pygame.display.update()
                self.needs_enter = True
                self.disable_esc = True

                if self.enter_pressed and self.current_value[self.index] == -1:
                    # Restore settings
                    self.surface.fill((0, 0, 0))
                    self.header_text = 'Please Wait...'
                    self.drawHeader(False, False, False, False)
                    pygame.display.update()
                    for set in factory_states:
                        gameDatabaseAccess().saveSetting(ValidatedDict(set))
                    time.sleep(2)
                    self.current_value[self.index] = 0
                    self.current_select = self.last_setting
                    self.needs_enter = False
                    self.enter_pressed = False
                    self.disable_esc = False
                    self.esc_go_back = False
                    self.kill_ud = False
                    self.test_state = None
                    
                elif self.enter_pressed and self.current_value[self.index] == 0:
                    self.surface.fill((0, 0, 0))
                    self.header_text = 'NO MODIFICATION'
                    self.drawHeader(False, False, False, False)
                    pygame.display.update()
                    time.sleep(2)
                    self.current_value[self.index] = 0
                    self.current_select = self.last_setting
                    self.needs_enter = False
                    self.enter_pressed = False
                    self.disable_esc = False
                    self.esc_go_back = False
                    self.kill_ud = False
                    self.test_state = None

            else:
                # Now, we can load the test menu for each setting.
                if self.test_state != None:
                    self.needs_enter = True
                    setting = gameDatabaseAccess().loadSetting(self.test_state)
                    if setting == None:
                        setting = ValidatedDict(factory_states[self.test_state-1])
                    # Standard shit, render sub-settings
                    self.surface.fill((0, 0, 0))
                    self.header_text = setting.get_str('name')
                    self.drawHeader(True, True, True, True)

                    index = 0
                    buffer = int(100*self.resolution[1]/768)
                    self.text_color = (255, 255, 255)
                    self.len_settings = len(setting['data'])
                    for sub_set in setting['data']:
                        if index == self.current_select:
                            self.text_color = (255, 0, 0)
                        self.drawTestMenuText(sub_set['name'], self.text_color, self.surface, self.resolution[0]/3, buffer, 32, 1)
                        if sub_set['set_to'] == None:
                            sub_set['set_to'] = sub_set['default']
                        if not self.enter_pressed:
                            self.text_color = (255, 255, 255)
                            self.len_values = 0
                        else:
                            self.current_value[self.set_value] = sub_set['set_to']
                            self.index = index
                            self.len_values = len(sub_set['values'])
                        self.drawTestMenuText(sub_set['values'][self.current_value[self.index]], self.text_color, self.surface, self.resolution[0]/1.75, buffer, 32, 1)
                        self.text_color = (255, 255, 255)
                        buffer += int(30*self.resolution[1]/768)
                        index +=1

            pygame.display.update()

        return None # Send the game back to an init state.