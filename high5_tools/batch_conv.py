import os
import sys

from ts_api_decrypt import TS_API_Crypt
from high5_to_cyclon import High5toCyclon

if len(sys.argv) != 2:
    print('usage: batch_conv.py <PATH TO CONVERT>')
    sys.exit()

filepath = sys.argv[1]
if os.path.exists(filepath):
    for subdir, dirs, files in os.walk(filepath):
        for file in files:
            if file.endswith('.txt'):
                chartPath = os.path.join(subdir, file)
                with open(chartPath, 'rb') as encryptedChart:
                    decryptedChart = TS_API_Crypt.Decrypt(encryptedChart.read())
                    encryptedChart.close()

                with open(chartPath.replace('.txt', '_tmp.xml'), 'wb') as saveFile:
                    saveFile.write(decryptedChart)
                    saveFile.close()

                with open(chartPath.replace('.txt', '.xml'), 'wb') as outFile:
                    outFile.write(High5toCyclon.ParseChart(High5toCyclon(), chartPath.replace('.txt', '_tmp.xml')))
                    outFile.close()

                os.remove(chartPath.replace('.txt', '_tmp.xml'))
    print("Done, enjoy!")
else:
    print("Path not found!")