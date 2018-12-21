
#include <ESP8266WiFi.h>
#include <WiFiUdp.h>
#define FASTLED_ESP8266_RAW_PIN_ORDER 
#include "FastLED.h"
#include <ArduinoJson.h>

// What is the objects name?
const char* objectname = "plant";

// How many leds in your strip?
#define NUM_LEDS 7

const char* ssid     = "YOUR-SSID";
const char* password = "YOUR-PASSWORD";

// For led chips like Neopixels, which have a data line, ground, and power, you just
// need to define DATA_PIN.  For led chipsets that are SPI based (four wires - data, clock,
// ground, and power), like the LPD8806 define both DATA_PIN and CLOCK_PIN
#define DATA_PIN 13 
#define CLOCK_PIN 14

// Define the array of leds
CRGB leds[NUM_LEDS];

WiFiUDP Udp;
unsigned int localUdpPort = 12000;  // local port to listen on
char incomingPacket[255];  // buffer for incoming packets

int colors[31] = {0x00FF00, 0x11FF00, 0x22FF00, 0x33FF00, 0x44FF00, 0x55FF00, 0x66FF00, 0x77FF00, 0x88FF00, 0x99FF00, 0xAAFF00, 0xBBFF00, 0xCCFF00, 0xDDFF00, 0xEEFF00, 0xFFFF00, 0xFFEE00, 0xFFDD00, 0xFFCC00, 0xFFBB00, 0xFFAA00, 0xFF9900, 0xFF8800, 0xFF7700, 0xFF6600, 0xFF5500, 0xFF4400, 0xFF3300, 0xFF2200, 0xFF1100, 0xFF0000};

int lastColor = -1;
void setup() {
  
  if (objectname == "plant"){
    FastLED.addLeds<APA102, DATA_PIN, CLOCK_PIN, BGR>(leds, NUM_LEDS);
  } else {
    FastLED.addLeds<WS2812B, DATA_PIN, GRB>(leds, NUM_LEDS);
  }
  leds[0] = CRGB(255, 0, 0);
  FastLED.show();
  delay(500);
  
  Serial.begin(115200);
  Serial.println();
  Serial.printf("Connecting to %s ", ssid);
  WiFi.begin(ssid, password, true);
  WiFi.setAutoConnect ( true );
  WiFi.setAutoReconnect ( false ); 
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
}

void loop() { 
  if (WiFi.status() != WL_CONNECTED) {
    Serial.println("restart");
    leds[0] = CRGB(255, 0, 0);
    for (int i=1; i < NUM_LEDS; i++){
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

    String objectnameSender = root["name"];
    if (objectnameSender.equals(objectname)){      
      // Parameters
      int value = root["value"]; 
      CRGB color;
      if (value == -1){
        color = CRGB(0, 0, 0);        
      } else {
        int colorid = (float)value/100.0*31.0;
        if (lastColor == colorid) {
          return;
        } else {
          lastColor = colorid;
        }
        Serial.printf("Color Id: %i\n", colorid);
        color = colors[colorid];
      }
      for (int i=0; i <= NUM_LEDS; i++){
        leds[i] = color;
      }
      FastLED.show();
    }
  }
}
