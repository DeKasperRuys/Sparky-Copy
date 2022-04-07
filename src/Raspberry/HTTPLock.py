#imports
import requests
import json
import time
import RPi.GPIO as GPIO
servoPIN = 17
GPIO.setmode(GPIO.BCM)
GPIO.setup(servoPIN, GPIO.OUT)

#variabelen
SECONDS = 60
minutes = 1
sleepTime = minutes*SECONDS
boolean = 0
p = GPIO.PWM(servoPIN, 50) # GPIO 17 for PWM with 50Hz
p.start(2.5) # Initialization

#functie dat elke ingestelde tijd een json object binnenhaalt
def objdata():
    response = requests.get('http://validate.jsontest.com/?json=%7B%22key%22:%22value%22%7D')
    object_json = requests.get('http://validate.jsontest.com/?json=%7B%22key%22:%22value%22%7D').json()
    correct = "<Response [200]>"
    print(str(response))
    if str(response) == correct:
        print(object_json)
        print(object_json['validate'])
        print(object_json['empty'])
        boolean = object_json['validate']
        if (boolean == 1):
            try:
                    p.ChangeDutyCycle(5)
                    time.sleep(0.5)
                    p.ChangeDutyCycle(7.5)
                    time.sleep(0.5)
                    p.ChangeDutyCycle(10)
            except KeyboardInterrupt:
                p.stop()
                GPIO.cleanup()
    else:
        print(str(response))
    time.sleep(sleepTime)

#while True:
objdata();