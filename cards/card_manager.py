import ctypes
import sys

# Load the DLL
try:
    lib = ctypes.windll.LoadLibrary("./CRT_R1.dll")
except OSError as e:
    print("Failed to load the library:", e)
    sys.exit(1)

# Define necessary ctypes types
LPSTR = ctypes.c_char_p
LPBYTE = ctypes.POINTER(ctypes.c_byte)

# Define the functions from the DLL
CommOpen = lib.CommOpen
CommOpen.argtypes = [LPSTR]
CommOpen.restype = ctypes.c_void_p

CommSetting = lib.CommSetting
CommSetting.argtypes = [ctypes.c_void_p, LPSTR]
CommSetting.restype = ctypes.c_int

CRT286_GetStatus = lib.CRT286_GetStatus
CRT286_GetStatus.argtypes = [ctypes.c_void_p, LPBYTE]
CRT286_GetStatus.restype = ctypes.c_int

CRT286_Eject = lib.CRT286_Eject
CRT286_Eject.argtypes = [ctypes.c_void_p]
CRT286_Eject.restype = ctypes.c_int

CommClose = lib.CommClose
CommClose.argtypes = [ctypes.c_void_p]
CommClose.restype = ctypes.c_int

RF_DetectCard = lib.RF_DetectCard
RF_DetectCard.argtypes = [ctypes.c_void_p]
RF_DetectCard.restype = ctypes.c_int

RF_GetCardID = lib.RF_GetCardID
RF_GetCardID.argtypes = [ctypes.c_void_p, LPBYTE]
RF_GetCardID.restype = ctypes.c_int

RF_LoadSecKey = lib.RF_LoadSecKey
RF_LoadSecKey.argtypes = [ctypes.c_void_p, ctypes.c_byte, ctypes.c_byte, LPBYTE]
RF_LoadSecKey.restype = ctypes.c_int

RF_ReadBlock = lib.RF_ReadBlock
RF_ReadBlock.argtypes = [ctypes.c_void_p, ctypes.c_byte, ctypes.c_byte, LPBYTE]
RF_ReadBlock.restype = ctypes.c_int

# Define the Python class equivalent
class CardManager:
    m_Comm = None
    m_bInit = False
    m_strId = ''

    @staticmethod
    def Init():
        text = b"Com6"
        CardManager.m_Comm = CommOpen(text)
        text = b"9600,n,8,1"
        if CommSetting(CardManager.m_Comm, text) != 0:
            print("COM Port not found!")
            CardManager.terminateCard()
            return
        if not CardManager.IsConnected():
            print("Card reader not found!")
            CardManager.terminateCard()
            return
        CardManager.m_bInit = True
        print("Card reader setup success!")
        CardManager.eject()

    @staticmethod
    def read(sector, block, pBuffer):
        return RF_ReadBlock(CardManager.m_Comm, sector, block, pBuffer) == 0

    @staticmethod
    def loadBase():
        array = (ctypes.c_byte * 32)()
        array2 = (ctypes.c_byte * 32)()
        if not CardManager.read(0, 1, array):
            return False
        if not CardManager.read(0, 2, array2):
            return False
        text = array.value.decode().rstrip('\x00')
        text2 = array2.value.decode().rstrip('\x00')
        CardManager.m_strId = text + text2
        return True

    @staticmethod
    def isInsertCard():
        if RF_DetectCard(CardManager.m_Comm) != 0:
            return False
        array = (ctypes.c_byte * 4)()
        return RF_GetCardID(CardManager.m_Comm, array) == 0

    @staticmethod
    def isDefaultPassword():
        default_passwords = [
            b"retsam",
            b"7!Sjr@",
            b"#Wn0!1",
            b"DMT#01",
            b"!DMT1!",
            b"^N!p@j"
        ]
        for password in default_passwords:
            if CardManager.verifySecureKey(0, password):
                return True
        return False

    @staticmethod
    def verifySecureKey(sector, pBuffer):
        return RF_LoadSecKey(CardManager.m_Comm, sector, 0, pBuffer) == 0

    @staticmethod
    def eject():
        return CardManager.m_bInit and CRT286_Eject(CardManager.m_Comm) == 0

    @staticmethod
    def terminateCard():
        CardManager.eject()
        CommClose(CardManager.m_Comm)

    @staticmethod
    def IsConnected():
        b = ctypes.c_byte()
        return CRT286_GetStatus(CardManager.m_Comm, ctypes.byref(b)) == 0

# Example usage:
CardManager.Init()
