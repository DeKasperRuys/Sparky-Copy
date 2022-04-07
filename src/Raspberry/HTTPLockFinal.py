#imports
import requests
import json
import RPi.GPIO as GPIO
import time
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

#setup servo
servoPIN = 17
ledPIN = 18
GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)
GPIO.setup(servoPIN, GPIO.OUT)
GPIO.setup(ledPIN, GPIO.OUT)
pwm = GPIO.PWM(servoPIN,50)
pwm.start(0)

#variabelen
sleepTime = 5
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
    if lockBool == True and angle == 0:
        print("turn to 90 is closed, led is of")
        if  angle != 90:
            GPIO.output(18,GPIO.LOW)
            SetAngle(90)
            angle = 90
    elif lockBool == False and  angle == 90:
        print("turn to 0 is open, led is on")
        if  angle != 0:
            GPIO.output(18,GPIO.HIGH)
            SetAngle(0)
            angle = 0

def SetAngle(angle):
    duty = angle / 18 + 2
    GPIO.output(servoPIN, True)
    pwm.ChangeDutyCycle(duty)
    time.sleep(1)
    GPIO.output(servoPIN, False)
    pwm.ChangeDutyCycle(0)

def lockUnlockPoll():
    response = requests.get('https://sparky1920api.azurewebsites.net/api/stations/poll/'+ str(stationId), headers=headers, )
    object_json = requests.get('https://sparky1920api.azurewebsites.net/api/stations/poll/' + str(stationId), headers=headers).json()
    correct = "<Response [200]>"
    if str(response) == correct:
        if object_json == False:
            print ("swagger value is False, lockBool set to True")
            lockBool = True
            checkBoolean(lockBool)
            time.sleep(sleepTime)
        else:
            print ("swagger value is True, lockBool set to False")
            lockBool = False
            checkBoolean(lockBool)
            time.sleep(sleepTime)
    else:
        print(str(response))
        time.sleep(sleepTime)
     
while True:
    lockUnlockPoll();

