from Crypto.Cipher import AES
import base64
import pkcs7
import sys
import os

AUTH_KEY = "1248674976451617"
MODE = AES.MODE_CBC

# Generate a random IV
IV = os.urandom(16)

# Convert the key to bytes (assuming it's a string)
AUTH_KEY = AUTH_KEY.encode('utf-8')

# Create the AES cipher object with the provided key and IV
crypto = AES.new(AUTH_KEY, MODE, iv=IV)

def Decrypt(Input):
    return pkcs7.PKCS7Encoder().decode(crypto.decrypt(base64.b64decode(Input.replace(' ','+'))))

def Encrypt(Input):
    return base64.b64encode(crypto.encrypt(pkcs7.PKCS7Encoder().encode(Input)))

if len(sys.argv) != 2:
    print('usage: decrypt_push.py <FILE TO DUMP>')
    sys.exit()

filepath = sys.argv[1]
if os.path.exists(filepath):
    with open(filepath, 'rb') as sourceFile:
        outFile = Decrypt(sourceFile.read())
    with open(f'{filepath}.dec', 'wb') as saveFile:
        saveFile.write(outFile)
else:
    print("File not found!")