class Pnix_Xts:
    @staticmethod
    def Decrypt(btEncrypted):
        bytes_key = b"smurfpass"  # Use bytes literal instead of string literal
        btEncrypted = list(btEncrypted)
        for i in range(len(btEncrypted)):
            length = i % len(bytes_key)
            btEncrypted[i] = chr(btEncrypted[i] ^ bytes_key[length])  # Remove ord() and chr()
        return ''.join(btEncrypted)

    @staticmethod
    def DecryptFile(sInputFilename, sOutputFilename):
        with open(sInputFilename, 'rb') as fileStream:
            with open(sOutputFilename, 'wb') as fileStream1:
                numArray = fileStream.read()
                numArray = Pnix_Xts.Decrypt(numArray)
                fileStream1.write(numArray.encode())

    @staticmethod
    def EncryptFile(sInputFilename, sOutputFilename):
        with open(sInputFilename, 'rb') as fileStream:
            with open(sOutputFilename, 'wb') as fileStream1:
                numArray = list(fileStream.read())
                bytes_key = "smurfpass"
                for i in range(len(numArray)):
                    length = i % len(bytes_key)
                    numArray[i] = chr(ord(numArray[i]) ^ ord(bytes_key[length]))
                numArray = ''.join(numArray)
                fileStream1.write(numArray.encode())

if __name__ == '__main__':
    import sys
    for i in sys.argv[1:]:
        Pnix_Xts.DecryptFile(i, i[:i.rfind('.')] + '.xml')