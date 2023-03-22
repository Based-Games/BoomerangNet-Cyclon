from datetime import datetime
from engine.common.constants import LogConstants

class LogManager:
    '''
    Main system logger. Saves log to a logfile, does some other things.
    '''

    def __init__(self, loglevel: str = "enable" ) -> None:
        # Get the log level.
        self.loglevel: int = LogConstants.toLoglevel(loglevel)

    def initLogFile(self):
        '''
        Wipe out the logfile upon loading of the engine. 
        '''
        # Make sure we want a logfile.
        if self.loglevel:
            with open(LogConstants.LOGFILE, 'w') as file:
                file.truncate(0)

    def writeLogEntry(self, message: str = None, status: int = LogConstants.STATUS_OK_GREEN, underline: bool = False, bold: bool = False, tool: str = "ENGINE") -> None:
        '''
        Given a log message, status, and some extra things and write a log message with it.

        Writes to the logfile and to the console, with cool colors too!

        Appends time and date to every entry as well.
        '''
        old_log = ''
        log_msg = '' # Don't add color data to this until AFTER we save to log.

        log_type = {
            LogConstants.STATUS_FAIL: 'F: ',
            LogConstants.STATUS_WARNING: 'W: ',
            LogConstants.STATUS_HEADER: '',
            LogConstants.STATUS_OK_BLUE: 'DBG: ',
            LogConstants.STATUS_OK_CYAN: '',
            LogConstants.STATUS_OK_GREEN: ''
        }[status]

        if self.loglevel:
            current_time = datetime.now().strftime("%m/%d/%Y %H:%M:%S")

            # Dump old logfile
            with open(LogConstants.LOGFILE, 'r') as logfile:
                old_log = logfile.read()
            
            # Don't log annoying stuff unless wanted.
            if status == LogConstants.STATUS_OK_BLUE and self.loglevel != LogConstants.LOGLEVEL_DEBUG:
                return
            if self.loglevel == LogConstants.LOGLEVEL_ERRORS and status < LogConstants.STATUS_WARNING:
                return

            log_msg = f"[{current_time} from {tool}] {log_type}{message}"
            print(f"{LogConstants.getColor(status)}{log_msg}{LogConstants.TEXT_END}")
            
            # Update logfile
            with open(LogConstants.LOGFILE, 'w') as file:
                old_log += f"{log_msg}\n"
                file.write(old_log)

            # Kill software if needed.
            if status == LogConstants.STATUS_FAIL:
                exit()
            
            # We're done!
            return