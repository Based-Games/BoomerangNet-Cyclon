# Simple BoomerangNet Cyclon launcher program
import os, sys, json, requests

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

        self.version = self.common.get('systemVersion', '0.0')
        print(f'Current Build: {self.version}')
        self.taskList()

    def taskList(self) -> None:
        '''
        Simple way of executing all required tasks.
        '''
        self.checkSystem()

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
        uri = self.network.get('hashUri')
        version = self.version.replace('.', '_')
        hashList = self.getNetworkData(f'{uri}hashList_{version}.json')

CyclonLauncher()