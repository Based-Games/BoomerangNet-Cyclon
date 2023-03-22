import os
from sys import exit
from json import dumps, loads

from engine.common.validated import ValidatedDict
from engine.common.constants import LogConstants
from engine.common.logger import LogManager

class JSONData:
    '''
    Used for loading and saving engine JSON data.
    '''
    def __init__(self, logger: LogManager) -> None:
        self.logger = logger

    def loadJsonFile(self, path: str) -> ValidatedDict:
        '''
        Load a given JSON file.
        '''
        if os.path.exists(path):
            self.logger.writeLogEntry(f'Loading JSON: {path}', status=LogConstants.STATUS_OK_BLUE, tool="JSON_MGR")

            out = None
            with open(path, 'r') as file:
                out = ValidatedDict(loads(file.read()))

            return out
        else:
            self.logger.writeLogEntry(f'Couldn\'t find {path}!', status=LogConstants.STATUS_FAIL, tool="JSON_MGR")
            exit()

    def writeJsonFile(self, data: ValidatedDict, path: str) -> None:
        '''
        Write a given JSON file.
        '''
        with open(path, 'w') as file:
            file.wite(dumps(data, indent=4))