#imports
import requests
import json
import RPi.GPIO as GPIO
import time
import random
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

#setup servo
servoPIN = 17
GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)
GPIO.setup(servoPIN, GPIO.OUT)
pwm = GPIO.PWM(servoPIN,50)
pwm.start(0)

#Leds
geelPIN = 12
groenPIN = 16
blauwPIN = 20
roodPIN = 21
pinList = [geelPIN,groenPIN,blauwPIN,roodPIN]
for x in range(4):
    GPIO.setup(pinList[x], GPIO.OUT)

GPIO.output(12,GPIO.LOW)
GPIO.output(16,GPIO.LOW)
GPIO.output(20,GPIO.LOW)
GPIO.output(21,GPIO.LOW)
#random slot waarde tussen 1 en 4
randomslot = random.randint(1,4)

#testwaarde rfid availablepowerbank
# waarde tussen 1 en 4
aPowerbank = 1 #random.randint(1,4)
#variabelen
sleepTime = 0.25
stationId = 1
lockBool = None
# 90 = closed
angle = 90

#request header
headers = {
    'Content-Type': 'application/json'
}

def checkBoolean(lockBool):
    global angle
    print("Angle is " + str(angle))
    print(str(randomslot) + " = randomslotnummer")
    if lockBool == True and angle == 0:
        print("turn to 90 is closed, led is of")
        if  angle != 90:
            GPIO.output(pinList[randomslot-1],GPIO.LOW)
            SetAngle(90)
            angle = 90
    elif lockBool == False and  angle == 90:
        print("turn to 0 is open, led is on")
        if  angle != 0:
            GPIO.output(pinList[randomslot-1],GPIO.HIGH)
            SetAngle(0)
            angle = 0

def SetAngle(angle):
    duty = angle / 18 + 2
    GPIO.output(servoPIN, True)
    pwm.ChangeDutyCycle(duty)
    time.sleep(1)
    GPIO.output(servoPIN, False)
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
            lockBool = True
            checkBoolean(lockBool)
            time.sleep(sleepTime)
        else:
            print ("Open station is True, lockBool set to False")
            lockBool = False
            checkBoolean(lockBool)
            time.sleep(sleepTime)
            
    else:
        print(str(response))
        time.sleep(sleepTime)
        
def pollReturnRequest():
    response = requests.get('https://sparky1920api.azurewebsites.net/api/stations/poll/isreturnrequest/'+ str(stationId), headers=headers,verify = False )
    object_json = requests.get('https://sparky1920api.azurewebsites.net/api/stations/poll/isreturnrequest/' + str(stationId), headers=headers, verify = False).json()
    correct = "<Response [200]>"
    if str(response) == correct:
        if object_json == False:
            print("Return request is false, lockBool set to True")
            lockBool = True
            checkBoolean(lockBool)
            print ("returnrequest is false, aPowerbank set to 0")
            aPowerbank = 0
            putAvailablePowerbank(aPowerbank)
            time.sleep(sleepTime)
        else:
            print ("Return request  is True, lockBool set to False")
            lockBool = False
            checkBoolean(lockBool)
            #luisteren rfid
            rfid = True
            if rfid == True:
                print ("Put Return OK")
                putReturnOk()
            #returnok = true
            #client --> polled op returnok
            #returnok - > knop stoploan zichtbaar
            #pollen op closestation
            time.sleep(sleepTime)
            
    else:
        print(str(response))
        time.sleep(sleepTime)

while True:
    #pollReturnRequest()
    pollLoanRequest(aPowerbank)
    #lockUnlockPoll();
    #putAvailablePowerbank(aPowerbank)
    



