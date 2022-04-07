#!/usr/bin/python3

import  RPi.GPIO as GPIO
from time import sleep
class IndicatorLicht:
    pinNummer = 0

    def __init__(self, pinNummer):
        self.pinNummer = pinNummer
        GPIO.setmode(GPIO.BCM)
        GPIO.setup(pinNummer,GPIO.OUT)

    def Blink(self): #blink 5 keer

        for x in range(6):
            GPIO.output(self.pinNummer,True)
            sleep(0.5)
            GPIO.output(self.pinNummer,False)
            sleep(0.5)




if __name__ == '__main__':

    test = IndicatorLicht(17)
    test.Blink()
    GPIO.cleanup()