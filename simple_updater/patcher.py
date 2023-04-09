# Simple BoomerangNet Cyclon patcher program
import os, sys, json, zipfile, time

if not os.path.exists('./config.json'):
    print("Unable to load the config file!")
    sys.exit()

class CyclonPatcher:
    '''
    Simple patcher program.
    '''
    def __init__(self) -> None:
        print("Welcome to the BoomerangNet Cyclon launcher program!")

        with open('./config.json', 'r') as jsonFile:
            config: dict = json.loads(jsonFile.read())
            self.common = config.get('common', {})
            self.patcher = config.get('patcher', {})

        self.updateFile = self.common.get('downloadFolder', '') + self.common.get('updateFile', '')
        self.taskList()

    def taskList(self) -> None:
        '''
        Simple way of executing all required tasks.
        '''
        self.unpackUpdateZip()
        self.installUpdate()

        if self.patcher.get('restartWhenDone', False):
            print('Done!\nRestarting the computer...')
            os.system("shutdown /r /t 5")
            time.sleep(10)

        print('Done!\nReturning to launcher!')
        sys.exit()

    def unpackUpdateZip(self) -> None:
        if not os.path.exists(self.updateFile):
            print('Unable to locate an update!\nRestarting to launcher...')
            sys.exit()

        print('\nUnpacking update...')
        with zipfile.ZipFile(self.updateFile, 'r') as zipRef:
            zipRef.extractall(self.patcher.get('tempFolder', ''))

    def installUpdate(self) -> None:
        print('\nInstalling update...')

        for subdir, dirs, files in os.walk(self.patcher.get('tempFolder', '')):
            for filename in files:
                fullfilepath = subdir + os.sep + filename
                fullfilepath = fullfilepath.replace('\\', '/')
                dst_path = fullfilepath.replace('./temp', '..')

                try:
                    os.rename(fullfilepath, dst_path)
                except FileExistsError:
                    os.remove(dst_path)
                    os.rename(fullfilepath, dst_path)
                except FileNotFoundError:
                    os.mkdir(subdir.replace('./temp', '..'))

CyclonPatcher()