import base64
import os
import sys

class TS_API_Crypt:
    @staticmethod
    def Decrypt(strInput):
        if strInput is None or len(strInput) < 1:
            return strInput
        
        strInput = strInput.replace(b' ', b'+')  # Ensure bytes-like object
        numArray = list(base64.b64decode(strInput))
        bytes_key = b"smurfpass"

        for i in range(len(numArray)):
            length = i % len(bytes_key)
            numArray[i] = bytes([numArray[i] ^ bytes_key[length]])

        return b''.join(numArray)

if len(sys.argv) != 2:
    print('usage: ts_api_decrypt.py <FILE TO DUMP>')
    sys.exit()

filepath = sys.argv[1]
if os.path.exists(filepath):
    with open(filepath, 'rb') as sourceFile:
        outFile = TS_API_Crypt.Decrypt(sourceFile.read())
    with open(filepath.replace('.txt', '.xml'), 'wb') as saveFile:
        saveFile.write(outFile)
else:
    print("File not found!")