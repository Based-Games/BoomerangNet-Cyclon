class LogConstants:
    '''
    Constant logger states and codes.
    '''

    LOGLEVEL_DISABLE = 0 # No console logging or text logging.
    LOGLEVEL_ENABLE = 1 # Log important data, but not too much.
    LOGLEVEL_DEBUG = 2 # Log everything.
    LOGLEVEL_ERRORS = 3 # Log only errors.

    LOG_DISABLE = "disable"
    LOG_ENABLE = "enable"
    LOG_DEBUG = "debug"
    LOG_ERRORS = "errors"

    STATUS_HEADER = 0
    STATUS_OK_BLUE = 1
    STATUS_OK_CYAN = 2
    STATUS_OK_GREEN = 3
    STATUS_WARNING = 4
    STATUS_FAIL = 5
    STATUS_BOLD = 6
    STATUS_UNDERLINE = 7

    TEXT_HEADER = '\033[95m'
    TEXT_OK_BLUE = '\033[94m'
    TEXT_OK_CYAN = '\033[96m'
    TEXT_OK_GREEN = '\033[92m'
    TEXT_WARNING = '\033[93m'
    TEXT_FAIL = '\033[91m'
    TEXT_END = '\033[0m'
    TEXT_BOLD = '\033[1m'
    TEXT_UNDERLINE = '\033[4m'

    LOGFILE = "./log.txt"

    @classmethod
    def toLoglevel(self, loglevel: str = None) -> int:
        ''' 
        Given a loglevel string, give a status code.
        '''
        return {
            self.LOG_DISABLE: self.LOGLEVEL_DISABLE,
            self.LOG_ENABLE: self.LOGLEVEL_ENABLE,
            self.LOG_DEBUG: self.LOGLEVEL_DEBUG,
            self.LOG_ERRORS: self.LOGLEVEL_ERRORS
        }[loglevel if loglevel != None else "enable"]
    
    @classmethod
    def fromLoglevel(self, loglevel: int = None) -> str:
        ''' 
        Given a loglevel code, give a status str.
        '''
        return {
            self.LOGLEVEL_DISABLE: self.LOG_DISABLE,
            self.LOGLEVEL_ENABLE: self.LOG_ENABLE,
            self.LOGLEVEL_DEBUG: self.LOG_DEBUG,
            self.LOGLEVEL_ERRORS: self.LOG_ERRORS
        }[loglevel if loglevel != None else 1]

    @classmethod
    def getColor(self, status_code: int = None) -> str:
        '''
        Given a color status code, return a color.
        '''
        return {
            self.STATUS_HEADER: self.TEXT_HEADER,
            self.STATUS_OK_BLUE: self.TEXT_OK_BLUE,
            self.STATUS_OK_CYAN: self.TEXT_OK_CYAN,
            self.STATUS_OK_GREEN: self.TEXT_OK_GREEN,
            self.STATUS_WARNING: self.TEXT_WARNING,
            self.STATUS_FAIL: self.TEXT_FAIL
        }[status_code if status_code != None else self.STATUS_OK_GREEN]