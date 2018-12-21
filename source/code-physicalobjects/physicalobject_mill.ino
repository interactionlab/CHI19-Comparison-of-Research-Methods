
#include <ESP8266WiFi.h>
#include <WiFiUdp.h>
//#define FASTLED_ESP8266_RAW_PIN_ORDER 
#define FASTLED_ALLOW_INTERRUPTS 0
#include "FastLED.h"
#include <ArduinoJson.h>

// What is the objects name?
const String objectname1 = "peppermill";
const String objectname2 = "saltmill";

// How many leds in your strip?
#define NUM_LEDS 8

// For led chips like Neopixels, which have a data line, ground, and power, you just
// need to define DATA_PIN.  For led chipsets that are SPI based (four wires - data, clock,
// ground, and power), like the LPD8806 define both DATA_PIN and CLOCK_PIN
#define DATA_PIN 0
#define CLOCK_PIN 14

const char* ssid     = "YOUR-SSID";
const char* password = "YOUR-PASSWORD";

// Define the array of leds
CRGB leds[NUM_LEDS];

WiFiUDP Udp;
const unsigned int localUdpPort = 12000;  // local port to listen on
char incomingPacket[255];  // buffer for incoming packets

const int colors[31] = {0x00FF00, 0x11FF00, 0x22FF00, 0x33FF00, 0x44FF00, 0x55FF00, 0x66FF00, 0x77FF00, 0x88FF00, 0x99FF00, 0xAAFF00, 0xBBFF00, 0xCCFF00, 0xDDFF00, 0xEEFF00, 0xFFFF00, 0xFFEE00, 0xFFDD00, 0xFFCC00, 0xFFBB00, 0xFFAA00, 0xFF9900, 0xFF8800, 0xFF7700, 0xFF6600, 0xFF5500, 0xFF4400, 0xFF3300, 0xFF2200, 0xFF1100, 0xFF0000};

int colorid1 = 15;
int lastColor1 = 15;
int colorid2 = 15;
int lastColor2 = 15;

CRGB color1 = colors[lastColor1];
CRGB color2 = colors[lastColor2];

bool newColor = false;
void setup() {  
  FastLED.addLeds<WS2812B, DATA_PIN, GRB>(leds, NUM_LEDS);
  for (int i=0; i < NUM_LEDS; i++){
    leds[i] = CRGB(0, 0, 0);
  }
  FastLED.show();
  delay(500);
  
  Serial.begin(115200);
  Serial.println();
  Serial.printf("Connecting to %s ", ssid);
  WiFi.begin(ssid, password, true);
  WiFi.setAutoReconnect (true);
  leds[0] = CRGB(0, 0, 255);
  FastLED.show();
  int i = 0;
  while (WiFi.status() != WL_CONNECTED)
  {
    if (i > 20)
      ESP.restart();
    delay(500);
    Serial.print(".");
    i++;
  }
  Serial.println(" connected");
  Udp.begin(localUdpPort);
  Serial.printf("Now listening at IP %s, UDP port %d\n", WiFi.localIP().toString().c_str(), localUdpPort);
  leds[0] = CRGB(0, 255, 0);
  FastLED.show();
  delay(500);
}

void loop() { 
    if (WiFi.status() != WL_CONNECTED) {
    Serial.println("restart");
    leds[0] = CRGB(255, 0, 0);
    for (int i=1; i <= NUM_LEDS; i++){
        leds[i] = CRGB(0, 0, 0);;
      }
    FastLED.show();
    delay(1000);
    ESP.restart();
  }
  int packetSize = Udp.parsePacket();
  if (packetSize)
  {
    // receive incoming UDP packets
    //Serial.printf("Received %d bytes from %s, port %d\n", packetSize, Udp.remoteIP().toString().c_str(), Udp.remotePort());
    int len = Udp.read(incomingPacket, 255);
    if (len > 0)
    {
      incomingPacket[len] = 0;
    }
    Serial.printf("UDP packet contents: %s\n", incomingPacket);
    
    DynamicJsonBuffer jsonBuffer;
    JsonObject& root = jsonBuffer.parseObject(incomingPacket);
    int value = root["value"]; 
    String objectnameSender = root["name"];
    newColor = false;
    if (objectnameSender.equals(objectname1)){ 
      
      if (value == -1){
        color1 = CRGB(0, 0, 0);
        newColor = true;
      } else {
        colorid1 = int((((float)value)/100.0) * ((float)(sizeof(colors)/sizeof(int))));
        if (lastColor1 != colorid1) {
          Serial.print("Color " + objectname1 + " Id: " + colorid1 + "\n");
          lastColor1 = colorid1;          
          color1 = colors[colorid1];
          newColor = true;
        }
      }
      FastLED.show();    
    } else if (objectnameSender.equals(objectname2)){
      CRGB color;
      if (value == -1){
        color2 = CRGB(0, 0, 0);
        newColor = true;
      } else {
        colorid2 = int((((float)value)/100.0) * ((float)(sizeof(colors)/sizeof(int))));
        if (lastColor2 != colorid2) {
          
          Serial.print("Color " + objectname2 + " Id: " + colorid2 + "\n");
          lastColor2 = colorid2;
          color2 = colors[colorid2];
          newColor = true;
        }
      }
       
    }

    if(newColor == true) {
      for (int i = 0; i < 4; i++){
          leds[i] = color1;
      }
      for (int i = 4; i < NUM_LEDS; i++){
          leds[i] = color2;
      }
      FastLED.show();
    }
  }
}
