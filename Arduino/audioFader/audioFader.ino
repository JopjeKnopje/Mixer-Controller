#define PIN_POT A0

#define LOG(x) Serial.println(x)
// todo make this a function
#define arrLength(x) sizeof(x) / sizeof(x[0])

#define FADER_STEP 2

// TODO Class for each potmeter with its last read value to compare the new one against
// TODO Add timer class
// DONE TODO to fix noise look at big change in value to "wakeup" the reading
// DONE TODO When fader has moved make the noise delta lower for a while


// URGENT!!
// TODO Fix random wrong fader detection
// for example; when moving fader 0 fader 1 gets also gets registerd as "moving" while the 
// read values aren't changing

// NOTE: its probably random noise?


/**
Serial packet
[Prefix],[id],[Val]

Prefix: either a button or fader
Id: the row its in 
Val: pressed or de-pressed or 0-100
**/

unsigned long zeroTime;
unsigned long nowTime;
unsigned long deltaTime;

int firstRead;

int faderDelta = 3;

// all the multi pot support stuff
int PINS[] = {A0, A1, A2, A3};


int firstReads[arrLength(PINS)];
int potValues[arrLength(PINS)];
int currentReads[arrLength(PINS)];
int deltas[arrLength(PINS)];

void setup()
{

    Serial.begin(9600);
    // pinMode(PIN_POT, INPUT);

    // setup all the analog pins
    // for (int i = 0; i < sizeof(PINS) / sizeof(PINS[0]); i++)
    for (int i = 0; i < arrLength(PINS); i++)
    {
        pinMode(PINS[i], INPUT);
        Serial.println("pin " + String(PINS[i]) + " set!");

    }


    // take first reading of fader's location
    for (int i = 0; i < arrLength(firstReads); i++)
    {
        firstReads[i] = convertPot(analogRead(PINS[i]));
        Serial.println("took a first reading of pin " + String(PINS[i]));
    }

    // int firstRead = convertPot(analogRead(PIN_POT));

    // zero the timer
    zeroTime = millis();
}

void loop() 
{

    for (int i = 0; i < arrLength(potValues); i++)
    {
        potValues[i] = convertPot(analogRead(PINS[i]));
        // Serial.println("took a another reading of pin " + String(PINS[i]));

    }

    // int potValue = convertPot(analogRead(PIN_POT));

    // take time measurement
    nowTime = millis();
    deltaTime = nowTime - zeroTime;
    // check time since last running the following code


    // TODO Make the 50 go down when moving the fader 
    // or not? 
    //doesn't really make difference?
    if(deltaTime >= 50)
    {
        // reset timer
        zeroTime = millis();

        // LOG("DID THE TIMER");

        // loop through all the pins and check if they have moved
        for (int i = 0; i < arrLength(PINS); i++)
        {
            // if(hasMoved(PINS[i]))
            if(hasMoved(i))
            {
                // String out = String(incrementStep(potValue)) + " " + String(potValue);
                // LOG(out);
                // sendSerial("fader", 0, incrementStep(potValue));
                sendSerial("fader", i, incrementStep(potValues[i]));
            }
        }
    }
}

bool hasMoved(int pinIndex)
{
    // int currentReads[pinIndex] = convertPot(analogRead(PINS[pinIndex]));
    currentReads[pinIndex] = convertPot(analogRead(PINS[pinIndex]));
    
    // since the firstRead - currentRead can be negative take the positive squareroot
    
    // int deltas[pinIndex] = pow(firstReads[pinIndex] - currentReads[pinIndex], 2);
    deltas[pinIndex] = pow(firstReads[pinIndex] - currentReads[pinIndex], 2);
    deltas[pinIndex] = sqrt(deltas[pinIndex]);
    // LOG(delta);

    // the amount the fader has to move before its registered as "MOVED"
    if (deltas[pinIndex] >= faderDelta)
    {
        // if the fader is moving set faderDelta to 1 so its more sensitive
        faderDelta = 1;

        firstReads[pinIndex] = convertPot(analogRead(PINS[pinIndex]));
        return true;
    }
    // if the fader is not moving set faderDelta back to 3 to prevent "random moving" aka noise
    faderDelta = 3;
    return false;
}



void sendSerial(String prefix, int id, int value)
{
    String out = prefix + "," + String(id) + "," + String(value);
    Serial.println(out);
}


// checks if the change in value is large enough to tell the pot has moved
// bool hasMoved(int pin)
// {
//     int currentRead = convertPot(analogRead(pin));
//     // since the firstRead - currentRead can be negative take the positive squareroot
//     int delta = pow(firstRead - currentRead, 2);
//     delta = sqrt(delta);
//     // LOG(delta);

//     // the amount the fader has to move before its registered as "MOVED"
//     if (delta >= faderDelta)
//     {
//         // if the fader is moving set faderDelta to 1 so its more sensitive
//         faderDelta = 1;

//         firstRead = convertPot(analogRead(pin));
//         return true;
//     }
//     // if the fader is not moving set faderDelta back to 3 to prevent "random moving" aka noise
//     faderDelta = 3;
//     return false;
// }

// super vies 
int incrementStep(int input)
{
    // make it go to the limit of the meter
    if (input >= (100 - FADER_STEP)) return 100;
    if (input <= 0 + FADER_STEP) return 0;
    return input - input % FADER_STEP;

}

int convertPot(int input)
{
    return int((10 * pow(input / 1023.,10)) * 10);
}

// todo make this function work
// int getSize(int *array)
// {
//     // return sizeof(array) / sizeof(array[0]);
//     return (sizeof(array)/sizeof(*array));
// }
