import sys
import os
from xml.dom import minidom

class Pnix_Xts:
    skey = "smurfpass"

    @staticmethod
    def Decrypt(btEncrypted):
        bytes_key = bytearray("smurfpass", "utf-8")
        for i in range(len(btEncrypted)):
            length = i % len(bytes_key)
            btEncrypted[i] = btEncrypted[i] ^ bytes_key[length]
        return btEncrypted

    @staticmethod
    def DecryptFile(sInputFilename, sOutputFilename):
        with open(sInputFilename, 'rb') as fileStream:
            numArray = bytearray(fileStream.read())
            numArray = Pnix_Xts.Decrypt(numArray)
            with open(sOutputFilename, 'wb') as fileStream1:
                fileStream1.write(numArray)

    @staticmethod
    def EncryptFile(sInputFilename, sOutputFilename):
        with open(sInputFilename, 'rb') as fileStream:
            numArray = bytearray(fileStream.read())
            bytes_key = bytearray("smurfpass", "utf-8")
            for i in range(len(numArray)):
                length = i % len(bytes_key)
                numArray[i] = numArray[i] ^ bytes_key[length]
            with open(sOutputFilename, 'wb') as fileStream1:
                fileStream1.write(numArray)

    @staticmethod
    def XtsToXml(strXtsFilePath):
        with open(strXtsFilePath, 'rb') as fileStream:
            numArray = bytearray(fileStream.read())
            numArray = Pnix_Xts.Decrypt(numArray)
            str_data = numArray.decode('utf-8')  # Specify the correct encoding
            print("Decrypted data:", str_data)  # Print the decrypted data
            xmlDocument = minidom.parseString(str_data)
        return xmlDocument

if len(sys.argv) != 2:
    print('usage: decrypt_pnix.py <FILE TO DUMP>')
    sys.exit()

filepath = sys.argv[1]
if os.path.exists(filepath):
    # Decrypt the text file and convert it to XML
    xml_doc = Pnix_Xts.XtsToXml(filepath)

    # Save the XML document to the output file
    with open(filepath.replace('.txt', '.xml'), 'w') as f:
        f.write(xml_doc.toprettyxml(indent='  '))