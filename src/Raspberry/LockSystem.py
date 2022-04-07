
import RPi.GPIO as GPIO
import time
GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)
GPIO.setup(5,GPIO.OUT) #LED pinnen kunnen nog aangepast worden
GPIO.setup(6,GPIO.OUT)
GPIO.setup(13,GPIO.OUT)
GPIO.setup(19,GPIO.OUT)
GPIO.setup(17, GPIO.OUT)
GPIO.setup(18, GPIO.OUT)
GPIO.setup(27, GPIO.OUT)
GPIO.setup(22, GPIO.OUT)


    p = GPIO.PWM(servoPIN, 50); # GPIO 17 for PWM with 50Hz
    p1 = GPIO.PWM(servoPIN, 50);
    p2 = GPIO.PWM(servoPIN, 50);
    p3 = GPIO.PWM(servoPIN, 50);
    p.start(2.5);
    p1.start(2.5);
    p2.start(2.5);
    p3.start(2.5)
    int switcher = 0;
    while true: 
        switch(switcher){
            case 0: p.ChangeDutyCycle(5);
                    time.sleep(0.5);
                    p.ChangeDutyCycle(7.5);
                    switcher++;
                    GPIO.output(18,GPIO.HIGH);
                    break;
            case 1: p1.ChangeDutyCycle(5);
                    time.sleep(0.5);
                    p1.ChangeDutyCycle(7.5);
                    #GPIO.output(18,GPIO.HIGH);
                    switcher++;
                    break;
            case 2: p2.ChangeDutyCycle(5);
                    time.sleep(0.5);
                    p2.ChangeDutyCycle(7.5);
                    #GPIO.output(18,GPIO.HIGH);
                    switcher++;
                    break;
            case 3: p3.ChangeDutyCycle(5);
                    time.sleep(0.5);
                    p3.ChangeDutyCycle(7.5);
                    #GPIO.output(18,GPIO.HIGH);
                    switcher = 0;
                    break;

        }
