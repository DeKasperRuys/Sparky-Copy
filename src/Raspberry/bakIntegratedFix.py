#imports
import requests
import json
import RPi.GPIO as GPIO
import time
import random
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
#RFID
import sys
from mfrc522 import SimpleMFRC522
reader = SimpleMFRC522()
#setup servo
servoPIN1 = 7
servoPIN2 = 15
servoPIN3 = 11
servoPIN4 = 13
servoList = [servoPIN1,servoPIN2,servoPIN3,servoPIN4]
GPIO.setmode(GPIO.BOARD)
GPIO.setwarnings(False)
for y in range(4):
    GPIO.setup(servoList[y], GPIO.OUT)

#Leds
geelPIN = 32
groenPIN = 36
blauwPIN = 38
roodPIN = 35
rfidPIN = 37
pinList = [geelPIN,groenPIN,blauwPIN,roodPIN,rfidPIN]
for x in range(5):
    GPIO.setup(pinList[x], GPIO.OUT)
    GPIO.output(pinList[x],GPIO.LOW)
    
#bugfix led 4
GPIO.output(roodPIN,GPIO.HIGH)


#random slot waarde tussen 1 ,3, 4 , 2 werkt niet
randomlist = [1,3,4]
randomslot = random.choice(randomlist)


#testwaarde rfid availablepowerbank
# waarde tussen 1 en 4
aPowerbank = 1 #random.randint(1,4)
#variabelen
sleepTime = 0.25
stationId = 1
lockBool = None
# 140  of  100 = closed
angle = 140
angleEven = 100
setPB =""
boolPB = None
rfidText =""

#request header
headers = {
    'Content-Type': 'application/json'
}

def checkBoolean(lockBool):
    global angle
    global angleEven
    print("Angle of slot 1,3 is " + str(angle))
    print("Angle of slot 2,4 is " + str(angleEven))
    print(str(randomslot) + " = randomslotnummer")
    if randomslot == 1 or randomslot == 3:
        if lockBool == True and angle == 80:
            print("turn to 140 is closed, led is off")
            if  angle != 140:
                GPIO.output(pinList[randomslot-1],GPIO.HIGH)
                SetAngle(140)
                angle = 140
        elif lockBool == False and  angle == 140:
            print("turn to 80 is open, led is on")
            if  angle != 80:
                GPIO.output(pinList[randomslot-1],GPIO.LOW)
                SetAngle(80)
                angle = 80
    elif randomslot == 2 or randomslot == 4:
        if lockBool == False and angleEven == 100:
            print("turn to 150 is closed, led is off")
            if  angleEven != 150:
                GPIO.output(pinList[randomslot-1],GPIO.HIGH)
                SetAngle(150)
                angleEven = 150
        elif lockBool == True and  angleEven == 150:
            print("turn to 100 is open, led is on")
            if  angleEven != 100:
                GPIO.output(pinList[randomslot-1],GPIO.LOW)
                SetAngle(100)
                angleEven = 100
        

def SetAngle(angle):
    pwm = GPIO.PWM(servoList[randomslot-1],50)
    pwm.start(0)
    duty = angle / 18 + 2
    GPIO.output(servoList[randomslot-1], True)
    pwm.ChangeDutyCycle(duty)
    time.sleep(1)
    GPIO.output(servoList[randomslot-1], False)
    pwm.ChangeDutyCycle(0)

def putAvailablePowerbank(aPowerbank):
    baseurl = 'https://sparky1920api.azurewebsites.net/api/stations/availablepowerbank/'
    url = baseurl + str(stationId)
    print (url)
    # zoeken hoe ik available powerbank hier in krijg altijd een andere waarde
    myobj = {}
    myobj['id'] = aPowerbank
    x = requests.put(url, data=json.dumps(myobj),headers=headers,verify = False)#,verify=False
    print(x.text)
    time.sleep(sleepTime)
    
def putReturnOk():
    baseurl = 'https://sparky1920api.azurewebsites.net/api/stations/returnOk/'
    url = baseurl + str(stationId)
    print (url)
    # zoeken hoe ik available powerbank hier in krijg altijd een andere waarde
    myobj = {}
    myobj['setBoolean'] = True
    x = requests.put(url, data=json.dumps(myobj),headers=headers,verify = False)#,verify=False
    print(x.text)
    time.sleep(sleepTime)

def putReturnOkReset():
    baseurl = 'https://sparky1920api.azurewebsites.net/api/stations/returnOk/'
    url = baseurl + str(stationId)
    print (url)
    # zoeken hoe ik available powerbank hier in krijg altijd een andere waarde
    myobj = {}
    myobj['setBoolean'] = False
    x = requests.put(url, data=json.dumps(myobj),headers=headers,verify = False)#,verify=False
    print(x.text)
    time.sleep(sleepTime)
    
def pollLoanRequest(aPowerbank):
    response = requests.get('https://sparky1920api.azurewebsites.net/api/stations/poll/loanRequest/'+ str(stationId), headers=headers,verify = False)
    object_json = requests.get('https://sparky1920api.azurewebsites.net/api/stations/poll/loanRequest/' + str(stationId), headers=headers, verify = False).json()
    correct = "<Response [200]>"
    if str(response) == correct:
        if object_json == False:
            #pollen op returnrequest
            pollReturnRequest()
            time.sleep(sleepTime)
        else:
            putAvailablePowerbank(aPowerbank)
            print ("available powerbank is " + str(aPowerbank))
            pollOpenStation()
            time.sleep(sleepTime)
            
    else:
        print(str(response))
        time.sleep(sleepTime)


def pollOpenStation():
    response = requests.get('https://sparky1920api.azurewebsites.net/api/stations/poll/openstation/'+ str(stationId), headers=headers,verify = False )
    object_json = requests.get('https://sparky1920api.azurewebsites.net/api/stations/poll/openstation/' + str(stationId), headers=headers, verify = False).json()
    correct = "<Response [200]>"
    if str(response) == correct:
        if object_json == False:
            print ("Open station is False, lockBool set to True")
            lockBool = False
            checkBoolean(lockBool)
            time.sleep(sleepTime)
        else:
            print ("Open station is True, lockBool set to False")
            lockBool = True
            checkBoolean(lockBool)
            time.sleep(sleepTime)
            
    else:
        print(str(response))
        time.sleep(sleepTime)
        
def pollReturnRequest():
    global boolPB
    response = requests.get('https://sparky1920api.azurewebsites.net/api/stations/poll/isreturnrequest/'+ str(stationId), headers=headers,verify = False )
    object_json = requests.get('https://sparky1920api.azurewebsites.net/api/stations/poll/isreturnrequest/' + str(stationId), headers=headers, verify = False).json()
    correct = "<Response [200]>"
    if str(response) == correct:
        if object_json == False:
            print("Return request is false, lockBool set to True")
            lockBool = False
            checkBoolean(lockBool)
            print ("returnrequest is false, aPowerbank set to 0")
            aPowerbank = 0
            #tijdelijk
            putReturnOkReset()
            putAvailablePowerbank(aPowerbank)
            time.sleep(sleepTime)
        else:
            print ("Return request  is True, lockBool set to False")
            lockBool = True
            checkBoolean(lockBool)
            readRFID()
            #luisteren rfid
            if boolPB == True:
                print ("Put Return OK")
                putReturnOk()
                boolPB = False
                GPIO.output(37,GPIO.LOW)
            #returnok = true
            #client --> polled op returnok
            #returnok - > knop stoploan zichtbaar
            #pollen op closestation
            time.sleep(sleepTime)
            
    else:
        print(str(response))
        time.sleep(sleepTime)

def readRFID():
    global setPB
    global boolPB
    global rfidText
    checker = ""
    #alles resetten
    boolPB = False
    setPB = ""
    rfidText = ""
    if rfidText == checker:
        while True:
            print("Hold a tag near the reader")
            id, rfidText = reader.read()
            print("ID: %s\nText: %s" % (id,rfidText))
            GPIO.output(37,GPIO.HIGH)
            time.sleep(1)
            if rfidText != checker:
                setPB = rfidText
                if setPB != checker:
                    boolPB = True
                else:
                    boolPB = False
                break

    
while True:
    #pollLoanrequest = main loop
    pollLoanRequest(aPowerbank)
    #prit global sets
    print("global setpb set to " + setPB);
    print("global boolPB set to " + str(boolPB));
    
    #lockUnlockPoll();
    #putAvailablePowerbank(aPowerbank)
    #pollReturnRequest()
    #readRFID();
    
    #random slot waarde tussen 1 ,3, 4 , 2 werkt niet
    #randomlist = [1,3,4]
    #randomslot = random.choice(randomlist)
    #lockBool = False
    #checkBoolean(lockBool)
    #time.sleep(5)
    #lockBool = True
    #checkBoolean(lockBool)
    #time.sleep(5)



