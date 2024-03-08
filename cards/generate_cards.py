import csv
import random
import string
import binascii

count = 100
prefix = "BOOM"
sector_key = [0x37, 0x21, 0x53, 0x6a, 0x72, 0x40]
cardList = []

def generate_random_hex(length):
    hex_chars = string.hexdigits[:-6].upper()  # Exclude lowercase letters
    return ''.join(random.choice(hex_chars) for _ in range(length))

for i in range(count):
    cardID = prefix + generate_random_hex(16)
    while cardID in cardList:
        print('Repeat ID!')
        cardID = prefix + generate_random_hex(16)
    cardList.append(cardID)

with open('./cards.csv', 'w', newline='') as csvFile:
    output = csv.writer(csvFile)
    fields = ['PRINT', 'KEY', 'SECTOR0_BLOCK0', 'SECTOR0_BLOCK1']
    output.writerow(fields)

    for cardID in cardList:
        printCode = '-'.join(cardID[i:i+4] for i in range(0, len(cardID), 4))
        cardID = binascii.hexlify(cardID.encode()).decode()
        block0 = cardID[:32]  # First 32 characters
        block1 = cardID[32:]  # Last 32 characters
        
        output.writerow([
            printCode,
            ''.join(format(byte, '02X') for byte in sector_key),
            block0,
            block1,
        ])