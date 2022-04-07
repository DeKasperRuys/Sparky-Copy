#imports
import requests
import json
import RPi.GPIO as GPIO
import time
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

#setup servo
servoPIN = 17
GPIO.setmode(GPIO.BCM)
GPIO.setup(servoPIN, GPIO.OUT)
pwm = GPIO.PWM(servoPIN,50)
pwm.start(0)

#variabelen
sleepTime = 5
stationId = 1
lockBool = False
#request header
headers = {
    'Content-Type': 'application/json'
}
def checkBoolean(lockBool):
    if lockBool == True:
        print("turn to 90")
        SetAngle(90)
    else:
        print("turn to 0")
        SetAngle(0)

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
            print ("unlock")
            lockBool = True
            checkBoolean(lockBool)
            time.sleep(sleepTime)
        else:
            print ("lock")
            lockBool = False
            checkBoolean(lockBool)
            time.sleep(sleepTime)
    else:
        print(str(response))
        time.sleep(sleepTime)
     
while True:
    lockUnlockPoll();

