# Simple BoomerangNet Cyclon launcher program
import os, sys, json, requests, hashlib, subprocess

if not os.path.exists('./config.json'):
    print("Unable to load the config file!")
    sys.exit()

class CyclonLauncher:
    '''
    Simple launcher and updater program.
    '''
    def __init__(self) -> None:
        print("Welcome to the BoomerangNet Cyclon launcher program!")

        with open('./config.json', 'r') as jsonFile:
            config: dict = json.loads(jsonFile.read())
            self.common = config.get('common', {})
            self.network = config.get('network', {})
            self.launcher = config.get('launcher', {})

        self.updateneeded = False
        self.version = self.common.get('systemVersion', '0.0')
        self.taskList()

    def taskList(self) -> None:
        '''
        Simple way of executing all required tasks.
        '''
        self.checkSystem()
        self.checkForUpdates()
        self.startProcess()

    def getNetworkData(self, uri: str) -> dict:
        '''
        Given a URI, turn it into JSON.
        '''
        head_url = self.network.get('baseUrl')
        print(f'Connecting to: {head_url}{uri}')

        try:
            request = requests.get(f'{head_url}{uri}')
        except requests.exceptions.ConnectionError:
            print('Unable to connect to the server!')
            sys.exit()

        if request.status_code != 200:
            print(f'Bad response from server! Status code {request.status_code}')
            sys.exit()
        return request.json()

    def checkSystem(self) -> None:
        '''
        Check all game files.
        '''
        print('\nChecking all files...')
        uri = self.network.get('hashUri')
        version = self.version.replace('.', '_')
        hashList = self.getNetworkData(f'{uri}hashList_{version}.json')
        if hashList.get("systemVersion", "") != self.version:
            print('Version mismatch!')
            sys.exit()

        head_folder = self.common.get('headFolder', '')

        for file in hashList.get('hashes', []):
            filepath = head_folder + file.get('name', '')
            if not os.path.exists(filepath):
                print(f'{filepath} does not exist!')
                print('Please check your install!')
                sys.exit()

            if not self.launcher.get('skipHashCheck', False):
                with open(filepath, 'rb') as checkFile:
                    hash_ = hashlib.md5(checkFile.read())

                if file.get('hash', '') != hash_.hexdigest():
                    print(f'{filepath} has a hash mismatch!')
                    if self.launcher.get('haltOnMismatch', True):
                        sys.exit()

    def downloadUpdateFile(self, fileName: str, assetURL: str) -> None:
        outputPath = self.common.get('downloadFolder', '')

        try:
            request = requests.get(f'{assetURL}')
        except requests.exceptions.ConnectionError:
            print('Unable to connect to the server!')
            sys.exit()

        if request.status_code != 200:
            print(f'Bad response from server! Status code {request.status_code}')
            sys.exit()

        with open(f'{outputPath}{fileName}.zip', 'wb') as outFile:
            outFile.write(request.content)

    def checkForUpdates(self):
        '''
        Ask the server for updates
        '''
        print('\nChecking for updates...')
        buildData = self.getNetworkData(self.network.get('buildFile', '')).get('builds', {})
        thisBuild = buildData.get(self.version, {})
        if thisBuild == {}:
            print('Version not in server database!')
            self.exit()
        
        current_release = thisBuild.get('releaseDate', '')
        print(f'Current Build: {self.version} released on {current_release}')
        
        if thisBuild.get('isLatest', True):
            print('No update required!')
            return
        
        new_version = thisBuild.get('updateTo', '')
        new_build = buildData.get(new_version, {})
        new_release = new_build.get('releaseDate', '')
        self.updateneeded = True

        print(f'Update required! Version {self.version} to {new_version} released on {new_release}')
        new_version = new_version.replace('.', '_')
        update_url = new_build.get('storedAt', '')
        print(f'Downloading {update_url} as update_{new_version}')
        self.downloadUpdateFile(f'update_{new_version}', update_url)

    def startProcess(self):
        '''
        Starts the program to run after this.
        If self.updateneeded, run the patcher.
        Otherwise, start the game.
        '''
        startFile = self.launcher.get('patcher', ) if self.updateneeded else self.launcher.get('execute', )

        print(f'\nStarting {startFile}\nGoodbye!')
        subprocess.call(startFile)

CyclonLauncher()