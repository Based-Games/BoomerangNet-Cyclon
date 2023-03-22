from pygame import Surface, image, mixer, font, transform
import os

from engine.common.validated import ValidatedDict
from engine.common.constants import LogConstants
from engine.common.logger import LogManager

class AssetManager:
    '''
    Asset loaders, renderers, transformers and more!
    '''
    asset_prefix = "./engine/assets"

    def __init__(self, config: ValidatedDict, logger: LogManager) -> None:
        self.logger = logger
        self.config = config
        self.loaded = {}
    
    def loadImage(self, asset_name: str) -> Surface:
        '''
        Load an image in Texture form.

        Given:
            - asset_name: name of the asset, including extension.
        
        Returns: Asset as a texture.
        '''
        asset_path = f"{self.asset_prefix}/images/{asset_name}"

        if asset_path not in self.loaded.keys():
            if os.path.exists(asset_path):
                self.logger.writeLogEntry(f'Loading asset: {asset_name}', status=LogConstants.STATUS_OK_BLUE, tool="ASSET_MGR")
                self.loaded[asset_path] = image.load(asset_path)
            else:
                self.logger.writeLogEntry(f'Couldn\'t find {asset_name}!', status=LogConstants.STATUS_FAIL, tool="ASSET_MGR")
        
        return self.loaded[asset_path]
    
    def drawTexture(self, texture: Surface, coordinates: tuple[int, int], size: tuple[int, int] = None, direct: bool = False):
        # let's throw this on the screen.
        original_res = texture.get_size()
        textrect = texture.get_rect()
        textrect.center = coordinates
        if size != None:
            if direct:
                texture = transform.smoothscale(texture, (
                    int(original_res[0]/size[0]),
                    int(original_res[1]/size[1])
                ))
            else:
                texture = transform.smoothscale(texture, (
                    int(size[0]*original_res[0]/self.resolution[0]),
                    int(size[1]*original_res[1]/self.resolution[1])
                ))
        self.screen.blit(texture, coordinates)

    def loadFont(self, asset_name: str, system_res: tuple, size: int) -> Surface:
        '''
        Load a font in PyGame Font form.

        Given:
            - asset_name: name of the asset, including extension.
        
        Returns: Asset as a PyGame Font.
        '''
        asset_path = f"{self.asset_prefix}/fonts/{asset_name}.ttf"

        if asset_path not in self.loaded.keys():
            if os.path.exists(asset_path):
                self.logger.writeLogEntry(f'Loading asset: {asset_name}', status=LogConstants.STATUS_OK_BLUE, tool="ASSET_MGR")
                self.loaded[asset_path] = font.Font(asset_path, int(size*(system_res[1]/768)))
            else:
                self.logger.writeLogEntry(f'Couldn\'t find {asset_name}!', status=LogConstants.STATUS_FAIL, tool="ASSET_MGR")

        return self.loaded[asset_path]

    def drawText(self, text: str, color: tuple, surface: Surface, x: int, y: int, size: int, align: int, system_res: tuple, font: str = 'system'):
        font = self.loadFont(font, system_res, size)
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

    def playSfx(self, asset_name: str) -> Surface:
        '''
        Load a sound in sound form.

        Given:
            - asset_name: name of the asset, including extension.
        
        Returns: Nothing.
        '''
        asset_path = f"{self.asset_prefix}/sfx/{asset_name}"
        sound_settings = self.config.get_dict('sound')
        if sound_settings == None:
            raise Exception("Sound settings in JSON are missing!")

        if os.path.exists(asset_path):
            self.logger.writeLogEntry(f'Loading asset: {asset_name}', status=LogConstants.STATUS_OK_BLUE, tool="ASSET_MGR")
            sound = mixer.Sound(asset_path)
            sound.set_volume(sound_settings.get('sfx_volume', 1.0)-0.4)
            sound.play()

        else:
            self.logger.writeLogEntry(f'Couldn\'t find {asset_name}!', status=LogConstants.STATUS_FAIL, tool="ASSET_MGR")